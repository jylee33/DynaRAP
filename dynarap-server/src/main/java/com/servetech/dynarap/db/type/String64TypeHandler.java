package com.servetech.dynarap.db.type;

import org.apache.ibatis.type.Alias;
import org.apache.ibatis.type.BaseTypeHandler;
import org.apache.ibatis.type.JdbcType;
import org.apache.ibatis.type.MappedJdbcTypes;

import java.sql.CallableStatement;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

@MappedJdbcTypes(JdbcType.VARCHAR)
@Alias("String64")
public class String64TypeHandler extends BaseTypeHandler<String64> {

    @Override
    public String64 getNullableResult(ResultSet rs, String columnName) throws SQLException {
        return new String64(rs.getString(columnName));
    }

    @Override
    public String64 getNullableResult(ResultSet rs, int columnIndex) throws SQLException {
        return new String64(rs.getString(columnIndex));
    }

    @Override
    public String64 getNullableResult(CallableStatement cstmt, int columnIndex)
            throws SQLException {
        return new String64(cstmt.getString(columnIndex));
    }

    @Override
    public void setNonNullParameter(PreparedStatement pstmt, int columnIndex, String64 param,
            JdbcType jdbcType) throws SQLException {
        pstmt.setString(columnIndex, param.originOf());
    }
}
