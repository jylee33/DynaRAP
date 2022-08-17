package com.servetech.dynarap.db.service;

import com.google.gson.JsonObject;
import com.servetech.dynarap.config.ServerConstants;
import com.servetech.dynarap.controller.ApiController;
import com.servetech.dynarap.db.mapper.BinTableMapper;
import com.servetech.dynarap.db.mapper.DirMapper;
import com.servetech.dynarap.db.mapper.ParamMapper;
import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.db.type.String64;
import com.servetech.dynarap.ext.HandledServiceException;
import com.servetech.dynarap.vo.BinTableVO;
import com.servetech.dynarap.vo.DataPropVO;
import com.servetech.dynarap.vo.DirVO;
import lombok.Data;
import org.apache.ibatis.annotations.*;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.*;

@Service("binTableService")
public class BinTableService {

    @Autowired
    private BinTableMapper binTableMapper;

    @Autowired
    private RawService rawService;

    public List<BinTableVO> getBinTableList() throws HandledServiceException {
        try {
            return binTableMapper.selectBinTableList();
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
            binTable.setDataProps(rawService.getDataPropList("bintable", binTable.getSeq()));
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
            }

            return binTable;
        } catch(Exception e) {
            throw new HandledServiceException(411, e.getMessage());
        }
    }
}
