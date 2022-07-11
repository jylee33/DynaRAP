package com.servetech.dynarap.db.service.task;

import com.servetech.dynarap.db.service.ParamService;
import com.servetech.dynarap.db.service.PartService;
import com.servetech.dynarap.db.service.RawService;
import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.String64;
import com.servetech.dynarap.vo.*;
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
import java.util.*;

@Component
public class ShortBlockCreateTask {
    private static final Logger logger = LoggerFactory.getLogger(ShortBlockCreateTask.class);

    private ListOperations<String, String> listOps;
    private ZSetOperations<String, String> zsetOps;

    public void setListOps(ListOperations<String, String> listOps) {
        this.listOps = listOps;
    }

    public void setZsetOps(ZSetOperations<String, String> zsetOps) {
        this.zsetOps = zsetOps;
    }

    @Async("texecutor")
    public Runnable asyncRunCreate(final JdbcTemplate jdbcTemplate,
                                   final ParamService paramService,
                                   final PartService partService,
                                   final ShortBlockVO.Meta shortBlockMeta) {
        return new Runnable() {

            @Override
            public void run() {
                Connection conn = null;

                try {
                    DataSource ds = jdbcTemplate.getDataSource();
                    conn = ds.getConnection();
                    conn.setAutoCommit(false);

                    if (shortBlockMeta.getStatus().equals("prepare")) {
                        // 파트 정보 로딩하기
                        shortBlockMeta.setPartInfo(partService.getPartBySeq(shortBlockMeta.getPartSeq()));

                        // 생성 준비 작업.
                        // blockMeta 정보를 토대로
                        // param 삭제, raw 삭제, block 삭제.
                        Statement stmt = conn.createStatement();
                        stmt.executeUpdate("delete from dynarap_sblock_param where blockMetaSeq = " + shortBlockMeta.getSeq().originOf());
                        stmt.executeUpdate("delete from dynarap_sblock_raw where blockSeq in " +
                                "(select seq from dynarap_sblock where blockMetaSeq = " + shortBlockMeta.getSeq().originOf() + ")");
                        stmt.executeUpdate("delete from dynarap_sblock where blockMetaSeq = " + shortBlockMeta.getSeq().originOf());

                        // param 넣기
                        PreparedStatement pstmt = conn.prepareStatement(
                                "insert into dynarap_sblock_param (" +
                                        "blockMetaSeq,paramNo,paramPack,paramSeq," +
                                        "paramKey,adamsKey,zaeroKey,grtKey," +
                                        "fltpKey,fltsKey,unionParamSeq,propSeq" +
                                        ") values (?,?,?,?," +
                                        "?,?,?,?," +
                                        "?,?,?,?)");
                        int paramNo = 1;
                        for (ShortBlockVO.CreateRequest.Parameter p : shortBlockMeta.getCreateRequest().getParameters()) {
                            // parameter 처리.
                            ResultSet rs = stmt.executeQuery(
                                    "select seq from dynarap_preset_param " +
                                            "where paramPack = " + p.getParamPack().originOf() + " " +
                                            "  and paramSeq = " + p.getParamSeq().originOf() + " " +
                                            "union " +
                                            "select seq from dynarap_notmapped_param " +
                                            "where uploadSeq = " + shortBlockMeta.getPartInfo().getUploadSeq().originOf() + " " +
                                            "  and paramPack = " + p.getParamPack().originOf() + " " +
                                            "  and paramSeq = " + p.getParamSeq().originOf() + " " +
                                            "limit 0, 1");

                            if (!rs.next()) {
                                logger.info("[[[[[ " + p.getParamKey() + " not found on part info");
                                rs.close();
                                continue;
                            }

                            Long paramSeq = rs.getLong(1);
                            rs.close();

                            pstmt.setLong(1, shortBlockMeta.getSeq().originOf());
                            pstmt.setInt(2, paramNo);
                            pstmt.setLong(3, p.getParamPack() == null || p.getParamPack().isEmpty() ? 0 : p.getParamPack().originOf());
                            pstmt.setLong(4, p.getParamSeq() == null || p.getParamSeq().isEmpty() ? 0 : p.getParamSeq().originOf());
                            pstmt.setString(5, p.getParamKey());
                            pstmt.setString(6, p.getAdamsKey());
                            pstmt.setString(7, p.getZaeroKey());
                            pstmt.setString(8, p.getGrtKey());
                            pstmt.setString(9, p.getFltpKey());
                            pstmt.setString(10, p.getFltsKey());
                            pstmt.setLong(11, paramSeq);
                            pstmt.setLong(12, p.getPropSeq().originOf());
                            pstmt.addBatch();
                            pstmt.clearParameters();
                            paramNo++;
                        }
                        pstmt.executeBatch();
                        pstmt.clearBatch();
                        pstmt.close();

                        conn.commit(); /* 파라미터 정보까지 커밋 */

                        stmt.close();

                        shortBlockMeta.setStatus("create-shortblock");
                        shortBlockMeta.setStatusMessage("숏블록을 생성하고 있습니다.");
                        shortBlockMeta.setFetchCount(0);
                        shortBlockMeta.setTotalFetchCount(shortBlockMeta.getCreateRequest().getShortBlocks().size());
                    }

                    if (shortBlockMeta.getStatus().startsWith("create-shortblock")) {
                        // 파트 정보 로딩하기
                        shortBlockMeta.setPartInfo(partService.getPartBySeq(shortBlockMeta.getPartSeq()));

                        // 파라미터 목록 불러오기
                        shortBlockMeta.setShortBlockParamList(
                                partService.getShortBlockParamList(shortBlockMeta.getSeq()));

                        // 업로드 정보 토대로 신규 데이터 생성.
                        // payload -> [] julianStartAt, julianEndAt
                        List<ShortBlockVO> shortBlockList = new ArrayList<>();
                        if (shortBlockMeta.getCreateRequest().getShortBlocks() != null
                                && shortBlockMeta.getCreateRequest().getShortBlocks().size() > 0) {

                            for (int i = 0; i < shortBlockMeta.getCreateRequest().getShortBlocks().size(); i++) {
                                ShortBlockVO.CreateRequest.ShortBlock blockInfo = shortBlockMeta.getCreateRequest().getShortBlocks().get(i);

                                // partName, julianStartAt, julianEndAt
                                if (blockInfo.getBlockName() == null || blockInfo.getBlockName().isEmpty()) {
                                    blockInfo.setBlockName(new String64(
                                            shortBlockMeta.getPartInfo().getPartName().originOf() + "_SB_"
                                                    + String.format("%04d", blockInfo.getBlockNo())));
                                }

                                if (blockInfo.getJulianStartAt() == null || blockInfo.getJulianEndAt().isEmpty()
                                        || blockInfo.getJulianEndAt() == null || blockInfo.getJulianEndAt().isEmpty()) {
                                    continue;
                                }

                                shortBlockMeta.setFetchCount(i + 1);

                                String julianStartFrom = "";
                                Statement stmt = conn.createStatement();
                                ResultSet rs = null;

                                if (shortBlockMeta.getPartInfo().getJulianStartAt() == null
                                    || shortBlockMeta.getPartInfo().getJulianStartAt().isEmpty()) {
                                    rs = stmt.executeQuery(
                                            "select min(offsetTimeAt) from dynarap_part_raw " +
                                                    "where partSeq = " + shortBlockMeta.getPartSeq().originOf() + " limit 0, 1");
                                    if (rs.next())
                                        julianStartFrom = rs.getString(1);
                                    rs.close();
                                }
                                else {
                                    rs = stmt.executeQuery(
                                            "select min(julianTimeAt) from dynarap_part_raw " +
                                                    "where partSeq = " + shortBlockMeta.getPartSeq().originOf() + " limit 0, 1");
                                    if (rs.next())
                                        julianStartFrom = rs.getString(1);
                                    rs.close();
                                }

                                // short block 생성.
                                ShortBlockVO shortBlock = new ShortBlockVO();
                                shortBlock.setRegisterUid(shortBlockMeta.getRegisterUid());
                                shortBlock.setPartSeq(shortBlockMeta.getPartSeq());
                                shortBlock.setBlockMetaSeq(shortBlockMeta.getSeq());
                                shortBlock.setBlockNo(blockInfo.getBlockNo());
                                shortBlock.setBlockName(blockInfo.getBlockName());

                                if (julianStartFrom.indexOf(":") > -1) {
                                    shortBlock.setJulianStartAt(blockInfo.getJulianStartAt());
                                    shortBlock.setJulianEndAt(blockInfo.getJulianEndAt());
                                    shortBlock.setOffsetStartAt(PartService.getJulianTimeOffset(julianStartFrom, shortBlock.getJulianStartAt()));
                                    shortBlock.setOffsetEndAt(PartService.getJulianTimeOffset(julianStartFrom, shortBlock.getJulianEndAt()));
                                }
                                else {
                                    shortBlock.setJulianStartAt("");
                                    shortBlock.setJulianEndAt("");
                                    shortBlock.setOffsetStartAt(Double.parseDouble(blockInfo.getJulianStartAt()));
                                    shortBlock.setOffsetEndAt(Double.parseDouble(blockInfo.getJulianEndAt()));
                                }

                                PreparedStatement pstmt = conn.prepareStatement(
                                        "insert into dynarap_sblock (" +
                                        "partSeq,blockMetaSeq,blockNo,blockName,julianStartAt,julianEndAt," +
                                        "offsetStartAt,offsetEndAt,registerUid" +
                                        ") values (" +
                                        "?,?,?,?,?,?,?,?,?)");
                                pstmt.setLong(1, shortBlock.getPartSeq().originOf());
                                pstmt.setLong(2, shortBlock.getBlockMetaSeq().originOf());
                                pstmt.setInt(3, shortBlock.getBlockNo());
                                pstmt.setString(4, shortBlock.getBlockName().originOf());
                                pstmt.setString(5, shortBlock.getJulianStartAt());
                                pstmt.setString(6, shortBlock.getJulianEndAt());
                                pstmt.setDouble(7, shortBlock.getOffsetStartAt());
                                pstmt.setDouble(8, shortBlock.getOffsetEndAt());
                                pstmt.setLong(9, shortBlock.getRegisterUid().originOf());
                                pstmt.executeUpdate();
                                pstmt.close();

                                rs = stmt.executeQuery("select last_insert_id() from dynarap_sblock limit 0, 1");
                                if (rs.next())
                                    shortBlock.setSeq(new CryptoField(rs.getLong(1)));
                                rs.close();

                                // 기존 오더 삭제
                                listOps.trim("S" + shortBlock.getSeq().originOf(), 0, Integer.MAX_VALUE);
                                zsetOps.removeRangeByScore("S" + shortBlock.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);

                                for (ShortBlockVO.Param param : shortBlockMeta.getShortBlockParamList()) {
                                    listOps.rightPush("S" + shortBlock.getSeq().originOf(), "P" + param.getUnionParamSeq());
                                    zsetOps.removeRangeByScore("S" + shortBlock.getSeq().originOf() + ".N" + param.getUnionParamSeq(), 0, Integer.MAX_VALUE);
                                    zsetOps.removeRangeByScore("S" + shortBlock.getSeq().originOf() + ".L" + param.getUnionParamSeq(), 0, Integer.MAX_VALUE);
                                    zsetOps.removeRangeByScore("S" + shortBlock.getSeq().originOf() + ".H" + param.getUnionParamSeq(), 0, Integer.MAX_VALUE);
                                }

                                // dump part raw from raw_temp table
                                int minRowNo = -1;
                                int maxRowNo = -1;
                                String minMaxRowQuery = "";

                                if (julianStartFrom.indexOf(":") > -1) {
                                    minMaxRowQuery = "select\n" +
                                            "    (select distinct rowNo from dynarap_part_raw " +
                                            "      where partSeq = " + shortBlock.getPartSeq().originOf() + " " +
                                            "        and julianTimeAt = (\n" +
                                            "        select min(julianTimeAt) from dynarap_part_raw " +
                                            "         where partSeq = " + shortBlock.getPartSeq().originOf() + " " +
                                            "           and julianTimeAt >= '" + shortBlock.getJulianStartAt() + "')) as minRowNo,\n" +
                                            "    (select distinct rowNo from dynarap_part_raw " +
                                            "      where partSeq = " + shortBlock.getPartSeq().originOf() + " " +
                                            "        and julianTimeAt = (\n" +
                                            "        select max(julianTimeAt) from dynarap_part_raw " +
                                            "         where partSeq = " + shortBlock.getPartSeq().originOf() + " " +
                                            "           and julianTimeAt <= '" + shortBlock.getJulianEndAt() + "')) as maxRowNo";
                                }
                                else {
                                    minMaxRowQuery = "select\n" +
                                            "    (select distinct rowNo from dynarap_part_raw " +
                                            "      where partSeq = " + shortBlock.getPartSeq().originOf() + " " +
                                            "        and offsetTimeAt = (\n" +
                                            "        select min(offsetTimeAt) from dynarap_part_raw " +
                                            "         where partSeq = " + shortBlock.getPartSeq().originOf() + " " +
                                            "           and offsetTimeAt >= '" + shortBlock.getOffsetStartAt() + "')) as minRowNo,\n" +
                                            "    (select distinct rowNo from dynarap_part_raw " +
                                            "      where partSeq = " + shortBlock.getPartSeq().originOf() + " " +
                                            "        and offsetTimeAt = (\n" +
                                            "        select max(offsetTimeAt) from dynarap_part_raw " +
                                            "         where partSeq = " + shortBlock.getPartSeq().originOf() + " " +
                                            "           and offsetTimeAt <= '" + shortBlock.getOffsetEndAt() + "')) as maxRowNo";
                                }

                                rs = stmt.executeQuery(minMaxRowQuery);
                                if (rs.next()) {
                                    minRowNo = rs.getInt("minRowNo");
                                    maxRowNo = rs.getInt("maxRowNo");
                                }
                                rs.close();

                                if (minRowNo == -1 || maxRowNo == -1)
                                    throw new Exception("기준 데이터에서 ROW를 찾을 수 없습니다.");

                                long jobStartAt = System.currentTimeMillis();

                                String paramQuery = "select seq, presetParamSeq, rowNo, julianTimeAt, offsetTimeAt, paramVal, paramValStr " +
                                        "from dynarap_part_raw " +
                                        "where partSeq = ? " +
                                        "and rowNo between ? and ? " +
                                        "and presetParamSeq = ? " +
                                        "order by rowNo asc";
                                pstmt = conn.prepareStatement(paramQuery);

                                PreparedStatement pstmt_insert = conn.prepareStatement(
                                        "insert into dynarap_sblock_raw (" +
                                                "blockSeq,partSeq,blockParamSeq,rowNo," +
                                                "julianTimeAt,offsetTimeAt,paramVal,paramValStr," +
                                                "lpf,hpf" +
                                        ") values (?,?,?,?,?,?,?,?,?,?)");

                                for (ShortBlockVO.Param param : shortBlockMeta.getShortBlockParamList()) {
                                    if (param.getUnionParamSeq() == null || param.getUnionParamSeq() == 0) continue;

                                    // part 데이터에서 param에 해당 하는 내용을 가져와서 sblock에 넣어주기.
                                    pstmt.setLong(1, shortBlock.getPartSeq().originOf());
                                    pstmt.setInt(2, minRowNo);
                                    pstmt.setInt(3, maxRowNo);
                                    pstmt.setLong(4, param.getUnionParamSeq());
                                    rs = pstmt.executeQuery();

                                    String rowKey = "S" + shortBlock.getSeq().originOf() + ".N" + param.getUnionParamSeq();
                                    String lpfKey = "S" + shortBlock.getSeq().originOf() + ".L" + param.getUnionParamSeq();
                                    String hpfKey = "S" + shortBlock.getSeq().originOf() + ".H" + param.getUnionParamSeq();

                                    int countTotal = 0;
                                    double sumTotal = 0.0;
                                    double sumLpfTotal = 0.0;
                                    double sumHpfTotal = 0.0;
                                    double blockMin = Integer.MAX_VALUE;
                                    double blockMax = Integer.MIN_VALUE;
                                    double blockLpfMin = Integer.MAX_VALUE;
                                    double blockLpfMax = Integer.MIN_VALUE;
                                    double blockHpfMin = Integer.MAX_VALUE;
                                    double blockHpfMax = Integer.MIN_VALUE;

                                    int batchCount = 1;
                                    while (rs.next()) {
                                        int rowNo = rs.getInt("rowNo");
                                        String rowId = rs.getString("julianTimeAt");
                                        if (julianStartFrom.indexOf(":") == -1)
                                            rowId = String.format("%.05f", rs.getDouble("offsetTimeAt"));

                                        Double dblVal = rs.getDouble("paramVal");

                                        zsetOps.addIfAbsent("S" + shortBlock.getSeq().originOf() + ".R", rowId, rowNo);

                                        pstmt_insert.setLong(1, shortBlock.getSeq().originOf());
                                        pstmt_insert.setLong(2, shortBlock.getPartSeq().originOf());
                                        pstmt_insert.setLong(3, param.getSeq().originOf());
                                        pstmt_insert.setInt(4, rowNo);

                                        if (julianStartFrom.indexOf(":") > -1) {
                                            pstmt_insert.setString(5, rowId);
                                            pstmt_insert.setDouble(6, PartService.getJulianTimeOffset(julianStartFrom, rowId));
                                        }
                                        else {
                                            pstmt_insert.setString(5, "");
                                            pstmt_insert.setDouble(6, Double.parseDouble(rowId));
                                        }

                                        pstmt_insert.setDouble(7, dblVal);
                                        pstmt_insert.setString(8, rs.getString("paramValStr"));

                                        Double lpfVal = getFilteredVal(
                                                "P" + shortBlockMeta.getPartInfo().getSeq().originOf() + ".N" + param.getUnionParamSeq(), "lpf", rowId, dblVal);
                                        Double hpfVal = getFilteredVal(
                                                "P" + shortBlockMeta.getPartInfo().getSeq().originOf() + ".N" + param.getUnionParamSeq(), "hpf", rowId, dblVal);

                                        pstmt_insert.setDouble(9, lpfVal);
                                        pstmt_insert.setDouble(10, hpfVal);

                                        pstmt_insert.addBatch();
                                        pstmt_insert.clearParameters();

                                        if ((batchCount % 1000) == 0) {
                                            pstmt_insert.executeBatch();
                                            pstmt_insert.clearBatch();
                                        }
                                        batchCount++;

                                        if (dblVal != null) {
                                            zsetOps.add(rowKey, rowId + ":" + dblVal, rowNo);
                                            zsetOps.add(lpfKey, rowId + ":" + lpfVal, rowNo);
                                            zsetOps.add(hpfKey, rowId + ":" + hpfVal, rowNo);
                                        }
                                        else
                                            zsetOps.add(rowKey, rowId + ":" + rs.getString("paramValStr"), rowNo);

                                        sumTotal += dblVal;
                                        sumLpfTotal += lpfVal;
                                        sumHpfTotal += hpfVal;

                                        blockMin = Math.min(blockMin, dblVal);
                                        blockMax = Math.max(blockMax, dblVal);
                                        blockLpfMin = Math.min(blockLpfMin, dblVal);
                                        blockLpfMax = Math.max(blockLpfMax, dblVal);
                                        blockHpfMin = Math.min(blockHpfMin, dblVal);
                                        blockHpfMax = Math.max(blockHpfMax, dblVal);

                                        countTotal++;
                                    }
                                    if ((batchCount % 1000) > 0) {
                                        pstmt_insert.executeBatch();
                                        pstmt_insert.clearBatch();
                                        conn.commit();
                                    }
                                    rs.close();

                                    pstmt.clearParameters();

                                    if (countTotal > 0) {
                                        // 파라미터 평균, 민맥스 처리
                                        Statement stmt_block = conn.createStatement();
                                        stmt_block.executeUpdate("insert into dynarap_sblock_param_val (" +
                                                "blockMetaSeq,blockSeq,unionParamSeq,psd,rms,n0,zarray,zPeak,zValley," +
                                                "blockMin,blockMax,blockAvg,blockLpfMin,blockLpfMax,blockLpfAvg,blockHpfMin,blockHpfMax,blockHpfAvg)" +
                                                " values (" +
                                                "" + shortBlockMeta.getSeq().originOf() + "" +
                                                "," + shortBlock.getSeq().originOf() + "" +
                                                "," + param.getUnionParamSeq() + "" +
                                                "," + 0 + "" +
                                                "," + 0 + "" +
                                                "," + 0 + "" +
                                                "," + 0 + "" +
                                                "," + 0 + "" +
                                                "," + 0 + "" +
                                                "," + blockMin + "" +
                                                "," + blockMax + "" +
                                                "," + (sumTotal / countTotal) + "" +
                                                "," + blockLpfMin + "" +
                                                "," + blockLpfMax + "" +
                                                "," + (sumLpfTotal / countTotal) + "" +
                                                "," + blockHpfMin + "" +
                                                "," + blockHpfMax + "" +
                                                "," + (sumHpfTotal / countTotal) + ")");
                                        stmt_block.close();
                                    }
                                }

                                pstmt_insert.close();

                                conn.commit();
                                shortBlockList.add(shortBlock);

                                stmt.close();
                                pstmt.close();

                                logger.info("[[[[[ part data dump completed (" + (System.currentTimeMillis() - jobStartAt) + " msec)" );
                            }
                        }

                        shortBlockMeta.setShortBlockList(shortBlockList);

                        shortBlockMeta.setStatus("create-done");
                        shortBlockMeta.setStatusMessage("이미 완료된 요청입니다.");
                    }

                    conn.commit();
                    conn.setAutoCommit(true);

                    shortBlockMeta.setCreateDone(true);
                    partService.updateShortBlockMeta(shortBlockMeta);

                } catch(Exception ex) {
                    ex.printStackTrace();

                    try {
                        shortBlockMeta.setCreateDone(false);
                        partService.updateShortBlockMeta(shortBlockMeta);
                    } catch(Exception subex) {
                        subex.printStackTrace();
                    }
                }
            }
        };
    }

    public double getFilteredVal(String rowKey, String filterType, String rowId, double rowVal) {
        Long rank = zsetOps.rank(rowKey, rowId + ":" + rowVal);
        if (rank == null) return 0.0;

        Set<String> rows = null;
        if (filterType.equals("lpf")) {
            rows = zsetOps.rangeByScore(
                    rowKey.replaceAll(".N", ".L"), 0, Integer.MAX_VALUE, rank, rank + 1);
        }
        else {
            rows = zsetOps.rangeByScore(
                    rowKey.replaceAll(".N", ".H"), 0, Integer.MAX_VALUE, rank, rank + 1);
        }

        if (rows != null && rows.size() > 0) {
            Iterator<String> iterRows = rows.iterator();
            if (iterRows.hasNext()) {
                String dataKey = iterRows.next();
                String[] splitData = dataKey.split(":");
                return Double.parseDouble(splitData[1]);
            }
        }

        return 0.0;
    }

    public static class Builder {
        private ListOperations<String, String> listOps;
        private ZSetOperations<String, String> zsetOps;

        public ShortBlockCreateTask.Builder setListOps(ListOperations<String, String> listOps) {
            this.listOps = listOps;
            return this;
        }

        public ShortBlockCreateTask.Builder setZsetOps(ZSetOperations<String, String> zsetOps) {
            this.zsetOps = zsetOps;
            return this;
        }

        public ShortBlockCreateTask createShortBlockCreateTask() {
            ShortBlockCreateTask task = new ShortBlockCreateTask();
            task.setListOps(this.listOps);
            task.setZsetOps(this.zsetOps);
            return task;
        }
    }
}
