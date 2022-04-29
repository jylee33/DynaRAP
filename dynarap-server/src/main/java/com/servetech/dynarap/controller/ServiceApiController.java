package com.servetech.dynarap.controller;

import com.google.gson.JsonObject;
import com.servetech.dynarap.db.mapper.DirMapper;
import com.servetech.dynarap.db.service.DirService;
import com.servetech.dynarap.db.service.ParamService;
import com.servetech.dynarap.db.service.UserService;
import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.ext.HandledServiceException;
import com.servetech.dynarap.ext.ResponseHelper;
import com.servetech.dynarap.vo.DirVO;
import com.servetech.dynarap.vo.ParamVO;
import com.servetech.dynarap.vo.PresetVO;
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
import java.util.List;

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

    @RequestMapping(value = "/param")
    @ResponseBody
    public Object apiParam(HttpServletRequest request, @PathVariable String serviceVersion,
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
            Integer pageNo = 1;
            if (!checkJsonEmpty(payload, "pageNo"))
                pageNo = payload.get("pageNo").getAsInt();

            Integer pageSize = 15;
            if (!checkJsonEmpty(payload, "pageSize"))
                pageSize = payload.get("pageSize").getAsInt();

            List<ParamVO> params = getService(ParamService.class).getParamList(pageNo, pageSize);
            int paramCount = getService(ParamService.class).getParamCount();

            return ResponseHelper.response(200, "Success - Param List", paramCount, params);
        }

        if (command.equals("info")) {
            CryptoField paramSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "seq"))
                paramSeq = CryptoField.decode(payload.get("seq").getAsString(), 0L);

            CryptoField paramPack = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "paramPack"))
                paramPack = CryptoField.decode(payload.get("paramPack").getAsString(), 0L);

            if (paramPack == null || paramPack.isEmpty())
                throw new HandledServiceException(404, "파라미터를 확인하세요.");

            ParamVO param = null;
            if (paramSeq == null || paramSeq.isEmpty())
                param = getService(ParamService.class).getActiveParam(paramPack);
            else
                param = getService(ParamService.class).getParamBySeq(paramSeq);

            return ResponseHelper.response(200, "Success - Param Info", param);
        }

        if (command.equals("add")) {
            ParamVO param = getService(ParamService.class).insertParam(user.getUid(), payload);
            return ResponseHelper.response(200, "Success - Param Added", param);
        }

        if (command.equals("modify")) {
            ParamVO param = getService(ParamService.class).updateParam(user.getUid(), payload);
            return ResponseHelper.response(200, "Success - Param Updated", param);
        }

        if (command.equals("remove")) {
            getService(ParamService.class).deleteParam(payload);
            return ResponseHelper.response(200, "Success - Param Deleted", "");
        }

        if (command.equals("group-list")) {
            List<ParamVO.Group> paramGroups = getService(ParamService.class).getParamGroupList();
            return ResponseHelper.response(200, "Success - Param Group List", paramGroups);
        }

        if (command.equals("group-add")) {
            ParamVO.Group paramGroup = getService(ParamService.class).insertParamGroup(user.getUid(), payload);
            return ResponseHelper.response(200, "Success - Param Group Add", paramGroup);
        }

        if (command.equals("group-modify")) {
            ParamVO.Group paramGroup = getService(ParamService.class).updateParamGroup(user.getUid(), payload);
            return ResponseHelper.response(200, "Success - Param Group Modify", paramGroup);
        }

        throw new HandledServiceException(411, "명령이 정의되지 않았습니다.");
    }

    @RequestMapping(value = "/preset")
    @ResponseBody
    public Object apiPreset(HttpServletRequest request, @PathVariable String serviceVersion,
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
            Integer pageNo = 1;
            if (!checkJsonEmpty(payload, "pageNo"))
                pageNo = payload.get("pageNo").getAsInt();

            Integer pageSize = 15;
            if (!checkJsonEmpty(payload, "pageSize"))
                pageSize = payload.get("pageSize").getAsInt();

            List<PresetVO> presets = getService(ParamService.class).getPresetList(pageNo, pageSize);
            int presetCount = getService(ParamService.class).getPresetCount();

            return ResponseHelper.response(200, "Success - Preset List", presetCount, presets);
        }

        if (command.equals("info")) {
            CryptoField presetSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "seq"))
                presetSeq = CryptoField.decode(payload.get("seq").getAsString(), 0L);

            CryptoField presetPack = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "presetPack"))
                presetPack = CryptoField.decode(payload.get("presetPack").getAsString(), 0L);

            if (presetPack == null || presetPack.isEmpty())
                throw new HandledServiceException(404, "파라미터를 확인하세요.");

            PresetVO preset = null;
            if (presetSeq == null || presetSeq.isEmpty())
                preset = getService(ParamService.class).getActivePreset(presetPack);
            else
                preset = getService(ParamService.class).getPresetBySeq(presetSeq);

            return ResponseHelper.response(200, "Success - Preset Info", preset);
        }

        if (command.equals("add")) {
            PresetVO preset = getService(ParamService.class).insertPreset(user.getUid(), payload);
            return ResponseHelper.response(200, "Success - Preset Added", preset);
        }

        if (command.equals("modify")) {
            PresetVO preset = getService(ParamService.class).updatePreset(user.getUid(), payload);
            return ResponseHelper.response(200, "Success - Preset Updated", preset);
        }

        if (command.equals("remove")) {
            getService(ParamService.class).deletePreset(payload);
            return ResponseHelper.response(200, "Success - Preset Deleted", "");
        }

        if (command.equals("param-list")) {
            CryptoField presetPack = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "presetPack"))
                presetPack = CryptoField.decode(payload.get("presetPack").getAsString(), 0L);

            CryptoField presetSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "presetSeq"))
                presetSeq = CryptoField.decode(payload.get("presetSeq").getAsString(), 0L);

            CryptoField paramPack = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "paramPack"))
                paramPack = CryptoField.decode(payload.get("paramPack").getAsString(), 0L);

            CryptoField paramSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "paramSeq"))
                paramSeq = CryptoField.decode(payload.get("paramSeq").getAsString(), 0L);

            Integer pageNo = 1;
            if (!checkJsonEmpty(payload, "pageNo"))
                pageNo = payload.get("pageNo").getAsInt();

            Integer pageSize = 15;
            if (!checkJsonEmpty(payload, "pageSize"))
                pageSize = payload.get("pageSize").getAsInt();

            List<ParamVO> paramList = getService(ParamService.class).getPresetParamList(
                    presetPack, presetSeq, paramPack, paramSeq, pageNo, pageSize);
            int paramCount = getService(ParamService.class).getPresetParamCount(
                    presetPack, presetSeq, paramPack, paramSeq);

            return ResponseHelper.response(200, "Success - Preset Param List", paramCount, paramList);
        }

        if (command.equals("param-add")) {
            getService(ParamService.class).insertPresetParam(payload);
            return ResponseHelper.response(200, "Success - Preset Param Add", "");
        }

        if (command.equals("param-remove")) {
            getService(ParamService.class).deletePresetParam(payload);
            return ResponseHelper.response(200, "Success - Preset Param Remove", "");
        }

        throw new HandledServiceException(411, "명령이 정의되지 않았습니다.");
    }
}
