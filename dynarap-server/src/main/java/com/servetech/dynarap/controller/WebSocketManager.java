package com.servetech.dynarap.controller;

import com.servetech.dynarap.config.ServerConstants;
import com.servetech.dynarap.db.service.UserService;
import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.String64;
import com.servetech.dynarap.ext.HandledServiceException;
import com.servetech.dynarap.vo.UserVO;
import lombok.Data;
import lombok.RequiredArgsConstructor;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.context.annotation.Configuration;
import org.springframework.stereotype.Component;
import org.springframework.web.socket.BinaryMessage;
import org.springframework.web.socket.TextMessage;
import org.springframework.web.socket.WebSocketSession;

import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.util.*;
import java.util.concurrent.ConcurrentHashMap;

@Component("webSocketManager")
@Configuration
@RequiredArgsConstructor
public class WebSocketManager {
    private static final Logger logger = LoggerFactory.getLogger(WebSocketHandler.class);

    public static final int WS_TEXT = 0x01;
    public static final int WS_BINARY = 0x10;

    private final UserService userService;

    private static Set<WebSocketSession> sessions = new ConcurrentHashMap<>().newKeySet();
    private static Map<String, List<WebSocketSession>> channelSessions = new HashMap<>();
    private static Map<String, ChatChannel> channelInfos = new HashMap<>();
    private static Map<WebSocketSession, Integer> sessionSocketTypes = new HashMap<>();
    private static Map<String, UserVO.Simple> issuerMap = new HashMap<>();

    @Override
    public String toString() {
        return "WebSocketManager";
    }

    public ChatChannel getChannelInfo(String channelSeq) {
        return channelInfos.get(channelSeq);
    }

    public void addSession(int socketType, WebSocketSession session) {
        sessions.add(session);
    }

    public void removeSession(int socketType, WebSocketSession session) {
        sessions.remove(session);

        Set<String> keySet = channelSessions.keySet();
        for (String key : keySet) {
            List<WebSocketSession> sessionList = channelSessions.get(key);
            if (sessionList.contains(session)) {
                sessionList.remove(session);

                // 해당 채널 사용자에게 퇴장 노티
                Iterator<UserVO.Simple> iterUserSimple = issuerMap.values().iterator();
                UserVO.Simple disconnectUser = null;
                while (iterUserSimple.hasNext()) {
                    UserVO.Simple user = iterUserSimple.next();
                    if (user.getChannelSession() != null && user.getChannelSession().get(key) == session) {
                        disconnectUser = user;
                        break;
                    }
                }

                if (disconnectUser != null && !key.equals(CryptoField.GCC.valueOf())) {
                    WSBroadcast wsBroadcast = new WSBroadcast();
                    wsBroadcast.setChannel(CryptoField.decode(key, 0L));
                    wsBroadcast.setWsCommand("disconnect");
                    wsBroadcast.setIssuer(disconnectUser);
                    wsBroadcast.setMessage(new String64(wsBroadcast.getIssuer().getDisplayName().originOf() + "님이 퇴장했습니다."));

                    for (WebSocketSession s : sessionList) {
                        if (!s.isOpen()) continue;
                        try {
                            Integer sst = sessionSocketTypes.get(s);
                            if (sst != WS_TEXT && sst != WS_BINARY) continue;
                            sendMessage(sst, s, ServerConstants.GSON.toJson(wsBroadcast));
                        } catch (Exception e) {
                            e.printStackTrace();
                        }
                    }
                }

                if (sessionSocketTypes.containsKey(session))
                    sessionSocketTypes.remove(session);
                break;
            }
        }
    }

    public void serverPush(String pushType, CryptoField channelSeq,
                           Object sendObject, CryptoField.NAuth receiver) throws IOException {
        WebSocketManager.WSBroadcast wsBroadcast = new WebSocketManager.WSBroadcast();
        wsBroadcast.setWsAction(null);
        wsBroadcast.setWsCommand("server-push");
        wsBroadcast.setServerPushType(pushType);
        wsBroadcast.setChannel(channelSeq);

        if (sendObject != null)
            wsBroadcast.setMessage(new String64(ServerConstants.GSON.toJson(sendObject)));
        else
            wsBroadcast.setMessage(new String64("{}"));

        if (receiver != null && !receiver.isEmpty())
            wsBroadcast.setIssuer(issuerMap.get(receiver.valueOf()));

        if (receiver == null || receiver.isEmpty()) {
            List<WebSocketSession> sessionList = channelSessions.get(channelSeq.valueOf());
            if (sessionList == null) {
                sessionList = new ArrayList<>();
                channelSessions.put(channelSeq.valueOf(), sessionList);
            }

            for (WebSocketSession webSocketSession : sessionList) {
                if (webSocketSession.isOpen() == false)
                    continue;

                Integer sst = sessionSocketTypes.get(webSocketSession);
                if (sst == null) continue;

                if (sst != WS_TEXT && sst != WS_BINARY) {
                    logger.debug("[[[[[ [WEBSOCKET] Unknown socket connect mode. " + webSocketSession + " ]]]]]");
                    continue;
                }

                sendMessage(sst, webSocketSession, ServerConstants.GSON.toJson(wsBroadcast));
            }
        }
        else {
            UserVO.Simple issuerInfo = issuerMap.get(receiver.valueOf());
            if (issuerInfo != null) {
                Integer sst = sessionSocketTypes.get(issuerInfo.getChannelSession().get(channelSeq.valueOf()));
                if (sst != null) {
                    if (sst != WS_TEXT && sst != WS_BINARY) {
                        logger.debug("[[[[[ [WEBSOCKET] Unknown socket connect mode. " + issuerInfo.getChannelSession() + " ]]]]]");
                    } else {
                        WebSocketSession issuerSession = issuerInfo.getChannelSession().get(channelSeq.valueOf());
                        if (issuerSession != null)
                            sendMessage(sst, issuerSession, ServerConstants.GSON.toJson(wsBroadcast));
                    }
                }
            }
        }
    }

    public void sendMessage(int socketType, WebSocketSession session, String message) throws IOException {
        if (socketType == WS_TEXT)
            session.sendMessage(new TextMessage(message));
        else if (socketType == WS_BINARY)
            session.sendMessage(new BinaryMessage(message.getBytes(StandardCharsets.UTF_8)));
    }

    public void handleSocketMessage(int socketType, WebSocketSession session, String messagePayload) throws HandledServiceException, IOException {
        WebSocketManager.WSAction wsAction = ServerConstants.GSON.fromJson(
                messagePayload, WebSocketManager.WSAction.class);
        WebSocketManager.WSBroadcast wsBroadcast = new WebSocketManager.WSBroadcast();
        wsBroadcast.setWsAction(wsAction);
        wsBroadcast.setChannel(wsAction.getChannel());

        ChatChannel channelInfo = channelInfos.get(wsAction.getChannel().valueOf());
        if (channelInfo == null) {
            if (wsAction.getChannel().originOf().equals("global.chat.channel")) {
                channelInfo = new ChatChannel();
                channelInfo.setChannelName("global.chat.channel");
                channelInfo.setRoomMembers(new ArrayList<>());
                channelInfo.setSessions(new LinkedHashMap<>());
            }
            else {
                channelInfo = new ChatChannel();
                channelInfo.setChannelName("chatroom-" + wsAction.getChannel().valueOf());
                channelInfo.setChatRoomSeq(wsAction.getChannel());

                /*
                // 채팅방 정보를 로딩하면서 참여자 정보 및 채팅 탑메시지를 유지함.
                channelInfo.setChatRoomInfo(chatCacheService.getChatRoom(
                        wsAction.getIssuerUid(), "wooriga.chatRoom", wsAction.getChannel().valueOf()));

                if (channelInfo.getChatRoomInfo() == null) {
                    wsBroadcast.setWsCommand("error");
                    wsBroadcast.setMessage(new String64("채팅방 정보 조회 실패"));
                    sendMessage(socketType, session, ServerConstants.GSON.toJson(wsBroadcast));
                    return;
                }
                */
                channelInfo.setSessions(new LinkedHashMap<>());
            }
            channelInfos.put(wsAction.getChannel().valueOf(), channelInfo);
        }

        if (wsAction.getIssuerUid() == null || wsAction.getIssuerUid().isEmpty()) {
            wsBroadcast.setWsCommand("error");
            wsBroadcast.setMessage(new String64("사용자 정보 조회 실패"));
            sendMessage(socketType, session, ServerConstants.GSON.toJson(wsBroadcast));
            return;
        }

        UserVO.Simple issuer = issuerMap.get(wsAction.getIssuerUid().valueOf());
        if (issuer == null) {
            UserVO issuerFull = userService.getUserBySeq(wsAction.getIssuerUid());
            if (issuerFull == null) {
                wsBroadcast.setWsCommand("error");
                wsBroadcast.setMessage(new String64("사용자 정보 조회 실패"));
                sendMessage(socketType, session, ServerConstants.GSON.toJson(wsBroadcast));
                return;
            }
            issuer = issuerFull.getSimple();
            issuerMap.put(wsAction.getIssuerUid().valueOf(), issuer);
        }

        List<WebSocketSession> sessionList = channelSessions.get(wsAction.getChannel().valueOf());
        if (sessionList == null) {
            sessionList = new ArrayList<>();
            channelSessions.put(wsAction.getChannel().valueOf(), sessionList);
        }

        if (!sessionList.contains(session)) sessionList.add(session);
        sessionSocketTypes.put(session, socketType);

        wsBroadcast.setIssuer(issuer);
        if (wsBroadcast.getIssuer().getChannelSession() == null)
            wsBroadcast.getIssuer().setChannelSession(new LinkedHashMap<>());

        wsBroadcast.getIssuer().getChannelSession().put(wsAction.getChannel().valueOf(), session);

        if (channelInfo.getChannelName().equals("global.chat.channel")) {
            if (!channelInfo.getRoomMembers().contains(issuer.getUid().valueOf())) {
                channelInfo.getRoomMembers().add(issuer.getUid().valueOf());
            }
            if (!channelInfo.getSessions().containsKey(issuer.getUid().valueOf())) {
                channelInfo.getSessions().put(issuer.getUid().valueOf(), session);
            }
        }

        if (wsAction.getWsCommand().equals("connect")) {
            wsBroadcast.setWsCommand(wsAction.getWsCommand());
            wsBroadcast.setMessage(new String64("conn_" + wsBroadcast.getIssuer().getUid().valueOf()));

            if (wsAction.getChannel().equals(CryptoField.GCC)) {
            }
        } else if (wsAction.getWsCommand().equals("disconnect")) {
            wsBroadcast.setWsCommand(wsAction.getWsCommand());
            wsBroadcast.setMessage(new String64("disconn_" + wsBroadcast.getIssuer().getUid().valueOf()));
        } else if (wsAction.getWsCommand().equals("send-message")) {
            wsBroadcast.setWsCommand(wsAction.getWsCommand());
            wsBroadcast.setMessage(wsAction.getMessage());
        }

        for (WebSocketSession webSocketSession : sessionList) {
            if (webSocketSession.isOpen() == false)
                continue;
            if (session == webSocketSession && !wsAction.getUseEcho())
                continue;

            Integer sst = sessionSocketTypes.get(webSocketSession);
            if (sst == null) continue;

            if (sst != WS_TEXT && sst != WS_BINARY) {
                logger.debug("[[[[[ [WEBSOCKET] Unknown socket connect mode. " + webSocketSession + " ]]]]]");
                continue;
            }

            sendMessage(sst, webSocketSession, ServerConstants.GSON.toJson(wsBroadcast));
        }

        if (wsAction.getWsCommand().equals("disconnect")) {
            if (sessionList.contains(session))
                sessionList.remove(session);

            if (sessionSocketTypes.containsKey(session))
                sessionSocketTypes.remove(session);

            // global chat info channel 대응. 접속 종료.
            if (wsAction.getChannel().equals(CryptoField.GCC)) {
                channelInfo.getSessions().remove(wsAction.getIssuerUid().valueOf());
                channelInfo.getRoomMembers().remove(wsAction.getIssuerUid().valueOf());
            }
        }
    }

    @Data
    public static class WSAction {
        private CryptoField channel;
        private String wsCommand;
        private CryptoField.NAuth issuerUid;
        private Boolean useEcho;
        private String64 message;
    }

    @Data
    public static class WSBroadcast {
        private CryptoField channel;
        private String wsCommand;
        private String serverPushType;
        private UserVO.Simple issuer;
        private String64 message;

        private transient WSAction wsAction;
    }

    @Data
    public static class ChatChannel {
        private String channelName;
        private CryptoField chatRoomSeq;
        private List<String> roomMembers;
        //private List<ChatVO> chatMessages;
        //private ChatVO.Room chatRoomInfo;
        private LinkedHashMap<String, WebSocketSession> sessions;
    }
}
