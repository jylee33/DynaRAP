package com.servetech.dynarap.vo;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.db.type.String64;
import lombok.Data;

@Data
public class ParamVO {
    private CryptoField seq;
    private CryptoField paramPack;
    private CryptoField paramGroupSeq;
    private String64 paramName;
    private String paramKey;
    private String paramSpec;
    private String adamsKey;
    private String zaeroKey;
    private String grtKey;
    private String fltpKey;
    private String fltsKey;
    private String partInfo;
    private String partInfoSub;
    private Double lrpX;
    private Double lrpY;
    private Double lrpZ;
    private String paramUnit;
    private Double domainMin;
    private Double domainMax;
    private Double specified;
    private String paramVal;
    private CryptoField.NAuth registerUid;
    private transient LongDate appliedAt;
    private transient LongDate appliedEndAt;

    private Long presetParamSeq;
    private CryptoField presetPack;
    private CryptoField presetSeq;

    private Group groupInfo;

    @Data
    public static class Group {
        private CryptoField seq;
        private String64 groupName;
        private String groupType;
        private CryptoField.NAuth registerUid;
        private LongDate createdAt;
    }

    @Data
    public static class NotMapped {
        private CryptoField seq;
        private CryptoField uploadSeq;
        private CryptoField paramPack;
        private CryptoField paramSeq;
        private String notMappedParamKey;
    }
}
