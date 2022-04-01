package com.servetech.dynarap.security;

import com.servetech.dynarap.config.ServerConstants;
import org.springframework.security.crypto.codec.Hex;

import javax.crypto.Cipher;
import javax.crypto.spec.SecretKeySpec;


public class EnvCrypto {
    public static String decrypt(String decryptSrc) throws Exception {
        return decrypt(ServerConstants.ENV_SECRET, decryptSrc);
    }

    public static String encrypt(String encryptSrc) throws Exception {
        return encrypt(ServerConstants.ENV_SECRET, encryptSrc);
    }

    public static String decrypt(String secret, String decryptSrc) throws Exception {
        SecretKeySpec sks = new SecretKeySpec(Hex.decode(secret), "AES");
        Cipher cipher = Cipher.getInstance("AES");
        cipher.init(Cipher.DECRYPT_MODE, sks);
        return new String(cipher.doFinal(Hex.decode(decryptSrc)));
    }

    public static String encrypt(String secret, String encryptSrc) throws Exception {
        SecretKeySpec sks = new SecretKeySpec(Hex.decode(secret), "AES");
        Cipher cipher = Cipher.getInstance("AES");
        cipher.init(Cipher.ENCRYPT_MODE, sks);
        byte[] encrypted = cipher.doFinal(encryptSrc.getBytes());
        return new String(Hex.encode(encrypted));
    }
}
