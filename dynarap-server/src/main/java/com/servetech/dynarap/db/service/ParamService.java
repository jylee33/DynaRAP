package com.servetech.dynarap.db.service;

import com.google.gson.JsonObject;
import com.servetech.dynarap.config.ServerConstants;
import com.servetech.dynarap.controller.ApiController;
import com.servetech.dynarap.db.mapper.ParamMapper;
import com.servetech.dynarap.db.mapper.PartMapper;
import com.servetech.dynarap.db.mapper.RawMapper;
import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.ext.HandledServiceException;
import com.servetech.dynarap.vo.ParamVO;
import com.servetech.dynarap.vo.PartVO;
import com.servetech.dynarap.vo.PresetVO;
import com.servetech.dynarap.vo.RawVO;
import org.apache.catalina.util.ParameterMap;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.*;

@Service("paramService")
public class ParamService {

    @Autowired
    private ParamMapper paramMapper;

    @Autowired
    private PartMapper partMapper;

    @Autowired
    private RawMapper rawMapper;

    // 활성 파라미터 페이징 리스트
    public List<ParamVO> getParamList(String keyword, int pageNo, int pageSize) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("startIndex", (pageNo - 1) * pageSize);
            params.put("pageSize", pageSize);
            params.put("keyword", keyword);

            List<ParamVO> paramList = paramMapper.selectParamList(params);
            if (paramList == null) paramList = new ArrayList<>();
            for (ParamVO param : paramList) {
                params.put("paramPack", param.getParamPack());
                params.put("propSeq", param.getPropSeq());
                param.setPropInfo(paramMapper.selectParamPropBySeq(params));
                param.setExtras(getParamExtraMap(param.getParamPack()));
            }
            return paramList;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public int getParamCount(String keyword) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("keyword", keyword);
            int paramCount = paramMapper.selectParamCount(params);
            return paramCount;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public List<ParamVO> getParamListByProp(CryptoField propSeq, int pageNo, int pageSize) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("propSeq", propSeq);
            params.put("startIndex", (pageNo - 1) * pageSize);
            params.put("pageSize", pageSize);
            List<ParamVO> paramList = paramMapper.selectParamListByProp(params);
            if (paramList == null) paramList = new ArrayList<>();
            for (ParamVO param : paramList) {
                params.put("paramPack", param.getParamPack());
                params.put("propSeq", param.getPropSeq());
                param.setPropInfo(paramMapper.selectParamPropBySeq(params));
                param.setExtras(getParamExtraMap(param.getParamPack()));
            }
            return paramList;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public ParamVO getActiveParam(CryptoField paramPack) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("paramPack", paramPack);
            ParamVO paramInfo = paramMapper.selectActiveParam(params);
            if (paramInfo != null) {
                params.put("propSeq", paramInfo.getPropSeq());
                paramInfo.setPropInfo(paramMapper.selectParamPropBySeq(params));
                paramInfo.setExtras(getParamExtraMap(paramInfo.getParamPack()));
            }
            return paramInfo;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public List<ParamVO> getParamPackList(CryptoField paramPack) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("paramPack", paramPack);
            List<ParamVO> paramList = paramMapper.selectParamPackList(params);
            if (paramList == null) paramList = new ArrayList<>();
            for (ParamVO param : paramList) {
                params.put("paramPack", param.getParamPack());
                params.put("propSeq", param.getPropSeq());
                param.setPropInfo(paramMapper.selectParamPropBySeq(params));
                param.setExtras(getParamExtraMap(param.getParamPack()));
            }
            return paramList;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public ParamVO getParamBySeq(CryptoField paramSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", paramSeq);
            ParamVO paramInfo = paramMapper.selectParamBySeq(params);
            if (paramInfo != null) {
                params.put("propSeq", paramInfo.getPropSeq());
                paramInfo.setPropInfo(paramMapper.selectParamPropBySeq(params));
                paramInfo.setExtras(getParamExtraMap(paramInfo.getParamPack()));
            }
            return paramInfo;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public ParamVO getParamByParamKey(String dataType, String paramKey) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("dataTypeKey", dataType + "Key");
            params.put("paramKey", paramKey);

            ParamVO paramInfo = paramMapper.selectParamByParamKey(params);
            if (paramInfo != null) {
                params.put("propSeq", paramInfo.getPropSeq());
                paramInfo.setPropInfo(paramMapper.selectParamPropBySeq(params));
                paramInfo.setExtras(getParamExtraMap(paramInfo.getParamPack()));
            }
            return paramInfo;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public ParamVO insertParam(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            ParamVO param = ServerConstants.GSON.fromJson(payload, ParamVO.class);
            if (param == null)
                throw new HandledServiceException(411, "요청 내용이 파라미터 형식에 맞지 않습니다.");

            param.setRegisterUid(uid);
            param.setAppliedAt(LongDate.now());
            param.setAppliedEndAt(LongDate.zero());
            paramMapper.insertParam(param);

            param.setParamPack(param.getSeq());
            paramMapper.updateParamNoRenew(param);

            // extra 처리.
            if (param.getExtras() != null) {
                Set<String> keys = param.getExtras().keySet();
                Iterator<String> iterKeys = keys.iterator();
                while (iterKeys.hasNext()) {
                    String key = iterKeys.next();

                    Map<String, Object> params = new HashMap<>();
                    params.put("paramPack", param.getParamPack());
                    params.put("extraKey", key);
                    params.put("extraValue", param.getExtras().get(key));
                    paramMapper.insertParamExtra(params);
                }
                param.setExtras(getParamExtraMap(param.getParamPack()));
            }

            if (param.getPropSeq() != null && !param.getPropSeq().isEmpty())
                param.setPropInfo(getParamPropBySeq(param.getPropSeq()));

            return param;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public List<PresetVO> getPresetByParamPack(CryptoField paramPack, CryptoField oldParamSeq) throws HandledServiceException
    {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("paramPack", paramPack);
            params.put("oldParamSeq", oldParamSeq);
            return paramMapper.selectPresetByParamPack(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public ParamVO insertParam(ParamVO param) throws HandledServiceException {
        try {
            ParamVO oldParam = getActiveParam(param.getParamPack());
            if (oldParam != null) {
                oldParam.setAppliedEndAt(LongDate.now());
                paramMapper.updateParam(oldParam);
            }

            param.setAppliedAt(LongDate.now());
            param.setAppliedEndAt(LongDate.zero());
            paramMapper.insertParam(param);

            if (param.getParamPack() == null || param.getParamPack().isEmpty()) {
                param.setParamPack(param.getSeq());
                paramMapper.updateParamNoRenew(param);
            }

            // preset 에서 param을 가지고 있는 경우 처리.
            List<PresetVO> presets = getPresetByParamPack(param.getParamPack(), oldParam.getSeq());
            if (presets != null && presets.size() > 0) {
                for (PresetVO preset : presets) {
                    CryptoField oldSeq = preset.getSeq();

                    preset = insertPreset(preset);
                    if (!preset.getSeq().equals(oldSeq)) {
                        // parameter copy
                        List<ParamVO> presetParams = getPresetParamList(preset.getPresetPack(), oldSeq, null, null, 1, 999999);
                        if (presetParams == null) presetParams = new ArrayList<>();
                        for (ParamVO p : presetParams) {
                            PresetVO.Param pparam = new PresetVO.Param();
                            pparam.setPresetPack(preset.getPresetPack());
                            pparam.setPresetSeq(preset.getSeq());
                            if (p.getParamPack().equals(param.getParamPack())) {
                                pparam.setParamPack(param.getParamPack());
                                pparam.setParamSeq(param.getSeq());
                            }
                            else {
                                pparam.setParamPack(p.getParamPack());
                                pparam.setParamSeq(p.getSeq());
                            }
                            paramMapper.insertPresetParam(pparam);
                        }
                    }
                }
            }

            // extra 처리.
            if (param.getExtras() != null) {
                // 기존 extra 가져오기
                Map<String, Object> oldExtras = getParamExtraMap(param.getParamPack());
                List<String> existKeys = new ArrayList<>();

                Set<String> keys = param.getExtras().keySet();
                Iterator<String> iterKeys = keys.iterator();
                while (iterKeys.hasNext()) {
                    String key = iterKeys.next();
                    existKeys.add(key);
                    insertParamExtra(param.getParamPack(), key, param.getExtras().get(key));
                }

                if (oldExtras != null) {
                    Set<String> oldKeys = oldExtras.keySet();
                    Iterator<String> iterOldKeys = oldKeys.iterator();
                    while (iterOldKeys.hasNext()) {
                        String key = iterOldKeys.next();
                        if (!existKeys.contains(key))
                            deleteParamExtra(param.getParamPack(), key);
                    }
                }

                param.setExtras(getParamExtraMap(param.getParamPack()));
            }

            if (param.getPropSeq() != null && !param.getPropSeq().isEmpty())
                param.setPropInfo(getParamPropBySeq(param.getPropSeq()));

            return param;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public ParamVO updateParam(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            ParamVO param = ServerConstants.GSON.fromJson(payload, ParamVO.class);
            if (param == null || param.getParamPack() == null || param.getParamPack().isEmpty())
                throw new HandledServiceException(411, "요청 내용이 파라미터 형식에 맞지 않습니다.");

            param = insertParam(param);

            return param;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteParam(JsonObject payload) throws HandledServiceException {
        try {
            CryptoField paramPack = CryptoField.LZERO;
            if (!ApiController.checkJsonEmpty(payload, "paramPack"))
                paramPack = CryptoField.decode(payload.get("paramPack").getAsString(), 0L);

            if (paramPack == null || paramPack.isEmpty())
                throw new HandledServiceException(404, "파라미터가 없습니다.");

            ParamVO param = getActiveParam(paramPack);
            param.setAppliedEndAt(LongDate.now());
            paramMapper.updateParam(param);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public ParamVO updateParamNoRenew(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            ParamVO param = ServerConstants.GSON.fromJson(payload, ParamVO.class);
            if (param == null || param.getParamPack() == null || param.getParamPack().isEmpty()
                || param.getSeq() == null || param.getSeq().isEmpty()) {
                throw new HandledServiceException(411, "요청 내용이 파라미터 형식에 맞지 않습니다.");
            }
            paramMapper.updateParamNoRenew(param);
            return param;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }


    public List<ParamVO.Prop> getParamPropList(String propType) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("propType", propType);
            return paramMapper.selectParamPropList(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public ParamVO.Prop getParamPropBySeq(CryptoField propSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("propSeq", propSeq);
            return paramMapper.selectParamPropBySeq(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public ParamVO.Prop insertParamProp(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            ParamVO.Prop paramProp = ServerConstants.GSON.fromJson(payload, ParamVO.Prop.class);
            if (paramProp == null || paramProp.getPropType() == null || paramProp.getPropType().isEmpty()
                || paramProp.getPropCode() == null || paramProp.getPropCode().isEmpty()) {
                throw new HandledServiceException(411, "요청 내용이 파라미터 그룹 형식에 맞지 않습니다. [propType, propCode 필수]");
            }

            Map<String, Object> params = new HashMap<>();
            params.put("propType", paramProp.getPropType());
            params.put("propCode", paramProp.getPropCode());
            ParamVO.Prop oldParamProp = paramMapper.selectParamPropByType(params);
            if (oldParamProp != null) {
                oldParamProp.setDeleted(!oldParamProp.isDeleted());
                if (paramProp.getParamUnit() != null && !paramProp.getParamUnit().isEmpty())
                    oldParamProp.setParamUnit(paramProp.getParamUnit());
                paramMapper.updateParamProp(oldParamProp);
                return oldParamProp;
            }

            paramProp.setRegisterUid(uid);
            paramProp.setCreatedAt(LongDate.now());
            paramMapper.insertParamProp(paramProp);
            paramProp = getParamPropBySeq(paramProp.getSeq());

            return paramProp;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public ParamVO.Prop updateParamProp(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            ParamVO.Prop paramProp = ServerConstants.GSON.fromJson(payload, ParamVO.Prop.class);
            if (paramProp == null || paramProp.getPropType() == null || paramProp.getPropType().isEmpty()
                    || paramProp.getPropCode() == null || paramProp.getPropCode().isEmpty()) {
                throw new HandledServiceException(411, "요청 내용이 파라미터 그룹 형식에 맞지 않습니다. [propType, propCode 필수]");
            }

            Map<String, Object> params = new HashMap<>();
            params.put("propType", paramProp.getPropType());
            params.put("propCode", paramProp.getPropCode());
            ParamVO.Prop oldParamProp = paramMapper.selectParamPropByType(params);
            if (oldParamProp != null) {
                oldParamProp.setDeleted(paramProp.isDeleted());
                if (paramProp.getParamUnit() != null && !paramProp.getParamUnit().isEmpty())
                    oldParamProp.setParamUnit(paramProp.getParamUnit());
                paramMapper.updateParamProp(oldParamProp);

                paramProp = getParamPropBySeq(oldParamProp.getSeq());
            }

            return paramProp;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }


    public List<Map<String, Object>> getParamExtraList(CryptoField paramPack) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("paramPack", paramPack);
            return paramMapper.selectParamExtraList(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public Map<String, Object> getParamExtraMap(CryptoField paramPack) throws HandledServiceException {
        try {
            List<Map<String, Object>> extraList = getParamExtraList(paramPack);
            if (extraList == null) extraList = new ArrayList<>();
            LinkedHashMap<String, Object> extraMap = new LinkedHashMap<>();
            for (Map<String, Object> extra: extraList) {
                extraMap.put(extra.get("extraKey").toString(), extra.get("extraValue"));
            }
            return extraMap;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public Map<String, Object> insertParamExtra(CryptoField paramPack, String key, Object value) throws HandledServiceException {
        try {
            List<Map<String, Object>> extraList = getParamExtraList(paramPack);
            if (extraList == null) extraList = new ArrayList<>();
            Map<String, Object> extraInfo = null;
            for (Map<String, Object> extra : extraList) {
                if (extra.get("extraKey").toString().equals(key)) {
                    extraInfo = extra;
                    break;
                }
            }

            Map<String, Object> params = new HashMap<>();
            params.put("paramPack", paramPack);
            params.put("extraKey", key);
            params.put("extraValue", value);
            if (extraInfo != null) {
                paramMapper.updateParamExtra(params);
            }
            else {
                paramMapper.insertParamExtra(params);
            }
            return extraInfo;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public Map<String, Object> updateParamExtra(CryptoField paramPack, String key, Object value) throws HandledServiceException {
        try {
            List<Map<String, Object>> extraList = getParamExtraList(paramPack);
            if (extraList == null) extraList = new ArrayList<>();
            Map<String, Object> extraInfo = null;
            for (Map<String, Object> extra : extraList) {
                if (extra.get("extraKey").toString().equals(key)) {
                    extraInfo = extra;
                    break;
                }
            }

            if (extraInfo != null) {
                Map<String, Object> params = new HashMap<>();
                params.put("paramPack", paramPack);
                params.put("extraKey", key);
                params.put("extraValue", value);
                paramMapper.updateParamExtra(params);
                extraInfo.put("extraValue", value);
            }
            return extraInfo;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteParamExtra(CryptoField paramPack, String key) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("paramPack", paramPack);
            params.put("extraKey", key);
            paramMapper.deleteParamExtra(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public List<PresetVO> getPresetList(int pageNo, int pageSize) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("startIndex", (pageNo - 1) * pageSize);
            params.put("pageSize", pageSize);
            List<PresetVO> presetList = paramMapper.selectPresetList(params);
            if (presetList == null) presetList = new ArrayList<>();
            return presetList;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public int getPresetCount() throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            int presetCount = paramMapper.selectPresetCount(params);
            return presetCount;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public PresetVO getActivePreset(CryptoField presetPack) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("presetPack", presetPack);
            return paramMapper.selectActivePreset(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public List<PresetVO> getPresetPackList(CryptoField presetPack) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("presetPack", presetPack);
            List<PresetVO> presetList = paramMapper.selectPresetPackList(params);
            if (presetList == null) presetList = new ArrayList<>();
            return presetList;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public PresetVO getPresetBySeq(CryptoField presetSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", presetSeq);
            return paramMapper.selectPresetBySeq(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public PresetVO insertPreset(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            PresetVO preset = ServerConstants.GSON.fromJson(payload, PresetVO.class);
            if (preset == null)
                throw new HandledServiceException(411, "요청 내용이 프리셋 형식에 맞지 않습니다.");

            preset.setRegisterUid(uid);
            preset.setAppliedAt(LongDate.now());
            preset.setAppliedEndAt(LongDate.zero());
            paramMapper.insertPreset(preset);

            preset.setPresetPack(preset.getSeq());
            preset.setPresetPackFrom(CryptoField.LZERO);
            paramMapper.updatePresetNoRenew(preset);

            return preset;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public PresetVO insertPreset(PresetVO preset) throws HandledServiceException {
        try {
            PresetVO oldPreset = getActivePreset(preset.getPresetPack());
            if (oldPreset != null) {
                oldPreset.setAppliedEndAt(LongDate.now());
                paramMapper.updatePreset(oldPreset);
            }

            preset.setAppliedAt(LongDate.now());
            preset.setAppliedEndAt(LongDate.zero());
            paramMapper.insertPreset(preset);

            if (preset.getPresetPack() == null || preset.getPresetPack().isEmpty()) {
                preset.setPresetPack(preset.getSeq());
                paramMapper.updatePresetNoRenew(preset);
            }

            return preset;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public PresetVO updatePreset(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            PresetVO preset = ServerConstants.GSON.fromJson(payload, PresetVO.class);
            if (preset == null || preset.getPresetPack() == null || preset.getPresetPack().isEmpty())
                throw new HandledServiceException(411, "요청 내용이 프리셋 형식에 맞지 않습니다.");

            CryptoField oldSeq = preset.getSeq();

            preset = insertPreset(preset);
            if (!preset.getSeq().equals(oldSeq)) {
                // parameter copy
                List<ParamVO> presetParams = getPresetParamList(preset.getPresetPack(), oldSeq, null, null, 1, 999999);
                if (presetParams == null) presetParams = new ArrayList<>();
                for (ParamVO p : presetParams) {
                    PresetVO.Param pparam = new PresetVO.Param();
                    pparam.setPresetPack(preset.getPresetPack());
                    pparam.setPresetSeq(preset.getSeq());
                    pparam.setParamPack(p.getParamPack());
                    pparam.setParamSeq(p.getSeq());
                    paramMapper.insertPresetParam(pparam);
                }
            }

            return preset;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deletePreset(JsonObject payload) throws HandledServiceException {
        try {
            CryptoField presetPack = CryptoField.LZERO;
            if (!ApiController.checkJsonEmpty(payload, "presetPack"))
                presetPack = CryptoField.decode(payload.get("presetPack").getAsString(), 0L);

            if (presetPack == null || presetPack.isEmpty())
                throw new HandledServiceException(404, "파라미터가 없습니다.");

            PresetVO preset = getActivePreset(presetPack);
            preset.setAppliedEndAt(LongDate.now());
            paramMapper.updatePreset(preset);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public PresetVO updatePresetNoRenew(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            PresetVO preset = ServerConstants.GSON.fromJson(payload, PresetVO.class);
            if (preset == null || preset.getPresetPack() == null || preset.getPresetPack().isEmpty()
                    || preset.getSeq() == null || preset.getSeq().isEmpty()) {
                throw new HandledServiceException(411, "요청 내용이 프리셋 형식에 맞지 않습니다.");
            }

            paramMapper.updatePresetNoRenew(preset);
            return preset;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public ParamVO getParamByReference(CryptoField partSeq, CryptoField paramPack, CryptoField paramSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", partSeq);
            PartVO part = partMapper.selectPartBySeq(params);

            params.clear();
            params.put("uploadSeq", part.getUploadSeq());
            RawVO.Upload uploadInfo = rawMapper.selectRawUploadBySeq(params);

            params.put("presetPack", uploadInfo.getPresetPack());
            params.put("presetSeq", uploadInfo.getPresetSeq());
            params.put("paramPack", paramPack);

            ParamVO current = paramMapper.selectActiveParam(params);
            if (paramSeq == null || paramSeq.isEmpty()) {
                paramSeq = current.getSeq();
            }
            params.put("paramSeq", paramSeq);

            Long referenceSeq = paramMapper.selectReferenceSeq(params);
            current.setReferenceSeq(referenceSeq);

            params.put("propSeq", current.getPropSeq());
            current.setPropInfo(paramMapper.selectParamPropBySeq(params));
            current.setExtras(getParamExtraMap(current.getParamPack()));

            return current;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public List<ParamVO> getPresetParamList(CryptoField presetPack, CryptoField presetSeq,
                                             CryptoField paramPack, CryptoField paramSeq,
                                             int pageNo, int pageSize) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("presetPack", presetPack);
            params.put("presetSeq", presetSeq == null || presetSeq.isEmpty() ? null : presetSeq);

            if (paramPack != null && !paramPack.isEmpty()) {
                params.put("paramPack", paramPack == null || paramPack.isEmpty() ? null : paramPack);
                params.put("paramSeq", paramSeq == null || paramSeq.isEmpty() ? null : paramSeq);
            }
            else {
                params.put("paramPack", null);
                params.put("paramSeq", null);
            }

            params.put("startIndex", (pageNo - 1) * pageSize);
            params.put("pageSize", pageSize);
            List<ParamVO> paramList = paramMapper.selectPresetParamList(params);

            if (paramList == null) paramList = new ArrayList<>();
            for (ParamVO param : paramList) {
                params.put("paramPack", param.getParamPack());
                params.put("propSeq", param.getPropSeq());
                param.setPropInfo(paramMapper.selectParamPropBySeq(params));
                param.setExtras(getParamExtraMap(param.getParamPack()));
            }
            return paramList;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public int getPresetParamCount(CryptoField presetPack, CryptoField presetSeq,
                                            CryptoField paramPack, CryptoField paramSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("presetPack", presetPack);
            params.put("presetSeq", presetSeq == null || presetSeq.isEmpty() ? null : presetSeq);
            if (paramPack != null && !paramPack.isEmpty()) {
                params.put("paramPack", paramPack == null || paramPack.isEmpty() ? null : paramPack);
                params.put("paramSeq", paramSeq == null || paramSeq.isEmpty() ? null : paramSeq);
            }
            else {
                params.put("paramPack", null);
                params.put("paramSeq", null);
            }
            int paramCount = paramMapper.selectPresetParamCount(params);
            return paramCount;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public ParamVO getPresetParamBySeq(CryptoField presetParamSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", presetParamSeq);

            ParamVO paramInfo = paramMapper.selectPresetParamBySeq(params);
            if (paramInfo != null) {
                params.put("paramPack", paramInfo.getParamPack());
                params.put("propSeq", paramInfo.getPropSeq());
                paramInfo.setPropInfo(paramMapper.selectParamPropBySeq(params));
                paramInfo.setExtras(getParamExtraMap(paramInfo.getParamPack()));
            }
            return paramInfo;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public ParamVO getNotMappedParamBySeq(CryptoField notMappedParamSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", notMappedParamSeq);

            ParamVO paramInfo = paramMapper.selectNotMappedParamBySeq(params);
            if (paramInfo != null) {
                params.put("paramPack", paramInfo.getParamPack());
                params.put("propSeq", paramInfo.getPropSeq());
                paramInfo.setPropInfo(paramMapper.selectParamPropBySeq(params));
                paramInfo.setExtras(getParamExtraMap(paramInfo.getParamPack()));
            }
            return paramInfo;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void insertPresetParam(JsonObject payload) throws HandledServiceException {
        try {
            PresetVO.Param presetParam = ServerConstants.GSON.fromJson(payload, PresetVO.Param.class);
            if (presetParam == null || presetParam.getPresetPack() == null || presetParam.getPresetPack().isEmpty()) {
                throw new HandledServiceException(411, "요청 내용이 프리셋 파라미터 형식에 맞지 않습니다.");
            }

            PresetVO preset = null;
            if (presetParam.getPresetSeq() == null || presetParam.getPresetSeq().isEmpty())
                preset = getActivePreset(presetParam.getPresetPack());
            else
                preset = getPresetBySeq(presetParam.getPresetSeq());

            presetParam.setPresetPack(preset.getPresetPack());
            presetParam.setPresetSeq(preset.getSeq());

            // 기존 저장된 내용 체크 후 없다면 저장.
            Map<String, Object> params = new HashMap<>();
            params.put("presetPack", presetParam.getPresetPack());
            params.put("presetSeq", presetParam.getPresetSeq());
            params.put("paramPack", presetParam.getParamPack());
            params.put("paramSeq", presetParam.getParamSeq());
            PresetVO.Param currentParam = paramMapper.selectPresetParam(params);
            if (currentParam == null)
                paramMapper.insertPresetParam(presetParam);

        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deletePresetParam(JsonObject payload) throws HandledServiceException {
        try {
            PresetVO.Param presetParam = ServerConstants.GSON.fromJson(payload, PresetVO.Param.class);
            if (presetParam == null || presetParam.getPresetPack() == null || presetParam.getPresetPack().isEmpty()) {
                throw new HandledServiceException(411, "요청 내용이 프리셋 파라미터 형식에 맞지 않습니다.");
            }

            // FIXME: 삭제에 대한 처리가 결국 preset을 변형하는 거로 처리해야 되는가?

            PresetVO preset = null;
            if (presetParam.getPresetSeq() == null || presetParam.getPresetSeq().isEmpty())
                preset = getActivePreset(presetParam.getPresetPack());
            else
                preset = getPresetBySeq(presetParam.getPresetSeq());

            Map<String, Object> params = new HashMap<>();
            params.put("presetPack", preset.getPresetPack());
            params.put("presetSeq", preset.getSeq());

            if (presetParam.getParamPack() != null && !presetParam.getParamPack().isEmpty()) {
                params.put("paramPack", presetParam.getParamPack());
                params.put("paramSeq", presetParam.getParamSeq());
                paramMapper.deletePresetParam(params);
            }
            else {
                paramMapper.deleteAllPresetParam(params);
            }

        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public List<ParamVO> getNotMappedParams(CryptoField uploadSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("uploadSeq", uploadSeq);
            return paramMapper.selectNotMappedParamList(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public List<CryptoField> getPartParamList(CryptoField partSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("partSeq", partSeq);
            return paramMapper.selectPartParamList(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public List<CryptoField> getShortBlockParamList(CryptoField blockSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("blockSeq", blockSeq);
            return paramMapper.selectShortBlockParamList(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

}
