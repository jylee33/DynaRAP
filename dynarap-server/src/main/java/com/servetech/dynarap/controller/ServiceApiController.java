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
import org.jsoup.internal.StringUtil;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.core.io.FileSystemResource;
import org.springframework.data.redis.core.HashOperations;
import org.springframework.data.redis.core.ListOperations;
import org.springframework.data.redis.core.ZSetOperations;
import org.springframework.security.core.Authentication;
import org.springframework.stereotype.Controller;
import org.springframework.util.StreamUtils;
import org.springframework.web.bind.annotation.*;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.*;
import java.lang.reflect.Type;
import java.nio.charset.StandardCharsets;
import java.util.*;
import java.util.zip.ZipEntry;
import java.util.zip.ZipOutputStream;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

@Controller
@RequestMapping(value = "/api/{serviceVersion}")
public class ServiceApiController extends ApiController {
    private static final Logger logger = LoggerFactory.getLogger(ServiceApiController.class);

    @Value("${neoulsoft.auth.client-id}")
    private String authClientId;

    @Value("${neoulsoft.auth.client-secret}")
    private String authClientSecret;

    @Value("${dynarap.process.path}")
    private String processPath;

    @Value("${static.resource.location}")
    private String staticLocation;

    private EquationHelper equationHelper = new EquationHelper(
            getOperation("list"),
            getOperation("hash"),
            getOperation("zset"));

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
                    } else {
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
            } else {
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
            } else {
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
            paramModule.setModuleName(new String64(oldParamModule.getModuleName().originOf() + "_Copy"));
            getService(ParamModuleService.class).insertParamModule(paramModule);

            if (oldParamModule.getDataProp() != null && oldParamModule.getDataProp().size() > 0) {
                paramModule.setDataProp(new HashMap<>());

                Set<String> keys = oldParamModule.getDataProp().keySet();
                Iterator<String> iterKeys = keys.iterator();
                while (iterKeys.hasNext()) {
                    String key = iterKeys.next();
                    String value = oldParamModule.getDataProp().get(key);

                    DataPropVO dataProp = new DataPropVO();
                    dataProp.setPropName(new String64(key));
                    dataProp.setPropValue(new String64(value));
                    dataProp.setReferenceType("parammodule");
                    dataProp.setReferenceKey(paramModule.getSeq());
                    dataProp.setUpdatedAt(LongDate.now());
                    getService(RawService.class).insertDataProp(dataProp);
                    paramModule.getDataProp().put(key, value);
                }
            }

            // source
            // eq
            // plot
            // plot sources
            // plot series
            // plot save


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

//            if (keyword == null || keyword.isEmpty())
//                throw new HandledServiceException(411, "검색 키워드를 입력하세요.");

            if (sourceType.equalsIgnoreCase("part")) {
                List<PartVO> parts = getService(ParamModuleService.class).getPartListByKeyword(keyword);
                if (parts == null) parts = new ArrayList<>();

                List<PartVO> availParts = new ArrayList<>();

                for (PartVO part : parts) {
                    RawVO.Upload rawUpload = getService(RawService.class).getUploadBySeq(part.getUploadSeq());
                    if (rawUpload == null) continue;

                    part.setParams(getService(ParamService.class).getPresetParamListBySource(part.getPresetPack(), part.getPresetSeq()));

                    part.setUseTime("julian");
                    if (rawUpload.getDataType().equalsIgnoreCase("adams") || rawUpload.getDataType().equalsIgnoreCase("zaero"))
                        part.setUseTime("offset");

                    availParts.add(part);

                    if (part.getParams() == null || part.getParams().size() == 0) continue;
                    for (PresetVO.Param pparam : part.getParams()) {
                        pparam.setParamInfo(getService(ParamService.class).getParamBySeq(pparam.getParamSeq()));
                    }
                }
                return ResponseHelper.response(200, "Success - ParamModule Source Search", availParts);
            } else if (sourceType.equalsIgnoreCase("shortblock")) {
                List<ShortBlockVO> shortBlocks = getService(ParamModuleService.class).getShortBlockListByKeyword(keyword);
                if (shortBlocks == null) shortBlocks = new ArrayList<>();

                List<ShortBlockVO> availBlocks = new ArrayList<>();

                for (ShortBlockVO shortBlock : shortBlocks) {
                    PartVO partInfo = getService(PartService.class).getPartBySeq(shortBlock.getPartSeq());
                    RawVO.Upload rawUpload = getService(RawService.class).getUploadBySeq(partInfo.getUploadSeq());
                    if (rawUpload == null) continue;

                    shortBlock.setUseTime("julian");
                    if (rawUpload.getDataType().equalsIgnoreCase("adams") || rawUpload.getDataType().equalsIgnoreCase("zaero"))
                        shortBlock.setUseTime("offset");

                    availBlocks.add(shortBlock);

                    List<ShortBlockVO.Param> sparams = getService(PartService.class).getShortBlockParamList(shortBlock.getBlockMetaSeq());
                    if (sparams == null) sparams = new ArrayList<>();
                    for (ShortBlockVO.Param sparam : sparams) {
                        ParamVO param = getService(ParamService.class).getParamBySeq(sparam.getParamSeq());
                        if (param != null) sparam.setParamInfo(param);
                        sparam.setParamSearchSeq(new CryptoField(sparam.getUnionParamSeq()));
                    }
                    shortBlock.setParams(sparams);
                }
                return ResponseHelper.response(200, "Success - ParamModule Source Search", availBlocks);
            } else if (sourceType.equalsIgnoreCase("dll")) {
                List<DLLVO> dlls = getService(ParamModuleService.class).getDLLListByKeyword(keyword);
                if (dlls == null) dlls = new ArrayList<>();
                for (DLLVO dll : dlls) {
                    dll.setParams(getService(DLLService.class).getDLLParamList(dll.getSeq()));
                }
                return ResponseHelper.response(200, "Success - ParamModule Source Search", dlls);
            } else if (sourceType.equalsIgnoreCase("parammodule")) {
                List<ParamModuleVO> paramModules = getService(ParamModuleService.class).getParamModuleListByKeyword(keyword);
                if (paramModules == null) paramModules = new ArrayList<>();
                for (ParamModuleVO paramModule : paramModules) {
                    paramModule.setEquations(getService(ParamModuleService.class).getParamModuleEqList(paramModule.getSeq()));
                }
                return ResponseHelper.response(200, "Success - ParamModule Source Search", paramModules);
            } else if (sourceType.equalsIgnoreCase("bintable")) {
                List<BinTableVO> binTables = getService(BinTableService.class).getBinTableListByKeyword(keyword);
                if (binTables == null) binTables = new ArrayList<>();
                List<BinTableVO> validBinTables = new ArrayList<>();
                for (BinTableVO binTable : binTables) {
                    // shortblock params.
                    String jarrParams = hashOps.get("BT" + binTable.getSeq().originOf(), "PARAMS");
                    if (jarrParams == null || jarrParams.isEmpty()) continue;
                    Type paramType = new TypeToken<List<String>>() {
                    }.getType();
                    List<String> binParams = ServerConstants.GSON.fromJson(jarrParams, paramType);
                    if (binParams == null || binParams.size() == 0) continue;
                    List<ShortBlockVO.Param> sparams = new ArrayList<>();
                    for (String binParam : binParams) {
                        CryptoField seq = CryptoField.decode(binParam, 0L);
                        ShortBlockVO.Param sparam = getService(ParamService.class).getShortBlockParamBySeq(seq);
                        if (sparam == null) continue;
                        sparams.add(sparam);
                    }
                    if (sparams.size() == 0) continue;
                    binTable.setParams(sparams);
                    validBinTables.add(binTable);
                }
                return ResponseHelper.response(200, "Success - ParamModule Source Search", validBinTables);
            }

            throw new HandledServiceException(411, "지원하지 않는 소스 형식입니다.");
        }

        if (command.equals("source-list")) {
            CryptoField moduleSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "moduleSeq"))
                moduleSeq = CryptoField.decode(payload.get("moduleSeq").getAsString(), 0L);

            if (moduleSeq == null || moduleSeq.isEmpty())
                throw new HandledServiceException(411, "파라미터를 확인하세요.");

            // load all sources and equations.
            equationHelper.setListOps(listOps);
            equationHelper.setHashOps(hashOps);
            equationHelper.setZsetOps(zsetOps);

            equationHelper.loadParamModuleData(moduleSeq);

            List<ParamModuleVO.Source> sources = equationHelper.getParamModuleSources(moduleSeq);

            //List<ParamModuleVO.Source> sources = getService(ParamModuleService.class).getParamModuleSourceList(moduleSeq);
            if (sources == null) sources = new ArrayList<>();

            return ResponseHelper.response(200, "Success - ParamModule Source List", sources);
        }

        if (command.equals("source-count")) {
            CryptoField moduleSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "moduleSeq"))
                moduleSeq = CryptoField.decode(payload.get("moduleSeq").getAsString(), 0L);

            CryptoField sourceSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "sourceSeq"))
                sourceSeq = CryptoField.decode(payload.get("sourceSeq").getAsString(), 0L);

            if (moduleSeq == null || moduleSeq.isEmpty() || sourceSeq == null || sourceSeq.isEmpty())
                throw new HandledServiceException(411, "파라미터를 확인하세요.");

            // load all sources and equations.
            equationHelper.setListOps(listOps);
            equationHelper.setHashOps(hashOps);
            equationHelper.setZsetOps(zsetOps);

            equationHelper.loadParamModuleData(moduleSeq);

            List<ParamModuleVO.Source> sources = equationHelper.getParamModuleSources(moduleSeq);
            if (sources == null) sources = new ArrayList<>();
            ParamModuleVO.Source findSource = null;
            for (ParamModuleVO.Source source : sources) {
                if (source.getSeq().equals(sourceSeq)) {
                    findSource = source;
                    break;
                }
            }

            return ResponseHelper.response(200, "Success - ParamModule Source Count", findSource == null ? 0 : findSource.getDataCount());
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

            List<ParamModuleVO.Source> paramSources = getService(ParamModuleService.class).getParamModuleSourceList(moduleSeq);

            JsonArray jarrSources = null;
            if (!checkJsonEmpty(payload, "sources"))
                jarrSources = payload.get("sources").getAsJsonArray();

            List<String> sourceTypes = Arrays.asList("part", "shortblock", "dll", "parammodule", "bintable");

            if (jarrSources.size() > 0) {
                if (paramSources == null) paramSources = new ArrayList<>();
                for (ParamModuleVO.Source paramSource : paramSources)
                    paramSource.setMark(false);

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

                    ParamModuleVO.Source findSource = null;
                    for (ParamModuleVO.Source paramSource : paramSources) {
                        if (paramSource.getSeq().equals(moduleSource.getSeq())) {
                            paramSource.setMark(true);
                            findSource = paramSource;
                            break;
                        }
                    }

                    moduleSource.setDataCount(getDataCount(
                            moduleSource.getSourceType(), moduleSource.getSourceSeq(),
                            moduleSource.getParamSeq()));

                    if (findSource != null) {
                        moduleSource.setModuleSeq(moduleSeq);
                        getService(ParamModuleService.class).updateParamModuleSource(moduleSource);
                    } else {
                        moduleSource.setModuleSeq(moduleSeq);
                        getService(ParamModuleService.class).insertParamModuleSource(moduleSource);
                    }
                }
            } else {
                // 모두 삭제된 것으로 간주함.
                getService(ParamModuleService.class).deleteParamModuleSourceByModuleSeq(moduleSeq);
            }

            // 요청에 없는 소스 삭제.
            for (ParamModuleVO.Source paramSource : paramSources) {
                if (paramSource.isMark() == false)
                    getService(ParamModuleService.class).deleteParamModuleSource(paramSource.getSeq());
            }

            // load all sources and equations.
            equationHelper.setListOps(listOps);
            equationHelper.setHashOps(hashOps);
            equationHelper.setZsetOps(zsetOps);

            equationHelper.loadParamModuleData(moduleSeq, true);

            List<ParamModuleVO.Source> sources = equationHelper.getParamModuleSources(moduleSeq);
            if (sources == null) sources = new ArrayList<>();

            paramModule.setSources(sources);

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

            List<String> sourceTypes = Arrays.asList("part", "shortblock", "dll", "parammodule", "bintable");

            ParamModuleVO.Source moduleSource = ServerConstants.GSON.fromJson(jobjSource, ParamModuleVO.Source.class);
            if (moduleSource.getSourceType() == null || moduleSource.getSourceType().isEmpty()
                    || !sourceTypes.contains(moduleSource.getSourceType())
                    || moduleSource.getSourceSeq() == null || moduleSource.getSourceSeq().isEmpty()) {
                throw new HandledServiceException(411, "저장 형식이 올바르지 않습니다.");
            }

            ParamModuleVO.Source findSource = getService(ParamModuleService.class).getParamModuleSourceBySeq(moduleSource.getSeq());

            moduleSource.setDataCount(getDataCount(
                    moduleSource.getSourceType(), moduleSource.getSourceSeq(),
                    moduleSource.getParamSeq()));

            if (findSource != null) {
                moduleSource.setModuleSeq(moduleSeq);
                getService(ParamModuleService.class).updateParamModuleSource(moduleSource);
            } else {
                moduleSource.setModuleSeq(moduleSeq);
                getService(ParamModuleService.class).insertParamModuleSource(moduleSource);
            }

            paramModule.setSources(getService(ParamModuleService.class).getParamModuleSourceList(moduleSeq));

            return ResponseHelper.response(200, "Success - ParamModule Save Source Single", paramModule);
        }

        if (command.equals("eq-list")) {
            CryptoField moduleSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "moduleSeq"))
                moduleSeq = CryptoField.decode(payload.get("moduleSeq").getAsString(), 0L);

            if (moduleSeq == null || moduleSeq.isEmpty())
                throw new HandledServiceException(411, "파라미터를 확인하세요.");

            // load all sources and equations.
            equationHelper.setListOps(listOps);
            equationHelper.setHashOps(hashOps);
            equationHelper.setZsetOps(zsetOps);

            equationHelper.loadParamModuleData(moduleSeq);

            ParamModuleVO paramModule = equationHelper.getParamModule(moduleSeq);
            if (paramModule.getEquations() == null || paramModule.getEquations().size() == 0)
                return ResponseHelper.response(200, "Success - ParamModule Eq List", paramModule.getEquations());

            List<ParamModuleVO.Equation> equations = new ArrayList<>(paramModule.getEqMap().values());
            equationHelper.calculateEquations(moduleSeq, equations);

            return ResponseHelper.response(200, "Success - ParamModule Eq List", equations);
        }

        if (command.equals("eq-data")) {
            CryptoField moduleSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "moduleSeq"))
                moduleSeq = CryptoField.decode(payload.get("moduleSeq").getAsString(), 0L);

            CryptoField eqSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "eqSeq"))
                eqSeq = CryptoField.decode(payload.get("eqSeq").getAsString(), 0L);

            if (moduleSeq == null || moduleSeq.isEmpty() || eqSeq == null || eqSeq.isEmpty())
                throw new HandledServiceException(411, "파라미터를 확인하세요.");

            //ParamModuleVO.Equation equation = getService(ParamModuleService.class).getParamModuleEqBySeq(eqSeq);

            String jsonData = hashOps.get("PM" + moduleSeq.originOf(), "E" + eqSeq.originOf());
            if (jsonData == null || jsonData.isEmpty())
                throw new HandledServiceException(411, "수식이 계산되지 않았습니다. 다시 저장해주세요.");

            JsonArray jarrData = ServerConstants.GSON.fromJson(jsonData, JsonArray.class);
            if (jarrData == null)
                throw new HandledServiceException(411, "수식 데이터에 오류가 있습니다.");

            return ResponseHelper.response(200, "Success - ParamModule Eq Data", jarrData);
        }

        if (command.equals("save-eq")) {
            CryptoField moduleSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "moduleSeq"))
                moduleSeq = CryptoField.decode(payload.get("moduleSeq").getAsString(), 0L);

            JsonArray jarrEquations = null;
            if (!checkJsonEmpty(payload, "equations"))
                jarrEquations = payload.get("equations").getAsJsonArray();

            if (moduleSeq == null || moduleSeq.isEmpty() || jarrEquations == null)
                throw new HandledServiceException(411, "파라미터를 확인하세요.");

            List<ParamModuleVO.Equation> paramEquations = getService(ParamModuleService.class).getParamModuleEqList(moduleSeq);
            if (paramEquations == null) paramEquations = new ArrayList<>();
            for (ParamModuleVO.Equation paramEquation : paramEquations)
                paramEquation.setMark(false);

            ParamModuleVO paramModule = getService(ParamModuleService.class).getParamModuleBySeq(moduleSeq);

            if (jarrEquations.size() > 0) {
                paramModule.setEquations(new ArrayList<>());
                for (int i = 0; i < jarrEquations.size(); i++) {
                    JsonObject jobjEquation = jarrEquations.get(i).getAsJsonObject();
                    ParamModuleVO.Equation equation = ServerConstants.GSON.fromJson(jobjEquation, ParamModuleVO.Equation.class);
                    if (equation == null
                            || equation.getEqName() == null || equation.getEqName().isEmpty()
                            || equation.getEquation() == null || equation.getEquation().isEmpty()) {
                        // skip equation
                        continue;
                    }
                    equation.setEqOrder(paramModule.getEquations().size() + 1);
                    equation.setModuleSeq(moduleSeq);

                    ParamModuleVO.Equation findEquation = null;
                    for (ParamModuleVO.Equation paramEquation : paramEquations) {
                        if (paramEquation.getSeq().equals(equation.getSeq())) {
                            paramEquation.setMark(true);
                            findEquation = paramEquation;
                            break;
                        }
                    }

                    if (findEquation == null) {
                        try {
                            getService(ParamModuleService.class).insertParamModuleEq(equation);
                        } catch (HandledServiceException hse) {
                            continue;
                        }
                        findEquation = equation;
                    } else {
                        try {
                            getService(ParamModuleService.class).updateParamModuleEq(equation);
                        } catch (HandledServiceException hse) {
                            continue;
                        }
                        // 저장이후...
                        findEquation.setEqOrder(equation.getEqOrder());
                        findEquation.setEqName(equation.getEqName());
                        findEquation.setEquation(equation.getEquation());
                    }

                    findEquation.setDataCount(getDataCount("eq", paramModule.getSeq(), findEquation.getSeq()));
                    getService(ParamModuleService.class).updateParamModuleEq(findEquation);

                    // dataProp 처리
                    if (!checkJsonEmpty(jobjEquation, "dataProp")) {
                        Type type = new TypeToken<Map<String, String>>() {
                        }.getType();
                        Map<String, String> dataProp = ServerConstants.GSON.fromJson(
                                jobjEquation.get("dataProp").getAsJsonObject(), type);
                        Set<String> keys = dataProp.keySet();
                        Iterator<String> iterKeys = keys.iterator();
                        while (iterKeys.hasNext()) {
                            String key = iterKeys.next();
                            String value = dataProp.get(key);

                            DataPropVO saveProp = new DataPropVO();
                            saveProp.setPropName(new String64(key));
                            saveProp.setPropValue(new String64(value));
                            saveProp.setReferenceType("eq");
                            saveProp.setReferenceKey(findEquation.getSeq());
                            saveProp.setUpdatedAt(LongDate.now());
                            getService(RawService.class).insertDataProp(saveProp);
                        }
                        findEquation.setDataProp(getService(RawService.class).getDataPropListToMap("eq", findEquation.getSeq()));
                    }

                    findEquation.setEqNo("E" + findEquation.getSeq().originOf());
                    paramModule.getEquations().add(findEquation);
                }

                // 기존 것들 중에 제외된거 지우기
                for (ParamModuleVO.Equation paramEquation : paramEquations) {
                    if (paramEquation.isMark() == false)
                        getService(ParamModuleService.class).deleteParamModuleEq(paramEquation.getSeq());
                }
            } else {
                // 기존 수식 삭제. => 저장되는 equations가 없으면 모두 삭제.
                getService(ParamModuleService.class).deleteParamModuleEqByModuleSeq(moduleSeq);
                paramModule.setEquations(new ArrayList<>());
            }

            equationHelper.setListOps(listOps);
            equationHelper.setHashOps(hashOps);
            equationHelper.setZsetOps(zsetOps);

            equationHelper.calculateEquations(moduleSeq, paramModule.getEquations(), true); // 강제 로딩후 재 계산.

            //List<ParamModuleVO.Equation> equations = getService(ParamModuleService.class).getParamModuleEqList(moduleSeq);

            return ResponseHelper.response(200, "Success - ParamModule Save Eq", paramModule.getEquations());
        }

        if (command.equals("evaluation")) {
            CryptoField moduleSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "moduleSeq"))
                moduleSeq = CryptoField.decode(payload.get("moduleSeq").getAsString(), 0L);

            String equation = "";
            if (!checkJsonEmpty(payload, "equation"))
                equation = payload.get("equation").getAsString();

            if (moduleSeq == null || moduleSeq.isEmpty() || equation == null || equation.isEmpty())
                throw new HandledServiceException(411, "파라미터를 확인하세요.");

            equationHelper.setListOps(listOps);
            equationHelper.setHashOps(hashOps);
            equationHelper.setZsetOps(zsetOps);

            equationHelper.loadParamModuleData(moduleSeq);
            JsonArray evalResult = equationHelper.calculateEquationSingle(moduleSeq, equation);

            return ResponseHelper.response(200, "Success - ParamModule Eq Evaluation", evalResult);
        }

        if (command.equals("plot-list")) {
            CryptoField moduleSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "moduleSeq"))
                moduleSeq = CryptoField.decode(payload.get("moduleSeq").getAsString(), 0L);

            if (moduleSeq == null || moduleSeq.isEmpty())
                throw new HandledServiceException(411, "파라미터를 확인하세요.");

            // all sources.
            equationHelper.setListOps(listOps);
            equationHelper.setHashOps(hashOps);
            equationHelper.setZsetOps(zsetOps);

            equationHelper.loadParamModuleData(moduleSeq);

            ParamModuleVO paramModule = equationHelper.getParamModule(moduleSeq);
            JsonObject jobjSources = new JsonObject();
            jobjSources.add("sourceList", ServerConstants.GSON.toJsonTree(paramModule.getSources()));
            jobjSources.add("eqList", ServerConstants.GSON.toJsonTree(paramModule.getEquations()));

            List<ParamModuleVO.Plot> plots = getService(ParamModuleService.class).getParamModulePlotList(moduleSeq);
            if (plots == null) plots = new ArrayList<>();

            for (ParamModuleVO.Plot plot : plots) {
                plot.setDataProp(getService(RawService.class).getDataPropListToMap("plot", plot.getSeq()));

                plot.setPlotSeries(getService(ParamModuleService.class).getParamModulePlotSeriesList(plot.getSeq()));
                if (plot.getPlotSeries() == null) plot.setPlotSeries(new ArrayList<>());

                plot.setPlotSourceList(getService(ParamModuleService.class).getParamModulePlotSourceList(moduleSeq, plot.getSeq()));
                if (plot.getPlotSourceList() == null) plot.setPlotSourceList(new ArrayList<>());

                List<List<Object>> plotDataList = new ArrayList<>();
                plot.setPlotSources(new ArrayList<>());

                for (ParamModuleVO.Plot.Source plotSource : plot.getPlotSourceList()) {
                    plot.getPlotSources().add(ParamModuleVO.Plot.Source.getSimple(plotSource));
                    List<Object> plotData = equationHelper.loadPlotData(moduleSeq, ParamModuleVO.Plot.Source.getSimple(plotSource));
                    plotDataList.add(plotData);
                }

                plot.setSelectPoints(getService(ParamModuleService.class).getPlotSavePointList(moduleSeq, plot.getSeq()));
            }

            return ResponseHelper.response(200, "Success - ParamModule Plot List", plots.size(), plots, jobjSources);
        }

        if (command.equals("plot-data")) {
            CryptoField moduleSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "moduleSeq"))
                moduleSeq = CryptoField.decode(payload.get("moduleSeq").getAsString(), 0L);

            CryptoField plotSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "plotSeq"))
                plotSeq = CryptoField.decode(payload.get("plotSeq").getAsString(), 0L);

            if (moduleSeq == null || moduleSeq.isEmpty() || plotSeq == null || plotSeq.isEmpty())
                throw new HandledServiceException(411, "파라미터를 확인하세요.");

            equationHelper.setListOps(listOps);
            equationHelper.setHashOps(hashOps);
            equationHelper.setZsetOps(zsetOps);

            equationHelper.loadParamModuleData(moduleSeq);

            ParamModuleVO.Plot plotInfo = getService(ParamModuleService.class).getParamModulePlotBySeq(plotSeq);
            if (plotInfo == null)
                throw new HandledServiceException(411, "PLOT 정보를 찾을 수 없습니다.");

            plotInfo.setDataProp(getService(RawService.class).getDataPropListToMap("plot", plotInfo.getSeq()));

            plotInfo.setPlotSeries(getService(ParamModuleService.class).getParamModulePlotSeriesList(plotSeq));
            if (plotInfo.getPlotSeries() == null) plotInfo.setPlotSeries(new ArrayList<>());

            plotInfo.setPlotSourceList(getService(ParamModuleService.class).getParamModulePlotSourceList(moduleSeq, plotSeq));
            if (plotInfo.getPlotSourceList() == null) plotInfo.setPlotSourceList(new ArrayList<>());

            List<List<Object>> plotDataList = new ArrayList<>();
            plotInfo.setPlotSources(new ArrayList<>());

            for (ParamModuleVO.Plot.Source plotSource : plotInfo.getPlotSourceList()) {
                plotInfo.getPlotSources().add(ParamModuleVO.Plot.Source.getSimple(plotSource));

                List<Object> plotData = equationHelper.loadPlotData(moduleSeq, ParamModuleVO.Plot.Source.getSimple(plotSource));
                plotDataList.add(plotData);
            }

            plotInfo.setSelectPoints(getService(ParamModuleService.class).getPlotSavePointList(moduleSeq, plotInfo.getSeq()));

            return ResponseHelper.response(200, "Success - ParamModule Plot Data", 0, plotDataList, plotInfo);
        }

        if (command.equals("save-plot")) {
            CryptoField moduleSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "moduleSeq"))
                moduleSeq = CryptoField.decode(payload.get("moduleSeq").getAsString(), 0L);

            if (moduleSeq == null || moduleSeq.isEmpty())
                throw new HandledServiceException(411, "파라미터를 확인하세요.");

            // all sources.
            equationHelper.setListOps(listOps);
            equationHelper.setHashOps(hashOps);
            equationHelper.setZsetOps(zsetOps);

            equationHelper.loadParamModuleData(moduleSeq);

            ParamModuleVO paramModule = equationHelper.getParamModule(moduleSeq);
            JsonObject jobjSources = new JsonObject();
            jobjSources.add("sourceList", ServerConstants.GSON.toJsonTree(paramModule.getSources()));
            jobjSources.add("eqList", ServerConstants.GSON.toJsonTree(paramModule.getEquations()));

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
                    if (checkJsonEmpty(jobjPlot, "plotSeries")) continue;

                    String plotType = "";
                    if (!checkJsonEmpty(jobjPlot, "plotType"))
                        plotType = jobjPlot.get("plotType").getAsString();

                    ParamModuleVO.Plot plot = new ParamModuleVO.Plot();
                    plot.setModuleSeq(moduleSeq);
                    plot.setPlotName(String64.decode(jobjPlot.get("plotName").getAsString()));
                    plot.setPlotType(plotType);
                    plot.setPlotOrder(plots.size() + 1);
                    plot.setCreatedAt(LongDate.now());
                    getService(ParamModuleService.class).insertParamModulePlot(plot);

                    plots.add(plot);

                    if (!checkJsonEmpty(jobjPlot, "dataProp")) {
                        Type type = new TypeToken<Map<String, String>>() {
                        }.getType();
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

                    JsonArray jarrSeries = jobjPlot.get("plotSeries").getAsJsonArray();
                    plot.setPlotSeries(new ArrayList<>());
                    if (jarrSeries.size() > 0) {
                        for (int j = 0; j < jarrSeries.size(); j++) {
                            JsonObject jobjSeries = jarrSeries.get(j).getAsJsonObject();
                            ParamModuleVO.Plot.Series plotSeries = ServerConstants.GSON.fromJson(jobjSeries, ParamModuleVO.Plot.Series.class);
                            plotSeries.setPlotSeq(plot.getSeq());
                            plotSeries.setModuleSeq(plot.getModuleSeq());
                            getService(ParamModuleService.class).insertParamModulePlotSeries(plotSeries);
                            plot.getPlotSeries().add(plotSeries);
                        }
                    }

                    // remove old points
                    getService(ParamModuleService.class).deletePlotSavePoint(moduleSeq, plot.getSeq());

                    if (!checkJsonEmpty(jobjPlot, "selectPoints")) {
                        JsonArray jarrSavePoints = jobjPlot.get("selectPoints").getAsJsonArray();
                        plot.setSelectPoints(new ArrayList<>());
                        if (jarrSavePoints.size() > 0) {
                            for (int j = 0; j < jarrSavePoints.size(); j++) {
                                JsonObject jobjSavePoint = jarrSavePoints.get(j).getAsJsonObject();
                                ParamModuleVO.Plot.SavePoint savePoint = ServerConstants.GSON.fromJson(jobjSavePoint, ParamModuleVO.Plot.SavePoint.class);
                                savePoint.setPlotSeq(plot.getSeq());
                                savePoint.setModuleSeq(plot.getModuleSeq());
                                getService(ParamModuleService.class).insertPlotSavePoint(savePoint);
                                plot.getSelectPoints().add(savePoint);
                            }
                        }
                    }
                }
            }

            return ResponseHelper.response(200, "Success - ParamModule Save Plot", plots.size(), plots, jobjSources);
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
            } else {
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

                Type type = new TypeToken<Map<String, String>>() {
                }.getType();
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
            List<String> sourceTypes = Arrays.asList("part", "shortblock", "dll", "parammodule", "eq");

            // 기존 소스 삭제.
            getService(ParamModuleService.class).deleteParamModulePlotSourceByPlotSeq(moduleSeq, plotSeq);

            plot.setPlotSourceList(new ArrayList<>());
            if (jarrSources.size() > 0) {
                for (int j = 0; j < jarrSources.size(); j++) {
                    JsonObject jobjSource = jarrSources.get(j).getAsJsonObject();
                    String sourceType = "";
                    if (!checkJsonEmpty(jobjSource, "sourceType"))
                        sourceType = jobjSource.get("sourceType").getAsString();

                    CryptoField sourceSeq = CryptoField.LZERO;
                    if (!checkJsonEmpty(jobjSource, "sourceSeq"))
                        sourceSeq = CryptoField.decode(jobjSource.get("sourceSeq").getAsString(), 0L);

                    ParamModuleVO.Plot.Source plotSource = new ParamModuleVO.Plot.Source();
                    if (!sourceType.equals("eq")) {
                        ParamModuleVO.Source paramSource = getService(ParamModuleService.class).getParamModuleSourceBySeq(sourceSeq);
                        if (paramSource == null) continue;
                        plotSource.setModuleSeq(moduleSeq);
                        plotSource.setPlotSeq(plot.getSeq());
                        plotSource.setSourceType(sourceType);
                        plotSource.setSourceSeq(sourceSeq);
                        plotSource.setJulianStartAt(paramSource.getJulianStartAt());
                        plotSource.setJulianEndAt(paramSource.getJulianEndAt());
                        plotSource.setOffsetStartAt(paramSource.getOffsetStartAt());
                        plotSource.setOffsetEndAt(paramSource.getOffsetEndAt());
                    } else {
                        ParamModuleVO.Equation eq = getService(ParamModuleService.class).getParamModuleEqBySeq(sourceSeq);
                        if (eq == null) continue;
                        plotSource.setModuleSeq(moduleSeq);
                        plotSource.setPlotSeq(plot.getSeq());
                        plotSource.setSourceType(sourceType);
                        plotSource.setSourceSeq(sourceSeq);
                        plotSource.setJulianStartAt(eq.getJulianStartAt());
                        plotSource.setJulianEndAt(eq.getJulianEndAt());
                        plotSource.setOffsetStartAt(eq.getOffsetStartAt());
                        plotSource.setOffsetEndAt(eq.getOffsetEndAt());
                    }
                    getService(ParamModuleService.class).insertParamModulePlotSource(plotSource);
                    plot.getPlotSourceList().add(plotSource);
                }

                if (plot.getPlotSourceList().size() > 0) {
                    plot.setPlotSources(new ArrayList<>());
                    for (ParamModuleVO.Plot.Source plotSource : plot.getPlotSourceList()) {
                        plot.getPlotSources().add(ParamModuleVO.Plot.Source.getSimple(plotSource));
                    }
                }
            }

            return ResponseHelper.response(200, "Success - ParamModule Save Plot Single", plot);
        }

        if (command.equals("find-source")) {
            CryptoField moduleSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "moduleSeq"))
                moduleSeq = CryptoField.decode(payload.get("moduleSeq").getAsString(), 0L);

            CryptoField eqSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "eqSeq"))
                eqSeq = CryptoField.decode(payload.get("eqSeq").getAsString(), 0L);

            String xValue = "";
            if (!checkJsonEmpty(payload, "xValue"))
                xValue = payload.get("xValue").getAsString();

            String yValue = "";
            if (!checkJsonEmpty(payload, "yValue"))
                yValue = payload.get("yValue").getAsString();

            String chartType = "";
            if (!checkJsonEmpty(payload, "chartType"))
                chartType = payload.get("chartType").getAsString();

            int pointIndex = 0;
            if (!checkJsonEmpty(payload, "pointIndex"))
                pointIndex = payload.get("pointIndex").getAsInt();

            if (moduleSeq == null || moduleSeq.isEmpty() || eqSeq == null || eqSeq.isEmpty()
                    || chartType == null || chartType.isEmpty())
                throw new HandledServiceException(411, "파라미터를 확인하세요.");

            // all sources.
            equationHelper.setListOps(listOps);
            equationHelper.setHashOps(hashOps);
            equationHelper.setZsetOps(zsetOps);

            equationHelper.loadParamModuleData(moduleSeq);
            ParamModuleVO paramModule = equationHelper.getParamModule(moduleSeq);

            if (paramModule.getEqMap() == null || paramModule.getEqMap().size() == 0)
                throw new HandledServiceException(404, "파라미터 모듈에 수식 정보가 없습니다.");

            JsonObject jobjOutput = new JsonObject();

            ParamModuleVO.Equation eq = paramModule.getEqMap().get("E" + eqSeq.originOf());
            if (eq == null)
                throw new HandledServiceException(404, "파라미터 모듈에 수식 정보가 없습니다.");

            if (chartType.equalsIgnoreCase("Potato")) {
                if (!eq.getEquation().contains("convh"))
                    throw new HandledServiceException(404, "Convex Hull 수식이 아닙니다.");

                boolean validConvexHull = equationHelper.findConvexHullTime(paramModule, eq, xValue, yValue, jobjOutput);
                if (validConvexHull == false)
                    throw new HandledServiceException(411, jobjOutput.get("message").getAsString());
            }
            else if (chartType.equalsIgnoreCase("CrossPlot")) {
                if (eq.getData() == null || eq.getData().size() == 0
                        || eq.getData().size() <= pointIndex) {
                    throw new HandledServiceException(404, "수식 데이터가 조건에 맞지 않습니다.");
                }

                if (!eq.getEquation().contains("min") && !eq.getEquation().contains("max"))
                    throw new HandledServiceException(404, "수식 데이터가 조건에 맞지 않습니다.");

                boolean validCrossPlot = equationHelper.findCrossPlotTime(paramModule, eq, xValue, yValue, pointIndex, jobjOutput);
                if (validCrossPlot == false)
                    throw new HandledServiceException(411, jobjOutput.get("message").getAsString());
            }
            else {
                throw new HandledServiceException(411, "지원하지 않는 형식입니다.");
            }

            return ResponseHelper.response(200, "Success - ParamModule FindSource", jobjOutput);
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

        if (command.equals("clear-summary")) {
            CryptoField binMetaSeq = CryptoField.LZERO;
            if (checkJsonEmpty(payload, "binMetaSeq"))
                binMetaSeq = CryptoField.decode(payload.get("binMetaSeq").getAsString(), 0L);

            if (binMetaSeq == null || binMetaSeq.isEmpty())
                throw new HandledServiceException(404, "파라미터를 확인하세요.");

            String jsonCellList = hashOps.get("BT" + binMetaSeq.originOf(), "CELL");
            List<String> cells = new ArrayList<>();
            if (jsonCellList != null && !jsonCellList.isEmpty()) {
                Type cellType = new TypeToken<List<String>>() {
                }.getType();
                cells = ServerConstants.GSON.fromJson(jsonCellList, cellType);
            }

            String jsonParamList = hashOps.get("BT" + binMetaSeq.originOf(), "PARAMS");
            List<String> params = new ArrayList<>();
            if (jsonParamList != null && !jsonParamList.isEmpty()) {
                Type paramType = new TypeToken<List<String>>() {
                }.getType();
                params = ServerConstants.GSON.fromJson(jsonParamList, paramType);
            }

            for (String cell : cells) {
                hashOps.delete("BT" + binMetaSeq.originOf() + "." + cell);
                for (String param : params) {
                    hashOps.delete("BT" + binMetaSeq.originOf() + "." + cell + "." + param);
                }
            }

            hashOps.delete("BT" + binMetaSeq.originOf(), "CELL");
            hashOps.delete("BT" + binMetaSeq.originOf(), "PARAMS");

            return ResponseHelper.response(200, "Success - BinTable summary cleared.", "");
        }

        if (command.equals("calculate")) {
            BinTableVO.CalculateRequest calRequest = ServerConstants.GSON.fromJson(payload, BinTableVO.CalculateRequest.class);
            BinTableVO.BinSummary binSummary = getService(BinTableService.class).calculateBinSummary(user.getUid(), calRequest);
            return ResponseHelper.response(200, "Success - BinTable summary saved", binSummary);
        }

        if (command.equals("load")) {
            CryptoField binMetaSeq = CryptoField.LZERO;
            if (checkJsonEmpty(payload, "binMetaSeq"))
                binMetaSeq = CryptoField.decode(payload.get("binMetaSeq").getAsString(), 0L);

            if (binMetaSeq == null || binMetaSeq.isEmpty())
                throw new HandledServiceException(404, "파라미터를 확인하세요.");

            JsonArray cellIndexes = null;
            if (checkJsonEmpty(payload, "indexes"))
                cellIndexes = payload.get("indexes").getAsJsonArray();

            String responseType = "list";
            if (checkJsonEmpty(payload, "responseType"))
                responseType = payload.get("responseType").getAsString();

            String jsonCellList = hashOps.get("BT" + binMetaSeq.originOf(), "CELL");
            List<String> cells = new ArrayList<>();
            if (jsonCellList != null && !jsonCellList.isEmpty()) {
                Type cellType = new TypeToken<List<String>>() {
                }.getType();
                cells = ServerConstants.GSON.fromJson(jsonCellList, cellType);
            }

            String jsonParamList = hashOps.get("BT" + binMetaSeq.originOf(), "PARAMS");
            List<String> params = new ArrayList<>();
            if (jsonParamList != null && !jsonParamList.isEmpty()) {
                Type paramType = new TypeToken<List<String>>() {
                }.getType();
                params = ServerConstants.GSON.fromJson(jsonParamList, paramType);
            }

            List<BinTableVO.BinSummary> binSummaries = new ArrayList<>();
            Map<String, BinTableVO.BinSummary> mapSummaries = new HashMap<>();

            for (String cell : cells) {
                if (cellIndexes != null && cellIndexes.size() > 0) {
                    boolean requested = false;
                    for (int i = 0; i < cellIndexes.size(); i++) {
                        if (cellIndexes.get(i).getAsString().equals(cell)) {
                            requested = true;
                            break;
                        }
                    }
                    if (requested == false) continue;
                } else {
                    // no condition -> all list
                }

                BinTableVO.BinSummary binSummary = new BinTableVO.BinSummary();
                binSummary.setBinMetaSeq(binMetaSeq);
                binSummary.setSummary(new HashMap<>());

                String[] typeSet = new String[]{"", ".L", ".H", ".B"};
                for (String param : params) {
                    binSummary.getSummary().put(param, new HashMap<>());

                    for (String type : typeSet) {
                        BinTableVO.BinSummary.SummaryItem summaryItem = new BinTableVO.BinSummary.SummaryItem();

                        String min = hashOps.get("BT" + binMetaSeq.originOf() + "." + cell + "." + param, "MIN" + type);
                        if (min != null && !min.isEmpty())
                            summaryItem.setMin(Double.parseDouble(min));

                        String max = hashOps.get("BT" + binMetaSeq.originOf() + "." + cell + "." + param, "MAX" + type);
                        if (max != null && !max.isEmpty())
                            summaryItem.setMax(Double.parseDouble(max));

                        String avg = hashOps.get("BT" + binMetaSeq.originOf() + "." + cell + "." + param, "AVG" + type);
                        if (avg != null && !avg.isEmpty())
                            summaryItem.setAvg(Double.parseDouble(avg));

                        String psd = hashOps.get("BT" + binMetaSeq.originOf() + "." + cell + "." + param, "PSD" + type);
                        if (psd != null && !psd.isEmpty()) {
                            Type psdType = new TypeToken<List<Double>>() {
                            }.getType();
                            List<Double> psdList = ServerConstants.GSON.fromJson(psd, psdType);
                            summaryItem.setPsd(psdList);
                        }

                        String freq = hashOps.get("BT" + binMetaSeq.originOf() + "." + cell + "." + param, "FREQ" + type);
                        if (freq != null && !freq.isEmpty()) {
                            Type freqType = new TypeToken<List<Double>>() {
                            }.getType();
                            List<Double> freqList = ServerConstants.GSON.fromJson(freq, freqType);
                            summaryItem.setFrequency(freqList);
                        }

                        String rms = hashOps.get("BT" + binMetaSeq.originOf() + "." + cell + "." + param, "RMS" + type);
                        if (rms != null && !rms.isEmpty()) {
                            Type rmsType = new TypeToken<List<Double>>() {
                            }.getType();
                            List<Double> rmsList = ServerConstants.GSON.fromJson(rms, rmsType);
                            summaryItem.setRms(rmsList);
                        }

                        String avg_rms = hashOps.get("BT" + binMetaSeq.originOf() + "." + cell + "." + param, "AVG.RMS" + type);
                        if (avg_rms != null && !avg_rms.isEmpty())
                            summaryItem.setAvg_rms(Double.parseDouble(avg_rms));

                        String n0 = hashOps.get("BT" + binMetaSeq.originOf() + "." + cell + "." + param, "N0" + type);
                        if (n0 != null && !n0.isEmpty()) {
                            Type n0Type = new TypeToken<List<Double>>() {
                            }.getType();
                            List<Double> n0List = ServerConstants.GSON.fromJson(n0, n0Type);
                            summaryItem.setN0(n0List);
                        }

                        String zeta = hashOps.get("BT" + binMetaSeq.originOf() + "." + cell + "." + param, "ZETA" + type);
                        if (zeta != null && !zeta.isEmpty()) {
                            Type zetaType = new TypeToken<List<Double>>() {
                            }.getType();
                            List<Double> zetaList = ServerConstants.GSON.fromJson(zeta, zetaType);
                            summaryItem.setZeta(zetaList);
                        }

                        String burstFactor = hashOps.get("BT" + binMetaSeq.originOf() + "." + cell + "." + param, "BF" + type);
                        if (burstFactor != null && !burstFactor.isEmpty())
                            summaryItem.setBurstFactor(Double.parseDouble(burstFactor));

                        String rmsToPeak = hashOps.get("BT" + binMetaSeq.originOf() + "." + cell + "." + param, "RTP" + type);
                        if (rmsToPeak != null && !rmsToPeak.isEmpty()) {
                            Type rmsToPeakType = new TypeToken<List<Double>>() {
                            }.getType();
                            List<Double> rmsToPeakList = ServerConstants.GSON.fromJson(rmsToPeak, rmsToPeakType);
                            summaryItem.setRmsToPeak(rmsToPeakList);
                        }

                        String maxRmsToPeak = hashOps.get("BT" + binMetaSeq.originOf() + "." + cell + "." + param, "MRTP" + type);
                        if (maxRmsToPeak != null && !maxRmsToPeak.isEmpty())
                            summaryItem.setMaxRmsToPeak(Double.parseDouble(maxRmsToPeak));

                        String maxLoadAccel = hashOps.get("BT" + binMetaSeq.originOf() + "." + cell + "." + param, "MLA" + type);
                        if (maxLoadAccel != null && !maxLoadAccel.isEmpty())
                            summaryItem.setMaxLoadAccel(Double.parseDouble(maxLoadAccel));

                        binSummary.getSummary().get(param).put(
                                type.equals(".L") ? "lpf" : type.equals(".H") ? "hpf" : type.equals(".B") ? "bpf" : "normal",
                                summaryItem);
                    }
                }

                binSummaries.add(binSummary);
                mapSummaries.put(cell, binSummary);
            }

            return ResponseHelper.response(200, "Success - BinTable summary loaded",
                    responseType.equals("list") ? binSummaries : ServerConstants.GSON.toJsonTree(mapSummaries));
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
                } catch (HandledServiceException hse) {
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
            } else if (jarrRows.size() < 2) {
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
            } else {
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
                    } else {
                        // 모두 삭제
                        getService(DLLService.class).deleteDLLData(dllSeq);
                    }
                }
            } else {
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

        if (command.equals("remove-upload")) {
            CryptoField uploadSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "uploadSeq"))
                uploadSeq = CryptoField.decode(payload.get("uploadSeq").getAsString(), 0L);

            if (uploadSeq == null || uploadSeq.isEmpty())
                throw new HandledServiceException(404, "필요 파라미터가 누락됐습니다.");

            // part
            // shortblock

            List<PartVO> partList = getService(PartService.class).getPartList(user.getUid(), uploadSeq, 1, 999999);
            if (partList == null || partList.size() == 0)
                throw new HandledServiceException(411, "분할데이터가 없습니다.");

            // param-module check.
            List<ParamModuleVO.Source> sources = getService(ParamModuleService.class).getReferencedSourceList(partList);
            if (sources != null || sources.size() > 0)
                throw new HandledServiceException(411, "분할데이터가 사용중입니다.");

            getService(PartService.class).deleteShortBlockByPartList(partList);
            getService(PartService.class).deletePartList(partList);
            getService(RawService.class).deleteRawUploadBySeq(uploadSeq);

            return ResponseHelper.response(200, "Success - Remove part by upload", "");
        }

        if (command.equals("flight-type")) {
            List<RawVO.FlightType> flightTypes = getService(RawService.class).getFlightTypeList();
            return ResponseHelper.response(200, "Success - FlightType List", flightTypes);
        }

        if (command.equals("save-flight-type")) {
            List<RawVO.FlightType> flightTypes = getService(RawService.class).getFlightTypeList();
            if (flightTypes == null) flightTypes = new ArrayList<>();
            for (RawVO.FlightType ft : flightTypes)
                ft.setMark(false);

            JsonArray reqList = null;
            if (!checkJsonEmpty(payload, "flightTypes"))
                reqList = payload.get("flightTypes").getAsJsonArray();

            if (reqList != null && reqList.size() > 0) {
                for (int i = 0; i < reqList.size(); i++) {
                    JsonObject jobjType = reqList.get(i).getAsJsonObject();
                    if (checkJsonEmpty(jobjType, "typeCode") || checkJsonEmpty(jobjType, "typeName"))
                        continue;

                    String typeCode = jobjType.get("typeCode").getAsString();
                    String64 typeName = String64.decode(jobjType.get("typeName").getAsString());

                    RawVO.FlightType findType = null;
                    for (RawVO.FlightType ft : flightTypes) {
                        if (ft.getTypeCode().equals(typeCode)) {
                            findType = ft;
                            break;
                        }
                    }

                    if (findType != null) {
                        findType.setTypeName(typeName);
                        getService(RawService.class).updateFlightType(findType);
                        findType.setMark(true);
                    }
                    else {
                        RawVO.FlightType newType = new RawVO.FlightType();
                        newType.setTypeCode(typeCode);
                        newType.setTypeName(typeName);
                        newType.setCreatedAt(LongDate.now());
                        getService(RawService.class).insertFlightType(newType);
                    }
                }
            }

            for (RawVO.FlightType ft : flightTypes) {
                if (ft.isMark() == false)
                    getService(RawService.class).deleteFlightType(ft.getTypeCode());
            }

            flightTypes = getService(RawService.class).getFlightTypeList();

            return ResponseHelper.response(200, "Success - FlightType Save", flightTypes);
        }

        if (command.equals("remove-flight-type")) {
            String typeCode = "";
            if (!checkJsonEmpty(payload, "typeCode"))
                typeCode = payload.get("typeCode").getAsString();

            if (typeCode == null || typeCode.isEmpty())
                throw new HandledServiceException(411, "파라미터를 확인하세요.");

            getService(RawService.class).deleteFlightType(typeCode);

            return ResponseHelper.response(200, "Success - FlightType Remove", "");
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

            logger.debug("[[[[[ headerRow=" + headerRow + ":: ]]]]]");

            /*
            if (!headerRow.contains("DL001_DEG")) {
              if (!headerRow.trim().endsWith(","))
                headerRow += ",";
              headerRow += "DL001_DEG";
            } */

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
            } else {
                if (presetParams == null) presetParams = new ArrayList<>();
                Map<String, ParamVO> adamsMap = new LinkedHashMap<>();
                Map<String, ParamVO> zaeroMap = new LinkedHashMap<>();
                for (ParamVO param : presetParams) {
                    adamsMap.put(param.getAdamsKey(), param);
                    zaeroMap.put(param.getZaeroKey(), param);
                }

                if (importFilePath != null) {
                    importFilePath = "E:\\" + importFilePath.substring("E:\\".length());
                }

                File fStatic = new File(importFilePath);
                if (fStatic == null || !fStatic.exists())
                    throw new HandledServiceException(404, "파일을 찾을 수 없습니다. [" + importFilePath + "]");

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
                } catch (IOException ex) {
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
                    } else {
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

    private void flushResponse(HttpServletResponse response, String flushData, String outFileName) throws HandledServiceException {
        response.setHeader("Content-Type", "application/octet-stream");
        response.addHeader("Content-Disposition",
                "attachment; filename=\"" + outFileName + "\";");
        response.addHeader("Content-Transfer-Encoding", "binary");

        try {
            byte[] outBuf = flushData.getBytes(StandardCharsets.UTF_8);
            OutputStream os = response.getOutputStream();
            os.write(outBuf, 0, outBuf.length);
            os.flush();
        } catch (IOException ioe) {
            throw new HandledServiceException(411, ioe.getMessage());
        }
    }

    private void flushResponseZip(HttpServletResponse response, List<File> fileOut, String outFileName) throws HandledServiceException {
        response.setContentType("application/zip");
        response.setHeader("Content-Disposition", "attachment;filename=\"" + outFileName + "\";");

        try (ZipOutputStream zipOutputStream = new ZipOutputStream(response.getOutputStream())) {
            for (File file : fileOut) {
                FileSystemResource fileSystemResource = new FileSystemResource(file.getAbsolutePath());
                ZipEntry zipEntry = new ZipEntry(fileSystemResource.getFilename());
                zipEntry.setSize(fileSystemResource.contentLength());
                zipEntry.setTime(System.currentTimeMillis());

                zipOutputStream.putNextEntry(zipEntry);

                StreamUtils.copy(fileSystemResource.getInputStream(), zipOutputStream);
                zipOutputStream.closeEntry();
            }

            zipOutputStream.finish();
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            for (File file : fileOut) {
                if (file.exists()) file.delete();
            }
        }
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
            } else {
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

                List<ParamVO> notMappedParams = getService(ParamService.class).getNotMappedParams(partInfo.getUploadSeq());
                if (notMappedParams != null && notMappedParams.size() > 0) {
                    for (ParamVO p : notMappedParams)
                        params.add(p);
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

            LinkedHashMap<String, List<Double>> rowData = new LinkedHashMap<>();
            if (julianFrom == null || julianFrom.isEmpty() || julianTo == null || julianTo.isEmpty()) {
                jobjResult.add("paramSet", ServerConstants.GSON.toJsonTree(params));
                jobjResult.add("julianSet", ServerConstants.GSON.toJsonTree(new ArrayList<String>()));
                jobjResult.add("data", ServerConstants.GSON.toJsonTree(rowData));

                StringBuilder sbOut = new StringBuilder();
                sbOut.append("DATE,");
                for (ParamVO p : params) sbOut.append(p.getParamKey().replaceAll(",", "_")).append(",");
                sbOut.append("\n");
                sbOut.append(",");
                for (ParamVO p : params) sbOut.append(",");
                sbOut.append("\n");

                flushResponse(response, sbOut.toString(), "P_" + partInfo.getPartName().originOf() + "_" + filterType + ".csv");
            } else {
                Set<String> listSet = zsetOps.rangeByScore("P" + partInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);
                if (listSet == null || listSet.size() == 0) {
                    jobjResult.add("paramSet", ServerConstants.GSON.toJsonTree(params));
                    jobjResult.add("julianSet", ServerConstants.GSON.toJsonTree(new ArrayList<String>()));
                    jobjResult.add("data", ServerConstants.GSON.toJsonTree(rowData));

                    StringBuilder sbOut = new StringBuilder();
                    sbOut.append("DATE,");
                    for (ParamVO p : params) sbOut.append(p.getParamKey().replaceAll(",", "_")).append(",");
                    sbOut.append("\n");
                    sbOut.append(",");
                    for (ParamVO p : params) sbOut.append(",");
                    sbOut.append("\n");

                    flushResponse(response, sbOut.toString(), "P_" + partInfo.getPartName().originOf() + "_" + filterType + ".csv");
                } else {
                    if (!filterType.equals("A")) {
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

                        flushResponse(response, sbOut.toString(), "P_" + partInfo.getPartName().originOf() + "_" + filterType + ".csv");
                    } else {
                        // N,L,H,B making and compress.
                        String[] filterTypes = new String[]{"N", "L", "H", "B"};
                        List<File> fileOut = new ArrayList<>();

                        for (String ft : filterTypes) {
                            listSet = zsetOps.rangeByScore("P" + partInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);
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
                                        "P" + partInfo.getSeq().originOf() + "." + ft + p.getReferenceSeq(), startRowAt + rankFrom, startRowAt + rankTo);

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

                            try {
                                File fSave = new File(processPath, "P_" + partInfo.getPartName().originOf() + "_" + ft + ".csv");
                                FileOutputStream fos = new FileOutputStream(fSave);
                                BufferedWriter bw = new BufferedWriter(new OutputStreamWriter(fos));
                                bw.write(sbOut.toString(), 0, sbOut.length());
                                bw.flush();
                                bw.close();
                                fileOut.add(fSave);
                            } catch (IOException ioe) {
                                ioe.printStackTrace();
                            }
                        }

                        flushResponseZip(response, fileOut, "P_" + partInfo.getPartName().originOf() + ".zip");
                    }
                }
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
            } else {
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
            } catch (IOException ioe) {
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
            } catch (Exception e) {
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
            } else {
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

                List<ParamVO> notMappedParams = getService(ParamService.class).getNotMappedParams(partInfo.getUploadSeq());
                if (notMappedParams != null && notMappedParams.size() > 0) {
                    for (ParamVO p : notMappedParams)
                        params.add(p);
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
            } else {
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

            // all params.
            List<CryptoField> paramList = getService(ParamService.class).getShortBlockParamList(blockSeq);
            if (paramList == null) paramList = new ArrayList<>();

            for (CryptoField seq : paramList) {
                ShortBlockVO.Param sparam = getService(ParamService.class).getShortBlockParamBySeq(seq);
                ParamVO param = null;

                List<ParamVO> presetParams = getService(ParamService.class).getPresetParamList(
                        partInfo.getPresetPack(), partInfo.getPresetSeq(), sparam.getParamPack(), sparam.getParamSeq(), 1, 9999);
                if (presetParams == null) presetParams = new ArrayList<>();

                for (ParamVO p : presetParams) {
                    logger.debug("[[[[[ " + p.getReferenceSeq() + ", " + sparam.getUnionParamSeq() + " ]]]]]");
                    if (p.getReferenceSeq().longValue() == sparam.getUnionParamSeq().longValue()) {
                        param = p;
                        break;
                    }
                }
                if (param != null)
                    params.add(param);
            }

            List<ParamVO> notMappedParams = getService(ParamService.class).getNotMappedParams(partInfo.getUploadSeq());
            if (notMappedParams != null && notMappedParams.size() > 0) {
                for (ParamVO p : notMappedParams) {
                    params.add(p);
                }
            }

            List<ParamVO> activeParams = new ArrayList<>();
            if (jarrParams != null && jarrParams.size() > 0) {
                for (int i = 0; i < jarrParams.size(); i++) {
                    String paramSeq = jarrParams.get(i).getAsString();
                    for (ParamVO p : params) {
                        if (p.getSeq().valueOf().equals(paramSeq)) {
                            activeParams.add(p);
                            break;
                        }
                    }
                }
            } else {
                activeParams = new ArrayList<>(params);
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
                jobjResult.add("paramSet", ServerConstants.GSON.toJsonTree(activeParams));
                jobjResult.add("julianSet", ServerConstants.GSON.toJsonTree(new ArrayList<String>()));
                jobjResult.add("data", ServerConstants.GSON.toJsonTree(rowData));

                StringBuilder sbOut = new StringBuilder();
                sbOut.append("DATE,");
                for (ParamVO p : params) sbOut.append(p.getParamKey().replaceAll(",", "_")).append(",");
                sbOut.append("\n");
                sbOut.append(",");
                for (ParamVO p : params) sbOut.append(",");
                sbOut.append("\n");

                flushResponse(response, sbOut.toString(), "S_" + blockInfo.getBlockName().originOf() + "_" + filterType + ".csv");
            } else {
                Set<String> listSet = zsetOps.rangeByScore("S" + blockInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);
                if (listSet == null || listSet.size() == 0) {
                    jobjResult.add("paramSet", ServerConstants.GSON.toJsonTree(activeParams));
                    jobjResult.add("julianSet", ServerConstants.GSON.toJsonTree(new ArrayList<String>()));
                    jobjResult.add("data", ServerConstants.GSON.toJsonTree(rowData));

                    StringBuilder sbOut = new StringBuilder();
                    sbOut.append("DATE,");
                    for (ParamVO p : params) sbOut.append(p.getParamKey().replaceAll(",", "_")).append(",");
                    sbOut.append("\n");
                    sbOut.append(",");
                    for (ParamVO p : params) sbOut.append(",");
                    sbOut.append("\n");

                    flushResponse(response, sbOut.toString(), "S_" + blockInfo.getBlockName().originOf() + "_" + filterType + ".csv");
                } else {
                    if (!(filterType.equals("A") || filterType.equals("Z"))) {
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
                        for (ParamVO p : activeParams) {
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

                        flushResponse(response, sbOut.toString(), "S_" + blockInfo.getBlockName().originOf() + "_" + filterType + ".csv");
                    }
                    else if (filterType.equals("A")) {
                        // N,L,H,B making and compress.
                        String[] filterTypes = new String[]{"N", "L", "H", "B"};
                        List<File> fileOut = new ArrayList<>();

                        for (String ft : filterTypes) {
                            listSet = zsetOps.rangeByScore("S" + blockInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);

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
                            for (ParamVO p : activeParams) {
                                listSet = zsetOps.rangeByScore(
                                        "S" + blockInfo.getSeq().originOf() + "." + ft + p.getReferenceSeq(), startRowAt + rankFrom, startRowAt + rankTo);

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

                            try {
                                File fSave = new File(processPath, "S_" + blockInfo.getBlockName().originOf() + "_" + ft + ".csv");
                                FileOutputStream fos = new FileOutputStream(fSave);
                                BufferedWriter bw = new BufferedWriter(new OutputStreamWriter(fos));
                                bw.write(sbOut.toString(), 0, sbOut.length());
                                bw.flush();
                                bw.close();
                                fileOut.add(fSave);
                            } catch (IOException ioe) {
                                ioe.printStackTrace();
                            }
                        }

                        flushResponseZip(response, fileOut, "S_" + blockInfo.getBlockName().originOf() + ".zip");
                    }
                    else if (filterType.equals("Z")) {
                        List<ShortBlockVO> blocks = getService(PartService.class).getShortBlockList(user.getUid(), partInfo.getSeq(), 1, 99999);
                        List<File> fileOut = new ArrayList<>();

                        for (ShortBlockVO block : blocks) {
                            // N,L,H,B making and compress.
                            String[] filterTypes = new String[]{"N", "L", "H", "B"};

                            for (String ft : filterTypes) {
                                listSet = zsetOps.rangeByScore("S" + block.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);

                                String julianStart = listSet.iterator().next();
                                Long startRowAt = zsetOps.score("S" + block.getSeq().originOf() + ".R", julianStart).longValue();

                                Long rankFrom = zsetOps.rank("S" + block.getSeq().originOf() + ".R", julianFrom);
                                if (rankFrom == null) {
                                    julianFrom = zsetOps.rangeByScore("S" + block.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE).iterator().next();
                                    rankFrom = zsetOps.rank("S" + block.getSeq().originOf() + ".R", julianFrom);
                                }
                                Long rankTo = zsetOps.rank("S" + block.getSeq().originOf() + ".R", julianTo);
                                if (rankTo == null) {
                                    julianTo = zsetOps.reverseRangeByScore("S" + block.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE).iterator().next();
                                    rankTo = zsetOps.rank("S" + block.getSeq().originOf() + ".R", julianTo);
                                }

                                rowData = new LinkedHashMap<>();
                                for (ParamVO p : activeParams) {
                                    listSet = zsetOps.rangeByScore(
                                            "S" + block.getSeq().originOf() + "." + ft + p.getReferenceSeq(), startRowAt + rankFrom, startRowAt + rankTo);

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

                                try {
                                    File fSave = new File(processPath, "S_" + block.getBlockName().originOf() + "_" + ft + ".csv");
                                    FileOutputStream fos = new FileOutputStream(fSave);
                                    BufferedWriter bw = new BufferedWriter(new OutputStreamWriter(fos));
                                    bw.write(sbOut.toString(), 0, sbOut.length());
                                    bw.flush();
                                    bw.close();
                                    fileOut.add(fSave);
                                } catch (IOException ioe) {
                                    ioe.printStackTrace();
                                }
                            }
                        }

                        flushResponseZip(response, fileOut, "P_" + partInfo.getPartName().originOf() + "_shortblocks.zip");
                    }
                }
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

            // all params.
            List<CryptoField> paramList = getService(ParamService.class).getShortBlockParamList(blockSeq);
            if (paramList == null) paramList = new ArrayList<>();

            for (CryptoField seq : paramList) {
                ShortBlockVO.Param sparam = getService(ParamService.class).getShortBlockParamBySeq(seq);
                ParamVO param = null;

                List<ParamVO> presetParams = getService(ParamService.class).getPresetParamList(
                        partInfo.getPresetPack(), partInfo.getPresetSeq(), sparam.getParamPack(), sparam.getParamSeq(), 1, 9999);
                if (presetParams == null) presetParams = new ArrayList<>();

                for (ParamVO p : presetParams) {
                    logger.debug("[[[[[ " + p.getReferenceSeq() + ", " + sparam.getUnionParamSeq() + " ]]]]]");
                    if (p.getReferenceSeq().longValue() == sparam.getUnionParamSeq().longValue()) {
                        param = p;
                        break;
                    }
                }
                if (param != null)
                    params.add(param);
            }

            List<ParamVO> notMappedParams = getService(ParamService.class).getNotMappedParams(partInfo.getUploadSeq());
            if (notMappedParams != null && notMappedParams.size() > 0) {
                for (ParamVO p : notMappedParams) {
                    params.add(p);
                }
            }

            List<ParamVO> activeParams = new ArrayList<>();
            if (jarrParams != null && jarrParams.size() > 0) {
                for (int i = 0; i < jarrParams.size(); i++) {
                    String paramSeq = jarrParams.get(i).getAsString();
                    for (ParamVO p : params) {
                        if (p.getSeq().valueOf().equals(paramSeq)) {
                            activeParams.add(p);
                            break;
                        }
                    }
                }
            } else {
                activeParams = new ArrayList<>(params);
            }

            List<String> julianData = new ArrayList<>();
            LinkedHashMap<String, List<Double>> paramData = new LinkedHashMap<>();
            for (ParamVO p : activeParams) {
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
            } catch (IOException ioe) {
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

            for (CryptoField seq : paramList) {
                ShortBlockVO.Param sparam = getService(ParamService.class).getShortBlockParamBySeq(seq);
                ParamVO param = null;

                List<ParamVO> presetParams = getService(ParamService.class).getPresetParamList(
                        partInfo.getPresetPack(), partInfo.getPresetSeq(), sparam.getParamPack(), sparam.getParamSeq(), 1, 9999);
                if (presetParams == null) presetParams = new ArrayList<>();

                for (ParamVO p : presetParams) {
                    logger.debug("[[[[[ " + p.getReferenceSeq() + ", " + sparam.getUnionParamSeq() + " ]]]]]");
                    if (p.getReferenceSeq().longValue() == sparam.getUnionParamSeq().longValue()) {
                        param = p;
                        break;
                    }
                    //if (p.getParamPack().equals(sparam.getParamPack()) && p.getSeq().equals(sparam.getParamSeq())) {
                    //    param = p;
                    //    break;
                    //}
                }
                if (param != null) {
                    resultParams.add(param);

                    param.setParamValueMap(getService(PartService.class).getShortBlockParamData(
                            blockInfo.getBlockMetaSeq(), blockInfo.getSeq(), param.getReferenceSeq()));
                }
            }

            List<ParamVO> notMappedParams = getService(ParamService.class).getNotMappedParams(partInfo.getUploadSeq());
            if (notMappedParams != null && notMappedParams.size() > 0) {
                for (ParamVO p : notMappedParams) {
                    paramList.add(p.getSeq());
                    resultParams.add(p);

                    p.setParamValueMap(getService(PartService.class).getShortBlockParamData(
                            blockInfo.getBlockMetaSeq(), blockInfo.getSeq(), p.getReferenceSeq()));
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

            // all params.
            List<CryptoField> paramList = getService(ParamService.class).getShortBlockParamList(blockSeq);
            if (paramList == null) paramList = new ArrayList<>();

            for (CryptoField seq : paramList) {
                ShortBlockVO.Param sparam = getService(ParamService.class).getShortBlockParamBySeq(seq);
                ParamVO param = null;

                List<ParamVO> presetParams = getService(ParamService.class).getPresetParamList(
                        partInfo.getPresetPack(), partInfo.getPresetSeq(), sparam.getParamPack(), sparam.getParamSeq(), 1, 9999);
                if (presetParams == null) presetParams = new ArrayList<>();

                for (ParamVO p : presetParams) {
                    logger.debug("[[[[[ " + p.getReferenceSeq() + ", " + sparam.getUnionParamSeq() + " ]]]]]");
                    if (p.getReferenceSeq().longValue() == sparam.getUnionParamSeq().longValue()) {
                        param = p;
                        break;
                    }
                }
                if (param != null)
                    params.add(param);
            }

            List<ParamVO> notMappedParams = getService(ParamService.class).getNotMappedParams(partInfo.getUploadSeq());
            if (notMappedParams != null && notMappedParams.size() > 0) {
                for (ParamVO p : notMappedParams) {
                    params.add(p);
                }
            }

            List<ParamVO> activeParams = new ArrayList<>();
            if (jarrParams != null && jarrParams.size() > 0) {
                for (int i = 0; i < jarrParams.size(); i++) {
                    String paramSeq = jarrParams.get(i).getAsString();
                    for (ParamVO p : params) {
                        if (p.getSeq().valueOf().equals(paramSeq)) {
                            activeParams.add(p);
                            break;
                        }
                    }
                }
            } else {
                activeParams = new ArrayList<>(params);
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
                jobjResult.add("paramSet", ServerConstants.GSON.toJsonTree(activeParams));
                jobjResult.add("julianSet", ServerConstants.GSON.toJsonTree(new ArrayList<String>()));
                jobjResult.add("data", ServerConstants.GSON.toJsonTree(new LinkedHashMap<String, List<Double>>()));
                return ResponseHelper.response(200, "Success - rowData", jobjResult);
            }

            Set<String> listSet = zsetOps.rangeByScore("S" + blockInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);
            if (listSet == null || listSet.size() == 0) {
                jobjResult.add("paramSet", ServerConstants.GSON.toJsonTree(activeParams));
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
            for (ParamVO p : activeParams) {
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

            jobjResult.add("paramSet", ServerConstants.GSON.toJsonTree(activeParams));
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

            // all params.
            List<CryptoField> paramList = getService(ParamService.class).getShortBlockParamList(blockSeq);
            if (paramList == null) paramList = new ArrayList<>();

            for (CryptoField seq : paramList) {
                ShortBlockVO.Param sparam = getService(ParamService.class).getShortBlockParamBySeq(seq);
                ParamVO param = null;

                List<ParamVO> presetParams = getService(ParamService.class).getPresetParamList(
                        partInfo.getPresetPack(), partInfo.getPresetSeq(), sparam.getParamPack(), sparam.getParamSeq(), 1, 9999);
                if (presetParams == null) presetParams = new ArrayList<>();

                for (ParamVO p : presetParams) {
                    logger.debug("[[[[[ " + p.getReferenceSeq() + ", " + sparam.getUnionParamSeq() + " ]]]]]");
                    if (p.getReferenceSeq().longValue() == sparam.getUnionParamSeq().longValue()) {
                        param = p;
                        break;
                    }
                }
                if (param != null)
                    params.add(param);
            }

            List<ParamVO> notMappedParams = getService(ParamService.class).getNotMappedParams(partInfo.getUploadSeq());
            if (notMappedParams != null && notMappedParams.size() > 0) {
                for (ParamVO p : notMappedParams) {
                    params.add(p);
                }
            }

            List<ParamVO> activeParams = new ArrayList<>();
            if (jarrParams != null && jarrParams.size() > 0) {
                for (int i = 0; i < jarrParams.size(); i++) {
                    String paramSeq = jarrParams.get(i).getAsString();
                    for (ParamVO p : params) {
                        if (p.getSeq().valueOf().equals(paramSeq)) {
                            activeParams.add(p);
                            break;
                        }
                    }
                }
            } else {
                activeParams = new ArrayList<>(params);
            }

            List<String> julianData = new ArrayList<>();
            LinkedHashMap<String, List<Double>> paramData = new LinkedHashMap<>();
            for (ParamVO p : activeParams) {
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

    private int getDataCount(String sourceType, CryptoField sourceSeq, CryptoField paramSeq) {
        return 0;
    }

}
