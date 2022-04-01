package com.servetech.dynarap.vo;

import lombok.Data;

@Data
public class AuthRefreshRequestVO {
    private String clientId;
    private String clientSecret;
    private String refreshToken;
}

