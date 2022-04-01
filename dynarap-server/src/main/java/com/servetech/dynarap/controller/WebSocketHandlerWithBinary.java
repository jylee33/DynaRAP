package com.servetech.dynarap.controller;

import com.servetech.dynarap.DynaRAPServerApplication;
import lombok.RequiredArgsConstructor;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Component;
import org.springframework.web.socket.BinaryMessage;
import org.springframework.web.socket.CloseStatus;
import org.springframework.web.socket.WebSocketSession;
import org.springframework.web.socket.handler.BinaryWebSocketHandler;

@Component
@RequiredArgsConstructor
public class WebSocketHandlerWithBinary extends BinaryWebSocketHandler {
    private static final Logger logger = LoggerFactory.getLogger(WebSocketHandlerWithBinary.class);

    private WebSocketManager webSocketManager = null;

    @Override
    public void afterConnectionEstablished(WebSocketSession session) throws Exception {
        super.afterConnectionEstablished(session);

        if (webSocketManager == null) webSocketManager = DynaRAPServerApplication.webSocketManager;
        webSocketManager.addSession(WebSocketManager.WS_BINARY, session);
        //session.sendMessage(new TextMessage("{\"message\":\"websocket connected. " + System.currentTimeMillis() + "\"}"));
        logger.debug("[[[[[ [WEBSOCKET] connected {} ]]]]]", session.getRemoteAddress());
    }

    @Override
    protected void handleBinaryMessage(WebSocketSession session, BinaryMessage message) throws Exception {
        super.handleBinaryMessage(session, message);
        logger.debug("[[[[[ [WEBSOCKET] message : " + message.toString() + " ]]]]]");

        byte[] rtnBytes = message.getPayload().array();
        String strMessage = new String(rtnBytes, "UTF-8");
        logger.debug("[[[[[ [WEBSOCKET] message(converted) : " + strMessage + " ]]]]]");

        if (webSocketManager == null) webSocketManager = DynaRAPServerApplication.webSocketManager;
        webSocketManager.handleSocketMessage(WebSocketManager.WS_BINARY, session, strMessage);
    }

    @Override
    public void afterConnectionClosed(WebSocketSession session, CloseStatus status)
            throws Exception {
        super.afterConnectionClosed(session, status);

        if (webSocketManager == null) webSocketManager = DynaRAPServerApplication.webSocketManager;
        webSocketManager.removeSession(WebSocketManager.WS_BINARY, session);
        logger.debug("[[[[[ [WEBSOCKET] disconnect {} ]]]]]", session.getRemoteAddress());
    }
}
