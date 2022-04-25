package com.servetech.dynarap.config;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.servetech.dynarap.db.type.*;
import com.servetech.dynarap.security.HmacPasswordEncoder;
import org.apache.commons.codec.binary.Hex;
import org.springframework.security.crypto.password.PasswordEncoder;

import javax.crypto.KeyGenerator;
import javax.crypto.SecretKey;
import java.text.NumberFormat;
import java.util.regex.Pattern;

public class ServerConstants {
    public static final String CONTEXT = "dynarap-server";
    public static final String SERVICE_NAME = "dynarap";

    public static final String SERVER_SECRET = "1f1605ff-1adc-43f4-b94f-11ebcdf90b4c";
    public static final String ENV_SECRET = "f35d7ac973623f2f1106829079ece74f";

    public static final String NAUTH_SECRET = "602dab6f-bdbc-43bb-9708-11341d7adb49";
    public static final String NAUTH_ENV_SECRET = "f2d3fa9e26d9e01376a3af87bf58f748";

    //public static final String DEFAULT_FORMAT = "yyyy-MM-dd HH:mm:ss";
    public static final String DEFAULT_FORMAT = "yyyy.MM.dd HH:mm:ss";
    public static final String WELL_FORMED_DATETIME = "yyyy-MM-dd HH:mm:ss";
    public static final String NOCHAR_FORMAT = "yyyyMMddHHmmss";

    public static final NumberFormat CURRENCY = NumberFormat.getInstance();

    public static Gson GSON = null;

    public static final Pattern DATE_PATTERN = Pattern.compile(
            "^((2000|2400|2800|(19|2[0-9](0[48]|[2468][048]|[13579][26])))-?02-?29)$"
                    + "|^(((19|2[0-9])[0-9]{2})-?02-?(0[1-9]|1[0-9]|2[0-8]))$"
                    + "|^(((19|2[0-9])[0-9]{2})-?(0[13578]|10|12)-?(0[1-9]|[12][0-9]|3[01]))$"
                    + "|^(((19|2[0-9])[0-9]{2})-?(0[469]|11)-?(0[1-9]|[12][0-9]|30))$");

    public static final Pattern EXTRACT_URL = Pattern.compile("(?:^|[\\W])((ht|f)tp(s?):\\/\\/|www\\.)*"
                    + "(([\\w\\-]+\\.){1,}?([\\w\\-.~]+\\/?)*"
                    + "[\\p{Alnum}.,%_=?&#\\-+()\\[\\]\\*$~@!:/{};']*)",
            Pattern.CASE_INSENSITIVE | Pattern.MULTILINE | Pattern.DOTALL);

    public static final PasswordEncoder PASSWORD_ENCODER;

    static {
        CURRENCY.setMaximumIntegerDigits(16);
        CURRENCY.setMaximumFractionDigits(0);

        GSON = new GsonBuilder()
                .registerTypeHierarchyAdapter(String64.class,
                        new String64TypeAdapter())
                .registerTypeHierarchyAdapter(LongDate.class,
                        new LongDateTypeAdapter())
                .registerTypeHierarchyAdapter(CryptoField.class,
                        new CryptoFieldTypeAdapter())
                .registerTypeHierarchyAdapter(CryptoField.NAuth.class,
                        new CryptoFieldNAuthTypeAdapter())
                .registerTypeHierarchyAdapter(UserType.class,
                        new UserTypeTypeAdapter())
                .registerTypeHierarchyAdapter(UserType.NAuth.class,
                        new UserTypeNAuthTypeAdapter())
                .disableHtmlEscaping().create();

        PASSWORD_ENCODER = new HmacPasswordEncoder(NAUTH_SECRET);
    }

    public static void main(String[] args) {
        try {
            KeyGenerator keygen = KeyGenerator.getInstance("AES");
            keygen.init(128);
            SecretKey secret = keygen.generateKey();
            String secretKey = Hex.encodeHexString(secret.getEncoded());
            System.out.println(secretKey);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
