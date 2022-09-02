package com.servetech.dynarap.vo;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.db.type.String64;
import lombok.Data;

import java.util.List;

@Data
public class ParamModuleVO {
    private CryptoField seq;
    private String64 moduleName;
    private CryptoField copyFromSeq;
    private LongDate createdAt;
    private boolean referenced;
    private transient boolean deleted;

    private ParamModuleVO originInfo;

    @Data
    public static class Source {
        private CryptoField seq;
        private CryptoField moduleSeq;
        private String sourceType;
        private CryptoField sourceSeq;
        private CryptoField paramPack;
        private CryptoField paramSeq;
        private String julianStartAt;
        private String julianEndAt;
        private double offsetStartAt;
        private double offsetEndAt;
    }

    @Data
    public static class Equation {
        private CryptoField seq;
        private CryptoField moduleSeq;
        private String64 eqName;
        private String equation;
        private String julianStartAt;
        private String julianEndAt;
        private double offsetStartAt;
        private double offsetEndAt;
    }

    @Data
    public static class Plot {
        private CryptoField seq;
        private CryptoField moduleSeq;
        private String64 plotName;
        private LongDate createdAt;

        private List<Source> plotSourceList;

        @Data
        public static class Source {
            private CryptoField seq;
            private CryptoField moduleSeq;
            private CryptoField plotSeq;
            private String sourceType;
            private CryptoField sourceSeq;
            private CryptoField paramPack;
            private CryptoField paramSeq;
            private String julianStartAt;
            private String julianEndAt;
            private double offsetStartAt;
            private double offsetEndAt;
        }
    }
}
