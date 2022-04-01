package com.servetech.dynarap.security;

import com.auth0.jwt.JWT;
import com.auth0.jwt.JWTVerifier;
import com.auth0.jwt.algorithms.Algorithm;
import com.auth0.jwt.exceptions.JWTVerificationException;
import com.auth0.jwt.exceptions.TokenExpiredException;
import com.auth0.jwt.interfaces.DecodedJWT;
import com.servetech.dynarap.db.type.UserType;
import com.servetech.dynarap.ext.HandledServiceException;
import org.springframework.core.io.ClassPathResource;
import org.springframework.security.oauth2.provider.token.store.KeyStoreKeyFactory;

import java.security.KeyPair;
import java.security.interfaces.RSAPrivateKey;
import java.security.interfaces.RSAPublicKey;

public class JwtTokenHelper {
    private static KeyPair keyPair = new KeyStoreKeyFactory(new ClassPathResource("neoulsoft.com.jks"),
            "2019neoulsoft!#A".toCharArray()).getKeyPair("Neoulsoft",
            "2019neoulsoft!#A".toCharArray());
    private static JWTVerifier verifier = JWT.require(Algorithm.RSA256((RSAPublicKey) keyPair.getPublic(),
            (RSAPrivateKey) keyPair.getPrivate())).acceptExpiresAt(0).build();

    public static DecodedJWT decodeAccessToken(String accessToken) throws HandledServiceException {
        DecodedJWT jwt = null;
        try {
            jwt = JWT.decode(accessToken);
            if (jwt.getClaim("userType") != null
                    && !jwt.getClaim("userType").isNull()
                    && jwt.getClaim("userType").asString().equals(UserType.GUEST.toString())) {
                return jwt;
            }

            jwt = verifier.verify(accessToken);
        } catch (TokenExpiredException tee) {
            throw new HandledServiceException(401, "Invalid accessToken. accessToken expired.");
        } catch (JWTVerificationException verificationEx) {
            throw new HandledServiceException(401, verificationEx.getMessage());
        }
        return jwt;
    }
}
