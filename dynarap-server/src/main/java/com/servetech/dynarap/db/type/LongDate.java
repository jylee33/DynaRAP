package com.servetech.dynarap.db.type;

import com.servetech.dynarap.config.ServerConstants;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;

public class LongDate {
    public static final int DAY = 0x01;
    public static final int HOUR = 0x02;
    public static final int MIN = 0x04;
    public static final int SEC = 0x08;
    public static final int MSEC = 0x10;

    private long timestamp = 0L;
    private String dateFormat = null;
    private transient SimpleDateFormat sdf = null;
    private String dateTime = "";

    public LongDate(long timestamp) {
        this(timestamp, ServerConstants.DEFAULT_FORMAT);
    }

    public LongDate(long timestamp, String dateFormat) {
        this.timestamp = timestamp;
        this.dateFormat = dateFormat;
        this.sdf = new SimpleDateFormat(dateFormat);
        this.dateTime = sdf.format(timestamp);
    }

    public String valueOf() {
        return dateTime;
    }

    public long originOf() {
        return timestamp;
    }

    public String getDateFormat() {
        return dateFormat;
    }

    @Override
    public String toString() {
        return ServerConstants.GSON.toJson(this);
    }

    public static LongDate now() {
        return new LongDate(System.currentTimeMillis());
    }
    public static LongDate zero() {
        return new LongDate(0L);
    }

    public static LongDate parse(String dateTime, String dateFormat) {
        SimpleDateFormat sdf_decode = new SimpleDateFormat(dateFormat);
        Date parsed = null;
        try {
            parsed = sdf_decode.parse(dateTime);
        } catch (ParseException e) {
            return null;
        }
        return new LongDate(parsed.getTime());
    }

    public static LongDate lastDiff(int diffType, long diffVal) {
        long currentTime = System.currentTimeMillis();
        long measureDiffVal = diffVal;
        if (diffType == DAY)
            measureDiffVal = (measureDiffVal * 24 * 60 * 60 * 1000);
        else if (diffType == HOUR)
            measureDiffVal = (measureDiffVal * 60 * 60 * 1000);
        else if (diffType == MIN)
            measureDiffVal = (measureDiffVal * 60 * 1000);
        else if (diffType == SEC)
            measureDiffVal = (measureDiffVal * 1000);
        else if (diffType == MSEC)
            measureDiffVal = (measureDiffVal * 1);
        return new LongDate(currentTime - measureDiffVal);
    }

    public static boolean isExpired(int diffType, long diffVal, LongDate diffWhat) {
        LongDate diff = lastDiff(diffType, diffVal);
        return diff.originOf() >= diffWhat.originOf();
    }
}
