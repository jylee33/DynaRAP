package com.servetech.dynarap.db.mapper;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.vo.DLLVO;
import com.servetech.dynarap.vo.FlightVO;
import org.apache.ibatis.annotations.*;

import java.util.List;
import java.util.Map;

@Mapper
public interface DLLMapper {

    /* ============================================== DLL 관리 */
    @Select({
            "<script>" +
                    "select * from dynarap_dll " +
//                    "order by dataSetCode asc" +
                    "order by seq desc " +
                    "</script>"
    })
    List<DLLVO> selectDLLList() throws Exception;

    @Insert({
            "<script>" +
                    "insert into dynarap_dll (" +
                    "dataSetCode,dataSetName,dataVersion,createdAt,registerUid,tags" +
                    ") values (" +
                    "#{dataSetCode}" +
                    ",#{dataSetName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64}" +
                    ",#{dataVersion}" +
                    ",#{createdAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate}" +
                    ",#{registerUid,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField_NAuth}" +
                    ",#{tags,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64}" +
                    ")" +
                    "</script>"
    })
    @SelectKey(before = false, keyColumn = "seq", keyProperty = "seq",
            resultType = CryptoField.class,
            statement = "SELECT last_insert_id() FROM dynarap_dll LIMIT 0, 1")
    void insertDLL(DLLVO dll) throws Exception;

    @Update({
            "<script>" +
                    " update dynarap_dll set " +
                    " dataSetName=#{dataSetName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64} " +
                    ",dataVersion=#{dataVersion} " +
                    ",tags=#{tags,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64}" +
                    " where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void updateDLL(DLLVO dll) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_dll " +
                    "where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void deleteDLL(Map<String, Object> params) throws Exception;


    /* ============================================== DLL parameter 관리 */
    @Select({
            "<script>" +
                    "select * from dynarap_dll_param " +
                    "where dllSeq = #{dllSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "order by paramNo asc, seq asc" +
                    "</script>"
    })
    List<DLLVO.Param> selectDLLParamList(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select * from dynarap_dll_param " +
                    "where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "limit 0, 1" +
                    "</script>"
    })
    DLLVO.Param selectDLLParamBySeq(Map<String, Object> params) throws Exception;

    @Insert({
            "<script>" +
                    "insert into dynarap_dll_param (" +
                    "dllSeq,paramName,paramType,paramNo,registerUid" +
                    ") values (" +
                    "#{dllSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{paramName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64}" +
                    ",#{paramType}" +
                    ",#{paramNo}" +
                    ",#{registerUid,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField_NAuth}" +
                    ")" +
                    "</script>"
    })
    @SelectKey(before = false, keyColumn = "seq", keyProperty = "seq",
            resultType = CryptoField.class,
            statement = "SELECT last_insert_id() FROM dynarap_dll_param LIMIT 0, 1")
    void insertDLLParam(DLLVO.Param dllParam) throws Exception;

    @Update({
            "<script>" +
                    " update dynarap_dll_param set " +
                    " paramName=#{paramName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64}" +
                    ",paramType=#{paramType}" +
                    ",paramNo=#{paramNo}" +
                    " where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    "</script>"
    })
    void updateDLLParam(DLLVO.Param dllParam) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_dll_param " +
                    "where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    "</script>"
    })
    void deleteDLLParam(Map<String, Object> params) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_dll_param " +
                    "where dllSeq = #{dllSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    "</script>"
    })
    void deleteDLLParamByMulti(Map<String, Object> params) throws Exception;


    /* ============================================== DLL parameter data 관리 */
    // 데이터를 로딩하여 메모리에서 정렬하고 반환함.
    @Select({
            "<script>" +
                    "select a.* from dynarap_dll_raw a, dynarap_dll_param b " +
                    "where a.dllSeq = b.dllSeq " +
                    "and a.paramSeq = b.seq " +
                    "and a.dllSeq = #{dllSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "<if test='@com.servetech.dynarap.db.type.MybatisEmptyChecker@isNotEmpty(paramSeq)'>" +
                    "and a.paramSeq = #{paramSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</if>" +
                    "order by b.paramNo asc, a.rowNo asc" +
                    "</script>"
    })
    List<DLLVO.Raw> selectDLLData(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select ifnull(max(a.rowNo),0) from dynarap_dll_raw a, dynarap_dll_param b " +
                    "where a.dllSeq = b.dllSeq " +
                    "and a.paramSeq = b.seq " +
                    "and a.dllSeq = #{dllSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "<if test='@com.servetech.dynarap.db.type.MybatisEmptyChecker@isNotEmpty(paramSeq)'>" +
                    "and a.paramSeq = #{paramSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</if>" +
                    "</script>"
    })
    int selectDLLDataMaxRow(Map<String, Object> params) throws Exception;

    @Insert({
            "<script>" +
                    "insert into dynarap_dll_raw (" +
                    "dllSeq,paramSeq,paramVal,paramValStr,rowNo" +
                    ") values (" +
                    "#{dllSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{paramSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{paramVal}" +
                    ",#{paramValStr}" +
                    ",#{rowNo}" +
                    ")" +
                    "</script>"
    })
    @SelectKey(before = false, keyColumn = "seq", keyProperty = "seq",
            resultType = CryptoField.class,
            statement = "SELECT last_insert_id() FROM dynarap_dll_raw LIMIT 0, 1")
    void insertDLLData(DLLVO.Raw dllRaw) throws Exception;

    @Update({
            "<script>" +
                    " update dynarap_dll_raw set " +
                    " paramVal=#{paramVal}" +
                    ",paramValStr=#{paramValStr}" +
                    ",rowNo=#{rowNo}" +
                    " where paramSeq = #{paramSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    " and rowNo = #{rowNo} " +
                    "</script>"
    })
    void updateDLLData(DLLVO.Raw dllRaw) throws Exception;

    @Update({
            "<script>" +
                    " update dynarap_dll_raw set " +
                    " paramVal=#{paramVal}" +
                    ",paramValStr=#{paramValStr}" +
                    ",rowNo=#{rowNo}" +
                    " where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void updateDLLDataBySeq(DLLVO.Raw dllRaw) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_dll_raw " +
                    "where dllSeq = #{dllSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    "</script>"
    })
    void deleteDLLData(Map<String, Object> params) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_dll_raw " +
                    "where dllSeq = #{dllSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "and rowNo = #{rowNo} " +
                    "</script>"
    })
    void deleteDLLDataByRow(Map<String, Object> params) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_dll_raw " +
                    "where dllSeq = #{dllSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "and rowNo between #{fromRowNo} and #{toRowNo} " +
                    "</script>"
    })
    void deleteDLLDataByRowNo(Map<String, Object> params) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_dll_raw " +
                    "where dllSeq = #{dllSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "<if test='@com.servetech.dynarap.db.type.MybatisEmptyChecker@isNotEmpty(paramSeq)'>" +
                    "and paramSeq = #{paramSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</if>" +
                    "</script>"
    })
    void deleteDLLDataByParam(Map<String, Object> params) throws Exception;

}
