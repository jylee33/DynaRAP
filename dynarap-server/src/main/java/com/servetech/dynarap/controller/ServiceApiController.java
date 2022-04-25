package com.servetech.dynarap.controller;

import com.google.gson.JsonObject;
import com.servetech.dynarap.db.mapper.DirMapper;
import com.servetech.dynarap.db.service.DirService;
import com.servetech.dynarap.db.service.UserService;
import com.servetech.dynarap.ext.HandledServiceException;
import com.servetech.dynarap.ext.ResponseHelper;
import com.servetech.dynarap.vo.DirVO;
import com.servetech.dynarap.vo.UserVO;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.boot.web.servlet.error.ErrorController;
import org.springframework.http.HttpStatus;
import org.springframework.security.core.Authentication;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.servlet.ModelAndView;

import javax.servlet.RequestDispatcher;
import javax.servlet.http.HttpServletRequest;

@Controller
@RequestMapping(value = "/api/{serviceVersion}")
public class ServiceApiController extends ApiController {
    @Value("${neoulsoft.auth.client-id}")
    private String authClientId;

    @Value("${neoulsoft.auth.client-secret}")
    private String authClientSecret;

    @RequestMapping(value = "/dir")
    @ResponseBody
    public Object apiDir(HttpServletRequest request, @PathVariable String serviceVersion,
                         @RequestBody JsonObject payload, Authentication authentication) throws HandledServiceException {
        /*
        String accessToken = request.getHeader("Authorization");
        if (accessToken == null || (!accessToken.startsWith("bearer") && !accessToken.startsWith("Bearer")))
            return ResponseHelper.error(403, "권한이 없습니다.");

        String username = authentication.getPrincipal().toString();
        */
        UserVO user = getService(UserService.class).getUser("admin@dynarap@dynarap");

        if (checkJsonEmpty(payload, "command"))
            throw new HandledServiceException(404, "파라미터를 확인하세요.");

        String command = payload.get("command").getAsString();

        if (command.equals("list")) {
            JsonObject result = getService(DirService.class).getDirList(user.getUid());
            return ResponseHelper.response(200, "Success - Dir List", result);
        }

        if (command.equals("add")) {
            DirVO added = getService(DirService.class).insertDir(user.getUid(), payload);
            return ResponseHelper.response(200, "Success - Dir Added", added);
        }

        if (command.equals("modify")) {
            DirVO updated = getService(DirService.class).updateDir(user.getUid(), payload);
            return ResponseHelper.response(200, "Success - Dir Updated", updated);
        }

        if (command.equals("remove")) {
            getService(DirService.class).deleteDir(user.getUid(), payload);
            return ResponseHelper.response(200, "Success - Dir Deleted", "");
        }

        throw new HandledServiceException(411, "명령이 정의되지 않았습니다.");
    }
}
