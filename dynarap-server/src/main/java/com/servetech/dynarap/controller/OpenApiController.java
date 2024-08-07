package com.servetech.dynarap.controller;

import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.google.gson.reflect.TypeToken;
import com.servetech.dynarap.DynaRAPServerApplication;
import com.servetech.dynarap.config.ServerConstants;
import com.servetech.dynarap.db.service.ParamService;
import com.servetech.dynarap.db.service.RawService;
import com.servetech.dynarap.db.service.SendMailService;
import com.servetech.dynarap.db.service.UserService;
import com.servetech.dynarap.db.type.CryptoField;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.db.type.String64;
import com.servetech.dynarap.db.type.UserType;
import com.servetech.dynarap.ext.HandledServiceException;
import com.servetech.dynarap.ext.ResponseHelper;
import com.servetech.dynarap.vo.*;
import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.RequiredArgsConstructor;
import lombok.SneakyThrows;
import org.apache.commons.rng.simple.RandomSource;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.core.env.Environment;
import org.springframework.http.HttpEntity;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.client.ClientHttpResponse;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.client.ResponseErrorHandler;
import org.springframework.web.client.RestTemplate;
import org.springframework.web.multipart.MultipartFile;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.*;
import java.lang.reflect.Type;
import java.nio.charset.StandardCharsets;
import java.util.*;

import static org.springframework.http.HttpStatus.Series.CLIENT_ERROR;
import static org.springframework.http.HttpStatus.Series.SERVER_ERROR;

@Controller
@RequestMapping(value = "/open-api")
@RequiredArgsConstructor
public class OpenApiController extends ApiController {
    private static final Logger logger = LoggerFactory.getLogger(OpenApiController.class);

    @Value("${dynarap.host.server}")
    private String serverHost;

    @Value("${dynarap.auth-root}")
    private String authRoot;

    @Value("${neoulsoft.auth.client-id}")
    private String authClientId;

    @Value("${neoulsoft.auth.client-secret}")
    private String authClientSecret;

    @Value(value = "${static.resource.location}")
    private String staticLocation;

    @Autowired
    private Environment envServer;

    @Autowired
    private SendMailService sendMailService;

    @Autowired
    private RestTemplate restTemplate;

    private static long EMAIL_NEXT_SEQ = System.currentTimeMillis();

    private static Map<Long, EmailAuthVO> emailAuthMap = new HashMap<>();

    @RequestMapping(value = "/env")
    @ResponseBody
    public Object openApiEnvironment() throws HandledServiceException {
        EnvironmentVO env = new EnvironmentVO();
        env.setAllowedAppVersions(Arrays.asList("1.0.0"));
        env.setNeedUpdateAppVersions(new ArrayList<String>());
        env.setUseEncryption(false);
        env.setAndroidServiceVersion("0.0.1");
        env.setIosServiceVersion("0.0.1");
        env.setWebappServiceVersion("0.0.1");
        env.setActiveHost(envServer.getProperty("dynarap.active.host"));
        if (env.getActiveHost() == null || env.getActiveHost().isEmpty() == true) {
            env.setActiveHost("http://" + serverHost + ":43443");
        }
        env.setUploadHost(envServer.getProperty("dynarap.upload.host"));
        if (env.getUploadHost() == null || env.getUploadHost().isEmpty() == true) {
            env.setUploadHost(env.getActiveHost());
        }

        boolean debug = false;
        if (DynaRAPServerApplication.global.getEnvironment() != null
                && DynaRAPServerApplication.global.getEnvironment().getProperty("debug") != null) {
            debug = Boolean.parseBoolean(DynaRAPServerApplication.global.getEnvironment().getProperty("debug"));
        }
        env.setDebugMode(debug);

        return env;
    }

    @RequestMapping(value = "/verify")
    @ResponseBody
    public Object openApiVerify(@RequestBody JsonObject payload) throws HandledServiceException {
        String epVerify = authRoot + "/auth/verify";
        HttpHeaders headers = new HttpHeaders();
        headers.setContentType(MediaType.APPLICATION_JSON);
        payload.addProperty("clientId", authClientId);
        payload.addProperty("clientSecret", authClientSecret);
        HttpEntity<JsonObject> reqPost = new HttpEntity<>(payload, headers);

        JsonObject verifyResultData = restTemplate.postForObject(epVerify, reqPost, JsonObject.class);
        VerifyResultVO verifyResult = ServerConstants.GSON.fromJson(verifyResultData, VerifyResultVO.class);

        if (verifyResult.getNauthUserInfo() != null) {
            UserVO user = getService(UserService.class).getUserBySeq(verifyResult.getNauthUserInfo().getUid());
            if (user != null) {
                if (payload.get("adid") != null && payload.get("adid").isJsonNull() == false) {
                    UserVO.Device userDevice = getService(UserService.class).getUserDeviceByAdid(payload.get("adid").getAsString());
                    if (userDevice == null) {
                        userDevice = new UserVO.Device();
                        userDevice.setAdid(payload.get("adid").getAsString());
                        userDevice.setUid(user.getUid());
                        userDevice.setPushToken(payload.get("pushToken").getAsString());
                        userDevice.setLastConnectedAt(LongDate.now());
                        getService(UserService.class).insertUserDevice(userDevice);
                    } else {
                        userDevice.setLastConnectedAt(LongDate.now());
                        userDevice.setPushToken(payload.get("pushToken").getAsString());
                        getService(UserService.class).updateUserDevice(userDevice);
                    }
                }

                verifyResult.setUserInfo(user);
            }
            verifyResult.setNauthUserInfo(null);
        }

        return verifyResult;
    }

    // refresh
    @RequestMapping(value = "/refresh")
    @ResponseBody
    public Object openApiRefresh(@RequestBody JsonObject payload) throws HandledServiceException {
        String epRefresh = authRoot + "/auth/refresh";
        HttpHeaders headers = new HttpHeaders();
        headers.setContentType(MediaType.APPLICATION_JSON);
        payload.addProperty("clientId", authClientId);
        payload.addProperty("clientSecret", authClientSecret);
        HttpEntity<JsonObject> reqPost = new HttpEntity<>(payload, headers);
        VerifyResultVO refreshResult = restTemplate.postForObject(epRefresh, reqPost, VerifyResultVO.class);

        if (refreshResult.getNauthUserInfo() != null) {
            UserVO user = getService(UserService.class).getUserBySeq(refreshResult.getNauthUserInfo().getUid());
            if (user != null) {
                if (payload.get("adid") != null && payload.get("adid").isJsonNull() == false) {
                    UserVO.Device userDevice = getService(UserService.class).getUserDeviceByAdid(payload.get("adid").getAsString());
                    if (userDevice == null) {
                        userDevice = new UserVO.Device();
                        userDevice.setAdid(payload.get("adid").getAsString());
                        userDevice.setUid(user.getUid());
                        userDevice.setPushToken(payload.get("pushToken").getAsString());
                        userDevice.setLastConnectedAt(LongDate.now());
                        getService(UserService.class).insertUserDevice(userDevice);
                    } else {
                        userDevice.setLastConnectedAt(LongDate.now());
                        userDevice.setPushToken(payload.get("pushToken").getAsString());
                        getService(UserService.class).updateUserDevice(userDevice);
                    }
                }

                refreshResult.setUserInfo(user);
            }
            refreshResult.setNauthUserInfo(null);
        }

        return refreshResult;
    }

    // login
    @RequestMapping(value = "/login")
    @ResponseBody
    public Object openApiLogin(HttpServletRequest request, @RequestBody JsonObject payload) throws HandledServiceException {
        String accessToken = request.getHeader("Authorization");
        if (accessToken == null)
            throw new HandledServiceException(401, "로그인 과정을 다시 시도해 주세요.");

        if (payload == null || payload.isJsonNull() == true)
            throw new HandledServiceException(404, "잘못된 로그인 시도입니다.");

        String epLogin = authRoot + "/auth/login";
        HttpHeaders headers = new HttpHeaders();
        headers.set("Authorization", request.getHeader("Authorization"));
        headers.setContentType(MediaType.APPLICATION_JSON);
        payload.addProperty("clientId", authClientId);
        payload.addProperty("clientSecret", authClientSecret);
        HttpEntity<JsonObject> reqPost = new HttpEntity<>(payload, headers);

        ResponseErrorHandler errorHandler = restTemplate.getErrorHandler();
        restTemplate.setErrorHandler(new ResponseErrorHandler() {
            @Override
            public boolean hasError(ClientHttpResponse response) throws IOException {
                return (response.getStatusCode().series() == CLIENT_ERROR
                        || response.getStatusCode().series() == SERVER_ERROR);
            }

            @SneakyThrows
            @Override
            public void handleError(ClientHttpResponse clientResponse) throws IOException {
                if (clientResponse.getStatusCode().series() == HttpStatus.Series.SERVER_ERROR) {
                    // handle SERVER_ERROR
                } else if (clientResponse.getStatusCode().series() == HttpStatus.Series.CLIENT_ERROR) {
                    String responseText = null;
                    try (Scanner scanner = new Scanner(clientResponse.getBody(), StandardCharsets.UTF_8.name())) {
                        responseText = scanner.useDelimiter("\\A").next();
                    }
                    ResponseVO respError = ServerConstants.GSON.fromJson(responseText, ResponseVO.class);
                    String respErrorMessage = "";
                    if (respError.getMessage() instanceof String)
                        respErrorMessage = (String) respError.getMessage();
                    else if (respError.getMessage() instanceof String64)
                        respErrorMessage = ((String64) respError.getMessage()).originOf();

                    if (respError.getCode() == 403) {
                        if (respErrorMessage.contains("locked"))
                            respErrorMessage = "로그인이 실패했습니다. 계정이 잠겨 있습니다.";
                        else
                            respErrorMessage = "로그인이 실패했습니다. 아이디 혹은 비밀번호를 확인하세요.";
                    }
                    else if (respError.getCode() == 401)
                        respErrorMessage = "로그인 과정을 다시 시도해 주세요.";
                    else
                        respErrorMessage = "로그인에 실패했습니다.";

                    throw new HandledServiceException(respError.getCode(), respErrorMessage);
                }
            }
        });

        JsonObject loginResultData = restTemplate.postForObject(epLogin, reqPost, JsonObject.class);
        VerifyResultVO loginResult = ServerConstants.GSON.fromJson(loginResultData, VerifyResultVO.class);

        restTemplate.setErrorHandler(errorHandler);

        UserVO user = getService(UserService.class).getUserBySeq(loginResult.getNauthUserInfo().getUid());
        if (user == null)
            throw new HandledServiceException(404, "사용자를 찾을 수 없습니다.");

        if (payload.get("adid") != null && payload.get("adid").isJsonNull() == false) {
            UserVO.Device userDevice = getService(UserService.class).getUserDeviceByAdid(payload.get("adid").getAsString());
            if (userDevice == null) {
                userDevice = new UserVO.Device();
                userDevice.setAdid(payload.get("adid").getAsString());
                userDevice.setUid(user.getUid());
                userDevice.setPushToken(payload.get("pushToken").getAsString());
                userDevice.setLastConnectedAt(LongDate.now());
                getService(UserService.class).insertUserDevice(userDevice);
            } else {
                userDevice.setLastConnectedAt(LongDate.now());
                userDevice.setPushToken(payload.get("pushToken").getAsString());
                getService(UserService.class).updateUserDevice(userDevice);
            }
        }
        loginResult.setUserInfo(user);
        loginResult.setNauthUserInfo(null);

        return loginResult;
    }

    @RequestMapping(value = "/user-check")
    @ResponseBody
    public Object openApiUserCheck(HttpServletRequest request, @RequestBody JsonObject payload)
            throws HandledServiceException {
        String accessToken = request.getHeader("Authorization");
        if (accessToken == null)
            throw new HandledServiceException(401, "Missing accessToken.");

        if (payload == null || payload.isJsonNull() == true)
            throw new HandledServiceException(404, "Authentication parameter missing.");

        UserVO.Device userDevice = getService(UserService.class).getUserDeviceByAdid(payload.get("adid").getAsString());
        if (userDevice == null)
            return ResponseHelper.response(404, "미등록장치", "");

        return ResponseHelper.response(200, "이미등록된장치", "");
    }

    @RequestMapping(value = "/check-dup")
    @ResponseBody
    public Object openApiCheckDup(HttpServletRequest request, @RequestBody JsonObject payload)
            throws HandledServiceException {
        String accessToken = request.getHeader("Authorization");
        if (accessToken == null)
            throw new HandledServiceException(401, "Missing accessToken.");

        if (payload == null || payload.isJsonNull() == true)
            throw new HandledServiceException(404, "Authentication parameter missing.");

        UserVO user = getService(UserService.class).getUser(payload.get("username").getAsString() + "@" + authClientId);
        if (user == null)
            return ResponseHelper.response(200, "사용자 없음", false);

        return ResponseHelper.response(200, "사용자 있음", true);
    }

    @RequestMapping(value = "/join")
    @ResponseBody
    public Object openApiJoin(HttpServletRequest request, @RequestBody JsonObject payload)
            throws HandledServiceException {
        String accessToken = request.getHeader("Authorization");
        if (accessToken == null)
            throw new HandledServiceException(401, "Missing accessToken.");

        JsonObject jobj = payload;
        if (jobj == null || jobj.isJsonNull() == true)
            throw new HandledServiceException(404, "Authentication parameter missing.");

        String epRegister = authRoot + "/auth/register";
        HttpHeaders headers = new HttpHeaders();
        headers.set("Authorization", request.getHeader("Authorization"));
        headers.setContentType(MediaType.APPLICATION_JSON);
        payload.addProperty("clientId", authClientId);
        payload.addProperty("clientSecret", authClientSecret);
        payload.addProperty("userType", UserType.NAuth.USER.toString());
        HttpEntity<JsonObject> reqPost = new HttpEntity<>(payload, headers);
        JsonObject registerResultData = restTemplate.postForObject(epRegister, reqPost, JsonObject.class);
        ResponseVO registerResult = ServerConstants.GSON.fromJson(registerResultData, ResponseVO.class);

        if (registerResult.getCode() != 200) {
            boolean debug = false;
            if (DynaRAPServerApplication.global.getEnvironment() != null
                    && DynaRAPServerApplication.global.getEnvironment().getProperty("debug") != null) {
                debug = Boolean.parseBoolean(DynaRAPServerApplication.global.getEnvironment().getProperty("debug"));
            }
            String errorMessage = "";
            if (debug) errorMessage = (String) registerResult.getMessage();
            else errorMessage = ((String64) registerResult.getMessage()).originOf();
            throw new HandledServiceException(registerResult.getCode(), errorMessage);
        }

        try {
            // insert client side user info
            Type nauthUserType = TypeToken.getParameterized(NAuthUserVO.class).getType();
            NAuthUserVO registerUser = registerResult.getTypedResponse(nauthUserType);

            UserVO clientUser = new UserVO();
            clientUser.setUid(registerUser.getUid());
            //clientUser.setEmail(registerUser.getUsername());
            clientUser.setNickname(registerUser.getAccountName());
            clientUser.setPhoneNumber(payload.get("phoneNumber").getAsString());

            if (payload.get("pushToken") != null && payload.get("pushToken").isJsonNull() == false) {
                clientUser.setUsePush(true);
                clientUser.setPushToken(payload.get("pushToken").getAsString());
            }

            clientUser.setNAuthUserType(registerUser.getNAuthUserType());
            clientUser.setUserType(UserType.USER);
            clientUser.setClientId(registerUser.getClientId());
            clientUser.setJoinedAt(LongDate.now());
            clientUser.setLeftAt(LongDate.zero());
            clientUser.setPrivacyTermsReadAt(LongDate.now());
            clientUser.setServiceTermsReadAt(LongDate.now());
            clientUser.setAccountLocked(false);
            clientUser.setAccountName(registerUser.getAccountName());
            clientUser.setUsername(registerUser.getUsername());
            clientUser.setProvider(registerUser.getProvider());
            clientUser.setAuthProvider(registerUser.getAuthProvider());

            String userPassword = (payload.get("password") == null || payload.get("password").isJsonNull())
                    ? "" : payload.get("password").getAsString();
            if (clientUser.getUsername().contains("@") && !clientUser.getProvider().equals("neoulsoft")) {
                clientUser.setPassword(ServerConstants.PASSWORD_ENCODER.encode(registerUser.getUsername()));
            } else {
                clientUser.setPassword(ServerConstants.PASSWORD_ENCODER.encode(userPassword));
            }
            getService(UserService.class).insertUser(clientUser);

            try {
                if (payload.get("adid") != null && payload.get("adid").isJsonNull() == false) {
                    UserVO.Device userDevice = getService(UserService.class).getUserDeviceByAdid(payload.get("adid").getAsString());
                    if (userDevice == null) {
                        userDevice = new UserVO.Device();
                        userDevice.setAdid(payload.get("adid").getAsString());
                        userDevice.setUid(clientUser.getUid());
                        userDevice.setPushToken(payload.get("pushToken").getAsString());
                        userDevice.setLastConnectedAt(LongDate.now());
                        getService(UserService.class).insertUserDevice(userDevice);
                    } else {
                        userDevice.setLastConnectedAt(LongDate.now());
                        userDevice.setPushToken(payload.get("pushToken").getAsString());
                        getService(UserService.class).updateUserDevice(userDevice);
                    }
                }
            } catch(Exception ex) {
                // skip user device register
            }

        } catch(Exception ex) {
            String epUnregister = authRoot + "/auth/unregister";
            headers = new HttpHeaders();
            headers.set("Authorization", request.getHeader("Authorization"));
            headers.setContentType(MediaType.APPLICATION_JSON);
            payload.addProperty("clientId", authClientId);
            payload.addProperty("clientSecret", authClientSecret);
            reqPost = new HttpEntity<>(payload, headers);
            JsonObject unregisterResultData = restTemplate.postForObject(epUnregister, reqPost, JsonObject.class);
            ResponseVO unregisterResult = ServerConstants.GSON.fromJson(unregisterResultData, ResponseVO.class);
            throw new HandledServiceException(411, ex.getMessage());
        }

        return ResponseHelper.response(200, "회원등록성공", "");
    }

    @RequestMapping(value = "/email-auth-req")
    @ResponseBody
    public ResponseVO openApiEmailAuthReq(HttpServletRequest request, @RequestBody JsonObject payload) {
        try {
            JsonObject jobj = payload;
            if (jobj == null || jobj.isJsonNull() == true)
                throw new Exception("Authentication parameter missing.");

            String email = jobj.get("email").getAsString();

            EmailAuthVO emailAuthVO = new EmailAuthVO();
            emailAuthVO.setSeq(new CryptoField(EMAIL_NEXT_SEQ++));
            emailAuthVO.setEmail(email);
            emailAuthVO.setAuthNo(String.format("%6d", 100000 + Math.abs(RandomSource.createInt()) % 900000));
            emailAuthVO.setAuthorized(false);
            emailAuthVO.setRequestAt(new LongDate(System.currentTimeMillis()));
            emailAuthMap.put((Long) emailAuthVO.getSeq().originOf(), emailAuthVO);

            JsonObject result = new JsonObject();
            result.addProperty("seq", emailAuthVO.getSeq().valueOf());

            // 이메일 발송.
            StringBuilder sbMail = new StringBuilder();
            sbMail.append("<html lang=\"ko\">");
            sbMail.append("<head><title>").append("[DynaRAP] 인증번호를 확인하세요.").append("</title></head>");
            sbMail.append("<body>");
            sbMail.append("<p>안녕하세요. 'DynaRAP' 입니다.</p>");
            sbMail.append("<p>이메일을 인증하기위해 아래 인증번호를 입력해 주세요.</p>");
            sbMail.append("<br/><br/>");
            sbMail.append("<p>인증번호 : ").append(emailAuthVO.getAuthNo()).append("</p>");
            sbMail.append("<br/><br/>");
            sbMail.append("<p>감사합니다.</p>");
            sbMail.append("</body>");
            sbMail.append("</html>");
            sendMailService.send("neoul@neoulsoft.com", Arrays.asList(emailAuthVO.getEmail()),
                    "[DynaRAP] 인증번호를 확인하세요.", sbMail.toString());

            return ResponseHelper.response(200, "Success - EMAIL_REQ", result);
        } catch (Exception e) {
            e.printStackTrace();
            return ResponseHelper.error(411, e.getMessage());
        }
    }

    @RequestMapping(value = "/email-auth-verify")
    @ResponseBody
    public ResponseVO openApiEmailAuthVerify(HttpServletRequest request, @RequestBody JsonObject payload) {
        try {
            JsonObject jobj = payload;
            if (jobj == null || jobj.isJsonNull() == true)
                throw new Exception("Authentication parameter missing.");

            String email = jobj.get("email").getAsString();
            String authNo = jobj.get("authNo").getAsString();
            String encAuthSeq = jobj.get("authSeq").getAsString();
            long authSeq = CryptoField.decode(encAuthSeq, 0L).originOf();

            EmailAuthVO emailAuthVO = emailAuthMap.get(authSeq);
            if (emailAuthVO == null)
                throw new Exception("인증 요청 정보가 없습니다.");

            if (!email.equals(emailAuthVO.getEmail()))
                throw new Exception("요청한 이메일과 맞지 않습니다.");

            if (!emailAuthVO.getAuthNo().equals(authNo))
                throw new Exception("인증번호가 맞지 않습니다.");

            if (emailAuthVO.getRequestAt().originOf() < System.currentTimeMillis() - 10 * 60 * 1000)
                throw new Exception("인증 유효 시간이 초과되었습니다. 다시 시도해 주세요.");

            if (emailAuthVO.isAuthorized() == true)
                throw new Exception("이미 인증이 완료된 요청입니다. 다시 시도해 주세요.");

            emailAuthVO.setAuthorized(true);

            return ResponseHelper.response(200, "Success - Email Auth Verify", "");
        } catch (Exception e) {
            e.printStackTrace();
            return ResponseHelper.error(411, e.getMessage());
        }
    }

    @RequestMapping(value = "/upload")
    @ResponseBody
    public ResponseVO openApiUpload(HttpServletRequest request,
                                    @RequestParam(name = "attach", required = false) MultipartFile attach,
                                    @RequestParam(name = "importFilePath", required = false) String importFilePath) throws Exception
    {
        try {
            UserVO user = getService(UserService.class).getUser("admin@dynarap@dynarap");

            File uploadRoot = new File(staticLocation.replaceAll("file:", ""), "dynarap/raw");
            if (uploadRoot.exists() == false) uploadRoot.mkdirs();

            RawVO.Upload upload = null;
            if (attach != null && !attach.isEmpty() && attach.getSize() > 0) {
                String originalFileName = attach.getOriginalFilename().replaceAll("_", "");
                String fileExt = originalFileName.substring(originalFileName.lastIndexOf(".") + 1);

                String uploadId = new CryptoField(originalFileName + "_" + attach.getSize()).valueOf();

                upload = getService(RawService.class).getUploadById(uploadId);
                if (upload == null) {
                    upload = new RawVO.Upload();
                    upload.setUploadId(uploadId);
                    upload.setUploadName(new String64(originalFileName));
                    upload.setFileSize(attach.getSize());
                    upload.setRegisterUid(user.getUid());
                    upload.setUploadedAt(LongDate.now());
                    getService(RawService.class).insertRawUpload(upload);
                }

                File fSave = new File(uploadRoot, upload.getUploadId() + "." + fileExt);
                if (fSave.exists() == true) fSave.delete();
                attach.transferTo(fSave);

                upload.setStorePath(fSave.getAbsolutePath());
            }
            else {
                if (importFilePath == null || importFilePath.isEmpty())
                    throw new HandledServiceException(404, "파일 정보를 찾을 수 없습니다.");

                // change to test file path
                if (importFilePath.contains("C:\\")) {
                    importFilePath = importFilePath.replaceAll("\\\\", "/");
                    //importFilePath = importFilePath.replaceAll("C:/", "/Users/aloepigeon/");
                    importFilePath = importFilePath.replaceAll("C:/", "/home/ubuntu/");
                }

                File fStatic = new File(importFilePath);
                if (fStatic == null || !fStatic.exists())
                    throw new HandledServiceException(404, "파일을 찾을 수 없습니다. [" + importFilePath + "]");

                String originalFileName = fStatic.getName().replaceAll("_", "");
                String fileExt = originalFileName.substring(originalFileName.lastIndexOf(".") + 1);

                String uploadId = new CryptoField(originalFileName + "_" + fStatic.length()).valueOf();

                upload = getService(RawService.class).getUploadById(uploadId);
                if (upload == null) {
                    upload = new RawVO.Upload();
                    upload.setUploadId(uploadId);
                    upload.setUploadName(new String64(originalFileName));
                    upload.setFileSize(fStatic.length());
                    upload.setRegisterUid(user.getUid());
                    upload.setUploadedAt(LongDate.now());
                    getService(RawService.class).insertRawUpload(upload);
                }

                upload.setStorePath(fStatic.getAbsolutePath());
            }
            getService(RawService.class).updateRawUpload(upload);

            return ResponseHelper.response(200, "Success - Upload Completed", upload);
        }
        catch (Exception e) {
            e.printStackTrace();
            return ResponseHelper.error(500, e.getMessage());
        }
    }

    @RequestMapping(value = "/res/{uploadId}")
    public void openApiRes(HttpServletRequest request,
                           HttpServletResponse response,
                           @PathVariable("uploadId") String uploadId,
                           @RequestParam(value = "d", required = false) Boolean download) throws Exception
    {
        try {
            download = (download == null) ? false: download;

            File uploadRoot = new File(staticLocation.replaceAll("file:", ""), "dynarap/raw");
            if (uploadRoot.exists() == false) uploadRoot.mkdirs();

            CryptoField decUploadId = CryptoField.decode(uploadId, "");
            if (decUploadId == null || decUploadId.isEmpty())
                throw new Exception("파일 정보가 올바르지 않습니다.");

            String uploadKey = decUploadId.originOf();
            String originalFileName = uploadKey.substring(0, uploadKey.lastIndexOf("_"));
            Long fileSize = Long.parseLong(uploadKey.substring(uploadKey.lastIndexOf("_") + 1));
            String fileExt = originalFileName.substring(originalFileName.lastIndexOf(".") + 1);

            Map<String, Object> params = new HashMap<>();
            params.put("uploadName", originalFileName);
            params.put("fileSize", fileSize);

            RawVO.Upload upload = getService(RawService.class).getUploadByParams(params);
            if (upload == null)
                throw new Exception("파일을 찾을 수 없습니다.");

            File fAttach = new File(upload.getStorePath());
            if (fAttach == null || fAttach.exists() == false)
                throw new Exception("파일을 찾을 수 없습니다.");

            if (download) {
                response.setContentType("application/octet-stream");
            }
            else {
                if (fileExt.equalsIgnoreCase("jpg")
                        || fileExt.equalsIgnoreCase("jpeg"))
                    response.setContentType("image/jpeg");
                else if (fileExt.equalsIgnoreCase("png"))
                    response.setContentType("image/png");
                else if (fileExt.equalsIgnoreCase("gif"))
                    response.setContentType("image/gif");
                else if (fileExt.equalsIgnoreCase("svg"))
                    response.setContentType("image/svg+xml");
                else if (fileExt.equalsIgnoreCase("mp4"))
                    response.setContentType("video/mp4");
                else
                    response.setContentType("application/octet-stream");
            }

            String browserName = getBrowser(request);
            String downloadFileName = getDownloadFileName(browserName, originalFileName);

            response.addHeader("Content-Disposition",
                    "attachment; filename=\"" + downloadFileName + "\";");
            response.addHeader("Content-Transfer-Encoding", "binary");

            FileInputStream fis = new FileInputStream(fAttach);
            BufferedInputStream bis = new BufferedInputStream(fis);
            byte[] readBuf = new byte[1024 * 256];
            OutputStream os = response.getOutputStream();
            int readLen = 0;
            while ((readLen = bis.read(readBuf, 0, 1024 * 256)) != -1) {
                os.write(readBuf, 0, readLen);
            }
            os.flush();
        }
        catch (Exception e) {
            e.printStackTrace();

            response.setContentType("application/json");
            try {
                String errorResponse = ServerConstants.GSON.toJson(ResponseHelper.error(500, e.getMessage()));
                OutputStream os = response.getOutputStream();
                os.write(errorResponse.getBytes("UTF-8"));
                os.flush();
            }
            catch (Exception ex) {
                ex.printStackTrace();
            }
        }
    }

    @RequestMapping(value = "/redis")
    @ResponseBody
    public Object openApiRedisTest(@RequestBody JsonObject payload) throws HandledServiceException {
        String command = payload.get("command").getAsString();

        if (command.equals("list-ops")) {
            List<String> params = listOps.range(payload.get("partKey").getAsString(), 0, Integer.MAX_VALUE);
            for (String p : params)
                System.out.println(p);
        }

        if (command.equals("zset-ops")) {
            JsonArray jarrParams = payload.get("paramSet").getAsJsonArray();
            for (int i = 0; i < jarrParams.size(); i++) {
                Long paramKey = jarrParams.get(i).getAsLong();
                ParamVO paramInfo = getService(ParamService.class).getPresetParamBySeq(new CryptoField(paramKey));

                System.out.println("param=" + paramInfo.getFltpKey() + "," + paramInfo.getFltsKey());
                Set<String> listSet = zsetOps.rangeByScore(payload.get("partKey").getAsString() + ".N" + paramKey, 0, Integer.MAX_VALUE);
                Set<String> rowSet = zsetOps.rangeByScore(payload.get("partKey").getAsString() + ".R", 0, Integer.MAX_VALUE);

                Iterator<String> iterListSet = listSet.iterator();
                Iterator<String> iterRowSet = rowSet.iterator();

                int dataOrder = 1;
                while (iterListSet.hasNext()) {
                    System.out.println(iterRowSet.next() + " = " + iterListSet.next());
                    dataOrder++;
                }
            }
        }

        return ResponseHelper.response(200, "Success - Redis Test", "");
    }

    @Data
    @EqualsAndHashCode(callSuper = false)
    public static class EmailAuthVO {
        private CryptoField seq;
        private String email;
        private transient String authNo;
        private LongDate requestAt;
        private boolean authorized;
    }

}
