package com.servetech.dynarap.db.type;

import com.servetech.dynarap.config.ServerConstants;
import org.apache.commons.codec.binary.Base64;

import java.io.UnsupportedEncodingException;
import java.util.regex.PatternSyntaxException;

public class String64 {

    private transient String origin = null;
    private String encoded = null;

    public String64() {
        origin = "";
    }

    public String64(String value) {
        origin = value;
        encode(origin);
    }

    public String64(byte[] byteValues) {
        origin = new String(byteValues);
        encode(origin);
    }

    public String64(byte[] byteValues, String encoding) {
        try {
            origin = new String(byteValues, encoding);
        } catch (UnsupportedEncodingException e) {
        }
        encode(origin);
    }

    public String valueOf() {
        return encoded;
    }

    public String originOf() {
        if (origin == null)
            return "";
        return origin;
    }

    public boolean isEmpty() {
        if (origin == null || origin.length() == 0)
            return true;
        return false;
    }

    @Override
    public boolean equals(Object obj) {
        if (!(obj instanceof String64))
            return false;

        String64 targetObj = (String64) obj;
        if (this.originOf() == null && ((String64) obj).originOf() == null)
            return true;

        if (this.originOf() != null)
            return this.originOf().equals(targetObj.originOf());

        return false;
    }

    public static String64 decode(String encoded) {
        if (encoded == null)
            return new String64("");

        if (isBase64(encoded)) {
            try {
                String decoded =
                        new String(Base64.decodeBase64(encoded.getBytes("UTF-8")), "UTF-8");
                return new String64(decoded);
            } catch (UnsupportedEncodingException e) {
                return new String64(encoded);
            }
        }

        return new String64(encoded);
    }

    public static boolean isBase64(String encoded) {
        if (encoded == null || encoded.isEmpty())
            return false;

        String regExpTel = "^(01[016789]{1}|02|0[3-7]{1}[0-9]{1})-?[0-9]{3,4}-?[0-9]{4}$";
        String regExpBizNo = "^[0-9]{3}-?[0-9]{2}-?[0-9]{5}$";

        try {
            if (encoded.matches(regExpTel))
                return false;
            if (encoded.matches(regExpBizNo))
                return false;
        } catch(PatternSyntaxException pse) {
            pse.printStackTrace();
            System.out.println("[[[[[ ERROR=" + encoded + " ]]]]]");
        }

        return Base64.isBase64(encoded);
    }

    @Override
    public String toString() {
        return ServerConstants.GSON.toJson(this);
    }

    private void encode(String source) {
        if (source == null)
            encoded = "";
        else {
            try {
                encoded = new String(Base64.encodeBase64(source.getBytes("UTF-8")), "UTF-8");
            } catch (UnsupportedEncodingException e) {
                encoded = "Pw==";
            }
        }
    }
}
