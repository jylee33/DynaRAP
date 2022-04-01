package com.servetech.dynarap.ext;


import com.servetech.dynarap.DynaRAPServerApplication;
import com.servetech.dynarap.db.type.LongDate;
import com.servetech.dynarap.db.type.String64;
import com.servetech.dynarap.vo.ResponseVO;

public class ResponseHelper {
    public static ResponseVO error(int code, String message) {
        boolean debug = false;
        if (DynaRAPServerApplication.global.getEnvironment() != null
                && DynaRAPServerApplication.global.getEnvironment().getProperty("debug") != null) {
            debug = Boolean.parseBoolean(DynaRAPServerApplication.global.getEnvironment().getProperty("debug"));
        }

        ResponseVO voError = new ResponseVO();
        voError.setCode(code);

        if (debug) {
            if (message != null)
                voError.setMessage(message);
            else
                voError.setMessage("Unknown server error.");
        }
        else {
            if (message != null)
                voError.setMessage(new String64(message));
            else
                voError.setMessage(new String64("Unknown server error."));
        }

        voError.setResponseAt(new LongDate(System.currentTimeMillis()));
        voError.setResponse(null);
        return voError;
    }

    public static ResponseVO response(int code, String message, Object response)
            throws HandledServiceException {
        return response(code, message, 0, response, null);
    }

    public static ResponseVO response(int code, String message, int resultCount, Object response)
            throws HandledServiceException {
        return response(code, message, resultCount, response, null);
    }

    public static ResponseVO response(int code, String message, int resultCount, Object response, Object additionalResponse)
            throws HandledServiceException {
        boolean debug = false;
        if (DynaRAPServerApplication.global.getEnvironment() != null
                && DynaRAPServerApplication.global.getEnvironment().getProperty("debug") != null) {
            debug = Boolean.parseBoolean(DynaRAPServerApplication.global.getEnvironment().getProperty("debug"));
        }

        ResponseVO voResponse = new ResponseVO();
        voResponse.setCode(code);

        if (debug)
            voResponse.setMessage(message);
        else
            voResponse.setMessage(new String64(message));

        voResponse.setResponseAt(new LongDate(System.currentTimeMillis()));
        voResponse.setResponse(response);
        voResponse.setAdditionalResponse(additionalResponse);
        voResponse.setResultCount(resultCount);
        return voResponse;
    }
}
