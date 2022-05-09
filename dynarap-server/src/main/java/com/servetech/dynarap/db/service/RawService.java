package com.servetech.dynarap.db.service;

import com.google.gson.JsonObject;
import com.servetech.dynarap.config.ServerConstants;
import com.servetech.dynarap.controller.ApiController;
import com.servetech.dynarap.db.mapper.RawMapper;
import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.ext.HandledServiceException;
import com.servetech.dynarap.vo.ParamVO;
import com.servetech.dynarap.vo.PresetVO;
import com.servetech.dynarap.vo.RawVO;
import lombok.RequiredArgsConstructor;
import org.apache.ibatis.session.ExecutorType;
import org.apache.ibatis.session.SqlSession;
import org.apache.ibatis.session.SqlSessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.redis.core.*;
import org.springframework.scheduling.annotation.EnableScheduling;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import javax.annotation.Resource;
import java.io.*;
import java.util.*;

@Service("rawService")
@EnableScheduling
@RequiredArgsConstructor
public class RawService {
    @Resource(name = "redisTemplate")
    private ValueOperations<String, String> valueOps;

    @Resource(name = "redisTemplate")
    private ListOperations<String, String> listOps;

    @Resource(name = "redisTemplate")
    private HashOperations<String, String, String> hashOps;

    @Resource(name = "redisTemplate")
    private SetOperations<String, String> setOps;

    @Resource(name = "redisTemplate")
    private ZSetOperations<String, String> zsetOps;

    @Autowired
    private RawMapper rawMapper;

    @Autowired
    private ParamService paramService;

    private final SqlSessionFactory sqlSessionFactory;

    public List<RawVO> getRawData(CryptoField uploadSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("uploadSeq", uploadSeq);
            return rawMapper.selectRawData(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public RawVO insertRawData(JsonObject payload) throws HandledServiceException {
        try {
            RawVO raw = ServerConstants.GSON.fromJson(payload, RawVO.class);
            if (raw == null)
                throw new HandledServiceException(411, "요청 내용이 데이터 형식에 맞지 않습니다.");

            rawMapper.insertRawData(raw);
            return raw;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public RawVO uploadRawData(JsonObject payload) throws HandledServiceException {
        try {
            RawVO raw = ServerConstants.GSON.fromJson(payload, RawVO.class);
            if (raw == null)
                throw new HandledServiceException(411, "요청 내용이 데이터 형식에 맞지 않습니다.");

            rawMapper.updateRawData(raw);
            return raw;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteRawDataByRow(JsonObject payload) throws HandledServiceException {
        try {
            CryptoField uploadSeq = CryptoField.LZERO;
            if (!ApiController.checkJsonEmpty(payload, "uploadSeq"))
                uploadSeq = CryptoField.decode(payload.get("uploadSeq").getAsString(), 0L);

            if (uploadSeq == null || uploadSeq.isEmpty())
                throw new HandledServiceException(411, "요청 파라미터 오류입니다. [필수 파라미터 누락]");

            Integer rowNo = 0;
            if (!ApiController.checkJsonEmpty(payload, "rowNo"))
                rowNo = payload.get("rowNo").getAsInt();

            Map<String, Object> params = new HashMap<>();
            params.put("uploadSeq", uploadSeq);
            params.put("rowNo", rowNo);

            rawMapper.deleteRawDataByRow(params);

        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteRawDataByParam(JsonObject payload) throws HandledServiceException {
        try {
            CryptoField uploadSeq = CryptoField.LZERO;
            if (!ApiController.checkJsonEmpty(payload, "uploadSeq"))
                uploadSeq = CryptoField.decode(payload.get("uploadSeq").getAsString(), 0L);

            if (uploadSeq == null || uploadSeq.isEmpty())
                throw new HandledServiceException(411, "요청 파라미터 오류입니다. [필수 파라미터 누락]");

            CryptoField presetParamSeq = CryptoField.LZERO;
            if (!ApiController.checkJsonEmpty(payload, "presetParamSeq"))
                presetParamSeq = CryptoField.decode(payload.get("presetParamSeq").getAsString(), 0L);

            Map<String, Object> params = new HashMap<>();
            params.put("uploadSeq", uploadSeq);
            params.put("presetParamSeq", presetParamSeq == null || presetParamSeq.isEmpty() ? null : presetParamSeq);

            rawMapper.deleteRawDataByParam(params);

        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public List<RawVO.Upload> getUploadList() throws HandledServiceException {
        try {
            return rawMapper.selectRawUploadList();
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public RawVO.Upload getUploadBySeq(CryptoField uploadSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("uploadSeq", uploadSeq);
            return rawMapper.selectRawUploadBySeq(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public RawVO.Upload getUploadById(String uploadId) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("uploadId", uploadId);
            return rawMapper.selectRawUploadById(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public RawVO.Upload getUploadByParams(Map<String, Object> params) throws HandledServiceException {
        try {
            return rawMapper.selectRawUploadByParam(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public RawVO.Upload insertRawUpload(RawVO.Upload rawUpload) throws HandledServiceException {
        try {
            rawMapper.insertRawUpload(rawUpload);
            return rawUpload;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public RawVO.Upload updateRawUpload(RawVO.Upload rawUpload) throws HandledServiceException {
        try {
            rawMapper.updateRawUpload(rawUpload);
            return rawUpload;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteRawUpload(JsonObject payload) throws HandledServiceException {
        try {
            CryptoField uploadSeq = CryptoField.LZERO;
            if (!ApiController.checkJsonEmpty(payload, "seq"))
                uploadSeq = CryptoField.decode(payload.get("seq").getAsString(), 0L);

            if (uploadSeq == null || uploadSeq.isEmpty())
                throw new HandledServiceException(411, "요청 파라미터 오류입니다. [필수 파라미터 누락]");

            Map<String, Object> params = new HashMap<>();
            params.put("uploadSeq", uploadSeq);
            rawMapper.deleteRawDataByParam(params);
            rawMapper.deleteRawUpload(params);

        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public JsonObject createCache(CryptoField.NAuth uid, CryptoField uploadSeq) throws HandledServiceException {
        try {
            JsonObject jobjResult = new JsonObject();

            RawVO.Upload rawUpload = getUploadBySeq(uploadSeq);
            PresetVO preset = paramService.getPresetBySeq(rawUpload.getPresetSeq());
            List<ParamVO> presetParams = paramService.getPresetParamList(
                    preset.getPresetPack(), preset.getSeq(), CryptoField.LZERO, CryptoField.LZERO,
                    1, 99999);

            Map<String, RawVO.CacheColumn> cacheColumnMap = new LinkedHashMap<>();
            for (ParamVO pp : presetParams) {
                RawVO.CacheColumn cacheColumn = new RawVO.CacheColumn();
                cacheColumn.setPresetSeq(pp.getPresetSeq());
                cacheColumn.setParamSeq(pp.getSeq());
                cacheColumn.setTimes(new ArrayList<>());
                cacheColumn.setColumnData(new ArrayList<>());
                cacheColumn.setColumnStrData(new ArrayList<>());
                cacheColumnMap.put(new CryptoField(pp.getPresetParamSeq()).valueOf(), cacheColumn);
            }

            Map<String, Object> params = new HashMap<>();
            params.put("uploadSeq", uploadSeq);
            List<RawVO> rawData = rawMapper.selectRawData(params);

            // gen row , col
            Map<Integer, RawVO.CacheRow> cacheRowMap = new LinkedHashMap<>();
            for (RawVO raw : rawData) {
                RawVO.CacheRow cacheRow = cacheRowMap.get(raw.getRowNo());
                if (cacheRow == null) {
                    cacheRow = new RawVO.CacheRow();
                    cacheRow.setRowNo(raw.getRowNo());
                    cacheRow.setJulianTimeAt(raw.getJulianTimeAt());
                    cacheRow.setRowData(new ArrayList<>());
                    cacheRow.setRowStrData(new ArrayList<>());
                    cacheRowMap.put(raw.getRowNo(), cacheRow);
                }

                RawVO.CacheColumn cacheColumn = cacheColumnMap.get(raw.getPresetParamSeq().valueOf());

                cacheRow.getRowData().add(raw.getParamVal());
                cacheRow.getRowStrData().add(raw.getParamValStr());

                cacheColumn.getColumnData().add(raw.getParamVal());
                cacheColumn.getColumnStrData().add(raw.getParamValStr());
            }

            jobjResult.add("rows", ServerConstants.GSON.toJsonTree(cacheRowMap));
            jobjResult.add("columns", ServerConstants.GSON.toJsonTree(cacheColumnMap));

            return jobjResult;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public JsonObject runImport(CryptoField.NAuth uid, CryptoField uploadSeq,
                          CryptoField presetPack, CryptoField presetSeq,
                          CryptoField flightSeq, String flightAt) throws HandledServiceException {
        try {
            JsonObject jobjResult = new JsonObject();

            RawVO.Upload rawUpload = getUploadBySeq(uploadSeq);
            rawUpload.setImportDone(false);
            rawUpload.setPresetPack(presetPack);
            if (presetSeq == null || presetSeq.isEmpty()) {
                PresetVO preset = paramService.getActivePreset(presetPack);
                rawUpload.setPresetSeq(preset.getSeq());
            }
            else {
                rawUpload.setPresetSeq(presetSeq);
            }
            rawUpload.setFlightSeq(flightSeq == null ? CryptoField.LZERO : flightSeq);
            if (flightAt != null && !flightAt.isEmpty())
                rawUpload.setFlightAt(LongDate.parse(flightAt, "yyyy-MM-dd"));
            updateRawUpload(rawUpload);

            // 기존 raw 테이블 삭제.
            Map<String, Object> params = new HashMap<>();
            params.put("uploadSeq", uploadSeq);
            rawMapper.deleteRawDataByParam(params);

            // 파라미터 맵 구성
            PresetVO preset = null;
            if (presetSeq == null || presetSeq.isEmpty())
                preset = paramService.getActivePreset(presetPack);
            else
                preset = paramService.getPresetBySeq(presetSeq);

            List<ParamVO> presetParams = paramService.getPresetParamList(
                    preset.getPresetPack(), preset.getSeq(), CryptoField.LZERO, CryptoField.LZERO,
                    1, 99999);

            if (presetParams == null) presetParams = new ArrayList<>();
            Map<String, ParamVO> paramMap = new LinkedHashMap<>();
            Map<String, ParamVO> adamsMap = new LinkedHashMap<>();
            Map<String, ParamVO> zaeroMap = new LinkedHashMap<>();
            Map<String, ParamVO> grtMap = new LinkedHashMap<>();
            Map<String, ParamVO> fltpMap = new LinkedHashMap<>();
            Map<String, ParamVO> fltsMap = new LinkedHashMap<>();
            for (ParamVO param : presetParams) {
                paramMap.put(param.getParamKey() + "_" + param.getParamUnit(), param);
                adamsMap.put(param.getAdamsKey() + "_" + param.getParamUnit(), param);
                zaeroMap.put(param.getZaeroKey() + "_" + param.getParamUnit(), param);
                grtMap.put(param.getGrtKey() + "_" + param.getParamUnit(), param);
                fltpMap.put(param.getFltpKey() + "_" + param.getParamUnit(), param);
                fltsMap.put(param.getFltsKey() + "_" + param.getParamUnit(), param);
            }

            // File loading
            File fImport = new File(rawUpload.getStorePath());
            FileInputStream fis = new FileInputStream(fImport);
            BufferedReader br = new BufferedReader(new InputStreamReader(fis));
            String line = null;

            // first line param list
            line = br.readLine();
            String[] splitParam = line.trim().split(",");
            // data 0 is date
            List<String> notMappedParams = new ArrayList<>();

            List<Integer> mappedIndexes = new ArrayList<>();
            List<ParamVO> mappedParams = new ArrayList<>();

            for (int i = 0; i < splitParam.length; i++) {
                String p = splitParam[i];
                if (p.equalsIgnoreCase("date")) continue;

                ParamVO pi = null;
                if (paramMap.containsKey(p)) pi = paramMap.get(p);
                else if (adamsMap.containsKey(p)) pi = adamsMap.get(p);
                else if (zaeroMap.containsKey(p)) pi = zaeroMap.get(p);
                else if (grtMap.containsKey(p)) pi = grtMap.get(p);
                else if (fltpMap.containsKey(p)) pi = fltpMap.get(p);
                else if (fltsMap.containsKey(p)) pi = fltsMap.get(p);

                if (pi == null) {
                    notMappedParams.add(p);
                    continue;
                }

                mappedIndexes.add(i);
                mappedParams.add(pi);
            }

            jobjResult.add("notMappedParams", ServerConstants.GSON.toJsonTree(notMappedParams));
            jobjResult.add("mappedParams", ServerConstants.GSON.toJsonTree(mappedParams));

            // skip next line [blank link]
            br.readLine();

            // real data insert
            int rowNo = 1;
            List<Map<String, Object>> bulkList = new ArrayList<>();

            while ((line = br.readLine()) != null) {
                String[] splitData = line.trim().split(",");

                String julianTimeAt = splitData[0];
                for (int i = 0; i < mappedParams.size(); i++) {
                    ParamVO pi = mappedParams.get(i);
                    int spi = mappedIndexes.get(i);

                    Map<String, Object> param = new HashMap<>();
                    param.put("presetPack", pi.getPresetPack());
                    param.put("presetSeq", pi.getPresetSeq());
                    param.put("presetParamSeq", pi.getPresetParamSeq());
                    param.put("julianTimeAt", julianTimeAt);

                    String paramValStr = splitData[spi];
                    try {
                        param.put("paramVal", Double.parseDouble(paramValStr));
                    } catch(NumberFormatException nfe) {
                        param.put("paramValStr", paramValStr);
                    }

                    param.put("rowNo", rowNo);
                    param.put("uploadSeq", uploadSeq);
                    bulkList.add(param);
                }

                // bulk insert
                if ((rowNo % 1000) == 0) {
                    // execute bulk insert

                    SqlSession sqlSession = sqlSessionFactory.openSession(ExecutorType.BATCH);
                    try {
                        sqlSession.insert("com.servetech.dynarap.db.mapper.RawMapper.insertRawDataBulk", bulkList);
                    } finally {
                        sqlSession.flushStatements();
                        sqlSession.close();
                        sqlSession.clearCache();

                        bulkList.clear();
                    }
                }

                rowNo++;
            }

            if (bulkList.size() > 0) {
                SqlSession sqlSession = sqlSessionFactory.openSession(ExecutorType.BATCH);
                try {
                    sqlSession.insert("com.servetech.dynarap.db.mapper.RawMapper.insertRawDataBulk", bulkList);
                } finally {
                    sqlSession.flushStatements();
                    sqlSession.close();
                    sqlSession.clearCache();
                    bulkList.clear();
                }
            }

            br.close();
            fis.close();

            return jobjResult;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }
}
