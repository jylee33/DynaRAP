package com.servetech.dynarap.db.service;

import com.google.gson.JsonObject;
import com.servetech.dynarap.config.ServerConstants;
import com.servetech.dynarap.controller.ApiController;
import com.servetech.dynarap.db.mapper.FlightMapper;
import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.db.type.String64;
import com.servetech.dynarap.ext.HandledServiceException;
import com.servetech.dynarap.vo.FlightVO;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

@Service("flightService")
public class FlightService {

    @Autowired
    private FlightMapper flightMapper;

    public List<FlightVO> getFlightList() throws HandledServiceException {
        try {
            return flightMapper.selectFlightList();
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public FlightVO insertFlight(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            FlightVO flight = ServerConstants.GSON.fromJson(payload, FlightVO.class);
            if (flight == null)
                throw new HandledServiceException(411, "요청 내용이 비행기체 형식에 맞지 않습니다.");

            flight.setRegisteredAt(LongDate.now());
            flight.setRegisterUid(uid);

            flightMapper.insertFlight(flight);

            return flight;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public FlightVO updateFlight(CryptoField.NAuth uid, JsonObject payload) throws HandledServiceException {
        try {
            FlightVO flight = ServerConstants.GSON.fromJson(payload, FlightVO.class);
            if (flight == null || flight.getSeq() == null || flight.getSeq().isEmpty())
                throw new HandledServiceException(411, "요청 파라미터 오류입니다. [필수 파라미터 누락]");

            flightMapper.updateFlight(flight);

            return flight;
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }

    @Transactional
    public void deleteFlight(JsonObject payload) throws HandledServiceException {
        try {
            CryptoField flightSeq = CryptoField.LZERO;
            if (!ApiController.checkJsonEmpty(payload, "seq"))
                flightSeq = CryptoField.decode(payload.get("seq").getAsString(), 0L);

            if (flightSeq == null || flightSeq.isEmpty())
                throw new HandledServiceException(411, "요청 파라미터 오류입니다. [필수 파라미터 누락]");

            Map<String, Object> params = new HashMap<>();
            params.put("seq", flightSeq);

            flightMapper.deleteFlight(params);
        } catch(Exception e) {
            throw new HandledServiceException(410, e.getMessage());
        }
    }
}
