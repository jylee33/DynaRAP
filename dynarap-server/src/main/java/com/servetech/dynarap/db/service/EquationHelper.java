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

    public void loadParamModuleData(CryptoField moduleSeq) throws HandledServiceException {
        ParamModuleVO paramModule = ApiController.getService(ParamModuleService.class).getParamModuleBySeq(moduleSeq);
        paramModule.setSources(ApiController.getService(ParamModuleService.class).getParamModuleSourceList(moduleSeq));
        if (paramModule.getSources() == null) paramModule.setSources(new ArrayList<>());

        for (ParamModuleVO.Source source : paramModule.getSources()) {
            if (source.getSourceType().equalsIgnoreCase("part")) {
                loadPartData(source);
                paramModule.getParamData().put("P_" + source.getParam().getParamKey(), source);
            }
            else if (source.getSourceType().equalsIgnoreCase("shortblock")) {
                loadShortBlockData(source);
                paramModule.getParamData().put("S_" + source.getParam().getParamKey(), source);
            }
            else if (source.getSourceType().equalsIgnoreCase("dll")) {
                loadDllData(source);
                paramModule.getParamData().put("D_" + source.getDllParam().getParamName().originOf(), source);
            }
            else if (source.getSourceType().equalsIgnoreCase("parammodule")) {
                loadEquationData(source);
                paramModule.getParamData().put("E_" + source.getEquation().getEqName().originOf(), source);
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

        paramModuleData.put(moduleSeq.valueOf(), paramModule);
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

            if (reCalculate) {
                tryCalculate(paramModule, equations.get(i));
                paramModule.getEqMap().put(equations.get(i).getEqName().originOf(), equations.get(i));
            }
        }
    }

    private void tryCalculate(ParamModuleVO paramModule, ParamModuleVO.Equation equation) throws HandledServiceException {
        String eq = equation.getEquation();

        // 싱글 파라미터 처리 (lrp xyz, shortblock psd,rms,n0)
        Set<String> paramSet = paramModule.getParamData().keySet();
        Iterator<String> iterParamSet = paramSet.iterator();
        while (iterParamSet.hasNext()) {
            String paramKey = iterParamSet.next();
            ParamModuleVO.Source paramSource = paramModule.getParamData().get(paramKey);

            // lrp 처리
            if (paramKey.startsWith("P_") || paramKey.startsWith("S_")) {
                if (paramSource.getParam() != null) {
                    eq = eq.replaceAll("\\$\\{" + paramKey + "_X\\}", "(" + paramSource.getParam().getLrpX() + ")");
                    eq = eq.replaceAll("\\$\\{" + paramKey + "_Y\\}", "(" + paramSource.getParam().getLrpY() + ")");
                    eq = eq.replaceAll("\\$\\{" + paramKey + "_Z\\}", "(" + paramSource.getParam().getLrpZ() + ")");
                }

                // shortblock 싱글 처리
                if (paramKey.startsWith("S_")) {
                    if (paramSource.getParamData() != null) {
                        eq = eq.replaceAll("\\$\\{" + paramKey + "_psd\\}", "(" + paramSource.getParamData().getPsd() + ")");
                        eq = eq.replaceAll("\\$\\{" + paramKey + "_rms\\}", "(" + paramSource.getParamData().getRms() + ")");
                        eq = eq.replaceAll("\\$\\{" + paramKey + "_n0\\}", "(" + paramSource.getParamData().getN0() + ")");
                    }
                }
            }
        }

        // 싱글 도출 함수 처리 (min,max,avg)
        if (eq.contains("max(") || eq.contains("min(") || eq.contains("avg(")) {
            while (eq.contains("max(") || eq.contains("min(") || eq.contains("avg(")) {

            }
        }

        // 배열 데이터 처리
        // 최종 결과값 저장 => 올 배열
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

        String julianFrom = "", julianTo = "";
        Set<String> listSet = zsetOps.rangeByScore("P" + partInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);
        if (listSet != null && listSet.size() > 0)
            julianFrom = listSet.iterator().next();

        listSet = zsetOps.reverseRangeByScore("P" + partInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);
        if (listSet != null && listSet.size() > 0)
            julianTo = listSet.iterator().next();

        String julianStart = listSet.iterator().next();
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
        List<Double> lpfData = new ArrayList<>();
        List<Double> hpfData = new ArrayList<>();

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
        ParamVO param = ApiController.getService(ParamService.class).getPresetParamBySeq(new CryptoField(paramKey));
        if (param == null) {
            param = ApiController.getService(ParamService.class).getNotMappedParamBySeq(new CryptoField(paramKey));
            if (param == null) throw new HandledServiceException(411, "파라미터 조회 실패");
        }
        source.setParam(param);
        source.setParamData(ApiController.getService(PartService.class).getShortBlockParamData(
                blockInfo.getBlockMetaSeq(), blockInfo.getSeq(), param.getReferenceSeq()));

        String julianFrom = "", julianTo = "";
        Set<String> listSet = zsetOps.rangeByScore("S" + blockInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);
        if (listSet != null && listSet.size() > 0)
            julianFrom = listSet.iterator().next();

        listSet = zsetOps.reverseRangeByScore("S" + blockInfo.getSeq().originOf() + ".R", 0, Integer.MAX_VALUE);
        if (listSet != null && listSet.size() > 0)
            julianTo = listSet.iterator().next();

        String julianStart = listSet.iterator().next();
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
        List<Double> lpfData = new ArrayList<>();
        List<Double> hpfData = new ArrayList<>();

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