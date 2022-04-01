package com.servetech.dynarap.db.type;

public enum UserType {
    SA(0, "superadmin"),
    ADMIN(1, "admin"),
    SERVICE_ADMIN(3, "serviceadmin"),
    USER(8, "user"),
    GUEST(9, "guest");

    private int userType;
    private String typeName;

    UserType(int userType, String typeName) {
        this.userType = userType;
        this.typeName = typeName;
    }

    public int getUserType() {
        return userType;
    }

    @Override
    public String toString() {
        return typeName;
    }

    public static UserType parse(String type) {
        if (type.equals(SA.toString()))
            return SA;
        if (type.equals(ADMIN.toString()))
            return ADMIN;
        if (type.equals(SERVICE_ADMIN.toString()))
            return SERVICE_ADMIN;
        if (type.equals(USER.toString()))
            return USER;
        return GUEST;
    }

    public static UserType parse(Integer type) {
        if (type == SA.getUserType())
            return SA;
        if (type == ADMIN.getUserType())
            return ADMIN;
        if (type == SERVICE_ADMIN.getUserType())
            return SERVICE_ADMIN;
        if (type == USER.getUserType())
            return USER;
        return GUEST;
    }

    public enum NAuth {
        SA(0, "superadmin"),
        ADMIN(1, "admin"),

        USER_LEVEL_2(2, "userlevel2"),
        USER_LEVEL_3(3, "userlevel3"),
        USER_LEVEL_4(4, "userlevel4"),
        USER_LEVEL_5(5, "userlevel5"),
        USER_LEVEL_6(6, "userlevel6"),
        USER_LEVEL_7(7, "userlevel7"),

        USER(8, "user"),
        GUEST(9, "guest");

        private int userType;
        private String typeName;

        NAuth(int userType, String typeName) {
            this.userType = userType;
            this.typeName = typeName;
        }

        public int getUserType() {
            return userType;
        }

        @Override
        public String toString() {
            return typeName;
        }

        public static NAuth parse(String type) {
            if (type.equals(SA.toString()))
                return SA;
            if (type.equals(ADMIN.toString()))
                return ADMIN;
            if (type.equals(USER_LEVEL_2.toString()))
                return USER_LEVEL_2;
            if (type.equals(USER_LEVEL_3.toString()))
                return USER_LEVEL_3;
            if (type.equals(USER_LEVEL_4.toString()))
                return USER_LEVEL_4;
            if (type.equals(USER_LEVEL_5.toString()))
                return USER_LEVEL_5;
            if (type.equals(USER_LEVEL_6.toString()))
                return USER_LEVEL_6;
            if (type.equals(USER_LEVEL_7.toString()))
                return USER_LEVEL_7;
            if (type.equals(USER.toString()))
                return USER;
            return GUEST;
        }

        public static NAuth parse(Integer type) {
            if (type == SA.getUserType())
                return SA;
            if (type == ADMIN.getUserType())
                return ADMIN;
            if (type == USER_LEVEL_2.getUserType())
                return USER_LEVEL_2;
            if (type == USER_LEVEL_3.getUserType())
                return USER_LEVEL_3;
            if (type == USER_LEVEL_4.getUserType())
                return USER_LEVEL_4;
            if (type == USER_LEVEL_5.getUserType())
                return USER_LEVEL_5;
            if (type == USER_LEVEL_6.getUserType())
                return USER_LEVEL_6;
            if (type == USER_LEVEL_7.getUserType())
                return USER_LEVEL_7;
            if (type == USER.getUserType())
                return USER;
            return GUEST;
        }
    }
}
