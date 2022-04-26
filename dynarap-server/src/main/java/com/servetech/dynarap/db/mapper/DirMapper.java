package com.servetech.dynarap.db.mapper;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.vo.DirVO;
import org.apache.ibatis.annotations.*;

import java.util.List;
import java.util.Map;

@Mapper
public interface DirMapper {

    @Select({
            "<script>" +
                    "select x.* " +
                    "from (" +
                    "select a.*, 0 as dirOrder from dynarap_dir a " +
                    "where a.uid = #{uid,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField_NAuth} " +
                    "and a.parentDirSeq = 0 " +
                    "union " +
                    "select b.*, 1 as dirOrder from dynarap_dir b " +
                    "where b.uid = #{uid,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField_NAuth} " +
                    "<![CDATA[ and b.parentDirSeq > 0 ]]> " +
                    ") x " +
                    "order by x.dirOrder asc, x.dirName asc" +
                    "</script>"
    })
    List<DirVO> selectUserDirList(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select * from dynarap_dir " +
                    "where uid = #{uid,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField_NAuth} " +
                    "and parentDirSeq = #{parentDirSeq} " +
                    "order by dirName asc" +
                    "</script>"
    })
    List<DirVO> selectUserDirListByParent(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select * from dynarap_dir " +
                    "where seq = #{seq} " +
                    "limit 0, 1" +
                    "</script>"
    })
    DirVO selectUserDir(Map<String, Object> params) throws Exception;

    @Insert({
            "<script>" +
                    "insert into dynarap_dir (" +
                    "parentDirSeq,uid,dirName,dirType,dirIcon,createdAt,refSeq,refSubSeq" +
                    ") values (" +
                    "#{parentDirSeq}" +
                    ",#{uid,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField_NAuth}" +
                    ",#{dirName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64}" +
                    ",#{dirType}" +
                    ",#{dirIcon}" +
                    ",#{createdAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate}" +
                    ",#{refSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ",#{refSubSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}" +
                    ")" +
                    "</script>"
    })
    @SelectKey(before = false, keyColumn = "seq", keyProperty = "seq",
            resultType = Long.class,
            statement = "SELECT last_insert_id() FROM dynarap_dir LIMIT 0, 1")
    void insertDir(DirVO dir) throws Exception;

    @Update({
            "<script>" +
                    " update dynarap_dir set " +
                    " parentDirSeq = #{parentDirSeq} " +
                    ",dirName = #{dirName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64} " +
                    ",dirType = #{dirType} " +
                    ",dirIcon = #{dirIcon} " +
                    ",refSeq = #{refSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    ",refSubSeq = #{refSubSeq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    " where seq = #{seq}" +
                    "</script>"
    })
    void updateDir(DirVO dir) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_dir " +
                    "where seq = #{seq}" +
                    "</script>"
    })
    void deleteDir(Map<String, Object> params) throws Exception;

}
