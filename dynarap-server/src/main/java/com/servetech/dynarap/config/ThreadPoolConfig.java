package com.servetech.dynarap.config;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.scheduling.annotation.EnableAsync;
import org.springframework.scheduling.concurrent.ThreadPoolTaskExecutor;

import java.util.concurrent.Executor;

@Configuration
@EnableAsync
public class ThreadPoolConfig {

    @Bean(name="texecutor")
    public Executor setThreadExecutor() {
        ThreadPoolTaskExecutor tpte = new ThreadPoolTaskExecutor();
        tpte.setCorePoolSize(16);
        tpte.setMaxPoolSize(256);
        tpte.setQueueCapacity((int) (256 * 1.5));
        tpte.setThreadNamePrefix("tpte1-");
        return tpte;
    }

    @Bean(name="tsingle")
    public Executor setSingleThreadExecutor() {
        ThreadPoolTaskExecutor tpte = new ThreadPoolTaskExecutor();
        tpte.setCorePoolSize(1);
        tpte.setMaxPoolSize(1);
        tpte.setQueueCapacity(10);
        tpte.setThreadNamePrefix("tsingle-");
        return tpte;
    }

}
