package com.servetech.dynarap.db.mapper;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.vo.PartVO;
import org.apache.ibatis.annotations.*;

import java.util.List;
import java.util.Map;

@Mapper
public interface PartMapper {

    @Select({
            "<script>" +
                    "select * from dynarap_part " +
                    "where 1 = 1 " +
                    "<if test='@com.servetech.dynarap.db.type.MybatisEmptyChecker@isNotEmpty(registerUid)'>" +
                    " and registerUid = #{registerUid,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField_NAuth} " +
                    "</if>" +
                    "<if test='@com.servetech.dynarap.db.type.MybatisEmptyChecker@isNotEmpty(uploadSeq)'>" +
                    " and uploadSeq = #{uploadSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</if>" +
                    "order by partName asc, seq desc " +
                    "limit #{startIndex}, #{pageSize}" +
                    "</script>"
    })
    List<PartVO> selectPartList(Map<String, Object> params) throws Exception;


    @Select({
            "<script>" +
                    "select * from dynarap_part " +
                    "where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "limit 0, 1" +
                    "</script>"
    })
    PartVO selectPartBySeq(Map<String, Object> params) throws Exception;


    @Insert({
            "<script>" +
                    "insert into dynarap_part (" +
                    "uploadSeq,partName,presetPack,presetSeq,julianStartAt,julianEndAt," +
                    "offsetStartAt,offsetEndAt,registerUid" +
                    ") values (" +
                    "#{uploadSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{partName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64}" +
                    ",#{presetPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{presetSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{julianStartAt}" +
                    ",#{julianEndAt}" +
                    ",#{offsetStartAt}" +
                    ",#{offsetEndAt}" +
                    ",#{registerUid,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField_NAuth}" +
                    ")" +
                    "</script>"
    })
    @SelectKey(before = false, keyColumn = "seq", keyProperty = "seq",
            resultType = CryptoField.class,
            statement = "SELECT last_insert_id() FROM dynarap_part LIMIT 0, 1")
    void insertPart(PartVO part) throws Exception;

    @Update({
            "<script>" +
                    " update dynarap_part set " +
                    " partName = #{partName,} " +
                    ",julianStartAt = #{julianStartAt} " +
                    ",julianEndAt = #{julianEndAt} " +
                    ",offsetStartAt = #{offsetStartAt} " +
                    ",offsetEndAt = #{offsetEndAt} " +
                    " where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void updatePart(PartVO part) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_part " +
                    "where seq = #{partSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    "</script>"
    })
    void deletePartBySeq(Map<String, Object> params) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_part " +
                    "where uploadSeq = #{uploadSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    "</script>"
    })
    void deletePartByUploadSeq(Map<String, Object> params) throws Exception;


    @Select({
            "<script>" +
                    "select * from dynarap_part_raw " +
                    "where partSeq = #{partSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "<if test='@com.servetech.dynarap.db.type.MybatisEmptyChecker@isNotEmpty(presetParamSeq)'>" +
                    " and presetParamSeq = #{presetParamSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</if>" +
                    "<if test='@com.servetech.dynarap.db.type.MybatisEmptyChecker@isNotEmpty(rowNo)'>" +
                    " and rowNo = #{rowNo} " +
                    "</if>" +
                    "order by rowNo asc, presetParamSeq asc" +
                    "</script>"
    })
    List<PartVO.Raw> selectPartData(Map<String, Object> params) throws Exception;


    // insert for batch
    // update for batch

    @Delete({
            "<script>" +
                    "delete from dynarap_part_raw " +
                    "where partSeq = #{partSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "<if test='@com.servetech.dynarap.db.type.MybatisEmptyChecker@isNotEmpty(presetParamSeq)'>" +
                    " and presetParamSeq = #{presetParamSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</if>" +
                    "<if test='@com.servetech.dynarap.db.type.MybatisEmptyChecker@isNotEmpty(rowNo)'>" +
                    " and rowNo = #{rowNo} " +
                    "</if>" +
                    "</script>"
    })
    void deletePartData(Map<String, Object> params) throws Exception;


}
