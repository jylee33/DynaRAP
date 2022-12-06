package com.servetech.dynarap.db.service;

import com.servetech.dynarap.db.mapper.ParamModuleMapper;
import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.ext.HandledServiceException;
import com.servetech.dynarap.vo.DLLVO;
import com.servetech.dynarap.vo.ParamModuleVO;
import com.servetech.dynarap.vo.PartVO;
import com.servetech.dynarap.vo.ShortBlockVO;
import org.apache.ibatis.annotations.*;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@Service("paramModuleService")
public class ParamModuleService {

    @Autowired
    private ParamModuleMapper paramModuleMapper;

    @Autowired
    private RawService rawService;

    public List<ParamModuleVO> getParamModuleList() throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            List<ParamModuleVO> paramModules = paramModuleMapper.selectParamModuleList(params);
            if (paramModules == null) paramModules = new ArrayList<>();
            for (ParamModuleVO paramModule : paramModules) {
                paramModule.setDataProp(rawService.getDataPropListToMap("parammodule", paramModule.getSeq()));
            }
            return paramModules;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public ParamModuleVO getParamModuleBySeq(CryptoField seq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", seq);
            ParamModuleVO paramModule = paramModuleMapper.selectParamModuleBySeq(params);
            if (paramModule != null) {
                paramModule.setDataProp(rawService.getDataPropListToMap("parammodule", paramModule.getSeq()));
            }
            return paramModule;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void insertParamModule(ParamModuleVO paramModule) throws HandledServiceException {
        try {
            paramModuleMapper.insertParamModule(paramModule);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void updateParamModule(ParamModuleVO paramModule) throws HandledServiceException {
        try {
            paramModuleMapper.updateParamModule(paramModule);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteParamModule(CryptoField moduleSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", moduleSeq);
            paramModuleMapper.deleteParamModule(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public List<ParamModuleVO.Source> getParamModuleSourceList(CryptoField moduleSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("moduleSeq", moduleSeq);
            return paramModuleMapper.selectParamModuleSourceList(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public ParamModuleVO.Source getParamModuleSourceBySeq(CryptoField seq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", seq);
            ParamModuleVO.Source paramModuleSource = paramModuleMapper.selectParamModuleSourceBySeq(params);
            return paramModuleSource;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void insertParamModuleSource(ParamModuleVO.Source paramModuleSource) throws HandledServiceException {
        try {
            paramModuleMapper.insertParamModuleSource(paramModuleSource);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void updateParamModuleSource(ParamModuleVO.Source paramModuleSource) throws HandledServiceException {
        try {
            paramModuleMapper.updateParamModuleSource(paramModuleSource);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteParamModuleSource(CryptoField seq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", seq);
            paramModuleMapper.deleteParamModuleSource(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteParamModuleSourceByModuleSeq(CryptoField moduleSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("moduleSeq", moduleSeq);
            paramModuleMapper.deleteParamModuleSourceByModuleSeq(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public List<ParamModuleVO.Equation> getParamModuleEqList(CryptoField moduleSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("moduleSeq", moduleSeq);
            List<ParamModuleVO.Equation> equations = paramModuleMapper.selectParamModuleEqList(params);
            if (equations == null) equations = new ArrayList<>();
            for (ParamModuleVO.Equation equation : equations) {
                equation.setEqNo("E" + equation.getSeq().originOf());
                equation.setDataProp(rawService.getDataPropListToMap("eq", equation.getSeq()));
            }
            return equations;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public ParamModuleVO.Equation getParamModuleEqBySeq(CryptoField seq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", seq);
            ParamModuleVO.Equation equation = paramModuleMapper.selectParamModuleEqBySeq(params);
            if (equation != null) {
                equation.setDataProp(rawService.getDataPropListToMap("eq", equation.getSeq()));
            }
            return equation;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void insertParamModuleEq(ParamModuleVO.Equation equation) throws HandledServiceException {
        try {
            paramModuleMapper.insertParamModuleEq(equation);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void updateParamModuleEq(ParamModuleVO.Equation equation) throws HandledServiceException {
        try {
            paramModuleMapper.updateParamModuleEq(equation);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteParamModuleEq(CryptoField seq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", seq);
            paramModuleMapper.deleteParamModuleEq(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteParamModuleEqByModuleSeq(CryptoField moduleSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("moduleSeq", moduleSeq);

            // dataProp 삭제.
            List<ParamModuleVO.Equation> equations = getParamModuleEqList(moduleSeq);
            if (equations == null) equations = new ArrayList<>();
            for (ParamModuleVO.Equation eq : equations) {
                rawService.deleteDataPropByType("eq", eq.getSeq());
            }

            paramModuleMapper.deleteParamModuleEqByModuleSeq(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public List<ParamModuleVO.Plot> getParamModulePlotList(CryptoField moduleSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("moduleSeq", moduleSeq);
            List<ParamModuleVO.Plot> plots = paramModuleMapper.selectParamModulePlotList(params);
            if (plots == null) plots = new ArrayList<>();
            for (ParamModuleVO.Plot plot : plots) {
                plot.setDataProp(rawService.getDataPropListToMap("plot", plot.getSeq()));

                List<ParamModuleVO.Plot.Source> plotSources = getParamModulePlotSourceList(plot.getModuleSeq(), plot.getSeq());
                if (plotSources == null) plotSources = new ArrayList<>();
                plot.setPlotSourceList(plotSources);
                plot.setPlotSources(new ArrayList<>());
                for (ParamModuleVO.Plot.Source plotSource : plotSources) {
                    plot.getPlotSources().add(ParamModuleVO.Plot.Source.getSimple(plotSource));
                }
            }
            return plots;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public ParamModuleVO.Plot getParamModulePlotBySeq(CryptoField seq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", seq);
            ParamModuleVO.Plot plot = paramModuleMapper.selectParamModulePlotBySeq(params);
            if (plot != null) {
                plot.setDataProp(rawService.getDataPropListToMap("plot", plot.getSeq()));
            }
            return plot;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void insertParamModulePlot(ParamModuleVO.Plot plot) throws HandledServiceException {
        try {
            paramModuleMapper.insertParamModulePlot(plot);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void updateParamModulePlot(ParamModuleVO.Plot plot) throws HandledServiceException {
        try {
            paramModuleMapper.updateParamModulePlot(plot);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteParamModulePlot(CryptoField seq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", seq);

            ParamModuleVO.Plot plot = getParamModulePlotBySeq(seq);
            if (plot != null) {
                deleteParamModulePlotSourceByPlotSeq(plot.getModuleSeq(), plot.getSeq());

                // delete all savepoint
                deletePlotSavePoint(plot.getModuleSeq(), plot.getSeq());
            }

            paramModuleMapper.deleteParamModulePlot(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteParamModulePlotByModuleSeq(CryptoField moduleSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("moduleSeq", moduleSeq);

            // 모든 소스 지우기.
            deleteParamModulePlotSourceByModuleSeq(moduleSeq);

            // delete all seires.
            deleteParamModulePlotSeriesByModuleSeq(moduleSeq);

            // delete all savepoint
            deletePlotSavePoint(moduleSeq, CryptoField.LZERO);

            // dataProp 삭제.
            List<ParamModuleVO.Plot> plots = getParamModulePlotList(moduleSeq);
            if (plots == null) plots = new ArrayList<>();
            for (ParamModuleVO.Plot plot : plots) {
                rawService.deleteDataPropByType("plot", plot.getSeq());
            }

            paramModuleMapper.deleteParamModulePlotByModuleSeq(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public List<ParamModuleVO.Plot.Source> getParamModulePlotSourceList(CryptoField moduleSeq, CryptoField plotSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("moduleSeq", moduleSeq);
            params.put("plotSeq", plotSeq);
            return paramModuleMapper.selectParamModulePlotSourceList(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public ParamModuleVO.Plot.Source getParamModulePlotSourceBySeq(CryptoField seq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", seq);
            return paramModuleMapper.selectParamModulePlotSourceBySeq(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void insertParamModulePlotSource(ParamModuleVO.Plot.Source plotSource) throws HandledServiceException {
        try {
            paramModuleMapper.insertParamModulePlotSource(plotSource);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void updateParamModulePlotSource(ParamModuleVO.Plot.Source plotSource) throws HandledServiceException {
        try {
            paramModuleMapper.updateParamModulePlotSource(plotSource);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteParamModulePlotSource(CryptoField seq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", seq);
            paramModuleMapper.deleteParamModulePlotSource(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteParamModulePlotSourceByPlotSeq(CryptoField moduleSeq, CryptoField plotSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("moduleSeq", moduleSeq);
            params.put("plotSeq", plotSeq);
            paramModuleMapper.deleteParamModulePlotSourceByPlotSeq(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteParamModulePlotSourceByModuleSeq(CryptoField moduleSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("moduleSeq", moduleSeq);
            paramModuleMapper.deleteParamModulePlotSourceByModuleSeq(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }



    public List<ParamModuleVO.Plot.Series> getParamModulePlotSeriesList(CryptoField plotSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("plotSeq", plotSeq);
            return paramModuleMapper.selectParamModulePlotSeriesList(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public ParamModuleVO.Plot.Series selectParamModulePlotSeriesBySeq(CryptoField seq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", seq);
            return paramModuleMapper.selectParamModulePlotSeriesBySeq(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void insertParamModulePlotSeries(ParamModuleVO.Plot.Series plotSeries) throws HandledServiceException {
        try {
            paramModuleMapper.insertParamModulePlotSeries(plotSeries);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void updateParamModulePlotSeries(ParamModuleVO.Plot.Series plotSeries) throws HandledServiceException {
        try {
            paramModuleMapper.updateParamModulePlotSeries(plotSeries);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteParamModulePlotSeries(CryptoField seq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("seq", seq);
            paramModuleMapper.deleteParamModulePlotSeries(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteParamModulePlotSeriesByPlotSeq(CryptoField plotSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("plotSeq", plotSeq);
            paramModuleMapper.deleteParamModulePlotSeriesByPlotSeq(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteParamModulePlotSeriesByModuleSeq(CryptoField moduleSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("moduleSeq", moduleSeq);
            paramModuleMapper.deleteParamModulePlotSeriesByModuleSeq(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public List<PartVO> getPartListByKeyword(String keyword) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("keyword", keyword);
            return paramModuleMapper.selectPartListByKeyword(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public List<ShortBlockVO> getShortBlockListByKeyword(String keyword) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("keyword", keyword);
            return paramModuleMapper.selectShortBlockListByKeyword(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public List<DLLVO> getDLLListByKeyword(String keyword) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("keyword", keyword);
            return paramModuleMapper.selectDLLListByKeyword(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public List<ParamModuleVO> getParamModuleListByKeyword(String keyword) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("keyword", keyword);
            return paramModuleMapper.selectParamModuleListByKeyword(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public List<ParamModuleVO.Source> getReferencedSourceList(List<PartVO> partList) throws HandledServiceException {
        try {
            if (partList == null) partList = new ArrayList<>();

            StringBuilder sbPart = new StringBuilder();
            for (PartVO part : partList) {
                sbPart.append((Long) part.getSeq().originOf()).append(",");
            }
            if (partList.size() > 0) sbPart.setLength(sbPart.length() - 1);

            if (sbPart.length() == 0) return null;

            Map<String, Object> params = new HashMap<>();
            params.put("partSet", sbPart.toString());
            return paramModuleMapper.selectReferencedSourceList(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public List<ParamModuleVO.Plot.SavePoint> getPlotSavePointList(CryptoField moduleSeq, CryptoField plotSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("moduleSeq", moduleSeq);
            params.put("plotSeq", (plotSeq == null) ? CryptoField.LZERO : plotSeq);
            return paramModuleMapper.selectPlotSavePointList(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public void insertPlotSavePoint(ParamModuleVO.Plot.SavePoint savePoint) throws HandledServiceException {
        try {
            paramModuleMapper.insertPlotSavePoint(savePoint);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    public void deletePlotSavePoint(CryptoField moduleSeq, CryptoField plotSeq) throws HandledServiceException {
        try {
            Map<String, Object> params = new HashMap<>();
            params.put("moduleSeq", moduleSeq);
            params.put("plotSeq", (plotSeq == null) ? CryptoField.LZERO : plotSeq);
            paramModuleMapper.deletePlotSavePoint(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

}
