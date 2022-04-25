package com.servetech.dynarap.db.mapper;

import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.vo.FlightVO;
import org.apache.ibatis.annotations.*;

import java.util.List;
import java.util.Map;

@Mapper
public interface FlightMapper {

    @Select({
            "<script>" +
                    "select * from dynarap_flight " +
                    "order by flightName asc, registeredAt desc" +
                    "</script>"
    })
    List<FlightVO> selectFlightList() throws Exception;

    @Insert({
            "<script>" +
                    "insert into dynarap_flight (" +
                    "flightName,flightType,createdAt,lastFlightAt,registeredAt,registerUid" +
                    ") values (" +
                    "#{flightName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64}" +
                    ",#{flightType}" +
                    ",#{createdAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate}" +
                    ",#{lastFlightAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate}" +
                    ",#{registeredAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate}" +
                    ",#{registerUid,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField_NAuth}" +
                    ")" +
                    "</script>"
    })
    @SelectKey(before = false, keyColumn = "seq", keyProperty = "seq",
            resultType = CryptoField.class,
            statement = "SELECT last_insert_id() FROM dynarap_flight LIMIT 0, 1")
    void insertFlight(FlightVO flight) throws Exception;

    @Update({
            "<script>" +
                    " update dynarap_flight set " +
                    " flightName=#{flightName,javaType=java.lang.String,jdbcType=VARCHAR,typeHandler=String64} " +
                    ",flightType=#{flightType} " +
                    ",lastFlightAt=#{lastFlightAt,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=LongDate} " +
                    " where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void updateFlight(FlightVO flight) throws Exception;

    @Delete({
            "<script>" +
                    "delete from dynarap_flight " +
                    "where seq = #{seq,javaType=java.lang.Long,jdbcType=BIGINT,typeHandler=CryptoField} " +
                    "</script>"
    })
    void deleteFlight(Map<String, Object> params) throws Exception;
}
