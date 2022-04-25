package com.servetech.dynarap.db.service;

import com.google.gson.JsonObject;
import com.servetech.dynarap.config.ServerConstants;
import com.servetech.dynarap.controller.ApiController;
import com.servetech.dynarap.db.mapper.DLLMapper;
import com.servetech.dynarap.db.mapper.FlightMapper;
import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.ext.HandledServiceException;
import com.servetech.dynarap.vo.DLLVO;
import com.servetech.dynarap.vo.FlightVO;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

@Service("dllService")
public class DLLService {

    @Autowired
    private DLLMapper dllMapper;

    public List<DLLVO> getDLLList() throws HandledServiceException {
        try {
            return dllMapper.selectDLLList();
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void insertDLL(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            DLLVO dll = ServerConstants.GSON.fromJson(payload, DLLVO.class);
            if (dll == null)
                throw new HandledServiceException(411, "요청 내용이 DLL 형식에 맞지 않습니다.");

            dll.setCreatedAt(LongDate.now());
            dll.setRegisterUid(uid);

            dllMapper.insertDLL(dll);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void updateDLL(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            DLLVO dll = ServerConstants.GSON.fromJson(payload, DLLVO.class);
            if (dll == null || dll.getSeq() == null || dll.getSeq().isEmpty())
                throw new HandledServiceException(411, "요청 파라미터 오류입니다. [필수 파라미터 누락]");

            dllMapper.updateDLL(dll);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteDLL(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            CryptoField dllSeq = CryptoField.LZERO;
            if (!ApiController.checkJsonEmpty(payload, "seq"))
                dllSeq = CryptoField.decode(payload.get("seq").getAsString(), 0L);

            if (dllSeq == null || dllSeq.isEmpty())
                throw new HandledServiceException(411, "요청 파라미터 오류입니다. [필수 파라미터 누락]");

            Map<String, Object> params = new HashMap<>();
            params.put("seq", dllSeq);

            dllMapper.deleteDLL(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public List<DLLVO.Param> getDLLParamList(CryptoField dllSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("dllSeq", dllSeq);
            return dllMapper.selectDLLParamList(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void insertDLLParam(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            DLLVO.Param dllParam = ServerConstants.GSON.fromJson(payload, DLLVO.Param.class);
            if (dllParam == null)
                throw new HandledServiceException(411, "요청 내용이 DLL 파라미터 형식에 맞지 않습니다.");

            dllParam.setRegisterUid(uid);

            dllMapper.insertDLLParam(dllParam);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void updateDLLParam(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            DLLVO.Param dllParam = ServerConstants.GSON.fromJson(payload, DLLVO.Param.class);
            if (dllParam == null || dllParam.getSeq() == null || dllParam.getSeq().isEmpty())
                throw new HandledServiceException(411, "요청 파라미터 오류입니다. [필수 파라미터 누락]");

            dllMapper.updateDLLParam(dllParam);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteDLLParam(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            CryptoField dllParamSeq = CryptoField.LZERO;
            if (!ApiController.checkJsonEmpty(payload, "seq"))
                dllParamSeq = CryptoField.decode(payload.get("seq").getAsString(), 0L);

            if (dllParamSeq == null || dllParamSeq.isEmpty())
                throw new HandledServiceException(411, "요청 파라미터 오류입니다. [필수 파라미터 누락]");

            Map<String, Object> params = new HashMap<>();
            params.put("seq", dllParamSeq);

            dllMapper.deleteDLLParam(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteDLLParamByMulti(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            CryptoField dllSeq = CryptoField.LZERO;
            if (!ApiController.checkJsonEmpty(payload, "dllSeq"))
                dllSeq = CryptoField.decode(payload.get("dllSeq").getAsString(), 0L);

            if (dllSeq == null || dllSeq.isEmpty())
                throw new HandledServiceException(411, "요청 파라미터 오류입니다. [필수 파라미터 누락]");

            Map<String, Object> params = new HashMap<>();
            params.put("dllSeq", dllSeq);

            dllMapper.deleteDLLParamByMulti(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

}
