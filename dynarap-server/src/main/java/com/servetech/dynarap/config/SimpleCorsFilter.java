package com.servetech.dynarap.config;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.core.Ordered;
import org.springframework.core.annotation.Order;
import org.springframework.http.HttpHeaders;
import org.springframework.stereotype.Component;

import javax.servlet.*;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;
import java.util.Collection;

@Component
@Order(Ordered.HIGHEST_PRECEDENCE)
public class SimpleCorsFilter implements Filter {
    private static final Logger logger = LoggerFactory.getLogger(SimpleCorsFilter.class);

    @Override
    public void init(FilterConfig filterConfig) throws ServletException {

    }

    @Override
    public void doFilter(ServletRequest req, ServletResponse res, FilterChain chain)
            throws IOException, ServletException {
        HttpServletResponse response = (HttpServletResponse) res;
        response.setHeader("Access-Control-Allow-Origin", "*");
        response.setHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS, HEAD");
        response.setHeader("Access-Control-Max-Age", "3600");
        response.setHeader("Access-Control-Allow-Headers",
                "Origin, X-Requested-With, Content-Type, Accept, X-Auth-Token, Authorization");
        response.setHeader("Access-Control-Allow-Credentials", "true");
        //response.setHeader("X-Frame-Options", "SAMEORIGIN");
        chain.doFilter(req, res);

        addSameSite(response, "None");
    }

    @Override
    public void destroy() {

    }

    private void addSameSite(HttpServletResponse response, String sameSite) {
        Collection<String> headers = response.getHeaders(HttpHeaders.SET_COOKIE);
        boolean firstHeader = true;
        for (String header : headers) {
            if (firstHeader) {
                response.setHeader(HttpHeaders.SET_COOKIE,
                        String.format("%s; Secure; %s", header, "SameSite=" + sameSite));
                firstHeader = false;
                continue;
            }
            response.addHeader(HttpHeaders.SET_COOKIE,
                    String.format("%s; Secure; $s", header, "SameSite=" + sameSite));
        }
    }
}
