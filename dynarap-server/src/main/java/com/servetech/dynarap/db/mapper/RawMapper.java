package com.servetech.dynarap.db.mapper;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.vo.FlightVO;
import com.servetech.dynarap.vo.RawVO;
import org.apache.ibatis.annotations.*;

import java.util.List;
import java.util.Map;

@Mapper
public interface RawMapper {

    @Select({
            "<script>" +
                    "select * from dynarap_raw " +
                    "where uploadSeq = #{uploadSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "order by rowNo asc, presetParamSeq asc" +
                    "</script>"
    })
    List<RawVO> selectRawData(Map<String, Object> params) throws Exception;

    @Insert({
            "<script>" +
                    "insert into dynarap_raw (" +
                    "uploadSeq,presetPack,presetSeq,presetParamSeq,rowNo,paramVal,paramValStr" +
                    ") values (" +
                    "#{uploadSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{presetPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{presetSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{presetParamSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{rowNo}" +
                    ",#{paramVal}" +
                    ",#{paramValStr}" +
                    ")" +
                    "</script>"
    })
    @SelectKey(before = false, keyColumn = "seq", keyProperty = "seq",
            resultType = CryptoField.class,
            statement = "SELECT last_insert_id() FROM dynarap_raw LIMIT 0, 1")
    void insertRawData(RawVO raw) throws Exception;

    @Insert({
            "<script>" +
                    "insert ignore into dynarap_raw (" +
                    "presetPack,presetSeq,presetParamSeq,julianTimeAt,paramVal,paramValStr,rowNo,uploadSeq" +
                    ") values " +
                    "<foreach item='row' collection='list' open='' separator=',' close=''>" +
                    "(#{row.presetPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{row.presetSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{row.presetParamSeq}" +
                    ",#{row.julianTimeAt}" +
                    ",#{row.paramVal}" +
                    ",#{row.paramValStr}" +
                    ",#{row.rowNo}" +
                    ",#{row.uploadSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ")" +
                    "</foreach>" +
                    "</script>"
    })
    void insertRawDataBulk(@Param("list") List<Map<String, Object>> list) throws Exception;

    @Update({
            "<script>" +
                    " update dynarap_raw set " +
                    " paramVal = #{paramVal}" +
                    ",paramValStr = #{paramValStr}" +
                    " where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    "</script>"
    })
    void updateRawData(RawVO raw) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_raw " +
                    "where uploadSeq = #{uploadSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "<if test='@com.servetech.dynarap.db.type.MybatisEmptyChecker@isNotEmpty(presetParamSeq)'>" +
                    "and presetParamSeq = #{presetParamSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</if>" +
                    "</script>"
    })
    void deleteRawDataByParam(Map<String, Object> params) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_raw " +
                    "where uploadSeq = #{uploadSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "and rowNo = #{rowNo} " +
                    "</script>"
    })
    void deleteRawDataByRow(Map<String, Object> params) throws Exception;



    @Select({
            "<script>" +
                    "select * from dynarap_raw_upload " +
                    "order by seq desc" +
                    "</script>"
    })
    List<RawVO.Upload> selectRawUploadList() throws Exception;

    @Select({
            "<script>" +
                    "select * from dynarap_raw_upload " +
                    "where seq = #{uploadSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "limit 0, 1" +
                    "</script>"
    })
    RawVO.Upload selectRawUploadBySeq(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select * from dynarap_raw_upload " +
                    "where uploadId = #{uploadId} " +
                    "limit 0, 1" +
                    "</script>"
    })
    RawVO.Upload selectRawUploadById(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select * from dynarap_raw_upload " +
                    "where uploadName = #{uploadName} " +
                    "and fileSize = #{fileSize} " +
                    "limit 0, 1" +
                    "</script>"
    })
    RawVO.Upload selectRawUploadByParam(Map<String, Object> params) throws Exception;

    @Insert({
            "<script>" +
                    "insert into dynarap_raw_upload (" +
                    "uploadName,dataType,storePath,fileSize,flightSeq,presetPack,presetSeq,uploadedAt,flightAt,registerUid,uploadId,importDone" +
                    ") values (" +
                    "#{uploadName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64}" +
                    ",#{dataType}" +
                    ",#{storePath}" +
                    ",#{fileSize}" +
                    ",#{flightSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{presetPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{presetSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{uploadedAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate}" +
                    ",#{flightAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate}" +
                    ",#{registerUid,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField_NAuth}" +
                    ",#{uploadId}" +
                    ",#{importDone}" +
                    ")" +
                    "</script>"
    })
    @SelectKey(before = false, keyColumn = "seq", keyProperty = "seq",
            resultType = CryptoField.class,
            statement = "SELECT last_insert_id() FROM dynarap_raw_upload LIMIT 0, 1")
    void insertRawUpload(RawVO.Upload rawUpload) throws Exception;

    @Update({
            "<script>" +
                    " update dynarap_raw_upload set " +
                    " storePath = #{storePath} " +
                    ",dataType = #{dataType} " +
                    ",flightSeq = #{flightSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    ",presetPack = #{presetPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    ",presetSeq = #{presetSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    ",flightAt = #{flightAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate} " +
                    ",importDone = #{importDone} " +
                    " where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void updateRawUpload(RawVO.Upload rawUpload) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_raw_upload " +
                    "where seq = #{uploadSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void deleteRawUpload(Map<String, Object> params) throws Exception;

}
