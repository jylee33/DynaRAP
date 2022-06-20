package com.servetech.dynarap.controller;

import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.google.gson.JsonPrimitive;
import com.servetech.dynarap.config.ServerConstants;
import com.servetech.dynarap.db.service.*;
import com.servetech.dynarap.db.service.task.PartImportTask;
import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.String64;
import com.servetech.dynarap.ext.HandledServiceException;
import com.servetech.dynarap.ext.ResponseHelper;
import com.servetech.dynarap.vo.*;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.security.core.Authentication;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseBody;

import javax.servlet.http.HttpServletRequest;
import java.io.*;
import java.util.*;

@Controller
@RequestMapping(value = "/api/{serviceVersion}")
public class ServiceApiController extends ApiController {
    @Value("${neoulsoft.auth.client-id}")
    private String authClientId;

    @Value("${neoulsoft.auth.client-secret}")
    private String authClientSecret;

    @Value("${dynarap.process.path}")
    private String processPath;

    @Value("${static.resource.location}")
    private String staticLocation;

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

            String keyword = "";
            if (!checkJsonEmpty(payload, "keyword"))
                keyword = payload.get("keyword").getAsString();

            String resultDataType = "array";
            if (!checkJsonEmpty(payload, "resultDataType"))
                resultDataType = payload.get("resultDataType").getAsString();

            List<ParamVO> params = getService(ParamService.class).getParamList(keyword, pageNo, pageSize);
            int paramCount = getService(ParamService.class).getParamCount(keyword);

            Map<String, ParamVO> resultMap = new LinkedHashMap<>();
            if (resultDataType.equalsIgnoreCase("map")) {
                for (ParamVO p : params) {
                    resultMap.put(p.getParamKey(), p);
                }
                return ResponseHelper.response(200, "Success - Param List", paramCount, resultMap);
            }

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

        if (command.equals("prop-list")) {
            String propType = null;
            if (!checkJsonEmpty(payload, "propType"))
                propType = payload.get("propType").getAsString();

            List<ParamVO.Prop> paramProps = getService(ParamService.class).getParamPropList(propType);
            return ResponseHelper.response(200, "Success - Param Prop List", paramProps);
        }

        if (command.equals("prop-add")) {
            ParamVO.Prop paramProp = getService(ParamService.class).insertParamProp(user.getUid(), payload);
            return ResponseHelper.response(200, "Success - Param Prop Add", paramProp);
        }

        if (command.equals("prop-modify")) {
            ParamVO.Prop paramProp = getService(ParamService.class).updateParamProp(user.getUid(), payload);
            return ResponseHelper.response(200, "Success - Param Prop Modify", paramProp);
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

            String resultDataType = "map";
            if (!checkJsonEmpty(payload, "resultDataType"))
                resultDataType = payload.get("resultDataType").getAsString();

            // paramSeq array
            Map<String, DLLVO.Param> paramMap = new HashMap<>();
            List<DLLVO.Param> dllParams = getService(DLLService.class).getDLLParamList(dllSeq);
            if (dllParams == null) dllParams = new ArrayList<>();
            for (DLLVO.Param dp : dllParams)
                paramMap.put(dp.getSeq().valueOf(), dp);

            JsonArray jarrParams = null;
            if (!checkJsonEmpty(payload, "dllParamSet"))
                jarrParams = payload.get("dllParamSet").getAsJsonArray();

            if (jarrParams == null || jarrParams.size() == 0) {
                jarrParams = new JsonArray();
                for (DLLVO.Param dp : dllParams)
                    jarrParams.add(dp.getSeq().valueOf());
            }

            JsonArray jarrRows = null;
            if (!checkJsonEmpty(payload, "dllRowRange"))
                jarrRows = payload.get("dllRowRange").getAsJsonArray();

            if (jarrRows == null || jarrRows.size() == 0) {
                jarrRows = new JsonArray();
                jarrRows.add(0);
                jarrRows.add(Integer.MAX_VALUE);
            }
            else if (jarrRows.size() < 2) {
                jarrRows.add(Integer.MAX_VALUE);
            }

            if (jarrRows.get(1).getAsInt() == 0)
                jarrRows.set(1, new JsonPrimitive(Integer.MAX_VALUE));

            LinkedHashMap<String, List<Object>> paramValueMap = new LinkedHashMap<>();
            List<List<Object>> paramValueArray = new ArrayList<>();

            for (int i = 0; i < jarrParams.size(); i++) {
                DLLVO.Param dllParam = paramMap.get(jarrParams.get(i).getAsString());
                List<DLLVO.Raw> rawData = getService(DLLService.class).getDLLData(dllSeq, dllParam.getSeq());
                List<Object> filteredData = new ArrayList<>();
                for (DLLVO.Raw dr : rawData) {
                    if (dr.getRowNo() >= jarrRows.get(0).getAsInt()
                        && dr.getRowNo() <= jarrRows.get(1).getAsInt()) {
                        if (dllParam.getParamType().equalsIgnoreCase("string"))
                            filteredData.add(dr.getParamValStr());
                        else
                            filteredData.add(dr.getParamVal());
                    }
                }
                paramValueMap.put(dllParam.getSeq().valueOf(), filteredData);
                paramValueArray.add(filteredData);
            }

            JsonObject jobjResult = new JsonObject();
            jobjResult.add("dllParamSet", jarrParams);
            jobjResult.add("dllRowRange", jarrRows);

            if (resultDataType.equals("map")) {
                jobjResult.add("data", ServerConstants.GSON.toJsonTree(paramValueMap));
            }
            else {
                jobjResult.add("data", ServerConstants.GSON.toJsonTree(paramValueArray));
            }

            return ResponseHelper.response(200, "Success - DLL Data List", jobjResult);
        }

        if (command.equals("data-add")) {
            DLLVO.Raw dllRaw = getService(DLLService.class).insertDLLData(user.getUid(), payload);
            return ResponseHelper.response(200, "Success - DLL Data Add", dllRaw);
        }

        if (command.equals("data-modify")) {
            DLLVO.Raw dllRaw = getService(DLLService.class).updateDLLData(user.getUid(), payload);
            return ResponseHelper.response(200, "Success - DLL Data Modify", dllRaw);
        }

        if (command.equals("data-remove")) {
            CryptoField dllSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "dllSeq"))
                dllSeq = CryptoField.decode(payload.get("dllSeq").getAsString(), 0L);

            if (dllSeq == null || dllSeq.isEmpty())
                throw new HandledServiceException(411, "요청 파라미터 오류입니다. [필수 파라미터 누락]");

            // paramSeq array
            Map<String, DLLVO.Param> paramMap = new HashMap<>();
            List<DLLVO.Param> dllParams = getService(DLLService.class).getDLLParamList(dllSeq);
            if (dllParams == null) dllParams = new ArrayList<>();
            for (DLLVO.Param dp : dllParams)
                paramMap.put(dp.getSeq().valueOf(), dp);

            JsonArray jarrParams = null;
            if (!checkJsonEmpty(payload, "dllParamSet"))
                jarrParams = payload.get("dllParamSet").getAsJsonArray();

            JsonArray jarrRows = null;
            if (!checkJsonEmpty(payload, "dllRowRange"))
                jarrRows = payload.get("dllRowRange").getAsJsonArray();

            if (jarrParams != null && jarrParams.size() > 0) {
                // 파라미터 하나 이상 정해서 삭제
                if (jarrParams.size() == dllParams.size()) {
                    // row 조건에 따라서 지우기, 해당 row 전체 삭제만 있음.
                    if (jarrRows != null && jarrRows.size() == 2) {
                        if (jarrRows.get(1).getAsInt() == 0)
                            jarrRows.set(1, new JsonPrimitive(Integer.MAX_VALUE));

                        getService(DLLService.class).deleteDLLDataByRow(dllSeq, jarrRows.get(0).getAsInt(), jarrRows.get(1).getAsInt());
                    }
                    else {
                        // 모두 삭제
                        getService(DLLService.class).deleteDLLData(dllSeq);
                    }
                }
                else {
                    // 일부 내용만 삭제 (사실상 업데이트)
                    for (int i = 0; i < jarrParams.size(); i++) {
                        DLLVO.Param dp = paramMap.get(jarrParams.get(i).getAsString());
                        if (dp == null) continue;

                        List<DLLVO.Raw> dllRaws = getService(DLLService.class).getDLLData(dllSeq, dp.getSeq());
                        if (dllRaws == null) continue;

                        for (DLLVO.Raw dr : dllRaws) {
                            if (jarrRows != null && jarrRows.size() == 2) {
                                if (jarrRows.get(1).getAsInt() == 0)
                                    jarrRows.set(1, new JsonPrimitive(Integer.MAX_VALUE));

                                if (dr.getRowNo() >= jarrRows.get(0).getAsInt()
                                        && dr.getRowNo() <= jarrRows.get(1).getAsInt()) {
                                    dr.setParamVal(0.0);
                                    dr.setParamValStr("");
                                    getService(DLLService.class).updateDLLData(dr);
                                }
                            }
                            else {
                                dr.setParamVal(0.0);
                                dr.setParamValStr("");
                                getService(DLLService.class).updateDLLData(dr);
                            }
                        }
                    }
                }
            }
            else {
                // row 조건에 따라서 지우기, 해당 row 전체 삭제만 있음.
                if (jarrRows != null && jarrRows.size() == 2) {
                    if (jarrRows.get(1).getAsInt() == 0)
                        jarrRows.set(1, new JsonPrimitive(Integer.MAX_VALUE));

                    getService(DLLService.class).deleteDLLDataByRow(dllSeq, jarrRows.get(0).getAsInt(), jarrRows.get(1).getAsInt());
                }
            }

            // arrange rowNo
            for (DLLVO.Param dp : dllParams) {
                int rowNo = 1;
                List<DLLVO.Raw> dllRaws = getService(DLLService.class).getDLLData(dllSeq, dp.getSeq());
                if (dllRaws == null) continue;
                for (DLLVO.Raw dr : dllRaws) {
                    dr.setRowNo(rowNo++);
                    getService(DLLService.class).updateDLLData(dr);
                }
            }

            return ResponseHelper.response(200, "Success - DLL Data Remove By Row", "");
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
        if (command.equals("upload")) {
            RawVO.Upload rawUpload = getService(RawService.class).doUpload(user.getUid(), payload);
            return ResponseHelper.response(200, "Success - Upload Raw Data", rawUpload);
        }

        if (command.equals("check-param")) {
            CryptoField presetPack = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "presetPack"))
                presetPack = CryptoField.decode(payload.get("presetPack").getAsString(), 0L);

            if (presetPack == null || presetPack.isEmpty())
                throw new HandledServiceException(404, "파라미터를 확인하세요.");

            String headerRow = "";
            if (!checkJsonEmpty(payload, "headerRow"))
                headerRow = payload.get("headerRow").getAsString();

            String importFilePath = "";
            if (!checkJsonEmpty(payload, "importFilePath"))
                importFilePath = payload.get("importFilePath").getAsString();

            if ((headerRow == null || headerRow.isEmpty()) && (importFilePath == null || importFilePath.isEmpty()))
                throw new HandledServiceException(404, "파라미터를 확인하세요.");

            CryptoField presetSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "presetSeq"))
                presetSeq = CryptoField.decode(payload.get("presetSeq").getAsString(), 0L);

            PresetVO preset = null;
            if (presetSeq == null || presetSeq.isEmpty())
                preset = getService(ParamService.class).getActivePreset(presetPack);
            else
                preset = getService(ParamService.class).getPresetBySeq(presetSeq);

            String dataType = "fltp";
            if (!checkJsonEmpty(payload, "dataType"))
                dataType = payload.get("dataType").getAsString();

            List<ParamVO> presetParams = getService(ParamService.class).getPresetParamList(
                    preset.getPresetPack(), preset.getSeq(), CryptoField.LZERO, CryptoField.LZERO,
                    1, 99999);

            // data 0 is date
            List<String> notMappedParams = new ArrayList<>();
            List<Integer> mappedIndexes = new ArrayList<>();
            List<ParamVO> mappedParams = new ArrayList<>();

            if (dataType.equals("grt") || dataType.equals("fltp") || dataType.equals("flts")) {
                if (presetParams == null) presetParams = new ArrayList<>();
                Map<String, ParamVO> adamsMap = new LinkedHashMap<>();
                Map<String, ParamVO> zaeroMap = new LinkedHashMap<>();
                Map<String, ParamVO> grtMap = new LinkedHashMap<>();
                Map<String, ParamVO> fltpMap = new LinkedHashMap<>();
                Map<String, ParamVO> fltsMap = new LinkedHashMap<>();
                for (ParamVO param : presetParams) {
                    adamsMap.put(param.getAdamsKey() + "_" + param.getPropInfo().getParamUnit(), param);
                    zaeroMap.put(param.getZaeroKey() + "_" + param.getPropInfo().getParamUnit(), param);
                    grtMap.put(param.getGrtKey() + "_" + param.getPropInfo().getParamUnit(), param);
                    fltpMap.put(param.getFltpKey() + "_" + param.getPropInfo().getParamUnit(), param);
                    fltsMap.put(param.getFltsKey() + "_" + param.getPropInfo().getParamUnit(), param);
                }

                String[] splitParam = headerRow.trim().split(",");

                for (int i = 0; i < splitParam.length; i++) {
                    String p = splitParam[i];
                    if (p.equalsIgnoreCase("date")) continue;

                    ParamVO pi = null;
                    if (dataType.equals("adams") && adamsMap.containsKey(p)) pi = adamsMap.get(p);
                    if (dataType.equals("zaero") && zaeroMap.containsKey(p)) pi = zaeroMap.get(p);
                    if (dataType.equals("grt") && grtMap.containsKey(p)) pi = grtMap.get(p);
                    if (dataType.equals("fltp") && fltpMap.containsKey(p)) pi = fltpMap.get(p);
                    if (dataType.equals("flts") && fltsMap.containsKey(p)) pi = fltsMap.get(p);

                    if (pi == null) {
                        notMappedParams.add(p);
                    } else {
                        mappedParams.add(pi);
                        mappedIndexes.add(i);
                    }
                }
            }
            else {
                if (presetParams == null) presetParams = new ArrayList<>();
                Map<String, ParamVO> adamsMap = new LinkedHashMap<>();
                Map<String, ParamVO> zaeroMap = new LinkedHashMap<>();
                for (ParamVO param : presetParams) {
                    adamsMap.put(param.getAdamsKey(), param);
                    zaeroMap.put(param.getZaeroKey(), param);
                }

                // File loading
                if (importFilePath.contains("C:\\")) {
                    importFilePath = importFilePath.replaceAll("\\\\", "/");
                    importFilePath = importFilePath.replaceAll("C:/", staticLocation.substring("file:".length()));
                }

                List<String> allParams = null;
                List<List<Double>> allData = null;

                try {
                    File fImport = new File(importFilePath);
                    FileInputStream fis = new FileInputStream(fImport);

                    BufferedReader br = new BufferedReader(new InputStreamReader(fis));
                    String line = null;

                    String state = "before";
                    List<List<String>> parameters = new ArrayList<>();
                    List<List<List<Double>>> dataList = new ArrayList<>();
                    LinkedList<Double> timeSet = new LinkedList<>();

                    List<String> blockParams = null;
                    List<List<Double>> blockDatas = null;

                    while ((line = br.readLine()) != null) {
                        line = line.trim();

                        String[] splitted = line.split("\\s+");
                        if (splitted == null || splitted.length < 2) {
                            if (!state.equals("before")) {
                                state = "before";
                            }
                            continue;
                        }

                        if (state.equals("before")) {
                            if (!splitted[0].equalsIgnoreCase("UNITS"))
                                continue;

                            // append parameter array
                            blockParams = new ArrayList<>();
                            blockDatas = new ArrayList<>();

                            for (int i = 1; i < splitted.length; i++) {
                                blockParams.add(splitted[i]);
                                blockDatas.add(new ArrayList<>());
                            }

                            parameters.add(blockParams);
                            dataList.add(blockDatas);

                            state = "extract";
                            continue;
                        }

                        if (state.equals("extract")) {
                            if (!timeSet.contains(Double.parseDouble(splitted[0])))
                                timeSet.add(Double.parseDouble(splitted[0]));

                            for (int i = 1; i < splitted.length; i++) {
                                if (i < splitted.length)
                                    blockDatas.get(i - 1).add(Double.parseDouble(splitted[i]));
                                else
                                    blockDatas.get(i - 1).add(0.0);
                            }
                        }
                    }

                    allParams = new ArrayList<>();
                    for (List<String> p : parameters)
                        allParams.addAll(p);

                    allData = new ArrayList<>();
                    for (List<List<Double>> d : dataList)
                        allData.addAll(d);

                    br.close();
                    fis.close();
                } catch(IOException ex) {
                    ex.printStackTrace();
                    throw new HandledServiceException(411, "분석중 오류가 발생했습니다. " + ex.getMessage());
                }

                for (int i = 0; i < allParams.size(); i++) {
                    String p = allParams.get(i);
                    ParamVO pi = null;
                    if (dataType.equals("adams") && adamsMap.containsKey(p)) pi = adamsMap.get(p);
                    if (dataType.equals("zaero") && zaeroMap.containsKey(p)) pi = zaeroMap.get(p);

                    if (pi == null) {
                        notMappedParams.add(p);
                    }
                    else {
                        mappedParams.add(pi);
                    }
                }
            }

            JsonObject jobjResult = new JsonObject();
            jobjResult.add("notMappedParams", ServerConstants.GSON.toJsonTree(notMappedParams));
            jobjResult.add("mappedParams", ServerConstants.GSON.toJsonTree(mappedParams));

            return ResponseHelper.response(200, "Success - Check matching params", jobjResult);
        }

        if (command.equals("progress")) {
            RawVO.Upload rawUpload = getService(RawService.class).getProgress(user.getUid(), payload);
            return ResponseHelper.response(200, "Success - Get Upload Progress", rawUpload);
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

    @RequestMapping(value = "/part")
    @ResponseBody
    public Object apiPart(HttpServletRequest request, @PathVariable String serviceVersion,
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
            CryptoField.NAuth registerUid = null;
            if (!checkJsonEmpty(payload, "registerUid"))
                registerUid = CryptoField.NAuth.decode(payload.get("registerUid").getAsString(), 0L);

            CryptoField uploadSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "uploadSeq"))
                uploadSeq = CryptoField.decode(payload.get("uploadSeq").getAsString(), 0L);

            Integer pageNo = 1;
            if (!checkJsonEmpty(payload, "pageNo"))
                pageNo = payload.get("pageNo").getAsInt();

            Integer pageSize = 15;
            if (!checkJsonEmpty(payload, "pageSize"))
                pageSize = payload.get("pageSize").getAsInt();

            List<PartVO> partList = getService(PartService.class).getPartList(registerUid, uploadSeq, pageNo, pageSize);
            int partCount = getService(PartService.class).getPartCount(registerUid, uploadSeq);

            return ResponseHelper.response(200, "Success - Part List", partCount, partList);
        }

        if (command.equals("info")) {
            CryptoField partSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "partSeq"))
                partSeq = CryptoField.decode(payload.get("partSeq").getAsString(), 0L);

            PartVO partInfo = getService(PartService.class).getPartBySeq(partSeq);

            return ResponseHelper.response(200, "Success - Part Info", partInfo);
        }

        // 업로드 리스트 가져가기
        if (command.equals("create-shortblock")) {
            ShortBlockVO.Meta shortBlockMeta = getService(PartService.class).doCreateShortBlock(user.getUid(), payload);
            return ResponseHelper.response(200, "Success - Create ShortBlock Data", shortBlockMeta);
        }

        if (command.equals("progress")) {
            ShortBlockVO.Meta shortBlockMeta = getService(PartService.class).getProgress(user.getUid(), payload);
            return ResponseHelper.response(200, "Success - Get Create ShortBlock Progress", shortBlockMeta);
        }

        if (command.equals("edit")) {
            CryptoField partSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "partSeq"))
                partSeq = CryptoField.decode(payload.get("partSeq").getAsString(), 0L);

            CryptoField paramPack = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "paramPack"))
                paramPack = CryptoField.decode(payload.get("paramPack").getAsString(), 0L);

            CryptoField paramSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "paramSeq"))
                paramSeq = CryptoField.decode(payload.get("paramSeq").getAsString(), 0L);

            String julianTimeAt = "";
            if (!checkJsonEmpty(payload, "julianTimeAt"))
                julianTimeAt = payload.get("julianTimeAt").getAsString();

            if (paramSeq == null || paramSeq.isEmpty()
                    || paramPack == null || paramPack.isEmpty()
                    || paramSeq == null || paramSeq.isEmpty()
                    || julianTimeAt.isEmpty()) {
                throw new HandledServiceException(411, "값을 수정하기 위한 기초정보가 없습니다.");
            }

            Double dblVal = null;
            if (!checkJsonEmpty(payload, "value"))
                dblVal = payload.get("value").getAsDouble();

            if (dblVal == null)
                throw new HandledServiceException(411, "변경할 값이 없습니다.");

            ParamVO param = getService(ParamService.class).getParamByReference(partSeq, paramPack, paramSeq);
            if (param == null || param.getReferenceSeq() == null)
                throw new HandledServiceException(411, "미관여 파라미터입니다.");

            Map<String, Object> params = new HashMap<>();
            params.put("partSeq", partSeq);
            params.put("referenceSeq", param.getReferenceSeq());
            params.put("julianTimeAt", julianTimeAt);
            params.put("dblVal", dblVal.doubleValue());
            PartVO.Raw rawData = getService(PartService.class).updatePartValueByParams(params);
            if (rawData == null)
                throw new HandledServiceException(404, "변경하려는 정보를 찾지 못했습니다.");

            // 정상 수정 되면 lpf, hpf 새로 생성해야함. => 개별로 처리되지 않기 때문에.. 전체 필터 적용 후 업데이트
            try {
                updateFilterData(partSeq);
            } catch(Exception e) {
                throw new HandledServiceException(411, e.getMessage());
            }

            return ResponseHelper.response(200, "Success - Updated part cell value", rawData);
        }

        if (command.equals("row-data")) {
            CryptoField partSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "partSeq"))
                partSeq = CryptoField.decode(payload.get("partSeq").getAsString(), 0L);

            String filterType = "N";
            if (!checkJsonEmpty(payload, "filterType"))
                filterType = payload.get("filterType").getAsString();

            PartVO partInfo = getService(PartService.class).getPartBySeq(partSeq);

            // 요청 파라미터 셋.
            JsonObject jobjResult = new JsonObject();
            JsonArray jarrParams = null;
            if (!checkJsonEmpty(payload, "paramSet"))
                jarrParams = payload.get("paramSet").getAsJsonArray();
            List<ParamVO> params = new ArrayList<>();

            if (jarrParams == null || jarrParams.size() == 0) {
                List<String> paramSet = listOps.range("P" + partInfo.getSeq().originOf(), 0, Integer.MAX_VALUE);
                for (String p : paramSet) {
                    ParamVO param = getService(ParamService.class).getPresetParamBySeq(
                            new CryptoField(Long.parseLong(p.substring(1))));
                    if (param == null) {
                        param = getService(ParamService.class).getNotMappedParamBySeq(
                                new CryptoField(Long.parseLong(p.substring(1))));
                        if (param == null) continue;
                    }

                    params.add(param);
                }
            }
            else {
                for (int i = 0; i < jarrParams.size(); i++) {
                    Long paramKey = jarrParams.get(i).getAsLong();
                    ParamVO param = getService(ParamService.class).getPresetParamBySeq(new CryptoField(paramKey));
                    if (param == null) {
                        param = getService(ParamService.class).getNotMappedParamBySeq(
                                new CryptoField(paramKey));
                        if (param == null) continue;
                    }
                    params.add(param);
                }
            }

            JsonArray jarrJulian = payload.get("julianRange").getAsJsonArray();
            String julianFrom = jarrJulian.get(0).getAsString();
            if (julianFrom == null || julianFrom.isEmpty()) {
                Set<String> listSet = zsetOps.rangeByScore("P" + partInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);
                if (listSet != null && listSet.size() > 0)
                    julianFrom = listSet.iterator().next();
            }
            String julianTo = jarrJulian.get(1).getAsString();
            if (julianTo == null || julianTo.isEmpty()) {
                Set<String> listSet = zsetOps.reverseRangeByScore("P" + partInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);
                if (listSet != null && listSet.size() > 0)
                    julianTo = listSet.iterator().next();
            }

            if (julianFrom == null || julianFrom.isEmpty() || julianTo == null || julianTo.isEmpty()) {
                jobjResult.add("paramSet", ServerConstants.GSON.toJsonTree(params));
                jobjResult.add("julianSet", ServerConstants.GSON.toJsonTree(new ArrayList<String>()));
                jobjResult.add("data", ServerConstants.GSON.toJsonTree(new LinkedHashMap<String, List<Double>>()));
                return ResponseHelper.response(200, "Success - rowData", jobjResult);
            }

            Set<String> listSet = zsetOps.rangeByScore("P" + partInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);
            if (listSet == null || listSet.size() == 0) {
                jobjResult.add("paramSet", ServerConstants.GSON.toJsonTree(params));
                jobjResult.add("julianSet", ServerConstants.GSON.toJsonTree(new ArrayList<String>()));
                jobjResult.add("data", ServerConstants.GSON.toJsonTree(new LinkedHashMap<String, List<Double>>()));
                return ResponseHelper.response(200, "Success - rowData", jobjResult);
            }

            String julianStart = listSet.iterator().next();
            Long startRowAt = zsetOps.score("P" + partInfo.getSeq().originOf() + ".R", julianStart).longValue();

            Long rankFrom = zsetOps.rank("P" + partInfo.getSeq().originOf() + ".R", julianFrom);
            if (rankFrom == null) {
                julianFrom = zsetOps.rangeByScore("P" + partInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE).iterator().next();
                rankFrom = zsetOps.rank("P" + partInfo.getSeq().originOf() + ".R", julianFrom);
            }
            Long rankTo = zsetOps.rank("P" + partInfo.getSeq().originOf() + ".R", julianTo);
            if (rankTo == null) {
                julianTo = zsetOps.reverseRangeByScore("P" + partInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE).iterator().next();
                rankTo = zsetOps.rank("P" + partInfo.getSeq().originOf() + ".R", julianTo);
            }

            LinkedHashMap<String, List<Double>> rowData = new LinkedHashMap<>();
            for (ParamVO p : params) {
                listSet = zsetOps.rangeByScore(
                        "P" + partInfo.getSeq().originOf() + "." + filterType + p.getReferenceSeq(), startRowAt + rankFrom, startRowAt + rankTo);

                Iterator<String> iterListSet = listSet.iterator();
                while (iterListSet.hasNext()) {
                    String rowVal = iterListSet.next();
                    String julianTime = rowVal.substring(0, rowVal.lastIndexOf(":"));
                    List<Double> rowList = rowData.get(julianTime);
                    if (rowList == null) {
                        rowList = new ArrayList<>();
                        rowData.put(julianTime, rowList);
                    }
                    Double dblVal = Double.parseDouble(rowVal.substring(rowVal.lastIndexOf(":") + 1));
                    rowList.add(dblVal);
                }
            }

            jobjResult.add("paramSet", ServerConstants.GSON.toJsonTree(params));
            jobjResult.add("julianSet", ServerConstants.GSON.toJsonTree(Arrays.asList(rowData.keySet())));
            jobjResult.add("data", ServerConstants.GSON.toJsonTree(rowData.values()));

            return ResponseHelper.response(200, "Success - rowData", jobjResult);
        }

        if (command.equals("column-data")) {
            CryptoField partSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "partSeq"))
                partSeq = CryptoField.decode(payload.get("partSeq").getAsString(), 0L);

            String filterType = "N";
            if (!checkJsonEmpty(payload, "filterType"))
                filterType = payload.get("filterType").getAsString();

            PartVO partInfo = getService(PartService.class).getPartBySeq(partSeq);

            // 요청 파라미터 셋.
            JsonObject jobjResult = new JsonObject();
            JsonArray jarrParams = null;
            if (!checkJsonEmpty(payload, "paramSet"))
                jarrParams = payload.get("paramSet").getAsJsonArray();
            List<ParamVO> params = new ArrayList<>();

            if (jarrParams == null || jarrParams.size() == 0) {
                List<String> paramSet = listOps.range("P" + partInfo.getSeq().originOf(), 0, Integer.MAX_VALUE);
                for (String p : paramSet) {
                    ParamVO param = getService(ParamService.class).getPresetParamBySeq(
                            new CryptoField(Long.parseLong(p.substring(1))));
                    if (param == null) {
                        param = getService(ParamService.class).getNotMappedParamBySeq(
                                new CryptoField(Long.parseLong(p.substring(1))));
                        if (param == null) continue;
                    }

                    params.add(param);
                }
            }
            else {
                for (int i = 0; i < jarrParams.size(); i++) {
                    Long paramKey = jarrParams.get(i).getAsLong();
                    ParamVO param = getService(ParamService.class).getPresetParamBySeq(new CryptoField(paramKey));
                    if (param == null) {
                        param = getService(ParamService.class).getNotMappedParamBySeq(
                                new CryptoField(paramKey));
                        if (param == null) continue;
                    }
                    params.add(param);
                }
            }

            List<String> julianData = new ArrayList<>();
            LinkedHashMap<String, List<Double>> paramData = new LinkedHashMap<>();
            for (ParamVO p : params) {
                Set<String> listSet = zsetOps.rangeByScore(
                        "P" + partInfo.getSeq().originOf() + "." + filterType + p.getReferenceSeq(), 0, Integer.MAX_VALUE);
                if (listSet == null || listSet.size() == 0) continue;

                List<Double> rowData = new ArrayList<>();
                paramData.put(p.getParamKey(), rowData);

                Iterator<String> iterListSet = listSet.iterator();
                while (iterListSet.hasNext()) {
                    String rowVal = iterListSet.next();
                    String julianTime = rowVal.substring(0, rowVal.lastIndexOf(":"));
                    if (!julianData.contains(julianTime)) julianData.add(julianTime);

                    Double dblVal = Double.parseDouble(rowVal.substring(rowVal.lastIndexOf(":") + 1));
                    rowData.add(dblVal);
                }
            }

            jobjResult.add("julianSet", ServerConstants.GSON.toJsonTree(Arrays.asList(julianData)));
            jobjResult.add("data", ServerConstants.GSON.toJsonTree(paramData.values()));

            return ResponseHelper.response(200, "Success - columnData", jobjResult);
        }

        throw new HandledServiceException(411, "명령이 정의되지 않았습니다.");
    }

    @RequestMapping(value = "/shortblock")
    @ResponseBody
    public Object apiShortBlock(HttpServletRequest request, @PathVariable String serviceVersion,
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
            CryptoField.NAuth registerUid = null;
            if (!checkJsonEmpty(payload, "registerUid"))
                registerUid = CryptoField.NAuth.decode(payload.get("registerUid").getAsString(), 0L);

            CryptoField partSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "partSeq"))
                partSeq = CryptoField.decode(payload.get("partSeq").getAsString(), 0L);

            Integer pageNo = 1;
            if (!checkJsonEmpty(payload, "pageNo"))
                pageNo = payload.get("pageNo").getAsInt();

            Integer pageSize = 15;
            if (!checkJsonEmpty(payload, "pageSize"))
                pageSize = payload.get("pageSize").getAsInt();

            List<ShortBlockVO> shortBlockList = getService(PartService.class).getShortBlockList(registerUid, partSeq, pageNo, pageSize);
            int shortBlockCount = getService(PartService.class).getShortBlockCount(registerUid, partSeq);

            return ResponseHelper.response(200, "Success - Short Block List", shortBlockCount, shortBlockList);
        }

        if (command.equals("info")) {
            CryptoField blockSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "blockSeq"))
                blockSeq = CryptoField.decode(payload.get("blockSeq").getAsString(), 0L);

            ShortBlockVO shortBlockInfo = getService(PartService.class).getShortBlockBySeq(blockSeq);

            return ResponseHelper.response(200, "Success - ShortBlock Info", shortBlockInfo);
        }

        if (command.equals("remove-meta")) {
            CryptoField blockMetaSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "blockMetaSeq"))
                blockMetaSeq = CryptoField.decode(payload.get("blockMetaSeq").getAsString(), 0L);

            //getService(PartService.class).deleteShortBlockMeta(blockMetaSeq);

            return ResponseHelper.response(200, "Success - Remove ShortBlock Meta", "");
        }

        if (command.equals("row-data")) {
            CryptoField blockSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "blockSeq"))
                blockSeq = CryptoField.decode(payload.get("blockSeq").getAsString(), 0L);

            ShortBlockVO blockInfo = getService(PartService.class).getShortBlockBySeq(blockSeq);

            // 요청 파라미터 셋.
            JsonObject jobjResult = new JsonObject();
            JsonArray jarrParams = null;
            if (!checkJsonEmpty(payload, "paramSet"))
                jarrParams = payload.get("paramSet").getAsJsonArray();
            List<ParamVO> params = new ArrayList<>();

            if (jarrParams == null || jarrParams.size() == 0) {
                List<String> paramSet = listOps.range("S" + blockInfo.getSeq().originOf(), 0, Integer.MAX_VALUE);
                for (String p : paramSet) {
                    ParamVO param = getService(ParamService.class).getPresetParamBySeq(
                            new CryptoField(Long.parseLong(p.substring(1))));
                    if (param == null) {
                        param = getService(ParamService.class).getNotMappedParamBySeq(
                                new CryptoField(Long.parseLong(p.substring(1))));
                        if (param == null) continue;
                    }

                    params.add(param);
                }
            }
            else {
                for (int i = 0; i < jarrParams.size(); i++) {
                    Long paramKey = jarrParams.get(i).getAsLong();
                    ParamVO param = getService(ParamService.class).getPresetParamBySeq(new CryptoField(paramKey));
                    if (param == null) {
                        param = getService(ParamService.class).getNotMappedParamBySeq(
                                new CryptoField(paramKey));
                        if (param == null) continue;
                    }
                    params.add(param);
                }
            }

            JsonArray jarrJulian = payload.get("julianRange").getAsJsonArray();
            String julianFrom = jarrJulian.get(0).getAsString();
            if (julianFrom == null || julianFrom.isEmpty())
                julianFrom = zsetOps.rangeByScore("S" + blockInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE).iterator().next();
            String julianTo = jarrJulian.get(1).getAsString();
            if (julianTo == null || julianTo.isEmpty())
                julianTo = zsetOps.reverseRangeByScore("S" + blockInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE).iterator().next();

            String julianStart = zsetOps.rangeByScore("S" + blockInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE).iterator().next();
            Long startRowAt = zsetOps.score("S" + blockInfo.getSeq().originOf() + ".R", julianStart).longValue();

            Long rankFrom = zsetOps.rank("S" + blockInfo.getSeq().originOf() + ".R", julianFrom);
            if (rankFrom == null) {
                julianFrom = zsetOps.rangeByScore("S" + blockInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE).iterator().next();
                rankFrom = zsetOps.rank("S" + blockInfo.getSeq().originOf() + ".R", julianFrom);
            }
            Long rankTo = zsetOps.rank("S" + blockInfo.getSeq().originOf() + ".R", julianTo);
            if (rankTo == null) {
                julianTo = zsetOps.reverseRangeByScore("S" + blockInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE).iterator().next();
                rankTo = zsetOps.rank("S" + blockInfo.getSeq().originOf() + ".R", julianTo);
            }

            LinkedHashMap<String, List<Double>> rowData = new LinkedHashMap<>();
            for (ParamVO p : params) {
                Set<String> listSet = zsetOps.rangeByScore(
                        "S" + blockInfo.getSeq().originOf() + ".N" + p.getReferenceSeq(), startRowAt + rankFrom, startRowAt + rankTo);

                Iterator<String> iterListSet = listSet.iterator();
                while (iterListSet.hasNext()) {
                    String rowVal = iterListSet.next();
                    String julianTime = rowVal.substring(0, rowVal.lastIndexOf(":"));
                    List<Double> rowList = rowData.get(julianTime);
                    if (rowList == null) {
                        rowList = new ArrayList<>();
                        rowData.put(julianTime, rowList);
                    }
                    Double dblVal = Double.parseDouble(rowVal.substring(rowVal.lastIndexOf(":") + 1));
                    rowList.add(dblVal);
                }
            }

            jobjResult.add("paramSet", ServerConstants.GSON.toJsonTree(params));
            jobjResult.add("julianSet", ServerConstants.GSON.toJsonTree(Arrays.asList(rowData.keySet())));
            jobjResult.add("data", ServerConstants.GSON.toJsonTree(rowData.values()));

            return ResponseHelper.response(200, "Success - rowData", jobjResult);
        }

        if (command.equals("column-data")) {
            CryptoField blockSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "blockSeq"))
                blockSeq = CryptoField.decode(payload.get("blockSeq").getAsString(), 0L);

            ShortBlockVO blockInfo = getService(PartService.class).getShortBlockBySeq(blockSeq);

            // 요청 파라미터 셋.
            JsonObject jobjResult = new JsonObject();
            JsonArray jarrParams = null;
            if (!checkJsonEmpty(payload, "paramSet"))
                jarrParams = payload.get("paramSet").getAsJsonArray();
            List<ParamVO> params = new ArrayList<>();

            if (jarrParams == null || jarrParams.size() == 0) {
                List<String> paramSet = listOps.range("S" + blockInfo.getSeq().originOf(), 0, Integer.MAX_VALUE);
                for (String p : paramSet) {
                    ParamVO param = getService(ParamService.class).getPresetParamBySeq(
                            new CryptoField(Long.parseLong(p.substring(1))));
                    if (param == null) {
                        param = getService(ParamService.class).getNotMappedParamBySeq(
                                new CryptoField(Long.parseLong(p.substring(1))));
                        if (param == null) continue;
                    }

                    params.add(param);
                }
            }
            else {
                for (int i = 0; i < jarrParams.size(); i++) {
                    Long paramKey = jarrParams.get(i).getAsLong();
                    ParamVO param = getService(ParamService.class).getPresetParamBySeq(new CryptoField(paramKey));
                    if (param == null) {
                        param = getService(ParamService.class).getNotMappedParamBySeq(
                                new CryptoField(paramKey));
                        if (param == null) continue;
                    }
                    params.add(param);
                }
            }

            List<String> julianData = new ArrayList<>();
            LinkedHashMap<String, List<Double>> paramData = new LinkedHashMap<>();
            for (ParamVO p : params) {
                Set<String> listSet = zsetOps.rangeByScore(
                        "S" + blockInfo.getSeq().originOf() + ".N" + p.getReferenceSeq(), 0, Integer.MAX_VALUE);
                List<Double> rowData = new ArrayList<>();
                paramData.put(p.getParamKey(), rowData);

                Iterator<String> iterListSet = listSet.iterator();
                while (iterListSet.hasNext()) {
                    String rowVal = iterListSet.next();
                    String julianTime = rowVal.substring(0, rowVal.lastIndexOf(":"));
                    if (!julianData.contains(julianTime)) julianData.add(julianTime);

                    Double dblVal = Double.parseDouble(rowVal.substring(rowVal.lastIndexOf(":") + 1));
                    rowData.add(dblVal);
                }
            }

            jobjResult.add("julianSet", ServerConstants.GSON.toJsonTree(Arrays.asList(julianData)));
            jobjResult.add("data", ServerConstants.GSON.toJsonTree(paramData.values()));

            return ResponseHelper.response(200, "Success - columnData", jobjResult);
        }

        throw new HandledServiceException(411, "명령이 정의되지 않았습니다.");
    }

    private void updateFilterData(CryptoField partSeq) throws Exception {
        PartVO part = getService(PartService.class).getPartBySeq(partSeq);

        String julianFrom = "";
        if (julianFrom == null || julianFrom.isEmpty())
            julianFrom = zsetOps.rangeByScore("P" + part.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE).iterator().next();
        String julianTo = "";
        if (julianTo == null || julianTo.isEmpty())
            julianTo = zsetOps.reverseRangeByScore("P" + part.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE).iterator().next();

        String julianStart = zsetOps.rangeByScore("P" + part.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE).iterator().next();
        Long startRowAt = zsetOps.score("P" + part.getSeq().originOf() + ".R", julianStart).longValue();

        Long rankFrom = zsetOps.rank("P" + part.getSeq().originOf() + ".R", julianFrom);
        if (rankFrom == null) {
            julianFrom = zsetOps.rangeByScore("P" + part.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE).iterator().next();
            rankFrom = zsetOps.rank("P" + part.getSeq().originOf() + ".R", julianFrom);
        }
        Long rankTo = zsetOps.rank("P" + part.getSeq().originOf() + ".R", julianTo);
        if (rankTo == null) {
            julianTo = zsetOps.reverseRangeByScore("P" + part.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE).iterator().next();
            rankTo = zsetOps.rank("P" + part.getSeq().originOf() + ".R", julianTo);
        }

        List<ParamVO> mappedParams = new ArrayList<>();
        List<ParamVO> presetParams = getService(ParamService.class).getPresetParamList(
                part.getPresetPack(), part.getPresetSeq(), null, null, 1, 999999);
        if (presetParams != null) mappedParams.addAll(presetParams);

        List<ParamVO> notMappedParams = getService(ParamService.class).getNotMappedParams(part.getUploadSeq());
        if (notMappedParams != null) mappedParams.addAll(notMappedParams);

        for (ParamVO param : mappedParams) {
            String rowKey = "P" + part.getSeq().originOf() + ".N" + param.getReferenceSeq();

            Set<String> listSet = zsetOps.rangeByScore(
                    rowKey, startRowAt + rankFrom, startRowAt + rankTo);
            Iterator<String> iterListSet = listSet.iterator();

            List<String> jts = new ArrayList<>();
            List<Double> pvs = new ArrayList<>();

            while (iterListSet.hasNext()) {
                String rowVal = iterListSet.next();
                String julianTime = rowVal.substring(0, rowVal.lastIndexOf(":"));
                jts.add(julianTime);
                Double dblVal = Double.parseDouble(rowVal.substring(rowVal.lastIndexOf(":") + 1));
                pvs.add(dblVal);
            }

            PartImportTask.applyFilterData(processPath, zsetOps, "lpf", "10", "0.4", "", part, param, jts, pvs, startRowAt + rankFrom);
            PartImportTask.applyFilterData(processPath, zsetOps, "hpf", "10", "0.02", "high", part, param, jts, pvs, startRowAt + rankFrom);
        }

        part.setLpfDone(true);
        part.setHpfDone(true);
        getService(PartService.class).updatePart(part);
    }
}
