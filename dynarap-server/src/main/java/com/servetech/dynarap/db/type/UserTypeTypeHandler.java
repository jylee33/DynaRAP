package com.servetech.dynarap.db.type;

import org.apache.ibatis.type.Alias;
import org.apache.ibatis.type.BaseTypeHandler;
import org.apache.ibatis.type.JdbcType;
import org.apache.ibatis.type.MappedJdbcTypes;

import java.sql.CallableStatement;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

@MappedJdbcTypes(JdbcType.TINYINT)
@Alias("UserType")
public class UserTypeTypeHandler extends BaseTypeHandler<UserType> {

    @Override
    public UserType getNullableResult(ResultSet rs, String columnName) throws SQLException {
        return UserType.parse(rs.getInt(columnName));
    }

    @Override
    public UserType getNullableResult(ResultSet rs, int columnIndex) throws SQLException {
        return UserType.parse(rs.getInt(columnIndex));
    }

    @Override
    public UserType getNullableResult(CallableStatement cstmt, int columnIndex)
            throws SQLException {
        return UserType.parse(cstmt.getInt(columnIndex));
    }

    @Override
    public void setNonNullParameter(PreparedStatement pstmt, int columnIndex, UserType param,
            JdbcType jdbcType) throws SQLException {
        pstmt.setInt(columnIndex, param.getUserType());
    }
}
