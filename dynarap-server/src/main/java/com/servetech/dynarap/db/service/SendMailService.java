package com.servetech.dynarap.db.service;

import com.amazonaws.AmazonClientException;
import com.amazonaws.auth.AWSStaticCredentialsProvider;
import com.amazonaws.auth.BasicAWSCredentials;
import com.amazonaws.services.simpleemail.AmazonSimpleEmailService;
import com.amazonaws.services.simpleemail.AmazonSimpleEmailServiceClientBuilder;
import com.amazonaws.services.simpleemail.model.*;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;

import java.util.List;

@Service("sendMailService")
public class SendMailService {
    @Value("${neoulsoft.email.aws-access-key}")
    private String emailAccessKey;

    @Value("${neoulsoft.email.aws-secret-key}")
    private String emailSecretKey;

    public void send(String from, List<String> recipients, String subject, String content) {
        try {
            BasicAWSCredentials awsCreds = new BasicAWSCredentials(emailAccessKey, emailSecretKey);
            AWSStaticCredentialsProvider credentialsProvider = new AWSStaticCredentialsProvider(awsCreds);

            try {
                credentialsProvider.getCredentials();
            } catch (Exception e) {
                throw new AmazonClientException("Credentials not valid.", e);
            }

            AmazonSimpleEmailService client = AmazonSimpleEmailServiceClientBuilder.standard()
                    .withCredentials(credentialsProvider)
                    .withRegion("ap-northeast-2")
                    .build();

            // Send the email.
            Destination destination = new Destination().withToAddresses(recipients);
            Message message = new Message()
                    .withSubject(createContent(subject))
                    .withBody(new Body().withHtml(createContent(content)));
            SendEmailRequest sendEmailRequest = new SendEmailRequest()
                    .withSource(from)
                    .withDestination(destination)
                    .withMessage(message);
            client.sendEmail(sendEmailRequest);
        } catch (Exception ex) {
            throw new AmazonClientException(ex.getMessage(), ex);
        }
    }

    private Content createContent(String content) {
        return new Content().withCharset("UTF-8").withData(content);
    }
}
