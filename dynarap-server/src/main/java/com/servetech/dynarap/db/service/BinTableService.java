package com.servetech.dynarap.db.service;

import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.google.gson.reflect.TypeToken;
import com.servetech.dynarap.config.ServerConstants;
import com.servetech.dynarap.controller.ApiController;
import com.servetech.dynarap.db.mapper.BinTableMapper;
import com.servetech.dynarap.db.mapper.DirMapper;
import com.servetech.dynarap.db.mapper.ParamMapper;
import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.db.type.String64;
import com.servetech.dynarap.ext.HandledServiceException;
import com.servetech.dynarap.vo.*;
import lombok.Data;
import org.apache.ibatis.annotations.*;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.redis.core.HashOperations;
import org.springframework.data.redis.core.ZSetOperations;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import javax.annotation.Resource;
import java.lang.reflect.Type;
import java.util.*;

import static org.thymeleaf.util.StringUtils.join;

@Service("binTableService")
public class BinTableService {

    @Autowired
    private BinTableMapper binTableMapper;

    @Autowired
    private RawService rawService;

    @Autowired
    private PartService partService;

    @Autowired
    private ParamService paramService;

    @Resource(name = "redisTemplate")
    private HashOperations<String, String, String> hashOps;

    @Resource(name = "redisTemplate")
    private ZSetOperations<String, String> zsetOps;

    public List<BinTableVO> getBinTableList() throws HandledServiceException {
        try {
            return binTableMapper.selectBinTableList();
        } catch(Exception e) {
            throw new HandledServiceException(411, e.getMessage());
        }
    }

    public List<BinTableVO> getBinTableListByKeyword(String keyword) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("keyword", keyword);
            return binTableMapper.selectBinTableListByKeyword(params);
        } catch(Exception e) {
            throw new HandledServiceException(411, e.getMessage());
        }
    }

    public BinTableVO getBinTableBySeq(CryptoField binMetaSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", binMetaSeq);

            BinTableVO binTable = binTableMapper.selectBinTableMetaBySeq(params);
            binTable.setParts(getBinTableDataList(binTable.getSeq(), "part"));
            binTable.setSelectedShortBlocks(getBinTableDataList(binTable.getSeq(), "shortblock"));
            binTable.setDataProps(rawService.getDataPropListToMap("bintable", binTable.getSeq()));
            binTable.setPickUpParams(getBinTableParamList(binTable.getSeq()));
            if (binTable.getPickUpParams() != null && binTable.getPickUpParams().size() > 0) {
                for (BinTableVO.BinParam binParam : binTable.getPickUpParams()) {
                    binParam.setUserParamTable(getBinTableParamData(binTable.getSeq(), binParam.getSeq()));
                }
            }

            return binTable;
        } catch(Exception e) {
            throw new HandledServiceException(411, e.getMessage());
        }
    }

    @Transactional
    public void insertBinTableMeta(BinTableVO binTable) throws HandledServiceException {
        try {
            binTableMapper.insertBinTableMeta(binTable);
        } catch(Exception e) {
            throw new HandledServiceException(411, e.getMessage());
        }
    }

    @Transactional
    public void updateBinTableMeta(BinTableVO binTable) throws HandledServiceException {
        try {
            binTableMapper.updateBinTableMeta(binTable);
        } catch(Exception e) {
            throw new HandledServiceException(411, e.getMessage());
        }
    }

    @Transactional
    public void deleteBinTableMeta(CryptoField binMetaSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", binMetaSeq);
            binTableMapper.deleteBinTableMeta(params);
        } catch(Exception e) {
            throw new HandledServiceException(411, e.getMessage());
        }
    }

    public List<BinTableVO.BinData> getBinTableDataList(CryptoField binMetaSeq, String dataFrom) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("binMetaSeq", binMetaSeq);
            params.put("dataFrom", dataFrom);
            return binTableMapper.selectBinTableDataList(params);
        } catch(Exception e) {
            throw new HandledServiceException(411, e.getMessage());
        }
    }

    @Transactional
    public void insertBinTableData(BinTableVO.BinData binData) throws HandledServiceException {
        try {
            binTableMapper.insertBinTableData(binData);
        } catch(Exception e) {
            throw new HandledServiceException(411, e.getMessage());
        }
    }

    @Transactional
    public void deleteBinTableData(CryptoField binDataSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", binDataSeq);
            binTableMapper.deleteBinTableData(params);
        } catch(Exception e) {
            throw new HandledServiceException(411, e.getMessage());
        }
    }

    @Transactional
    public void deleteBinTableDataByMeta(CryptoField binMetaSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("binMetaSeq", binMetaSeq);
            binTableMapper.deleteBinTableDataByMeta(params);
        } catch(Exception e) {
            throw new HandledServiceException(411, e.getMessage());
        }
    }

    public List<BinTableVO.BinParam> getBinTableParamList(CryptoField binMetaSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("binMetaSeq", binMetaSeq);
            return binTableMapper.selectBinTableParamList(params);
        } catch(Exception e) {
            throw new HandledServiceException(411, e.getMessage());
        }
    }

    @Transactional
    public void insertBinTableParam(BinTableVO.BinParam binParam) throws HandledServiceException {
        try {
            binTableMapper.insertBinTableParam(binParam);
        } catch(Exception e) {
            throw new HandledServiceException(411, e.getMessage());
        }
    }

    @Transactional
    public void deleteBinTableParam(CryptoField binParamSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", binParamSeq);
            binTableMapper.deleteBinTableParam(params);
        } catch(Exception e) {
            throw new HandledServiceException(411, e.getMessage());
        }
    }

    @Transactional
    public void deleteBinTableParamByMeta(CryptoField binMetaSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("binMetaSeq", binMetaSeq);
            binTableMapper.deleteBinTableParamByMeta(params);
        } catch(Exception e) {
            throw new HandledServiceException(411, e.getMessage());
        }
    }

    public List<BinTableVO.BinParam.BinParamData> getBinTableParamData(
            CryptoField binMetaSeq, CryptoField paramSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("binMetaSeq", binMetaSeq);
            params.put("paramSeq", paramSeq);
            List<BinTableVO.BinParam.BinParamData> binParamData = binTableMapper.selectBinTableParamData(params);
            if (binParamData != null && binParamData.size() > 0) {
                for (BinTableVO.BinParam.BinParamData bpd : binParamData) {
                    bpd.setNominal(bpd.getDataNominal());
                    bpd.setMin(bpd.getDataMin());
                    bpd.setMax(bpd.getDataMax());
                }
            }
            return binParamData;
        } catch(Exception e) {
            throw new HandledServiceException(411, e.getMessage());
        }
    }

    @Transactional
    public void insertBinTableParamData(BinTableVO.BinParam.BinParamData binParamData) throws HandledServiceException {
        try {
            binTableMapper.insertBinTableParamData(binParamData);
        } catch(Exception e) {
            throw new HandledServiceException(411, e.getMessage());
        }
    }

    @Transactional
    public void deleteBinTableParamData(CryptoField binParamDataSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", binParamDataSeq);
            binTableMapper.deleteBinTableParamData(params);
        } catch(Exception e) {
            throw new HandledServiceException(411, e.getMessage());
        }
    }

    @Transactional
    public void deleteBinTableParamDataByMeta(CryptoField binMetaSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("binMetaSeq", binMetaSeq);
            binTableMapper.deleteBinTableParamDataByMeta(params);
        } catch(Exception e) {
            throw new HandledServiceException(411, e.getMessage());
        }
    }

    @Transactional
    public void deleteBinTableParamDataByParam(CryptoField binMetaSeq, CryptoField paramSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("binMetaSeq", binMetaSeq);
            params.put("paramSeq", paramSeq);
            binTableMapper.deleteBinTableParamDataByParam(params);
        } catch(Exception e) {
            throw new HandledServiceException(411, e.getMessage());
        }
    }


    @Transactional
    public BinTableVO saveBinTableMeta(CryptoField.NAuth uid, BinTableVO.SaveRequest saveRequest) throws HandledServiceException {
        try {
            if (saveRequest == null)
                throw new HandledServiceException(410, "저장할 데이터가 없습니다.");

            BinTableVO binTable = null;
            if (saveRequest.getBinMetaSeq() == null || saveRequest.getBinMetaSeq().isEmpty()) {
                binTable = new BinTableVO();
                binTable.setMetaName(saveRequest.getMetaName());
                binTable.setCreatedAt(LongDate.now());
                insertBinTableMeta(binTable);

                // part 정보 넣기
                if (saveRequest.getParts() != null && saveRequest.getParts().size() > 0) {
                    for (String partSeq : saveRequest.getParts()) {
                        BinTableVO.BinData binData = new BinTableVO.BinData();
                        binData.setDataFrom("part");
                        binData.setBinMetaSeq(binTable.getSeq());
                        binData.setRefSeq(CryptoField.decode(partSeq, 0L));
                        insertBinTableData(binData);
                    }
                }

                // shortblock 정보 넣기
                if (saveRequest.getSelectedShortBlocks() != null && saveRequest.getSelectedShortBlocks().size() > 0) {
                    for (String shortBlockSeq : saveRequest.getSelectedShortBlocks()) {
                        BinTableVO.BinData binData = new BinTableVO.BinData();
                        binData.setDataFrom("shortblock");
                        binData.setBinMetaSeq(binTable.getSeq());
                        binData.setRefSeq(CryptoField.decode(shortBlockSeq, 0L));
                        insertBinTableData(binData);
                    }
                }

                // dataProps
                if (saveRequest.getDataProps() != null && saveRequest.getDataProps().size() > 0) {
                    Set<String> propKeys = saveRequest.getDataProps().keySet();
                    Iterator<String> iterPropKeys = propKeys.iterator();
                    while (iterPropKeys.hasNext()) {
                        String propKey = iterPropKeys.next();
                        String propValue = saveRequest.getDataProps().get(propKey);
                        DataPropVO dataProp = new DataPropVO();
                        dataProp.setPropName(new String64(propKey));
                        dataProp.setPropValue(new String64(propValue));
                        dataProp.setReferenceType("bintable");
                        dataProp.setReferenceKey(binTable.getSeq());
                        dataProp.setUpdatedAt(LongDate.now());
                        rawService.insertDataProp(dataProp);
                    }
                }

                // param 추가하기
                if (saveRequest.getPickUpParams() != null && saveRequest.getPickUpParams().size() > 0) {
                    for (BinTableVO.BinParam binParam : saveRequest.getPickUpParams()) {
                        binParam.setBinMetaSeq(binTable.getSeq());
                        insertBinTableParam(binParam);

                        if (binParam.getUserParamTable() != null && binParam.getUserParamTable().size() > 0) {
                            for (BinTableVO.BinParam.BinParamData binParamData : binParam.getUserParamTable()) {
                                binParamData.setBinMetaSeq(binTable.getSeq());
                                binParamData.setParamSeq(binParam.getSeq());
                                binParamData.setDataNominal(binParamData.getNominal());
                                binParamData.setDataMin(binParamData.getMin());
                                binParamData.setDataMax(binParamData.getMax());
                                insertBinTableParamData(binParamData);
                            }
                        }
                    }
                }

                binTable = getBinTableBySeq(binTable.getSeq());
            }
            else {
                binTable = getBinTableBySeq(saveRequest.getBinMetaSeq());
                if (binTable == null)
                    throw new HandledServiceException(410, "Bin Table 정보를 찾을 수 없습니다.");

                binTable.setMetaName(saveRequest.getMetaName());
                binTableMapper.updateBinTableMeta(binTable);

                // part
                if (binTable.getParts() == null) binTable.setParts(new ArrayList<>());
                for (BinTableVO.BinData partData : binTable.getParts())
                    partData.setMark(false);

                if (saveRequest.getParts() != null && saveRequest.getParts().size() > 0) {
                    for (String partSeq : saveRequest.getParts()) {
                        CryptoField seq = CryptoField.decode(partSeq, 0L);
                        if (seq == null || seq.isEmpty()) continue;

                        BinTableVO.BinData findPartData = null;
                        for (BinTableVO.BinData partData : binTable.getParts()) {
                            if (partData.getRefSeq().equals(seq)) {
                                findPartData = partData;
                                findPartData.setMark(true);
                                break;
                            }
                        }

                        if (findPartData == null) {
                            BinTableVO.BinData binData = new BinTableVO.BinData();
                            binData.setDataFrom("part");
                            binData.setBinMetaSeq(binTable.getSeq());
                            binData.setRefSeq(seq);
                            insertBinTableData(binData);
                        }
                        else {
                            // update 는 없음.
                        }
                    }
                }

                for (BinTableVO.BinData partData : binTable.getParts()) {
                    if (partData.isMark() == false)
                        deleteBinTableData(partData.getSeq());
                }

                // shortblock
                if (binTable.getSelectedShortBlocks() == null) binTable.setSelectedShortBlocks(new ArrayList<>());
                for (BinTableVO.BinData blockData : binTable.getSelectedShortBlocks())
                    blockData.setMark(false);

                if (saveRequest.getSelectedShortBlocks() != null && saveRequest.getSelectedShortBlocks().size() > 0) {
                    for (String shortBlockSeq : saveRequest.getSelectedShortBlocks()) {
                        CryptoField seq = CryptoField.decode(shortBlockSeq, 0L);
                        if (seq == null || seq.isEmpty()) continue;

                        BinTableVO.BinData findBlockData = null;
                        for (BinTableVO.BinData blockData : binTable.getSelectedShortBlocks()) {
                            if (blockData.getRefSeq().equals(seq)) {
                                findBlockData = blockData;
                                findBlockData.setMark(true);
                                break;
                            }
                        }

                        if (findBlockData == null) {
                            BinTableVO.BinData binData = new BinTableVO.BinData();
                            binData.setDataFrom("shortblock");
                            binData.setBinMetaSeq(binTable.getSeq());
                            binData.setRefSeq(seq);
                            insertBinTableData(binData);
                        }
                        else {
                            // update 는 없음.
                        }
                    }
                }

                for (BinTableVO.BinData blockData : binTable.getSelectedShortBlocks()) {
                    if (blockData.isMark() == false)
                        deleteBinTableData(blockData.getSeq());
                }

                // dataprop
                rawService.deleteDataPropByType("bintable", binTable.getSeq());
                binTable.setDataProps(null);

                if (saveRequest.getDataProps() != null && saveRequest.getDataProps().size() > 0) {
                    Set<String> propKeys = saveRequest.getDataProps().keySet();
                    Iterator<String> iterPropKeys = propKeys.iterator();
                    while (iterPropKeys.hasNext()) {
                        String propKey = iterPropKeys.next();
                        String propValue = saveRequest.getDataProps().get(propKey);
                        DataPropVO dataProp = new DataPropVO();
                        dataProp.setPropName(new String64(propKey));
                        dataProp.setPropValue(new String64(propValue));
                        dataProp.setReferenceType("bintable");
                        dataProp.setReferenceKey(binTable.getSeq());
                        dataProp.setUpdatedAt(LongDate.now());
                        rawService.insertDataProp(dataProp);
                    }
                }

                // pickupparams
                if (binTable.getPickUpParams() == null) binTable.setPickUpParams(new ArrayList<>());
                for (BinTableVO.BinParam binParam : binTable.getPickUpParams())
                    binParam.setMark(false);

                if (saveRequest.getPickUpParams() != null && saveRequest.getPickUpParams().size() > 0) {
                    for (BinTableVO.BinParam binParam : saveRequest.getPickUpParams()) {
                        BinTableVO.BinParam findParam = null;
                        for (BinTableVO.BinParam param : binTable.getPickUpParams()) {
                            if (param.getSeq().equals(binParam.getSeq())) {
                                findParam = param;
                                findParam.setMark(true);
                                break;
                            }
                        }

                        if (findParam == null) {
                            binParam.setBinMetaSeq(binTable.getSeq());
                            insertBinTableParam(binParam);
                        }

                        deleteBinTableParamDataByParam(binTable.getSeq(), binParam.getSeq());

                        if (binParam.getUserParamTable() != null && binParam.getUserParamTable().size() > 0) {
                            for (BinTableVO.BinParam.BinParamData binParamData : binParam.getUserParamTable()) {
                                binParamData.setBinMetaSeq(binTable.getSeq());
                                binParamData.setParamSeq(binParam.getSeq());
                                binParamData.setDataNominal(binParamData.getNominal());
                                binParamData.setDataMin(binParamData.getMin());
                                binParamData.setDataMax(binParamData.getMax());
                                insertBinTableParamData(binParamData);
                            }
                        }
                    }

                    for (BinTableVO.BinParam binParam : binTable.getPickUpParams()) {
                        if (binParam.isMark() == false) {
                            deleteBinTableParamDataByParam(binTable.getSeq(), binParam.getSeq());
                            deleteBinTableParam(binParam.getSeq());
                        }
                    }
                }

                binTable = getBinTableBySeq(binTable.getSeq());
            }

            return binTable;
        } catch(Exception e) {
            throw new HandledServiceException(411, e.getMessage());
        }
    }

    public Map<String, BinTableVO.BinSummary> loadBinTableSummary(CryptoField binMetaSeq, String encParam) throws HandledServiceException {
        String jsonCellList = hashOps.get("BT" + binMetaSeq.originOf(), "CELL");
        List<String> cells = new ArrayList<>();
        if (jsonCellList != null && !jsonCellList.isEmpty()) {
            Type cellType = new TypeToken<List<String>>() {}.getType();
            cells = ServerConstants.GSON.fromJson(jsonCellList, cellType);
        }

        String jsonParamList = hashOps.get("BT" + binMetaSeq.originOf(), "PARAMS");
        List<String> params = new ArrayList<>();
        boolean paramExist = false;
        if (jsonParamList != null && !jsonParamList.isEmpty()) {
            Type paramType = new TypeToken<List<String>>() {}.getType();
            params = ServerConstants.GSON.fromJson(jsonParamList, paramType);

            if (params != null && params.size() > 0) {
                for (String param : params) {
                    if (param.equals(encParam)) {
                        paramExist = true;
                        break;
                    }
                }
            }
        }

        if (paramExist == false) {
            return null;
        }

        Map<String, BinTableVO.BinSummary> mapSummaries = new HashMap<>();

        for (String cell : cells) {
            BinTableVO.BinSummary binSummary = new BinTableVO.BinSummary();
            binSummary.setBinMetaSeq(binMetaSeq);
            binSummary.setSummary(new HashMap<>());

            String[] typeSet = new String[] { "", ".L", ".H", ".B" };

            binSummary.getSummary().put(encParam, new HashMap<>());

            for (String type : typeSet) {
                BinTableVO.BinSummary.SummaryItem summaryItem = new BinTableVO.BinSummary.SummaryItem();

                String min = hashOps.get("BT" + binMetaSeq.originOf() + "." + cell + "." + encParam, "MIN" + type);
                if (min != null && !min.isEmpty())
                    summaryItem.setMin(Double.parseDouble(min));

                String max = hashOps.get("BT" + binMetaSeq.originOf() + "." + cell + "." + encParam, "MAX" + type);
                if (max != null && !max.isEmpty())
                    summaryItem.setMax(Double.parseDouble(max));

                String avg = hashOps.get("BT" + binMetaSeq.originOf() + "." + cell + "." + encParam, "AVG" + type);
                if (avg != null && !avg.isEmpty())
                    summaryItem.setAvg(Double.parseDouble(avg));

                String psd = hashOps.get("BT" + binMetaSeq.originOf() + "." + cell + "." + encParam, "PSD" + type);
                if (psd != null && !psd.isEmpty()) {
                    Type psdType = new TypeToken<List<Double>>() {}.getType();
                    List<Double> psdList = ServerConstants.GSON.fromJson(psd, psdType);
                    summaryItem.setPsd(psdList);
                }

                String freq = hashOps.get("BT" + binMetaSeq.originOf() + "." + cell + "." + encParam, "FREQ" + type);
                if (freq != null && !freq.isEmpty()) {
                    Type freqType = new TypeToken<List<Double>>() {}.getType();
                    List<Double> freqList = ServerConstants.GSON.fromJson(freq, freqType);
                    summaryItem.setFrequency(freqList);
                }

                String rms = hashOps.get("BT" + binMetaSeq.originOf() + "." + cell + "." + encParam, "RMS" + type);
                if (rms != null && !rms.isEmpty()) {
                    Type rmsType = new TypeToken<List<Double>>() {}.getType();
                    List<Double> rmsList = ServerConstants.GSON.fromJson(rms, rmsType);
                    summaryItem.setRms(rmsList);
                }

                String avg_rms = hashOps.get("BT" + binMetaSeq.originOf() + "." + cell + "." + encParam, "AVG.RMS" + type);
                if (avg_rms != null && !avg_rms.isEmpty())
                    summaryItem.setAvg_rms(Double.parseDouble(avg_rms));

                String n0 = hashOps.get("BT" + binMetaSeq.originOf() + "." + cell + "." + encParam, "N0" + type);
                if (n0 != null && !n0.isEmpty()) {
                    Type n0Type = new TypeToken<List<Double>>() {}.getType();
                    List<Double> n0List = ServerConstants.GSON.fromJson(n0, n0Type);
                    summaryItem.setN0(n0List);
                }

                String zeta = hashOps.get("BT" + binMetaSeq.originOf() + "." + cell + "." + encParam, "ZETA" + type);
                if (zeta != null && !zeta.isEmpty()) {
                    Type zetaType = new TypeToken<List<Double>>() {}.getType();
                    List<Double> zetaList = ServerConstants.GSON.fromJson(zeta, zetaType);
                    summaryItem.setZeta(zetaList);
                }

                String burstFactor = hashOps.get("BT" + binMetaSeq.originOf() + "." + cell + "." + encParam, "BF" + type);
                if (burstFactor != null && !burstFactor.isEmpty())
                    summaryItem.setBurstFactor(Double.parseDouble(burstFactor));

                String rmsToPeak = hashOps.get("BT" + binMetaSeq.originOf() + "." + cell + "." + encParam, "RTP" + type);
                if (rmsToPeak != null && !rmsToPeak.isEmpty()) {
                    Type rmsToPeakType = new TypeToken<List<Double>>() {}.getType();
                    List<Double> rmsToPeakList = ServerConstants.GSON.fromJson(rmsToPeak, rmsToPeakType);
                    summaryItem.setRmsToPeak(rmsToPeakList);
                }

                String maxRmsToPeak = hashOps.get("BT" + binMetaSeq.originOf() + "." + cell + "." + encParam, "MRTP" + type);
                if (maxRmsToPeak != null && !maxRmsToPeak.isEmpty())
                    summaryItem.setMaxRmsToPeak(Double.parseDouble(maxRmsToPeak));

                String maxLoadAccel = hashOps.get("BT" + binMetaSeq.originOf() + "." + cell + "." + encParam, "MLA" + type);
                if (maxLoadAccel != null && !maxLoadAccel.isEmpty())
                    summaryItem.setMaxLoadAccel(Double.parseDouble(maxLoadAccel));

                binSummary.getSummary().get(encParam).put(
                        type.equals(".L") ? "lpf" : type.equals(".H") ? "hpf" : type.equals(".B") ? "bpf" : "normal",
                        summaryItem);
            }

            mapSummaries.put(cell, binSummary);
        }

        return mapSummaries;
    }

    public BinTableVO.BinSummary calculateBinSummary(CryptoField.NAuth uid, BinTableVO.CalculateRequest request) throws HandledServiceException {
        BinTableVO.BinSummary binSummary = null;

        BinTableVO binTable = getBinTableBySeq(request.getBinMetaSeq());

        binSummary = new BinTableVO.BinSummary();
        binSummary.setBinMetaSeq(binTable.getSeq());
        binSummary.setFactorIndexes(request.getFactorIndexes());

        // shortblock data loading
        Map<String, List<Double>> minMaxAvg = new LinkedHashMap<>();
        Map<String, List<Double>> lpfMinMaxAvg = new LinkedHashMap<>();
        Map<String, List<Double>> hpfMinMaxAvg = new LinkedHashMap<>();
        Map<String, List<Double>> bpfMinMaxAvg = new LinkedHashMap<>();

        Map<String, List<String>> psdMap = new LinkedHashMap<>();
        Map<String, List<String>> lpfPsdMap = new LinkedHashMap<>();
        Map<String, List<String>> hpfPsdMap = new LinkedHashMap<>();
        Map<String, List<String>> bpfPsdMap = new LinkedHashMap<>();
        Map<String, String> freqMap = new LinkedHashMap<>();
        Map<String, String> lpfFreqMap = new LinkedHashMap<>();
        Map<String, String> hpfFreqMap = new LinkedHashMap<>();
        Map<String, String> bpfFreqMap = new LinkedHashMap<>();

        Map<String, List<Double>> avgPsdMap = new LinkedHashMap<>();
        Map<String, List<Double>> avgLpfPsdMap = new LinkedHashMap<>();
        Map<String, List<Double>> avgHpfPsdMap = new LinkedHashMap<>();
        Map<String, List<Double>> avgBpfPsdMap = new LinkedHashMap<>();

        Map<String, Double> avgRmsMap = new LinkedHashMap<>();
        Map<String, List<Double>> rmsMap = new LinkedHashMap<>();
        Map<String, Double> avgN0Map = new LinkedHashMap<>();
        Map<String, List<Double>> n0Map = new LinkedHashMap<>();
        Map<String, List<Double>> zetaMap = new LinkedHashMap<>();
        Map<String, Double> burstFactorMap = new LinkedHashMap<>();
        Map<String, List<String>> zarrayMap = new LinkedHashMap<>();
        Map<String, List<Double>> rmsToPeakMap = new LinkedHashMap<>();
        Map<String, Double> maxRmsToPeakMap = new LinkedHashMap<>();
        Map<String, Double> maxLoadAccel = new LinkedHashMap<>();

        Map<String, Double> lpfAvgRmsMap = new LinkedHashMap<>();
        Map<String, List<Double>> lpfRmsMap = new LinkedHashMap<>();
        Map<String, Double> lpfAvgN0Map = new LinkedHashMap<>();
        Map<String, List<Double>> lpfN0Map = new LinkedHashMap<>();
        Map<String, List<Double>> lpfZetaMap = new LinkedHashMap<>();
        Map<String, Double> lpfBurstFactorMap = new LinkedHashMap<>();
        Map<String, List<String>> lpfZarrayMap = new LinkedHashMap<>();
        Map<String, List<Double>> lpfRmsToPeakMap = new LinkedHashMap<>();
        Map<String, Double> lpfMaxRmsToPeakMap = new LinkedHashMap<>();
        Map<String, Double> lpfMaxLoadAccel = new LinkedHashMap<>();

        Map<String, Double> hpfAvgRmsMap = new LinkedHashMap<>();
        Map<String, List<Double>> hpfRmsMap = new LinkedHashMap<>();
        Map<String, Double> hpfAvgN0Map = new LinkedHashMap<>();
        Map<String, List<Double>> hpfN0Map = new LinkedHashMap<>();
        Map<String, List<Double>> hpfZetaMap = new LinkedHashMap<>();
        Map<String, Double> hpfBurstFactorMap = new LinkedHashMap<>();
        Map<String, List<String>> hpfZarrayMap = new LinkedHashMap<>();
        Map<String, List<Double>> hpfRmsToPeakMap = new LinkedHashMap<>();
        Map<String, Double> hpfMaxRmsToPeakMap = new LinkedHashMap<>();
        Map<String, Double> hpfMaxLoadAccel = new LinkedHashMap<>();

        Map<String, Double> bpfAvgRmsMap = new LinkedHashMap<>();
        Map<String, List<Double>> bpfRmsMap = new LinkedHashMap<>();
        Map<String, Double> bpfAvgN0Map = new LinkedHashMap<>();
        Map<String, List<Double>> bpfN0Map = new LinkedHashMap<>();
        Map<String, List<Double>> bpfZetaMap = new LinkedHashMap<>();
        Map<String, Double> bpfBurstFactorMap = new LinkedHashMap<>();
        Map<String, List<String>> bpfZarrayMap = new LinkedHashMap<>();
        Map<String, List<Double>> bpfRmsToPeakMap = new LinkedHashMap<>();
        Map<String, Double> bpfMaxRmsToPeakMap = new LinkedHashMap<>();
        Map<String, Double> bpfMaxLoadAccel = new LinkedHashMap<>();

        Map<String, List<Double>> valueMap = new LinkedHashMap<>();
        Map<String, List<Double>> lpfValueMap = new LinkedHashMap<>();
        Map<String, List<Double>> hpfValueMap = new LinkedHashMap<>();
        Map<String, List<Double>> bpfValueMap = new LinkedHashMap<>();

        for (String encBlockSeq : request.getShortBlocks()) {
            CryptoField blockSeq = CryptoField.decode(encBlockSeq, 0L);
            ShortBlockVO shortBlock = partService.getShortBlockBySeq(blockSeq);
            PartVO part = partService.getPartBySeq(shortBlock.getPartSeq());
            RawVO.Upload upload = rawService.getUploadBySeq(part.getUploadSeq());
            if (upload != null) part.setDataType(upload.getDataType());

            List<ShortBlockVO.Param> sparams = partService.getShortBlockParamList(shortBlock.getBlockMetaSeq());
            for (ShortBlockVO.Param sparam : sparams) {
                /*
                boolean isBinParam = false;
                for (BinTableVO.BinParam binParam : binTable.getPickUpParams()) {
                    if (sparam.getParamSeq().equals(binParam.getParamSeq())
                            && sparam.getParamPack().equals(binParam.getParamPack())) {
                        isBinParam = true;
                        break;
                    }
                }
                if (isBinParam == false) continue;
                */

                sparam.setParamInfo(paramService.getParamBySeq(sparam.getParamSeq()));

                ShortBlockVO.ParamData paramData = partService.getShortBlockParamData(
                        shortBlock.getBlockMetaSeq(), shortBlock.getSeq(), sparam.getUnionParamSeq());

                List<String> psd = psdMap.get(sparam.getSeq().valueOf());
                if (psd == null) {
                    psd = new ArrayList<>();
                    psdMap.put(sparam.getSeq().valueOf(), psd);
                    avgPsdMap.put(sparam.getSeq().valueOf(), new ArrayList<>());
                }
                psd.add(paramData.getPsd());

                List<String> lpfPsd = lpfPsdMap.get(sparam.getSeq().valueOf());
                if (lpfPsd == null) {
                    lpfPsd = new ArrayList<>();
                    lpfPsdMap.put(sparam.getSeq().valueOf(), lpfPsd);
                    avgLpfPsdMap.put(sparam.getSeq().valueOf(), new ArrayList<>());
                }
                lpfPsd.add(paramData.getLpfPsd());

                List<String> hpfPsd = hpfPsdMap.get(sparam.getSeq().valueOf());
                if (hpfPsd == null) {
                    hpfPsd = new ArrayList<>();
                    hpfPsdMap.put(sparam.getSeq().valueOf(), hpfPsd);
                    avgHpfPsdMap.put(sparam.getSeq().valueOf(), new ArrayList<>());
                }
                hpfPsd.add(paramData.getHpfPsd());

                List<String> bpfPsd = bpfPsdMap.get(sparam.getSeq().valueOf());
                if (bpfPsd == null) {
                    bpfPsd = new ArrayList<>();
                    bpfPsdMap.put(sparam.getSeq().valueOf(), bpfPsd);
                    avgBpfPsdMap.put(sparam.getSeq().valueOf(), new ArrayList<>());
                }
                bpfPsd.add(paramData.getBpfPsd());

                String freq = freqMap.get(sparam.getSeq().valueOf());
                if (freq == null)
                    freqMap.put(sparam.getSeq().valueOf(), paramData.getFrequency());

                String lpfFreq = lpfFreqMap.get(sparam.getSeq().valueOf());
                if (lpfFreq == null)
                    lpfFreqMap.put(sparam.getSeq().valueOf(), paramData.getLpfFrequency());

                String hpfFreq = hpfFreqMap.get(sparam.getSeq().valueOf());
                if (hpfFreq == null)
                    hpfFreqMap.put(sparam.getSeq().valueOf(), paramData.getHpfFrequency());

                String bpfFreq = bpfFreqMap.get(sparam.getSeq().valueOf());
                if (bpfFreq == null)
                    bpfFreqMap.put(sparam.getSeq().valueOf(), paramData.getBpfFrequency());

                List<Double> rms = rmsMap.get(sparam.getSeq().valueOf());
                if (rms == null) {
                    rms = new ArrayList<>();
                    rmsMap.put(sparam.getSeq().valueOf(), rms);
                }
                rms.add(paramData.getRms());

                List<Double> lpfRms = lpfRmsMap.get(sparam.getSeq().valueOf());
                if (lpfRms == null) {
                    lpfRms = new ArrayList<>();
                    lpfRmsMap.put(sparam.getSeq().valueOf(), lpfRms);
                }
                lpfRms.add(paramData.getLpfRms());

                List<Double> hpfRms = hpfRmsMap.get(sparam.getSeq().valueOf());
                if (hpfRms == null) {
                    hpfRms = new ArrayList<>();
                    hpfRmsMap.put(sparam.getSeq().valueOf(), hpfRms);
                }
                hpfRms.add(paramData.getHpfRms());

                List<Double> bpfRms = bpfRmsMap.get(sparam.getSeq().valueOf());
                if (bpfRms == null) {
                    bpfRms = new ArrayList<>();
                    bpfRmsMap.put(sparam.getSeq().valueOf(), bpfRms);
                }
                bpfRms.add(paramData.getBpfRms());

                List<Double> n0 = n0Map.get(sparam.getSeq().valueOf());
                if (n0 == null) {
                    n0 = new ArrayList<>();
                    n0Map.put(sparam.getSeq().valueOf(), n0);
                }
                n0.add(paramData.getN0());

                List<Double> lpfN0 = lpfN0Map.get(sparam.getSeq().valueOf());
                if (lpfN0 == null) {
                    lpfN0 = new ArrayList<>();
                    lpfN0Map.put(sparam.getSeq().valueOf(), lpfN0);
                }
                lpfN0.add(paramData.getLpfN0());

                List<Double> hpfN0 = hpfN0Map.get(sparam.getSeq().valueOf());
                if (hpfN0 == null) {
                    hpfN0 = new ArrayList<>();
                    hpfN0Map.put(sparam.getSeq().valueOf(), hpfN0);
                }
                hpfN0.add(paramData.getHpfN0());

                List<Double> bpfN0 = bpfN0Map.get(sparam.getSeq().valueOf());
                if (bpfN0 == null) {
                    bpfN0 = new ArrayList<>();
                    bpfN0Map.put(sparam.getSeq().valueOf(), bpfN0);
                }
                bpfN0.add(paramData.getBpfN0());

                List<String> zarray = zarrayMap.get(sparam.getSeq().valueOf());
                if (zarray == null) {
                    zarray = new ArrayList<>();
                    zarrayMap.put(sparam.getSeq().valueOf(), zarray);
                }
                zarray.add(paramData.getZarray());

                List<String> lpfZarray = lpfZarrayMap.get(sparam.getSeq().valueOf());
                if (lpfZarray == null) {
                    lpfZarray = new ArrayList<>();
                    lpfZarrayMap.put(sparam.getSeq().valueOf(), lpfZarray);
                }
                lpfZarray.add(paramData.getLpfZarray());

                List<String> hpfZarray = hpfZarrayMap.get(sparam.getSeq().valueOf());
                if (hpfZarray == null) {
                    hpfZarray = new ArrayList<>();
                    hpfZarrayMap.put(sparam.getSeq().valueOf(), hpfZarray);
                }
                hpfZarray.add(paramData.getHpfZarray());

                List<String> bpfZarray = bpfZarrayMap.get(sparam.getSeq().valueOf());
                if (bpfZarray == null) {
                    bpfZarray = new ArrayList<>();
                    bpfZarrayMap.put(sparam.getSeq().valueOf(), bpfZarray);
                }
                bpfZarray.add(paramData.getBpfZarray());

                // shortblock data loading.
                String julianFrom = "";
                Set<String> listSet = zsetOps.rangeByScore("S" + shortBlock.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);
                if (listSet != null && listSet.size() > 0)
                    julianFrom = listSet.iterator().next();

                String julianTo = "";
                listSet = zsetOps.reverseRangeByScore("S" + shortBlock.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);
                if (listSet != null && listSet.size() > 0)
                    julianTo = listSet.iterator().next();

                listSet = zsetOps.rangeByScore("S" + shortBlock.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);
                String julianStart = listSet.iterator().next();
                Long startRowAt = zsetOps.score("S" + shortBlock.getSeq().originOf() + ".R", julianStart).longValue();

                Long rankFrom = zsetOps.rank("S" + shortBlock.getSeq().originOf() + ".R", julianFrom);
                if (rankFrom == null) {
                    julianFrom = zsetOps.rangeByScore("S" + shortBlock.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE).iterator().next();
                    rankFrom = zsetOps.rank("S" + shortBlock.getSeq().originOf() + ".R", julianFrom);
                }
                Long rankTo = zsetOps.rank("S" + shortBlock.getSeq().originOf() + ".R", julianTo);
                if (rankTo == null) {
                    julianTo = zsetOps.reverseRangeByScore("S" + shortBlock.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE).iterator().next();
                    rankTo = zsetOps.rank("S" + shortBlock.getSeq().originOf() + ".R", julianTo);
                }

                List<Double> rowList = new ArrayList<>();
                listSet = zsetOps.rangeByScore(
                        "S" + shortBlock.getSeq().originOf() + ".N" + sparam.getUnionParamSeq(), startRowAt + rankFrom, startRowAt + rankTo);

                Iterator<String> iterListSet = listSet.iterator();
                while (iterListSet.hasNext()) {
                    String rowVal = iterListSet.next();
                    Double dblVal = Double.parseDouble(rowVal.substring(rowVal.lastIndexOf(":") + 1));
                    rowList.add(dblVal);
                }
                List<Double> valueList = valueMap.get(sparam.getSeq().valueOf());
                if (valueList == null) {
                    valueList = new ArrayList<>();
                    valueMap.put(sparam.getSeq().valueOf(), valueList);
                }
                valueList.addAll(rowList);
                valueMap.put(sparam.getSeq().valueOf(), valueList);

                rowList = new ArrayList<>();
                listSet = zsetOps.rangeByScore(
                        "S" + shortBlock.getSeq().originOf() + ".L" + sparam.getUnionParamSeq(), startRowAt + rankFrom, startRowAt + rankTo);

                iterListSet = listSet.iterator();
                while (iterListSet.hasNext()) {
                    String rowVal = iterListSet.next();
                    Double dblVal = Double.parseDouble(rowVal.substring(rowVal.lastIndexOf(":") + 1));
                    rowList.add(dblVal);
                }
                valueList = lpfValueMap.get(sparam.getSeq().valueOf());
                if (valueList == null) {
                    valueList = new ArrayList<>();
                    lpfValueMap.put(sparam.getSeq().valueOf(), valueList);
                }
                valueList.addAll(rowList);
                lpfValueMap.put(sparam.getSeq().valueOf(), valueList);

                rowList = new ArrayList<>();
                listSet = zsetOps.rangeByScore(
                        "S" + shortBlock.getSeq().originOf() + ".H" + sparam.getUnionParamSeq(), startRowAt + rankFrom, startRowAt + rankTo);

                iterListSet = listSet.iterator();
                while (iterListSet.hasNext()) {
                    String rowVal = iterListSet.next();
                    Double dblVal = Double.parseDouble(rowVal.substring(rowVal.lastIndexOf(":") + 1));
                    rowList.add(dblVal);
                }
                valueList = hpfValueMap.get(sparam.getSeq().valueOf());
                if (valueList == null) {
                    valueList = new ArrayList<>();
                    hpfValueMap.put(sparam.getSeq().valueOf(), valueList);
                }
                valueList.addAll(rowList);
                hpfValueMap.put(sparam.getSeq().valueOf(), valueList);

                rowList = new ArrayList<>();
                listSet = zsetOps.rangeByScore(
                        "S" + shortBlock.getSeq().originOf() + ".B" + sparam.getUnionParamSeq(), startRowAt + rankFrom, startRowAt + rankTo);

                iterListSet = listSet.iterator();
                while (iterListSet.hasNext()) {
                    String rowVal = iterListSet.next();
                    Double dblVal = Double.parseDouble(rowVal.substring(rowVal.lastIndexOf(":") + 1));
                    rowList.add(dblVal);
                }
                valueList = bpfValueMap.get(sparam.getSeq().valueOf());
                if (valueList == null) {
                    valueList = new ArrayList<>();
                    bpfValueMap.put(sparam.getSeq().valueOf(), valueList);
                }
                valueList.addAll(rowList);
                bpfValueMap.put(sparam.getSeq().valueOf(), valueList);

            }
        }

        binSummary.setSummary(new HashMap<>());

        // psd string array -> split -> index sum -> avg
        Set<String> keySet = psdMap.keySet();
        Iterator<String> iterKeySet = keySet.iterator();
        while (iterKeySet.hasNext()) {
            String encParam = iterKeySet.next();

            binSummary.getSummary().put(encParam, new HashMap<>());

            // psd
            List<Map<String, List<String>>> targetMap = Arrays.asList(psdMap, lpfPsdMap, hpfPsdMap, bpfPsdMap);
            List<Map<String, List<Double>>> targetDestMap = Arrays.asList(avgPsdMap, avgLpfPsdMap, avgHpfPsdMap, avgBpfPsdMap);

            for (int j = 0; j < targetMap.size(); j++) {
                List<String> psdList = targetMap.get(j).get(encParam);
                List<Double> psdSumList = new ArrayList<>();
                List<Double> avgPsdList = new ArrayList<>();

                for (String p : psdList) {
                    Type psdType = new TypeToken<List<Double>>() {
                    }.getType();

                    List<Double> blockPsd = ServerConstants.GSON.fromJson(p, psdType);
                    if (psdSumList.size() == 0) {
                        for (int i = 0; i < blockPsd.size(); i++) psdSumList.add(0.0);
                    }

                    for (int i = 0; i < blockPsd.size(); i++) {
                        if (i < psdSumList.size()) {
                            psdSumList.set(i, psdSumList.get(i) + blockPsd.get(i));
                        } else {
                            psdSumList.add(blockPsd.get(i));
                        }
                    }
                }

                for (int i = 0; i < psdSumList.size(); i++) {
                    avgPsdList.add(psdSumList.get(i) / psdList.size());
                }

                targetDestMap.get(j).put(encParam, avgPsdList);
            }

            // rms
            List<Map<String, List<Double>>> rmsTargetMap = Arrays.asList(rmsMap, lpfRmsMap, hpfRmsMap, bpfRmsMap);
            List<Map<String, Double>> rmsTargetDestMap = Arrays.asList(avgRmsMap, lpfAvgRmsMap, hpfAvgRmsMap, bpfAvgRmsMap);
            for (int j = 0; j < rmsTargetMap.size(); j++) {
                Double sumRms = 0.0;
                List<Double> rmsList = rmsTargetMap.get(j).get(encParam);
                for (int i = 0; i < rmsList.size(); i++)
                    sumRms += rmsList.get(i);
                rmsTargetDestMap.get(j).put(encParam, sumRms / rmsList.size());
            }

            // n0
            List<Map<String, List<Double>>> n0TargetMap = Arrays.asList(n0Map, lpfN0Map, hpfN0Map, bpfN0Map);
            List<Map<String, Double>> n0TargetDestMap = Arrays.asList(avgN0Map, lpfAvgN0Map, hpfAvgN0Map, bpfAvgN0Map);
            for (int j = 0; j < n0TargetMap.size(); j++) {
                Double sumN0 = 0.0;
                List<Double> n0List = n0TargetMap.get(j).get(encParam);
                for (int i = 0; i < n0List.size(); i++)
                    sumN0 += n0List.get(i);
                n0TargetDestMap.get(j).put(encParam, sumN0 / n0List.size());
            }

            // zeta
            List<Map<String, List<Double>>> zetaRmsMap = Arrays.asList(rmsMap, lpfRmsMap, hpfRmsMap, bpfRmsMap);
            List<Map<String, Double>> zetaAvgRmsMap = Arrays.asList(avgRmsMap, lpfAvgRmsMap, hpfAvgRmsMap, bpfAvgRmsMap);
            List<Map<String, List<Double>>> zetaDestMap = Arrays.asList(zetaMap, lpfZetaMap, hpfZetaMap, bpfZetaMap);
            for (int j = 0; j < zetaRmsMap.size(); j++) {
                List<Double> zetaRms = zetaRmsMap.get(j).get(encParam);
                Double zetaAvgRms = zetaAvgRmsMap.get(j).get(encParam);
                List<Double> zetaList = new ArrayList<>();
                for (int i = 0; i < zetaRms.size(); i++) {
                    if (zetaAvgRms != 0) {
                        Double zeta = zetaRms.get(i) / zetaAvgRms;
                        zetaList.add(zeta);
                    }
                    else {
                        zetaList.add(0.0);
                    }
                }
                zetaDestMap.get(j).put(encParam, zetaList);
            }

            // burstfactor
            List<Map<String, List<Double>>> zetaTargetMap = Arrays.asList(zetaMap, lpfZetaMap, hpfZetaMap, bpfZetaMap);
            List<Map<String, Double>> burstFactorDestMap = Arrays.asList(burstFactorMap, lpfBurstFactorMap, hpfBurstFactorMap, bpfBurstFactorMap);
            for (int j = 0; j < zetaTargetMap.size(); j++) {
                List<Double> zetaList = zetaTargetMap.get(j).get(encParam);
                Double burstFactor = Double.MIN_VALUE;
                for (int i = 0; i < zetaList.size(); i++) {
                    burstFactor = Math.max(zetaList.get(i), burstFactor);
                }
                burstFactorDestMap.get(j).put(encParam, burstFactor);
            }

            // zarray
            List<Map<String, List<String>>> zarrayTargetMap = Arrays.asList(zarrayMap, lpfZarrayMap, hpfZarrayMap, bpfZarrayMap);
            List<Map<String, List<Double>>> rmsToPeakDestMap = Arrays.asList(rmsToPeakMap, lpfRmsToPeakMap, hpfRmsToPeakMap, bpfRmsToPeakMap);
            List<Map<String, Double>> maxRmsToPeakDestMap = Arrays.asList(maxRmsToPeakMap, lpfMaxRmsToPeakMap, hpfMaxRmsToPeakMap, bpfMaxRmsToPeakMap);
            for (int j = 0; j < zarrayTargetMap.size(); j++) {
                List<String> zarrayList = zarrayTargetMap.get(j).get(encParam);
                List<Double> rmsToPeak = new ArrayList<>();
                for (String z : zarrayList) {
                    Type zarrayType = new TypeToken<List<Double>>() {
                    }.getType();
                    List<Double> zarray = ServerConstants.GSON.fromJson(z, zarrayType);
                    rmsToPeak.addAll(zarray);
                }
                List<Double> targetList = rmsToPeakDestMap.get(j).get(encParam);
                if (targetList == null) {
                    targetList = new ArrayList<>();
                    rmsToPeakDestMap.get(j).put(encParam, targetList);
                }
                targetList.addAll(rmsToPeak);
                Collections.sort(targetList);
                Collections.reverse(targetList); // desc order
                if (targetList != null && targetList.size() > 0)
                    maxRmsToPeakDestMap.get(j).put(encParam, Collections.max(targetList));
                else
                    maxRmsToPeakDestMap.get(j).put(encParam, 0.0);
                rmsToPeakDestMap.get(j).put(encParam, targetList);
            }

            // maxloadaccel
            List<Map<String, Double>> maxLoadAccelDestMap = Arrays.asList(maxLoadAccel, lpfMaxLoadAccel, hpfMaxLoadAccel, bpfMaxLoadAccel);
            for (int j = 0; j < maxLoadAccelDestMap.size(); j++) {
                Double mla = rmsTargetDestMap.get(j).get(encParam) * 4 * burstFactorDestMap.get(j).get(encParam);
                maxLoadAccelDestMap.get(j).put(encParam, mla);
            }

            // value processing (min, max, avg)
            List<Map<String, List<Double>>> mmaTargetMap = Arrays.asList(valueMap, lpfValueMap, hpfValueMap, bpfValueMap);
            List<Map<String, List<Double>>> mmaTargetDestMap = Arrays.asList(minMaxAvg, lpfMinMaxAvg, hpfMinMaxAvg, bpfMinMaxAvg);
            for (int j = 0; j < mmaTargetMap.size(); j++) {
                List<Double> values = mmaTargetMap.get(j).get(encParam);
                Double min = Double.MAX_VALUE;
                Double max = Double.MIN_VALUE;
                Double sum = 0.0;
                for (Double v : values) {
                    min = Math.min(min, v);
                    max = Math.max(max, v);
                    sum += v;
                }
                List<Double> result = new ArrayList<>();
                result.add(min);
                result.add(max);
                result.add(sum / values.size());
                mmaTargetDestMap.get(j).put(encParam, result);
            }

            // bintable save to redis.
            // BT<seq>.<fa_path> / <type>.<filter> / values

            String fa_path = join(request.getFactorIndexes(), ",");

            String jsonCellList = hashOps.get("BT" + binTable.getSeq().originOf(), "CELL");
            List<String> cells = new ArrayList<>();
            if (jsonCellList != null && !jsonCellList.isEmpty()) {
                Type cellType = new TypeToken<List<String>>() {}.getType();
                cells = ServerConstants.GSON.fromJson(jsonCellList, cellType);
            }
            if (!cells.contains(fa_path)) cells.add(fa_path);

            hashOps.put("BT" + binTable.getSeq().originOf(), "CELL", ServerConstants.GSON.toJson(cells));

            // min, max, avg
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "MIN", Double.toString(minMaxAvg.get(encParam).get(0)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "MIN.L", Double.toString(lpfMinMaxAvg.get(encParam).get(0)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "MIN.H", Double.toString(hpfMinMaxAvg.get(encParam).get(0)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "MIN.B", Double.toString(bpfMinMaxAvg.get(encParam).get(0)));

            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "MAX", Double.toString(minMaxAvg.get(encParam).get(1)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "MAX.L", Double.toString(lpfMinMaxAvg.get(encParam).get(1)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "MAX.H", Double.toString(hpfMinMaxAvg.get(encParam).get(1)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "MAX.B", Double.toString(bpfMinMaxAvg.get(encParam).get(1)));

            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "AVG", Double.toString(minMaxAvg.get(encParam).get(2)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "AVG.L", Double.toString(lpfMinMaxAvg.get(encParam).get(2)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "AVG.H", Double.toString(hpfMinMaxAvg.get(encParam).get(2)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "AVG.B", Double.toString(bpfMinMaxAvg.get(encParam).get(2)));

            // psd
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "PSD", ServerConstants.GSON.toJson(avgPsdMap.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "PSD.L", ServerConstants.GSON.toJson(avgLpfPsdMap.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "PSD.H", ServerConstants.GSON.toJson(avgHpfPsdMap.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "PSD.B", ServerConstants.GSON.toJson(avgBpfPsdMap.get(encParam)));

            // frequency
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "FREQ", freqMap.get(encParam));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "FREQ.L", lpfFreqMap.get(encParam));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "FREQ.H", hpfFreqMap.get(encParam));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "FREQ.B", bpfFreqMap.get(encParam));

            // rms
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "RMS", ServerConstants.GSON.toJson(rmsMap.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "RMS.L", ServerConstants.GSON.toJson(lpfRmsMap.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "RMS.H", ServerConstants.GSON.toJson(hpfRmsMap.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "RMS.B", ServerConstants.GSON.toJson(bpfRmsMap.get(encParam)));

            // avg_rms
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "AVG.RMS", Double.toString(avgRmsMap.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "AVG.RMS.L", Double.toString(lpfAvgRmsMap.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "AVG.RMS.H", Double.toString(hpfAvgRmsMap.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "AVG.RMS.B", Double.toString(bpfAvgRmsMap.get(encParam)));

            // n0
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "N0", ServerConstants.GSON.toJson(n0Map.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "N0.L", ServerConstants.GSON.toJson(lpfN0Map.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "N0.H", ServerConstants.GSON.toJson(hpfN0Map.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "N0.B", ServerConstants.GSON.toJson(bpfN0Map.get(encParam)));

            // avg_n0
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "AVG.N0", Double.toString(avgN0Map.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "AVG.N0.L", Double.toString(lpfAvgN0Map.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "AVG.N0.H", Double.toString(hpfAvgN0Map.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "AVG.N0.B", Double.toString(bpfAvgN0Map.get(encParam)));

            // zeta
            if (zetaMap.get(encParam) == null) {
                List<Double> rmsList = rmsMap.get(encParam);
                List<Double> zetaList = new ArrayList<>();
                for (Double d : rmsList) zetaList.add(0.0);
                zetaMap.put(encParam, zetaList);
            }
            if (lpfZetaMap.get(encParam) == null) {
                List<Double> rmsList = lpfRmsMap.get(encParam);
                List<Double> zetaList = new ArrayList<>();
                for (Double d : rmsList) zetaList.add(0.0);
                lpfZetaMap.put(encParam, zetaList);
            }
            if (hpfZetaMap.get(encParam) == null) {
                List<Double> rmsList = hpfRmsMap.get(encParam);
                List<Double> zetaList = new ArrayList<>();
                for (Double d : rmsList) zetaList.add(0.0);
                hpfZetaMap.put(encParam, zetaList);
            }
            if (bpfZetaMap.get(encParam) == null) {
                List<Double> rmsList = bpfRmsMap.get(encParam);
                List<Double> zetaList = new ArrayList<>();
                for (Double d : rmsList) zetaList.add(0.0);
                bpfZetaMap.put(encParam, zetaList);
            }
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "ZETA", ServerConstants.GSON.toJson(zetaMap.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "ZETA.L", ServerConstants.GSON.toJson(lpfZetaMap.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "ZETA.H", ServerConstants.GSON.toJson(hpfZetaMap.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "ZETA.B", ServerConstants.GSON.toJson(bpfZetaMap.get(encParam)));

            // burstFactor
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "BF", Double.toString(burstFactorMap.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "BF.L", Double.toString(lpfBurstFactorMap.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "BF.H", Double.toString(hpfBurstFactorMap.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "BF.B", Double.toString(bpfBurstFactorMap.get(encParam)));

            // rms-to-peak
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "RTP", ServerConstants.GSON.toJson(rmsToPeakMap.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "RTP.L", ServerConstants.GSON.toJson(lpfRmsToPeakMap.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "RTP.H", ServerConstants.GSON.toJson(hpfRmsToPeakMap.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "RTP.B", ServerConstants.GSON.toJson(bpfRmsToPeakMap.get(encParam)));

            // max rms-to-peak
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "MRTP", Double.toString(maxRmsToPeakMap.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "MRTP.L", Double.toString(lpfMaxRmsToPeakMap.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "MRTP.H", Double.toString(hpfMaxRmsToPeakMap.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "MRTP.B", Double.toString(bpfMaxRmsToPeakMap.get(encParam)));

            // max load acceleration
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "MLA", Double.toString(maxLoadAccel.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "MLA.L", Double.toString(lpfMaxLoadAccel.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "MLA.H", Double.toString(hpfMaxLoadAccel.get(encParam)));
            hashOps.put("BT" + binTable.getSeq().originOf() + "." + fa_path + "." + encParam, "MLA.B", Double.toString(bpfMaxLoadAccel.get(encParam)));

            BinTableVO.BinSummary.SummaryItem summaryItem = new BinTableVO.BinSummary.SummaryItem();
            summaryItem.setMin(minMaxAvg.get(encParam).get(0));
            summaryItem.setMax(minMaxAvg.get(encParam).get(1));
            summaryItem.setAvg(minMaxAvg.get(encParam).get(2));
            summaryItem.setPsd(avgPsdMap.get(encParam));

            Type freqType = new TypeToken<List<Double>>() {}.getType();
            String freqData = freqMap.get(encParam);
            if (freqData != null) {
                summaryItem.setFrequency(ServerConstants.GSON.fromJson(freqData, freqType));
            }
            else {
                summaryItem.setFrequency(null);
            }

            summaryItem.setRms(rmsMap.get(encParam));
            summaryItem.setAvg_rms(avgRmsMap.get(encParam));
            summaryItem.setN0(n0Map.get(encParam));
            summaryItem.setAvg_n0(avgN0Map.get(encParam));
            summaryItem.setZeta(zetaMap.get(encParam));
            summaryItem.setBurstFactor(burstFactorMap.get(encParam));
            summaryItem.setRmsToPeak(rmsToPeakMap.get(encParam));
            summaryItem.setMaxRmsToPeak(maxRmsToPeakMap.get(encParam));
            summaryItem.setMaxLoadAccel(maxLoadAccel.get(encParam));
            binSummary.getSummary().get(encParam).put("normal", summaryItem);

            summaryItem = new BinTableVO.BinSummary.SummaryItem();
            summaryItem.setMin(lpfMinMaxAvg.get(encParam).get(0));
            summaryItem.setMax(lpfMinMaxAvg.get(encParam).get(1));
            summaryItem.setAvg(lpfMinMaxAvg.get(encParam).get(2));
            summaryItem.setPsd(avgLpfPsdMap.get(encParam));

            freqData = lpfFreqMap.get(encParam);
            if (freqData != null) {
                summaryItem.setFrequency(ServerConstants.GSON.fromJson(freqData, freqType));
            }
            else {
                summaryItem.setFrequency(null);
            }

            summaryItem.setRms(lpfRmsMap.get(encParam));
            summaryItem.setAvg_rms(lpfAvgRmsMap.get(encParam));
            summaryItem.setN0(lpfN0Map.get(encParam));
            summaryItem.setAvg_n0(lpfAvgN0Map.get(encParam));
            summaryItem.setZeta(lpfZetaMap.get(encParam));
            summaryItem.setBurstFactor(lpfBurstFactorMap.get(encParam));
            summaryItem.setRmsToPeak(lpfRmsToPeakMap.get(encParam));
            summaryItem.setMaxRmsToPeak(lpfMaxRmsToPeakMap.get(encParam));
            summaryItem.setMaxLoadAccel(lpfMaxLoadAccel.get(encParam));
            binSummary.getSummary().get(encParam).put("lpf", summaryItem);

            summaryItem = new BinTableVO.BinSummary.SummaryItem();
            summaryItem.setMin(hpfMinMaxAvg.get(encParam).get(0));
            summaryItem.setMax(hpfMinMaxAvg.get(encParam).get(1));
            summaryItem.setAvg(hpfMinMaxAvg.get(encParam).get(2));
            summaryItem.setPsd(avgHpfPsdMap.get(encParam));

            freqData = hpfFreqMap.get(encParam);
            if (freqData != null) {
                summaryItem.setFrequency(ServerConstants.GSON.fromJson(freqData, freqType));
            }
            else {
                summaryItem.setFrequency(null);
            }

            summaryItem.setRms(hpfRmsMap.get(encParam));
            summaryItem.setAvg_rms(hpfAvgRmsMap.get(encParam));
            summaryItem.setN0(hpfN0Map.get(encParam));
            summaryItem.setAvg_n0(hpfAvgN0Map.get(encParam));
            summaryItem.setZeta(hpfZetaMap.get(encParam));
            summaryItem.setBurstFactor(hpfBurstFactorMap.get(encParam));
            summaryItem.setRmsToPeak(hpfRmsToPeakMap.get(encParam));
            summaryItem.setMaxRmsToPeak(hpfMaxRmsToPeakMap.get(encParam));
            summaryItem.setMaxLoadAccel(hpfMaxLoadAccel.get(encParam));
            binSummary.getSummary().get(encParam).put("hpf", summaryItem);

            summaryItem = new BinTableVO.BinSummary.SummaryItem();
            summaryItem.setMin(bpfMinMaxAvg.get(encParam).get(0));
            summaryItem.setMax(bpfMinMaxAvg.get(encParam).get(1));
            summaryItem.setAvg(bpfMinMaxAvg.get(encParam).get(2));
            summaryItem.setPsd(avgBpfPsdMap.get(encParam));

            freqData = bpfFreqMap.get(encParam);
            if (freqData != null) {
                summaryItem.setFrequency(ServerConstants.GSON.fromJson(freqData, freqType));
            }
            else {
                summaryItem.setFrequency(null);
            }

            summaryItem.setRms(bpfRmsMap.get(encParam));
            summaryItem.setAvg_rms(bpfAvgRmsMap.get(encParam));
            summaryItem.setN0(bpfN0Map.get(encParam));
            summaryItem.setAvg_n0(bpfAvgN0Map.get(encParam));
            summaryItem.setZeta(bpfZetaMap.get(encParam));
            summaryItem.setBurstFactor(bpfBurstFactorMap.get(encParam));
            summaryItem.setRmsToPeak(bpfRmsToPeakMap.get(encParam));
            summaryItem.setMaxRmsToPeak(bpfMaxRmsToPeakMap.get(encParam));
            summaryItem.setMaxLoadAccel(bpfMaxLoadAccel.get(encParam));
            binSummary.getSummary().get(encParam).put("bpf", summaryItem);
        }

        hashOps.put("BT" + binTable.getSeq().originOf(), "PARAMS", ServerConstants.GSON.toJson(psdMap.keySet()));

        return binSummary;
    }
}
