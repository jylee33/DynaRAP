package com.servetech.dynarap.controller;

import com.servetech.dynarap.db.service.SendMailService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Arrays;

@Controller
@RequestMapping(value="/sendmail")
public class SendMailController {
    @Autowired
    private SendMailService sendMailService;

    @RequestMapping(value="/test")
    @ResponseBody
    public String sendMailTest() {
        sendMailService.send("neoul@neoulsoft.com", Arrays.asList("aloepigeon@naver.com", "publish.neoulsoft@gmail.com"),
                "제목을 달아봅니다.", "<span style='color: red;'>내용을 입력합니다.</span>");
        return "";
    }
}
