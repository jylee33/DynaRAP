package com.servetech.dynarap.controller;

import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.servetech.dynarap.DynaRAPServerApplication;
import org.apache.commons.codec.binary.Base64;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.env.Environment;
import org.springframework.data.redis.core.*;

import javax.annotation.Resource;
import javax.servlet.http.HttpServletRequest;
import java.net.URLDecoder;
import java.net.URLEncoder;
import java.util.HashMap;
import java.util.Map;

public class ApiController {
    @Autowired
    protected DynaRAPServerApplication application;

    @Autowired
    private static Environment environment;

    @Resource(name = "redisTemplate")
    protected ValueOperations<String, String> valueOps;

    @Resource(name = "redisTemplate")
    protected ListOperations<String, String> listOps;

    @Resource(name = "redisTemplate")
    protected HashOperations<String, String, String> hashOps;

    @Resource(name = "redisTemplate")
    protected SetOperations<String, String> setOps;

    @Resource(name = "redisTemplate")
    protected ZSetOperations<String, String> zsetOps;

    private static final Map<Class, Object> serviceMap = new HashMap<>();

    public static Environment getEnvironment() {
        if (environment == null) {
            environment = DynaRAPServerApplication.global.getEnvironment();
        }
        return environment;
    }

    public <T> T getOperation(String opType) {
        if (opType.equals("list"))
            return (T) listOps;
        if (opType.equals("hash"))
            return (T) hashOps;
        if (opType.equals("zset"))
            return (T) zsetOps;
        return null;
    }

    public static <T> T getController(Class<T> clazz) {
        return null;
    }

    public static <T> T getService(Class<T> clazz) {
        Object objService = serviceMap.get(clazz);
        if (objService == null) {
            T targetService = DynaRAPServerApplication.global.getBean(clazz);
            serviceMap.put(clazz, targetService);
            objService = targetService;
        }
        return (T) objService;
    }

    public static boolean checkJsonEmpty(JsonObject payload, String keyName) {
        if (payload.get(keyName) != null
                && !payload.get(keyName).isJsonNull()) {
            JsonElement elem = payload.get(keyName);
            if (elem.isJsonArray() && elem.getAsJsonArray().size() > 0)
                return false;

            if (elem.isJsonPrimitive() && elem.getAsJsonPrimitive().isString() && !elem.getAsString().isEmpty())
                return false;

            if (elem.isJsonPrimitive())
                return false;

            if (elem.isJsonObject() && !elem.getAsJsonObject().isJsonNull())
                return false;
        }
        return true;
    }

    public static boolean isMimeTypeImage(String mimeType) {
        if (mimeType.equalsIgnoreCase("image/jpeg"))
            return true;
        if (mimeType.equalsIgnoreCase("image/png"))
            return true;
        if (mimeType.equalsIgnoreCase("image/gif"))
            return true;
        if (mimeType.equalsIgnoreCase("image/bmp"))
            return true;
        if (mimeType.equalsIgnoreCase("image/svg+xml"))
            return true;
        return false;
    }

    public static String getMimeType(String fileExt) {
        if (fileExt.equalsIgnoreCase("jpg") || fileExt.equalsIgnoreCase("jpeg"))
            return "image/jpeg";
        if (fileExt.equalsIgnoreCase("png"))
            return "image/png";
        if (fileExt.equalsIgnoreCase("gif"))
            return "image/gif";
        if (fileExt.equalsIgnoreCase("bmp"))
            return "image/bmp";
        if (fileExt.equalsIgnoreCase("svg"))
            return "image/svg+xml";
        if (fileExt.equalsIgnoreCase("mp4"))
            return "video/mp4";
        if (fileExt.equalsIgnoreCase("doc"))
            return "application/msword";
        if (fileExt.equalsIgnoreCase("docx"))
            return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        if (fileExt.equalsIgnoreCase("xls"))
            return "application/vnd.ms-excel";
        if (fileExt.equalsIgnoreCase("xlsx"))
            return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        if (fileExt.equalsIgnoreCase("ppt"))
            return "application/vnd.ms-powerpoint";
        if (fileExt.equalsIgnoreCase("pptx"))
            return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
        if (fileExt.equalsIgnoreCase("pdf"))
            return "application/pdf";

        return "application/octet-stream";
    }

    protected static String getEncodedFileSeq(String fileUrl) {
        try {
            String findWhat = "/open-api/res/";
            if (fileUrl.contains(findWhat)) {
                String decResourcePath = URLDecoder.decode(fileUrl.substring(fileUrl.indexOf(findWhat) + findWhat.length()), "utf-8");
                decResourcePath = new String(Base64.decodeBase64(decResourcePath), "UTF-8");
                String encFileSeq = decResourcePath.substring(decResourcePath.lastIndexOf("/") + 1);
                encFileSeq = encFileSeq.substring(0, encFileSeq.lastIndexOf("."));
                return encFileSeq;
            }
            return "";
        } catch(Exception e) {
            return "";
        }
    }

    protected static String getHyphenFormat(String source) {
        StringBuilder sbHyphen = new StringBuilder();
        for (int i = 0; i < source.length(); i += 4) {
            if (sbHyphen.length() > 0) sbHyphen.append("-");
            sbHyphen.append(source.substring(i, Math.min(i + 4, source.length())));
        }
        return sbHyphen.toString();
    }

    protected static String getBrowser(HttpServletRequest req) {
        String userAgent = req.getHeader("User-Agent");
        if(userAgent.indexOf("MSIE") > -1 || userAgent.indexOf("Trident") > -1 //IE11
                || userAgent.indexOf("Edge") > -1) {
            return "MSIE";
        }
        else if(userAgent.indexOf("Chrome") > -1) {
            return "Chrome";
        }
        else if(userAgent.indexOf("Opera") > -1) {
            return "Opera";
        }
        else if(userAgent.indexOf("Safari") > -1) {
            return "Safari";
        }
        else if(userAgent.indexOf("Firefox") > -1) {
            return "Firefox";
        }
        else {
            return null;
        }
    }

    protected static String getDownloadFileName(String browser, String fileName) {
        String reFileName = null;
        try {
            if (browser.equals("MSIE") || browser.equals("Trident") || browser.equals("Edge")) {
                reFileName = URLEncoder.encode(fileName, "UTF-8").replaceAll("\\+", "%20");
            }
            else {
                if (browser.equals("Chrome")) {
                    StringBuffer sb = new StringBuffer();
                    for (int i = 0; i < fileName.length(); i++) {
                        char c = fileName.charAt(i);
                        if (c > '~') {
                            sb.append(URLEncoder.encode(Character.toString(c), "UTF-8"));
                        }
                        else {
                            sb.append(c);
                        }
                    }
                    reFileName = sb.toString();
                }
                else {
                    reFileName = new String(fileName.getBytes("UTF-8"), "ISO-8859-1");
                }

                if (browser.equals("Safari") || browser.equals("Firefox"))
                    reFileName = URLDecoder.decode(reFileName);
            }
        } catch(Exception e) {}

        return reFileName;
    }
}
