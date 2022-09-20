package com.servetech.dynarap.controller;

import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.google.gson.JsonPrimitive;
import com.google.gson.reflect.TypeToken;
import com.servetech.dynarap.config.ServerConstants;
import com.servetech.dynarap.db.service.*;
import com.servetech.dynarap.db.service.task.PartImportTask;
import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.db.type.String64;
import com.servetech.dynarap.ext.HandledServiceException;
import com.servetech.dynarap.ext.ResponseHelper;
import com.servetech.dynarap.vo.*;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.security.core.Authentication;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.*;
import java.lang.reflect.Type;
import java.nio.charset.StandardCharsets;
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

    @RequestMapping(value = "/param-module")
    @ResponseBody
    public Object apiParamModule(HttpServletRequest request, @PathVariable String serviceVersion,
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
            List<ParamModuleVO> paramModules = getService(ParamModuleService.class).getParamModuleList();
            return ResponseHelper.response(200, "Success - ParamModule List", paramModules);
        }

        if (command.equals("add")) {
            ParamModuleVO paramModule = ServerConstants.GSON.fromJson(payload, ParamModuleVO.class);
            if (paramModule == null)
                throw new HandledServiceException(411, "저장 데이터의 형식이 맞지 않습니다.");

            if (paramModule.getModuleName() == null || paramModule.getModuleName().isEmpty())
                throw new HandledServiceException(411, "파라미터 모듈의 이름을 입력하세요.");

            paramModule.setCreatedAt(LongDate.now());
            getService(ParamModuleService.class).insertParamModule(paramModule);

            if (paramModule.getDataProp() != null && paramModule.getDataProp().size() > 0) {
                Set<String> keys = paramModule.getDataProp().keySet();
                Iterator<String> iterKeys = keys.iterator();
                while (iterKeys.hasNext()) {
                    String key = iterKeys.next();
                    String value = paramModule.getDataProp().get(key);

                    DataPropVO dataProp = new DataPropVO();
                    dataProp.setPropName(new String64(key));
                    dataProp.setPropValue(new String64(value));
                    dataProp.setReferenceType("parammodule");
                    dataProp.setReferenceKey(paramModule.getSeq());
                    dataProp.setUpdatedAt(LongDate.now());
                    getService(RawService.class).insertDataProp(dataProp);
                }
            }

            paramModule = getService(ParamModuleService.class).getParamModuleBySeq(paramModule.getSeq());

            return ResponseHelper.response(200, "Success - ParamModule Added", paramModule);
        }

        if (command.equals("modify")) {
            CryptoField moduleSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "moduleSeq"))
                moduleSeq = CryptoField.decode(payload.get("moduleSeq").getAsString(), 0L);

            if (moduleSeq == null || moduleSeq.isEmpty())
                throw new HandledServiceException(411, "파라미터를 확인하세요.");

            ParamModuleVO paramModule = ServerConstants.GSON.fromJson(payload, ParamModuleVO.class);
            if (paramModule == null)
                throw new HandledServiceException(411, "저장 데이터의 형식이 맞지 않습니다.");

            paramModule.setSeq(moduleSeq);
            getService(ParamModuleService.class).updateParamModule(paramModule);

            List<DataPropVO> props = getService(RawService.class).getDataPropList("parammodule", moduleSeq);
            if (props == null) props = new ArrayList<>();

            if (paramModule.getDataProp() != null && paramModule.getDataProp().size() > 0) {
                for (DataPropVO prop : props)
                    prop.setMarked(false);

                Set<String> keys = paramModule.getDataProp().keySet();
                Iterator<String> iterKeys = keys.iterator();
                while (iterKeys.hasNext()) {
                    String key = iterKeys.next();
                    String value = paramModule.getDataProp().get(key);

                    DataPropVO oldProp = null;
                    for (DataPropVO prop : props) {
                        if (prop.getPropName().originOf().equals(key)) {
                            oldProp = prop;
                            break;
                        }
                    }

                    if (oldProp != null) {
                        oldProp.setMarked(true);
                        oldProp.setPropValue(new String64(value));
                        oldProp.setUpdatedAt(LongDate.now());
                        getService(RawService.class).updateDataProp(oldProp);
                    }
                    else {
                        DataPropVO dataProp = new DataPropVO();
                        dataProp.setPropName(new String64(key));
                        dataProp.setPropValue(new String64(value));
                        dataProp.setReferenceType("parammodule");
                        dataProp.setReferenceKey(paramModule.getSeq());
                        dataProp.setUpdatedAt(LongDate.now());
                        getService(RawService.class).insertDataProp(dataProp);
                    }
                }

                for (DataPropVO prop : props) {
                    if (prop.isMarked() == false)
                        getService(RawService.class).deleteDataPropBySeq(prop.getSeq());
                }
            }
            else {
                if (props.size() > 0) {
                    for (DataPropVO prop : props)
                        getService(RawService.class).deleteDataPropBySeq(prop.getSeq());
                }
            }

            paramModule = getService(ParamModuleService.class).getParamModuleBySeq(moduleSeq);

            return ResponseHelper.response(200, "Success - ParamModule Updated", paramModule);
        }

        if (command.equals("remove")) {
            CryptoField moduleSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "moduleSeq"))
                moduleSeq = CryptoField.decode(payload.get("moduleSeq").getAsString(), 0L);

            if (moduleSeq == null || moduleSeq.isEmpty())
                throw new HandledServiceException(411, "파라미터를 확인하세요.");

            ParamModuleVO paramModule = getService(ParamModuleService.class).getParamModuleBySeq(moduleSeq);
            if (paramModule.isReferenced()) {
                // 레퍼런스가 있으면 삭제 플래그만
                paramModule.setDeleted(true);
                getService(ParamModuleService.class).updateParamModule(paramModule);
            }
            else {
                // 레퍼런스가 없으면 완전 삭제
                // TODO : 데이터 삭제 redis
                getService(ParamModuleService.class).deleteParamModule(moduleSeq);
            }
            return ResponseHelper.response(200, "Success - ParamModule Deleted", "");
        }

        if (command.equals("copy")) {
            CryptoField moduleSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "moduleSeq"))
                moduleSeq = CryptoField.decode(payload.get("moduleSeq").getAsString(), 0L);

            if (moduleSeq == null || moduleSeq.isEmpty())
                throw new HandledServiceException(411, "파라미터를 확인하세요.");

            ParamModuleVO oldParamModule = getService(ParamModuleService.class).getParamModuleBySeq(moduleSeq);

            ParamModuleVO paramModule = getService(ParamModuleService.class).getParamModuleBySeq(moduleSeq);
            paramModule.setCopyFromSeq(oldParamModule.getSeq());
            paramModule.setOriginInfo(oldParamModule);
            paramModule.setCreatedAt(LongDate.now());
            getService(ParamModuleService.class).insertParamModule(paramModule);
            if (paramModule.getDataProp() != null && paramModule.getDataProp().size() > 0) {
                Set<String> keys = paramModule.getDataProp().keySet();
                Iterator<String> iterKeys = keys.iterator();
                while (iterKeys.hasNext()) {
                    String key = iterKeys.next();
                    String value = paramModule.getDataProp().get(key);

                    DataPropVO dataProp = new DataPropVO();
                    dataProp.setPropName(new String64(key));
                    dataProp.setPropValue(new String64(value));
                    dataProp.setReferenceType("parammodule");
                    dataProp.setReferenceKey(paramModule.getSeq());
                    dataProp.setUpdatedAt(LongDate.now());
                    getService(RawService.class).insertDataProp(dataProp);
                }
                paramModule = getService(ParamModuleService.class).getParamModuleBySeq(paramModule.getSeq());
            }

            oldParamModule.setReferenced(true);
            getService(ParamModuleService.class).updateParamModule(oldParamModule);

            // TODO: copy data

            return ResponseHelper.response(200, "Success - ParamModule Copy", paramModule);
        }

        if (command.equals("search")) {
            String sourceType = "";
            if (!checkJsonEmpty(payload, "sourceType"))
                sourceType = payload.get("sourceType").getAsString();

            if (sourceType == null || sourceType.isEmpty())
                throw new HandledServiceException(411, "검색을 원하는 소스를 선택하세요.");

            String keyword = "";
            if (!checkJsonEmpty(payload, "keyword"))
                keyword = payload.get("keyword").getAsString();

            if (keyword == null || keyword.isEmpty())
                throw new HandledServiceException(411, "검색 키워드를 입력하세요.");

            if (sourceType.equalsIgnoreCase("part")) {
                List<PartVO> parts = getService(ParamModuleService.class).getPartListByKeyword(keyword);
                if (parts == null) parts = new ArrayList<>();
                for (PartVO part : parts) {
                    part.setParams(getService(ParamService.class).getPresetParamList(
                            part.getPresetPack(), part.getPresetSeq(), CryptoField.LZERO, CryptoField.LZERO, 1, 999999));
                }
                return ResponseHelper.response(200, "Success - ParamModule Source Search", parts);
            }
            else if (sourceType.equalsIgnoreCase("shortblock")) {
                List<ShortBlockVO> shortBlocks = getService(ParamModuleService.class).getShortBlockListByKeyword(keyword);
                if (shortBlocks == null) shortBlocks = new ArrayList<>();
                for (ShortBlockVO shortBlock : shortBlocks) {
                    List<ShortBlockVO.Param> sparams = getService(PartService.class).getShortBlockParamList(shortBlock.getBlockMetaSeq());
                    if (sparams == null) sparams = new ArrayList<>();
                    for (ShortBlockVO.Param sparam : sparams) {
                        ParamVO param = getService(ParamService.class).getParamBySeq(sparam.getParamSeq());
                        if (param != null) sparam.setParamInfo(param);
                    }
                    shortBlock.setParams(sparams);
                }
                return ResponseHelper.response(200, "Success - ParamModule Source Search", shortBlocks);
            }
            else if (sourceType.equalsIgnoreCase("dll")) {
                List<DLLVO> dlls = getService(ParamModuleService.class).getDLLListByKeyword(keyword);
                if (dlls == null) dlls = new ArrayList<>();
                for (DLLVO dll : dlls) {
                    dll.setParams(getService(DLLService.class).getDLLParamList(dll.getSeq()));
                }
                return ResponseHelper.response(200, "Success - ParamModule Source Search", dlls);
            }
            else if (sourceType.equalsIgnoreCase("parammodule")) {
                List<ParamModuleVO> paramModules = getService(ParamModuleService.class).getParamModuleListByKeyword(keyword);
                if (paramModules == null) paramModules = new ArrayList<>();
                for (ParamModuleVO paramModule : paramModules) {
                    paramModule.setEquations(getService(ParamModuleService.class).getParamModuleEqList(paramModule.getSeq()));
                }
                return ResponseHelper.response(200, "Success - ParamModule Source Search", paramModules);
            }

            throw new HandledServiceException(411, "지원하지 않는 소스 형식입니다.");
        }

        if (command.equals("source-list")) {
            CryptoField moduleSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "moduleSeq"))
                moduleSeq = CryptoField.decode(payload.get("moduleSeq").getAsString(), 0L);

            if (moduleSeq == null || moduleSeq.isEmpty())
                throw new HandledServiceException(411, "파라미터를 확인하세요.");

            List<ParamModuleVO.Source> sources = getService(ParamModuleService.class).getParamModuleSourceList(moduleSeq);
            if (sources == null) sources = new ArrayList<>();

            return ResponseHelper.response(200, "Success - ParamModule Source List", sources);
        }

        if (command.equals("save-source")) {
            CryptoField moduleSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "moduleSeq"))
                moduleSeq = CryptoField.decode(payload.get("moduleSeq").getAsString(), 0L);

            if (moduleSeq == null || moduleSeq.isEmpty())
                throw new HandledServiceException(411, "파라미터를 확인하세요.");

            ParamModuleVO paramModule = getService(ParamModuleService.class).getParamModuleBySeq(moduleSeq);
            if (paramModule == null)
                throw new HandledServiceException(411, "파라미터 모듈을 찾을 수 없습니다.");

            if (paramModule.isDeleted())
                throw new HandledServiceException(411, "이미 삭제된 프로젝트입니다.");

            if (paramModule.isReferenced())
                throw new HandledServiceException(411, "파라미터 모듈을 참조하는 프로젝트가 있습니다. 복사 후 새로 작업하세요.");

            // 기존 소스 삭제 함.
            getService(ParamModuleService.class).deleteParamModuleSourceByModuleSeq(moduleSeq);

            JsonArray jarrSources = null;
            if (!checkJsonEmpty(payload, "sources"))
                jarrSources = payload.get("sources").getAsJsonArray();

            List<String> sourceTypes = Arrays.asList("part", "shortblock", "dll", "parammodule");

            if (jarrSources.size() > 0) {
                for (int i = 0; i < jarrSources.size(); i++) {
                    JsonObject jobjSource = jarrSources.get(i).getAsJsonObject();
                    ParamModuleVO.Source moduleSource = ServerConstants.GSON.fromJson(jobjSource, ParamModuleVO.Source.class);
                    if (moduleSource == null) continue;
                    if (moduleSource.getSourceType() == null || moduleSource.getSourceType().isEmpty()
                            || !sourceTypes.contains(moduleSource.getSourceType())
                            || moduleSource.getSourceSeq() == null || moduleSource.getSourceSeq().isEmpty()) {
                        // 잘못된 소스 데이터는 스킵함.
                        continue;
                    }
                    moduleSource.setModuleSeq(moduleSeq);
                    getService(ParamModuleService.class).insertParamModuleSource(moduleSource);
                }
            }

            paramModule.setSources(getService(ParamModuleService.class).getParamModuleSourceList(moduleSeq));

            return ResponseHelper.response(200, "Success - ParamModule Save Source", paramModule);
        }

        if (command.equals("save-source-single")) {
            CryptoField moduleSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "moduleSeq"))
                moduleSeq = CryptoField.decode(payload.get("moduleSeq").getAsString(), 0L);

            if (moduleSeq == null || moduleSeq.isEmpty())
                throw new HandledServiceException(411, "파라미터를 확인하세요.");

            ParamModuleVO paramModule = getService(ParamModuleService.class).getParamModuleBySeq(moduleSeq);
            if (paramModule == null)
                throw new HandledServiceException(411, "파라미터 모듈을 찾을 수 없습니다.");

            if (paramModule.isDeleted())
                throw new HandledServiceException(411, "이미 삭제된 프로젝트입니다.");

            if (paramModule.isReferenced())
                throw new HandledServiceException(411, "파라미터 모듈을 참조하는 프로젝트가 있습니다. 복사 후 새로 작업하세요.");

            JsonObject jobjSource = null;
            if (!checkJsonEmpty(payload, "source"))
                jobjSource = payload.get("source").getAsJsonObject();

            if (jobjSource == null)
                throw new HandledServiceException(411, "저장 형식이 올바르지 않습니다.");

            List<String> sourceTypes = Arrays.asList("part", "shortblock", "dll", "parammodule");

            ParamModuleVO.Source moduleSource = ServerConstants.GSON.fromJson(jobjSource, ParamModuleVO.Source.class);
            if (moduleSource.getSourceType() == null || moduleSource.getSourceType().isEmpty()
                    || !sourceTypes.contains(moduleSource.getSourceType())
                    || moduleSource.getSourceSeq() == null || moduleSource.getSourceSeq().isEmpty()) {
                throw new HandledServiceException(411, "저장 형식이 올바르지 않습니다.");
            }
            moduleSource.setModuleSeq(moduleSeq);
            getService(ParamModuleService.class).insertParamModuleSource(moduleSource);

            paramModule.setSources(getService(ParamModuleService.class).getParamModuleSourceList(moduleSeq));

            return ResponseHelper.response(200, "Success - ParamModule Save Source Single", paramModule);
        }

        if (command.equals("eq-list")) {
            CryptoField moduleSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "moduleSeq"))
                moduleSeq = CryptoField.decode(payload.get("moduleSeq").getAsString(), 0L);

            if (moduleSeq == null || moduleSeq.isEmpty())
                throw new HandledServiceException(411, "파라미터를 확인하세요.");

            List<ParamModuleVO.Equation> equations = getService(ParamModuleService.class).getParamModuleEqList(moduleSeq);

            return ResponseHelper.response(200, "Success - ParamModule Eq List", equations);
        }

        if (command.equals("eq-data")) {

            // TODO : eq data loading

            return ResponseHelper.response(200, "Success - ParamModule Eq Data", "");
        }

        if (command.equals("save-eq")) {
            //
            // TODO : 여기서 계산.
            //
            return ResponseHelper.response(200, "Success - ParamModule Save Eq", "");
        }

        if (command.equals("save-eq-single")) {
            //
            // TODO : 계산 때문에 여기서 진행하기 어려움.
            //
            return ResponseHelper.response(200, "Success - ParamModule Save Eq Single", "");
        }

        if (command.equals("plot-list")) {
            CryptoField moduleSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "moduleSeq"))
                moduleSeq = CryptoField.decode(payload.get("moduleSeq").getAsString(), 0L);

            if (moduleSeq == null || moduleSeq.isEmpty())
                throw new HandledServiceException(411, "파라미터를 확인하세요.");

            List<ParamModuleVO.Plot> plots = getService(ParamModuleService.class).getParamModulePlotList(moduleSeq);

            return ResponseHelper.response(200, "Success - ParamModule Plot List", plots);
        }

        if (command.equals("plot-data")) {
            //
            // TODO : data loading from redis
            //
            return ResponseHelper.response(200, "Success - ParamModule Plot Data", "");
        }

        if (command.equals("save-plot")) {
            CryptoField moduleSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "moduleSeq"))
                moduleSeq = CryptoField.decode(payload.get("moduleSeq").getAsString(), 0L);

            if (moduleSeq == null || moduleSeq.isEmpty())
                throw new HandledServiceException(411, "파라미터를 확인하세요.");

            // plot은 레퍼런스가 없음.
            // 기존 플랏 삭제
            getService(ParamModuleService.class).deleteParamModulePlotByModuleSeq(moduleSeq);

            JsonArray jarrPlots = null;
            if (!checkJsonEmpty(payload, "plots"))
                jarrPlots = payload.get("plots").getAsJsonArray();

            List<ParamModuleVO.Plot> plots = new ArrayList<>();
            if (jarrPlots != null && jarrPlots.size() > 0) {
                for (int i = 0; i < jarrPlots.size(); i++) {
                    JsonObject jobjPlot = jarrPlots.get(i).getAsJsonObject();
                    if (checkJsonEmpty(jobjPlot, "plotName")) continue;
                    if (checkJsonEmpty(jobjPlot, "sources")) continue;

                    ParamModuleVO.Plot plot = new ParamModuleVO.Plot();
                    plot.setModuleSeq(moduleSeq);
                    plot.setPlotName(String64.decode(jobjPlot.get("plotName").getAsString()));
                    plot.setPlotOrder(plots.size() + 1);
                    plot.setCreatedAt(LongDate.now());
                    getService(ParamModuleService.class).insertParamModulePlot(plot);

                    plots.add(plot);

                    if (!checkJsonEmpty(jobjPlot, "dataProp")) {
                        Type type = new TypeToken<Map<String, String>>(){}.getType();
                        Map<String, String> dataProp = ServerConstants.GSON.fromJson(
                                jobjPlot.get("dataProp").getAsJsonObject(), type);
                        Set<String> keys = dataProp.keySet();
                        Iterator<String> iterKeys = keys.iterator();
                        while (iterKeys.hasNext()) {
                            String key = iterKeys.next();
                            String value = dataProp.get(key);

                            DataPropVO saveProp = new DataPropVO();
                            saveProp.setPropName(new String64(key));
                            saveProp.setPropValue(new String64(value));
                            saveProp.setReferenceType("plot");
                            saveProp.setReferenceKey(plot.getSeq());
                            saveProp.setUpdatedAt(LongDate.now());
                            getService(RawService.class).insertDataProp(saveProp);
                        }
                        plot.setDataProp(getService(RawService.class).getDataPropListToMap("plot", plot.getSeq()));
                    }

                    JsonArray jarrSources = jobjPlot.get("sources").getAsJsonArray();
                    List<String> sourceTypes = Arrays.asList("part", "shortblock", "dll", "parammodule");

                    plot.setPlotSourceList(new ArrayList<>());
                    if (jarrSources.size() > 0) {
                        for (int j = 0; j < jarrSources.size(); j++) {
                            JsonObject jobjSource = jarrSources.get(j).getAsJsonObject();
                            ParamModuleVO.Plot.Source plotSource = ServerConstants.GSON.fromJson(jobjSource, ParamModuleVO.Plot.Source.class);
                            if (plotSource == null) continue;
                            if (plotSource.getSourceType() == null || plotSource.getSourceType().isEmpty()
                                    || !sourceTypes.contains(plotSource.getSourceType())
                                    || plotSource.getSourceSeq() == null || plotSource.getSourceSeq().isEmpty()) {
                                // 잘못된 소스 데이터는 스킵함.
                                continue;
                            }
                            plotSource.setPlotSeq(plot.getSeq());
                            plotSource.setModuleSeq(moduleSeq);
                            getService(ParamModuleService.class).insertParamModulePlotSource(plotSource);

                            plot.getPlotSourceList().add(plotSource);
                        }
                    }
                }
            }

            return ResponseHelper.response(200, "Success - ParamModule Save Plot", plots);
        }

        if (command.equals("save-plot-single")) {
            CryptoField moduleSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "moduleSeq"))
                moduleSeq = CryptoField.decode(payload.get("moduleSeq").getAsString(), 0L);

            if (moduleSeq == null || moduleSeq.isEmpty())
                throw new HandledServiceException(411, "파라미터를 확인하세요.");

            if (checkJsonEmpty(payload, "plot"))
                throw new HandledServiceException(411, "저장할 데이터가 없습니다.");

            JsonObject jobjPlot = payload.get("plot").getAsJsonObject();
            if (checkJsonEmpty(jobjPlot, "plotName") || checkJsonEmpty(jobjPlot, "sources"))
                throw new HandledServiceException(411, "Plot 데이터 형식에 맞지 않습니다.");

            CryptoField plotSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(jobjPlot, "plotSeq"))
                plotSeq = CryptoField.decode(jobjPlot.get("plotSeq").getAsString(), 0L);

            List<ParamModuleVO.Plot> plots = getService(ParamModuleService.class).getParamModulePlotList(moduleSeq);
            if (plots == null) plots = new ArrayList<>();

            ParamModuleVO.Plot plot = null;
            if (plotSeq != null && !plotSeq.isEmpty()) {
                for (ParamModuleVO.Plot p : plots) {
                    if (p.getSeq().equals(plotSeq)) {
                        plot = p;
                        break;
                    }
                }
                if (plot == null)
                    throw new HandledServiceException(411, "해당 파라미터 모듈의 Plot 이 아닙니다.");
            }
            else {
                plot = new ParamModuleVO.Plot();
                plot.setModuleSeq(moduleSeq);
                plot.setPlotName(String64.decode(jobjPlot.get("plotName").getAsString()));
                plot.setPlotOrder(plots.size() + 1);
                plot.setCreatedAt(LongDate.now());
                getService(ParamModuleService.class).insertParamModulePlot(plot);

                plots.add(plot);
            }

            if (!checkJsonEmpty(jobjPlot, "dataProp")) {
                getService(RawService.class).deleteDataPropByType("plot", plot.getSeq());

                Type type = new TypeToken<Map<String, String>>(){}.getType();
                Map<String, String> dataProp = ServerConstants.GSON.fromJson(
                        jobjPlot.get("dataProp").getAsJsonObject(), type);
                Set<String> keys = dataProp.keySet();
                Iterator<String> iterKeys = keys.iterator();
                while (iterKeys.hasNext()) {
                    String key = iterKeys.next();
                    String value = dataProp.get(key);

                    DataPropVO saveProp = new DataPropVO();
                    saveProp.setPropName(new String64(key));
                    saveProp.setPropValue(new String64(value));
                    saveProp.setReferenceType("plot");
                    saveProp.setReferenceKey(plot.getSeq());
                    saveProp.setUpdatedAt(LongDate.now());
                    getService(RawService.class).insertDataProp(saveProp);
                }
                plot.setDataProp(getService(RawService.class).getDataPropListToMap("plot", plot.getSeq()));
            }

            JsonArray jarrSources = jobjPlot.get("sources").getAsJsonArray();
            List<String> sourceTypes = Arrays.asList("part", "shortblock", "dll", "parammodule");

            // 기존 소스 삭제.
            getService(ParamModuleService.class).deleteParamModulePlotSourceByPlotSeq(moduleSeq, plotSeq);

            plot.setPlotSourceList(new ArrayList<>());
            if (jarrSources.size() > 0) {
                for (int j = 0; j < jarrSources.size(); j++) {
                    JsonObject jobjSource = jarrSources.get(j).getAsJsonObject();
                    ParamModuleVO.Plot.Source plotSource = ServerConstants.GSON.fromJson(jobjSource, ParamModuleVO.Plot.Source.class);
                    if (plotSource == null) continue;
                    if (plotSource.getSourceType() == null || plotSource.getSourceType().isEmpty()
                            || !sourceTypes.contains(plotSource.getSourceType())
                            || plotSource.getSourceSeq() == null || plotSource.getSourceSeq().isEmpty()) {
                        // 잘못된 소스 데이터는 스킵함.
                        continue;
                    }
                    plotSource.setPlotSeq(plot.getSeq());
                    plotSource.setModuleSeq(moduleSeq);
                    getService(ParamModuleService.class).insertParamModulePlotSource(plotSource);

                    plot.getPlotSourceList().add(plotSource);
                }
            }

            return ResponseHelper.response(200, "Success - ParamModule Save Plot Single", plot);
        }

        if (command.equals("remove-plot")) {
            CryptoField moduleSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "moduleSeq"))
                moduleSeq = CryptoField.decode(payload.get("moduleSeq").getAsString(), 0L);

            if (moduleSeq == null || moduleSeq.isEmpty())
                throw new HandledServiceException(411, "파라미터를 확인하세요.");

            getService(ParamModuleService.class).deleteParamModulePlotByModuleSeq(moduleSeq);

            return ResponseHelper.response(200, "Success - ParamModule Remove Plot", "");
        }

        if (command.equals("remove-plot-single")) {
            CryptoField moduleSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "moduleSeq"))
                moduleSeq = CryptoField.decode(payload.get("moduleSeq").getAsString(), 0L);

            if (moduleSeq == null || moduleSeq.isEmpty())
                throw new HandledServiceException(411, "파라미터를 확인하세요.");

            CryptoField plotSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "plotSeq"))
                plotSeq = CryptoField.decode(payload.get("plotSeq").getAsString(), 0L);

            if (plotSeq == null || plotSeq.isEmpty())
                throw new HandledServiceException(411, "파라미터를 확인하세요.");

            getService(ParamModuleService.class).deleteParamModulePlot(plotSeq);

            return ResponseHelper.response(200, "Success - ParamModule Remove Plot Single", "");
        }

        throw new HandledServiceException(411, "명령이 정의되지 않았습니다.");
    }

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

    @RequestMapping(value = "/bin-table")
    @ResponseBody
    public Object apiBinTable(HttpServletRequest request, @PathVariable String serviceVersion,
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
            List<BinTableVO> binTables = getService(BinTableService.class).getBinTableList();
            List<BinTableVO> resultBinTables = new ArrayList<>();
            if (binTables != null && binTables.size() > 0) {
                for (BinTableVO binTable : binTables) {
                    resultBinTables.add(getService(BinTableService.class).getBinTableBySeq(binTable.getSeq()));
                }
            }
            return ResponseHelper.response(200, "Success - BinTable List", resultBinTables);
        }

        if (command.equals("detail")) {
            CryptoField binMetaSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "binMetaSeq"))
                binMetaSeq = CryptoField.decode(payload.get("binMetaSeq").getAsString(), 0L);

            if (binMetaSeq == null || binMetaSeq.isEmpty())
                throw new HandledServiceException(404, "파라미터를 확인하세요.");

            BinTableVO binTable = getService(BinTableService.class).getBinTableBySeq(binMetaSeq);

            return ResponseHelper.response(200, "Success - BinTable Detail", binTable);
        }

        if (command.equals("save")) {
            BinTableVO.SaveRequest saveRequest = ServerConstants.GSON.fromJson(payload, BinTableVO.SaveRequest.class);
            BinTableVO binTable = getService(BinTableService.class).saveBinTableMeta(user.getUid(), saveRequest);
            return ResponseHelper.response(200, "Success - BinTable saved", binTable);
        }

        if (command.equals("remove")) {
            CryptoField binMetaSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "binMetaSeq"))
                binMetaSeq = CryptoField.decode(payload.get("binMetaSeq").getAsString(), 0L);

            if (binMetaSeq == null || binMetaSeq.isEmpty())
                throw new HandledServiceException(404, "파라미터를 확인하세요.");

            getService(BinTableService.class).deleteBinTableMeta(binMetaSeq);

            return ResponseHelper.response(200, "Success - BinTable Deleted", "");
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
                try {
                    getService(ParamService.class).insertPresetParam(jobjParam);
                } catch(HandledServiceException hse) {
                    // skip insert
                }
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

        /*
        if (command.equals("data-add")) {
            DLLVO.Raw dllRaw = getService(DLLService.class).insertDLLData(user.getUid(), payload);
            return ResponseHelper.response(200, "Success - DLL Data Add", dllRaw);
        } */

        if (command.equals("data-add-bulk")) {
            List<DLLVO.Raw> dllRawData = getService(DLLService.class).insertDLLDataBulk(user.getUid(), payload);
            return ResponseHelper.response(200, "Success - DLL Data Add Bulk", dllRawData);
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
            }
            else {
                // row 조건에 따라서 지우기, 해당 row 전체 삭제만 있음.
                if (jarrRows != null && jarrRows.size() == 2) {
                    if (jarrRows.get(1).getAsInt() == 0)
                        jarrRows.set(1, new JsonPrimitive(Integer.MAX_VALUE));

                    getService(DLLService.class).deleteDLLDataByRow(dllSeq,
                            jarrRows.get(0).getAsInt(), jarrRows.get(1).getAsInt());
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
                Map<String, ParamVO> grtMap = new LinkedHashMap<>();
                Map<String, ParamVO> fltpMap = new LinkedHashMap<>();
                Map<String, ParamVO> fltsMap = new LinkedHashMap<>();
                for (ParamVO param : presetParams) {
                    grtMap.put(param.getGrtKey(), param);
                    fltpMap.put(param.getFltpKey(), param);
                    fltsMap.put(param.getFltsKey(), param);
                }

                String[] splitParam = headerRow.trim().split(",");

                for (int i = 0; i < splitParam.length; i++) {
                    String p = splitParam[i];
                    if (p.equalsIgnoreCase("date")) continue;

                    ParamVO pi = null;
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
                if (importFilePath.contains("C:\\")
                    || importFilePath.contains("c:\\")) {
                    importFilePath = importFilePath.toLowerCase(Locale.ROOT);
                    importFilePath = importFilePath.replaceAll("\\\\", "/");
                    importFilePath = importFilePath.replaceAll("c:/", staticLocation.substring("file:".length()));
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

    @RequestMapping(value = "/part/d")
    public void apiPartDownload(HttpServletRequest request, HttpServletResponse response,
                                      @PathVariable String serviceVersion,
                                      @RequestBody JsonObject payload, Authentication authentication) throws HandledServiceException {
        UserVO user = getService(UserService.class).getUser("admin@dynarap@dynarap");

        if (checkJsonEmpty(payload, "command"))
            throw new HandledServiceException(404, "파라미터를 확인하세요.");

        String command = payload.get("command").getAsString();

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
                    Long paramKey = CryptoField.decode(jarrParams.get(i).getAsString(), 0L).originOf();
                    ParamVO param = getService(ParamService.class).getPresetParamBySeq(new CryptoField(paramKey));
                    if (param == null) {
                        param = getService(ParamService.class).getNotMappedParamBySeq(
                                new CryptoField(paramKey));
                        if (param == null) continue;
                    }
                    params.add(param);
                }
            }

            List<ParamVO> notMappedParams = getService(ParamService.class).getNotMappedParams(partInfo.getUploadSeq());
            if (notMappedParams != null && notMappedParams.size() > 0) {
                for (ParamVO p : notMappedParams)
                    params.add(p);
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

            LinkedHashMap<String, List<Double>> rowData = new LinkedHashMap<>();
            if (julianFrom == null || julianFrom.isEmpty() || julianTo == null || julianTo.isEmpty()) {
                jobjResult.add("paramSet", ServerConstants.GSON.toJsonTree(params));
                jobjResult.add("julianSet", ServerConstants.GSON.toJsonTree(new ArrayList<String>()));
                jobjResult.add("data", ServerConstants.GSON.toJsonTree(rowData));
            }
            else {
                Set<String> listSet = zsetOps.rangeByScore("P" + partInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);
                if (listSet == null || listSet.size() == 0) {
                    jobjResult.add("paramSet", ServerConstants.GSON.toJsonTree(params));
                    jobjResult.add("julianSet", ServerConstants.GSON.toJsonTree(new ArrayList<String>()));
                    jobjResult.add("data", ServerConstants.GSON.toJsonTree(rowData));
                }
                else {
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

                    rowData = new LinkedHashMap<>();
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
                }
            }

            StringBuilder sbOut = new StringBuilder();
            sbOut.append("DATE,");
            for (ParamVO p : params) sbOut.append(p.getParamKey().replaceAll(",", "_")).append(",");
            sbOut.append("\n");
            sbOut.append(",");
            for (ParamVO p : params) sbOut.append(",");
            sbOut.append("\n");

            // flush data
            Set<String> keys = rowData.keySet();
            Iterator<String> iterKeys = keys.iterator();
            while (iterKeys.hasNext()) {
                String time = iterKeys.next();
                List<Double> row = rowData.get(time);
                sbOut.append(time).append(",");
                for (Double d : row) sbOut.append(d).append(",");
                sbOut.append("\n");
            }

            response.setHeader("Content-Type", "application/octet-stream");
            response.addHeader("Content-Disposition",
                    "attachment; filename=\"part_" + System.currentTimeMillis() + ".csv\";");
            response.addHeader("Content-Transfer-Encoding", "binary");

            try {
                byte[] outBuf = sbOut.toString().getBytes(StandardCharsets.UTF_8);
                OutputStream os = response.getOutputStream();
                os.write(outBuf, 0, outBuf.length);
                os.flush();
            } catch(IOException ioe) {
                throw new HandledServiceException(411, ioe.getMessage());
            }
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
                    Long paramKey = CryptoField.decode(jarrParams.get(i).getAsString(), 0L).originOf();
                    ParamVO param = getService(ParamService.class).getPresetParamBySeq(new CryptoField(paramKey));
                    if (param == null) {
                        param = getService(ParamService.class).getNotMappedParamBySeq(
                                new CryptoField(paramKey));
                        if (param == null) continue;
                    }
                    params.add(param);
                }
            }

            List<ParamVO> notMappedParams = getService(ParamService.class).getNotMappedParams(partInfo.getUploadSeq());
            if (notMappedParams != null && notMappedParams.size() > 0) {
                for (ParamVO p : notMappedParams)
                    params.add(p);
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

            StringBuilder sbOut = new StringBuilder();
            sbOut.append("DATE,");
            for (ParamVO p : params) sbOut.append(p.getParamKey().replaceAll(",", "_")).append(",");
            sbOut.append("\n");
            sbOut.append(",");
            for (ParamVO p : params) sbOut.append(",");
            sbOut.append("\n");

            // flush data
            Set<String> keys = paramData.keySet();
            Iterator<String> iterKeys = keys.iterator();
            while (iterKeys.hasNext()) {
                String time = iterKeys.next();
                List<Double> row = paramData.get(time);
                sbOut.append(time).append(",");
                for (Double d : row) sbOut.append(d).append(",");
                sbOut.append("\n");
            }

            response.setHeader("Content-Type", "application/octet-stream");
            response.addHeader("Content-Disposition",
                    "attachment; filename=\"part_" + System.currentTimeMillis() + ".csv\";");
            response.addHeader("Content-Transfer-Encoding", "binary");

            try {
                byte[] outBuf = sbOut.toString().getBytes(StandardCharsets.UTF_8);
                OutputStream os = response.getOutputStream();
                os.write(outBuf, 0, outBuf.length);
                os.flush();
            } catch(IOException ioe) {
                throw new HandledServiceException(411, ioe.getMessage());
            }
        }
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

        if (command.equals("param-list")) {
            CryptoField partSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "partSeq"))
                partSeq = CryptoField.decode(payload.get("partSeq").getAsString(), 0L);

            PartVO partInfo = getService(PartService.class).getPartBySeq(partSeq);

            JsonObject jobjParams = new JsonObject();
            List<ParamVO> resultParams = new ArrayList<>();
            List<CryptoField> paramList = getService(ParamService.class).getPartParamList(partSeq);
            if (paramList == null) paramList = new ArrayList<>();

            jobjParams.add("paramSet", ServerConstants.GSON.toJsonTree(paramList));

            for (CryptoField seq : paramList) {
                ParamVO param = getService(ParamService.class).getPresetParamBySeq(seq);
                if (param == null) {
                    param = getService(ParamService.class).getNotMappedParamBySeq(seq);
                    if (param == null) continue;
                }
                resultParams.add(param);
            }

            List<ParamVO> notMappedParams = getService(ParamService.class).getNotMappedParams(partInfo.getUploadSeq());
            if (notMappedParams != null && notMappedParams.size() > 0) {
                for (ParamVO p : notMappedParams)
                    resultParams.add(p);
            }

            jobjParams.add("paramData", ServerConstants.GSON.toJsonTree(resultParams));

            return ResponseHelper.response(200, "Success - Part Info", jobjParams);
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
                    Long paramKey = CryptoField.decode(jarrParams.get(i).getAsString(), 0L).originOf();
                    ParamVO param = getService(ParamService.class).getPresetParamBySeq(new CryptoField(paramKey));
                    if (param == null) {
                        param = getService(ParamService.class).getNotMappedParamBySeq(
                                new CryptoField(paramKey));
                        if (param == null) continue;
                    }
                    params.add(param);
                }
            }

            List<ParamVO> notMappedParams = getService(ParamService.class).getNotMappedParams(partInfo.getUploadSeq());
            if (notMappedParams != null && notMappedParams.size() > 0) {
                for (ParamVO p : notMappedParams)
                    params.add(p);
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
                    Long paramKey = CryptoField.decode(jarrParams.get(i).getAsString(), 0L).originOf();
                    ParamVO param = getService(ParamService.class).getPresetParamBySeq(new CryptoField(paramKey));
                    if (param == null) {
                        param = getService(ParamService.class).getNotMappedParamBySeq(
                                new CryptoField(paramKey));
                        if (param == null) continue;
                    }
                    params.add(param);
                }
            }

            List<ParamVO> notMappedParams = getService(ParamService.class).getNotMappedParams(partInfo.getUploadSeq());
            if (notMappedParams != null && notMappedParams.size() > 0) {
                for (ParamVO p : notMappedParams)
                    params.add(p);
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

    @RequestMapping(value = "/shortblock/d")
    public void apiShortBlockDownload(HttpServletRequest request, HttpServletResponse response,
                                @PathVariable String serviceVersion,
                                @RequestBody JsonObject payload, Authentication authentication) throws HandledServiceException {
        UserVO user = getService(UserService.class).getUser("admin@dynarap@dynarap");

        if (checkJsonEmpty(payload, "command"))
            throw new HandledServiceException(404, "파라미터를 확인하세요.");

        String command = payload.get("command").getAsString();

        if (command.equals("row-data")) {
            CryptoField blockSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "blockSeq"))
                blockSeq = CryptoField.decode(payload.get("blockSeq").getAsString(), 0L);

            String filterType = "N";
            if (!checkJsonEmpty(payload, "filterType"))
                filterType = payload.get("filterType").getAsString();

            ShortBlockVO blockInfo = getService(PartService.class).getShortBlockBySeq(blockSeq);
            PartVO partInfo = getService(PartService.class).getPartBySeq(blockInfo.getPartSeq());

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
                    Long paramKey = CryptoField.decode(jarrParams.get(i).getAsString(), 0L).originOf();
                    ShortBlockVO.Param sparam = getService(ParamService.class).getShortBlockParamBySeq(new CryptoField(paramKey));
                    ParamVO param = getService(ParamService.class).getParamBySeq(sparam.getParamSeq());
                    if (param != null) {
                        param.setReferenceSeq(sparam.getUnionParamSeq());
                        params.add(param);
                    }
                }

                List<ParamVO> notMappedParams = getService(ParamService.class).getNotMappedParams(partInfo.getUploadSeq());
                if (notMappedParams != null && notMappedParams.size() > 0) {
                    for (ParamVO p : notMappedParams)
                        params.add(p);
                }
            }

            JsonArray jarrJulian = payload.get("julianRange").getAsJsonArray();
            String julianFrom = jarrJulian.get(0).getAsString();
            if (julianFrom == null || julianFrom.isEmpty()) {
                Set<String> listSet = zsetOps.rangeByScore("S" + blockInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);
                if (listSet != null && listSet.size() > 0)
                    julianFrom = listSet.iterator().next();
            }
            String julianTo = jarrJulian.get(1).getAsString();
            if (julianTo == null || julianTo.isEmpty()) {
                Set<String> listSet = zsetOps.reverseRangeByScore("S" + blockInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);
                if (listSet != null && listSet.size() > 0)
                    julianTo = listSet.iterator().next();
            }

            LinkedHashMap<String, List<Double>> rowData = new LinkedHashMap<>();
            if (julianFrom == null || julianFrom.isEmpty() || julianTo == null || julianTo.isEmpty()) {
                jobjResult.add("paramSet", ServerConstants.GSON.toJsonTree(params));
                jobjResult.add("julianSet", ServerConstants.GSON.toJsonTree(new ArrayList<String>()));
                jobjResult.add("data", ServerConstants.GSON.toJsonTree(rowData));
            }
            else {
                Set<String> listSet = zsetOps.rangeByScore("S" + blockInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);
                if (listSet == null || listSet.size() == 0) {
                    jobjResult.add("paramSet", ServerConstants.GSON.toJsonTree(params));
                    jobjResult.add("julianSet", ServerConstants.GSON.toJsonTree(new ArrayList<String>()));
                    jobjResult.add("data", ServerConstants.GSON.toJsonTree(rowData));
                }
                else {
                    String julianStart = listSet.iterator().next();
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

                    rowData = new LinkedHashMap<>();
                    for (ParamVO p : params) {
                        listSet = zsetOps.rangeByScore(
                                "S" + blockInfo.getSeq().originOf() + "." + filterType + p.getReferenceSeq(), startRowAt + rankFrom, startRowAt + rankTo);

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
                }
            }

            StringBuilder sbOut = new StringBuilder();
            sbOut.append("DATE,");
            for (ParamVO p : params) sbOut.append(p.getParamKey().replaceAll(",", "_")).append(",");
            sbOut.append("\n");
            sbOut.append(",");
            for (ParamVO p : params) sbOut.append(",");
            sbOut.append("\n");

            // flush data
            Set<String> keys = rowData.keySet();
            Iterator<String> iterKeys = keys.iterator();
            while (iterKeys.hasNext()) {
                String time = iterKeys.next();
                List<Double> row = rowData.get(time);
                sbOut.append(time).append(",");
                for (Double d : row) sbOut.append(d).append(",");
                sbOut.append("\n");
            }

            response.setHeader("Content-Type", "application/octet-stream");
            response.addHeader("Content-Disposition",
                    "attachment; filename=\"shortblock_" + System.currentTimeMillis() + ".csv\";");
            response.addHeader("Content-Transfer-Encoding", "binary");

            try {
                byte[] outBuf = sbOut.toString().getBytes(StandardCharsets.UTF_8);
                OutputStream os = response.getOutputStream();
                os.write(outBuf, 0, outBuf.length);
                os.flush();
            } catch(IOException ioe) {
                throw new HandledServiceException(411, ioe.getMessage());
            }
        }

        if (command.equals("column-data")) {
            CryptoField blockSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "blockSeq"))
                blockSeq = CryptoField.decode(payload.get("blockSeq").getAsString(), 0L);

            String filterType = "N";
            if (!checkJsonEmpty(payload, "filterType"))
                filterType = payload.get("filterType").getAsString();

            ShortBlockVO blockInfo = getService(PartService.class).getShortBlockBySeq(blockSeq);
            PartVO partInfo = getService(PartService.class).getPartBySeq(blockInfo.getPartSeq());

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
                    Long paramKey = CryptoField.decode(jarrParams.get(i).getAsString(), 0L).originOf();
                    ShortBlockVO.Param sparam = getService(ParamService.class).getShortBlockParamBySeq(new CryptoField(paramKey));
                    ParamVO param = getService(ParamService.class).getParamBySeq(sparam.getParamSeq());
                    if (param != null) {
                        param.setReferenceSeq(sparam.getUnionParamSeq());
                        params.add(param);
                    }
                }

                List<ParamVO> notMappedParams = getService(ParamService.class).getNotMappedParams(partInfo.getUploadSeq());
                if (notMappedParams != null && notMappedParams.size() > 0) {
                    for (ParamVO p : notMappedParams)
                        params.add(p);
                }
            }

            List<String> julianData = new ArrayList<>();
            LinkedHashMap<String, List<Double>> paramData = new LinkedHashMap<>();
            for (ParamVO p : params) {
                Set<String> listSet = zsetOps.rangeByScore(
                        "S" + blockInfo.getSeq().originOf() + "." + filterType + p.getReferenceSeq(), 0, Integer.MAX_VALUE);
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

            StringBuilder sbOut = new StringBuilder();
            sbOut.append("DATE,");
            for (ParamVO p : params) sbOut.append(p.getParamKey().replaceAll(",", "_")).append(",");
            sbOut.append("\n");
            sbOut.append(",");
            for (ParamVO p : params) sbOut.append(",");
            sbOut.append("\n");

            // flush data
            Set<String> keys = paramData.keySet();
            Iterator<String> iterKeys = keys.iterator();
            while (iterKeys.hasNext()) {
                String time = iterKeys.next();
                List<Double> row = paramData.get(time);
                sbOut.append(time).append(",");
                for (Double d : row) sbOut.append(d).append(",");
                sbOut.append("\n");
            }

            response.setHeader("Content-Type", "application/octet-stream");
            response.addHeader("Content-Disposition",
                    "attachment; filename=\"shortblock_" + System.currentTimeMillis() + ".csv\";");
            response.addHeader("Content-Transfer-Encoding", "binary");

            try {
                byte[] outBuf = sbOut.toString().getBytes(StandardCharsets.UTF_8);
                OutputStream os = response.getOutputStream();
                os.write(outBuf, 0, outBuf.length);
                os.flush();
            } catch(IOException ioe) {
                throw new HandledServiceException(411, ioe.getMessage());
            }
        }
    }

    @RequestMapping(value = "/shortblock")
    @ResponseBody
    public Object apiShortBlock(HttpServletRequest request, HttpServletResponse response,
                                @PathVariable String serviceVersion,
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

        if (command.equals("param-list")) {
            CryptoField blockSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "blockSeq"))
                blockSeq = CryptoField.decode(payload.get("blockSeq").getAsString(), 0L);

            ShortBlockVO blockInfo = getService(PartService.class).getShortBlockBySeq(blockSeq);
            PartVO partInfo = getService(PartService.class).getPartBySeq(blockInfo.getPartSeq());

            JsonObject jobjParams = new JsonObject();
            List<ParamVO> resultParams = new ArrayList<>();
            List<CryptoField> paramList = getService(ParamService.class).getShortBlockParamList(blockSeq);
            if (paramList == null) paramList = new ArrayList<>();

            List<ParamVO> presetParams = getService(ParamService.class).getPresetParamList(
                    partInfo.getPresetPack(), partInfo.getPresetSeq(), CryptoField.LZERO, CryptoField.LZERO, 1, 9999);
            if (presetParams == null) presetParams = new ArrayList<>();

            for (CryptoField seq : paramList) {
                ShortBlockVO.Param sparam = getService(ParamService.class).getShortBlockParamBySeq(seq);
                ParamVO param = null;
                for (ParamVO p : presetParams) {
                    if (p.getParamPack().equals(sparam.getParamPack()) && p.getSeq().equals(sparam.getParamSeq())) {
                        param = p;
                        break;
                    }
                }
                if (param != null) resultParams.add(param);
            }

            List<ParamVO> notMappedParams = getService(ParamService.class).getNotMappedParams(partInfo.getUploadSeq());
            if (notMappedParams != null && notMappedParams.size() > 0) {
                for (ParamVO p : notMappedParams) {
                    paramList.add(p.getSeq());
                    resultParams.add(p);
                }
            }

            jobjParams.add("paramSet", ServerConstants.GSON.toJsonTree(paramList));
            jobjParams.add("paramData", ServerConstants.GSON.toJsonTree(resultParams));

            return ResponseHelper.response(200, "Success - Shortblock Param List", jobjParams);
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

            String filterType = "N";
            if (!checkJsonEmpty(payload, "filterType"))
                filterType = payload.get("filterType").getAsString();

            ShortBlockVO blockInfo = getService(PartService.class).getShortBlockBySeq(blockSeq);
            PartVO partInfo = getService(PartService.class).getPartBySeq(blockInfo.getPartSeq());

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
                    Long paramKey = CryptoField.decode(jarrParams.get(i).getAsString(), 0L).originOf();
                    ShortBlockVO.Param sparam = getService(ParamService.class).getShortBlockParamBySeq(new CryptoField(paramKey));
                    ParamVO param = getService(ParamService.class).getParamBySeq(sparam.getParamSeq());
                    if (param != null) {
                        param.setReferenceSeq(sparam.getUnionParamSeq());
                        params.add(param);
                    }
                }

                List<ParamVO> notMappedParams = getService(ParamService.class).getNotMappedParams(partInfo.getUploadSeq());
                if (notMappedParams != null && notMappedParams.size() > 0) {
                    for (ParamVO p : notMappedParams)
                        params.add(p);
                }
            }

            JsonArray jarrJulian = payload.get("julianRange").getAsJsonArray();
            String julianFrom = jarrJulian.get(0).getAsString();
            if (julianFrom == null || julianFrom.isEmpty()) {
                Set<String> listSet = zsetOps.rangeByScore("S" + blockInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);
                if (listSet != null && listSet.size() > 0)
                    julianFrom = listSet.iterator().next();
            }
            String julianTo = jarrJulian.get(1).getAsString();
            if (julianTo == null || julianTo.isEmpty()) {
                Set<String> listSet = zsetOps.reverseRangeByScore("S" + blockInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);
                if (listSet != null && listSet.size() > 0)
                    julianTo = listSet.iterator().next();
            }

            if (julianFrom == null || julianFrom.isEmpty() || julianTo == null || julianTo.isEmpty()) {
                jobjResult.add("paramSet", ServerConstants.GSON.toJsonTree(params));
                jobjResult.add("julianSet", ServerConstants.GSON.toJsonTree(new ArrayList<String>()));
                jobjResult.add("data", ServerConstants.GSON.toJsonTree(new LinkedHashMap<String, List<Double>>()));
                return ResponseHelper.response(200, "Success - rowData", jobjResult);
            }

            Set<String> listSet = zsetOps.rangeByScore("S" + blockInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);
            if (listSet == null || listSet.size() == 0) {
                jobjResult.add("paramSet", ServerConstants.GSON.toJsonTree(params));
                jobjResult.add("julianSet", ServerConstants.GSON.toJsonTree(new ArrayList<String>()));
                jobjResult.add("data", ServerConstants.GSON.toJsonTree(new LinkedHashMap<String, List<Double>>()));
                return ResponseHelper.response(200, "Success - rowData", jobjResult);
            }

            String julianStart = listSet.iterator().next();
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
                listSet = zsetOps.rangeByScore(
                        "S" + blockInfo.getSeq().originOf() + "." + filterType + p.getReferenceSeq(), startRowAt + rankFrom, startRowAt + rankTo);

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

            String filterType = "N";
            if (!checkJsonEmpty(payload, "filterType"))
                filterType = payload.get("filterType").getAsString();

            ShortBlockVO blockInfo = getService(PartService.class).getShortBlockBySeq(blockSeq);
            PartVO partInfo = getService(PartService.class).getPartBySeq(blockInfo.getPartSeq());

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
                    Long paramKey = CryptoField.decode(jarrParams.get(i).getAsString(), 0L).originOf();
                    ShortBlockVO.Param sparam = getService(ParamService.class).getShortBlockParamBySeq(new CryptoField(paramKey));
                    ParamVO param = getService(ParamService.class).getParamBySeq(sparam.getParamSeq());
                    if (param != null) {
                        param.setReferenceSeq(sparam.getUnionParamSeq());
                        params.add(param);
                    }
                }

                List<ParamVO> notMappedParams = getService(ParamService.class).getNotMappedParams(partInfo.getUploadSeq());
                if (notMappedParams != null && notMappedParams.size() > 0) {
                    for (ParamVO p : notMappedParams)
                        params.add(p);
                }
            }

            List<String> julianData = new ArrayList<>();
            LinkedHashMap<String, List<Double>> paramData = new LinkedHashMap<>();
            for (ParamVO p : params) {
                Set<String> listSet = zsetOps.rangeByScore(
                        "S" + blockInfo.getSeq().originOf() + "." + filterType + p.getReferenceSeq(), 0, Integer.MAX_VALUE);
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

    @RequestMapping(value = "/data-prop")
    @ResponseBody
    public Object apiDataProp(HttpServletRequest request, HttpServletResponse response,
                                @PathVariable String serviceVersion,
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
            CryptoField referenceKey = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "referenceKey"))
                referenceKey = CryptoField.decode(payload.get("referenceKey").getAsString(), 0L);

            String referenceType = "";
            if (!checkJsonEmpty(payload, "referenceType"))
                referenceType = payload.get("referenceType").getAsString();

            if (referenceType == null || referenceType.isEmpty() || referenceKey.isEmpty())
                throw new HandledServiceException(411, "조회를 위한 필수값이 없습니다.");

            List<DataPropVO> dataPropList = getService(RawService.class).getDataPropList(referenceType, referenceKey);

            return ResponseHelper.response(200, "Success - DataProp list", dataPropList);
        }

        if (command.equals("add")) {
            CryptoField referenceKey = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "referenceKey"))
                referenceKey = CryptoField.decode(payload.get("referenceKey").getAsString(), 0L);

            String referenceType = "";
            if (!checkJsonEmpty(payload, "referenceType"))
                referenceType = payload.get("referenceType").getAsString();

            String64 propName = null;
            if (!checkJsonEmpty(payload, "propName"))
                propName = String64.decode(payload.get("propName").getAsString());

            if (referenceType == null || referenceType.isEmpty() || referenceKey.isEmpty())
                throw new HandledServiceException(411, "조회를 위한 필수값이 없습니다.");

            String64 propValue = null;
            if (!checkJsonEmpty(payload, "propValue"))
                propValue = String64.decode(payload.get("propValue").getAsString());

            DataPropVO dataProp = new DataPropVO();
            dataProp.setPropName(propName);
            dataProp.setPropValue(propValue);
            dataProp.setReferenceKey(referenceKey);
            dataProp.setReferenceType(referenceType);
            dataProp.setUpdatedAt(LongDate.now());
            getService(RawService.class).insertDataProp(dataProp);

            return ResponseHelper.response(200, "Success - DataProp add", dataProp);
        }

        if (command.equals("update")) {
            CryptoField referenceKey = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "referenceKey"))
                referenceKey = CryptoField.decode(payload.get("referenceKey").getAsString(), 0L);

            String referenceType = "";
            if (!checkJsonEmpty(payload, "referenceType"))
                referenceType = payload.get("referenceType").getAsString();

            String64 propName = null;
            if (!checkJsonEmpty(payload, "propName"))
                propName = String64.decode(payload.get("propName").getAsString());

            if (referenceType == null || referenceType.isEmpty() || referenceKey.isEmpty())
                throw new HandledServiceException(411, "조회를 위한 필수값이 없습니다.");

            DataPropVO dataProp = getService(RawService.class).getDataPropByName(
                    referenceType, referenceKey, propName);
            if (dataProp == null)
                throw new HandledServiceException(411, "등록되지 않은 이름입니다.");

            String64 propValue = null;
            if (!checkJsonEmpty(payload, "propValue"))
                propValue = String64.decode(payload.get("propValue").getAsString());

            dataProp.setPropValue(propValue);
            dataProp.setUpdatedAt(LongDate.now());
            getService(RawService.class).updateDataProp(dataProp);

            return ResponseHelper.response(200, "Success - DataProp update", dataProp);
        }

        if (command.equals("remove-all")) {
            CryptoField referenceKey = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "referenceKey"))
                referenceKey = CryptoField.decode(payload.get("referenceKey").getAsString(), 0L);

            String referenceType = "";
            if (!checkJsonEmpty(payload, "referenceType"))
                referenceType = payload.get("referenceType").getAsString();

            getService(RawService.class).deleteDataPropByType(referenceType, referenceKey);

            return ResponseHelper.response(200, "Success - DataProp remove by all", "");
        }

        if (command.equals("remove-name")) {
            CryptoField referenceKey = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "referenceKey"))
                referenceKey = CryptoField.decode(payload.get("referenceKey").getAsString(), 0L);

            String referenceType = "";
            if (!checkJsonEmpty(payload, "referenceType"))
                referenceType = payload.get("referenceType").getAsString();

            String64 propName = null;
            if (!checkJsonEmpty(payload, "propName"))
                propName = String64.decode(payload.get("propName").getAsString());

            getService(RawService.class).deleteDataPropByName(referenceType, referenceKey, propName);

            return ResponseHelper.response(200, "Success - DataProp remove by name", "");
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
