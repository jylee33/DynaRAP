package com.servetech.dynarap.vo;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.String64;
import lombok.Data;

import java.util.List;

@Data
public class PartVO {
    private CryptoField seq;
    private CryptoField uploadSeq;
    private String64 partName;
    private CryptoField presetPack;
    private CryptoField presetSeq;
    private String julianStartAt;
    private String julianEndAt;
    private Double offsetStartAt;
    private Double offsetEndAt;
    private CryptoField.NAuth registerUid;

    private boolean lpfDone;
    private boolean hpfDone;

    private PresetVO presetInfo;
    private List<PresetVO.Param> params;
    private String useTime;

    @Data
    public static class DataSource {
        protected String sourceType = "part";
        protected CryptoField sourceSeq = null;
        protected CryptoField paramPack;
        protected CryptoField paramSeq;
        protected String julianStartAt;
        protected String julianEndAt;
        protected double offsetStartAt;
        protected double offsetEndAt;

        public static DataSource getSource(PartVO part, ParamVO param) {
            DataSource source = new DataSource();
            source.setSourceType("part");
            source.setSourceSeq(part.getSeq());
            source.setParamPack(param.getParamPack());
            source.setParamSeq(param.getSeq());
            source.setJulianStartAt(part.getJulianStartAt());
            source.setJulianEndAt(part.getJulianEndAt());
            source.setOffsetStartAt(part.getOffsetStartAt());
            source.setOffsetEndAt(part.getOffsetEndAt());
            return source;
        }
    }

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
