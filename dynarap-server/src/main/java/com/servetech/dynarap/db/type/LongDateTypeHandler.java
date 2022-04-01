package com.servetech.dynarap.db.type;

import org.apache.ibatis.type.Alias;
import org.apache.ibatis.type.BaseTypeHandler;
import org.apache.ibatis.type.JdbcType;
import org.apache.ibatis.type.MappedJdbcTypes;

import java.sql.CallableStatement;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

@MappedJdbcTypes(JdbcType.BIGINT)
@Alias("LongDate")
public class LongDateTypeHandler extends BaseTypeHandler<LongDate> {

    @Override
    public LongDate getNullableResult(ResultSet rs, String columnName) throws SQLException {
        return new LongDate(rs.getLong(columnName));
    }

    @Override
    public LongDate getNullableResult(ResultSet rs, int columnIndex) throws SQLException {
        return new LongDate(rs.getLong(columnIndex));
    }

    @Override
    public LongDate getNullableResult(CallableStatement cstmt, int columnIndex)
            throws SQLException {
        return new LongDate(cstmt.getLong(columnIndex));
    }

    @Override
    public void setNonNullParameter(PreparedStatement pstmt, int columnIndex, LongDate param,
            JdbcType jdbcType) throws SQLException {
        pstmt.setLong(columnIndex, param.originOf());
    }
}
