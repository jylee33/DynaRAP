package com.servetech.dynarap.db.type;

import com.google.gson.*;

import java.lang.reflect.Type;

public class String64TypeAdapter implements JsonSerializer<String64>, JsonDeserializer<String64> {

    @Override
    public String64 deserialize(JsonElement json, Type typeOfT, JsonDeserializationContext context)
            throws JsonParseException {
        if (json == null || json.isJsonNull())
            return new String64("");

        if (String64.isBase64(json.getAsString()))
            return String64.decode(json.getAsString());

        return new String64(json.getAsString());
    }

    @Override
    public JsonElement serialize(String64 src, Type typeOfSrc, JsonSerializationContext context) {
        if (src == null || src.isEmpty())
            return new JsonPrimitive("");

        if (String64.isBase64(src.valueOf()))
            return new JsonPrimitive(src.valueOf());

        return new JsonPrimitive(new String64(src.valueOf()).valueOf());
    }
}
