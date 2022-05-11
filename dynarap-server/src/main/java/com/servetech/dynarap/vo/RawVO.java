package com.servetech.dynarap.vo;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.db.type.String64;
import lombok.Data;

import java.util.List;

@Data
public class RawVO implements IFlexibleValue {
    private CryptoField seq;
    private CryptoField uploadSeq;
    private CryptoField presetPack;
    private CryptoField presetSeq;
    private CryptoField presetParamSeq;

    private CryptoField paramPack;
    private CryptoField paramSeq;

    private String julianTimeAt;
    private Integer rowNo;
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
        private String uploadId;
        private String dataType;
        private String64 uploadName;
        private String storePath;
        private Long fileSize;
        private CryptoField flightSeq;
        private CryptoField presetPack;
        private CryptoField presetSeq;
        private LongDate uploadedAt;
        private LongDate flightAt;
        private CryptoField.NAuth registerUid;
        private boolean importDone;

        private PresetVO presetInfo;
        private UploadRequest uploadRequest;

        private String status;
        private String statusMessage;
        private long fetchCount;
        private long totalFetchCount;
        private List<String> notMappedParams;
        private List<ParamVO> mappedParams;
        private List<PartVO> partList;
    }

    @Data
    public static class UploadRequest {
        private String sourcePath;
        private CryptoField presetPack;
        private CryptoField presetSeq;
        private CryptoField flightSeq;
        private String flightAt;
        private String dataType;
        private List<UploadPart> parts;

        @Data
        public static class UploadPart {
            private String64 partName;
            private String julianStartAt;
            private String julianEndAt;
        }
    }

    @Data
    public static class CacheRow {
        private String julianTimeAt;
        private Integer rowNo;
        private List<Double> rowData;
        private List<String> rowStrData;
    }

    @Data
    public static class CacheColumn {
        private CryptoField presetSeq;
        private CryptoField paramSeq;
        private List<String> times;
        private List<Double> columnData;
        private List<String> columnStrData;
    }
}
