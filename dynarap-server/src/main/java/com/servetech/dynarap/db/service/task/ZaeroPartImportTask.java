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

import javax.script.ScriptEngine;
import javax.script.ScriptEngineManager;
import javax.sql.DataSource;
import java.io.*;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.Statement;
import java.util.*;

import static com.servetech.dynarap.db.service.task.PartImportTask.extractParams;

@Component
public class ZaeroPartImportTask {
    private static final Logger logger = LoggerFactory.getLogger(ZaeroPartImportTask.class);

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
                        if (presetParams == null) presetParams = new ArrayList<>();
                        Map<String, ParamVO> adamsMap = new LinkedHashMap<>();
                        Map<String, ParamVO> zaeroMap = new LinkedHashMap<>();
                        for (ParamVO param : presetParams) {
                            adamsMap.put(param.getAdamsKey(), param);
                            zaeroMap.put(param.getZaeroKey(), param);
                        }

                        // File loading
                        File fImport = new File(rawUpload.getStorePath());
                        FileInputStream fis = new FileInputStream(fImport);

                        BufferedReader br = new BufferedReader(new InputStreamReader(fis));
                        String line = null;

                        String state = "before";
                        List<List<String>> parameters = new ArrayList<>();
                        List<List<List<Double>>> dataList = new ArrayList<>();
                        LinkedList<Double> timeSet = new LinkedList<>();

                        List<String> blockParams = null;
                        List<List<Double>> blockDatas = null;

                        while ((line = br.readLine()) != null) {
                            line = line.trim();

                            String[] splitted = line.split("\\s+");
                            if (splitted == null || splitted.length < 2) {
                                if (!state.equals("before")) {
                                    state = "before";
                                }
                                continue;
                            }

                            if (state.equals("before")) {
                                if (!splitted[0].equalsIgnoreCase("UNITS"))
                                    continue;

                                // append parameter array
                                blockParams = new ArrayList<>();
                                blockDatas = new ArrayList<>();

                                for (int i = 1; i < splitted.length; i++) {
                                    blockParams.add(splitted[i]);
                                    blockDatas.add(new ArrayList<>());
                                }

                                parameters.add(blockParams);
                                dataList.add(blockDatas);

                                state = "extract";
                                continue;
                            }

                            if (state.equals("extract")) {
                                if (!timeSet.contains(Double.parseDouble(splitted[0])))
                                    timeSet.add(Double.parseDouble(splitted[0]));

                                for (int i = 1; i < splitted.length; i++) {
                                    if (i < splitted.length)
                                        blockDatas.get(i - 1).add(Double.parseDouble(splitted[i]));
                                    else
                                        blockDatas.get(i - 1).add(0.0);
                                }
                            }
                        }

                        List<String> allParams = new ArrayList<>();
                        for (List<String> p : parameters)
                            allParams.addAll(p);

                        List<List<Double>> allData = new ArrayList<>();
                        for (List<List<Double>> d : dataList)
                            allData.addAll(d);

                        br.close();
                        fis.close();

                        // data 0 is date
                        List<String> notMappedParams = new ArrayList<>();

                        List<Integer> mappedIndexes = new ArrayList<>();
                        List<ParamVO> mappedParams = new ArrayList<>();
                        List<ParamVO> tempMappedParams = new ArrayList<>();

                        for (int i = 0; i < allParams.size(); i++) {
                            String p = allParams.get(i);
                            ParamVO pi = null;
                            if (rawUpload.getDataType().equals("adams") && adamsMap.containsKey(p)) pi = adamsMap.get(p);
                            if (rawUpload.getDataType().equals("zaero") && zaeroMap.containsKey(p)) pi = zaeroMap.get(p);

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

                                        ParamVO tempParam = paramService.getParamByParamKey("param", tempMappedKey);
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

                        Map<String, List<String>> qMap = new LinkedHashMap<>();
                        if (rawUpload.getUploadRequest().getQEquation() != null
                                && !rawUpload.getUploadRequest().getQEquation().isEmpty()) {
                            List<String> sensorParams = extractParams(rawUpload.getUploadRequest().getQEquation());
                            if (sensorParams != null && sensorParams.size() > 0) {
                                for (String sensor : sensorParams) {
                                    ParamVO findParam = null;
                                    for (ParamVO p : mappedParams) {
                                        if (rawUpload.getDataType().equals("adams") && p.getAdamsKey().equals(sensor)) {
                                            findParam = p;
                                            break;
                                        }
                                        if (rawUpload.getDataType().equals("zaero") && p.getZaeroKey().equals(sensor)) {
                                            findParam = p;
                                            break;
                                        }
                                    }
                                    if (findParam == null) {
                                        // 여기서 중단.
                                        br.close();
                                        fis.close();

                                        rawUpload.setImportDone(false);
                                        rawUpload.setStatus("error");
                                        rawUpload.setStatusMessage("동압 식에 매핑되지 않은 센서 이름이 있습니다.");
                                        rawService.updateRawUpload(rawUpload);

                                        conn.setAutoCommit(true);
                                        conn.close();
                                        return;
                                    }
                                    qMap.put(sensor, new ArrayList<String>());
                                }
                            }
                        }

                        if (notMappedParams.size() > 0) {
                            rawUpload.setImportDone(false);
                            rawUpload.setStatus("error");
                            rawUpload.setStatusMessage("매치되지 않은 파라미터가 있습니다.");
                            rawService.updateRawUpload(rawUpload);

                            conn.setAutoCommit(true);
                            conn.close();
                            return;
                        }

                        // 기존 데이터 삭제
                        stmt.executeUpdate("delete from dynarap_part_raw where partSeq in " +
                                "(select seq from dynarap_part where uploadSeq = " + rawUpload.getSeq().originOf() + ")");
                        stmt.executeUpdate("delete from dynarap_part where uploadSeq = " + rawUpload.getSeq().originOf());
                        conn.commit();

                        // real data insert
                        long startTime = System.currentTimeMillis();

                        // inbound row 찾기
                        List<PartVO> partList = new ArrayList<>();
                        ParamVO qParam = null;

                        if (rawUpload.getUploadRequest().getParts() != null && rawUpload.getUploadRequest().getParts().size() > 0) {

                            String startJulianTime = "";
                            int partIndex = 0;
                            PartVO currentPart = null;

                            for (int i = 0; i < rawUpload.getUploadRequest().getParts().size(); i++) {
                                RawVO.UploadRequest.UploadPart partInfo = rawUpload.getUploadRequest().getParts().get(i);

                                // partName, julianStartAt, julianEndAt
                                if (partInfo.getPartName() == null || partInfo.getPartName().isEmpty())
                                    partInfo.setPartName(new String64(rawUpload.getUploadName().originOf() + "_분할_" + String.format("%04d", (i + 1))));

                                if (partInfo.getOffsetStartAt() == null || partInfo.getOffsetStartAt().isEmpty()
                                        || partInfo.getOffsetEndAt() == null || partInfo.getOffsetEndAt().isEmpty()) {
                                    continue;
                                }

                                currentPart = new PartVO();
                                currentPart.setRegisterUid(rawUpload.getRegisterUid());
                                currentPart.setUploadSeq(rawUpload.getSeq());
                                currentPart.setPresetPack(rawUpload.getPresetPack());
                                currentPart.setPresetSeq(rawUpload.getPresetSeq());
                                currentPart.setPartName(partInfo.getPartName());
                                currentPart.setJulianStartAt("");
                                currentPart.setJulianEndAt("");
                                currentPart.setOffsetStartAt(Double.parseDouble(partInfo.getOffsetStartAt()));
                                currentPart.setOffsetEndAt(Double.parseDouble(partInfo.getOffsetEndAt()));

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

                            for (rowNo = 0; rowNo < timeSet.size(); ) {
                                currentPart = partList.get(partIndex);
                                double offsetTimeAt = timeSet.get(rowNo);
                                if (offsetTimeAt < currentPart.getOffsetStartAt()) {
                                    // 현재 블록 보다 값이 작으므로 패스 => 버림.
                                    rowNo++;
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
                                        zsetOps.removeRangeByScore("P" + currentPart.getSeq().originOf() + ".B" + param.getReferenceSeq(), 0, Integer.MAX_VALUE);
                                    }

                                    // 동압 있으면
                                    if (rawUpload.getUploadRequest().getQEquation() != null && !rawUpload.getUploadRequest().getQEquation().isEmpty()) {
                                        // 동압 파라미터 처리
                                        Statement stmt_q = conn.createStatement();
                                        ResultSet rs_q = stmt_q.executeQuery("select * from dynarap_param where paramKey = 'Dynamic Pressure' and appliedEndAt = 0 limit 0, 1");
                                        qParam = null;

                                        if (!rs_q.next()) {
                                            rs_q.close();
                                            stmt_q.executeUpdate("insert into dynarap_param (paramPack, paramKey, appliedAt, appliedEndAt) values (0, 'Dynamic Pressure', " + System.currentTimeMillis() + ", 0)");
                                            rs_q = stmt_q.executeQuery("select last_insert_id() from dynarap_param limit 0, 1");
                                            if (rs_q.next())
                                                stmt_q.executeUpdate("update dynarap_param set paramPack = " + rs_q.getLong(1) + " where seq = " + rs_q.getLong(1));
                                            rs_q.close();

                                            rs_q = stmt_q.executeQuery("select * from dynarap_param where paramKey = 'Dynamic Pressure' and appliedEndAt = 0 limit 0, 1");
                                            rs_q.next();
                                        }

                                        qParam = new ParamVO();
                                        qParam.setSeq(new CryptoField(rs_q.getLong("seq")));
                                        qParam.setParamPack(new CryptoField(rs_q.getLong("paramPack")));
                                        qParam.setParamKey(rs_q.getString("paramKey"));
                                        rs_q.close();

                                        rs_q = stmt_q.executeQuery("select * from dynarap_notmapped_param where uploadSeq = " + rawUpload.getSeq().originOf()
                                                + " and paramSeq = " + qParam.getSeq().originOf()
                                                + " limit 0, 1");
                                        if (!rs_q.next()) {
                                            rs_q.close();
                                            stmt_q.executeUpdate("insert into dynarap_notmapped_param (uploadSeq,paramPack,paramSeq,notMappedParamKey) " +
                                                    "values (" + rawUpload.getSeq().originOf() + ", " + qParam.getParamPack().originOf() + ", " + qParam.getSeq().originOf() + ", 'Dynamic Pressure')");
                                            rs_q = stmt_q.executeQuery("select last_insert_id() from dynarap_notmapped_param limit 0, 1");
                                            rs_q.next();
                                        }

                                        qParam.setReferenceSeq(rs_q.getLong(1));
                                        rs_q.close();

                                        listOps.rightPush("P" + currentPart.getSeq().originOf(), "P" + qParam.getReferenceSeq());
                                        zsetOps.removeRangeByScore("P" + currentPart.getSeq().originOf() + ".N" + qParam.getReferenceSeq(), 0, Integer.MAX_VALUE);
                                        zsetOps.removeRangeByScore("P" + currentPart.getSeq().originOf() + ".L" + qParam.getReferenceSeq(), 0, Integer.MAX_VALUE);
                                        zsetOps.removeRangeByScore("P" + currentPart.getSeq().originOf() + ".H" + qParam.getReferenceSeq(), 0, Integer.MAX_VALUE);
                                        zsetOps.removeRangeByScore("P" + currentPart.getSeq().originOf() + ".B" + qParam.getReferenceSeq(), 0, Integer.MAX_VALUE);

                                        stmt_q.close();
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
                                    pstmt_insert.setString(4, "");
                                    pstmt_insert.setDouble(5, offsetTimeAt);

                                    pstmt_insert.setDouble(6, allData.get(spi).get(rowNo));
                                    pstmt_insert.setString(7, null);

                                    pstmt_insert.setDouble(8, allData.get(spi).get(rowNo)); // temp lpf
                                    pstmt_insert.setDouble(9, allData.get(spi).get(rowNo)); // temp hpf
                                    pstmt_insert.addBatch();
                                    pstmt_insert.clearParameters();

                                    if ((batchCount % 1000) == 0) {
                                        pstmt_insert.executeBatch();
                                        pstmt_insert.clearBatch();
                                    }
                                    batchCount++;

                                    // 실 데이터만 저장. lpf, hpf 는 redis에 아직 없음.
                                    zsetOps.add(rowKey, String.format("%.05f", offsetTimeAt) + ":" + allData.get(spi).get(rowNo), rowNo);
                                }

                                if (rawUpload.getUploadRequest().getQEquation() != null && !rawUpload.getUploadRequest().getQEquation().isEmpty()) {
                                    // qParam은 설정되어 있음.
                                    List<String> sensorParams = extractParams(rawUpload.getUploadRequest().getQEquation());
                                    String equation = rawUpload.getUploadRequest().getQEquation();
                                    for (String sensor : sensorParams) {
                                        for (int i = 0; i < mappedParams.size(); i++) {
                                            if ((rawUpload.getDataType().equals("adams") && mappedParams.get(i).getAdamsKey().equals(sensor))
                                                    || (rawUpload.getDataType().equals("zaero") && mappedParams.get(i).getZaeroKey().equals(sensor))) {
                                                int spi = mappedIndexes.get(i);
                                                String paramValStr = Double.toString(allData.get(spi).get(rowNo));
                                                equation = equation.replaceAll("\\$\\{" + sensor + "\\}", "(" + paramValStr + ")");
                                                break;
                                            }
                                        }
                                    }
                                    Double qVal = 0.0;
                                    try {
                                        ScriptEngineManager mgr = new ScriptEngineManager();
                                        ScriptEngine engine = mgr.getEngineByName("JavaScript");
                                        qVal = (Double) engine.eval(equation);
                                    } catch(Exception e) {
                                        logger.debug("[[[[[ 동압 결과 오류... " + equation);
                                        qVal = 0.0;
                                    }

                                    String rowKey = "P" + currentPart.getSeq().originOf() + ".N" + qParam.getReferenceSeq();
                                    zsetOps.add(rowKey, String.format("%.05f", offsetTimeAt) + ":" + qVal, rowNo); // 동압 저장..
                                    logger.debug("[[[[[ 동압 결과 저장 ... " + rowKey + " ]]]]]");

                                    String insertQuery = "insert into dynarap_part_raw (" +
                                            "partSeq,presetParamSeq,rowNo,julianTimeAt,offsetTimeAt,paramVal,paramValStr,lpf,hpf" +
                                            ") values (" +
                                            currentPart.getSeq().originOf() + "," +
                                            qParam.getReferenceSeq() + "," +
                                            rowNo + "," +
                                            "''," +
                                            offsetTimeAt + "," +
                                            qVal + "," +
                                            "'',0.0,0.0)";
                                    //logger.debug("[[[[[ " + insertQuery + "]]]]]");

                                    stmt.executeUpdate(insertQuery);
                                }

                                zsetOps.add("P" + currentPart.getSeq().originOf() + ".R", String.format("%.05f", offsetTimeAt), rowNo);
                                logger.debug("[[[[[ " + "P" + currentPart.getSeq().originOf() + ".R, " + String.format("%.05f", offsetTimeAt) + ", " + rowNo);

                                rowNo++;
                                rawUpload.setFetchCount(rowNo); // row 단위 처리만 카운팅.
                            }

                            if ((batchCount % 1000) > 0) {
                                pstmt_insert.executeBatch();
                                pstmt_insert.clearBatch();
                            }

                            pstmt_insert.close();
                        }

                        // 여기까지 오면 raw 데이터는 성공했음.
                        rawUpload.setImportDone(true);
                        rawUpload.setQEquation(rawUpload.getUploadRequest().getQEquation());
                        rawService.updateRawUpload(rawUpload);

                        // 동압 파라미터 추가.
                        if (rawUpload.getUploadRequest().getQEquation() != null && !rawUpload.getUploadRequest().getQEquation().isEmpty())
                            mappedParams.add(qParam);

                        // lpf, hpf 처리
                        for (PartVO part : partList) {
                            String offsetFrom = "";
                            Set<String> listSet = zsetOps.rangeByScore("P" + part.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);
                            if (listSet != null && listSet.size() > 0)
                                offsetFrom = listSet.iterator().next();

                            String offsetTo = "";
                            listSet = zsetOps.reverseRangeByScore("P" + part.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);
                            if (listSet != null && listSet.size() > 0)
                                offsetTo = listSet.iterator().next();

                            if (offsetFrom == null || offsetFrom.isEmpty()
                                    || offsetTo == null || offsetTo.isEmpty()) {
                                logger.debug("[[[[[ There is no record on part " + part.getSeq().originOf());
                                continue;
                            }

                            //String offsetStart = zsetOps.rangeByScore("P" + part.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE).iterator().next();
                            Long startRowAt = zsetOps.score("P" + part.getSeq().originOf() + ".R", offsetFrom).longValue();

                            Long rankFrom = zsetOps.rank("P" + part.getSeq().originOf() + ".R", offsetFrom);
                            if (rankFrom == null) {
                                offsetFrom = zsetOps.rangeByScore("P" + part.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE).iterator().next();
                                rankFrom = zsetOps.rank("P" + part.getSeq().originOf() + ".R", offsetFrom);
                            }

                            Long rankTo = zsetOps.rank("P" + part.getSeq().originOf() + ".R", offsetTo);
                            if (rankTo == null) {
                                offsetTo = zsetOps.reverseRangeByScore("P" + part.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE).iterator().next();
                                rankTo = zsetOps.rank("P" + part.getSeq().originOf() + ".R", offsetTo);
                            }

                            for (ParamVO param : mappedParams) {
                                String rowKey = "P" + part.getSeq().originOf() + ".N" + param.getReferenceSeq();

                                listSet = zsetOps.rangeByScore(
                                        rowKey, startRowAt + rankFrom, startRowAt + rankTo);
                                Iterator<String> iterListSet = listSet.iterator();

                                List<String> ots = new ArrayList<>();
                                List<Double> pvs = new ArrayList<>();

                                while (iterListSet.hasNext()) {
                                    String rowVal = iterListSet.next();
                                    String offsetTime = rowVal.substring(0, rowVal.lastIndexOf(":"));
                                    ots.add(offsetTime);
                                    Double dblVal = Double.parseDouble(rowVal.substring(rowVal.lastIndexOf(":") + 1));
                                    pvs.add(dblVal);
                                }

                                List<Double> lpfSet = applyFilterData(processPath, zsetOps,"lpf",
                                    rawUpload.getUploadRequest().getLpfOption().getN(),
                                    rawUpload.getUploadRequest().getLpfOption().getCutoff(),
                                    rawUpload.getUploadRequest().getLpfOption().getBtype(),
                                    part, param, ots, pvs, startRowAt + rankFrom);
                                List<Double> hpfSet = applyFilterData(processPath, zsetOps,"hpf",
                                    rawUpload.getUploadRequest().getHpfOption().getN(),
                                    rawUpload.getUploadRequest().getHpfOption().getCutoff(),
                                    rawUpload.getUploadRequest().getHpfOption().getBtype(),
                                    part, param, ots, pvs, startRowAt + rankFrom);
                                List<Double> bpfSet = applyFilterData(processPath, zsetOps,"bpf",
                                        rawUpload.getUploadRequest().getHpfOption().getN(),
                                        rawUpload.getUploadRequest().getHpfOption().getCutoff(),
                                        rawUpload.getUploadRequest().getHpfOption().getBtype(),
                                        part, param, ots, lpfSet, startRowAt + rankFrom);
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

                    rawUpload.setStatus("error");
                    rawUpload.setStatusMessage("업로드에 오류가 있습니다. [" + ex.getMessage() + "]");

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

    public static List<Double> applyFilterData(String processPath, ZSetOperations<String, String> zsetOps,
                                       String filterType, String n, String cutoff, String btype,
                                 PartVO part, ParamVO param,
                                 List<String> ots, List<Double> pvs, long rowNo) throws IOException, InterruptedException {
        // get console run python
        ProcessBuilder builder = new ProcessBuilder();

        File fTemp = new File(processPath, part.getSeq().valueOf() + ".dat");
        FileOutputStream fos = new FileOutputStream(fTemp);
        BufferedWriter bw = new BufferedWriter(new OutputStreamWriter(fos));
        bw.write(join(pvs, ","));
        bw.flush();
        bw.close();

        if (filterType.equals("lpf")) {
            builder.command(processPath + "lpf_filter.bat",
                    fTemp.getAbsolutePath(), n, cutoff, btype == null ? "low" : btype);
        }
        else if (filterType.equals("hpf") || filterType.equals("bpf")) {
            builder.command(processPath + "hpf_filter.bat",
                    fTemp.getAbsolutePath(), n, cutoff, btype == null ? "high" : btype);
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

        logger.debug("[[[[[ filterResult = " + filterResult);

        // extract result1
        String[] splitData = null;
        if (filterResult != null && !filterResult.isEmpty())
            splitData = filterResult.split(",");

        if (splitData == null) {
          logger.debug("[[[[[ 필터값이 없음. [" + param.getParamKey() + "]");
          return new ArrayList<>();
        }
        //if (splitData == null || ots.size() != splitData.length) {
        //    logger.debug("[[[[[ 해석 결과와 길이가 같지 않음. [" + param.getParamKey() + "]");
        //    return;
        //}

        String rowKey = "P" + part.getSeq().originOf() + ".L" + param.getReferenceSeq();
        if (filterType.equals("hpf"))
            rowKey = "P" + part.getSeq().originOf() + ".H" + param.getReferenceSeq();
        else if (filterType.equals("bpf"))
            rowKey = "P" + part.getSeq().originOf() + ".B" + param.getReferenceSeq();

        List<Double> resultSet = new ArrayList<>();
        int outBound = Math.max(ots.size(), splitData.length);
        for (int i = 0; i < outBound; i++) {
            String splitVal = "0.0";
            if (i < splitData.length) splitVal = splitData[i];
            zsetOps.addIfAbsent(rowKey, ots.get(i) + ":" + splitVal, (int) (rowNo + i));
            logger.debug("[[[[[ " + rowKey + ", offsetTime = " + ots.get(i) + ", rowNo = " + (rowNo + i));
            resultSet.add(Double.parseDouble(splitVal));
        }

        logger.debug("[[[[[ " + filterType + " is done... " + part.getSeq().originOf());

        return resultSet;
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

        public ZaeroPartImportTask createPartImportTask() {
            ZaeroPartImportTask task = new ZaeroPartImportTask();
            task.setListOps(this.listOps);
            task.setZsetOps(this.zsetOps);
            return task;
        }
    }
}
