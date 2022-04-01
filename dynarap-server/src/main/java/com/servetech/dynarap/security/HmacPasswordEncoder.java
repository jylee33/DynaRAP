package com.servetech.dynarap.security;

import org.apache.commons.codec.binary.Hex;
import org.apache.commons.rng.simple.RandomSource;
import org.springframework.security.crypto.password.PasswordEncoder;

import javax.crypto.Mac;
import javax.crypto.spec.SecretKeySpec;
import java.security.GeneralSecurityException;

public class HmacPasswordEncoder implements PasswordEncoder {
    private String secret;

    public HmacPasswordEncoder(String secret) {
        this.secret = secret;
    }

    @Override
    public String encode(CharSequence rawPassword) {
        return Hex.encodeHexString(calculateHMAC(secret.toString(), rawPassword.toString()));
    }

    public String encodeWithSecret(CharSequence secret, CharSequence rawPassword) {
        return encodeWithSecret(secret, rawPassword, "HmacSHA256");
    }

    public String encodeWithSecret(CharSequence secret, CharSequence rawPassword,
                                   String algorithm) {
        return Hex.encodeHexString(
                calculateHMAC(secret.toString(), rawPassword.toString(), algorithm));
    }

    @Override
    public boolean matches(CharSequence rawPassword, String encodedPassword) {
        String encPassword =
                Hex.encodeHexString(calculateHMAC(secret.toString(), rawPassword.toString()));
        return encPassword.equals(encodedPassword);
    }

    public static String randomKey(int length) {
        String CHAR_LOWER = "abcdefghijklmnopqrstuvwxyz";
        String CHAR_UPPER = CHAR_LOWER.toUpperCase();
        String NUMBER = "0123456789";

        String DATA_FOR_RANDOM_STRING = CHAR_LOWER + CHAR_UPPER + NUMBER;

        if (length < 1) throw new IllegalArgumentException();

        StringBuilder sb = new StringBuilder(length);

        for (int i = 0; i < length; i++) {
            int rndCharAt = Math.abs(RandomSource.createInt()) % DATA_FOR_RANDOM_STRING.length();
            char rndChar = DATA_FOR_RANDOM_STRING.charAt(rndCharAt);
            sb.append(rndChar);
        }
        return sb.toString();
    }

    public static byte[] calculateHMAC(String secret, String data) {
        return calculateHMAC(secret, data, "HmacSHA256");
    }

    public static byte[] calculateHMAC(String secret, String data, String algorithm) {
        try {
            if (algorithm.equals("HmacSHA256")) {
                SecretKeySpec signingKey = new SecretKeySpec(secret.getBytes(), algorithm);
                Mac mac = Mac.getInstance(algorithm);
                mac.init(signingKey);
                byte[] rawHmac = mac.doFinal(data.getBytes());
                return rawHmac;
            } else if (algorithm.equals("MD5")) {
                SecretKeySpec signingKey = new SecretKeySpec(secret.getBytes(), "ASCII");
                Mac mac = Mac.getInstance("HMACMD5");
                mac.init(signingKey);
                mac.update(data.getBytes());
                byte[] rawHmac = mac.doFinal(data.getBytes());
                return rawHmac;
            } else {
                return new byte[0];
            }
        } catch (GeneralSecurityException e) {
            throw new IllegalArgumentException();
        }
    }
}
