package com.servetech.dynarap.db.service.task;

import com.servetech.dynarap.DynaRAPServerApplication;
import com.servetech.dynarap.db.service.ParamService;
import com.servetech.dynarap.db.service.PartService;
import com.servetech.dynarap.db.service.RawService;
import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.String64;
import com.servetech.dynarap.vo.ParamVO;
import com.servetech.dynarap.vo.PartVO;
import com.servetech.dynarap.vo.PresetVO;
import com.servetech.dynarap.vo.RawVO;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.core.env.Environment;
import org.springframework.data.redis.core.ListOperations;
import org.springframework.data.redis.core.ZSetOperations;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.scheduling.annotation.Async;
import org.springframework.stereotype.Component;

import javax.sql.DataSource;
import java.io.*;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.Statement;
import java.util.*;

@Component
public class PartImportTask {
    private static final Logger logger = LoggerFactory.getLogger(PartImportTask.class);

    @Value("${dynarap.process.path}")
    private String processPath;

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
                if (processPath == null || processPath.isEmpty()) {
                    Environment env = DynaRAPServerApplication.global.getEnvironment();
                    processPath = env.getProperty("dynarap.process.path");
                }

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

                    Statement stmt = conn.createStatement();

                    if (rawUpload.getStatus().equals("import")) {
                        // 기존 raw 테이블 삭제.
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
                        stmt.executeUpdate("create index `idx_dynarap_" + tempTableName + "_julian` on `dynarap_" + tempTableName + "` (`julianTimeAt`)");
                        stmt.executeUpdate("create index `idx_dynarap_" + tempTableName + "_row` on `dynarap_" + tempTableName + "` (`rowNo`)");
                        stmt.executeUpdate("create index `idx_dynarap_" + tempTableName + "_julian_row` on `dynarap_" + tempTableName + "` (`julianTimeAt`,`rowNo`)");
                        stmt.executeUpdate("create index `idx_dynarap_" + tempTableName + "_preset_row` on `dynarap_" + tempTableName + "` (`presetPack`,`presetSeq`,`presetParamSeq`,`rowNo`)");
                        conn.commit(); // drop 이후 커밋 처리.

                        if (presetParams == null) presetParams = new ArrayList<>();
                        Map<String, ParamVO> adamsMap = new LinkedHashMap<>();
                        Map<String, ParamVO> zaeroMap = new LinkedHashMap<>();
                        Map<String, ParamVO> grtMap = new LinkedHashMap<>();
                        Map<String, ParamVO> fltpMap = new LinkedHashMap<>();
                        Map<String, ParamVO> fltsMap = new LinkedHashMap<>();
                        for (ParamVO param : presetParams) {
                            adamsMap.put(param.getAdamsKey() + "_" + param.getPropInfo().getParamUnit(), param);
                            zaeroMap.put(param.getZaeroKey() + "_" + param.getPropInfo().getParamUnit(), param);
                            grtMap.put(param.getGrtKey() + "_" + param.getPropInfo().getParamUnit(), param);
                            fltpMap.put(param.getFltpKey() + "_" + param.getPropInfo().getParamUnit(), param);
                            fltsMap.put(param.getFltsKey() + "_" + param.getPropInfo().getParamUnit(), param);
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
                        List<ParamVO> tempMappedParams = new ArrayList<>();

                        for (int i = 0; i < splitParam.length; i++) {
                            String p = splitParam[i];
                            if (p.equalsIgnoreCase("date")) continue;

                            ParamVO pi = null;
                            if (rawUpload.getDataType().equals("adams") && adamsMap.containsKey(p)) pi = adamsMap.get(p);
                            if (rawUpload.getDataType().equals("zaero") && zaeroMap.containsKey(p)) pi = zaeroMap.get(p);
                            if (rawUpload.getDataType().equals("grt") && grtMap.containsKey(p)) pi = grtMap.get(p);
                            if (rawUpload.getDataType().equals("fltp") && fltpMap.containsKey(p)) pi = fltpMap.get(p);
                            if (rawUpload.getDataType().equals("flts") && fltsMap.containsKey(p)) pi = fltsMap.get(p);

                            if (pi == null) {
                                RawVO.UploadRequest uploadReq = rawUpload.getUploadRequest();
                                if (uploadReq.getTempMappingParams() != null) {
                                    String tempMappedKey = uploadReq.getTempMappingParams().get(p);
                                    if (tempMappedKey == null || tempMappedKey.isEmpty()) {
                                        notMappedParams.add(p);
                                        continue;
                                    }
                                    else if (tempMappedKey.equals("skip")) {
                                        logger.debug("[[[[[ parameter skipped [" + p + "]");
                                    }
                                    else {
                                        String sensorName = p;
                                        if (sensorName.indexOf("_") > -1)
                                            sensorName = sensorName.substring(0, sensorName.lastIndexOf("_"));

                                        ParamVO tempParam = paramService.getParamByParamKey(rawUpload.getDataType(), sensorName);
                                        if (tempParam == null) {
                                            notMappedParams.add(p);
                                            continue;
                                        }
                                        else {
                                            tempMappedParams.add(tempParam);
                                            mappedParams.add(tempParam);
                                            mappedIndexes.add(i);
                                        }
                                    }
                                }
                                else {
                                    notMappedParams.add(p);
                                }
                            }
                            else {
                                mappedParams.add(pi);
                                mappedIndexes.add(i);
                            }
                        }

                        if (tempMappedParams.size() > 0) {
                            for (ParamVO tempParam : tempMappedParams) {
                                ResultSet rs = stmt.executeQuery(
                                        "select seq from dynarap_notmapped_param " +
                                                "where uploadSeq = " + rawUpload.getSeq().originOf() + " " +
                                                "and paramPack = " + tempParam.getParamPack().originOf() + " " +
                                                "and paramSeq = " + tempParam.getSeq().originOf());
                                if (!rs.next()) {
                                    String notMappedParamKey = tempParam.getAdamsKey();
                                    if (rawUpload.getDataType().equals("zaero")) notMappedParamKey = tempParam.getZaeroKey();
                                    if (rawUpload.getDataType().equals("grt")) notMappedParamKey = tempParam.getGrtKey();
                                    if (rawUpload.getDataType().equals("fltp")) notMappedParamKey = tempParam.getFltpKey();
                                    if (rawUpload.getDataType().equals("flts")) notMappedParamKey = tempParam.getFltsKey();

                                    stmt.executeUpdate("insert into dynarap_notmapped_param (" +
                                            "uploadSeq,paramPack,paramSeq,notMappedParamKey) values (" +
                                            "" + rawUpload.getSeq().originOf() + "" +
                                            "," + tempParam.getParamPack().originOf() + "" +
                                            "," + tempParam.getSeq().originOf() + "" +
                                            ",'" + notMappedParamKey + "')");

                                    rs.close();

                                    rs = stmt.executeQuery("select last_insert_id() from dynarap_notmapped_param limit 0, 1");
                                    if (rs.next())
                                        tempParam.setReferenceSeq(rs.getLong(1));
                                    rs.close();
                                }
                                else {
                                    tempParam.setReferenceSeq(rs.getLong(1));
                                }
                                rs.close();
                            }
                        }

                        rawUpload.setNotMappedParams(notMappedParams);
                        rawUpload.setMappedParams(mappedParams);

                        if (notMappedParams.size() > 0) {
                            // 여기서 중단.
                            br.close();
                            fis.close();

                            rawUpload.setImportDone(false);
                            rawUpload.setStatus("error");
                            rawUpload.setStatusMessage("매치되지 않은 파라미터가 있습니다.");
                            rawService.updateRawUpload(rawUpload);

                            conn.setAutoCommit(true);
                            conn.close();
                            return;
                        }

                        // skip next line [blank link]
                        br.readLine();

                        // 기존 데이터 삭제
                        stmt.executeUpdate("delete from dynarap_part_raw where partSeq in " +
                                "(select seq from dynarap_part where uploadSeq = " + rawUpload.getSeq().originOf() + ")");
                        stmt.executeUpdate("delete from dynarap_part where uploadSeq = " + rawUpload.getSeq().originOf());
                        conn.commit();

                        // real data insert
                        long startTime = System.currentTimeMillis();

                        // inbound row 찾기
                        List<PartVO> partList = new ArrayList<>();

                        if (rawUpload.getUploadRequest().getParts() != null && rawUpload.getUploadRequest().getParts().size() > 0) {

                            String startJulianTime = "";
                            int partIndex = 0;
                            PartVO currentPart = null;

                            for (int i = 0; i < rawUpload.getUploadRequest().getParts().size(); i++) {
                                RawVO.UploadRequest.UploadPart partInfo = rawUpload.getUploadRequest().getParts().get(i);

                                // partName, julianStartAt, julianEndAt
                                if (partInfo.getPartName() == null || partInfo.getPartName().isEmpty())
                                    partInfo.setPartName(new String64(rawUpload.getUploadName().originOf() + "_분할_" + String.format("%04d", (i + 1))));

                                if (partInfo.getJulianStartAt() == null || partInfo.getJulianEndAt().isEmpty()
                                        || partInfo.getJulianEndAt() == null || partInfo.getJulianEndAt().isEmpty()) {
                                    continue;
                                }

                                currentPart = new PartVO();
                                currentPart.setRegisterUid(rawUpload.getRegisterUid());
                                currentPart.setUploadSeq(rawUpload.getSeq());
                                currentPart.setPresetPack(rawUpload.getPresetPack());
                                currentPart.setPresetSeq(rawUpload.getPresetSeq());
                                currentPart.setPartName(partInfo.getPartName());
                                currentPart.setJulianStartAt(partInfo.getJulianStartAt());
                                currentPart.setJulianEndAt(partInfo.getJulianEndAt());
                                currentPart.setOffsetStartAt(0.0);
                                currentPart.setOffsetEndAt(0.0);

                                PreparedStatement pstmt_part = conn.prepareStatement("insert into dynarap_part (" +
                                        "uploadSeq,partName,presetPack,presetSeq,julianStartAt,julianEndAt," +
                                        "offsetStartAt,offsetEndAt,registerUid" +
                                        ") values (" +
                                        "?,?,?,?,?,?,?,?,?)");
                                pstmt_part.setLong(1, currentPart.getUploadSeq().originOf());
                                pstmt_part.setString(2, currentPart.getPartName().originOf());
                                pstmt_part.setLong(3, currentPart.getPresetPack().originOf());
                                pstmt_part.setLong(4, currentPart.getPresetSeq().originOf());
                                pstmt_part.setString(5, currentPart.getJulianStartAt());
                                pstmt_part.setString(6, currentPart.getJulianEndAt());
                                pstmt_part.setDouble(7, currentPart.getOffsetStartAt());
                                pstmt_part.setDouble(8, currentPart.getOffsetEndAt());
                                pstmt_part.setLong(9, currentPart.getRegisterUid().originOf());
                                pstmt_part.executeUpdate();
                                pstmt_part.close();

                                ResultSet rs = stmt.executeQuery("select last_insert_id() from dynarap_part limit 0, 1");
                                if (rs.next())
                                    currentPart.setSeq(new CryptoField(rs.getLong(1)));
                                rs.close();

                                partList.add(currentPart);
                            }

                            boolean isInbound = false;
                            int rowNo = 1;
                            int batchCount = 1;
                            long jobStartAt = 0;

                            PreparedStatement pstmt_insert = conn.prepareStatement("insert into dynarap_part_raw (" +
                                    "partSeq,presetParamSeq,rowNo,julianTimeAt,offsetTimeAt,paramVal,paramValStr,lpf,hpf" +
                                    ") values (?,?,?,?,?,?,?,?,?)");

                            line = br.readLine();
                            while (line != null) {
                                String[] splitData = line.trim().split(",");

                                String julianTimeAt = splitData[0];
                                if (startJulianTime.isEmpty()) {
                                    startJulianTime = julianTimeAt;

                                    // part 3개에 대한 offsetTime 변경.
                                    for (PartVO part : partList) {
                                        part.setOffsetStartAt(PartService.getJulianTimeOffset(startJulianTime, part.getJulianStartAt()));
                                        part.setOffsetEndAt(PartService.getJulianTimeOffset(startJulianTime, part.getJulianEndAt()));

                                        stmt.executeUpdate("update dynarap_part set " +
                                                "offsetStartAt = " + part.getOffsetStartAt() + "," +
                                                "offsetEndAt = " + part.getOffsetEndAt() + " " +
                                                "where seq = " + part.getSeq().originOf());
                                    }
                                }

                                currentPart = partList.get(partIndex);
                                double offsetTimeAt = PartService.getJulianTimeOffset(startJulianTime, julianTimeAt);
                                if (offsetTimeAt < currentPart.getOffsetStartAt()) {
                                    // 현재 블록 보다 값이 작으므로 패스 => 버림.
                                    rowNo++;
                                    line = br.readLine();
                                    continue;
                                }

                                if (isInbound == false) {
                                    // 블록에 최초 진입.
                                    rawUpload.setStatus("create-part-" + partIndex);
                                    rawUpload.setStatusMessage((partIndex + 1) + "번째 분할 구간 데이터를 생성하고 있습니다.");

                                    // 기존 오더 삭제
                                    listOps.trim("P" + currentPart.getSeq().originOf(), 0, Integer.MAX_VALUE);
                                    zsetOps.removeRangeByScore("P" + currentPart.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);

                                    for (ParamVO param : mappedParams) {
                                        listOps.rightPush("P" + currentPart.getSeq().originOf(), "P" + param.getReferenceSeq());
                                        zsetOps.removeRangeByScore("P" + currentPart.getSeq().originOf() + ".N" + param.getReferenceSeq(), 0, Integer.MAX_VALUE);
                                        zsetOps.removeRangeByScore("P" + currentPart.getSeq().originOf() + ".L" + param.getReferenceSeq(), 0, Integer.MAX_VALUE);
                                        zsetOps.removeRangeByScore("P" + currentPart.getSeq().originOf() + ".H" + param.getReferenceSeq(), 0, Integer.MAX_VALUE);
                                    }
                                    isInbound = true;
                                    jobStartAt = System.currentTimeMillis();
                                }

                                if (offsetTimeAt > currentPart.getOffsetEndAt()) {
                                    logger.debug("[[[[[ " + partIndex + " job finished " + (System.currentTimeMillis() - jobStartAt) + "ms");

                                    if (isInbound == true) {
                                        partIndex++;
                                        isInbound = false;

                                        if (partIndex >= partList.size()) {
                                            // 전체 작업 종료.
                                            break;
                                        }
                                    }
                                    continue;
                                }

                                // 여기서 부터 저장 로직 시작.
                                for (int i = 0; i < mappedParams.size(); i++) {
                                    ParamVO pi = mappedParams.get(i);
                                    int spi = mappedIndexes.get(i);

                                    // mapped, notMapped 파라미터 처리
                                    // => param 의 seq 를 매핑하도록 하자.
                                    String rowKey = "P" + currentPart.getSeq().originOf() + ".N" + pi.getReferenceSeq();

                                    pstmt_insert.setLong(1, currentPart.getSeq().originOf());
                                    pstmt_insert.setLong(2, pi.getReferenceSeq());
                                    pstmt_insert.setInt(3, rowNo);
                                    pstmt_insert.setString(4, julianTimeAt);
                                    pstmt_insert.setDouble(5, PartService.getJulianTimeOffset(startJulianTime, julianTimeAt));

                                    String paramValStr = splitData[spi];
                                    try {
                                        pstmt_insert.setDouble(6, Double.parseDouble(paramValStr));
                                        pstmt_insert.setString(7, null);
                                    } catch(NumberFormatException nfe) {
                                        pstmt_insert.setDouble(6, 0);
                                        pstmt_insert.setString(7, paramValStr);
                                    }

                                    pstmt_insert.setDouble(8, Double.parseDouble(paramValStr)); // temp lpf
                                    pstmt_insert.setDouble(9, Double.parseDouble(paramValStr)); // temp hpf
                                    pstmt_insert.addBatch();
                                    pstmt_insert.clearParameters();

                                    if ((batchCount % 1000) == 0) {
                                        pstmt_insert.executeBatch();
                                        pstmt_insert.clearBatch();
                                    }
                                    batchCount++;

                                    // 실 데이터만 저장. lpf, hpf 는 redis에 아직 없음.
                                    zsetOps.add(rowKey, julianTimeAt + ":" + Double.parseDouble(paramValStr), rowNo);
                                }

                                zsetOps.add("P" + currentPart.getSeq().originOf() + ".R", julianTimeAt, rowNo);
                                logger.debug("[[[[[ " + "P" + currentPart.getSeq().originOf() + ".R, " + julianTimeAt + ", " + rowNo);

                                rowNo++;
                                line = br.readLine();
                                rawUpload.setFetchCount(rowNo); // row 단위 처리만 카운팅.
                            }

                            if ((batchCount % 1000) > 0) {
                                pstmt_insert.executeBatch();
                                pstmt_insert.clearBatch();
                            }

                            pstmt_insert.close();
                        }

                        br.close();
                        fis.close();

                        // 여기까지 오면 raw 데이터는 성공했음.
                        rawUpload.setImportDone(true);
                        rawService.updateRawUpload(rawUpload);

                        // lpf, hpf 처리
                        for (PartVO part : partList) {
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

                                applyFilterData(processPath, zsetOps,"lpf", "10", "0.4", "", part, param, jts, pvs, startRowAt + rankFrom);
                                applyFilterData(processPath, zsetOps,"hpf", "10", "0.02", "high", part, param, jts, pvs, startRowAt + rankFrom);
                            }

                            part.setLpfDone(true);
                            part.setHpfDone(true);
                            stmt.executeUpdate("update dynarap_part set lpfDone = 1, hpfDone = 1 where seq = " + part.getSeq().originOf());
                        }

                        rawUpload.setPartList(partList);

                        rawUpload.setStatus("import-done");
                        rawUpload.setStatusMessage("이미 완료된 요청입니다.");
                    }

                    stmt.close();

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

    public static void applyFilterData(String processPath, ZSetOperations<String, String> zsetOps,
                                       String filterType, String n, String cutoff, String btype,
                                 PartVO part, ParamVO param,
                                 List<String> jts, List<Double> pvs, long rowNo) throws IOException, InterruptedException {
        // get console run python
        ProcessBuilder builder = new ProcessBuilder();

        if (filterType.equals("lpf")) {
            builder.command(processPath + "/lpf_filter.sh",
                    join(pvs, ","), "10", "0.4");
        }
        else if (filterType.equals("hpf")) {
            builder.command(processPath + "/hpf_filter.sh",
                    join(pvs, ","), "10", "0.02");
        }

        Process process = builder.start();
        StringBuilder sbFilterResult = new StringBuilder();
        BufferedReader reader = new BufferedReader(new InputStreamReader(process.getInputStream()));

        String filterResult = null;
        String pline = null;
        while ((pline = reader.readLine()) != null) {
            if (pline.startsWith("result1=")) {
                filterResult = pline.substring("result1=".length());
                if (filterResult.endsWith(","))
                    filterResult = filterResult.substring(0, filterResult.length() - 1);
            }
            sbFilterResult.append(pline + "\n");
        }
        process.waitFor();

        // extract result1
        String[] splitData = null;
        if (filterResult != null && !filterResult.isEmpty())
            splitData = filterResult.split(",");

        if (splitData == null || jts.size() != splitData.length) {
            logger.debug("[[[[[ 해석 결과와 길이가 같지 않음. [" + param.getParamKey() + "]");
            return;
        }

        String rowKey = "P" + part.getSeq().originOf() + ".L" + param.getReferenceSeq();
        if (filterType.equals("hpf"))
            rowKey = "P" + part.getSeq().originOf() + ".H" + param.getReferenceSeq();

        for (int i = 0; i < jts.size(); i++) {
            zsetOps.addIfAbsent(rowKey, jts.get(i) + ":" + splitData[i], (int) (rowNo + i));
            logger.debug("[[[[[ " + rowKey + ", julianTime = " + jts.get(i) + ", rowNo = " + (rowNo + i));
        }

        logger.debug("[[[[[ " + filterType + " is done... " + part.getSeq().originOf());
    }

    public static String join(List<Double> dblData, String delim) {
        StringBuilder sbOut = new StringBuilder();
        for (Double d : dblData)
            sbOut.append(d).append(",");
        if (sbOut.length() > 0) sbOut.setLength(sbOut.length() - 1);
        return sbOut.toString();
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
