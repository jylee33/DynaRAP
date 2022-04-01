package com.servetech.dynarap.db.type;

public class MybatisConditionChecker {
    public static boolean isEquals(Object o, Object t) throws IllegalArgumentException {
        if (o == null && t == null) return true;

        if ((o == null && t != null) || (o != null && t == null)) return false;

        if (o instanceof String && t instanceof String) {
            return ((String) o).equals((String) t);
        } else if (o instanceof String64 && t instanceof String64) {
            if (((String64) o).originOf() == null && ((String64) t).originOf() == null)
                return true;
            if ((((String64) o).originOf() != null && ((String64) t).originOf() == null)
                || (((String64) o).originOf() == null && ((String64) t).originOf() != null))
                return false;
            return ((String64) o).originOf().equals(((String64) t).originOf());
        } else {
            return false;
        }
    }

    public static boolean contains(String source, String findWhat) throws IllegalArgumentException {
        if (source == null || findWhat == null) return false;

        return source.contains(findWhat);
    }
}
