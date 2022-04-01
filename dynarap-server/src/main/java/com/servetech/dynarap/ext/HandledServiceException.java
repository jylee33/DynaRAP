package com.servetech.dynarap.ext;

public class HandledServiceException extends Exception {
    private int code;
    private String message;

    public HandledServiceException(int code, String message) {
        super(message);

        this.code = code;
        this.message = message;
    }

    public int getCode() {
        return code;
    }

    public void update(int code, String message) {
        this.code = code;
        this.message = message;
    }

    @Override
    public String getMessage() {
        return this.message;
    }
}
