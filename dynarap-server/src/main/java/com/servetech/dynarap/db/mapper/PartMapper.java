package com.servetech.dynarap.db.mapper;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.vo.PartVO;
import com.servetech.dynarap.vo.ShortBlockVO;
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
                    "select count(*) from dynarap_part " +
                    "where 1 = 1 " +
                    "<if test='@com.servetech.dynarap.db.type.MybatisEmptyChecker@isNotEmpty(registerUid)'>" +
                    " and registerUid = #{registerUid,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField_NAuth} " +
                    "</if>" +
                    "<if test='@com.servetech.dynarap.db.type.MybatisEmptyChecker@isNotEmpty(uploadSeq)'>" +
                    " and uploadSeq = #{uploadSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</if>" +
                    "</script>"
    })
    int selectPartCount(Map<String, Object> params) throws Exception;

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



    @Select({
            "<script>" +
                    "select * from dynarap_sblock_meta " +
                    "where partSeq = #{partSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "order by createdAt desc" +
                    "</script>"
    })
    List<ShortBlockVO.Meta> selectShortBlockMetaList(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select * from dynarap_sblock_meta " +
                    "where seq = #{metaSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "limit 0, 1" +
                    "</script>"
    })
    ShortBlockVO.Meta selectShortBlockMetaBySeq(Map<String, Object> params) throws Exception;

    @Insert({
            "<script>" +
                    "insert into dynarap_sblock_meta (" +
                    "partSeq,overlap,sliceTime,registerUid,createdAt,selectedPresetPack,selectedPresetSeq,createDone" +
                    ") values (" +
                    "#{partSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{overlap}" +
                    ",#{sliceTime}" +
                    ",#{registerUid,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField_NAuth}" +
                    ",#{createdAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate}" +
                    ",#{selectedPresetPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{selectedPresetSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{createDone}" +
                    ")" +
                    "</script>"
    })
    @SelectKey(before = false, keyColumn = "seq", keyProperty = "seq",
            resultType = CryptoField.class,
            statement = "SELECT last_insert_id() FROM dynarap_sblock_meta LIMIT 0, 1")
    void insertShortBlockMeta(ShortBlockVO.Meta shortBlockMeta) throws Exception;

    @Update({
            "<script>" +
                    " update dynarap_sblock_meta set " +
                    " overlap = #{overlap}" +
                    ",sliceTime = #{sliceTime}" +
                    ",selectedPresetPack = #{selectedPresetPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",selectedPresetSeq = #{selectedPresetSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",createDone = #{createDone}" +
                    " where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void updateShortBlockMeta(ShortBlockVO.Meta shortBlockMeta) throws Exception;

    @Select({
            "<script>" +
                    "select * from dynarap_sblock_param " +
                    "where blockMetaSeq = #{blockMetaSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "order by paramNo asc" +
                    "</script>"
    })
    List<ShortBlockVO.Param> selectShortBlockParamList(Map<String, Object> params) throws Exception;

    @Insert({
            "<script>" +
                    "insert into dynarap_sblock_param (" +
                    "blockMetaSeq,paramNo,paramPack,paramSeq,paramKey,paramName,adamsKey,zaeroKey,grtKey,fltpKey,fltsKey,paramUnit" +
                    ") values (" +
                    "#{blockMetaSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{paramNo}" +
                    ",#{paramPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{paramSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{paramKey}" +
                    ",#{paramName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64}" +
                    ",#{adamsKey}" +
                    ",#{zaeroKey}" +
                    ",#{grtKey}" +
                    ",#{fltpKey}" +
                    ",#{fltsKey}" +
                    ",#{paramUnit}" +
                    ")" +
                    "</script>"
    })
    @SelectKey(before = false, keyColumn = "seq", keyProperty = "seq",
            resultType = CryptoField.class,
            statement = "SELECT last_insert_id() FROM dynarap_sblock_param LIMIT 0, 1")
    void insertShortBlockParam(ShortBlockVO.Param shortBlockParam) throws Exception;

    @Update({
            "<script>" +
                    " update dynarap_sblock_param set " +
                    " paramNo = #{paramNo}" +
                    ",paramPack = #{paramPack,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",paramSeq = #{paramSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",paramKey = #{paramKey}" +
                    ",paramName = #{paramName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64}" +
                    ",adamsKey = #{adamsKey}" +
                    ",zaeroKey = #{zaeroKey}" +
                    ",grtKey = #{grtKey}" +
                    ",fltpKey = #{fltpKey}" +
                    ",fltsKey = #{fltsKey}" +
                    ",paramUnit = #{paramUnit} " +
                    " where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    "</script>"
    })
    void updateShortBlockParma(ShortBlockVO.Param shortBlockParam) throws Exception;


    @Select({
            "<script>" +
                    "select * from dynarap_sblock " +
                    "where 1 = 1 " +
                    "<if test='@com.servetech.dynarap.db.type.MybatisEmptyChecker@isNotEmpty(registerUid)'>" +
                    " and registerUid = #{registerUid,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField_NAuth} " +
                    "</if>" +
                    "<if test='@com.servetech.dynarap.db.type.MybatisEmptyChecker@isNotEmpty(partSeq)'>" +
                    " and partSeq = #{partSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</if>" +
                    "order by partSeq desc, blockNo asc, seq desc " +
                    "limit #{startIndex}, #{pageSize}" +
                    "</script>"
    })
    List<ShortBlockVO> selectShortBlockList(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select count(*) from dynarap_sblock " +
                    "where 1 = 1 " +
                    "<if test='@com.servetech.dynarap.db.type.MybatisEmptyChecker@isNotEmpty(registerUid)'>" +
                    " and registerUid = #{registerUid,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField_NAuth} " +
                    "</if>" +
                    "<if test='@com.servetech.dynarap.db.type.MybatisEmptyChecker@isNotEmpty(partSeq)'>" +
                    " and partSeq = #{partSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</if>" +
                    "</script>"
    })
    int selectShortBlockCount(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select * from dynarap_sblock " +
                    "where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "limit 0, 1" +
                    "</script>"
    })
    ShortBlockVO selectShortBlockBySeq(Map<String, Object> params) throws Exception;


    @Insert({
            "<script>" +
                    "insert into dynarap_sblock (" +
                    "blockMetaSeq,partSeq,blockNo,blockName,julianStartAt,julianEndAt,offsetStartAt,offsetEndAt,registerUid" +
                    ") values (" +
                    "#{blockMetaSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{partSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{blockNo}" +
                    ",#{blockName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64}" +
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
            statement = "SELECT last_insert_id() FROM dynarap_sblock LIMIT 0, 1")
    void insertShortBlock(ShortBlockVO shortBlock) throws Exception;

    @Update({
            "<script>" +
                    " update dynarap_sblock set " +
                    " blockNo = #{blockNo}" +
                    ",blockName = #{blockName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64}" +
                    ",julianStartAt = #{julianStartAt}" +
                    ",julianEndAt = #{julianEndAt}" +
                    ",offsetStartAt = #{offsetStartAt}" +
                    ",offsetEndAt = #{offsetEndAt}" +
                    " where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void updateShortBlock(ShortBlockVO shortBlock) throws Exception;


}
