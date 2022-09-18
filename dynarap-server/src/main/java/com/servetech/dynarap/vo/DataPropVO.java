package com.servetech.dynarap.vo;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.db.type.String64;
import lombok.Data;

@Data
public class DataPropVO {
    private CryptoField seq;
    private String64 propName;
    private String64 propValue;
    private String referenceType;
    private CryptoField referenceKey;
    private LongDate updatedAt;

    private boolean marked = false;
}
