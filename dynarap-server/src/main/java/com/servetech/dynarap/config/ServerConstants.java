package com.servetech.dynarap.config;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.servetech.dynarap.db.type.*;
import com.servetech.dynarap.security.HmacPasswordEncoder;
import org.apache.commons.codec.binary.Hex;
import org.springframework.security.crypto.password.PasswordEncoder;

import javax.crypto.KeyGenerator;
import javax.crypto.SecretKey;
import javax.script.ScriptEngine;
import javax.script.ScriptEngineManager;
import java.text.NumberFormat;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.regex.Matcher;
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
        /*
        try {
            KeyGenerator keygen = KeyGenerator.getInstance("AES");
            keygen.init(128);
            SecretKey secret = keygen.generateKey();
            String secretKey = Hex.encodeHexString(secret.getEncoded());
            System.out.println(secretKey);
        } catch (Exception e) {
            e.printStackTrace();
        }
         */
        /*
        double jd = 31557600000L;
        double jdms = 3.1688087814029E-8 / 1000;
        System.out.println(jdms * (1640962800000L + 24 * 60 * 60 * 1000));
        System.out.println(jdms * (1672495200000L + 59 * 60 * 1000 + 59 * 1000 + 999));
        System.out.println(System.currentTimeMillis() / jd);
        System.out.println(getJulianTimeOffset("344:10:49:24.429500", "344:10:49:24.431500"));
        */
        String test = "${SN910P} + ${SN999S} * 1.2";
        test = test.replaceAll("\\s+", "");

        String loopTest = test;
        List<String> params = extractParams(loopTest);
        System.out.println(params);

        loopTest = loopTest.replaceAll("\\$\\{" + "SN910P" + "\\}", "123.45");
        loopTest = loopTest.replaceAll("\\$\\{" + "SN999S" + "\\}", "456.78");
        System.out.println(loopTest);

        try {
            ScriptEngineManager mgr = new ScriptEngineManager();
            ScriptEngine engine = mgr.getEngineByName("JavaScript");
            Double qVal = (Double) engine.eval(loopTest);
            System.out.println("qVal=" + qVal);
        } catch(Exception e) {

        }
    }

    private static List<String> extractParams(String source) {
        String prefix = "${";
        String postfix = "}";
        int si = -1, ei = -1;

        List<String> resultSet = new ArrayList<>();
        String test = source;
        while ((si = test.indexOf(prefix)) > -1) {
            ei = test.indexOf(postfix, si + prefix.length());
            if (ei == -1) break;
            resultSet.add(test.substring(si + prefix.length(), ei));
            test = test.substring(ei + postfix.length());
        }
        return resultSet;
    }

    private static double getJulianTimeOffset(String julianFrom, String julianCurrent) {
        // 년도 값을 뺀 소수 부분을 포메팅 함.
        // 년도 값을 포함한 msec로 변환후 차이 값을 반환함.
        double jd = 31557600000L;
        double jdms = 3.1688087814029E-8 / 1000;
        String jdStr = Double.toString(System.currentTimeMillis() * jdms);
        int jdPrefix = Integer.parseInt(jdStr.substring(0, jdStr.lastIndexOf(".")));

        String jdFrom = jdPrefix + "." + julianFrom.replaceAll("[^0-9]", "");
        double jdFromMillis = (Double.parseDouble(jdFrom) / jdms);

        String jdCurrent = jdPrefix + "." + julianCurrent.replaceAll("[^0-9]", "");
        double jdCurrentMillis = (Double.parseDouble(jdCurrent) / jdms);

        return jdCurrentMillis - jdFromMillis;
    }
}
