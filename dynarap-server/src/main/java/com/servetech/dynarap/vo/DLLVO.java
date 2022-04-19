package com.servetech.dynarap.vo;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.db.type.String64;
import lombok.Data;

@Data
public class DLLVO {
    private CryptoField seq;
    private String dataSetCode;
    private String64 dataSetName;
    private String dataVersion;
    private CryptoField.NAuth registerUid;
    private LongDate createdAt;

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
        private Double paramVal;
        private String paramValStr;

        @Override
        public <T> T getValue() {
            if (paramVal == null && paramValStr == null)
                return null;

            if (paramVal != null) return (T) paramVal;

            return (T) paramValStr;
        }
    }
}
