package com.servetech.dynarap.vo;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.db.type.String64;
import lombok.Data;

@Data
public class DirVO {
    private Long seq;
    private Long parentDirSeq;
    private CryptoField.NAuth uid;
    private String64 dirName;
    private String dirType;
    private String dirIcon;
    private LongDate createdAt;
    private CryptoField refSeq;
    private CryptoField refSubSeq;

    private DirVO parentDirInfo;
    private Object refInfo;
}
