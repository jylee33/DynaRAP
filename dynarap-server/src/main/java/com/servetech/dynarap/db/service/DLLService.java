package com.servetech.dynarap.db.service;

import com.google.gson.JsonArray;
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

import java.util.ArrayList;
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
    public DLLVO insertDLL(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            DLLVO dll = ServerConstants.GSON.fromJson(payload, DLLVO.class);
            if (dll == null)
                throw new HandledServiceException(411, "요청 내용이 DLL 형식에 맞지 않습니다.");

            dll.setCreatedAt(LongDate.now());
            dll.setRegisterUid(uid);

            dllMapper.insertDLL(dll);

            return dll;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public DLLVO updateDLL(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            DLLVO dll = ServerConstants.GSON.fromJson(payload, DLLVO.class);
            if (dll == null || dll.getSeq() == null || dll.getSeq().isEmpty())
                throw new HandledServiceException(411, "요청 파라미터 오류입니다. [필수 파라미터 누락]");

            dllMapper.updateDLL(dll);

            return dll;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteDLL(JsonObject payload) throws HandledServiceException {
        try {
            CryptoField dllSeq = CryptoField.LZERO;
            if (!ApiController.checkJsonEmpty(payload, "seq"))
                dllSeq = CryptoField.decode(payload.get("seq").getAsString(), 0L);

            if (dllSeq == null || dllSeq.isEmpty())
                throw new HandledServiceException(411, "요청 파라미터 오류입니다. [필수 파라미터 누락]");

            Map<String, Object> params = new HashMap<>();
            params.put("seq", dllSeq);
            params.put("dllSeq", dllSeq);

            // 관련 테이블 삭제
            dllMapper.deleteDLLDataByParam(params);
            dllMapper.deleteDLLParamByMulti(params);

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

    public DLLVO.Param getDLLParamBySeq(CryptoField dllParamSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", dllParamSeq);
            return dllMapper.selectDLLParamBySeq(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public DLLVO.Param insertDLLParam(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            DLLVO.Param dllParam = ServerConstants.GSON.fromJson(payload, DLLVO.Param.class);
            if (dllParam == null || dllParam.getDllSeq() == null || dllParam.getDllSeq().isEmpty())
                throw new HandledServiceException(411, "요청 내용이 DLL 파라미터 형식에 맞지 않습니다.");

            dllParam.setParamNo(127);
            dllParam.setRegisterUid(uid);
            dllMapper.insertDLLParam(dllParam);

            List<DLLVO.Param> dllParams = getDLLParamList(dllParam.getDllSeq());
            if (dllParams == null) dllParams = new ArrayList<>();
            int paramNo = 1;
            for (DLLVO.Param dp : dllParams) {
                dp.setParamNo(paramNo++);
                dllMapper.updateDLLParam(dp);
                if (dp.getSeq().equals(dllParam.getSeq()))
                    dllParam.setParamNo(dp.getParamNo());
            }

            return dllParam;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public DLLVO.Param updateDLLParam(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            DLLVO.Param dllParam = ServerConstants.GSON.fromJson(payload, DLLVO.Param.class);
            if (dllParam == null || dllParam.getSeq() == null || dllParam.getSeq().isEmpty())
                throw new HandledServiceException(411, "요청 파라미터 오류입니다. [필수 파라미터 누락]");

            Map<String, Object> params = new HashMap<>();
            params.put("dllSeq", dllParam.getDllSeq());
            params.put("seq", dllParam.getSeq());
            DLLVO.Param currentDllParam = dllMapper.selectDLLParamBySeq(params);

            if (dllParam.getParamName() != null && !dllParam.getParamName().isEmpty())
                currentDllParam.setParamName(dllParam.getParamName());
            if (dllParam.getParamType() != null && !dllParam.getParamType().isEmpty())
                currentDllParam.setParamType(dllParam.getParamType());

            dllMapper.updateDLLParam(currentDllParam);

            return currentDllParam;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteDLLParam(JsonObject payload) throws HandledServiceException {
        try {
            CryptoField dllParamSeq = CryptoField.LZERO;
            if (!ApiController.checkJsonEmpty(payload, "seq"))
                dllParamSeq = CryptoField.decode(payload.get("seq").getAsString(), 0L);

            if (dllParamSeq == null || dllParamSeq.isEmpty())
                throw new HandledServiceException(411, "요청 파라미터 오류입니다. [필수 파라미터 누락]");

            DLLVO.Param dllParam = getDLLParamBySeq(dllParamSeq);

            Map<String, Object> params = new HashMap<>();
            params.put("seq", dllParamSeq);
            params.put("paramSeq", dllParamSeq);
            params.put("dllSeq", dllParam.getDllSeq());

            // param에 해당하는 data 지우기.
            dllMapper.deleteDLLDataByParam(params);
            dllMapper.deleteDLLParam(params);

            // 파라미터 정보 재조정
            List<DLLVO.Param> dllParams = getDLLParamList(dllParam.getDllSeq());
            if (dllParams == null) dllParams = new ArrayList<>();
            int paramNo = 1;
            for (DLLVO.Param dp : dllParams) {
                dp.setParamNo(paramNo++);
                dllMapper.updateDLLParam(dp);
                if (dp.getSeq().equals(dllParam.getSeq()))
                    dllParam.setParamNo(dp.getParamNo());
            }

        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteDLLParamByMulti(JsonObject payload) throws HandledServiceException {
        try {
            CryptoField dllSeq = CryptoField.LZERO;
            if (!ApiController.checkJsonEmpty(payload, "dllSeq"))
                dllSeq = CryptoField.decode(payload.get("dllSeq").getAsString(), 0L);

            if (dllSeq == null || dllSeq.isEmpty())
                throw new HandledServiceException(411, "요청 파라미터 오류입니다. [필수 파라미터 누락]");

            Map<String, Object> params = new HashMap<>();
            params.put("dllSeq", dllSeq);

            dllMapper.deleteDLLDataByParam(params);

            dllMapper.deleteDLLParamByMulti(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public List<DLLVO.Raw> getDLLData(CryptoField dllSeq, CryptoField paramSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("dllSeq", dllSeq);
            params.put("paramSeq", paramSeq == null || paramSeq.isEmpty() ? null : paramSeq);
            return dllMapper.selectDLLData(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public int getDLLDataMaxRow(CryptoField dllSeq, CryptoField paramSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("dllSeq", dllSeq);
            params.put("paramSeq", paramSeq == null || paramSeq.isEmpty() ? null : paramSeq);
            return dllMapper.selectDLLDataMaxRow(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public List<DLLVO.Raw> insertDLLDataBulk(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            DLLVO.Raw dllRaw = ServerConstants.GSON.fromJson(payload, DLLVO.Raw.class);
            if (dllRaw == null || dllRaw.getDllSeq() == null || dllRaw.getDllSeq().isEmpty()
                    || dllRaw.getParamSeq() == null || dllRaw.getParamSeq().isEmpty()) {
                throw new HandledServiceException(411, "요청 내용이 DLL 데이터 형식에 맞지 않습니다.");
            }

            // 파람 정보
            DLLVO.Param dllParam = getDLLParamBySeq(dllRaw.getParamSeq());

            // 해당 파라미터 데이터 지우기
            deleteDLLDataByParam(payload);

            // 전달된 데이터로 채우기
            JsonArray jarrData = null;
            if (!ApiController.checkJsonEmpty(payload, "data"))
                jarrData = payload.get("data").getAsJsonArray();

            List<DLLVO.Raw> dllData = new ArrayList<>();
            int rowNo = 1;
            for (int i = 0; i < jarrData.size(); i++) {
                DLLVO.Raw ndr = new DLLVO.Raw();
                ndr.setDllSeq(dllRaw.getDllSeq());
                ndr.setRowNo(rowNo++);
                ndr.setParamSeq(dllRaw.getParamSeq());
                if (dllParam.getParamType().equalsIgnoreCase("string"))
                    ndr.setParamValStr(jarrData.get(i).getAsString());
                else
                    ndr.setParamVal(jarrData.get(i).getAsDouble());
                dllMapper.insertDLLData(ndr);
                dllData.add(ndr);
            }

            return dllData;
        } catch (Exception e) {
            throw new HandledServiceException(411, e.getMessage());
        }
    }

    @Transactional
    public DLLVO.Raw insertDLLData(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            DLLVO.Raw dllRaw = ServerConstants.GSON.fromJson(payload, DLLVO.Raw.class);
            if (dllRaw == null || dllRaw.getDllSeq() == null || dllRaw.getDllSeq().isEmpty()
                    || dllRaw.getParamSeq() == null || dllRaw.getParamSeq().isEmpty()) {
                throw new HandledServiceException(411, "요청 내용이 DLL 데이터 형식에 맞지 않습니다.");
            }

            int maxRowNo = getDLLDataMaxRow(dllRaw.getDllSeq(), dllRaw.getParamSeq());
            dllRaw.setRowNo(maxRowNo + 1);
            dllMapper.insertDLLData(dllRaw);

            List<DLLVO.Raw> dllRaws = getDLLData(dllRaw.getDllSeq(), dllRaw.getParamSeq());
            if (dllRaws == null) dllRaws = new ArrayList<>();

            int rowNo = 1;
            for (DLLVO.Raw dr : dllRaws) {
                dr.setRowNo(rowNo++);
                dllMapper.updateDLLDataBySeq(dr);
                if (dr.getSeq().equals(dllRaw.getSeq()))
                    dllRaw.setRowNo(dr.getRowNo());
            }

            // check another params
            List<DLLVO.Param> dllParams = getDLLParamList(dllRaw.getDllSeq());
            if (dllParams == null) dllParams = new ArrayList<>();
            for (DLLVO.Param dp : dllParams) {
                if (dp.getSeq().equals(dllRaw.getParamSeq())) continue;

                dllRaws = getDLLData(dllRaw.getDllSeq(), dp.getSeq());
                if (dllRaws == null) dllRaws = new ArrayList<>();
                if (dllRaws.size() != (rowNo - 1)) {
                    int paramRowNo = 1;
                    for (DLLVO.Raw dr : dllRaws) {
                        dr.setRowNo(paramRowNo++);
                        dllMapper.updateDLLDataBySeq(dr);
                    }
                    for (; paramRowNo < rowNo;) {
                        DLLVO.Raw ndr = new DLLVO.Raw();
                        ndr.setDllSeq(dllRaw.getDllSeq());
                        ndr.setRowNo(paramRowNo++);
                        ndr.setParamSeq(dp.getSeq());
                        if (dp.getParamType().equalsIgnoreCase("string"))
                            ndr.setParamValStr("");
                        else
                            ndr.setParamVal(0.0);
                        dllMapper.insertDLLData(ndr);
                    }
                }
            }

            return dllRaw;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void updateDLLData(DLLVO.Raw dllRaw) throws HandledServiceException {
        try {
            dllMapper.updateDLLDataBySeq(dllRaw);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public DLLVO.Raw updateDLLData(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            DLLVO.Raw dllRaw = ServerConstants.GSON.fromJson(payload, DLLVO.Raw.class);
            if (dllRaw == null || dllRaw.getParamSeq() == null || dllRaw.getParamSeq().isEmpty())
                throw new HandledServiceException(411, "요청 내용이 DLL 데이터 형식에 맞지 않습니다.");

            dllMapper.updateDLLData(dllRaw);

            return dllRaw;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteDLLData(CryptoField dllSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("dllSeq", dllSeq);
            dllMapper.deleteDLLData(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteDLLDataByRow(JsonObject payload) throws HandledServiceException {
        try {
            CryptoField dllSeq = CryptoField.LZERO;
            if (!ApiController.checkJsonEmpty(payload, "dllSeq"))
                dllSeq = CryptoField.decode(payload.get("dllSeq").getAsString(), 0L);

            if (dllSeq == null || dllSeq.isEmpty())
                throw new HandledServiceException(411, "요청 파라미터 오류입니다. [필수 파라미터 누락]");

            Integer rowNo = 0;
            if (!ApiController.checkJsonEmpty(payload, "rowNo"))
                rowNo = payload.get("rowNo").getAsInt();

            Map<String, Object> params = new HashMap<>();
            params.put("dllSeq", dllSeq);
            params.put("rowNo", rowNo);
            dllMapper.deleteDLLDataByRow(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteDLLDataByRow(CryptoField dllSeq, int fromRowNo, int toRowNo) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("dllSeq", dllSeq);
            params.put("fromRowNo", fromRowNo);
            params.put("toRowNo", toRowNo);
            dllMapper.deleteDLLDataByRowNo(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteDLLDataByParam(JsonObject payload) throws HandledServiceException {
        try {
            CryptoField dllSeq = CryptoField.LZERO;
            if (!ApiController.checkJsonEmpty(payload, "dllSeq"))
                dllSeq = CryptoField.decode(payload.get("dllSeq").getAsString(), 0L);

            if (dllSeq == null || dllSeq.isEmpty())
                throw new HandledServiceException(411, "요청 파라미터 오류입니다. [필수 파라미터 누락]");

            CryptoField dllParamSeq = CryptoField.LZERO;
            if (!ApiController.checkJsonEmpty(payload, "paramSeq"))
                dllParamSeq = CryptoField.decode(payload.get("paramSeq").getAsString(), 0L);

            Map<String, Object> params = new HashMap<>();
            params.put("dllSeq", dllSeq);
            params.put("paramSeq", dllParamSeq.isEmpty() ? null : dllParamSeq);
            dllMapper.deleteDLLDataByParam(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

}
