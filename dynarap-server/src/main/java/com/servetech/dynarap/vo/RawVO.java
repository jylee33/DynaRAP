package com.servetech.dynarap.vo;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.db.type.String64;
import lombok.Data;

@Data
public class RawVO implements IFlexibleValue {
    private CryptoField seq;
    private CryptoField presetPack;
    private CryptoField presetSeq;
    private CryptoField presetParamSeq;
    private Double paramVal;
    private String paramValStr;

    private PresetVO presetInfo;
    private PresetVO.Param presetParamInfo;

    @Override
    public <T> T getValue() {
        if (paramVal == null && paramValStr == null)
            return null;

        if (paramVal != null) return (T) paramVal;

        return (T) paramValStr;
    }

    @Data
    public static class Upload {
        private CryptoField seq;
        private String64 uploadName;
        private String storePath;
        private Long fileSize;
        private CryptoField flightSeq;
        private CryptoField presetPack;
        private CryptoField presetSeq;
        private LongDate uploadedAt;
        private LongDate flightAt;
        private CryptoField.NAuth registerUid;

        private PresetVO presetInfo;
    }
}
