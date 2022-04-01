package com.servetech.dynarap.security;

import java.util.regex.Pattern;

public enum PasswordPolicy {
    // 최소6자 문자 + 숫자.
    PWD_TYPE_6_AN("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{6,}$"),
    // 최소6자 문자 + 숫자 + 특문
    PWD_TYPE_6_ANS("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[$@$!%*#?&])[A-Za-z\\d$@$!%*#?&]{6,}$"),
    // 최소6자 대문자 + 소문자 + 숫자
    PWD_TYPE_6_CN("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{6,}$"),
    // 최소6자 대문자 + 소문자 + 숫자 + 특문
    PWD_TYPE_6_CNS("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[$@$!%*?&])[A-Za-z\\d$@$!%*?&]{6,}"),
    // 최소6자, 최대10자, 대문자 + 소문자 + 숫자 + 특문.
    PWD_TYPE_6_10_CNS("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[$@$!%*?&])[A-Za-z\\d$@$!%*?&]{6,10}");

    private String regExp;

    PasswordPolicy(String regExp) {
        this.regExp = regExp;
    }

    public String getRegExp() {
        return this.regExp;
    }

    public Pattern getRegExpPattern() {
        return Pattern.compile(this.regExp);
    }
}
