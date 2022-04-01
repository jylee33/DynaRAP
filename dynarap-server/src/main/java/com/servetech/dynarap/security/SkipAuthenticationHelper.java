package com.servetech.dynarap.security;

import org.springframework.security.web.util.matcher.AntPathRequestMatcher;

import javax.servlet.RequestDispatcher;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class SkipAuthenticationHelper {
    /* 인증을 하지 않는 경우에 여기에 패스를 추가함 */
    public static final AntPathRequestMatcher[] SKIP_URL = new AntPathRequestMatcher[] {};

    private static final List<AntPathRequestMatcher> antMatchers = new ArrayList<>(Arrays.asList(SKIP_URL));

    public static boolean isSkipAuthentication(HttpServletRequest request) {
        for (AntPathRequestMatcher matcher : antMatchers) {
            if (matcher.matches(request) == true)
                return true;
        }
        return false;
    }

    public static void forwardRequest(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        RequestDispatcher rd = request
                .getRequestDispatcher(request.getRequestURI() + "?" + request.getQueryString());
        rd.forward(request, response);
    }
}
