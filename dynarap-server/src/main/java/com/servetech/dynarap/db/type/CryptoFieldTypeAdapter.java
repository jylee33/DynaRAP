package com.servetech.dynarap.db.type;

import com.google.gson.*;

import java.lang.reflect.Type;

public class CryptoFieldTypeAdapter
        implements JsonSerializer<CryptoField>, JsonDeserializer<CryptoField> {

    @Override
    public CryptoField deserialize(JsonElement json, Type typeOfT,
                                   JsonDeserializationContext context) throws JsonParseException {
        try {
            Object defaultValue = "";
            try {
                CryptoField cf = CryptoField.forcedDecode(json.getAsString());
                if (cf != null) {
                    if (cf.originTypeOf() == Integer.class)
                        defaultValue = (int) 0;
                    else if (cf.originTypeOf() == Long.class)
                        defaultValue = 0L;
                }
            } catch (Exception ex) {
            }
            return CryptoField.decode(json.getAsString(), defaultValue);
        } catch (Exception e) {
            throw new JsonParseException(e.getMessage());
        }
    }

    @Override
    public JsonElement serialize(CryptoField src, Type typeOfSrc,
                                 JsonSerializationContext context) {
        return new JsonPrimitive(src.valueOf());
    }
}
