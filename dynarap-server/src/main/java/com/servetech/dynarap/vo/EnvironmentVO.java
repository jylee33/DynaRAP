package com.servetech.dynarap.vo;

import lombok.Data;

import java.util.List;

@Data
public class EnvironmentVO {
    private List<String> allowedAppVersions;
    private List<String> needUpdateAppVersions;
    private boolean useEncryption;

    private String androidServiceVersion;
    private String iosServiceVersion;
    private String webappServiceVersion;

    private String activeHost;
    private String uploadHost;

    private boolean debugMode;
}
