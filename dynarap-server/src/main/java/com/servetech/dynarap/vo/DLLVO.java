package com.servetech.dynarap.vo;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.db.type.String64;
import lombok.Data;

import java.util.List;

@Data
public class DLLVO {
    private CryptoField seq;
    private String dataSetCode;
    private String64 dataSetName;
    private String dataVersion;
    private CryptoField.NAuth registerUid;
    private LongDate createdAt;
    private String64 tags;

    private List<Param> params;

    @Data
    public static class DataSource {
        protected String sourceType = "dll";
        protected CryptoField sourceSeq = null;
        protected CryptoField paramPack;
        protected CryptoField paramSeq;
        protected String julianStartAt;
        protected String julianEndAt;
        protected double offsetStartAt;
        protected double offsetEndAt;

        public static DLLVO.DataSource getSource(DLLVO dll, DLLVO.Param param) {
            DLLVO.DataSource source = new DLLVO.DataSource();
            source.setSourceType("dll");
            source.setSourceSeq(dll.getSeq());
            source.setParamPack(param.getSeq());
            source.setParamSeq(param.getSeq());
            return source;
        }
    }

    @Data
    public static class Param {
        private CryptoField seq;
        private CryptoField dllSeq;
        private String64 paramName;
        private String paramType;
        private Integer paramNo;
        private CryptoField.NAuth registerUid;
    }

    @Data
    public static class Raw implements IFlexibleValue {
        private CryptoField seq;
        private CryptoField dllSeq;
        private CryptoField paramSeq;
        private Integer rowNo;
        private Double paramVal;
        private String paramValStr;
        private Object value;

        @Override
        public <T> T getValue() {
            if (paramVal == null && paramValStr == null)
                return null;

            if (paramVal != null) return (T) paramVal;

            return (T) paramValStr;
        }
    }
}
