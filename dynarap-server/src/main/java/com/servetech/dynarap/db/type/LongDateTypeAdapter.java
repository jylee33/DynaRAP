package com.servetech.dynarap.db.type;

import com.google.gson.*;
import com.servetech.dynarap.config.ServerConstants;

import java.lang.reflect.Type;

public class LongDateTypeAdapter implements JsonSerializer<LongDate>, JsonDeserializer<LongDate> {

    @Override
    public LongDate deserialize(JsonElement json, Type typeOfT, JsonDeserializationContext context)
            throws JsonParseException {
        long longVal = 0L;
        try {
            longVal = Long.parseLong(json.getAsString());
            return new LongDate(longVal, ServerConstants.DEFAULT_FORMAT);
        } catch (Exception e) {
            JsonObject jobj = json.getAsJsonObject();
            if (jobj.get("timestamp") != null) {
                return new LongDate(jobj.get("timestamp").getAsLong(),
                        jobj.get("dateFormat").getAsString());
            }
            return null;
        }
    }

    @Override
    public JsonElement serialize(LongDate src, Type typeOfSrc, JsonSerializationContext context) {
        JsonObject result = new JsonObject();
        result.add("timestamp", new JsonPrimitive(src.originOf()));
        result.add("dateFormat", new JsonPrimitive(src.getDateFormat()));
        result.add("dateTime", new JsonPrimitive(src.valueOf()));
        return result;
    }
}
