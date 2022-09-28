package com.servetech.dynarap.vo;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.db.type.String64;
import lombok.Data;

import java.util.List;
import java.util.Map;

@Data
public class BinTableVO {
    private CryptoField seq;
    private String64 metaName;
    private LongDate createdAt;

    private List<BinData> parts;
    private List<BinData> selectedShortBlocks;
    private Map<String, String> dataProps;
    private List<BinParam> pickUpParams;

    @Data
    public static class SaveRequest {
        private String command;
        private CryptoField binMetaSeq;
        private String64 metaName;
        private List<String> parts;
        private List<String> selectedShortBlocks;
        private Map<String, String> dataProps;
        private List<BinParam> pickUpParams;
    }

    @Data
    public static class BinData {
        private CryptoField seq;
        private CryptoField binMetaSeq;
        private String dataFrom;
        private CryptoField refSeq;

        private transient boolean mark;
    }

    @Data
    public static class BinParam {
        private CryptoField seq;
        private CryptoField binMetaSeq;
        private CryptoField paramSeq;
        private CryptoField paramPack;
        private String fieldType;
        private CryptoField fieldPropSeq;
        private String paramKey;
        private String adamsKey;
        private String zaeroKey;
        private String grtKey;
        private String fltpKey;
        private String fltsKey;

        private List<BinParamData> userParamTable;
        private transient boolean mark;

        @Data
        public static class BinParamData {
            private transient CryptoField seq;
            private transient CryptoField binMetaSeq;
            private transient CryptoField paramSeq;
            private transient double dataNominal;
            private transient double dataMin;
            private transient double dataMax;
            private double nominal;
            private double min;
            private double max;
        }
    }
}
