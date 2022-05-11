package com.servetech.dynarap.db.type;


import com.servetech.dynarap.config.ServerConstants;
import com.servetech.dynarap.security.EnvCrypto;

public class CryptoField {

    protected transient Object origin = null;
    protected transient Class<?> originType = Object.class;

    public static final CryptoField LZERO = new CryptoField(0L);
    public static final CryptoField ZERO = new CryptoField((int) 0);
    public static final CryptoField EMPTY = new CryptoField("");
    public static final CryptoField GCC = new CryptoField("global.chat.channel");

    protected String encoded = null;

    public CryptoField() {
        origin = "";
    }

    public CryptoField(Object value) {
        origin = value;

        if (value != null) {
            Class<?> enclosingClass = value.getClass().getEnclosingClass();
            if (enclosingClass != null) {
                originType = enclosingClass;
            } else {
                originType = value.getClass();
            }
        }
        else {
            originType = String.class;
        }

        encode(origin);
    }

    public String valueOf() {
        return encoded;
    }

    public <T> T originOf() {
        return (T) origin;
    }

    public boolean isEmpty() {
        if (origin == null) return true;
        if (originType == Long.class && (Long) origin == 0L) return true;
        if (originType == Integer.class && (Integer) origin == 0) return true;
        if (originType == String.class && ((String) origin).isEmpty()) return true;
        return false;
    }

    @Override
    public boolean equals(Object target) {
        if (!(target instanceof CryptoField))
            return false;

        if (originTypeOf() != ((CryptoField) target).originTypeOf())
            return false;

        if (originTypeOf() == Long.class) {
            long lv = ((Long) origin).longValue();
            long tv = ((Long) ((CryptoField) target).originOf()).longValue();
            return lv == tv;
        } else if (originTypeOf() == Integer.class) {
            int lv = ((Integer) origin).intValue();
            int tv = ((Integer) ((CryptoField) target).originOf()).intValue();
            return lv == tv;
        } else if (originTypeOf() == String.class) {
            return ((String) origin).equals(((CryptoField) target).originOf());
        }

        return false;
    }

    public Class<?> originTypeOf() {
        return originType;
    }

    @Override
    public String toString() {
        return valueOf();
    }

    public static class NAuth extends CryptoField {

        public NAuth() { super(); }
        public NAuth(Object value) {
            super();

            origin = value;

            Class<?> enclosingClass = value.getClass().getEnclosingClass();
            if (enclosingClass != null) {
                originType = enclosingClass;
            } else {
                originType = value.getClass();
            }

            encode(origin);
        }

        @Override
        protected void encode(Object source) {
            if (source == null) {
                encoded = "";
            } else {
                try {
                    encoded = EnvCrypto.encrypt(ServerConstants.NAUTH_ENV_SECRET, originType.getName() + "|" + origin.toString());
                } catch (Exception e) {
                    encoded = "err_crypto";
                }
            }
        }

        public static NAuth decode(String source, Object defaultValue) {
            try {
                String decrypted = EnvCrypto.decrypt(ServerConstants.NAUTH_ENV_SECRET, source);
                if (decrypted == null || decrypted.trim().length() == 0)
                    throw new Exception("BadCrypto 복호화할 수 없습니다.");

                String[] splitted = decrypted.split("\\|");
                if (splitted == null || splitted.length != 2)
                    throw new Exception("BadCrypto 형식이 맞지 않습니다.");

                Class<?> clazz = Class.forName(splitted[0]);
                if (clazz == null)
                    throw new Exception("BadCrypto 지원하지 않는 형식입니다.");

                if (splitted[1] == null)
                    throw new Exception("BadCrypto 복호화할 수 없는 값입니다.");

                if (defaultValue instanceof Integer || clazz == Integer.class) {
                    if (splitted[1].indexOf(".") > -1)
                        splitted[1] = splitted[1].substring(0, splitted[1].indexOf("."));
                    int intValue = Integer.parseInt(splitted[1]);
                    return new NAuth(intValue);
                }

                if (defaultValue instanceof Long || clazz == Long.class) {
                    if (splitted[1].indexOf(".") > -1)
                        splitted[1] = splitted[1].substring(0, splitted[1].indexOf("."));
                    long longValue = Long.parseLong(splitted[1]);
                    return new NAuth(longValue);
                }

                if (clazz == String.class) {
                    return new NAuth(splitted[1]);
                }

                throw new Exception("BadCrypto 지원하지 않는 형식입니다.");
            } catch (Exception e) {
                return new NAuth(defaultValue);
            }
        }

        public static NAuth forcedDecode(String source) {
            try {
                String decrypted = EnvCrypto.decrypt(ServerConstants.NAUTH_ENV_SECRET, source);
                if (decrypted == null || decrypted.trim().length() == 0)
                    throw new Exception("BadCrypto 복호화할 수 없습니다.");

                String[] splitted = decrypted.split("\\|");
                if (splitted == null || splitted.length != 2)
                    throw new Exception("BadCrypto 형식이 맞지 않습니다.");

                Class<?> clazz = Class.forName(splitted[0]);
                if (clazz == null)
                    throw new Exception("BadCrypto 지원하지 않는 형식입니다.");

                if (splitted[1] == null)
                    throw new Exception("BadCrypto 복호화할 수 없는 값입니다.");

                if (clazz == Integer.class) {
                    int intValue = Integer.parseInt(splitted[1]);
                    return new NAuth(intValue);
                }

                if (clazz == Long.class) {
                    long longValue = Long.parseLong(splitted[1]);
                    return new NAuth(longValue);
                }

                if (clazz == String.class) {
                    return new NAuth(splitted[1]);
                }

                throw new Exception("BadCrypto 지원하지 않는 형식입니다.");
            } catch (Exception e) {
                System.out.println("CryptoField_Error : " + e.getMessage());
                return null;
            }
        }
    }

    public static CryptoField decode(String source, Object defaultValue) {
        try {
            String decrypted = EnvCrypto.decrypt(source);
            if (decrypted == null || decrypted.trim().length() == 0)
                throw new Exception("BadCrypto 복호화할 수 없습니다.");

            String[] splitted = decrypted.split("\\|");
            if (splitted == null || splitted.length != 2)
                throw new Exception("BadCrypto 형식이 맞지 않습니다.");

            Class<?> clazz = Class.forName(splitted[0]);
            if (clazz == null)
                throw new Exception("BadCrypto 지원하지 않는 형식입니다.");

            if (splitted[1] == null)
                throw new Exception("BadCrypto 복호화할 수 없는 값입니다.");

            if (defaultValue instanceof Integer || clazz == Integer.class) {
                if (splitted[1].indexOf(".") > -1)
                    splitted[1] = splitted[1].substring(0, splitted[1].indexOf("."));
                int intValue = Integer.parseInt(splitted[1]);
                return new CryptoField(intValue);
            }

            if (defaultValue instanceof Long || clazz == Long.class) {
                if (splitted[1].indexOf(".") > -1)
                    splitted[1] = splitted[1].substring(0, splitted[1].indexOf("."));
                long longValue = Long.parseLong(splitted[1]);
                return new CryptoField(longValue);
            }

            if (clazz == String.class) {
                return new CryptoField(splitted[1]);
            }

            throw new Exception("BadCrypto 지원하지 않는 형식입니다.");
        } catch (Exception e) {
            return new CryptoField(defaultValue);
        }
    }

    public static CryptoField forcedDecode(String source) {
        try {
            String decrypted = EnvCrypto.decrypt(source);
            if (decrypted == null || decrypted.trim().length() == 0)
                throw new Exception("BadCrypto 복호화할 수 없습니다.");

            String[] splitted = decrypted.split("\\|");
            if (splitted == null || splitted.length != 2)
                throw new Exception("BadCrypto 형식이 맞지 않습니다.");

            Class<?> clazz = Class.forName(splitted[0]);
            if (clazz == null)
                throw new Exception("BadCrypto 지원하지 않는 형식입니다.");

            if (splitted[1] == null)
                throw new Exception("BadCrypto 복호화할 수 없는 값입니다.");

            if (clazz == Integer.class) {
                int intValue = Integer.parseInt(splitted[1]);
                return new CryptoField(intValue);
            }

            if (clazz == Long.class) {
                long longValue = Long.parseLong(splitted[1]);
                return new CryptoField(longValue);
            }

            if (clazz == String.class) {
                return new CryptoField(splitted[1]);
            }

            throw new Exception("BadCrypto 지원하지 않는 형식입니다.");
        } catch (Exception e) {
            System.out.println("CryptoField_Error : " + e.getMessage());
            return null;
        }
    }

    protected void encode(Object source) {
        if (source == null) {
            encoded = "";
        } else {
            try {
                encoded = EnvCrypto.encrypt(originType.getName() + "|" + origin.toString());
            } catch (Exception e) {
                encoded = "err_crypto";
            }
        }
    }

    public static void main(String[] args) {
        System.out.println((Long) decode("740b9a23300faec5e43d2ecf1af41e18d36252481a393af5e7ebb39bf7f1f0a1", 0L).originOf());
    }
}
