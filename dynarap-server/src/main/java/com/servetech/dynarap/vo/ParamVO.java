package com.servetech.dynarap.vo;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.db.type.String64;
import lombok.Data;

import java.util.Map;

@Data
public class ParamVO {
    private CryptoField seq;
    private CryptoField paramPack;
    private CryptoField propSeq;
    private String paramKey;
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
    private CryptoField.NAuth registerUid;
    private transient LongDate appliedAt;
    private transient LongDate appliedEndAt;

    private Map<String, Object> extras;
    private String64 tags;

    private Long referenceSeq;
    private CryptoField paramSearchSeq;

    private CryptoField presetPack;
    private CryptoField presetSeq;

    private Prop propInfo;
    private ShortBlockVO.ParamData paramValueMap;

    @Data
    public static class Prop {
        private CryptoField seq;
        private String propCode;
        private String propType;
        private String paramUnit;
        private CryptoField.NAuth registerUid;
        private LongDate createdAt;
        private boolean deleted;
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
