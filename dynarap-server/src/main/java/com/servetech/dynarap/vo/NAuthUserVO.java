package com.servetech.dynarap.vo;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.db.type.String64;
import com.servetech.dynarap.db.type.UserType;
import lombok.AccessLevel;
import lombok.Data;
import lombok.Getter;
import lombok.Setter;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.core.userdetails.UserDetails;

import java.util.Arrays;
import java.util.Collection;

@Data
public class NAuthUserVO implements UserDetails {
    protected CryptoField.NAuth uid;
    protected String clientId;
    protected String username;
    protected transient String password;

    @Getter(AccessLevel.NONE)
    @Setter(AccessLevel.NONE)
    private transient UserType.NAuth userType;

    protected String provider;
    protected boolean accountLocked;
    protected String64 accountName;

    protected transient AuthProvider authProvider;

    public String getRole() {
        return userType.toString();
    }

    public UserType.NAuth getNAuthUserType() {
        return userType;
    }

    public void setNAuthUserType(UserType.NAuth userType) {
        this.userType = userType;
    }

    @Override
    public Collection<? extends GrantedAuthority> getAuthorities() {
        SimpleGrantedAuthority sga = new SimpleGrantedAuthority(getRole());
        return Arrays.asList(sga);
    }

    @Override
    public boolean isAccountNonExpired() {
        return true;
    }

    @Override
    public boolean isAccountNonLocked() {
        return !accountLocked;
    }

    @Override
    public boolean isCredentialsNonExpired() {
        return true;
    }

    @Override
    public boolean isEnabled() {
        return true;
    }

    @Data
    public static class AuthProvider {
        private CryptoField.NAuth seq;
        private String clientId;
        private CryptoField.NAuth authUid;
        private String provider;
        private String providerPayload;
        private LongDate authorizedAt;
        private String providerId;
    }

    @Data
    public static class Login {
        private String username;
        private String password;
        private String provider;
        private String appVersion;
        private String providerPayload;
    }
}
