package com.servetech.dynarap.db.mapper;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.ext.HandledServiceException;
import com.servetech.dynarap.vo.ParamVO;
import com.servetech.dynarap.vo.PresetVO;
import org.apache.ibatis.annotations.*;

import java.util.List;
import java.util.Map;

@Mapper
public interface ParamMapper {

    @Select({
            "<script>" +
                    "select a.* from dynarap_param a " +
                    "where a.appliedEndAt = 0 " +
                    "order by a.paramName asc, a.appliedAt desc " +
                    "limit #{startIndex}, #{pageSize}" +
                    "</script>"
    })
    List<ParamVO> selectParamList(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select count(*) from dynarap_param a " +
                    "where a.appliedEndAt = 0 " +
                    "</script>"
    })
    int selectParamCount(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select a.* from dynarap_param a " +
                    "where a.appliedEndAt = 0 " +
                    "and a.paramGroupSeq = #{paramGroupSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "order by a.paramName asc, a.appliedAt desc " +
                    "limit #{startIndex}, #{pageSize}" +
                    "</script>"
    })
    List<ParamVO> selectParamListByGroup(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select * from dynarap_param " +
                    "where paramPack = #{paramPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "and appliedEndAt = 0 " +
                    "order by appliedAt desc " +
                    "limit 0, 1" +
                    "</script>"
    })
    ParamVO selectActiveParam(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select * from dynarap_param " +
                    "where paramPack = #{paramPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "order by appliedAt desc " +
                    "</script>"
    })
    List<ParamVO> selectParamPackList(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select * from dynarap_param " +
                    "where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "limit 0, 1" +
                    "</script>"
    })
    ParamVO selectParamBySeq(Map<String, Object> params) throws Exception;

    @Insert({
            "<script>" +
                    "insert into dynarap_param (" +
                    "paramPack, paramGroupSeq, paramKey, paramName, paramSpec, " +
                    "adamsKey, zaeroKey, grtKey, fltpKey, fltsKey, " +
                    "partInfo, partInfoSub, lrpX, lrpY, lrpZ, paramUnit, " +
                    "domainMin, domainMax, specified, paramVal, registerUid, appliedAt, appliedEndAt " +
                    ") values (" +
                    "#{paramPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{paramGroupSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{paramKey}" +
                    ",#{paramName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64}" +
                    ",#{paramSpec}" +
                    ",#{adamsKey}" +
                    ",#{zaeroKey}" +
                    ",#{grtKey}" +
                    ",#{fltpKey}" +
                    ",#{fltsKey}" +
                    ",#{partInfo}" +
                    ",#{partInfoSub}" +
                    ",#{lrpX}" +
                    ",#{lrpY}" +
                    ",#{lrpZ}" +
                    ",#{paramUnit}" +
                    ",#{domainMin}" +
                    ",#{domainMax}" +
                    ",#{specified}" +
                    ",#{paramVal}" +
                    ",#{registerUid,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField_NAuth}" +
                    ",#{appliedAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate}" +
                    ",#{appliedEndAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate}" +
                    ")" +
                    "</script>"
    })
    @SelectKey(before = false, keyColumn = "seq", keyProperty = "seq",
            resultType = CryptoField.class,
            statement = "SELECT last_insert_id() FROM dynarap_param LIMIT 0, 1")
    void insertParam(ParamVO param) throws Exception;

    @Update({
            "<script>" +
                    " update dynarap_param set " +
                    " appliedAt = #{appliedAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate} " +
                    ",appliedEndAt = #{appliedEndAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate} " +
                    " where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    "</script>"
    })
    void updateParam(ParamVO param) throws Exception;

    @Update({
            "<script>" +
                    " update dynarap_param set " +
                    " paramPack = #{paramPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    ",paramGroupSeq = #{paramGroupSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    ",paramKey = #{paramKey} " +
                    ",paramName = #{paramName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64} " +
                    ",paramSpec = #{paramSpec} " +
                    ",adamsKey = #{adamsKey} " +
                    ",zaeroKey = #{zaeroKey} " +
                    ",grtKey = #{grtKey} " +
                    ",fltpKey = #{fltpKey} " +
                    ",fltsKey = #{fltsKey} " +
                    ",partInfo = #{partInfo} " +
                    ",partInfoSub = #{partInfoSub} " +
                    ",lrpX = #{lrpX} " +
                    ",lrpY = #{lrpY} " +
                    ",lrpZ = #{lrpZ} " +
                    ",paramUnit = #{paramUnit} " +
                    ",domainMin = #{domainMin} " +
                    ",domainMax = #{domainMax} " +
                    ",specified = #{specified} " +
                    ",paramVal = #{paramVal} " +
                    ",appliedAt = #{appliedAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate} " +
                    ",appliedEndAt = #{appliedEndAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate} " +
                    " where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void updateParamNoRenew(ParamVO param) throws Exception;


    @Select({
            "<script>" +
                    "select * from dynarap_param_group " +
                    "order by groupName asc, groupType asc " +
                    "</script>"
    })
    List<ParamVO.Group> selectParamGroupList() throws Exception;

    @Select({
            "<script>" +
                    "select * from dynarap_param_group " +
                    "where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "limit 0, 1 " +
                    "</script>"
    })
    ParamVO.Group selectParamGroupBySeq(Map<String, Object> params) throws  Exception;

    @Insert({
            "<script>" +
                    "insert into dynarap_param_group (" +
                    "groupName, groupType, registerUid, createdAt" +
                    ") values (" +
                    "#{groupName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64}" +
                    ",#{groupType}" +
                    ",#{registerUid,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField_NAuth}" +
                    ",#{createdAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate}" +
                    ")" +
                    "</script>"
    })
    @SelectKey(before = false, keyColumn = "seq", keyProperty = "seq",
            resultType = CryptoField.class,
            statement = "SELECT last_insert_id() FROM dynarap_param_group LIMIT 0, 1")
    void insertParamGroup(ParamVO.Group paramGroup) throws Exception;

    @Update({
            "<script>" +
                    " update dynarap_param_group set " +
                    " groupName = #{groupName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64} " +
                    ",groupType = #{groupType} " +
                    " where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void updateParamGroup(ParamVO.Group paramGroup) throws Exception;


    @Select({
            "<script>" +
                    "select * from dynarap_preset " +
                    "where appliedEndAt = 0 " +
                    "order by presetName asc, appliedAt desc " +
                    "limit #{startIndex}, #{pageSize}" +
                    "</script>"
    })
    List<PresetVO> selectPresetList(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select count(*) from dynarap_preset " +
                    "where appliedEndAt = 0 " +
                    "</script>"
    })
    int selectPresetCount(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select * from dynarap_preset " +
                    "where presetPack = #{presetPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "order by appliedAt desc " +
                    "</script>"
    })
    List<PresetVO> selectPresetPackList(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select * from dynarap_preset " +
                    "where presetPack = #{presetPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "and appliedEndAt = 0 " +
                    "limit 0, 1" +
                    "</script>"
    })
    PresetVO selectActivePreset(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select * from dynarap_preset " +
                    "where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "limit 0, 1" +
                    "</script>"
    })
    PresetVO selectPresetBySeq(Map<String, Object> params) throws Exception;

    @Insert({
            "<script>" +
                    "insert into dynarap_preset (" +
                    "presetPack,presetName,presetPackFrom,createdAt,registerUid,appliedAt,appliedEndAt" +
                    ") values (" +
                    "#{presetPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{presetName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64}" +
                    ",#{presetPackFrom,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{createdAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate}" +
                    ",#{registerUid,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField_NAuth}" +
                    ",#{appliedAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate}" +
                    ",#{appliedEndAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate}" +
                    ")" +
                    "</script>"
    })
    @SelectKey(before = false, keyColumn = "seq", keyProperty = "seq",
            resultType = CryptoField.class,
            statement = "SELECT last_insert_id() FROM dynarap_preset LIMIT 0, 1")
    void insertPreset(PresetVO preset) throws Exception;

    @Update({
            "<script>" +
                    " update dynarap_preset set " +
                    " appliedAt = #{appliedAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate} " +
                    ",appliedEndAt = #{appliedEndAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate} " +
                    " where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    "</script>"
    })
    void updatePreset(PresetVO preset) throws Exception;

    @Update({
            "<script>" +
                    " update dynarap_preset set " +
                    " presetPack = #{presetPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    ",presetName = #{presetName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64} " +
                    ",appliedAt = #{appliedAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate} " +
                    ",appliedEndAt = #{appliedEndAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate} " +
                    " where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void updatePresetNoRenew(PresetVO preset) throws Exception;

    @Select({
            "<script>" +
                    "select c.*, a.presetPack, a.presetSeq, a.seq as presetParamSeq from dynarap_preset_param a, dynarap_preset b, dynarap_param c " +
                    "where a.presetPack = b.presetPack " +
                    "and b.presetPack = #{presetPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "<choose> " +
                    "<when test='@com.servetech.dynarap.db.type.MybatisEmptyChecker@isNotEmpty(presetSeq)'> " +
                    "and b.seq = #{presetSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</when> " +
                    "<otherwise> " +
                    "and b.appliedEndAt = 0 " +
                    "</otherwise> " +
                    "</choose> " +
                    "and a.paramPack = c.paramPack " +
                    "<choose> " +
                    "<when test='@com.servetech.dynarap.db.type.MybatisEmptyChecker@isNotEmpty(paramPack)'>" +
                    "and c.paramPack = #{paramPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</when> " +
                    "<when test='@com.servetech.dynarap.db.type.MybatisEmptyChecker@isNotEmpty(paramSeq)'> " +
                    "and c.seq = #{paramSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</when> " +
                    "<otherwise> " +
                    "and c.appliedEndAt = 0 " +
                    "</otherwise> " +
                    "</choose> " +
                    "order by c.paramName asc, c.appliedAt desc " +
                    "limit #{startIndex}, #{pageSize}" +
                    "</script>"
    })
    List<ParamVO> selectPresetParamList(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select count(*) from dynarap_preset_param a, dynarap_preset b, dynarap_param c " +
                    "where a.presetPack = b.presetPack " +
                    "and b.presetPack = #{presetPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "<choose> " +
                    "<when test='@com.servetech.dynarap.db.type.MybatisEmptyChecker@isNotEmpty(presetSeq)'> " +
                    "and b.seq = #{presetSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</when> " +
                    "<otherwise> " +
                    "and b.appliedEndAt = 0 " +
                    "</otherwise> " +
                    "</choose> " +
                    "and a.paramPack = c.paramPack " +
                    "<choose> " +
                    "<when test='@com.servetech.dynarap.db.type.MybatisEmptyChecker@isNotEmpty(paramPack)'>" +
                    "and c.paramPack = #{paramPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</when> " +
                    "<when test='@com.servetech.dynarap.db.type.MybatisEmptyChecker@isNotEmpty(paramSeq)'> " +
                    "and c.seq = #{paramSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</when> " +
                    "<otherwise> " +
                    "and c.appliedEndAt = 0 " +
                    "</otherwise> " +
                    "</choose> " +
                    "</script>"
    })
    int selectPresetParamCount(Map<String, Object> params) throws Exception;

    @Insert({
            "<script>" +
                    "insert into dynarap_preset_param (" +
                    "presetPack, presetSeq, paramPack, paramSeq" +
                    ") values (" +
                    "#{presetPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{presetSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{paramPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{paramSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ")" +
                    "</script>"
    })
    void insertPresetParam(PresetVO.Param presetParam) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_preset_param " +
                    "where presetPack = #{presetPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "and presetSeq = #{presetSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "and paramPack = #{paramPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "and paramSeq = #{paramSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void deletePresetParam(Map<String, Object> params) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_preset_param " +
                    "where presetPack = #{presetPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "and presetSeq = #{presetSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void deleteAllPresetParam(Map<String, Object> params) throws Exception;


}
