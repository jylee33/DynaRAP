package com.servetech.dynarap.vo;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.String64;
import lombok.Data;

@Data
public class PartVO {
    private CryptoField seq;
    private String64 partName;
    private CryptoField presetPack;
    private CryptoField presetSeq;
    private String julianStartAt;
    private String julianEndAt;
    private Double offsetStartAt;
    private Double offsetEndAt;
    private CryptoField.NAuth registerUid;

    private PresetVO presetInfo;

    @Data
    public static class Raw implements IFlexibleValue {
        private CryptoField seq;
        private CryptoField presetParamSeq;
        private Integer rowNo;
        private Double paramVal;
        private String paramValStr;
        private String julianTimeAt;
        private Double offsetTimeAt;
        private Double lpf;
        private Double hpf;

        private PresetVO.Param presetParamInfo;

        @Override
        public <T> T getValue() {
            if (paramVal == null && paramValStr == null)
                return null;

            if (paramVal != null) return (T) paramVal;

            return (T) paramValStr;
        }
    }
}
