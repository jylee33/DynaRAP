package com.servetech.dynarap.vo;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.db.type.String64;
import lombok.Data;

@Data
public class ShortBlockVO {
    private CryptoField seq;
    private CryptoField blockMetaSeq;
    private CryptoField partSeq;
    private Integer blockNo;
    private String64 blockName;
    private String julianStartAt;
    private String julianEndAt;
    private Double offsetStartAt;
    private Double offsetEndAt;
    private CryptoField.NAuth registerUid;

    private Meta blockMetaInfo;
    private PartVO partInfo;

    @Data
    public static class Meta {
        private CryptoField seq;
        private CryptoField partSeq;
        private CryptoField selectedPresetPack; /* 선택된 preset 정보 노출 */
        private CryptoField selectedPresetSeq; /* 선택된 preset 정보 노출 */
        private Float overlap;
        private Float sliceTime;
        private CryptoField.NAuth registerUid;
        private LongDate createdAt;

        private PartVO partInfo;
    }

    /* preset에 담긴 param을 개별로 저장함. 임의 파라미터 추가때문 */
    @Data
    public static class Param {
        private CryptoField seq;
        private CryptoField blockSeq;
        private Integer paramNo;
        private CryptoField paramPack;
        private CryptoField paramSeq;

        //private ShortBlockVO shortBlockInfo;
        private ParamVO paramInfo;
    }

    @Data
    public static class Raw implements IFlexibleValue {
        private CryptoField seq;
        private CryptoField blockSeq;
        private CryptoField paramPack;
        private CryptoField paramSeq;
        private Integer rowNo;
        private Double paramVal;
        private String paramValStr;
        private String julianTimeAt;
        private Double offsetTimeAt;
        private Double lpf;
        private Double hpf;

        private ShortBlockVO shortBlockInfo;
        private ParamVO paramInfo;

        @Override
        public <T> T getValue() {
            if (paramVal == null && paramValStr == null)
                return null;

            if (paramVal != null) return (T) paramVal;

            return (T) paramValStr;
        }
    }
}
