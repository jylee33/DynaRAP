package com.servetech.dynarap;

import com.servetech.dynarap.config.ServerConstants;
import com.servetech.dynarap.controller.WebSocketManager;
import com.servetech.dynarap.db.service.UserService;
import com.servetech.dynarap.ext.HandledServiceException;
import com.servetech.dynarap.ext.LoggingRequestInterceptor;
import com.servetech.dynarap.vo.ResponseVO;
import com.servetech.dynarap.db.type.*;
import lombok.SneakyThrows;
import net.javacrumbs.shedlock.core.LockProvider;
import net.javacrumbs.shedlock.provider.jdbctemplate.JdbcTemplateLockProvider;
import org.apache.http.conn.ssl.NoopHostnameVerifier;
import org.apache.http.impl.client.CloseableHttpClient;
import org.apache.http.impl.client.HttpClients;
import org.apache.ibatis.session.SqlSessionFactory;
import org.apache.ibatis.type.TypeHandler;
import org.mybatis.spring.SqlSessionFactoryBean;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.cache.annotation.EnableCaching;
import org.springframework.context.ConfigurableApplicationContext;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.PropertySource;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.client.BufferingClientHttpRequestFactory;
import org.springframework.http.client.ClientHttpRequestInterceptor;
import org.springframework.http.client.ClientHttpResponse;
import org.springframework.http.client.HttpComponentsClientHttpRequestFactory;
import org.springframework.http.converter.FormHttpMessageConverter;
import org.springframework.http.converter.json.GsonHttpMessageConverter;
import org.springframework.http.converter.json.MappingJackson2HttpMessageConverter;
import org.springframework.jdbc.datasource.DataSourceTransactionManager;
import org.springframework.scheduling.annotation.EnableAsync;
import org.springframework.scheduling.annotation.EnableScheduling;
import org.springframework.scheduling.concurrent.ThreadPoolTaskExecutor;
import org.springframework.stereotype.Component;
import org.springframework.web.client.ResponseErrorHandler;
import org.springframework.web.client.RestTemplate;
import org.springframework.web.socket.server.standard.ServerEndpointExporter;

import javax.sql.DataSource;
import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.util.*;
import java.util.concurrent.Executor;

import static org.springframework.http.HttpStatus.Series.CLIENT_ERROR;
import static org.springframework.http.HttpStatus.Series.SERVER_ERROR;

@EnableAsync
@EnableCaching
@Component("application")
@PropertySource("classpath:application.properties")
@SpringBootApplication
@EnableScheduling
public class DynaRAPServerApplication {
    private static final Logger logger = LoggerFactory.getLogger(DynaRAPServerApplication.class);

    public static ConfigurableApplicationContext global;
    public static WebSocketManager webSocketManager = null;

    public static void main(String[] args) {
        global = SpringApplication.run(DynaRAPServerApplication.class, args);

        webSocketManager = new WebSocketManager(
                global.getBean(UserService.class)
        );
    }

    @Bean
    public ServerEndpointExporter serverEndpointExporter() {
        return new ServerEndpointExporter();
    }

    @Bean
    public LockProvider lockProvider(DataSource dataSource) {
        return new JdbcTemplateLockProvider(dataSource);
    }

    @Bean
    public RestTemplate restTemplate() {
        CloseableHttpClient httpClient
                = HttpClients.custom()
                .setSSLHostnameVerifier(new NoopHostnameVerifier())
                .build();
        HttpComponentsClientHttpRequestFactory requestFactory
                = new HttpComponentsClientHttpRequestFactory();
        requestFactory.setHttpClient(httpClient);

        RestTemplate restTemplate = new RestTemplate(new BufferingClientHttpRequestFactory(
                requestFactory));
        restTemplate.setErrorHandler(new ResponseErrorHandler() {
            @Override
            public boolean hasError(ClientHttpResponse response) throws IOException {
                return (response.getStatusCode().series() == CLIENT_ERROR
                        || response.getStatusCode().series() == SERVER_ERROR);
            }

            @SneakyThrows
            @Override
            public void handleError(ClientHttpResponse response) throws IOException {
                if (response.getStatusCode().series() == HttpStatus.Series.SERVER_ERROR) {
                    // handle SERVER_ERROR
                } else if (response.getStatusCode().series() == HttpStatus.Series.CLIENT_ERROR) {
                    String responseText = null;
                    try (Scanner scanner = new Scanner(response.getBody(), StandardCharsets.UTF_8.name())) {
                        responseText = scanner.useDelimiter("\\A").next();
                    }

                    ResponseVO respError = ServerConstants.GSON.fromJson(responseText, ResponseVO.class);
                    String respErrorMessage = "";
                    if (respError.getMessage() instanceof String)
                        respErrorMessage = (String) respError.getMessage();
                    else if (respError.getMessage() instanceof String64)
                        respErrorMessage = ((String64) respError.getMessage()).originOf();

                    throw new HandledServiceException(respError.getCode(), respErrorMessage);
                }
            }
        });

        MappingJackson2HttpMessageConverter converter = new MappingJackson2HttpMessageConverter();
        converter.setSupportedMediaTypes(Collections.singletonList(MediaType.TEXT_HTML));

        FormHttpMessageConverter formConverter = new FormHttpMessageConverter();
        formConverter.setSupportedMediaTypes(Collections.singletonList(MediaType.APPLICATION_FORM_URLENCODED));

        List<ClientHttpRequestInterceptor> interceptors = new ArrayList<ClientHttpRequestInterceptor>();
        interceptors.add(new LoggingRequestInterceptor());
        restTemplate.setInterceptors(interceptors);
        restTemplate.setMessageConverters(Arrays.asList(gsonHttpMessageConverter(), converter, formConverter));
        return restTemplate;
    }

    @Bean
    public GsonHttpMessageConverter gsonHttpMessageConverter() {
        GsonHttpMessageConverter converter = new GsonHttpMessageConverter();
        converter.setSupportedMediaTypes(Collections.singletonList(MediaType.APPLICATION_JSON));
        converter.setGson(ServerConstants.GSON);
        return converter;
    }

    @Bean
    public SqlSessionFactory sqlSessionFactory(DataSource dataSource) throws Exception {
        SqlSessionFactoryBean sqlSessionFactoryBean = new SqlSessionFactoryBean();
        sqlSessionFactoryBean.setDataSource(dataSource);

        sqlSessionFactoryBean.setTypeHandlers(
                new TypeHandler[] {new LongDateTypeHandler(), new String64TypeHandler(),
                        new CryptoFieldTypeHandler(), new CryptoFieldNAuthTypeHandler(), new UserTypeTypeHandler()});

        sqlSessionFactoryBean
                .setTypeAliases(new Class[] {LongDateTypeHandler.class, String64TypeHandler.class,
                        CryptoFieldTypeHandler.class, CryptoFieldNAuthTypeHandler.class, UserTypeTypeHandler.class});

        return sqlSessionFactoryBean.getObject();
    }

    @Bean
    public DataSourceTransactionManager transactionManager(DataSource dataSource) {
        return new DataSourceTransactionManager(dataSource);
    }

    @Bean
    public Executor threadPoolTaskExecutor() {
        ThreadPoolTaskExecutor executor = new ThreadPoolTaskExecutor();
        executor.setCorePoolSize(16);
        executor.setQueueCapacity(100);
        executor.setMaxPoolSize(64);
        executor.setThreadNamePrefix("tdynarap-");
        return executor;
    }

}
