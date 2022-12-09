package com.servetech.dynarap.db.service;

import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.google.gson.JsonPrimitive;
import com.servetech.dynarap.config.ServerConstants;
import com.servetech.dynarap.controller.ApiController;
import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.ext.ConvexHull;
import com.servetech.dynarap.ext.HandledServiceException;
import com.servetech.dynarap.vo.*;
import org.apache.catalina.Server;
import org.springframework.data.redis.core.HashOperations;
import org.springframework.data.redis.core.ListOperations;
import org.springframework.data.redis.core.ZSetOperations;

import javax.script.ScriptEngine;
import javax.script.ScriptEngineManager;
import java.util.*;

public class EquationHelper {
    private ListOperations<String, String> listOps;
    private HashOperations<String, String, String> hashOps;
    private ZSetOperations<String, String> zsetOps;

    private Map<String, ParamModuleVO> paramModuleData = new HashMap<>();

    public EquationHelper(ListOperations<String, String> listOps,
                          HashOperations<String, String, String> hashOps,
                          ZSetOperations<String, String> zsetOps) {
        this.listOps = listOps;
        this.hashOps = hashOps;
        this.zsetOps = zsetOps;
    }

    public void setListOps(ListOperations listOps) {
        this.listOps = listOps;
    }
    public void setHashOps(HashOperations hashOps) {
        this.hashOps = hashOps;
    }
    public void setZsetOps(ZSetOperations zsetOps) {
        this.zsetOps = zsetOps;
    }

    public void loadParamModuleData(CryptoField moduleSeq) throws HandledServiceException {
        loadParamModuleData(moduleSeq, false);
    }

    public void loadParamModuleData(CryptoField moduleSeq, boolean forced) throws HandledServiceException {
        ParamModuleVO paramModule = paramModuleData.get(moduleSeq.valueOf());

        List<ParamModuleVO.Equation> lazyLoadEqs = new ArrayList<>();

        if (paramModule == null || forced) {
            paramModule = ApiController.getService(ParamModuleService.class).getParamModuleBySeq(moduleSeq);
            paramModule.setSources(ApiController.getService(ParamModuleService.class).getParamModuleSourceList(moduleSeq));
            if (paramModule.getSources() == null) paramModule.setSources(new ArrayList<>());

            paramModule.setParamData(new LinkedHashMap<>());

            for (ParamModuleVO.Source source : paramModule.getSources()) {
                if (source.getSourceType().equalsIgnoreCase("part")) {
                    loadPartData(source);
                    paramModule.getParamData().put(source.getSourceNo() + "_" + source.getParam().getParamKey(), source);
                } else if (source.getSourceType().equalsIgnoreCase("shortblock")) {
                    loadShortBlockData(source);
                    paramModule.getParamData().put(source.getSourceNo() + "_" + source.getParam().getParamKey(), source);
                } else if (source.getSourceType().equalsIgnoreCase("dll")) {
                    loadDllData(source);
                    paramModule.getParamData().put(source.getSourceNo() + "_" + source.getDllParam().getParamName().originOf(), source);
                } else if (source.getSourceType().equalsIgnoreCase("parammodule")) {
                    loadEquationData(source);
                    paramModule.getParamData().put(source.getSourceNo() + "_" + source.getEquation().getEqNo(), source);
                } else if (source.getSourceType().equalsIgnoreCase("bintable")) {
                    loadBintableData(source);
                    paramModule.getParamData().put(source.getSourceNo() + "_" + source.getParam().getParamKey(), source);
                }
            }

            paramModule.setEquations(ApiController.getService(ParamModuleService.class).getParamModuleEqList(moduleSeq));
            if (paramModule.getEquations() == null) paramModule.setEquations(new ArrayList<>());

            for (ParamModuleVO.Equation equation : paramModule.getEquations()) {
                loadEquationData(equation);
                if (equation.isLazyLoad()) {
                    lazyLoadEqs.add(equation);
                }
                else {
                    if (paramModule.getEqMap() == null)
                        paramModule.setEqMap(new LinkedHashMap<>());
                    paramModule.getEqMap().put(equation.getEqNo(), equation);
                }
            }
        }

        paramModuleData.put(moduleSeq.valueOf(), paramModule);

        if (lazyLoadEqs.size() > 0) {
            for (ParamModuleVO.Equation eq : lazyLoadEqs) {
                try {
                    JsonArray jarrData = tryCalculate(paramModule, eq.getEquation(), 0L);

                    List<Object> data = new ArrayList<>();
                    List<Double[]> convhData = new ArrayList<>();

                    for (int i = 0; i < jarrData.size(); i++) {
                        if (jarrData.get(i).isJsonNull())
                            data.add(null);
                        else if (jarrData.get(i).isJsonPrimitive()) {
                            if (jarrData.get(i).getAsJsonPrimitive().isString())
                                data.add(jarrData.get(i).getAsString());
                            else
                                data.add(jarrData.get(i).getAsDouble());
                        }
                        else if (jarrData.get(i).isJsonArray()) {
                            // for convh
                            data.add(jarrData.get(i).getAsJsonArray());
                            Double[] convhItem = new Double[] {
                                    jarrData.get(i).getAsJsonArray().get(0).getAsDouble(),
                                    jarrData.get(i).getAsJsonArray().get(1).getAsDouble()
                            };
                            convhData.add(convhItem);
                        }
                    }

                    if (convhData != null && convhData.size() > 0)
                        eq.setData(new ArrayList<>());
                    else
                        eq.setData(data);
                    eq.setConvhData(convhData);

                    if (eq.getEquation().contains("_time")) {
                        List<String> timeSet = new ArrayList<>();
                        for (Object time : data) {
                            timeSet.add((String) time);
                        }
                        eq.setTimeSet(timeSet);

                        hashOps.put("PM" + eq.getModuleSeq().originOf(), "E" + eq.getSeq().originOf() + ".TimeSet",
                                ServerConstants.GSON.toJson(eq.getTimeSet()));
                    }
                    else {
                        String jsonData = hashOps.get("PM" + eq.getModuleSeq().originOf(), "E" + eq.getSeq().originOf() + ".TimeSet");
                        if (jsonData != null && !jsonData.isEmpty()) {
                            jarrData = ServerConstants.GSON.fromJson(jsonData, JsonArray.class);
                            List<String> timeSet = new ArrayList<>();
                            for (int i = 0; i < jarrData.size(); i++)
                                timeSet.add(jarrData.get(i).getAsString());
                            eq.setTimeSet(timeSet);
                        }
                    }

                    eq.setDataCount(data == null ? 0 : data.size());

                    if (paramModule.getEqMap() == null)
                        paramModule.setEqMap(new LinkedHashMap<>());
                    paramModule.getEqMap().put(eq.getEqNo(), eq);
                } catch(HandledServiceException hse) {
                    throw hse;
                }
            }
            lazyLoadEqs.clear();
        }
    }

    public ParamModuleVO getParamModule(CryptoField moduleSeq) throws HandledServiceException {
        ParamModuleVO paramModule = paramModuleData.get(moduleSeq.valueOf());
        if (paramModule == null) {
            loadParamModuleData(moduleSeq, true);
            paramModule = paramModuleData.get(moduleSeq.valueOf());
        }
        return paramModule;
    }

    public List<ParamModuleVO.Source> getParamModuleSources(CryptoField moduleSeq) throws HandledServiceException {
        ParamModuleVO paramModule = paramModuleData.get(moduleSeq.valueOf());
        if (paramModule == null) throw new HandledServiceException(411, "파라미터 모듈의 소스가 준비되지 않았습니다.");
        return new ArrayList<ParamModuleVO.Source>(paramModule.getParamData().values());
    }

    public List<Object> loadPlotData(CryptoField moduleSeq, ParamModuleVO.Plot.PlotSource plotSource) throws HandledServiceException {
        ParamModuleVO paramModule = paramModuleData.get(moduleSeq.valueOf());
        if (paramModule == null) {
            loadParamModuleData(moduleSeq);
            paramModule = paramModuleData.get(moduleSeq.valueOf());
        }

        List<Object> sourceData = null;
        if (!plotSource.getSourceType().equals("eq")) {
            ParamModuleVO.Source moduleSource = null;
            for (ParamModuleVO.Source source : paramModule.getSources()) {
                if (source.getSeq().equals(plotSource.getSourceSeq())) {
                    moduleSource = source;
                    break;
                }
            }

            if (moduleSource == null)
                throw new HandledServiceException(411, "PLOT 데이터를 찾을 수 없습니다.");

            sourceData = moduleSource.getData();
        }
        else {
            ParamModuleVO.Equation equation = null;
            for (ParamModuleVO.Equation eq : paramModule.getEquations()) {
                if (eq.getSeq().equals(plotSource.getSourceSeq())) {
                    equation = eq;
                    break;
                }
            }

            if (equation == null)
                throw new HandledServiceException(411, "PLOT 데이터를 찾을 수 없습니다.");

            sourceData = equation.getData();
        }

        List<Object> plotData = null;
        if (sourceData != null) {
            plotData = new ArrayList<>();
            for (Object item : sourceData) {
                plotData.add(item);
            }
        }

        return plotData;
    }

    public JsonArray calculateEquationSingle(CryptoField moduleSeq, String equation) throws HandledServiceException {
        ParamModuleVO paramModule = paramModuleData.get(moduleSeq.valueOf());
        if (paramModule == null) {
            loadParamModuleData(moduleSeq);
            paramModule = paramModuleData.get(moduleSeq.valueOf());
        }

        try {
            JsonArray jarrResult = tryCalculate(paramModule, equation, 0L);
            return jarrResult;
        } catch(HandledServiceException hse) {
            throw hse;
        }
    }

    public void calculateEquations(CryptoField moduleSeq, List<ParamModuleVO.Equation> equations) throws HandledServiceException {
        calculateEquations(moduleSeq, equations, false);
    }

    public void calculateEquations(CryptoField moduleSeq, List<ParamModuleVO.Equation> equations, boolean forced) throws HandledServiceException {
        ParamModuleVO paramModule = paramModuleData.get(moduleSeq.valueOf());
        if (paramModule == null || forced) {
            loadParamModuleData(moduleSeq, forced);
            paramModule = paramModuleData.get(moduleSeq.valueOf());
        }

        for (int i = 0; i < equations.size(); i++) {
            ParamModuleVO.Equation oldEquation = null;
            if (i < paramModule.getEquations().size())
                oldEquation = paramModule.getEquations().get(i);

            boolean reCalculate = false;
            if (oldEquation == null || !oldEquation.getEquation().equals(equations.get(i).getEquation()))
                reCalculate = true;

            //reCalculate = true;

            if (reCalculate) {
                JsonArray calResult = tryCalculate(paramModule, equations.get(i));
                if (calResult != null)
                  equations.get(i).setDataCount(calResult.size());
                paramModule.getEqMap().put(equations.get(i).getEqNo(), equations.get(i));
            }
            else {
                if (equations.get(i).getData() == null) {
                    loadEquationData(equations.get(i));
                }
            }
        }
    }

    private JsonArray tryCalculate(ParamModuleVO paramModule, ParamModuleVO.Equation equation) throws HandledServiceException {
        return tryCalculate(paramModule, equation.getEquation(), equation.getSeq().originOf());
    }

    private JsonArray tryCalculate(ParamModuleVO paramModule, String equation) throws HandledServiceException {
        return tryCalculate(paramModule, equation, 0L);
    }

    private JsonArray tryCalculate(ParamModuleVO paramModule, String eq, Long eqSeq) throws HandledServiceException {
        // check convh
        if (eq.contains("convh(")) {
            eq = eq.replaceAll("\\\\", "");
            List<String> sensors = ServerConstants.extractParams(eq, "{", "}");
            List<Object> resultSet = new ArrayList<>();

            // not match convh
            if (sensors != null && sensors.size() != 2) {
                if (eqSeq > 0L) {
                    hashOps.put("PM" + paramModule.getSeq().originOf(), "E" + eqSeq,
                            "[]");
                }
                return ServerConstants.GSON.fromJson("[]", JsonArray.class);
            }

            // 센서 포함 수식. -> 계산 결과가 배열로 나옴.
            List<Object> src0 = null;
            List<Object> src1 = null;

            ParamModuleVO.Source partSource = paramModule.getParamData().get(sensors.get(0));
            if (partSource != null) {
                if (sensors.get(0).endsWith("_H") || sensors.get(0).contains("_H_")) {
                    src0 = partSource.getHpfData();
                } else if (sensors.get(0).endsWith("_L") || sensors.get(0).contains("_L_")) {
                    src0 = partSource.getLpfData();
                } else if (sensors.get(0).endsWith("_B") || sensors.get(0).contains("_B_")) {
                    src0 = partSource.getBpfData();
                } else {
                    src0 = partSource.getData();
                }
            } else {
                ParamModuleVO.Equation equation = paramModule.getEqMap().get(sensors.get(0));
                if (equation != null) {
                    if (equation.getData() == null) {
                        loadEquationData(equation);
                    }
                    src0 = equation.getData();
                }
            }

            partSource = paramModule.getParamData().get(sensors.get(1));
            if (partSource != null) {
                if (sensors.get(1).endsWith("_H") || sensors.get(1).contains("_H_")) {
                    src1 = partSource.getHpfData();
                } else if (sensors.get(1).endsWith("_L") || sensors.get(1).contains("_L_")) {
                    src1 = partSource.getLpfData();
                } else if (sensors.get(1).endsWith("_B") || sensors.get(1).contains("_B_")) {
                    src1 = partSource.getBpfData();
                } else {
                    src1 = partSource.getData();
                }
            } else {
                ParamModuleVO.Equation equation = paramModule.getEqMap().get(sensors.get(1));
                if (equation != null) {
                    if (equation.getData() == null) {
                        loadEquationData(equation);
                    }
                    src1 = equation.getData();
                }
            }

            if (src0 == null || src1 == null || src0.size() != src1.size())
                throw new HandledServiceException(411, "ConvexHull 계산을 위해서는 데이터수가 일치해야합니다.");

            List<ConvexHull.Point> cps = new ArrayList<>();
            for (int i = 0; i < src0.size(); i++) {
                cps.add(new ConvexHull.Point((Double) src0.get(i), (Double) src1.get(i)));
            }
            List<ConvexHull.Point> hull = ConvexHull.makeHull(cps);
            if (hull == null) hull = new ArrayList<>();
            List<List<Double>> hullArr = new ArrayList<>();
            for (ConvexHull.Point p : hull) {
                List<Double> hullItem = new ArrayList<>();
                hullItem.add(p.x);
                hullItem.add(p.y);
                hullArr.add(hullItem);
            }

            if (eqSeq > 0L) {
                hashOps.put("PM" + paramModule.getSeq().originOf(), "E" + eqSeq,
                        ServerConstants.GSON.toJson(hullArr));
            }
            return ServerConstants.GSON.fromJson(ServerConstants.GSON.toJson(hullArr), JsonArray.class);
        }

        // 싱글 파라미터 처리 (lrp xyz, shortblock min,max,avg,rms,n0)
        Set<String> paramSet = paramModule.getParamData().keySet();
        Iterator<String> iterParamSet = paramSet.iterator();
        while (iterParamSet.hasNext()) {
            String paramKey = iterParamSet.next();
            ParamModuleVO.Source paramSource = paramModule.getParamData().get(paramKey);

            if (paramSource.getParam() != null) {
                eq = eq.replaceAll("\\{" + paramKey + "_X\\}", "(" + paramSource.getParam().getLrpX() + ")");
                eq = eq.replaceAll("\\{" + paramKey + "_Y\\}", "(" + paramSource.getParam().getLrpY() + ")");
                eq = eq.replaceAll("\\{" + paramKey + "_Z\\}", "(" + paramSource.getParam().getLrpZ() + ")");
            }

            if (paramKey.startsWith("P")) {
                if (eq.contains(paramKey + "_time")) {
                    if (eqSeq > 0L) {
                        hashOps.put("PM" + paramModule.getSeq().originOf(), "E" + eqSeq,
                                ServerConstants.GSON.toJson(paramSource.getTimeSet()));
                    }
                    return ServerConstants.GSON.fromJson(ServerConstants.GSON.toJson(paramSource.getTimeSet()), JsonArray.class);
                }
            }

            // shortblock 싱글 처리
            if (paramKey.startsWith("S")) {
                if (eq.contains(paramKey + "_time")) {
                    if (eqSeq > 0L) {
                        hashOps.put("PM" + paramModule.getSeq().originOf(), "E" + eqSeq,
                                ServerConstants.GSON.toJson(paramSource.getTimeSet()));
                    }
                    return ServerConstants.GSON.fromJson(ServerConstants.GSON.toJson(paramSource.getTimeSet()), JsonArray.class);
                }

                if (paramSource.getParamData() != null) {
                    if (eq.contains(paramKey + "_L_psd") || eq.contains(paramKey + "_H_psd") || eq.contains(paramKey + "_B_psd") || eq.contains(paramKey + "_psd")) {
                        if (eq.contains(paramKey + "_L_psd")) {
                            if (eqSeq > 0L) {
                                hashOps.put("PM" + paramModule.getSeq().originOf(), "E" + eqSeq,
                                        paramSource.getParamData().getLpfPsd());
                            }
                            return ServerConstants.GSON.fromJson(paramSource.getParamData().getLpfPsd(), JsonArray.class);
                        }
                        else if (eq.contains(paramKey + "_H_psd")) {
                            if (eqSeq > 0L) {
                                hashOps.put("PM" + paramModule.getSeq().originOf(), "E" + eqSeq,
                                        paramSource.getParamData().getHpfPsd());
                            }
                            return ServerConstants.GSON.fromJson(paramSource.getParamData().getHpfPsd(), JsonArray.class);
                        }
                        else if (eq.contains(paramKey + "_B_psd")) {
                            if (eqSeq > 0L) {
                                hashOps.put("PM" + paramModule.getSeq().originOf(), "E" + eqSeq,
                                        paramSource.getParamData().getBpfPsd());
                            }
                            return ServerConstants.GSON.fromJson(paramSource.getParamData().getBpfPsd(), JsonArray.class);
                        }
                        else {
                            if (eqSeq > 0L) {
                                hashOps.put("PM" + paramModule.getSeq().originOf(), "E" + eqSeq,
                                        paramSource.getParamData().getPsd());
                            }
                            return ServerConstants.GSON.fromJson(paramSource.getParamData().getPsd(), JsonArray.class);
                        }
                    }
                    if (eq.contains(paramKey + "_L_freq") || eq.contains(paramKey + "_H_freq") || eq.contains(paramKey + "_B_freq") || eq.contains(paramKey + "_freq")) {
                        if (eq.contains(paramKey + "_L_freq")) {
                            if (eqSeq > 0L) {
                                hashOps.put("PM" + paramModule.getSeq().originOf(), "E" + eqSeq,
                                        paramSource.getParamData().getLpfFrequency());
                            }
                            return ServerConstants.GSON.fromJson(paramSource.getParamData().getLpfFrequency(), JsonArray.class);
                        }
                        else if (eq.contains(paramKey + "_H_freq")) {
                            if (eqSeq > 0L) {
                                hashOps.put("PM" + paramModule.getSeq().originOf(), "E" + eqSeq,
                                        paramSource.getParamData().getHpfFrequency());
                            }
                            return ServerConstants.GSON.fromJson(paramSource.getParamData().getHpfFrequency(), JsonArray.class);
                        }
                        else if (eq.contains(paramKey + "_B_freq")) {
                            if (eqSeq > 0L) {
                                hashOps.put("PM" + paramModule.getSeq().originOf(), "E" + eqSeq,
                                        paramSource.getParamData().getBpfFrequency());
                            }
                            return ServerConstants.GSON.fromJson(paramSource.getParamData().getBpfFrequency(), JsonArray.class);
                        }
                        else {
                            if (eqSeq > 0L) {
                                hashOps.put("PM" + paramModule.getSeq().originOf(), "E" + eqSeq,
                                        paramSource.getParamData().getFrequency());
                            }
                            return ServerConstants.GSON.fromJson(paramSource.getParamData().getFrequency(), JsonArray.class);
                        }
                    }

                    if (eq.contains(paramKey + "_L_peak") || eq.contains(paramKey + "_H_peak") || eq.contains(paramKey + "_B_peak") || eq.contains(paramKey + "_peak")) {
                        if (eq.contains(paramKey + "_L_peak")) {
                            if (eqSeq > 0L) {
                                hashOps.put("PM" + paramModule.getSeq().originOf(), "E" + eqSeq,
                                        paramSource.getParamData().getLpfPeak());
                            }
                            return ServerConstants.GSON.fromJson(paramSource.getParamData().getLpfPeak(), JsonArray.class);
                        }
                        else if (eq.contains(paramKey + "_H_peak")) {
                            if (eqSeq > 0L) {
                                hashOps.put("PM" + paramModule.getSeq().originOf(), "E" + eqSeq,
                                        paramSource.getParamData().getHpfPeak());
                            }
                            return ServerConstants.GSON.fromJson(paramSource.getParamData().getHpfPeak(), JsonArray.class);
                        }
                        else if (eq.contains(paramKey + "_B_peak")) {
                            if (eqSeq > 0L) {
                                hashOps.put("PM" + paramModule.getSeq().originOf(), "E" + eqSeq,
                                        paramSource.getParamData().getBpfPeak());
                            }
                            return ServerConstants.GSON.fromJson(paramSource.getParamData().getBpfPeak(), JsonArray.class);
                        }
                        else {
                            if (eqSeq > 0L) {
                                hashOps.put("PM" + paramModule.getSeq().originOf(), "E" + eqSeq,
                                        paramSource.getParamData().getPeak());
                            }
                            return ServerConstants.GSON.fromJson(paramSource.getParamData().getPeak(), JsonArray.class);
                        }
                    }

                    if (eq.contains(paramKey + "_L_zarray") || eq.contains(paramKey + "_H_zarray") || eq.contains(paramKey + "_B_zarray") || eq.contains(paramKey + "_zarray")) {
                        if (eq.contains(paramKey + "_L_zarray")) {
                            if (eqSeq > 0L) {
                                hashOps.put("PM" + paramModule.getSeq().originOf(), "E" + eqSeq,
                                        paramSource.getParamData().getLpfZarray());
                            }
                            return ServerConstants.GSON.fromJson(paramSource.getParamData().getLpfZarray(), JsonArray.class);
                        }
                        else if (eq.contains(paramKey + "_H_zarray")) {
                            if (eqSeq > 0L) {
                                hashOps.put("PM" + paramModule.getSeq().originOf(), "E" + eqSeq,
                                        paramSource.getParamData().getHpfZarray());
                            }
                            return ServerConstants.GSON.fromJson(paramSource.getParamData().getHpfZarray(), JsonArray.class);
                        }
                        else if (eq.contains(paramKey + "_B_zarray")) {
                            if (eqSeq > 0L) {
                                hashOps.put("PM" + paramModule.getSeq().originOf(), "E" + eqSeq,
                                        paramSource.getParamData().getBpfZarray());
                            }
                            return ServerConstants.GSON.fromJson(paramSource.getParamData().getBpfZarray(), JsonArray.class);
                        }
                        else {
                            if (eqSeq > 0L) {
                                hashOps.put("PM" + paramModule.getSeq().originOf(), "E" + eqSeq,
                                        paramSource.getParamData().getZarray());
                            }
                            return ServerConstants.GSON.fromJson(paramSource.getParamData().getZarray(), JsonArray.class);
                        }
                    }

                    eq = eq.replaceAll("\\{" + paramKey + "_L_min\\}", "(" + paramSource.getParamData().getBlockLpfMin() + ")");
                    eq = eq.replaceAll("\\{" + paramKey + "_L_max\\}", "(" + paramSource.getParamData().getBlockLpfMax() + ")");
                    eq = eq.replaceAll("\\{" + paramKey + "_L_avg\\}", "(" + paramSource.getParamData().getBlockLpfAvg() + ")");
                    eq = eq.replaceAll("\\{" + paramKey + "_L_rms\\}", "(" + paramSource.getParamData().getLpfRms() + ")");
                    eq = eq.replaceAll("\\{" + paramKey + "_L_n0\\}", "(" + paramSource.getParamData().getLpfN0() + ")");

                    eq = eq.replaceAll("\\{" + paramKey + "_H_min\\}", "(" + paramSource.getParamData().getBlockHpfMin() + ")");
                    eq = eq.replaceAll("\\{" + paramKey + "_H_max\\}", "(" + paramSource.getParamData().getBlockHpfMax() + ")");
                    eq = eq.replaceAll("\\{" + paramKey + "_H_avg\\}", "(" + paramSource.getParamData().getBlockHpfAvg() + ")");
                    eq = eq.replaceAll("\\{" + paramKey + "_H_rms\\}", "(" + paramSource.getParamData().getHpfRms() + ")");
                    eq = eq.replaceAll("\\{" + paramKey + "_H_n0\\}", "(" + paramSource.getParamData().getHpfN0() + ")");

                    eq = eq.replaceAll("\\{" + paramKey + "_B_min\\}", "(" + paramSource.getParamData().getBlockBpfMin() + ")");
                    eq = eq.replaceAll("\\{" + paramKey + "_B_max\\}", "(" + paramSource.getParamData().getBlockBpfMax() + ")");
                    eq = eq.replaceAll("\\{" + paramKey + "_B_avg\\}", "(" + paramSource.getParamData().getBlockBpfAvg() + ")");
                    eq = eq.replaceAll("\\{" + paramKey + "_B_rms\\}", "(" + paramSource.getParamData().getBpfRms() + ")");
                    eq = eq.replaceAll("\\{" + paramKey + "_B_n0\\}", "(" + paramSource.getParamData().getBpfN0() + ")");

                    eq = eq.replaceAll("\\{" + paramKey + "_min\\}", "(" + paramSource.getParamData().getBlockMin() + ")");
                    eq = eq.replaceAll("\\{" + paramKey + "_max\\}", "(" + paramSource.getParamData().getBlockMax() + ")");
                    eq = eq.replaceAll("\\{" + paramKey + "_avg\\}", "(" + paramSource.getParamData().getBlockAvg() + ")");
                    eq = eq.replaceAll("\\{" + paramKey + "_rms\\}", "(" + paramSource.getParamData().getRms() + ")");
                    eq = eq.replaceAll("\\{" + paramKey + "_n0\\}", "(" + paramSource.getParamData().getN0() + ")");
                }
            }

            // bintable single
            if (paramKey.startsWith("B")) {
                if (paramSource.getBinSummaries() != null) {
                    Set<String> cells = paramSource.getBinSummaries().keySet();
                    Iterator<String> iterCells = cells.iterator();
                    while (iterCells.hasNext()) {
                        String cell = iterCells.next();
                        BinTableVO.BinSummary binSummary = paramSource.getBinSummaries().get(cell);
                        if (binSummary == null) continue;

                        // source has only one, bpf
                        BinTableVO.BinSummary.SummaryItem summaryItem = binSummary.getSummary().values().iterator().next().get("bpf");

                        if (eq.contains(paramKey + "_" + cell + "_psd")) {
                            if (eqSeq > 0L) {
                                hashOps.put("PM" + paramModule.getSeq().originOf(), "E" + eqSeq,
                                        ServerConstants.GSON.toJson(summaryItem.getPsd()));
                            }
                            return ServerConstants.GSON.fromJson(ServerConstants.GSON.toJson(summaryItem.getPsd()), JsonArray.class);
                        }

                        if (eq.contains(paramKey + "_" + cell + "_freq")) {
                            if (eqSeq > 0L) {
                                hashOps.put("PM" + paramModule.getSeq().originOf(), "E" + eqSeq,
                                        ServerConstants.GSON.toJson(summaryItem.getFrequency()));
                            }
                            return ServerConstants.GSON.fromJson(ServerConstants.GSON.toJson(summaryItem.getFrequency()), JsonArray.class);
                        }

                        if (eq.contains(paramKey + "_" + cell + "_rms")) {
                            if (eqSeq > 0L) {
                                hashOps.put("PM" + paramModule.getSeq().originOf(), "E" + eqSeq,
                                        ServerConstants.GSON.toJson(summaryItem.getRms()));
                            }
                            return ServerConstants.GSON.fromJson(ServerConstants.GSON.toJson(summaryItem.getRms()), JsonArray.class);
                        }

                        if (eq.contains(paramKey + "_" + cell + "_n0")) {
                            if (eqSeq > 0L) {
                                hashOps.put("PM" + paramModule.getSeq().originOf(), "E" + eqSeq,
                                        ServerConstants.GSON.toJson(summaryItem.getN0()));
                            }
                            return ServerConstants.GSON.fromJson(ServerConstants.GSON.toJson(summaryItem.getN0()), JsonArray.class);
                        }

                        if (eq.contains(paramKey + "_" + cell + "_zeta")) {
                            if (eqSeq > 0L) {
                                hashOps.put("PM" + paramModule.getSeq().originOf(), "E" + eqSeq,
                                        ServerConstants.GSON.toJson(summaryItem.getZeta()));
                            }
                            return ServerConstants.GSON.fromJson(ServerConstants.GSON.toJson(summaryItem.getZeta()), JsonArray.class);
                        }

                        if (eq.contains(paramKey + "_" + cell + "_rtp")) {
                            if (eqSeq > 0L) {
                                hashOps.put("PM" + paramModule.getSeq().originOf(), "E" + eqSeq,
                                        ServerConstants.GSON.toJson(summaryItem.getRmsToPeak()));
                            }
                            return ServerConstants.GSON.fromJson(ServerConstants.GSON.toJson(summaryItem.getRmsToPeak()), JsonArray.class);
                        }

                        eq = eq.replaceAll("\\{" + paramKey + "_" + cell + "_min\\}", "(" + summaryItem.getMin() + ")");
                        eq = eq.replaceAll("\\{" + paramKey + "_" + cell + "_max\\}", "(" + summaryItem.getMax() + ")");
                        eq = eq.replaceAll("\\{" + paramKey + "_" + cell + "_avg\\}", "(" + summaryItem.getAvg() + ")");

                        eq = eq.replaceAll("\\{" + paramKey + "_" + cell + "_avgrms\\}", "(" + summaryItem.getAvg_rms() + ")");
                        eq = eq.replaceAll("\\{" + paramKey + "_" + cell + "_avgn0\\}", "(" + summaryItem.getAvg_n0() + ")");
                        eq = eq.replaceAll("\\{" + paramKey + "_" + cell + "_bf\\}", "(" + summaryItem.getBurstFactor() + ")");

                        eq = eq.replaceAll("\\{" + paramKey + "_" + cell + "_maxrtp\\}", "(" + summaryItem.getMaxRmsToPeak() + ")");
                        eq = eq.replaceAll("\\{" + paramKey + "_" + cell + "_maxla\\}", "(" + summaryItem.getMaxLoadAccel() + ")");
                    }
                }
            }
        }

        // 싱글 도출 함수 처리 (min,max,avg)
        if (eq.contains("max(") || eq.contains("min(") || eq.contains("avg(")) {
            while (eq.contains("max(") || eq.contains("min(") || eq.contains("avg(")) {
                String funcParam = ServerConstants.findFunctionParam(eq);
                if (funcParam == null) break;

                if (funcParam.startsWith("max")) {
                    // 찾아온 수식에서 센서 이름의 데이터를 계산 함.
                    eq = calcMaxFunc(paramModule, funcParam, eq);
                }
                else if (funcParam.startsWith("min")) {
                    eq = calcMinFunc(paramModule, funcParam, eq);
                }
                else if (funcParam.startsWith("avg")) {
                    eq = calcAvgFunc(paramModule, funcParam, eq);
                }
                else
                    break;
            }
        }

        // 배열 데이터 처리
        // -> 실제 데이터 처리 하여 배열이나 플랫한 값을 도출 하도록 함.
        eq = eq.replaceAll("\\\\", "");
        List<String> sensors = ServerConstants.extractParams(eq, "{", "}");
        List<Object> resultSet = new ArrayList<>();

        if (sensors != null && sensors.size() > 0) {
            // 센서 포함 수식. -> 계산 결과가 배열로 나옴.
            Map<String, List<Object>> sensorData = new HashMap<>();
            int outBound = 0;

            for (String sensor : sensors) {
                ParamModuleVO.Source partSource = paramModule.getParamData().get(sensor);
                if (partSource != null) {
                    if (sensor.endsWith("_H") || sensor.contains("_H_")) {
                        sensorData.put(sensor, partSource.getHpfData());
                        outBound = Math.max(outBound, partSource.getHpfData().size());
                    } else if (sensor.endsWith("_L") || sensor.contains("_L_")) {
                        sensorData.put(sensor, partSource.getLpfData());
                        outBound = Math.max(outBound, partSource.getLpfData().size());
                    } else if (sensor.endsWith("_B") || sensor.contains("_B_")) {
                        sensorData.put(sensor, partSource.getBpfData());
                        outBound = Math.max(outBound, partSource.getBpfData().size());
                    } else {
                        sensorData.put(sensor, partSource.getData());
                        outBound = Math.max(outBound, partSource.getData().size());
                    }
                }
                else {
                    ParamModuleVO.Equation equation = paramModule.getEqMap().get(sensor);
                    if (equation != null) {
                        if (equation.getData() == null) {
                          loadEquationData(equation);
                        }
                        sensorData.put(sensor, equation.getData());
                        if (equation.getData() != null)
                          outBound = Math.max(outBound, equation.getData().size());
                    }
                }
            }

            for (int i = 0; i < outBound; i++) {
                String calcEq = eq;
                for (String sensor : sensors) {
                    List<Object> data = sensorData.get(sensor);
                    if (data == null) continue;

                    Double calcVal = 0.0;
                    if (data.size() > i) {
                        try {
                            calcVal = Double.parseDouble(data.get(i) + "");
                        } catch (NumberFormatException nfe) {
                            throw new HandledServiceException(411, "문자를 계산에 사용할 수 없음.");
                        }
                    }

                    calcEq = calcEq.replaceAll("\\{" + sensor + "\\}", "(" + calcVal + ")");
                }

                if (outBound > 1) {
                    Double qVal = 0.0;
                    try {
                        ScriptEngineManager mgr = new ScriptEngineManager();
                        ScriptEngine engine = mgr.getEngineByName("JavaScript");
                        qVal = (Double) engine.eval(calcEq);
                    } catch (Exception e) {
                        throw new HandledServiceException(411, "스크립트로 계산할 수 없음 [" + calcEq + "]");
                    }

                    resultSet.add(qVal);
                }
                else if (outBound == 1) {
                    if (calcEq.startsWith("[") && calcEq.endsWith("]")) {
                        // 수식 처리로 된 것들.
                        String[] eqSplit = calcEq.substring(1, calcEq.length() - 1).split(",");
                        for (String partEq : eqSplit) {
                            if (partEq.contains("\"")) partEq = partEq.replaceAll("\\\"", "");

                            Double qVal = 0.0;
                            try {
                                ScriptEngineManager mgr = new ScriptEngineManager();
                                ScriptEngine engine = mgr.getEngineByName("JavaScript");
                                qVal = (Double) engine.eval(partEq.trim());
                                resultSet.add(qVal);
                            } catch (Exception e) {
                                try {
                                    qVal = Double.parseDouble(partEq.trim());
                                    resultSet.add(qVal);
                                } catch(Exception ex) {
                                    System.out.println("스크립트로 계산할 수 없음 [" + partEq + "]");
                                    resultSet.add(partEq.trim());
                                }
                            }
                        }
                    }
                    else {
                        if (calcEq.contains(","))
                            throw new HandledServiceException(411, "해석할 수 없는 수식입니다. [" + calcEq + "]");

                        if (calcEq.contains("\"")) {
                            resultSet.add(calcEq.trim());
                        }
                        else {
                            // 단일 식 혹은 배열 -> 수식을 배열로 바꿔서 결과 도출.
                            Double qVal = 0.0;
                            try {
                                ScriptEngineManager mgr = new ScriptEngineManager();
                                ScriptEngine engine = mgr.getEngineByName("JavaScript");
                                qVal = (Double) engine.eval(calcEq.trim());
                                resultSet.add(qVal);
                            } catch (Exception e) {
                                System.out.println("스크립트로 계산할 수 없음 [" + calcEq + "]");
                                resultSet.add(qVal);
                            }
                        }
                    }
                }
            }
        }
        else {
            if (eq.startsWith("[") && eq.endsWith("]")) {
                // 수식 처리로 된 것들.
                String[] eqSplit = eq.substring(1, eq.length() - 1).split(",");
                for (String partEq : eqSplit) {
                    if (partEq.contains("\"")) partEq = partEq.replaceAll("\\\"", "");

                    Double qVal = 0.0;
                    try {
                        ScriptEngineManager mgr = new ScriptEngineManager();
                        ScriptEngine engine = mgr.getEngineByName("JavaScript");
                        qVal = (Double) engine.eval(partEq.trim());
                        resultSet.add(qVal);
                    } catch (Exception e) {
                        try {
                            qVal = Double.parseDouble(partEq.trim());
                            resultSet.add(qVal);
                        } catch(Exception ex) {
                            System.out.println("스크립트로 계산할 수 없음 [" + partEq + "]");
                            resultSet.add(partEq.trim());
                        }
                    }
                }
            }
            else {
                if (eq.contains(","))
                    throw new HandledServiceException(411, "해석할 수 없는 수식입니다. [" + eq + "]");

                if (eq.contains("\"")) {
                    resultSet.add(eq.trim());
                }
                else {
                    // 단일 식 혹은 배열 -> 수식을 배열로 바꿔서 결과 도출.
                    Double qVal = 0.0;
                    try {
                        ScriptEngineManager mgr = new ScriptEngineManager();
                        ScriptEngine engine = mgr.getEngineByName("JavaScript");
                        qVal = (Double) engine.eval(eq.trim());
                        resultSet.add(qVal);
                    } catch (Exception e) {
                        System.out.println("스크립트로 계산할 수 없음 [" + eq + "]");
                        resultSet.add(qVal);
                    }
                }
            }
        }

        if (eqSeq > 0L) {
            System.out.println("PM" + paramModule.getSeq().originOf() + "/E" + eqSeq
                    + "=" + ServerConstants.GSON.toJson(resultSet));

            // 최종 결과값 저장 => 올 배열
            hashOps.put("PM" + paramModule.getSeq().originOf(), "E" + eqSeq,
                    ServerConstants.GSON.toJson(resultSet));
        }

        return ServerConstants.GSON.fromJson(ServerConstants.GSON.toJson(resultSet), JsonArray.class);
    }

    private String calcMaxFunc(ParamModuleVO paramModule, String partCalc, String currentEq) throws HandledServiceException {
        String partEq = partCalc.replaceAll("\\\\", "");
        partEq = partEq.substring("max(".length(), partEq.length() - ")".length());

        List<String> partSensors = ServerConstants.extractParams(partEq, "{", "}");
        if (partSensors == null || partSensors.size() == 0)
            throw new HandledServiceException(411, "추출된 센서 데이터가 없음.");

        Map<String, List<Object>> sensorData = new HashMap<>();
        int outBound = 0;

        for (String sensor : partSensors) {
            ParamModuleVO.Source partSource = paramModule.getParamData().get(sensor);
            if (partSource != null) {
                if (sensor.endsWith("_H") || sensor.contains("_H_")) {
                    sensorData.put(sensor, partSource.getHpfData());
                    outBound = Math.max(outBound, partSource.getHpfData().size());
                } else if (sensor.endsWith("_L") || sensor.contains("_L_")) {
                    sensorData.put(sensor, partSource.getLpfData());
                    outBound = Math.max(outBound, partSource.getLpfData().size());
                } else if (sensor.endsWith("_B") || sensor.contains("_B_")) {
                    sensorData.put(sensor, partSource.getBpfData());
                    outBound = Math.max(outBound, partSource.getBpfData().size());
                } else {
                    sensorData.put(sensor, partSource.getData());
                    outBound = Math.max(outBound, partSource.getData().size());
                }
            }
            else {
                ParamModuleVO.Equation equation = paramModule.getEqMap().get(sensor);
                if (equation != null) {
                    sensorData.put(sensor, equation.getData());
                    outBound = Math.max(outBound, equation.getData().size());
                }
            }
        }

        List<Double> resultSet = new ArrayList<>();
        for (int i = 0; i < outBound; i++) {
            String calcEq = partEq;
            for (String sensor : partSensors) {
                List<Object> data = sensorData.get(sensor);

                Double calcVal = 0.0;
                if (data.size() > i) {
                    try {
                        calcVal = Double.parseDouble(data.get(i) + "");
                    } catch(NumberFormatException nfe) {
                        throw new HandledServiceException(411, "문자를 계산에 사용할 수 없음.");
                    }
                }

                calcEq = calcEq.replaceAll("\\{" + sensor + "\\}", "(" + calcVal + ")");
            }

            Double qVal = 0.0;
            try {
                ScriptEngineManager mgr = new ScriptEngineManager();
                ScriptEngine engine = mgr.getEngineByName("JavaScript");
                qVal = (Double) engine.eval(calcEq);
            } catch(Exception e) {
                throw new HandledServiceException(411, "스크립트로 계산할 수 없음 [" + calcEq + "]");
            }

            resultSet.add(qVal);
        }

        if (resultSet.size() == 0)
            return currentEq.replaceAll(partCalc, "(0.0)");

        currentEq = currentEq.replaceAll(partCalc, "(" + Collections.max(resultSet) + ")");

        return currentEq;
    }

    private String calcMinFunc(ParamModuleVO paramModule, String partCalc, String currentEq) throws HandledServiceException {
        String partEq = partCalc.replaceAll("\\\\", "");
        partEq = partEq.substring("max(".length(), partEq.length() - ")".length());

        List<String> partSensors = ServerConstants.extractParams(partEq, "{", "}");
        if (partSensors == null || partSensors.size() == 0)
            throw new HandledServiceException(411, "추출된 센서 데이터가 없음.");

        Map<String, List<Object>> sensorData = new HashMap<>();
        int outBound = 0;

        for (String sensor : partSensors) {
            ParamModuleVO.Source partSource = paramModule.getParamData().get(sensor);
            if (partSource != null) {
                if (sensor.endsWith("_H") || sensor.contains("_H_")) {
                    sensorData.put(sensor, partSource.getHpfData());
                    outBound = Math.max(outBound, partSource.getHpfData().size());
                } else if (sensor.endsWith("_L") || sensor.contains("_L_")) {
                    sensorData.put(sensor, partSource.getLpfData());
                    outBound = Math.max(outBound, partSource.getLpfData().size());
                } else if (sensor.endsWith("_B") || sensor.contains("_B_")) {
                    sensorData.put(sensor, partSource.getBpfData());
                    outBound = Math.max(outBound, partSource.getBpfData().size());
                } else {
                    sensorData.put(sensor, partSource.getData());
                    outBound = Math.max(outBound, partSource.getData().size());
                }
            }
            else {
                ParamModuleVO.Equation equation = paramModule.getEqMap().get(sensor);
                if (equation != null) {
                    sensorData.put(sensor, equation.getData());
                    outBound = Math.max(outBound, equation.getData().size());
                }
            }
        }

        List<Double> resultSet = new ArrayList<>();
        for (int i = 0; i < outBound; i++) {
            String calcEq = partEq;
            for (String sensor : partSensors) {
                List<Object> data = sensorData.get(sensor);

                Double calcVal = 0.0;
                if (data.size() > i) {
                    try {
                        calcVal = Double.parseDouble(data.get(i) + "");
                    } catch(NumberFormatException nfe) {
                        throw new HandledServiceException(411, "문자를 계산에 사용할 수 없음.");
                    }
                }

                calcEq = calcEq.replaceAll("\\{" + sensor + "\\}", "(" + calcVal + ")");
            }

            Double qVal = 0.0;
            try {
                ScriptEngineManager mgr = new ScriptEngineManager();
                ScriptEngine engine = mgr.getEngineByName("JavaScript");
                qVal = (Double) engine.eval(calcEq);
            } catch(Exception e) {
                throw new HandledServiceException(411, "스크립트로 계산할 수 없음 [" + calcEq + "]");
            }

            resultSet.add(qVal);
        }

        if (resultSet.size() == 0)
            return currentEq.replaceAll(partCalc, "(0.0)");

        currentEq = currentEq.replaceAll(partCalc, "(" + Collections.min(resultSet) + ")");
        return currentEq;
    }

    private String calcAvgFunc(ParamModuleVO paramModule, String partCalc, String currentEq) throws HandledServiceException {
        String partEq = partCalc.replaceAll("\\\\", "");
        partEq = partEq.substring("max(".length(), partEq.length() - ")".length());

        List<String> partSensors = ServerConstants.extractParams(partEq, "{", "}");
        if (partSensors == null || partSensors.size() == 0)
            throw new HandledServiceException(411, "추출된 센서 데이터가 없음.");

        Map<String, List<Object>> sensorData = new HashMap<>();
        int outBound = 0;

        for (String sensor : partSensors) {
            ParamModuleVO.Source partSource = paramModule.getParamData().get(sensor);
            if (partSource != null) {
                if (sensor.endsWith("_H") || sensor.contains("_H_")) {
                    sensorData.put(sensor, partSource.getHpfData());
                    outBound = Math.max(outBound, partSource.getHpfData().size());
                } else if (sensor.endsWith("_L") || sensor.contains("_L_")) {
                    sensorData.put(sensor, partSource.getLpfData());
                    outBound = Math.max(outBound, partSource.getLpfData().size());
                } else if (sensor.endsWith("_B") || sensor.contains("_B_")) {
                    sensorData.put(sensor, partSource.getBpfData());
                    outBound = Math.max(outBound, partSource.getBpfData().size());
                } else {
                    sensorData.put(sensor, partSource.getData());
                    outBound = Math.max(outBound, partSource.getData().size());
                }
            }
            else {
                ParamModuleVO.Equation equation = paramModule.getEqMap().get(sensor);
                if (equation != null) {
                    sensorData.put(sensor, equation.getData());
                    outBound = Math.max(outBound, equation.getData().size());
                }
            }
        }

        List<Double> resultSet = new ArrayList<>();
        for (int i = 0; i < outBound; i++) {
            String calcEq = partEq;
            for (String sensor : partSensors) {
                List<Object> data = sensorData.get(sensor);

                Double calcVal = 0.0;
                if (data.size() > i) {
                    try {
                        calcVal = Double.parseDouble(data.get(i) + "");
                    } catch(NumberFormatException nfe) {
                        throw new HandledServiceException(411, "문자를 계산에 사용할 수 없음.");
                    }
                }

                calcEq = calcEq.replaceAll("\\{" + sensor + "\\}", "(" + calcVal + ")");
            }

            Double qVal = 0.0;
            try {
                ScriptEngineManager mgr = new ScriptEngineManager();
                ScriptEngine engine = mgr.getEngineByName("JavaScript");
                qVal = (Double) engine.eval(calcEq);
            } catch(Exception e) {
                throw new HandledServiceException(411, "스크립트로 계산할 수 없음 [" + calcEq + "]");
            }

            resultSet.add(qVal);
        }

        if (resultSet.size() == 0)
            return currentEq.replaceAll(partCalc, "(0.0)");

        OptionalDouble average = resultSet
                .stream()
                .mapToDouble(a -> a)
                .average();

        currentEq = currentEq.replaceAll(partCalc, "(" + (average.isPresent() ? average.getAsDouble() : 0) + ")");

        return currentEq;
    }

    private void loadPartData(ParamModuleVO.Source source) throws HandledServiceException {
        PartVO partInfo = ApiController.getService(PartService.class).getPartBySeq(source.getSourceSeq());
        if (partInfo == null) throw new HandledServiceException(411, "분할 정보 조회 실패");

        Long paramKey = source.getParamSeq().originOf();
        ParamVO param = ApiController.getService(ParamService.class).getPresetParamBySeq(new CryptoField(paramKey));
        if (param == null) {
            param = ApiController.getService(ParamService.class).getNotMappedParamBySeq(new CryptoField(paramKey));
            if (param == null) throw new HandledServiceException(411, "파라미터 조회 실패");
        }

        source.setParam(param);
        source.setSourceNo("P" + partInfo.getSeq().originOf());
        source.setParamKey(param.getParamKey());
        source.setSourceName(partInfo.getPartName());

        RawVO.Upload rawUpload = ApiController.getService(RawService.class).getUploadBySeq(partInfo.getUploadSeq());
        if (rawUpload != null) {
            source.setUseTime("julian");
            if (rawUpload.getDataType().equalsIgnoreCase("adams") || rawUpload.getDataType().equalsIgnoreCase("zaero"))
                source.setUseTime("offset");
        }

        String julianFrom = "", julianTo = "";
        Set<String> listSet = zsetOps.rangeByScore("P" + partInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);
        if (listSet != null && listSet.size() > 0)
            julianFrom = listSet.iterator().next();

        listSet = zsetOps.reverseRangeByScore("P" + partInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);
        if (listSet != null && listSet.size() > 0)
            julianTo = listSet.iterator().next();

        String julianStart = julianFrom;
        Long startRowAt = zsetOps.score("P" + partInfo.getSeq().originOf() + ".R", julianStart).longValue();

        Long rankFrom = zsetOps.rank("P" + partInfo.getSeq().originOf() + ".R", julianFrom);
        if (rankFrom == null) {
            julianFrom = zsetOps.rangeByScore("P" + partInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE).iterator().next();
            rankFrom = zsetOps.rank("P" + partInfo.getSeq().originOf() + ".R", julianFrom);
        }
        Long rankTo = zsetOps.rank("P" + partInfo.getSeq().originOf() + ".R", julianTo);
        if (rankTo == null) {
            julianTo = zsetOps.reverseRangeByScore("P" + partInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE).iterator().next();
            rankTo = zsetOps.rank("P" + partInfo.getSeq().originOf() + ".R", julianTo);
        }

        List<String> timeSet = new ArrayList<>();
        List<Object> data = new ArrayList<>();
        List<Object> lpfData = new ArrayList<>();
        List<Object> hpfData = new ArrayList<>();
        List<Object> bpfData = new ArrayList<>();

        listSet = zsetOps.rangeByScore(
                "P" + partInfo.getSeq().originOf() + ".N" +  + param.getReferenceSeq(), startRowAt + rankFrom, startRowAt + rankTo);

        Iterator<String> iterListSet = listSet.iterator();
        while (iterListSet.hasNext()) {
            String rowVal = iterListSet.next();
            String julianTime = rowVal.substring(0, rowVal.lastIndexOf(":"));
            timeSet.add(julianTime);
            Double dblVal = Double.parseDouble(rowVal.substring(rowVal.lastIndexOf(":") + 1));
            data.add(dblVal);
        }

        listSet = zsetOps.rangeByScore(
                "P" + partInfo.getSeq().originOf() + ".L" +  + param.getReferenceSeq(), startRowAt + rankFrom, startRowAt + rankTo);

        iterListSet = listSet.iterator();
        while (iterListSet.hasNext()) {
            String rowVal = iterListSet.next();
            Double dblVal = Double.parseDouble(rowVal.substring(rowVal.lastIndexOf(":") + 1));
            lpfData.add(dblVal);
        }

        listSet = zsetOps.rangeByScore(
                "P" + partInfo.getSeq().originOf() + ".H" +  + param.getReferenceSeq(), startRowAt + rankFrom, startRowAt + rankTo);

        iterListSet = listSet.iterator();
        while (iterListSet.hasNext()) {
            String rowVal = iterListSet.next();
            Double dblVal = Double.parseDouble(rowVal.substring(rowVal.lastIndexOf(":") + 1));
            hpfData.add(dblVal);
        }

        listSet = zsetOps.rangeByScore(
                "P" + partInfo.getSeq().originOf() + ".B" +  + param.getReferenceSeq(), startRowAt + rankFrom, startRowAt + rankTo);

        iterListSet = listSet.iterator();
        while (iterListSet.hasNext()) {
            String rowVal = iterListSet.next();
            Double dblVal = Double.parseDouble(rowVal.substring(rowVal.lastIndexOf(":") + 1));
            bpfData.add(dblVal);
        }

        source.setTimeSet(timeSet);
        source.setData(data);
        source.setLpfData(lpfData);
        source.setHpfData(hpfData);
        source.setBpfData(hpfData);

        source.setDataCount(data == null ? 0 : data.size());
    }

    private void loadShortBlockData(ParamModuleVO.Source source) throws HandledServiceException {
        ShortBlockVO blockInfo = ApiController.getService(PartService.class).getShortBlockBySeq(source.getSourceSeq());
        if (blockInfo == null) throw new HandledServiceException(411, "숏블록 정보 조회 실패");

        PartVO partInfo = ApiController.getService(PartService.class).getPartBySeq(blockInfo.getPartSeq());
        if (partInfo == null) throw new HandledServiceException(411, "분할 정보 조회 실패");

        Long paramKey = source.getParamSeq().originOf();
        ShortBlockVO.Param sbParam = ApiController.getService(ParamService.class).getShortBlockParamBySeq(new CryptoField(paramKey));
        if (sbParam == null)
            throw new HandledServiceException(411, "파라미터 조회 실패");

        ParamVO param = ApiController.getService(ParamService.class).getParamBySeq(sbParam.getParamSeq());
        param.setReferenceSeq(sbParam.getUnionParamSeq());

        source.setParam(param);
        source.setParamData(ApiController.getService(PartService.class).getShortBlockParamData(
                blockInfo.getBlockMetaSeq(), blockInfo.getSeq(), param.getReferenceSeq()));
        source.setSourceNo("S" + blockInfo.getSeq().originOf());
        source.setParamKey(param.getParamKey());
        source.setSourceName(blockInfo.getBlockName());

        RawVO.Upload rawUpload = ApiController.getService(RawService.class).getUploadBySeq(partInfo.getUploadSeq());
        if (rawUpload != null) {
            source.setUseTime("julian");
            if (rawUpload.getDataType().equalsIgnoreCase("adams") || rawUpload.getDataType().equalsIgnoreCase("zaero"))
                source.setUseTime("offset");
        }

        String julianFrom = "", julianTo = "";
        Set<String> listSet = zsetOps.rangeByScore("S" + blockInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);
        if (listSet != null && listSet.size() > 0)
            julianFrom = listSet.iterator().next();

        listSet = zsetOps.reverseRangeByScore("S" + blockInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);
        if (listSet != null && listSet.size() > 0)
            julianTo = listSet.iterator().next();

        String julianStart = julianFrom;
        Long startRowAt = zsetOps.score("S" + blockInfo.getSeq().originOf() + ".R", julianStart).longValue();

        Long rankFrom = zsetOps.rank("S" + blockInfo.getSeq().originOf() + ".R", julianFrom);
        if (rankFrom == null) {
            julianFrom = zsetOps.rangeByScore("S" + blockInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE).iterator().next();
            rankFrom = zsetOps.rank("S" + blockInfo.getSeq().originOf() + ".R", julianFrom);
        }
        Long rankTo = zsetOps.rank("S" + blockInfo.getSeq().originOf() + ".R", julianTo);
        if (rankTo == null) {
            julianTo = zsetOps.reverseRangeByScore("S" + blockInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE).iterator().next();
            rankTo = zsetOps.rank("S" + blockInfo.getSeq().originOf() + ".R", julianTo);
        }

        List<String> timeSet = new ArrayList<>();
        List<Object> data = new ArrayList<>();
        List<Object> lpfData = new ArrayList<>();
        List<Object> hpfData = new ArrayList<>();
        List<Object> bpfData = new ArrayList<>();

        listSet = zsetOps.rangeByScore(
                "S" + blockInfo.getSeq().originOf() + ".N" + param.getReferenceSeq(), startRowAt + rankFrom, startRowAt + rankTo);

        Iterator<String> iterListSet = listSet.iterator();
        while (iterListSet.hasNext()) {
            String rowVal = iterListSet.next();
            String julianTime = rowVal.substring(0, rowVal.lastIndexOf(":"));
            timeSet.add(julianTime);
            Double dblVal = Double.parseDouble(rowVal.substring(rowVal.lastIndexOf(":") + 1));
            data.add(dblVal);
        }

        listSet = zsetOps.rangeByScore(
                "S" + blockInfo.getSeq().originOf() + ".L" + param.getReferenceSeq(), startRowAt + rankFrom, startRowAt + rankTo);

        iterListSet = listSet.iterator();
        while (iterListSet.hasNext()) {
            String rowVal = iterListSet.next();
            Double dblVal = Double.parseDouble(rowVal.substring(rowVal.lastIndexOf(":") + 1));
            lpfData.add(dblVal);
        }

        listSet = zsetOps.rangeByScore(
                "S" + blockInfo.getSeq().originOf() + ".H" + param.getReferenceSeq(), startRowAt + rankFrom, startRowAt + rankTo);

        iterListSet = listSet.iterator();
        while (iterListSet.hasNext()) {
            String rowVal = iterListSet.next();
            Double dblVal = Double.parseDouble(rowVal.substring(rowVal.lastIndexOf(":") + 1));
            hpfData.add(dblVal);
        }

        listSet = zsetOps.rangeByScore(
                "S" + blockInfo.getSeq().originOf() + ".B" + param.getReferenceSeq(), startRowAt + rankFrom, startRowAt + rankTo);

        iterListSet = listSet.iterator();
        while (iterListSet.hasNext()) {
            String rowVal = iterListSet.next();
            Double dblVal = Double.parseDouble(rowVal.substring(rowVal.lastIndexOf(":") + 1));
            bpfData.add(dblVal);
        }

        source.setTimeSet(timeSet);
        source.setData(data);
        source.setLpfData(lpfData);
        source.setHpfData(hpfData);
        source.setBpfData(bpfData);

        source.setDataCount(data == null ? 0 : data.size());
    }

    private void loadBintableData(ParamModuleVO.Source source) throws HandledServiceException {
        BinTableVO binTableInfo = ApiController.getService(BinTableService.class).getBinTableBySeq(source.getSourceSeq());
        if (binTableInfo == null) throw new HandledServiceException(411, "BinTable 정보 조회 실패");

        Long paramKey = source.getParamSeq().originOf();
        ShortBlockVO.Param sbParam = ApiController.getService(ParamService.class).getShortBlockParamBySeq(new CryptoField(paramKey));
        if (sbParam == null)
            throw new HandledServiceException(411, "파라미터 조회 실패");

        ParamVO param = ApiController.getService(ParamService.class).getParamBySeq(sbParam.getParamSeq());
        param.setReferenceSeq(sbParam.getUnionParamSeq());

        source.setParam(param);
        source.setSourceNo("B" + binTableInfo.getSeq().originOf());
        source.setParamKey(param.getParamKey());
        source.setSourceName(binTableInfo.getMetaName());

        // bintable info setting. bin summary loading
        source.setBinSummaries(ApiController.getService(BinTableService.class).loadBinTableSummary(
                source.getSourceSeq(), sbParam.getSeq().valueOf()));
    }

    private void loadDllData(ParamModuleVO.Source source) throws HandledServiceException {
        DLLVO dllInfo = ApiController.getService(DLLService.class).getDLLBySeq(source.getSourceSeq());
        if (dllInfo == null) throw new HandledServiceException(411, "DLL 데이터 조회 실패");

        DLLVO.Param dllParam = ApiController.getService(DLLService.class).getDLLParamBySeq(source.getParamSeq());
        if (dllParam == null) throw new HandledServiceException(411, "DLL 파라미터 정보 조회 실패");

        source.setDllParam(dllParam);
        source.setSourceNo("D" + dllInfo.getSeq().originOf());
        source.setParamKey(dllParam.getParamName().originOf());
        source.setSourceName(dllInfo.getDataSetName());

        List<DLLVO.Raw> rawData = ApiController.getService(DLLService.class).getDLLData(dllInfo.getSeq(), dllParam.getSeq());
        List<Object> filteredData = new ArrayList<>();
        for (DLLVO.Raw dr : rawData) {
            if (dllParam.getParamType().equalsIgnoreCase("string"))
                filteredData.add(dr.getParamValStr());
            else
                filteredData.add(dr.getParamVal());
        }

        source.setData(filteredData);

        source.setDataCount(filteredData == null ? 0 : filteredData.size());
    }

    private void loadEquationData(ParamModuleVO.Source source) throws HandledServiceException {
        ParamModuleVO paramModule = ApiController.getService(ParamModuleService.class).getParamModuleBySeq(source.getSourceSeq());
        if (paramModule == null) throw new HandledServiceException(411, "파라미터 모듈 데이터 조회 실패");

        ParamModuleVO.Equation equation = ApiController.getService(ParamModuleService.class).getParamModuleEqBySeq(source.getParamSeq());
        if (equation == null) throw new HandledServiceException(411, "수식 데이터 조회 실패");

        source.setEquation(equation);
        if (equation.getEqNo() == null || equation.getEqNo().isEmpty())
            equation.setEqNo("E" + equation.getSeq().originOf());
        source.setSourceNo("M" + paramModule.getSeq().originOf());
        source.setParamKey(equation.getEqNo() + "_" + equation.getEqName().originOf());
        source.setSourceName(paramModule.getModuleName());

        // E<seq>.TimeSet = []
        // E<seq> = []
        String jsonData = hashOps.get("PM" + paramModule.getSeq().originOf(), "E" + equation.getSeq().originOf());
        if (jsonData == null || jsonData.isEmpty())
            throw new HandledServiceException(411, "수식 데이터가 없음");

        JsonArray jarrData = ServerConstants.GSON.fromJson(jsonData, JsonArray.class);
        List<Object> data = new ArrayList<>();
        List<Double[]> convhData = new ArrayList<>();

        for (int i = 0; i < jarrData.size(); i++) {
            if (jarrData.get(i).isJsonNull())
                data.add(null);
            else if (jarrData.get(i).isJsonPrimitive()) {
                if (jarrData.get(i).getAsJsonPrimitive().isString())
                    data.add(jarrData.get(i).getAsString());
                else
                    data.add(jarrData.get(i).getAsDouble());
            }
            else if (jarrData.get(i).isJsonArray()) {
                data.add(jarrData.get(i).getAsJsonArray());
                Double[] convhItem = new Double[] {
                        jarrData.get(i).getAsJsonArray().get(0).getAsDouble(),
                        jarrData.get(i).getAsJsonArray().get(1).getAsDouble()
                };
                convhData.add(convhItem);
            }
        }

        if (convhData != null && convhData.size() > 0)
            source.setData(new ArrayList<>());
        else
            source.setData(data);
        source.setConvhData(convhData);

        jsonData = hashOps.get("PM" + paramModule.getSeq().originOf(), "E" + equation.getSeq().originOf() + ".TimeSet");
        if (jsonData != null && !jsonData.isEmpty()) {
            jarrData = ServerConstants.GSON.fromJson(jsonData, JsonArray.class);
            List<String> timeSet = new ArrayList<>();
            for (int i = 0; i < jarrData.size(); i++)
                timeSet.add(jarrData.get(i).getAsString());

            source.setTimeSet(timeSet);
        }

        source.setDataCount(data == null ? 0 : data.size());
    }

    private void loadEquationData(ParamModuleVO.Equation equation) throws HandledServiceException {
        // E<seq>.TimeSet = []
        // E<seq> = []
        String jsonData = hashOps.get("PM" + equation.getModuleSeq().originOf(), "E" + equation.getSeq().originOf());
        JsonArray jarrData = null;

        if (jsonData == null || jsonData.isEmpty()) {
            equation.setData(new ArrayList<>());
            equation.setTimeSet(new ArrayList<>());
            equation.setDataCount(0);
            equation.setLazyLoad(true);
            return;
        }

        jarrData = ServerConstants.GSON.fromJson(jsonData, JsonArray.class);

        if (jarrData == null || jarrData.size() == 0) {
            equation.setData(new ArrayList<>());
            equation.setTimeSet(new ArrayList<>());
            equation.setDataCount(0);
            return;
        }

        List<Object> data = new ArrayList<>();
        List<Double[]> convhData = new ArrayList<>();

        for (int i = 0; i < jarrData.size(); i++) {
            if (jarrData.get(i).isJsonNull())
                data.add(null);
            else if (jarrData.get(i).isJsonPrimitive()) {
                if (jarrData.get(i).getAsJsonPrimitive().isString())
                    data.add(jarrData.get(i).getAsString());
                else
                    data.add(jarrData.get(i).getAsDouble());
            }
            else if (jarrData.get(i).isJsonArray()) {
                // for convh
                data.add(jarrData.get(i).getAsJsonArray());
                Double[] convhItem = new Double[] {
                        jarrData.get(i).getAsJsonArray().get(0).getAsDouble(),
                        jarrData.get(i).getAsJsonArray().get(1).getAsDouble()
                };
                convhData.add(convhItem);
            }
        }

        if (convhData != null && convhData.size() > 0)
            equation.setData(new ArrayList<>());
        else
            equation.setData(data);
        equation.setConvhData(convhData);

        if (equation.getEquation().contains("_time")) {
            List<String> timeSet = new ArrayList<>();
            for (Object time : data) {
                timeSet.add((String) time);
            }
            equation.setTimeSet(timeSet);

            hashOps.put("PM" + equation.getModuleSeq().originOf(), "E" + equation.getSeq().originOf() + ".TimeSet",
                    ServerConstants.GSON.toJson(equation.getTimeSet()));
        }
        else {
            jsonData = hashOps.get("PM" + equation.getModuleSeq().originOf(), "E" + equation.getSeq().originOf() + ".TimeSet");
            if (jsonData != null && !jsonData.isEmpty()) {
                jarrData = ServerConstants.GSON.fromJson(jsonData, JsonArray.class);
                List<String> timeSet = new ArrayList<>();
                for (int i = 0; i < jarrData.size(); i++)
                    timeSet.add(jarrData.get(i).getAsString());
                equation.setTimeSet(timeSet);
            }
        }

        equation.setDataCount(data == null ? 0 : data.size());
    }

    public boolean findConvexHullTime(ParamModuleVO paramModule, ParamModuleVO.Equation equation,
                                      String xValue, String yValue, JsonObject jobjOutput) throws HandledServiceException {
        String eq = equation.getEquation();

        eq = eq.replaceAll("\\\\", "");
        List<String> sensors = ServerConstants.extractParams(eq, "{", "}");

        // not match convh
        if (sensors != null && sensors.size() != 2) {
            jobjOutput.addProperty("message", "Convex Hull 형식이 맞지 않습니다.");
            return false;
        }

        // 센서 포함 수식. -> 계산 결과가 배열로 나옴.
        List<Object> src0 = null;
        List<Object> src1 = null;

        CryptoField src0PartSeq = null;
        CryptoField src0BlockSeq = null;
        CryptoField src0EqSeq = null;
        CryptoField src1PartSeq = null;
        CryptoField src1BlockSeq = null;
        CryptoField src1EqSeq = null;

        List<String> src0TimeSet = null;
        List<String> src1TimeSet = null;

        ParamModuleVO.Source partSource = paramModule.getParamData().get(sensors.get(0));
        if (partSource != null) {
            if (sensors.get(0).endsWith("_H") || sensors.get(0).contains("_H_")) {
                src0 = partSource.getHpfData();
            } else if (sensors.get(0).endsWith("_L") || sensors.get(0).contains("_L_")) {
                src0 = partSource.getLpfData();
            } else if (sensors.get(0).endsWith("_B") || sensors.get(0).contains("_B_")) {
                src0 = partSource.getBpfData();
            } else {
                src0 = partSource.getData();
            }
            if (partSource.getSourceType().equals("part"))
                src0PartSeq = partSource.getSourceSeq();
            else if (partSource.getSourceType().equals("shortblock"))
                src0BlockSeq = partSource.getSourceSeq();
            src0TimeSet = partSource.getTimeSet();
        } else {
            ParamModuleVO.Equation eqSource = paramModule.getEqMap().get(sensors.get(0));
            if (eqSource != null) {
                if (eqSource.getData() == null) {
                    loadEquationData(eqSource);
                }
                src0 = eqSource.getData();
                src0EqSeq = eqSource.getSeq();
                src0TimeSet = eqSource.getTimeSet();
            }
        }

        partSource = paramModule.getParamData().get(sensors.get(1));
        if (partSource != null) {
            if (sensors.get(1).endsWith("_H") || sensors.get(1).contains("_H_")) {
                src1 = partSource.getHpfData();
            } else if (sensors.get(1).endsWith("_L") || sensors.get(1).contains("_L_")) {
                src1 = partSource.getLpfData();
            } else if (sensors.get(1).endsWith("_B") || sensors.get(1).contains("_B_")) {
                src1 = partSource.getBpfData();
            } else {
                src1 = partSource.getData();
            }
            if (partSource.getSourceType().equals("part"))
                src1PartSeq = partSource.getSourceSeq();
            else if (partSource.getSourceType().equals("shortblock"))
                src1BlockSeq = partSource.getSourceSeq();
            src1TimeSet = partSource.getTimeSet();
        } else {
            ParamModuleVO.Equation eqSource = paramModule.getEqMap().get(sensors.get(1));
            if (eqSource != null) {
                if (eqSource.getData() == null) {
                    loadEquationData(eqSource);
                }
                src1 = eqSource.getData();
                src1EqSeq = eqSource.getSeq();
                src1TimeSet = eqSource.getTimeSet();
            }
        }

        if (src0 == null || src1 == null || src0.size() != src1.size()) {
            jobjOutput.addProperty("message", "ConvexHull 계산을 위해서는 데이터수가 일치해야합니다.");
            return false;
        }

        if (src0TimeSet == null || src1TimeSet == null || src0TimeSet.size() != src1TimeSet.size()) {
            jobjOutput.addProperty("message", "Convex Hull 타임테이블이 올바르지 않습니다.");
            return false;
        }

        double xVal = Double.parseDouble(xValue);
        double yVal = Double.parseDouble(yValue);
        List<String> timeSet = new ArrayList<>();

        for (int i = 0; i < src0.size(); i++) {
            double x = (Double) src0.get(i);
            double y = (Double) src1.get(i);
            if (xVal == x && yVal == y)
                timeSet.add(src0TimeSet.get(i));
        }

        jobjOutput.addProperty("partSeq", (src0PartSeq == null) ? "" : src0PartSeq.valueOf());
        jobjOutput.addProperty("blockSeq", (src0BlockSeq == null) ? "" : src0BlockSeq.valueOf());
        jobjOutput.addProperty("eqSeq", (src0EqSeq == null) ? "" : src0EqSeq.valueOf());
        jobjOutput.add("timeSet", ServerConstants.GSON.toJsonTree(timeSet));
        return true;
    }

    public boolean findCrossPlotTime(ParamModuleVO paramModule, ParamModuleVO.Equation equation,
                                      String xValue, String yValue, int pointIndex, JsonObject jobjOutput) throws HandledServiceException {
        String eq = equation.getEquation();

        eq = eq.trim();
        eq = eq.replaceAll("\\\\", "");

        if (!eq.startsWith("[") && !eq.endsWith("]")) {
            jobjOutput.addProperty("message", "수식 데이터가 조건에 맞지 않습니다. (Not Array Format)");
            return false;
        }

        eq = eq.substring(1, eq.length() - 1); // trim [, ]
        String[] eqSplit = eq.split(",");
        if (eqSplit == null || eqSplit.length == 0) {
            jobjOutput.addProperty("message", "수식 데이터가 조건에 맞지 않습니다. (No Data)");
            return false;
        }

        if (eqSplit.length <= pointIndex) {
            jobjOutput.addProperty("message", "수식 데이터가 조건에 맞지 않습니다. (IndexOutOfBound)");
            return false;
        }

        String targetEq = eqSplit[pointIndex];
        if (!targetEq.contains("min") && !targetEq.contains("max")) {
            jobjOutput.addProperty("message", "수식 데이터가 조건에 맞지 않습니다. (min, max function NotFound)");
            return false;
        }

        List<String> sensors = ServerConstants.extractParams(targetEq, "{", "}");
        if (sensors == null || sensors.size() != 1) {
            jobjOutput.addProperty("message", "수식 데이터가 조건에 맞지 않습니다. (NotSupport Multiple Sensors)");
            return false;
        }

        List<Object> src0 = null;

        CryptoField src0PartSeq = null;
        CryptoField src0BlockSeq = null;
        CryptoField src0EqSeq = null;

        List<String> src0TimeSet = null;

        ParamModuleVO.Source partSource = paramModule.getParamData().get(sensors.get(0));
        if (partSource != null) {
            if (sensors.get(0).endsWith("_H") || sensors.get(0).contains("_H_")) {
                src0 = partSource.getHpfData();
            } else if (sensors.get(0).endsWith("_L") || sensors.get(0).contains("_L_")) {
                src0 = partSource.getLpfData();
            } else if (sensors.get(0).endsWith("_B") || sensors.get(0).contains("_B_")) {
                src0 = partSource.getBpfData();
            } else {
                src0 = partSource.getData();
            }
            if (partSource.getSourceType().equals("part"))
                src0PartSeq = partSource.getSourceSeq();
            else if (partSource.getSourceType().equals("shortblock"))
                src0BlockSeq = partSource.getSourceSeq();
            src0TimeSet = partSource.getTimeSet();
        } else {
            ParamModuleVO.Equation eqSource = paramModule.getEqMap().get(sensors.get(0));
            if (eqSource != null) {
                if (eqSource.getData() == null) {
                    loadEquationData(eqSource);
                }
                src0 = eqSource.getData();
                src0EqSeq = eqSource.getSeq();
                src0TimeSet = eqSource.getTimeSet();
            }
        }

        if (src0 == null || src0.size() == 0 || src0TimeSet == null || src0TimeSet.size() == 0) {
            jobjOutput.addProperty("message", "수식 데이터가 조건에 맞지 않습니다. (No SourceData or TimeSet)");
            return false;
        }

        double findVal = Double.parseDouble(yValue);
        List<String> timeSet = new ArrayList<>();

        for (int i = 0; i < src0.size(); i++) {
            double v = (Double) src0.get(i);
            if (findVal == v)
                timeSet.add(src0TimeSet.get(i));
        }

        jobjOutput.addProperty("partSeq", (src0PartSeq == null) ? "" : src0PartSeq.valueOf());
        jobjOutput.addProperty("blockSeq", (src0BlockSeq == null) ? "" : src0BlockSeq.valueOf());
        jobjOutput.addProperty("eqSeq", (src0EqSeq == null) ? "" : src0EqSeq.valueOf());
        jobjOutput.add("timeSet", ServerConstants.GSON.toJsonTree(timeSet));
        return true;
    }

}
