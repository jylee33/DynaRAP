package com.servetech.dynarap.vo;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.db.type.String64;
import lombok.Data;

import java.util.List;

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
    private List<Param> params;
    private String useTime;

    @Data
    public static class DataSource {
        protected String sourceType = "shortblock";
        protected CryptoField sourceSeq = null;
        protected CryptoField paramPack;
        protected CryptoField paramSeq;
        protected String julianStartAt;
        protected String julianEndAt;
        protected double offsetStartAt;
        protected double offsetEndAt;

        public static ShortBlockVO.DataSource getSource(ShortBlockVO shortBlock, Param param) {
            ShortBlockVO.DataSource source = new ShortBlockVO.DataSource();
            source.setSourceType("shortblock");
            source.setSourceSeq(shortBlock.getSeq());
            source.setParamPack(param.getParamPack());
            source.setParamSeq(param.getSeq());
            source.setJulianStartAt(shortBlock.getJulianStartAt());
            source.setJulianEndAt(shortBlock.getJulianEndAt());
            source.setOffsetStartAt(shortBlock.getOffsetStartAt());
            source.setOffsetEndAt(shortBlock.getOffsetEndAt());
            return source;
        }
    }

    @Data
    public static class Meta {
        private CryptoField seq;
        private CryptoField partSeq;
        private Float overlap;
        private Float sliceTime;
        private CryptoField.NAuth registerUid;
        private LongDate createdAt;

        private boolean createDone;

        private CreateRequest createRequest;

        private String status;
        private String statusMessage;
        private long fetchCount;
        private long totalFetchCount;
        private List<ShortBlockVO> shortBlockList;

        private PartVO partInfo;
        private List<ShortBlockVO.Param> shortBlockParamList;
    }

    /* preset에 담긴 param을 개별로 저장함. 임의 파라미터 추가때문 */
    @Data
    public static class Param {
        private CryptoField seq;
        private CryptoField blockMetaSeq;
        private Integer paramNo;
        private CryptoField paramPack;
        private CryptoField paramSeq;

        private Long unionParamSeq;
        private CryptoField paramSearchSeq;

        private String64 paramName;
        private String paramKey;
        private String adamsKey;
        private String zaeroKey;
        private String grtKey;
        private String fltpKey;
        private String fltsKey;
        private String paramUnit;
        private String propType;
        private String propCode;

        private ParamVO paramInfo;
    }

    @Data
    public static class ParamData {
        private CryptoField seq;
        private CryptoField blockMetaSeq;
        private CryptoField blockSeq;
        private Long unionParamSeq;
        private double blockMin;
        private double blockMax;
        private double blockAvg;

        private String psd;
        private String frequency;
        private double rms;
        private double n0;
        private String peak;
        private String zarray;
        private String lpfPsd;
        private String lpfFrequency;
        private double lpfRms;
        private double lpfN0;
        private String lpfPeak;
        private String lpfZarray;
        private String hpfPsd;
        private String hpfFrequency;
        private double hpfRms;
        private double hpfN0;
        private String hpfPeak;
        private String hpfZarray;
        private String bpfPsd;
        private String bpfFrequency;
        private double bpfRms;
        private double bpfN0;
        private String bpfPeak;
        private String bpfZarray;

        private double blockLpfMin;
        private double blockLpfMax;
        private double blockLpfAvg;
        private double blockHpfMin;
        private double blockHpfMax;
        private double blockHpfAvg;
        private double blockBpfMin;
        private double blockBpfMax;
        private double blockBpfAvg;
    }

    @Data
    public static class Raw implements IFlexibleValue {
        private CryptoField seq;
        private CryptoField blockSeq;
        private CryptoField blockParamSeq;
        private Integer rowNo;
        private Double paramVal;
        private String paramValStr;
        private String julianTimeAt;
        private Double offsetTimeAt;
        private Double lpf;
        private Double hpf;

        private ShortBlockVO shortBlockInfo;
        private ShortBlockVO.Param paramInfo;

        @Override
        public <T> T getValue() {
            if (paramVal == null && paramValStr == null)
                return null;

            if (paramVal != null) return (T) paramVal;

            return (T) paramValStr;
        }
    }

    @Data
    public static class CreateRequest {
        private CryptoField blockMetaSeq;
        private CryptoField partSeq;
        private float sliceTime;
        private float overlap;
        private List<Parameter> parameters;
        private List<ShortBlock> shortBlocks;
        private boolean forcedCreate;

        @Data
        public static class Parameter {
            private CryptoField paramPack;
            private CryptoField paramSeq;
            private String paramKey;
            private String adamsKey;
            private String zaeroKey;
            private String grtKey;
            private String fltpKey;
            private String fltsKey;
            private CryptoField propSeq;
        }

        @Data
        public static class ShortBlock {
            private int blockNo;
            private String64 blockName;
            private String julianStartAt;
            private String julianEndAt;
        }
    }
}
