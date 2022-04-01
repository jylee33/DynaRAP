package com.servetech.dynarap.db.type;

import org.apache.ibatis.type.Alias;
import org.apache.ibatis.type.BaseTypeHandler;
import org.apache.ibatis.type.JdbcType;

import java.sql.CallableStatement;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

@Alias("CryptoField")
public class CryptoFieldTypeHandler extends BaseTypeHandler<CryptoField> {

    @Override
    public CryptoField getNullableResult(ResultSet rs, String columnName) throws SQLException {
        int columnIndex = -1;
        for (int i = 1; i <= rs.getMetaData().getColumnCount(); i++) {
            if (rs.getMetaData().getColumnName(i).equalsIgnoreCase(columnName)) {
                columnIndex = i;
                break;
            }
        }

        if (columnIndex == -1)
            return new CryptoField("");

        if (rs.getMetaData().getColumnType(columnIndex) == -5)
            return new CryptoField(rs.getLong(columnIndex));
        if (rs.getMetaData().getColumnType(columnIndex) == 4)
            return new CryptoField(rs.getInt(columnIndex));

        return new CryptoField(rs.getString(columnIndex));
    }

    @Override
    public CryptoField getNullableResult(ResultSet rs, int columnIndex) throws SQLException {
        if (rs.getMetaData().getColumnType(columnIndex) == -5)
            return new CryptoField(rs.getLong(columnIndex));
        if (rs.getMetaData().getColumnType(columnIndex) == 4)
            return new CryptoField(rs.getInt(columnIndex));

        return new CryptoField(rs.getString(columnIndex));
    }

    @Override
    public CryptoField getNullableResult(CallableStatement cstmt, int columnIndex)
            throws SQLException {
        if (cstmt.getMetaData().getColumnType(columnIndex) == -5)
            return new CryptoField(cstmt.getLong(columnIndex));
        if (cstmt.getMetaData().getColumnType(columnIndex) == 4)
            return new CryptoField(cstmt.getInt(columnIndex));

        return new CryptoField(cstmt.getString(columnIndex));
    }

    @Override
    public void setNonNullParameter(PreparedStatement pstmt, int columnIndex, CryptoField param,
            JdbcType jdbcType) throws SQLException {
        if (param.originTypeOf() == Integer.class)
            pstmt.setInt(columnIndex, param == null ? 0 : (Integer) param.originOf());
        else if (param.originTypeOf() == Long.class)
            pstmt.setLong(columnIndex, param == null ? 0L : (Long) param.originOf());
        else if (param.originTypeOf() == String.class)
            pstmt.setString(columnIndex, param == null ? "" : (String) param.originOf());
    }
}
