package com.servetech.dynarap.controller;

import com.servetech.dynarap.DynaRAPServerApplication;
import lombok.RequiredArgsConstructor;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Component;
import org.springframework.web.socket.CloseStatus;
import org.springframework.web.socket.TextMessage;
import org.springframework.web.socket.WebSocketSession;
import org.springframework.web.socket.handler.TextWebSocketHandler;

@Component
@RequiredArgsConstructor
public class WebSocketHandler extends TextWebSocketHandler {
    private static final Logger logger = LoggerFactory.getLogger(WebSocketHandler.class);

    private WebSocketManager webSocketManager = null;

    @Override
    public void afterConnectionEstablished(WebSocketSession session) throws Exception {
        super.afterConnectionEstablished(session);

        if (webSocketManager == null) webSocketManager = DynaRAPServerApplication.webSocketManager;
        webSocketManager.addSession(WebSocketManager.WS_TEXT, session);
        //session.sendMessage(new TextMessage("{\"message\":\"websocket connected. " + System.currentTimeMillis() + "\"}"));
        logger.debug("[[[[[ [WEBSOCKET] connected {} ]]]]]", session.getRemoteAddress());
    }

    @Override
    protected void handleTextMessage(WebSocketSession session, TextMessage message)
            throws Exception {

        logger.debug("[[[[[ [WEBSOCKET] client{} handle message:{} ]]]]]", session.getRemoteAddress(),
                message.getPayload());

        if (webSocketManager == null) webSocketManager = DynaRAPServerApplication.webSocketManager;
        webSocketManager.handleSocketMessage(WebSocketManager.WS_TEXT, session, message.getPayload());
    }

    @Override
    public void afterConnectionClosed(WebSocketSession session, CloseStatus status)
            throws Exception {
        super.afterConnectionClosed(session, status);

        if (webSocketManager == null) webSocketManager = DynaRAPServerApplication.webSocketManager;
        webSocketManager.removeSession(WebSocketManager.WS_TEXT, session);
        logger.info("[[[[[ [WEBSOCKET] disconnect {} ]]]]]", session.getRemoteAddress());
    }

}
