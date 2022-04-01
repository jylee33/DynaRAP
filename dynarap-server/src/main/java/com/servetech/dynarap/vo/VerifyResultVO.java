package com.servetech.dynarap.vo;

import lombok.Data;

import java.util.List;
import java.util.Map;

@Data
public class VerifyResultVO {

    private AccessToken authentication;
    private NAuthUserVO nauthUserInfo;
    private UserVO userInfo;

    @Data
    public static class RefreshToken {
        private String expiration;
        private String value;
    }

    @Data
    public static class AccessToken {
        private String value;
        private String expiration;
        private String tokenType;
        private RefreshToken refreshToken;
        private List<String> scope;
        private Map<String, String> additionalInformation;
        private transient String username;
        private transient String clientId;
    }
}
