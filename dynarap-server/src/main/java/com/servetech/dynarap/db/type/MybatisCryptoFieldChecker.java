package com.servetech.dynarap.db.type;

public class MybatisCryptoFieldChecker {
    public static boolean isEmpty(Object o) throws IllegalArgumentException {
        if (o == null) return true;

        if (o instanceof CryptoField) {
            CryptoField co = (CryptoField) o;
            if (co.isEmpty()) return true;
        } else {
            return false;
        }

        return false;
    }

    public static boolean isNotEmpty(Object o) {
        return !isEmpty(o);
    }
}
