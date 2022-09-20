package com.servetech.dynarap.db.mapper;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.vo.ParamVO;
import com.servetech.dynarap.vo.PresetVO;
import com.servetech.dynarap.vo.ShortBlockVO;
import org.apache.ibatis.annotations.*;

import java.util.List;
import java.util.Map;

@Mapper
public interface ParamMapper {

    @Select({
            "<script>" +
                    "select a.* from dynarap_param a " +
                    "where a.appliedEndAt = 0 " +
                    "<if test='@com.servetech.dynarap.db.type.MybatisEmptyChecker@isNotEmpty(keyword)'>" +
                    " and (paramKey like concat('%', #{keyword}, '%') " +
                    " or adamsKey like concat('%', #{keyword}, '%') " +
                    " or zaeroKey like concat('%', #{keyword}, '%') " +
                    " or grtKey like concat('%', #{keyword}, '%') " +
                    " or fltpKey like concat('%', #{keyword}, '%') " +
                    " or fltsKey like concat('%', #{keyword}, '%')) " +
                    "</if>" +
                    "order by a.appliedAt desc " +
                    "limit #{startIndex}, #{pageSize}" +
                    "</script>"
    })
    List<ParamVO> selectParamList(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select count(*) from dynarap_param a " +
                    "where a.appliedEndAt = 0 " +
                    "<if test='@com.servetech.dynarap.db.type.MybatisEmptyChecker@isNotEmpty(keyword)'>" +
                    " and (paramKey like concat('%', #{keyword}, '%') " +
                    " or adamsKey like concat('%', #{keyword}, '%') " +
                    " or zaeroKey like concat('%', #{keyword}, '%') " +
                    " or grtKey like concat('%', #{keyword}, '%') " +
                    " or fltpKey like concat('%', #{keyword}, '%') " +
                    " or fltsKey like concat('%', #{keyword}, '%')) " +
                    "</if>" +
                    "</script>"
    })
    int selectParamCount(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select a.* from dynarap_param a " +
                    "where a.appliedEndAt = 0 " +
                    "and a.propSeq = #{propSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "order by a.appliedAt desc " +
                    "limit #{startIndex}, #{pageSize}" +
                    "</script>"
    })
    List<ParamVO> selectParamListByProp(Map<String, Object> params) throws Exception;

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
                    "select seq from dynarap_preset_param " +
                    "where presetPack = #{presetPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "and presetSeq = #{presetSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "and paramPack = #{paramPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "and paramSeq = #{paramSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "union " +
                    "select seq from dynarap_notmapped_param " +
                    "where uploadSeq = #{uploadSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "and paramPack = #{paramPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "and paramSeq = #{paramSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "order by seq desc " +
                    "limit 0, 1" +
                    "</script>"
    })
    Long selectReferenceSeq(Map<String, Object> params) throws Exception;

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

    @Select({
            "<script>" +
                    "select * from dynarap_param " +
                    "where ${dataTypeKey} = #{paramKey} " +
                    "order by seq desc " +
                    "limit 0, 1" +
                    "</script>"
    })
    ParamVO selectParamByParamKey(Map<String, Object> params) throws Exception;

    @Insert({
            "<script>" +
                    "insert into dynarap_param (" +
                    "paramPack, propSeq, paramKey, " +
                    "adamsKey, zaeroKey, grtKey, fltpKey, fltsKey, " +
                    "partInfo, partInfoSub, lrpX, lrpY, lrpZ, tags, " +
                    "registerUid, appliedAt, appliedEndAt " +
                    ") values (" +
                    "#{paramPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{propSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{paramKey}" +
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
                    ",#{tags,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64}" +
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
                    ",propSeq = #{propSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    ",paramKey = #{paramKey} " +
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
                    ",tags = #{tags,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64} " +
                    ",appliedAt = #{appliedAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate} " +
                    ",appliedEndAt = #{appliedEndAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate} " +
                    " where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void updateParamNoRenew(ParamVO param) throws Exception;


    @Select({
            "<script>" +
                    "select * from dynarap_param_prop " +
                    "where deleted = 0 " +
                    "<if test='@com.servetech.dynarap.db.type.MybatisEmptyChecker@isNotEmpty(propType)'>" +
                    " and propType = #{propType} " +
                    "</if>" +
                    "order by propType asc, propCode asc " +
                    "</script>"
    })
    List<ParamVO.Prop> selectParamPropList(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select * from dynarap_param_prop " +
                    "where seq = #{propSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "limit 0, 1 " +
                    "</script>"
    })
    ParamVO.Prop selectParamPropBySeq(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select * from dynarap_param_prop " +
                    "where propType = #{propType} " +
                    "and propCode = #{propCode} " +
                    "order by createdAt desc " +
                    "limit 0, 1 " +
                    "</script>"
    })
    ParamVO.Prop selectParamPropByType(Map<String, Object> params) throws Exception;

    @Insert({
            "<script>" +
                    "insert into dynarap_param_prop (" +
                    "propType, propCode, paramUnit, registerUid, createdAt, deleted" +
                    ") values (" +
                    "#{propType}" +
                    ",#{propCode}" +
                    ",#{paramUnit}" +
                    ",#{registerUid,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField_NAuth}" +
                    ",#{createdAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate}" +
                    ",#{deleted}" +
                    ")" +
                    "</script>"
    })
    @SelectKey(before = false, keyColumn = "seq", keyProperty = "seq",
            resultType = CryptoField.class,
            statement = "SELECT last_insert_id() FROM dynarap_param_prop LIMIT 0, 1")
    void insertParamProp(ParamVO.Prop paramProp) throws Exception;

    @Update({
            "<script>" +
                    " update dynarap_param_prop set " +
                    " deleted = #{deleted} " +
                    ",paramUnit = #{paramUnit} " +
                    ",propType = #{propType} " +
                    ",propCode = #{propCode} " +
                    " where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void updateParamProp(ParamVO.Prop paramProp) throws Exception;


    @Select({
            "<script>" +
                    "select extraKey,extraValue from dynarap_param_extra " +
                    "where paramPack = #{paramPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "order by seq asc " +
                    "</script>"
    })
    List<Map<String, Object>> selectParamExtraList(Map<String, Object> params) throws Exception;

    @Insert({
            "<script>" +
                    "insert into dynarap_param_extra (" +
                    "paramPack,extraKey,extraValue" +
                    ") values (" +
                    "#{paramPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{extraKey}" +
                    ",#{extraValue}" +
                    ")" +
                    "</script>"
    })
    @SelectKey(before = false, keyColumn = "seq", keyProperty = "seq",
            resultType = CryptoField.class,
            statement = "SELECT last_insert_id() FROM dynarap_param_extra LIMIT 0, 1")
    void insertParamExtra(Map<String, Object> params) throws Exception;

    @Update({
            "<script>" +
                    " update dynarap_param_extra set " +
                    " extraValue = #{extraValue} " +
                    " where paramPack = #{paramPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    " and extraKey = #{extraKey} " +
                    "</script>"
    })
    void updateParamExtra(Map<String, Object> params) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_param_extra " +
                    "where paramPack = #{paramPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "and extraKey = #{extraKey}" +
                    "</script>"
    })
    void deleteParamExtra(Map<String, Object> params) throws Exception;

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
                    ",presetPackFrom = #{presetPackFrom,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",presetName = #{presetName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64} " +
                    ",appliedAt = #{appliedAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate} " +
                    ",appliedEndAt = #{appliedEndAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate} " +
                    " where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void updatePresetNoRenew(PresetVO preset) throws Exception;

    @Select({
            "<script>" +
                    "select c.*, a.presetPack, a.presetSeq, a.seq as referenceSeq " +
                    "from dynarap_preset_param a, dynarap_preset b, dynarap_param c " +
                    "where a.presetPack = b.presetPack and a.presetSeq = b.seq " +
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
                    "order by c.appliedAt desc " +
                    "limit #{startIndex}, #{pageSize}" +
                    "</script>"
    })
    List<ParamVO> selectPresetParamList(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select count(*) " +
                    "from dynarap_preset_param a, dynarap_preset b, dynarap_param c " +
                    "where a.presetPack = b.presetPack and a.presetSeq = b.seq  " +
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

    @Select({
            "<script>" +
                    "select c.*, a.seq as referenceSeq " +
                    "from dynarap_notmapped_param a, dynarap_param c " +
                    "where a.uploadSeq = #{uploadSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "and a.paramPack = c.paramPack " +
                    "and a.paramSeq = c.seq " +
                    "order by c.appliedAt desc " +
                    "</script>"
    })
    List<ParamVO> selectNotMappedParamList(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select a.*, b.seq as referenceSeq, b.presetPack, b.presetSeq from dynarap_param a, dynarap_preset_param b " +
                    "where a.seq = b.paramSeq " +
                    "and a.paramPack = b.paramPack " +
                    "and b.seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "limit 0, 1" +
                    "</script>"
    })
    ParamVO selectPresetParamBySeq(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select * from dynarap_preset_param " +
                    "where presetPack = #{presetPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "and presetSeq = #{presetSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "and paramPack = #{paramPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "and paramSeq = #{paramSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "order by seq desc " +
                    "limit 0, 1 " +
                    "</script>"
    })
    PresetVO.Param selectPresetParam(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select a.*, b.seq as referenceSeq, 0 as presetPack, 0 as presetSeq from dynarap_param a, dynarap_notmapped_param b " +
                    "where a.seq = b.paramSeq " +
                    "and a.paramPack = b.paramPack " +
                    "and b.seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "limit 0, 1" +
                    "</script>"
    })
    ParamVO selectNotMappedParamBySeq(Map<String, Object> params) throws Exception;

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


    @Select({
            "<script>" +
                    "select distinct presetParamSeq from dynarap_part_raw " +
                    "where partSeq = #{partSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    List<CryptoField> selectPartParamList(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select distinct blockParamSeq from dynarap_sblock_raw " +
                    "where blockSeq = #{blockSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    List<CryptoField> selectShortBlockParamList(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select * from dynarap_sblock_param " +
                    "where seq = #{blockParamSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "limit 0, 1" +
                    "</script>"
    })
    ShortBlockVO.Param selectShortBlockParamBySeq(Map<String, Object> params) throws Exception;


    @Select({
            "<script>" +
                    "select distinct * from dynarap_preset_param " +
                    "where paramPack = #{paramPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "and paramSeq = #{paramSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    List<PresetVO> selectPresetByParamPack(Map<String, Object> params) throws Exception;
}
