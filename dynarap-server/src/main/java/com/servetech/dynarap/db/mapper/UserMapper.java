package com.servetech.dynarap.db.mapper;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.vo.UserVO;
import org.apache.ibatis.annotations.*;

import java.util.List;
import java.util.Map;

@Mapper
public interface UserMapper {
    @Select({
            "<script>" +
                    "select a.* " +
                    "from dynarap_user a " +
                    "where a.username = #{username} " +
                    "limit 0, 1" +
                    "</script>"
    })
    UserVO selectUserByUsername(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select  a.* " +
                    "from dynarap_user a " +
                    "where a.username = #{username} " +
                    "and a.provider = #{provider} " +
                    "limit 0, 1" +
                    "</script>"
    })
    UserVO selectUserByProvider(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select a.* " +
                    "from dynarap_user a " +
                    "where a.uid = #{uid,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField_NAuth} " +
                    "limit 0, 1" +
                    "</script>"})
    UserVO selectUserBySeq(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select a.* " +
                    "from dynarap_user a " +
                    "where a.username = #{username} " +
                    "and a.password = #{password}" +
                    "limit 0, 1" +
                    "</script>"})
    UserVO selectUserByParams(Map<String, Object> params) throws Exception;


    @Select({
            "<script>" +
                    "select * from dynarap_user " +
                    "where accountName = #{accountName} " +
                    "and phoneNumber = #{phoneNumber} " +
                    "limit 0, 1" +
                    "</script>"
    })
    UserVO selectAccountByNamePhone(Map<String, Object> params) throws Exception;


    @Insert({
            "<script>" +
                    "insert into dynarap_user (" +
                    "uid, userType, username, password, accountLocked, accountName, provider," +
                    "joinedAt, leftAt, email, profileUrl, phoneNumber, " +
                    "privacyTermsReadAt, serviceTermsReadAt, pushToken, usePush, " +
                    "tempPassword, tempPasswordExpire, promotion, nickname" +
                    ") values (" +
                    "#{uid,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField_NAuth}" +
                    ",#{userType,javaType=java.lang.Integer,jdbcType=TINYINT,typeHandler=UserType}" +
                    ",#{username}" +
                    ",#{password}" +
                    ",#{accountLocked}" +
                    ",#{accountName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64}" +
                    ",#{provider}" +
                    ",#{joinedAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate}" +
                    ",#{leftAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate}" +
                    ",#{email}" +
                    ",#{profileUrl}" +
                    ",#{phoneNumber}" +
                    ",#{privacyTermsReadAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate}" +
                    ",#{serviceTermsReadAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate}" +
                    ",#{pushToken}" +
                    ",#{usePush}" +
                    ",#{tempPassword}" +
                    ",#{tempPasswordExpire,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate}" +
                    ",#{promotion,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64}" +
                    ",#{nickname,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64}" +
                    ")" +
                    "</script>"
    })
    void insertUser(UserVO user) throws Exception;

    @Update({
            "<script>" +
                    "update dynarap_user set " +
                    " accountName = #{accountName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64} " +
                    ",accountLocked = #{accountLocked}" +
                    "<if test='@com.servetech.dynarap.db.type.MybatisEmptyChecker@isNotEmpty(password)'>" +
                    ",password = #{password} " +
                    "</if> " +
                    ",userType = #{userType,javaType=java.lang.Integer,jdbcType=TINYINT,typeHandler=UserType}" +
                    ",leftAt = #{leftAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate}" +
                    ",email = #{email}" +
                    ",profileUrl = #{profileUrl}" +
                    ",phoneNumber = #{phoneNumber}" +
                    ",privacyTermsReadAt = #{privacyTermsReadAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate}" +
                    ",serviceTermsReadAt = #{serviceTermsReadAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate}" +
                    ",pushToken = #{pushToken}" +
                    ",usePush = #{usePush}" +
                    ",promotion = #{promotion,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64} " +
                    ",nickname = #{nickname,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64} " +
                    "<if test='@com.servetech.dynarap.db.type.MybatisEmptyChecker@isNotEmpty(tempPassword)'>" +
                    ",tempPassword = #{tempPassword} " +
                    "</if> " +
                    ",tempPasswordExpire = #{tempPasswordExpire,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate} " +
                    " where uid = #{uid,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField_NAuth} " +
                    "</script>"})
    void updateUser(UserVO user) throws Exception;


    @Insert({
            "insert into dynarap_user_device (" +
                    "uid,adid,lastConnectedAt,pushToken" +
                    ") values (" +
                    "#{uid,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField_NAuth}" +
                    ",#{adid}" +
                    ",#{lastConnectedAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate}" +
                    ",#{pushToken}" +
                    ")"
    })
    @SelectKey(before = false, keyColumn = "seq", keyProperty = "seq",
            resultType = CryptoField.class,
            statement = "SELECT last_insert_id() FROM dynarap_user_device LIMIT 0, 1")
    void insertUserDevice(UserVO.Device userDevice) throws Exception;

    @Update({
            "update dynarap_user_device set " +
                    " lastConnectedAt = #{lastConnectedAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate}" +
                    ",pushToken = #{pushToken}" +
                    " where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}"
    })
    void updateUserDevice(UserVO.Device userDevice) throws Exception;

    @Delete({
            "delete from dynarap_user_device where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField}"
    })
    void deleteUserDevice(Map<String, Object> params) throws Exception;

    @Delete({
            "delete from dynarap_user_device where uid = #{uid,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField_NAuth}"
    })
    void deleteUserDevices(Map<String, Object> params) throws Exception;

    @Select({
            "select * from dynarap_user_device " +
                    "where uid = #{uid,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField_NAuth} " +
                    "order by lastConnectedAt desc"
    })
    List<UserVO.Device> selectUserDevices(Map<String, Object> params) throws Exception;

    @Select({
            "select * from dynarap_user_device " +
                    "where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} "
    })
    UserVO.Device selectUserDeviceBySeq(Map<String, Object> params) throws Exception;

    @Select({
            "select * from dynarap_user_device " +
                    "where adid = #{adid} " +
                    "limit 0, 1"
    })
    UserVO.Device selectUserDeviceByAdid(Map<String, Object> params) throws Exception;


    @Select({
            "<script>" +
                    "select * from wooriga_user " +
                    "where leftAt = 0 " +
                    "and accountName = #{accountName} " +
                    "and phoneNumber = #{phoneNumber} " +
                    "order by username asc" +
                    "</script>"
    })
    List<UserVO> selectFindAccounts(Map<String, Object> params) throws Exception;

    @Select({
            "<script>" +
                    "select count(*) from wooriga_user " +
                    "where leftAt = 0 " +
                    "and uid = #{uid,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField_NAuth} " +
                    "and email = #{email} " +
                    "</script>"
    })
    int selectUserByIdEmail(Map<String, Object> params) throws Exception;

}
