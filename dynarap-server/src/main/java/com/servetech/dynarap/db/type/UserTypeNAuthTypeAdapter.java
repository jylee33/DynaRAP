package com.servetech.dynarap.db.type;

import com.google.gson.*;

import java.lang.reflect.Type;

public class UserTypeNAuthTypeAdapter implements JsonSerializer<UserType.NAuth>, JsonDeserializer<UserType.NAuth> {

    @Override
    public UserType.NAuth deserialize(JsonElement json, Type typeOfT, JsonDeserializationContext context)
            throws JsonParseException {
        return UserType.NAuth.parse(json.getAsString());
    }

    @Override
    public JsonElement serialize(UserType.NAuth src, Type typeOfSrc, JsonSerializationContext context) {
        return new JsonPrimitive(src.toString());
    }
}
