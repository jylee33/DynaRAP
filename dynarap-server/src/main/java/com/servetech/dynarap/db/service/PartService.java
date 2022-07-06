package com.servetech.dynarap.db.service;

import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.servetech.dynarap.config.ServerConstants;
import com.servetech.dynarap.db.mapper.PartMapper;
import com.servetech.dynarap.db.service.task.ShortBlockCreateTask;
import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.db.type.String64;
import com.servetech.dynarap.ext.HandledServiceException;
import com.servetech.dynarap.vo.*;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.redis.core.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import javax.annotation.Resource;
import javax.sql.DataSource;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.CompletableFuture;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.Executor;

import static com.servetech.dynarap.controller.ApiController.checkJsonEmpty;

@Service("partService")
public class PartService {
    private static final Logger logger = LoggerFactory.getLogger(PartService.class);

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
    private PartMapper partMapper;

    @Autowired
    private RawService rawService;

    @Autowired
    private ParamService paramService;

    @Autowired
    private Executor texecutor;

    private volatile Map<String, ShortBlockVO.Meta> createStat = new ConcurrentHashMap<>();

    public List<PartVO> getPartList(CryptoField.NAuth registerUid, CryptoField uploadSeq, int pageNo, int pageSize) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("registerUid", registerUid == null || registerUid.isEmpty() ? null : registerUid);
            params.put("uploadSeq", uploadSeq == null || uploadSeq.isEmpty() ? null : uploadSeq);
            params.put("startIndex", (pageNo - 1) * pageSize);
            params.put("pageSize", pageSize);
            return partMapper.selectPartList(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public int getPartCount(CryptoField.NAuth registerUid, CryptoField uploadSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("registerUid", registerUid == null || registerUid.isEmpty() ? null : registerUid);
            params.put("uploadSeq", uploadSeq == null || uploadSeq.isEmpty() ? null : uploadSeq);
            return partMapper.selectPartCount(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public PartVO getPartBySeq(CryptoField partSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", partSeq);
            return partMapper.selectPartBySeq(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public PartVO insertPart(Connection conn, CryptoField.NAuth uid, RawVO.Upload rawUpload, String64 partName, String julianStartAt, String julianEndAt) throws HandledServiceException {
        try {
            String tempTableName = rawUpload.getUploadId().substring(0, 8) + rawUpload.getUploadId().substring(
                    rawUpload.getUploadId().length() - 8);

            PresetVO preset = null;
            if (rawUpload.getPresetSeq() == null || rawUpload.getPresetSeq().isEmpty())
                preset = paramService.getActivePreset(rawUpload.getPresetPack());
            else
                preset = paramService.getPresetBySeq(rawUpload.getPresetSeq());

            List<ParamVO> presetParams = paramService.getPresetParamList(
                    preset.getPresetPack(), preset.getSeq(), CryptoField.LZERO, CryptoField.LZERO,
                    1, 99999);

            Statement stmt = conn.createStatement();
            ResultSet rs = stmt.executeQuery("select min(julianTimeAt) from dynarap_" + tempTableName + " limit 0, 1");
            String julianStartFrom = "";
            if (rs.next())
                julianStartFrom = rs.getString(1);
            rs.close();

            PartVO part = new PartVO();
            part.setRegisterUid(uid);
            part.setUploadSeq(rawUpload.getSeq());
            part.setPresetPack(rawUpload.getPresetPack());
            part.setPresetSeq(rawUpload.getPresetSeq());
            part.setPartName(partName);
            part.setJulianStartAt(julianStartAt);
            part.setJulianEndAt(julianEndAt);
            part.setOffsetStartAt(getJulianTimeOffset(julianStartFrom, part.getJulianStartAt()));
            part.setOffsetEndAt(getJulianTimeOffset(julianStartFrom, part.getJulianEndAt()));

            PreparedStatement pstmt = conn.prepareStatement("insert into dynarap_part (" +
                    "uploadSeq,partName,presetPack,presetSeq,julianStartAt,julianEndAt," +
                    "offsetStartAt,offsetEndAt,registerUid" +
                    ") values (" +
                    "?,?,?,?,?,?,?,?,?)");
            pstmt.setLong(1, part.getUploadSeq().originOf());
            pstmt.setString(2, part.getPartName().originOf());
            pstmt.setLong(3, part.getPresetPack().originOf());
            pstmt.setLong(4, part.getPresetSeq().originOf());
            pstmt.setString(5, part.getJulianStartAt());
            pstmt.setString(6, part.getJulianEndAt());
            pstmt.setDouble(7, part.getOffsetStartAt());
            pstmt.setDouble(8, part.getOffsetEndAt());
            pstmt.setLong(9, part.getRegisterUid().originOf());
            pstmt.executeUpdate();
            pstmt.close();

            rs = stmt.executeQuery("select last_insert_id() from dynarap_part limit 0, 1");
            if (rs.next())
                part.setSeq(new CryptoField(rs.getLong(1)));
            rs.close();

            // dump part raw from raw_temp table
            int minRowNo = -1;
            int maxRowNo = -1;
            rs = stmt.executeQuery("select " +
                    "(select rowNo from dynarap_" + tempTableName + " where julianTimeAt = '" + part.getJulianStartAt() + "' limit 0, 1) as minRowNo," +
                    "(select rowNo from dynarap_" + tempTableName + " where julianTimeAt = '" + part.getJulianEndAt() + "' limit 0, 1) as maxRowNo limit 0, 1");
            if (rs.next()) {
                minRowNo = rs.getInt("minRowNo");
                maxRowNo = rs.getInt("maxRowNo");
            }
            rs.close();

            if (minRowNo == -1 || maxRowNo == -1)
                throw new Exception("기준 데이터에서 ROW를 찾을 수 없습니다.");

            long jobStartAt = System.currentTimeMillis();

            String paramQuery = "select seq, presetParamSeq, rowNo, julianTimeAt, paramVal, paramValStr " +
                    "from dynarap_" + tempTableName + " " +
                    "where rowNo between " + minRowNo + " and " + maxRowNo + " " +
                    "and presetPack = ? " +
                    "and presetSeq = ? " +
                    "and presetParamSeq = ? " +
                    "order by rowNo asc";
            logger.info("[[[[[ " + paramQuery);

            pstmt = conn.prepareStatement(paramQuery);

            PreparedStatement pstmt_insert = conn.prepareStatement("insert into dynarap_part_raw (" +
                    "partSeq,presetParamSeq,rowNo,julianTimeAt,offsetTimeAt,paramVal,paramValStr,lpf,hpf" +
                    ") values (?,?,?,?,?,?,?,?,?)");

            for (ParamVO param : presetParams) {
                // raw 데이터에서 param에 해당 하는 내용을 가져와서 part에 넣어주기.
                pstmt.setLong(1, param.getPresetPack().originOf());
                pstmt.setLong(2, param.getPresetSeq().originOf());
                pstmt.setLong(3, param.getReferenceSeq());
                rs = pstmt.executeQuery();

                int batchCount = 1;
                while (rs.next()) {
                    pstmt_insert.setLong(1, part.getSeq().originOf());
                    pstmt_insert.setLong(2, param.getReferenceSeq());
                    pstmt_insert.setInt(3, rs.getInt("rowNo"));
                    pstmt_insert.setString(4, rs.getString("julianTimeAt"));
                    pstmt_insert.setDouble(5, getJulianTimeOffset(julianStartFrom, rs.getString("julianTimeAt")));
                    pstmt_insert.setDouble(6, rs.getDouble("paramVal"));
                    pstmt_insert.setString(7, rs.getString("paramValStr"));
                    pstmt_insert.setDouble(8, rs.getDouble("paramVal")); // temp lpf
                    pstmt_insert.setDouble(9, rs.getDouble("paramVal")); // temp hpf
                    pstmt_insert.addBatch();
                    pstmt_insert.clearParameters();

                    if ((batchCount % 1000) == 0) {
                        pstmt_insert.executeBatch();
                        pstmt_insert.clearBatch();
                    }
                    batchCount++;
                }
                if ((batchCount % 1000) > 0) {
                    pstmt_insert.executeBatch();
                    pstmt_insert.clearBatch();
                }
                rs.close();

                pstmt.clearParameters();
            }

            pstmt_insert.close();

            // TODO : creation cache data

            stmt.close();
            pstmt.close();

            logger.info("[[[[[ part data dump completed (" + (System.currentTimeMillis() - jobStartAt) + " msec)" );

            return part;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deletePartBySeq(JsonObject payload) throws HandledServiceException {
        try {
            CryptoField partSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "partSeq"))
                partSeq = CryptoField.decode(payload.get("partSeq").getAsString(), 0L);

            if (partSeq == null || partSeq.isEmpty())
                throw new HandledServiceException(411, "요청 파라미터 오류입니다. [필수 파라미터 누락]");

            Map<String, Object> params = new HashMap<>();
            params.put("partSeq", partSeq);
            partMapper.deletePartData(params);
            partMapper.deletePartBySeq(params);

        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deletePartByUploadSeq(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            CryptoField uploadSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "uploadSeq"))
                uploadSeq = CryptoField.decode(payload.get("uploadSeq").getAsString(), 0L);

            if (uploadSeq == null || uploadSeq.isEmpty())
                throw new HandledServiceException(411, "요청 파라미터 오류입니다. [필수 파라미터 누락]");

            Map<String, Object> params = new HashMap<>();
            params.put("uploadSeq", uploadSeq);

            List<PartVO> partList = getPartList(uid, uploadSeq, 1, 99999);
            if (partList == null) partList = new ArrayList<>();
            for (PartVO p : partList) {
                params.put("partSeq", p.getSeq());
                partMapper.deletePartData(params);
            }
            partMapper.deletePartByUploadSeq(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public List<PartVO> createPartList(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        Connection conn = null;

        try {
            CryptoField uploadSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "uploadSeq"))
                uploadSeq = CryptoField.decode(payload.get("uploadSeq").getAsString(), 0L);

            if (uploadSeq == null || uploadSeq.isEmpty())
                throw new HandledServiceException(411, "요청 파라미터 오류입니다. [필수 파라미터 누락]");

            DataSource ds = jdbcTemplate.getDataSource();
            conn = ds.getConnection();
            conn.setAutoCommit(false);

            // 기존 데이터 삭제.
            Statement stmt = conn.createStatement();
            stmt.executeUpdate("delete from dynarap_part_raw where partSeq in " +
                    "(select seq from dynarap_part where uploadSeq = " + uploadSeq.originOf() + ")");
            stmt.executeUpdate("delete from dynarap_part where uploadSeq = " + uploadSeq.originOf());
            stmt.close();

            RawVO.Upload rawUpload = rawService.getUploadBySeq(uploadSeq);

            // 업로드 정보 토대로 신규 데이터 생성.
            // payload -> [] julianStartAt, julianEndAt
            List<PartVO> partList = new ArrayList<>();
            JsonArray jarrParts = payload.get("parts").getAsJsonArray();
            if (jarrParts != null && jarrParts.size() > 0) {
                for (int i = 0; i < jarrParts.size(); i++) {
                    JsonObject partInfo = jarrParts.get(i).getAsJsonObject();

                    // partName, julianStartAt, julianEndAt
                    String64 partName = null;
                    if (!checkJsonEmpty(partInfo, "partName"))
                        partName = String64.decode(partInfo.get("partName").getAsString());
                    if (partName == null || partName.isEmpty())
                        partName = new String64(rawUpload.getUploadName().originOf() + "_분할_" + String.format("%04d", (i + 1)));

                    String julianStartAt = "";
                    if (!checkJsonEmpty(partInfo, "julianStartAt"))
                        julianStartAt = partInfo.get("julianStartAt").getAsString();

                    String julianEndAt = "";
                    if (!checkJsonEmpty(partInfo, "julianEndAt"))
                        julianEndAt = partInfo.get("julianEndAt").getAsString();

                    if (julianStartAt == null || julianStartAt.isEmpty() || julianEndAt == null || julianEndAt.isEmpty())
                        continue;

                    PartVO part = insertPart(conn, uid, rawUpload, partName, julianStartAt, julianEndAt);
                    partList.add(part);
                }
            }

            conn.commit();
            conn.setAutoCommit(true);

            return partList;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }



    @Transactional
    public ShortBlockVO.Meta doCreateShortBlock(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            ShortBlockVO.CreateRequest createReq = ServerConstants.GSON.fromJson(payload, ShortBlockVO.CreateRequest.class);
            if (createReq == null)
                throw new Exception("요청 형식이 올바르지 않습니다. 파라미터를 확인하세요.");

            ShortBlockVO.Meta shortBlockMeta = null;
            if (createReq.getBlockMetaSeq() == null || createReq.getBlockMetaSeq().isEmpty()) {
                shortBlockMeta = new ShortBlockVO.Meta();
                shortBlockMeta.setSeq(CryptoField.LZERO);
                shortBlockMeta.setPartSeq(createReq.getPartSeq());
                shortBlockMeta.setRegisterUid(uid);
                shortBlockMeta.setCreateDone(false);
                shortBlockMeta.setCreatedAt(LongDate.now());
                shortBlockMeta.setCreateRequest(createReq);
                shortBlockMeta.setOverlap(createReq.getOverlap());
                shortBlockMeta.setSliceTime(createReq.getSliceTime());
                insertShortBlockMeta(shortBlockMeta);
                createStat.put(shortBlockMeta.getSeq().valueOf(), shortBlockMeta);

                shortBlockMeta.setStatus("prepare");
                shortBlockMeta.setStatusMessage("생성 기준 데이터를 준비하고 있습니다.");
                shortBlockMeta.setTotalFetchCount(0);
                shortBlockMeta.setFetchCount(0);
            }
            else {
                shortBlockMeta = getShortBlockMetaBySeq(createReq.getBlockMetaSeq());
                if (!createStat.containsKey(shortBlockMeta.getSeq().valueOf())
                        || createReq.isForcedCreate()) {
                    shortBlockMeta.setCreateDone(false);
                    createStat.put(shortBlockMeta.getSeq().valueOf(), shortBlockMeta);

                    shortBlockMeta.setStatus("prepare");
                    shortBlockMeta.setStatusMessage("생성 기준 데이터를 준비하고 있습니다.");
                    shortBlockMeta.setTotalFetchCount(0);
                    shortBlockMeta.setFetchCount(0);
                }
                else
                    shortBlockMeta = createStat.get(shortBlockMeta.getSeq().valueOf());
                shortBlockMeta.setCreateRequest(createReq);
            }

            // create thread worker start
            if (shortBlockMeta.getStatus().equals("prepare")) {
                ShortBlockCreateTask createTask = new ShortBlockCreateTask.Builder().setListOps(listOps).setZsetOps(zsetOps).createShortBlockCreateTask();
                CompletableFuture.runAsync(createTask.asyncRunCreate(jdbcTemplate, paramService, PartService.this, shortBlockMeta), texecutor);
            }

            return shortBlockMeta;

        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public ShortBlockVO.Meta getProgress(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            CryptoField blockMetaSeq = CryptoField.LZERO;
            if (!checkJsonEmpty(payload, "blockMetaSeq"))
                blockMetaSeq = CryptoField.decode(payload.get("blockMetaSeq").getAsString(), 0L);

            if (blockMetaSeq == null || blockMetaSeq.isEmpty())
                throw new HandledServiceException(411, "요청 파라미터 오류입니다. [필수 파라미터 누락]");

            ShortBlockVO.Meta shortBlockMeta = createStat.get(blockMetaSeq.valueOf());
            if (shortBlockMeta == null)
                throw new HandledServiceException(411, "진행중인 생성 요청이 없습니다.");

            return shortBlockMeta;
        } catch(Exception e) {
            throw new HandledServiceException(411, e.getMessage());
        }
    }

    public List<ShortBlockVO> getShortBlockList(CryptoField.NAuth registerUid, CryptoField partSeq, int pageNo, int pageSize) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("registerUid", registerUid == null || registerUid.isEmpty() ? null : registerUid);
            params.put("partSeq", partSeq == null || partSeq.isEmpty() ? null : partSeq);
            params.put("startIndex", (pageNo - 1) * pageSize);
            params.put("pageSize", pageSize);
            return partMapper.selectShortBlockList(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public int getShortBlockCount(CryptoField.NAuth registerUid, CryptoField partSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("registerUid", registerUid == null || registerUid.isEmpty() ? null : registerUid);
            params.put("uploadSeq", partSeq == null || partSeq.isEmpty() ? null : partSeq);
            return partMapper.selectShortBlockCount(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public ShortBlockVO getShortBlockBySeq(CryptoField blockSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", blockSeq);
            return partMapper.selectShortBlockBySeq(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    // insert, update, delete shortblock ...

    public List<ShortBlockVO.Meta> getShortBlockMetaList(CryptoField partSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("partSeq", partSeq == null || partSeq.isEmpty() ? null : partSeq);
            return partMapper.selectShortBlockMetaList(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public ShortBlockVO.Meta getShortBlockMetaBySeq(CryptoField blockSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", blockSeq);
            return partMapper.selectShortBlockMetaBySeq(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    // insert, update, delete shortblock meta

    public List<ShortBlockVO.Param> getShortBlockParamList(CryptoField blockMetaSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("blockMetaSeq", blockMetaSeq);
            return partMapper.selectShortBlockParamList(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void insertShortBlockMeta(ShortBlockVO.Meta shortBlockMeta) throws HandledServiceException {
        try {
            partMapper.insertShortBlockMeta(shortBlockMeta);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void updateShortBlockMeta(ShortBlockVO.Meta shortBlockMeta) throws HandledServiceException {
        try {
            partMapper.updateShortBlockMeta(shortBlockMeta);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public PartVO.Raw updatePartValueByParams(Map<String, Object> params) throws  HandledServiceException {
        try {
            partMapper.updatePartRaw(params);
            PartVO.Raw rawData = partMapper.selectPartRaw(params);
            return rawData;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void updatePart(PartVO part) throws HandledServiceException {
        try {
            partMapper.updatePart(part);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    // insert, update, delete shortblock param

    public static double getJulianTimeOffset(String julianFrom, String julianCurrent) {
        // 년도 값을 뺀 소수 부분을 포메팅 함.
        // 년도 값을 포함한 msec로 변환후 차이 값을 반환함.
        double jd = 31557600000L;
        double jdms = 3.1688087814029E-8 / 1000;
        String jdStr = Double.toString(System.currentTimeMillis() * jdms);
        int jdPrefix = Integer.parseInt(jdStr.substring(0, jdStr.lastIndexOf(".")));

        String jdFrom = jdPrefix + "." + julianFrom.replaceAll("[^0-9]", "");
        double jdFromMillis = Double.parseDouble(jdFrom) / jdms;

        String jdCurrent = jdPrefix + "." + julianCurrent.replaceAll("[^0-9]", "");
        double jdCurrentMillis = Double.parseDouble(jdCurrent) / jdms;

        return jdCurrentMillis - jdFromMillis;
    }
}
