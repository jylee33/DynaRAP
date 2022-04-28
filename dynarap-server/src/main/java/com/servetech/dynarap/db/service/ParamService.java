package com.servetech.dynarap.db.service;

import com.google.gson.JsonObject;
import com.servetech.dynarap.config.ServerConstants;
import com.servetech.dynarap.controller.ApiController;
import com.servetech.dynarap.db.mapper.ParamMapper;
import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.ext.HandledServiceException;
import com.servetech.dynarap.vo.DirVO;
import com.servetech.dynarap.vo.ParamVO;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@Service("paramService")
public class ParamService {

    @Autowired
    private ParamMapper paramMapper;

    // 활성 파라미터 페이징 리스트
    public List<ParamVO> getParamList(int pageNo, int pageSize) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("startIndex", (pageNo - 1) * pageSize);
            params.put("pageSize", pageSize);
            List<ParamVO> paramList = paramMapper.selectParamList(params);
            if (paramList == null) paramList = new ArrayList<>();
            return paramList;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public List<ParamVO> getParamListByGroup(CryptoField paramGroupSeq, int pageNo, int pageSize) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("paramGroupSeq", paramGroupSeq);
            params.put("startIndex", (pageNo - 1) * pageSize);
            params.put("pageSize", pageSize);
            List<ParamVO> paramList = paramMapper.selectParamListByGroup(params);
            if (paramList == null) paramList = new ArrayList<>();
            return paramList;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public ParamVO getActiveParam(CryptoField paramPack) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("paramPack", paramPack);
            return paramMapper.selectActiveParam(params);
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
            return paramList;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public ParamVO getParamBySeq(CryptoField paramSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", paramSeq);
            return paramMapper.selectParamBySeq(params);
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

            return param;
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

    public List<ParamVO.Group> getParamGroupList() throws HandledServiceException {
        try {
            return paramMapper.selectParamGroupList();
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public ParamVO.Group getParamGroupBySeq(CryptoField groupSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", groupSeq);
            return paramMapper.selectParamGroupBySeq(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public ParamVO.Group insertParamGroup(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            ParamVO.Group paramGroup = ServerConstants.GSON.fromJson(payload, ParamVO.Group.class);
            if (paramGroup == null) {
                throw new HandledServiceException(411, "요청 내용이 파라미터 그룹 형식에 맞지 않습니다.");
            }

            paramGroup.setRegisterUid(uid);
            paramGroup.setCreatedAt(LongDate.now());
            paramMapper.insertParamGroup(paramGroup);

            return paramGroup;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public ParamVO.Group updateParamGroup(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            ParamVO.Group paramGroup = ServerConstants.GSON.fromJson(payload, ParamVO.Group.class);
            if (paramGroup == null || paramGroup.getSeq() == null || paramGroup.getSeq().isEmpty()) {
                throw new HandledServiceException(411, "요청 내용이 파라미터 그룹 형식에 맞지 않습니다.");
            }

            paramMapper.updateParamGroup(paramGroup);

            return paramGroup;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

}
