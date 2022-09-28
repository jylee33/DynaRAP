package com.servetech.dynarap.db.mapper;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.vo.BinTableVO;
import org.apache.ibatis.annotations.*;

import java.util.List;
import java.util.Map;

@Mapper
public interface BinTableMapper {

    @Select({
            "<script>" +
                    "select * from dynarap_bin_meta " +
                    "order by seq desc" +
                    "</script>"
    })
    List<BinTableVO> selectBinTableList() throws Exception;

    @Select({
            "<script>" +
                    "select * from dynarap_bin_meta " +
                    "where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "limit 0, 1" +
                    "</script>"
    })
    BinTableVO selectBinTableMetaBySeq(Map<String, Object> params) throws Exception;

    @Insert({
            "<script>" +
                    "insert into dynarap_bin_meta (metaName, createdAt) values (" +
                    "#{metaName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64}" +
                    ",#{createdAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate}" +
                    ")" +
                    "</script>"
    })
    @SelectKey(before = false, keyColumn = "seq", keyProperty = "seq",
            resultType = CryptoField.class,
            statement = "SELECT last_insert_id() FROM dynarap_bin_meta LIMIT 0, 1")
    void insertBinTableMeta(BinTableVO binTable) throws Exception;

    @Update({
            "<script>" +
                    " update dynarap_bin_meta set " +
                    " metaName = #{metaName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64} " +
                    " where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void updateBinTableMeta(BinTableVO binTable) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_bin_meta where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void deleteBinTableMeta(Map<String, Object> params) throws Exception;


    // bin data 담기
    @Select({
            "<script>" +
                    "select * from dynarap_bin_data " +
                    "where dataFrom = #{dataFrom} " +
                    "and binMetaSeq = #{binMetaSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "order by seq asc " +
                    "</script>"
    })
    List<BinTableVO.BinData> selectBinTableDataList(Map<String, Object> params) throws Exception;

    @Insert({
            "<script>" +
                    "insert into dynarap_bin_data (" +
                    "binMetaSeq,dataFrom,refSeq" +
                    ") values (" +
                    "#{binMetaSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{dataFrom}" +
                    ",#{refSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ")" +
                    "</script>"
    })
    @SelectKey(before = false, keyColumn = "seq", keyProperty = "seq",
            resultType = CryptoField.class,
            statement = "SELECT last_insert_id() FROM dynarap_bin_data LIMIT 0, 1")
    void insertBinTableData(BinTableVO.BinData binData) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_bin_data where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void deleteBinTableData(Map<String, Object> params) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_bin_data where binMetaSeq = #{binMetaSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void deleteBinTableDataByMeta(Map<String, Object> params) throws Exception;


    // 파라미터 정보
    @Select({
            "<script>" +
                    "select * from dynarap_bin_param " +
                    "where binMetaSeq = #{binMetaSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "order by seq asc " +
                    "</script>"
    })
    List<BinTableVO.BinParam> selectBinTableParamList(Map<String, Object> params) throws Exception;

    @Insert({
            "<script>" +
                    "insert into dynarap_bin_param (" +
                    "binMetaSeq,paramSeq,paramPack,fieldType,fieldPropSeq," +
                    "paramKey,adamsKey,zaeroKey,grtKey,fltpKey,fltsKey" +
                    ") values (" +
                    "#{binMetaSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{paramSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{paramPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{fieldType}" +
                    ",#{fieldPropSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{paramKey}" +
                    ",#{adamsKey}" +
                    ",#{zaeroKey}" +
                    ",#{grtKey}" +
                    ",#{fltpKey}" +
                    ",#{fltsKey}" +
                    ")" +
                    "</script>"
    })
    @SelectKey(before = false, keyColumn = "seq", keyProperty = "seq",
            resultType = CryptoField.class,
            statement = "SELECT last_insert_id() FROM dynarap_bin_param LIMIT 0, 1")
    void insertBinTableParam(BinTableVO.BinParam binParam) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_bin_param where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void deleteBinTableParam(Map<String, Object> params) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_bin_param where binMetaSeq = #{binMetaSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void deleteBinTableParamByMeta(Map<String, Object> params) throws Exception;


    // param data
    @Select({
            "<script>" +
                    "select * from dynarap_bin_param_data " +
                    "where binMetaSeq = #{binMetaSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "and paramSeq = #{paramSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "order by dataNominal asc " +
                    "</script>"
    })
    List<BinTableVO.BinParam.BinParamData> selectBinTableParamData(Map<String, Object> params) throws Exception;

    @Insert({
            "<script>" +
                    "insert into dynarap_bin_param_data (" +
                    "binMetaSeq,paramSeq,dataNominal,dataMin,dataMax" +
                    ") values (" +
                    "#{binMetaSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{paramSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{dataNominal}" +
                    ",#{dataMin}" +
                    ",#{dataMax}" +
                    ")" +
                    "</script>"
    })
    @SelectKey(before = false, keyColumn = "seq", keyProperty = "seq",
            resultType = CryptoField.class,
            statement = "SELECT last_insert_id() FROM dynarap_bin_param_data LIMIT 0, 1")
    void insertBinTableParamData(BinTableVO.BinParam.BinParamData binParamData) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_bin_param_data where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void deleteBinTableParamData(Map<String, Object> params) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_bin_param_data where binMetaSeq = #{binMetaSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void deleteBinTableParamDataByMeta(Map<String, Object> params) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_bin_param_data " +
                    "where binMetaSeq = #{binMetaSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "and paramSeq = #{paramSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    "</script>"
    })
    void deleteBinTableParamDataByParam(Map<String, Object> params) throws Exception;



}
