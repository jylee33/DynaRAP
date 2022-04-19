package com.servetech.dynarap.vo;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.db.type.String64;
import lombok.Data;

@Data
public class FlightVO {
    private CryptoField seq;
    private String64 flightName;
    private String flightType;
    private LongDate createdAt;
    private LongDate registeredAt;
    private CryptoField.NAuth registerUid;
    private LongDate lastFlightAt;
}
