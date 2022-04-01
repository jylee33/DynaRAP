package com.servetech.dynarap.vo;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.db.type.String64;
import com.servetech.dynarap.db.type.UserType;
import lombok.Data;
import lombok.EqualsAndHashCode;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.web.socket.WebSocketSession;

import java.util.Arrays;
import java.util.Collection;
import java.util.Map;

@Data
@EqualsAndHashCode(callSuper = false)
public class UserVO extends NAuthUserVO {
    private UserType userType;
    private LongDate joinedAt;
    private LongDate leftAt;
    private String email;
    private String profileUrl;
    private String phoneNumber;
    private transient LongDate privacyTermsReadAt;
    private transient LongDate serviceTermsReadAt;
    private transient String pushToken;
    private boolean usePush;
    private String64 nickname;
    private transient String tempPassword;
    private transient LongDate tempPasswordExpire;
    private String64 promotion;

    @Override
    public Collection<? extends GrantedAuthority> getAuthorities() {
        SimpleGrantedAuthority sga = new SimpleGrantedAuthority(getRole());
        return Arrays.asList(sga);
    }

    @Data
    public static class Device {
        private CryptoField seq;
        private CryptoField.NAuth uid;
        private String adid;
        private LongDate lastConnectedAt;
        private String pushToken;
    }

    @Data
    public static class Simple {
        private CryptoField.NAuth uid;
        private String64 displayName;
        private String email;
        private String phoneNumber;
        private String displayUrl;
        private String64 promotion;

        private transient Map<String, WebSocketSession> channelSession;
    }

    public Simple getSimple() {
        Simple simple = new Simple();
        simple.setUid(uid);
        if (nickname != null && !nickname.isEmpty())
            simple.setDisplayName(nickname);
        else
            simple.setDisplayName(accountName);
        simple.setPhoneNumber(phoneNumber);
        simple.setEmail(email);
        simple.setDisplayUrl(profileUrl);
        simple.setPromotion(promotion);
        return simple;
    }

}
