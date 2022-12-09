package com.servetech.dynarap.db.type;

import com.google.gson.*;

import java.lang.reflect.Type;

public class UserTypeTypeAdapter implements JsonSerializer<UserType>, JsonDeserializer<UserType> {

    @Override
    public UserType deserialize(JsonElement json, Type typeOfT, JsonDeserializationContext context)
            throws JsonParseException {
        return UserType.parse(json.getAsString());
    }

    @Override
    public JsonElement serialize(UserType src, Type typeOfSrc, JsonSerializationContext context) {
        return new JsonPrimitive(src.toString());
    }
}
