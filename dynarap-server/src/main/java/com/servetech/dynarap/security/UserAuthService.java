package com.servetech.dynarap.security;

import com.servetech.dynarap.db.service.UserService;
import com.servetech.dynarap.vo.UserVO;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.core.userdetails.User;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.stereotype.Service;

@Service
public class UserAuthService implements UserDetailsService {

    @Autowired
    private UserService userService;

    @Override
    public UserDetails loadUserByUsername(String username) throws UsernameNotFoundException {
        UserVO user = null;
        try {
            if (!username.contains("@")) {
                // called by refresh
                Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
                username = username + "@" + ((User) authentication.getPrincipal()).getUsername();
            }
            user = userService.getUser(username);
        } catch (Exception e) {
            e.printStackTrace();
        }
        if (user == null) {
            throw new UsernameNotFoundException("UserNotFound [" + username + "]");
        }
        return user;
    }
}
