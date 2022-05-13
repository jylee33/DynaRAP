package com.servetech.dynarap.db.service;

import com.google.gson.JsonObject;
import com.servetech.dynarap.config.ServerConstants;
import com.servetech.dynarap.controller.ApiController;
import com.servetech.dynarap.db.mapper.RawMapper;
import com.servetech.dynarap.db.service.task.PartImportTask;
import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.db.type.String64;
import com.servetech.dynarap.ext.HandledServiceException;
import com.servetech.dynarap.vo.ParamVO;
import com.servetech.dynarap.vo.PresetVO;
import com.servetech.dynarap.vo.RawVO;
import lombok.RequiredArgsConstructor;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.redis.core.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.scheduling.annotation.EnableScheduling;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import javax.annotation.Resource;
import javax.sql.DataSource;
import java.io.*;
import java.sql.*;
import java.util.*;
import java.util.concurrent.CompletableFuture;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.Executor;

import static com.servetech.dynarap.controller.ApiController.checkJsonEmpty;

@Service("rawService")
@EnableScheduling
@RequiredArgsConstructor
public class RawService {
    private static final Logger logger = LoggerFactory.getLogger(RawService.class);

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
    private JdbcTemplate jdbcTemplate;

    @Autowired
    private RawMapper rawMapper;

    @Autowired
    private ParamService paramService;

    @Autowired
    private Executor texecutor;

    private volatile Map<String, RawVO.Upload> uploadStat = new ConcurrentHashMap<>();

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
    public RawVO.Upload doUpload(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            RawVO.UploadRequest uploadReq = ServerConstants.GSON.fromJson(payload, RawVO.UploadRequest.class);
            if (uploadReq == null)
                throw new Exception("요청 형식이 올바르지 않습니다. 파라미터를 확인하세요.");

            if (uploadReq.getSourcePath().contains("C:\\")) {
                uploadReq.setSourcePath(uploadReq.getSourcePath().replaceAll("\\\\", "/"));
                //uploadReq.setSourcePath(uploadReq.getSourcePath().replaceAll("C:/", "/Users/aloepigeon/"));
                uploadReq.setSourcePath(uploadReq.getSourcePath().replaceAll("C:/", "/home/ubuntu/"));
            }

            File fStatic = new File(uploadReq.getSourcePath());
            if (fStatic == null || !fStatic.exists())
                throw new HandledServiceException(404, "파일을 찾을 수 없습니다. [" + uploadReq.getSourcePath() + "]");

            long lineCount = getLineCount(fStatic.getAbsolutePath());
            String originalFileName = fStatic.getName().replaceAll("_", "");
            String fileExt = originalFileName.substring(originalFileName.lastIndexOf(".") + 1);

            String uploadId = new CryptoField(originalFileName + "_" + fStatic.length()).valueOf();

            RawVO.Upload rawUpload = getUploadById(uploadId);
            if (rawUpload == null) {
                rawUpload = new RawVO.Upload();
                rawUpload.setUploadId(uploadId);
                rawUpload.setUploadName(new String64(originalFileName));
                rawUpload.setFileSize(fStatic.length());
                rawUpload.setRegisterUid(uid);
                rawUpload.setUploadedAt(LongDate.now());
                rawUpload.setStorePath(fStatic.getAbsolutePath());
                rawUpload.setImportDone(false);
                rawUpload.setPresetPack(uploadReq.getPresetPack());
                if (uploadReq.getPresetSeq() == null || uploadReq.getPresetSeq().isEmpty()) {
                    PresetVO preset = paramService.getActivePreset(uploadReq.getPresetPack());
                    rawUpload.setPresetSeq(preset.getSeq());
                }
                else {
                    rawUpload.setPresetSeq(uploadReq.getPresetSeq());
                }
                rawUpload.setFlightSeq(uploadReq.getFlightSeq() == null ? CryptoField.LZERO : uploadReq.getFlightSeq());
                if (uploadReq.getFlightAt() != null && !uploadReq.getFlightAt().isEmpty())
                    rawUpload.setFlightAt(LongDate.parse(uploadReq.getFlightAt(), "yyyy-MM-dd"));
                rawUpload.setDataType(uploadReq.getDataType());
                rawUpload.setUploadRequest(uploadReq);
                insertRawUpload(rawUpload);
                uploadStat.put(rawUpload.getSeq().valueOf(), rawUpload);

                rawUpload.setStatus("import");
                rawUpload.setStatusMessage("요청 파일을 데이터베이스에 저장하고 있습니다.");
                rawUpload.setTotalFetchCount(lineCount);
                rawUpload.setFetchCount(0);
            }
            else {
                if (!uploadStat.containsKey(rawUpload.getSeq().valueOf())
                    || uploadReq.isForcedImport()) {
                    rawUpload.setImportDone(false);
                    uploadStat.put(rawUpload.getSeq().valueOf(), rawUpload);

                    rawUpload.setStatus("import");
                    rawUpload.setStatusMessage("요청 파일을 데이터베이스에 저장하고 있습니다.");
                    rawUpload.setTotalFetchCount(lineCount);
                    rawUpload.setFetchCount(0);
                }
                else
                    rawUpload = uploadStat.get(rawUpload.getSeq().valueOf());
                rawUpload.setUploadRequest(uploadReq);
            }

            // create thread worker start
            if (rawUpload.getStatus().equals("import")) {
                PartImportTask importTask = new PartImportTask();
                CompletableFuture.runAsync(importTask.asyncRunImport(jdbcTemplate, paramService, RawService.this, rawUpload), texecutor);
            }

            return rawUpload;

        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public RawVO.Upload getProgress(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            CryptoField uploadSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "uploadSeq"))
                uploadSeq = CryptoField.decode(payload.get("uploadSeq").getAsString(), 0L);

            if (uploadSeq == null || uploadSeq.isEmpty())
                throw new HandledServiceException(411, "요청 파라미터 오류입니다. [필수 파라미터 누락]");

            RawVO.Upload rawUpload = uploadStat.get(uploadSeq.valueOf());
            if (rawUpload == null)
                throw new HandledServiceException(411, "진행중인 업로드 요청이 없습니다.");

            return rawUpload;
        } catch(Exception e) {
            throw new HandledServiceException(411, e.getMessage());
        }
    }

    @Transactional
    public JsonObject runImport(CryptoField.NAuth uid, CryptoField uploadSeq,
                          CryptoField presetPack, CryptoField presetSeq,
                          CryptoField flightSeq, String flightAt, String dataType) throws HandledServiceException {
        Connection conn = null;

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
            rawUpload.setDataType(dataType);

            DataSource ds = jdbcTemplate.getDataSource();
            conn = ds.getConnection();
            conn.setAutoCommit(false);

            String tempTableName = rawUpload.getUploadId().substring(0, 8)
                    + rawUpload.getUploadId().substring(rawUpload.getUploadId().length() - 8);

            // 기존 raw 테이블 삭제.
            Statement stmt = conn.createStatement();
            stmt.executeUpdate("drop table if exists `dynarap_" + tempTableName + "` cascade");
            stmt.executeUpdate("create table `dynarap_" + tempTableName + "`\n" +
                    "(\n" +
                    "    `seq` bigint auto_increment not null            comment '일련번호',\n" +
                    "    `uploadSeq` bigint default 0                    comment '업로드 일련번호',\n" +
                    "    `presetPack` bigint default 0                   comment '프리셋 관리번호',\n" +
                    "    `presetSeq` bigint default 0                    comment '프리셋 일련번호',\n" +
                    "    `presetParamSeq` bigint default 0               comment '프리셋 구성 파라미터 일련번호',\n" +
                    "    `rowNo` int default 0                           comment '데이터 row',\n" +
                    "    `julianTimeAt` varchar(32)                      comment '절대 시간 값',\n" +
                    "    `paramVal` double default 0.0                   comment '파라미터 값 (숫자)',\n" +
                    "    `paramValStr` varchar(128) default ''           comment '파라미터 값 (문자)',\n" +
                    "    constraint pk_dynarap_raw primary key (`seq`)\n" +
                    ")");
            stmt.executeUpdate("create index `idx_dynarap_" + tempTableName + "_param` on `dynarap_" + tempTableName + "` (`presetPack`, `presetSeq`, `presetParamSeq`)");
            stmt.executeUpdate("create index `idx_dynarap_" + tempTableName + "_row` on `dynarap_" + tempTableName + "` (`rowNo`, `presetPack`, `presetSeq`)");
            stmt.executeUpdate("create index `idx_dynarap_" + tempTableName + "_julian` on `dynarap_" + tempTableName + "` (`julianTimeAt`)");
            stmt.executeUpdate("create index `idx_dynarap_" + tempTableName + "_row2` on `dynarap_" + tempTableName + "` (`rowNo`)");
            conn.commit(); // drop 이후 커밋 처리.

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
            long snapTime = System.currentTimeMillis();

            PreparedStatement pstmt = conn.prepareStatement("insert into dynarap_" + tempTableName + " (" +
                    "presetPack,presetSeq,presetParamSeq,paramVal,paramValStr,rowNo,uploadSeq,julianTimeAt" +
                    ") values (?,?,?,?,?,?,?,?)");
            pstmt.setFetchSize(1000);

            int stmtCount = 1;
            long startTime = System.currentTimeMillis();

            while ((line = br.readLine()) != null) {
                String[] splitData = line.trim().split(",");

                String julianTimeAt = splitData[0];
                for (int i = 0; i < mappedParams.size(); i++) {
                    ParamVO pi = mappedParams.get(i);
                    int spi = mappedIndexes.get(i);

                    pstmt.setLong(1, pi.getPresetPack().originOf());
                    pstmt.setLong(2, pi.getPresetSeq().originOf());
                    pstmt.setLong(3, pi.getPresetParamSeq());

                    String paramValStr = splitData[spi];
                    try {
                        pstmt.setDouble(4, Double.parseDouble(paramValStr));
                        pstmt.setString(5, null);
                    } catch(NumberFormatException nfe) {
                        pstmt.setDouble(4, 0);
                        pstmt.setString(5, paramValStr);
                    }

                    pstmt.setInt(6, rowNo);
                    pstmt.setLong(7, uploadSeq.originOf());
                    pstmt.setString(8, julianTimeAt);
                    pstmt.addBatch();

                    pstmt.clearParameters();

                    if ((stmtCount % 1000) == 0) {
                        pstmt.executeBatch();
                        pstmt.clearBatch();
                    }

                    stmtCount++;
                }

                rowNo++;
            }

            br.close();
            fis.close();

            if ((stmtCount % 1000) > 0) {
                pstmt.executeBatch();
                pstmt.clearBatch();
            }

            pstmt.close();

            rawUpload.setImportDone(true);
            updateRawUpload(rawUpload);

            logger.info("[[[[[ batch completed " + (System.currentTimeMillis() - startTime) + "msec");

            try {
                conn.commit();
                conn.setAutoCommit(true);
                conn.close();
            } catch (Exception ex) {
                logger.debug("Batch Connection Close Error : " + ex.getMessage());
            }

            return jobjResult;
        } catch(Exception e) {
            try {
                if (conn != null && !conn.isClosed()) {
                    conn.rollback();
                    conn.close();
                }
            } catch(Exception ex) {
                // nothing to log
            }

            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public static long getLineCount(String absPath) {
        long lines = 0;
        try (InputStream is = new BufferedInputStream(new FileInputStream(absPath))) {
            byte[] c = new byte[1024];
            int count = 0;
            int readChars = 0;
            boolean endsWithoutNewLine = false;
            while ((readChars = is.read(c)) != -1) {
                for (int i = 0; i < readChars; ++i) {
                    if (c[i] == '\n')
                        ++count;
                }
                endsWithoutNewLine = (c[readChars - 1] != '\n');
            }
            if (endsWithoutNewLine) {
                ++count;
            }
            lines = count;
        } catch (IOException e) {
            e.printStackTrace();
        }
        return lines;
    }
}
