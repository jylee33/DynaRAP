package com.servetech.dynarap.db.type;

import java.lang.reflect.Array;
import java.util.Collection;
import java.util.Map;

public class MybatisEmptyChecker {
    public static boolean isEmpty(Object o) throws IllegalArgumentException {
        if (o == null) return true;

        if (o instanceof String) {
            if (((String) o).length() == 0) {
                return true;
            }
        } else if (o instanceof String64) {
            if (((String64) o).originOf() == null)
                return true;

            if (((String64) o).originOf().isEmpty()) {
                return true;
            }
        } else if (o instanceof CryptoField) {
            if (((CryptoField) o).originOf() == null)
                return true;

            if (((CryptoField) o).isEmpty()) {
                return true;
            }
        } else if (o instanceof Collection) {
            if (((Collection) o).isEmpty()) {
                return true;
            }
        } else if (o.getClass().isArray()) {
            if (Array.getLength(o) == 0) {
                return true;
            }
        } else if (o instanceof Map) {
            if (((Map) o).isEmpty()) {
                return true;
            }
        } else {
            return false;
        }

        return false;
    }

    public static boolean isNotEmpty(Object o) {
        return !isEmpty(o);
    }
}
