package com.servetech.dynarap.db.mapper;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.vo.DLLVO;
import com.servetech.dynarap.vo.ParamModuleVO;
import com.servetech.dynarap.vo.PartVO;
import com.servetech.dynarap.vo.ShortBlockVO;
import org.apache.ibatis.annotations.*;

import java.util.List;
import java.util.Map;

@Mapper
public interface ParamModuleMapper {

    @Select({
            "<script>" +
                    "select * from dynarap_param_module " +
                    "where deleted = 0 " +
                    "order by createdAt desc " +
                    "</script>"
    })
    List<ParamModuleVO> selectParamModuleList(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select * from dynarap_param_module " +
                    "where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "and deleted = 0 " +
                    "order by createdAt desc " +
                    "</script>"
    })
    ParamModuleVO selectParamModuleBySeq(Map<String, Object> params) throws Exception;

    @Insert({
            "<script>" +
                    "insert into dynarap_param_module (" +
                    "moduleName,copyFromSeq,createdAt,referenced,deleted" +
                    ") values (" +
                    "#{moduleName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64}" +
                    ",#{copyFromSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{createdAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate}" +
                    ",#{referenced}" +
                    ",#{deleted}" +
                    ")" +
                    "</script>"
    })
    @SelectKey(before = false, keyColumn = "seq", keyProperty = "seq",
            resultType = CryptoField.class,
            statement = "SELECT last_insert_id() FROM dynarap_param_module LIMIT 0, 1")
    void insertParamModule(ParamModuleVO paramModule) throws Exception;

    @Update({
            "<script>" +
                    " update dynarap_param_module set " +
                    " moduleName = #{moduleName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64} " +
                    ",referenced = #{referenced} " +
                    ",deleted = #{deleted} " +
                    " where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void updateParamModule(ParamModuleVO paramModule) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_param_module " +
                    "where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void deleteParamModule(Map<String, Object> params) throws Exception;


    @Select({
            "<script>" +
                    "select * from dynarap_param_module_source " +
                    "where moduleSeq = #{moduleSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "order by seq asc" +
                    "</script>"
    })
    List<ParamModuleVO.Source> selectParamModuleSourceList(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select * from dynarap_param_module_source " +
                    "where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "limit 0, 1" +
                    "</script>"
    })
    ParamModuleVO.Source selectParamModuleSourceBySeq(Map<String, Object> params) throws Exception;

    @Insert({
            "<script>" +
                    "insert into dynarap_param_module_source (" +
                    "moduleSeq,sourceType,sourceSeq,paramPack,paramSeq," +
                    "julianStartAt,julianEndAt,offsetStartAt,offsetEndAt,dataCount" +
                    ") values (" +
                    "#{moduleSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{sourceType}" +
                    ",#{sourceSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{paramPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{paramSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{julianStartAt}" +
                    ",#{julianEndAt}" +
                    ",#{offsetStartAt}" +
                    ",#{offsetEndAt}" +
                    ",#{dataCount}" +
                    ")" +
                    "</script>"
    })
    @SelectKey(before = false, keyColumn = "seq", keyProperty = "seq",
            resultType = CryptoField.class,
            statement = "SELECT last_insert_id() FROM dynarap_param_module_source LIMIT 0, 1")
    void insertParamModuleSource(ParamModuleVO.Source paramModuleSource) throws Exception;

    @Update({
            "<script>" +
                    " update dynarap_param_module_source set " +
                    " julianStartAt = #{julianStartAt} " +
                    ",julianEndAt = #{julianEndAt} " +
                    ",offsetStartAt = #{offsetStartAt} " +
                    ",offsetEndAt = #{offsetEndAt} " +
                    ",dataCount = #{dataCount} " +
                    " where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void updateParamModuleSource(ParamModuleVO.Source paramModuleSource) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_param_module_source " +
                    "where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void deleteParamModuleSource(Map<String, Object> params) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_param_module_source " +
                    "where moduleSeq = #{moduleSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void deleteParamModuleSourceByModuleSeq(Map<String, Object> params) throws Exception;


    @Select({
            "<script>" +
                    "select * from dynarap_param_module_eq " +
                    "where moduleSeq = #{moduleSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "order by eqOrder asc " +
                    "</script>"
    })
    List<ParamModuleVO.Equation> selectParamModuleEqList(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select * from dynarap_param_module_eq " +
                    "where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "limit 0, 1 " +
                    "</script>"
    })
    ParamModuleVO.Equation selectParamModuleEqBySeq(Map<String, Object> params) throws Exception;

    @Insert({
            "<script>" +
                    "insert into dynarap_param_module_eq (" +
                    "moduleSeq,eqName,equation,eqOrder,julianStartAt,julianEndAt,offsetStartAt,offsetEndAt,dataCount" +
                    ") values (" +
                    "#{moduleSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{eqName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64}" +
                    ",#{equation}" +
                    ",#{eqOrder}" +
                    ",#{julianStartAt}" +
                    ",#{julianEndAt}" +
                    ",#{offsetStartAt}" +
                    ",#{offsetEndAt}" +
                    ",#{dataCount}" +
                    ")" +
                    "</script>"
    })
    @SelectKey(before = false, keyColumn = "seq", keyProperty = "seq",
            resultType = CryptoField.class,
            statement = "SELECT last_insert_id() FROM dynarap_param_module_eq LIMIT 0, 1")
    void insertParamModuleEq(ParamModuleVO.Equation equation) throws Exception;

    @Update({
            "<script>" +
                    " update dynarap_param_module_eq set " +
                    " eqName = #{eqName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64} " +
                    ",equation = #{equation} " +
                    ",eqOrder = #{eqOrder} " +
                    ",julianStartAt = #{julianStartAt} " +
                    ",julianEndAt = #{julianEndAt} " +
                    ",offsetStartAt = #{offsetStartAt} " +
                    ",offsetEndAt = #{offsetEndAt} " +
                    ",dataCount = #{dataCount} " +
                    " where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void updateParamModuleEq(ParamModuleVO.Equation equation) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_param_module_eq " +
                    "where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void deleteParamModuleEq(Map<String, Object> params) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_param_module_eq " +
                    "where moduleSeq = #{moduleSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void deleteParamModuleEqByModuleSeq(Map<String, Object> params) throws Exception;


    @Select({
            "<script>" +
                    "select * from dynarap_param_module_plot " +
                    "where moduleSeq = #{moduleSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "order by plotOrder asc" +
                    "</script>"
    })
    List<ParamModuleVO.Plot> selectParamModulePlotList(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select * from dynarap_param_module_plot " +
                    "where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "limit 0, 1" +
                    "</script>"
    })
    ParamModuleVO.Plot selectParamModulePlotBySeq(Map<String, Object> params) throws Exception;

    @Insert({
            "<script>" +
                    "insert into dynarap_param_module_plot (" +
                    "moduleSeq,plotName,plotType,plotOrder,createdAt" +
                    ") values (" +
                    "#{moduleSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{plotName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64}" +
                    ",#{plotType}" +
                    ",#{plotOrder}" +
                    ",#{createdAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate}" +
                    ")" +
                    "</script>"
    })
    @SelectKey(before = false, keyColumn = "seq", keyProperty = "seq",
            resultType = CryptoField.class,
            statement = "SELECT last_insert_id() FROM dynarap_param_module_plot LIMIT 0, 1")
    void insertParamModulePlot(ParamModuleVO.Plot plot) throws Exception;

    @Update({
            "<script>" +
                    " update dynarap_param_module_plot set " +
                    " plotName = #{plotName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64} " +
                    ",plotOrder = #{plotOrder} " +
                    ",plotType = #{plotType} " +
                    " where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void updateParamModulePlot(ParamModuleVO.Plot plot) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_param_module_plot " +
                    "where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void deleteParamModulePlot(Map<String, Object> params) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_param_module_plot " +
                    "where moduleSeq = #{moduleSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void deleteParamModulePlotByModuleSeq(Map<String, Object> params) throws Exception;


    @Select({
            "<script>" +
                    "select * from dynarap_param_module_plot_source " +
                    "where moduleSeq = #{moduleSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "and plotSeq = #{plotSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "order by seq asc" +
                    "</script>"
    })
    List<ParamModuleVO.Plot.Source> selectParamModulePlotSourceList(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select * from dynarap_param_module_plot_source " +
                    "where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "limit 0, 1" +
                    "</script>"
    })
    ParamModuleVO.Plot.Source selectParamModulePlotSourceBySeq(Map<String, Object> params) throws Exception;

    @Insert({
            "<script>" +
                    "insert into dynarap_param_module_plot_source (" +
                    "moduleSeq,plotSeq,sourceType,sourceSeq,paramPack,paramSeq," +
                    "julianStartAt,julianEndAt,offsetStartAt,offsetEndAt" +
                    ") values (" +
                    "#{moduleSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{plotSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{sourceType}" +
                    ",#{sourceSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{paramPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{paramSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{julianStartAt}" +
                    ",#{julianEndAt}" +
                    ",#{offsetStartAt}" +
                    ",#{offsetEndAt}" +
                    ")" +
                    "</script>"
    })
    @SelectKey(before = false, keyColumn = "seq", keyProperty = "seq",
            resultType = CryptoField.class,
            statement = "SELECT last_insert_id() FROM dynarap_param_module_plot_source LIMIT 0, 1")
    void insertParamModulePlotSource(ParamModuleVO.Plot.Source plotSource) throws Exception;

    @Update({
            "<script>" +
                    " update dynarap_param_module_plot_source set " +
                    " julianStartAt = #{julianStartAt} " +
                    ",julianEndAt = #{julianEndAt} " +
                    ",offsetStartAt = #{offsetStartAt} " +
                    ",offsetEndAt = #{offsetEndAt} " +
                    " where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void updateParamModulePlotSource(ParamModuleVO.Plot.Source plotSource) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_param_module_plot_source " +
                    "where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void deleteParamModulePlotSource(Map<String, Object> params) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_param_module_plot_source " +
                    "where moduleSeq = #{moduleSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "and plotSeq = #{plotSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    "</script>"
    })
    void deleteParamModulePlotSourceByPlotSeq(Map<String, Object> params) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_param_module_plot_source " +
                    "where moduleSeq = #{moduleSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void deleteParamModulePlotSourceByModuleSeq(Map<String, Object> params) throws Exception;




    @Select({
            "<script>" +
                    "select a.* from dynarap_part a " +
                    "where a.partName like concat('%', #{keyword}, '%') " +
                    "or a.seq in (" +
                    "       select seq from dynarap_data_prop " +
                    "       where referenceType = 'part' " +
                    "       and propName in ('tags', 'Tags') " +
                    "       and propValue like concat('%', #{keyword}, '%')) " +
                    "order by a.partName asc " +
                    "</script>"
    })
    List<PartVO> selectPartListByKeyword(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select a.* from dynarap_sblock a " +
                    "where a.blockName like concat('%', #{keyword}, '%') " +
                    "or a.seq in (" +
                    "       select seq from dynarap_data_prop " +
                    "       where referenceType = 'shortblock' " +
                    "       and propName in ('tags', 'Tags') " +
                    "       and propValue like concat('%', #{keyword}, '%')) " +
                    "order by a.blockName asc " +
                    "</script>"
    })
    List<ShortBlockVO> selectShortBlockListByKeyword(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select a.* from dynarap_dll a " +
                    "where a.dataSetName like concat('%', #{keyword}, '%') " +
                    "or a.seq in (" +
                    "       select seq from dynarap_data_prop " +
                    "       where referenceType = 'dll' " +
                    "       and propName in ('tags', 'Tags') " +
                    "       and propValue like concat('%', #{keyword}, '%')) " +
                    "order by a.dataSetName asc " +
                    "</script>"
    })
    List<DLLVO> selectDLLListByKeyword(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select a.* from dynarap_param_module a " +
                    "where a.moduleName like concat('%', #{keyword}, '%') " +
                    "or a.seq in (" +
                    "       select seq from dynarap_data_prop " +
                    "       where (referenceType = 'parammodule' and propName in ('tags', 'Tags') and propValue like concat('%', #{keyword}, '%')) " +
                    "       or (referenceType = 'eq' and propName in ('tags', 'Tags') and propValue like concat('%', #{keyword}, '%'))) " +
                    "order by a.moduleName asc " +
                    "</script>"
    })
    List<ParamModuleVO> selectParamModuleListByKeyword(Map<String, Object> params) throws Exception;

}
