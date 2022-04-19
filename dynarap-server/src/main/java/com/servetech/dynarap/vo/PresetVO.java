package com.servetech.dynarap.vo;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.db.type.String64;
import lombok.Data;

import java.util.List;

@Data
public class PresetVO {
    private CryptoField seq;
    private CryptoField presetPack;
    private String64 presetName;
    private CryptoField presetPackFrom;
    private LongDate createdAt;
    private CryptoField.NAuth registerUid;
    private transient LongDate appliedAt;
    private transient LongDate appliedEndAt;

    private List<Param> params;

    @Data
    public static class Param {
        private CryptoField seq;
        private CryptoField presetPack;
        private CryptoField presetSeq;
        private CryptoField paramPack;
        private CryptoField paramSeq;

        private ParamVO paramInfo;
    }
}
