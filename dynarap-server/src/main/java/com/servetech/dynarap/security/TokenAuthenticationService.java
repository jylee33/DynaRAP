package com.servetech.dynarap.security;

import com.auth0.jwt.interfaces.DecodedJWT;
import com.servetech.dynarap.db.service.UserService;
import com.servetech.dynarap.db.type.UserType;
import com.servetech.dynarap.ext.HandledServiceException;
import com.servetech.dynarap.vo.UserVO;
import lombok.RequiredArgsConstructor;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.stereotype.Component;

import javax.servlet.http.HttpServletRequest;
import java.util.ArrayList;
import java.util.List;
import java.util.Locale;

@Component
@RequiredArgsConstructor
public class TokenAuthenticationService {
    private static Logger logger = LoggerFactory.getLogger(TokenAuthenticationService.class);

    static final long EXPIRATIONTIME = 3600 * 1000;
    static final String TOKEN_PREFIX = "Bearer";
    static final String HEADER_STRING = "Authorization";

    private final UserService userService;

    public Authentication getAuthentication(HttpServletRequest request)
            throws HandledServiceException {
        String token = request.getHeader(HEADER_STRING);
        if (token != null) {
            if (token.startsWith(TOKEN_PREFIX) || token.startsWith(TOKEN_PREFIX.toLowerCase(Locale.ROOT)))
                token = token.substring(TOKEN_PREFIX.length() + 1).trim();

            DecodedJWT jwt = JwtTokenHelper.decodeAccessToken(token);
            UserVO user = userService.getUser(
                    jwt.getClaim("user_name").asString() + "@" + jwt.getClaim("client_id").asString());
            List<SimpleGrantedAuthority> g_auths = new ArrayList<SimpleGrantedAuthority>();
            if (user != null) {
                g_auths.add(new SimpleGrantedAuthority(user.getUserType().toString()));
            }
            else {
                g_auths.add(new SimpleGrantedAuthority(UserType.GUEST.toString()));
            }

            UsernamePasswordAuthenticationToken auth = jwt.getClaim("user_name") != null
                    ? new UsernamePasswordAuthenticationToken(jwt.getClaim("user_name").asString(),
                    null, g_auths)
                    : null;

            return auth;
        }
        return null;
    }
}
