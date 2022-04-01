package com.servetech.dynarap.vo;

import com.servetech.dynarap.config.ServerConstants;
import com.servetech.dynarap.db.type.LongDate;
import lombok.Data;
import lombok.EqualsAndHashCode;

import java.lang.reflect.Type;

@Data
@EqualsAndHashCode(callSuper = false)
public class ResponseVO {
    private int code;
    private Object message;
    private Object response;
    private Object additionalResponse;
    private LongDate responseAt;
    private int resultCount;

    public <T> T getTypedResponse() {
        return (T) response;
    }

    public <T> T getTypedResponse(Type responseType) {
        String jsonSrc = ServerConstants.GSON.toJson(response);
        T instance = ServerConstants.GSON.fromJson(jsonSrc, responseType);
        return instance;
    }
}
