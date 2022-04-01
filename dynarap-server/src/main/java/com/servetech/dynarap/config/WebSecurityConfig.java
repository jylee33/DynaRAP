package com.servetech.dynarap.config;

import com.servetech.dynarap.security.JwtAuthorizationFilter;
import com.servetech.dynarap.security.UserAuthService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.config.annotation.authentication.builders.AuthenticationManagerBuilder;
import org.springframework.security.config.annotation.method.configuration.EnableGlobalMethodSecurity;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.builders.WebSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.config.annotation.web.configuration.WebSecurityConfigurerAdapter;
import org.springframework.security.config.http.SessionCreationPolicy;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.security.web.authentication.UsernamePasswordAuthenticationFilter;

@Configuration
@EnableWebSecurity
@EnableGlobalMethodSecurity(prePostEnabled = true)
public class WebSecurityConfig extends WebSecurityConfigurerAdapter {
    @Autowired
    private UserAuthService userAuthService;

    @Bean
    @Override
    protected AuthenticationManager authenticationManager() throws Exception {
        return super.authenticationManager();
    }

    @Override
    public void configure(WebSecurity web) {
        web.ignoring().antMatchers("/resources/**");
    }

    @Bean
    public static PasswordEncoder passwordEncoder() {
        return ServerConstants.PASSWORD_ENCODER;
    }

    @Override
    protected void configure(AuthenticationManagerBuilder auth) throws Exception {
        auth.userDetailsService(userAuthService).passwordEncoder(passwordEncoder());
    }

    @Override
    protected void configure(HttpSecurity http) throws Exception {
        http.csrf().disable().sessionManagement()
                .sessionCreationPolicy(SessionCreationPolicy.STATELESS)
                .and()
                .authorizeRequests()
                .antMatchers("/**").permitAll()
                .antMatchers("/page/**").permitAll()
                .antMatchers("/open-api/**").permitAll()
                .antMatchers("/sendmail/**").permitAll()
                .antMatchers("/api/**").permitAll()
                .anyRequest().authenticated()
                .and().addFilterBefore(new JwtAuthorizationFilter(authenticationManager()), UsernamePasswordAuthenticationFilter.class)
                .exceptionHandling()
                .and().formLogin().disable()
                .httpBasic();
        http.headers().frameOptions().disable();
    }
}
