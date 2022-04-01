package com.servetech.dynarap.security;

import org.springframework.security.core.AuthenticationException;

public class JwtAuthenticationException extends AuthenticationException {
    public JwtAuthenticationException(final String message) {
        super(message);
    }

    public JwtAuthenticationException(final String message, final Throwable throwable) {
        super(message, throwable);
    }
}
