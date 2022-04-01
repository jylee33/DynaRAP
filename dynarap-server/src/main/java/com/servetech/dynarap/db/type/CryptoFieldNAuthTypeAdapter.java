package com.servetech.dynarap.db.type;

import com.google.gson.*;

import java.lang.reflect.Type;

public class CryptoFieldNAuthTypeAdapter
        implements JsonSerializer<CryptoField.NAuth>, JsonDeserializer<CryptoField.NAuth> {

    @Override
    public CryptoField.NAuth deserialize(JsonElement json, Type typeOfT,
                                         JsonDeserializationContext context) throws JsonParseException {
        try {
            Object defaultValue = "";
            try {
                CryptoField.NAuth cf = CryptoField.NAuth.forcedDecode(json.getAsString());
                if (cf != null) {
                    if (cf.originTypeOf() == Integer.class)
                        defaultValue = (int) 0;
                    else if (cf.originTypeOf() == Long.class)
                        defaultValue = 0L;
                }
            } catch (Exception ex) {
            }
            return CryptoField.NAuth.decode(json.getAsString(), defaultValue);
        } catch (Exception e) {
            throw new JsonParseException(e.getMessage());
        }
    }

    @Override
    public JsonElement serialize(CryptoField.NAuth src, Type typeOfSrc,
                                 JsonSerializationContext context) {
        return new JsonPrimitive(src.valueOf());
    }
}
