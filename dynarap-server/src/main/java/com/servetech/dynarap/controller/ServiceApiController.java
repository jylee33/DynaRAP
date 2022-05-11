package com.servetech.dynarap.controller;

import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.servetech.dynarap.db.mapper.DirMapper;
import com.servetech.dynarap.db.service.*;
import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.String64;
import com.servetech.dynarap.ext.HandledServiceException;
import com.servetech.dynarap.ext.ResponseHelper;
import com.servetech.dynarap.vo.*;
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
import java.util.ArrayList;
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

    @RequestMapping(value = "/flight")
    @ResponseBody
    public Object apiFlight(HttpServletRequest request, @PathVariable String serviceVersion,
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
            List<FlightVO> flights = getService(FlightService.class).getFlightList();
            return ResponseHelper.response(200, "Success - Flight List", flights);
        }

        if (command.equals("add")) {
            FlightVO flight = getService(FlightService.class).insertFlight(user.getUid(), payload);
            return ResponseHelper.response(200, "Success - Flight Added", flight);
        }

        if (command.equals("modify")) {
            FlightVO flight = getService(FlightService.class).updateFlight(user.getUid(), payload);
            return ResponseHelper.response(200, "Success - Flight Updated", flight);
        }

        if (command.equals("remove")) {
            getService(FlightService.class).deleteFlight(payload);
            return ResponseHelper.response(200, "Success - Flight Deleted", "");
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

        if (command.equals("add-bulk")) {
            JsonArray jarrBulk = payload.get("params").getAsJsonArray();
            List<ParamVO> params = new ArrayList<>();
            long msec = System.currentTimeMillis();
            for (int i = 0; i < jarrBulk.size(); i++) {
                JsonObject jobjParam = jarrBulk.get(i).getAsJsonObject();
                jobjParam.addProperty("paramName", new String64("param_" + (msec + i)).valueOf());
                ParamVO param = getService(ParamService.class).insertParam(user.getUid(), jobjParam);
                params.add(param);
            }
            return ResponseHelper.response(200, "Success - Param Added (Bulk) ", params);
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

        if (command.equals("param-add-bulk")) {
            JsonArray jarrBulk = payload.get("params").getAsJsonArray();

            for (int i = 0; i < jarrBulk.size(); i++) {
                JsonObject jobjParam = jarrBulk.get(i).getAsJsonObject();
                getService(ParamService.class).insertPresetParam(jobjParam);
            }
            return ResponseHelper.response(200, "Success - Preset Param Add Bulk", "");
        }

        if (command.equals("param-remove")) {
            getService(ParamService.class).deletePresetParam(payload);
            return ResponseHelper.response(200, "Success - Preset Param Remove", "");
        }

        throw new HandledServiceException(411, "명령이 정의되지 않았습니다.");
    }

    @RequestMapping(value = "/dll")
    @ResponseBody
    public Object apiDLL(HttpServletRequest request, @PathVariable String serviceVersion,
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
            List<DLLVO> dlls = getService(DLLService.class).getDLLList();
            return ResponseHelper.response(200, "Success - DLL List", dlls);
        }

        if (command.equals("add")) {
            DLLVO dll = getService(DLLService.class).insertDLL(user.getUid(), payload);
            return ResponseHelper.response(200, "Success - DLL Added", dll);
        }

        if (command.equals("modify")) {
            DLLVO dll = getService(DLLService.class).updateDLL(user.getUid(), payload);
            return ResponseHelper.response(200, "Success - DLL Updated", dll);
        }

        if (command.equals("remove")) {
            getService(DLLService.class).deleteDLL(payload);
            return ResponseHelper.response(200, "Success - DLL Deleted", "");
        }

        if (command.equals("param-list")) {
            CryptoField dllSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "dllSeq"))
                dllSeq = CryptoField.decode(payload.get("dllSeq").getAsString(), 0L);

            if (dllSeq == null || dllSeq.isEmpty())
                throw new HandledServiceException(411, "요청 파라미터 오류입니다. [필수 파라미터 누락]");

            List<DLLVO.Param> paramList = getService(DLLService.class).getDLLParamList(dllSeq);
            return ResponseHelper.response(200, "Success - DLL Param List", paramList);
        }

        if (command.equals("param-add")) {
            DLLVO.Param dllParam = getService(DLLService.class).insertDLLParam(user.getUid(), payload);
            return ResponseHelper.response(200, "Success - DLL Param Add", dllParam);
        }

        if (command.equals("param-modify")) {
            DLLVO.Param dllParam = getService(DLLService.class).updateDLLParam(user.getUid(), payload);
            return ResponseHelper.response(200, "Success - DLL Param Modify", dllParam);
        }

        if (command.equals("param-remove")) {
            getService(DLLService.class).deleteDLLParam(payload);
            return ResponseHelper.response(200, "Success - DLL Param Remove", "");
        }

        if (command.equals("param-remove-multi")) {
            getService(DLLService.class).deleteDLLParamByMulti(payload);
            return ResponseHelper.response(200, "Success - DLL Param All Remove", "");
        }

        if (command.equals("data-list")) {
            CryptoField dllSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "dllSeq"))
                dllSeq = CryptoField.decode(payload.get("dllSeq").getAsString(), 0L);

            if (dllSeq == null || dllSeq.isEmpty())
                throw new HandledServiceException(411, "요청 파라미터 오류입니다. [필수 파라미터 누락]");

            CryptoField dllParamSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "dllParamSeq"))
                dllParamSeq = CryptoField.decode(payload.get("dllParamSeq").getAsString(), 0L);

            List<DLLVO.Raw> rawData = getService(DLLService.class).getDLLData(dllSeq, dllParamSeq);
            return ResponseHelper.response(200, "Success - DLL Data List", rawData);
        }

        if (command.equals("data-add")) {
            DLLVO.Raw dllRaw = getService(DLLService.class).insertDLLData(user.getUid(), payload);
            return ResponseHelper.response(200, "Success - DLL Data Add", dllRaw);
        }

        if (command.equals("data-modify")) {
            DLLVO.Raw dllRaw = getService(DLLService.class).updateDLLData(user.getUid(), payload);
            return ResponseHelper.response(200, "Success - DLL Data Modify", dllRaw);
        }

        if (command.equals("data-remove-row")) {
            getService(DLLService.class).deleteDLLDataByRow(payload);
            return ResponseHelper.response(200, "Success - DLL Data Remove By Row", "");
        }

        if (command.equals("data-remove-param")) {
            getService(DLLService.class).deleteDLLDataByParam(payload);
            return ResponseHelper.response(200, "Success - DLL Data Remove By Param or All", "");
        }

        throw new HandledServiceException(411, "명령이 정의되지 않았습니다.");
    }

    @RequestMapping(value = "/raw")
    @ResponseBody
    public Object apiRaw(HttpServletRequest request, @PathVariable String serviceVersion,
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

        if (command.equals("import")) {
            // need - uploadSeq, presetPack, presetSeq, flightSeq, flightAt
            CryptoField uploadSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "uploadSeq"))
                uploadSeq = CryptoField.decode(payload.get("uploadSeq").getAsString(), 0L);

            CryptoField presetPack = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "presetPack"))
                presetPack = CryptoField.decode(payload.get("presetPack").getAsString(), 0L);

            CryptoField presetSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "presetSeq"))
                presetSeq = CryptoField.decode(payload.get("presetSeq").getAsString(), 0L);

            CryptoField flightSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "flightSeq"))
                flightSeq = CryptoField.decode(payload.get("flightSeq").getAsString(), 0L);

            String flightAt = "";
            if (!checkJsonEmpty(payload, "flightAt"))
                flightAt = payload.get("flightAt").getAsString();

            String dataType = "";
            if (!checkJsonEmpty(payload, "dataType"))
                dataType = payload.get("dataType").getAsString();

            if (uploadSeq == null || uploadSeq.isEmpty() || presetPack == null || presetPack.isEmpty() || dataType == null || dataType.isEmpty())
                throw new HandledServiceException(404, "필요 파라미터가 누락됐습니다.");

            JsonObject jobjResult = getService(RawService.class).runImport(user.getUid(), uploadSeq, presetPack, presetSeq, flightSeq, flightAt, dataType);

            return ResponseHelper.response(200, "Success - Import request done", jobjResult);
        }

        if (command.equals("create-cache")) {
            // need - uploadSeq, presetPack, presetSeq, flightSeq, flightAt
            CryptoField uploadSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "uploadSeq"))
                uploadSeq = CryptoField.decode(payload.get("uploadSeq").getAsString(), 0L);

            if (uploadSeq == null || uploadSeq.isEmpty())
                throw new HandledServiceException(404, "필요 파라미터가 누락됐습니다.");

            JsonObject jobjResult = getService(RawService.class).createCache(user.getUid(), uploadSeq);

            return ResponseHelper.response(200, "Success - Create cache done", jobjResult);
        }

        if (command.equals("check-done")) {
            CryptoField uploadSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "uploadSeq"))
                uploadSeq = CryptoField.decode(payload.get("uploadSeq").getAsString(), 0L);

            RawVO.Upload rawUpload = getService(RawService.class).getUploadBySeq(uploadSeq);
            return ResponseHelper.response(200, "Success - Check Import Done", rawUpload.isImportDone());
        }

        // 업로드 리스트 가져가기
        if (command.equals("upload-list")) {
            List<RawVO.Upload> rawUploadList = getService(RawService.class).getUploadList();
            return ResponseHelper.response(200, "Success - Upload list", rawUploadList);
        }

        // 파트 쪼개는 작업 진행해야함.
        if (command.equals("create-part")) {
            List<PartVO> partList = getService(PartService.class).createPartList(user.getUid(), payload);
            return ResponseHelper.response(200, "Success - Create raw data part", partList);
        }

        throw new HandledServiceException(411, "명령이 정의되지 않았습니다.");
    }

}
