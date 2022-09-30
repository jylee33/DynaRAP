package com.servetech.dynarap.db.service;

import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.google.gson.JsonPrimitive;
import com.servetech.dynarap.config.ServerConstants;
import com.servetech.dynarap.controller.ApiController;
import com.servetech.dynarap.db.type.CryptoField;
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
        ParamModuleVO paramModule = paramModuleData.get(moduleSeq.valueOf());

        if (paramModule == null) {
            paramModule = ApiController.getService(ParamModuleService.class).getParamModuleBySeq(moduleSeq);
            paramModule.setSources(ApiController.getService(ParamModuleService.class).getParamModuleSourceList(moduleSeq));
            if (paramModule.getSources() == null) paramModule.setSources(new ArrayList<>());

            paramModule.setParamData(new HashMap<>());

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
                    paramModule.getParamData().put(source.getSourceNo() + "_" + source.getEquation().getEqName().originOf(), source);
                }
            }

            paramModule.setEquations(ApiController.getService(ParamModuleService.class).getParamModuleEqList(moduleSeq));
            if (paramModule.getEquations() == null) paramModule.setEquations(new ArrayList<>());

            for (ParamModuleVO.Equation equation : paramModule.getEquations()) {
                loadEquationData(equation);
                if (paramModule.getEqMap() == null)
                    paramModule.setEqMap(new HashMap<>());
                paramModule.getEqMap().put(equation.getEqName().originOf(), equation);
            }
        }

        paramModuleData.put(moduleSeq.valueOf(), paramModule);
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
        ParamModuleVO paramModule = paramModuleData.get(moduleSeq.valueOf());
        if (paramModule == null) {
            loadParamModuleData(moduleSeq);
            paramModule = paramModuleData.get(moduleSeq.valueOf());
        }

        for (int i = 0; i < equations.size(); i++) {
            ParamModuleVO.Equation oldEquation = null;
            if (i < paramModule.getEquations().size())
                oldEquation = paramModule.getEquations().get(i);

            boolean reCalculate = false;
            if (oldEquation == null || !oldEquation.getEquation().equals(equations.get(i).getEquation()))
                reCalculate = true;

            reCalculate = true;

            if (reCalculate) {
                tryCalculate(paramModule, equations.get(i));
                paramModule.getEqMap().put(equations.get(i).getEqName().originOf(), equations.get(i));
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
        // 싱글 파라미터 처리 (lrp xyz, shortblock psd,rms,n0)
        Set<String> paramSet = paramModule.getParamData().keySet();
        Iterator<String> iterParamSet = paramSet.iterator();
        while (iterParamSet.hasNext()) {
            String paramKey = iterParamSet.next();
            ParamModuleVO.Source paramSource = paramModule.getParamData().get(paramKey);

            // lrp 처리
            if (paramKey.startsWith("P") || paramKey.startsWith("S")) {
                if (paramSource.getParam() != null) {
                    eq = eq.replaceAll("\\{" + paramKey + "_X\\}", "(" + paramSource.getParam().getLrpX() + ")");
                    eq = eq.replaceAll("\\{" + paramKey + "_Y\\}", "(" + paramSource.getParam().getLrpY() + ")");
                    eq = eq.replaceAll("\\{" + paramKey + "_Z\\}", "(" + paramSource.getParam().getLrpZ() + ")");
                }

                // shortblock 싱글 처리
                if (paramKey.startsWith("S")) {
                    if (paramSource.getParamData() != null) {
                        eq = eq.replaceAll("\\{" + paramKey + "_psd\\}", "(" + paramSource.getParamData().getPsd() + ")");
                        eq = eq.replaceAll("\\{" + paramKey + "_rms\\}", "(" + paramSource.getParamData().getRms() + ")");
                        eq = eq.replaceAll("\\{" + paramKey + "_n0\\}", "(" + paramSource.getParamData().getN0() + ")");
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
                if (sensor.contains("_hpf")) {
                    sensorData.put(sensor, partSource.getHpfData());
                    outBound = Math.max(outBound, partSource.getHpfData().size());
                }
                else if (sensor.contains("_lpf")) {
                    sensorData.put(sensor, partSource.getLpfData());
                    outBound = Math.max(outBound, partSource.getLpfData().size());
                }
                else {
                    sensorData.put(sensor, partSource.getData());
                    outBound = Math.max(outBound, partSource.getData().size());
                }
            }

            for (int i = 0; i < outBound; i++) {
                String calcEq = eq;
                for (String sensor : sensors) {
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
            if (sensor.contains("_hpf")) {
                sensorData.put(sensor, partSource.getHpfData());
                outBound = Math.max(outBound, partSource.getHpfData().size());
            }
            else if (sensor.contains("_lpf")) {
                sensorData.put(sensor, partSource.getLpfData());
                outBound = Math.max(outBound, partSource.getLpfData().size());
            }
            else {
                sensorData.put(sensor, partSource.getData());
                outBound = Math.max(outBound, partSource.getData().size());
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
            if (sensor.contains("_hpf")) {
                sensorData.put(sensor, partSource.getHpfData());
                outBound = Math.max(outBound, partSource.getHpfData().size());
            }
            else if (sensor.contains("_lpf")) {
                sensorData.put(sensor, partSource.getLpfData());
                outBound = Math.max(outBound, partSource.getLpfData().size());
            }
            else {
                sensorData.put(sensor, partSource.getData());
                outBound = Math.max(outBound, partSource.getData().size());
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
            if (sensor.contains("_hpf")) {
                sensorData.put(sensor, partSource.getHpfData());
                outBound = Math.max(outBound, partSource.getHpfData().size());
            }
            else if (sensor.contains("_lpf")) {
                sensorData.put(sensor, partSource.getLpfData());
                outBound = Math.max(outBound, partSource.getLpfData().size());
            }
            else {
                sensorData.put(sensor, partSource.getData());
                outBound = Math.max(outBound, partSource.getData().size());
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

        source.setTimeSet(timeSet);
        source.setData(data);
        source.setLpfData(lpfData);
        source.setHpfData(hpfData);
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

        source.setTimeSet(timeSet);
        source.setData(data);
        source.setLpfData(lpfData);
        source.setHpfData(hpfData);
    }

    private void loadDllData(ParamModuleVO.Source source) throws HandledServiceException {
        DLLVO dllInfo = ApiController.getService(DLLService.class).getDLLBySeq(source.getSourceSeq());
        if (dllInfo == null) throw new HandledServiceException(411, "DLL 데이터 조회 실패");

        DLLVO.Param dllParam = ApiController.getService(DLLService.class).getDLLParamBySeq(source.getParamSeq());
        if (dllParam == null) throw new HandledServiceException(411, "DLL 파라미터 정보 조회 실패");

        source.setDllParam(dllParam);
        source.setSourceNo("D" + dllInfo.getSeq().originOf());

        List<DLLVO.Raw> rawData = ApiController.getService(DLLService.class).getDLLData(dllInfo.getSeq(), dllParam.getSeq());
        List<Object> filteredData = new ArrayList<>();
        for (DLLVO.Raw dr : rawData) {
            if (dllParam.getParamType().equalsIgnoreCase("string"))
                filteredData.add(dr.getParamValStr());
            else
                filteredData.add(dr.getParamVal());
        }

        source.setData(filteredData);
    }

    private void loadEquationData(ParamModuleVO.Source source) throws HandledServiceException {
        ParamModuleVO paramModule = ApiController.getService(ParamModuleService.class).getParamModuleBySeq(source.getSourceSeq());
        if (paramModule == null) throw new HandledServiceException(411, "파라미터 모듈 데이터 조회 실패");

        ParamModuleVO.Equation equation = ApiController.getService(ParamModuleService.class).getParamModuleEqBySeq(source.getParamSeq());
        if (equation == null) throw new HandledServiceException(411, "수식 데이터 조회 실패");

        source.setEquation(equation);
        source.setSourceNo("M" + paramModule.getSeq().originOf());

        // E<seq>.TimeSet = []
        // E<seq> = []
        String jsonData = hashOps.get("PM" + paramModule.getSeq().originOf(), "E" + equation.getSeq().originOf());
        if (jsonData == null || jsonData.isEmpty())
            throw new HandledServiceException(411, "수식 데이터가 없음");

        JsonArray jarrData = ServerConstants.GSON.fromJson(jsonData, JsonArray.class);
        List<Object> data = new ArrayList<>();

        for (int i = 0; i < jarrData.size(); i++) {
            if (jarrData.get(i).isJsonNull())
                data.add(null);
            else if (jarrData.get(i).isJsonPrimitive()) {
                if (jarrData.get(i).getAsJsonPrimitive().isString())
                    data.add(jarrData.get(i).getAsString());
                else
                    data.add(jarrData.get(i).getAsDouble());
            }
        }

        source.setData(data);

        jsonData = hashOps.get("PM" + paramModule.getSeq().originOf(), "E" + equation.getSeq().originOf() + ".TimeSet");
        if (jsonData != null && !jsonData.isEmpty()) {
            jarrData = ServerConstants.GSON.fromJson(jsonData, JsonArray.class);
            List<String> timeSet = new ArrayList<>();
            for (int i = 0; i < jarrData.size(); i++)
                timeSet.add(jarrData.get(i).getAsString());

            source.setTimeSet(timeSet);
        }
    }

    private void loadEquationData(ParamModuleVO.Equation equation) throws HandledServiceException {
        // E<seq>.TimeSet = []
        // E<seq> = []
        String jsonData = hashOps.get("PM" + equation.getModuleSeq().originOf(), "E" + equation.getSeq().originOf());
        if (jsonData == null || jsonData.isEmpty()) {
            equation.setData(new ArrayList<>());
            equation.setTimeSet(new ArrayList<>());
            return;
        }

        JsonArray jarrData = ServerConstants.GSON.fromJson(jsonData, JsonArray.class);
        List<Object> data = new ArrayList<>();

        for (int i = 0; i < jarrData.size(); i++) {
            if (jarrData.get(i).isJsonNull())
                data.add(null);
            else if (jarrData.get(i).isJsonPrimitive()) {
                if (jarrData.get(i).getAsJsonPrimitive().isString())
                    data.add(jarrData.get(i).getAsString());
                else
                    data.add(jarrData.get(i).getAsDouble());
            }
        }

        equation.setData(data);

        jsonData = hashOps.get("PM" + equation.getModuleSeq().originOf(), "E" + equation.getSeq().originOf() + ".TimeSet");
        if (jsonData != null && !jsonData.isEmpty()) {
            jarrData = ServerConstants.GSON.fromJson(jsonData, JsonArray.class);
            List<String> timeSet = new ArrayList<>();
            for (int i = 0; i < jarrData.size(); i++)
                timeSet.add(jarrData.get(i).getAsString());

            equation.setTimeSet(timeSet);
        }
    }
}