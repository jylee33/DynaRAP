package com.servetech.dynarap.db.mapper;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.ext.HandledServiceException;
import com.servetech.dynarap.vo.ParamVO;
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
    List<ParamVO> selectParamList(Map<String, Object> params) throws HandledServiceException;

    @Select({
            "<script>" +
                    "select a.* from dynarap_param a " +
                    "where a.appliedEndAt = 0 " +
                    "and a.paramGroupSeq = #{paramGroupSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "order by a.paramName asc, a.appliedAt desc " +
                    "limit #{startIndex}, #{pageSize}" +
                    "</script>"
    })
    List<ParamVO> selectParamListByGroup(Map<String, Object> params) throws HandledServiceException;

    @Select({
            "<script>" +
                    "select * from dynarap_param " +
                    "where paramPack = #{paramPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "and appliedEndAt = 0 " +
                    "order by appliedAt desc " +
                    "limit 0, 1" +
                    "</script>"
    })
    ParamVO selectActiveParam(Map<String, Object> params) throws HandledServiceException;

    @Select({
            "<script>" +
                    "select * from dynarap_param " +
                    "where paramPack = #{paramPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "order by appliedAt desc " +
                    "</script>"
    })
    List<ParamVO> selectParamPackList(Map<String, Object> params) throws HandledServiceException;

    @Select({
            "<script>" +
                    "select * from dynarap_param " +
                    "where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "limit 0, 1" +
                    "</script>"
    })
    ParamVO selectParamBySeq(Map<String, Object> params) throws HandledServiceException;

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
    void insertParam(ParamVO param) throws HandledServiceException;

    @Update({
            "<script>" +
                    " update dynarap_param set " +
                    " appliedAt = #{appliedAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate} " +
                    ",appliedEndAt = #{appliedEndAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate} " +
                    " where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    "</script>"
    })
    void updateParam(ParamVO param) throws HandledServiceException;

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
    void updateParamNoRenew(ParamVO param) throws HandledServiceException;


    @Select({
            "<script>" +
                    "select * from dynarap_param_group " +
                    "order by groupName asc, groupType asc " +
                    "</script>"
    })
    List<ParamVO.Group> selectParamGroupList() throws HandledServiceException;

    @Select({
            "<script>" +
                    "select * from dynarap_param_group " +
                    "where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "limit 0, 1 " +
                    "</script>"
    })
    ParamVO.Group selectParamGroupBySeq(Map<String, Object> params) throws  HandledServiceException;

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
    void insertParamGroup(ParamVO.Group paramGroup) throws HandledServiceException;

    @Update({
            "<script>" +
                    " update dynarap_param_group set " +
                    " groupName = #{groupName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64} " +
                    ",groupType = #{groupType} " +
                    " where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void updateParamGroup(ParamVO.Group paramGroup) throws HandledServiceException;


}
