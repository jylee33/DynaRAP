package com.servetech.dynarap.db.service.task;

import com.servetech.dynarap.db.service.ParamService;
import com.servetech.dynarap.db.service.PartService;
import com.servetech.dynarap.db.service.RawService;
import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.String64;
import com.servetech.dynarap.vo.ParamVO;
import com.servetech.dynarap.vo.PartVO;
import com.servetech.dynarap.vo.PresetVO;
import com.servetech.dynarap.vo.RawVO;
import lombok.RequiredArgsConstructor;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.data.redis.core.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.scheduling.annotation.Async;
import org.springframework.stereotype.Component;

import javax.annotation.Resource;
import javax.sql.DataSource;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileInputStream;
import java.io.InputStreamReader;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

@Component
public class PartImportTask {
    private static final Logger logger = LoggerFactory.getLogger(PartImportTask.class);

    private ListOperations<String, String> listOps;
    private ZSetOperations<String, String> zsetOps;

    public void setListOps(ListOperations<String, String> listOps) {
        this.listOps = listOps;
    }

    public void setZsetOps(ZSetOperations<String, String> zsetOps) {
        this.zsetOps = zsetOps;
    }

    @Async("texecutor")
    public Runnable asyncRunImport(final JdbcTemplate jdbcTemplate, final ParamService paramService, final RawService rawService, final RawVO.Upload rawUpload) {
        return new Runnable() {

            @Override
            public void run() {
                Connection conn = null;

                try {
                    DataSource ds = jdbcTemplate.getDataSource();
                    conn = ds.getConnection();
                    conn.setAutoCommit(false);

                    String tempTableName = rawUpload.getUploadId().substring(0, 8)
                            + rawUpload.getUploadId().substring(rawUpload.getUploadId().length() - 8);

                    // 파라미터 맵 구성
                    PresetVO preset = null;
                    if (rawUpload.getPresetSeq() == null || rawUpload.getPresetSeq().isEmpty())
                        preset = paramService.getActivePreset(rawUpload.getPresetPack());
                    else
                        preset = paramService.getPresetBySeq(rawUpload.getPresetSeq());

                    List<ParamVO> presetParams = paramService.getPresetParamList(
                            preset.getPresetPack(), preset.getSeq(), CryptoField.LZERO, CryptoField.LZERO,
                            1, 99999);

                    if (rawUpload.getStatus().equals("import")) {
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

                        rawUpload.setNotMappedParams(notMappedParams);
                        rawUpload.setMappedParams(mappedParams);

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
                                pstmt.setLong(7, rawUpload.getSeq().originOf());
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
                            rawUpload.setFetchCount(rowNo);
                        }

                        br.close();
                        fis.close();

                        if ((stmtCount % 1000) > 0) {
                            pstmt.executeBatch();
                            pstmt.clearBatch();
                        }

                        pstmt.close();

                        rawUpload.setFetchCount(rawUpload.getTotalFetchCount());

                        rawUpload.setStatus("create-part");
                        rawUpload.setStatusMessage("분할 정보를 생성하고 있습니다.");
                        rawUpload.setFetchCount(0);
                        rawUpload.setTotalFetchCount(0);
                    }

                    if (rawUpload.getStatus().startsWith("create-part")) {
                        // 기존 데이터 삭제.
                        Statement stmt = conn.createStatement();
                        stmt.executeUpdate("delete from dynarap_part_raw where partSeq in " +
                                "(select seq from dynarap_part where uploadSeq = " + rawUpload.getSeq().originOf() + ")");
                        stmt.executeUpdate("delete from dynarap_part where uploadSeq = " + rawUpload.getSeq().originOf());
                        stmt.close();

                        // 업로드 정보 토대로 신규 데이터 생성.
                        // payload -> [] julianStartAt, julianEndAt
                        List<PartVO> partList = new ArrayList<>();
                        if (rawUpload.getUploadRequest().getParts() != null && rawUpload.getUploadRequest().getParts().size() > 0) {
                            for (int i = 0; i < rawUpload.getUploadRequest().getParts().size(); i++) {
                                RawVO.UploadRequest.UploadPart partInfo = rawUpload.getUploadRequest().getParts().get(i);

                                // partName, julianStartAt, julianEndAt
                                if (partInfo.getPartName() == null || partInfo.getPartName().isEmpty())
                                    partInfo.setPartName(new String64(rawUpload.getUploadName().originOf() + "_분할_" + String.format("%04d", (i + 1))));

                                if (partInfo.getJulianStartAt() == null || partInfo.getJulianEndAt().isEmpty()
                                        || partInfo.getJulianEndAt() == null || partInfo.getJulianEndAt().isEmpty()) {
                                    continue;
                                }

                                rawUpload.setStatus("create-part-" + i);
                                rawUpload.setStatusMessage((i + 1) + "번째 분할 구간 데이터를 생성하고 있습니다.");

                                stmt = conn.createStatement();
                                ResultSet rs = stmt.executeQuery("select min(julianTimeAt) from dynarap_" + tempTableName + " limit 0, 1");
                                String julianStartFrom = "";
                                if (rs.next())
                                    julianStartFrom = rs.getString(1);
                                rs.close();

                                PartVO part = new PartVO();
                                part.setRegisterUid(rawUpload.getRegisterUid());
                                part.setUploadSeq(rawUpload.getSeq());
                                part.setPresetPack(rawUpload.getPresetPack());
                                part.setPresetSeq(rawUpload.getPresetSeq());
                                part.setPartName(partInfo.getPartName());
                                part.setJulianStartAt(partInfo.getJulianStartAt());
                                part.setJulianEndAt(partInfo.getJulianEndAt());
                                part.setOffsetStartAt(PartService.getJulianTimeOffset(julianStartFrom, part.getJulianStartAt()));
                                part.setOffsetEndAt(PartService.getJulianTimeOffset(julianStartFrom, part.getJulianEndAt()));

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

                                // 기존 오더 삭제
                                listOps.trim("P" + part.getSeq().originOf(), 0, Integer.MAX_VALUE);
                                zsetOps.removeRangeByScore("P" + part.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);

                                for (ParamVO param : presetParams) {
                                    listOps.rightPush("P" + part.getSeq().originOf(), "P" + param.getPresetParamSeq());
                                    zsetOps.removeRangeByScore("P" + part.getSeq().originOf() + ".N" + param.getPresetParamSeq(), 0, Integer.MAX_VALUE);
                                    zsetOps.removeRangeByScore("P" + part.getSeq().originOf() + ".L" + param.getPresetParamSeq(), 0, Integer.MAX_VALUE);
                                    zsetOps.removeRangeByScore("P" + part.getSeq().originOf() + ".H" + param.getPresetParamSeq(), 0, Integer.MAX_VALUE);
                                }

                                // dump part raw from raw_temp table
                                int minRowNo = -1;
                                int maxRowNo = -1;
                                rs = stmt.executeQuery("select " +
                                        "(select rowNo from dynarap_" + tempTableName + " where julianTimeAt = '" + part.getJulianStartAt() + "' limit 0, 1) as minRowNo," +
                                        "(select rowNo from dynarap_" + tempTableName + " where julianTimeAt = '" + part.getJulianEndAt() + "' limit 0, 1) as maxRowNo limit 0, 1");
                                if (rs.next()) {
                                    minRowNo = rs.getInt("minRowNo");
                                    maxRowNo = rs.getInt("maxRowNo");
                                    rawUpload.setTotalFetchCount(maxRowNo - minRowNo + 1);
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
                                    rawUpload.setStatus("create-part-" + i + "-" + param.getPresetParamSeq());
                                    rawUpload.setStatusMessage((i + 1) + "번째 분할 구간 데이터를 생성하고 있습니다. [" + param.getParamName().originOf() + "]");

                                    // raw 데이터에서 param에 해당 하는 내용을 가져와서 part에 넣어주기.
                                    pstmt.setLong(1, param.getPresetPack().originOf());
                                    pstmt.setLong(2, param.getPresetSeq().originOf());
                                    pstmt.setLong(3, param.getPresetParamSeq());
                                    rs = pstmt.executeQuery();

                                    String rowKey = "P" + part.getSeq().originOf() + ".N" + param.getPresetParamSeq();
                                    String lpfKey = "P" + part.getSeq().originOf() + ".L" + param.getPresetParamSeq();
                                    String hpfKey = "P" + part.getSeq().originOf() + ".H" + param.getPresetParamSeq();

                                    int batchCount = 1;
                                    while (rs.next()) {
                                        int rowNo = rs.getInt("rowNo");
                                        String julianTimeAt = rs.getString("julianTimeAt");
                                        Double dblVal = rs.getDouble("paramVal");

                                        zsetOps.addIfAbsent("P" + part.getSeq().originOf() + ".R", rs.getString("julianTimeAt"), rowNo);

                                        pstmt_insert.setLong(1, part.getSeq().originOf());
                                        pstmt_insert.setLong(2, param.getPresetParamSeq());
                                        pstmt_insert.setInt(3, rowNo);
                                        pstmt_insert.setString(4, julianTimeAt);
                                        pstmt_insert.setDouble(5, PartService.getJulianTimeOffset(julianStartFrom, julianTimeAt));
                                        pstmt_insert.setDouble(6, dblVal);
                                        pstmt_insert.setString(7, rs.getString("paramValStr"));

                                        // FIXME : lpf, hpf
                                        Double lpfVal = getLpfVal(dblVal);
                                        Double hpfVal = getHpfVal(dblVal);

                                        pstmt_insert.setDouble(8, lpfVal); // temp lpf
                                        pstmt_insert.setDouble(9, hpfVal); // temp hpf
                                        pstmt_insert.addBatch();
                                        pstmt_insert.clearParameters();

                                        if ((batchCount % 1000) == 0) {
                                            pstmt_insert.executeBatch();
                                            pstmt_insert.clearBatch();
                                        }
                                        batchCount++;
                                        rawUpload.setFetchCount(batchCount);

                                        if (dblVal != null) {
                                            zsetOps.add(rowKey, julianTimeAt + ":" + dblVal, rowNo);
                                            zsetOps.add(lpfKey, julianTimeAt + ":" + lpfVal, rowNo);
                                            zsetOps.add(hpfKey, julianTimeAt + ":" + hpfVal, rowNo);
                                        }
                                        else
                                            zsetOps.add(rowKey, julianTimeAt + ":" + rs.getString("paramValStr"), rowNo);
                                    }
                                    if ((batchCount % 1000) > 0) {
                                        pstmt_insert.executeBatch();
                                        pstmt_insert.clearBatch();
                                        conn.commit();
                                    }
                                    rs.close();

                                    pstmt.clearParameters();
                                }

                                pstmt_insert.close();
                                conn.commit();
                                partList.add(part);

                                stmt.close();
                                pstmt.close();

                                logger.info("[[[[[ part data dump completed (" + (System.currentTimeMillis() - jobStartAt) + " msec)" );
                            }
                        }

                        rawUpload.setPartList(partList);

                        rawUpload.setStatus("import-done");
                        rawUpload.setStatusMessage("이미 완료된 요청입니다.");
                    }

                    conn.commit();
                    conn.setAutoCommit(true);

                    rawUpload.setImportDone(true);
                    rawService.updateRawUpload(rawUpload);

                } catch(Exception ex) {
                    ex.printStackTrace();

                    try {
                        rawUpload.setImportDone(false);
                        rawService.updateRawUpload(rawUpload);
                    } catch(Exception subex) {
                        subex.printStackTrace();
                    }
                }
            }
        };
    }

    private Double getLpfVal(Double dblVal) {
        if (dblVal == null) return null;
        Double lpfVal = dblVal;
        return lpfVal;
    }

    private Double getHpfVal(Double dblVal) {
        if (dblVal == null) return null;
        Double hpfVal = dblVal;
        return hpfVal;
    }

    public static class Builder {
        private ListOperations<String, String> listOps;
        private ZSetOperations<String, String> zsetOps;

        public Builder setListOps(ListOperations<String, String> listOps) {
            this.listOps = listOps;
            return this;
        }

        public Builder setZsetOps(ZSetOperations<String, String> zsetOps) {
            this.zsetOps = zsetOps;
            return this;
        }

        public PartImportTask createPartImportTask() {
            PartImportTask task = new PartImportTask();
            task.setListOps(this.listOps);
            task.setZsetOps(this.zsetOps);
            return task;
        }
    }
}
