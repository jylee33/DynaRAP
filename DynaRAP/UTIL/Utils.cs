using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.UTIL
{
    public static class Utils
    {
        public static DateTime GetDateFromJulian(string julianDate)
        {
            string[] splits = julianDate.Split(':');
            var arg = string.Format("{0} {1}:{2}:{3}", new DateTime().AddYears(DateTime.Now.Year - 1).AddDays(double.Parse(splits[0])).ToString("yyyy-MM-dd"), splits[1], splits[2], splits[3]);

            DateTime dt = DateTime.ParseExact(arg, "yyyy-MM-dd HH:mm:ss.ffffff", null);

            return dt;

        }

        public static DateTime GetDateFromJulian2(string julianDate)
        {
            int year = 21;
            int day = Convert.ToInt32(julianDate.Substring(0, 3));
            DateTime dt = new DateTime(1999 + year, 12, 18, new JulianCalendar());

            dt = dt.AddDays(day);
            dt = dt.AddHours(Convert.ToInt32(julianDate.Substring(4, 2)));
            dt = dt.AddMinutes(Convert.ToInt32(julianDate.Substring(7, 2)));
            dt = dt.AddSeconds(Convert.ToInt32(julianDate.Substring(10, 2)));
            dt = dt.AddMilliseconds(Convert.ToInt32(julianDate.Substring(13, 3)));
            dt = dt.AddTicks(Convert.ToInt32(julianDate.Substring(13, 6)) % TimeSpan.TicksPerMillisecond / 100);
            //dt = dt.AddTicks(-Convert.ToInt32(julianDate.Substring(13, 6)) % TimeSpan.TicksPerMillisecond / 100);
            ////myDateTime.AddTicks(-(myDateTime.Ticks % TimeSpan.TicksPerMillisecond) / 100);

            dt = DateTime.Parse(string.Format("{0}-{1}-{2} {3}:{4}:{5}.{6}", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, julianDate.Substring(13, 6)));

            return dt;

        }

        public static DateTime ConvertFromJulian(int m_JulianDate)
        {

            long L = m_JulianDate + 68569;
            long N = (long)((4 * L) / 146097);
            L = L - ((long)((146097 * N + 3) / 4));
            long I = (long)((4000 * (L + 1) / 1461001));
            L = L - (long)((1461 * I) / 4) + 31;
            long J = (long)((80 * L) / 2447);
            int Day = (int)(L - (long)((2447 * J) / 80));
            L = (long)(J / 11);
            int Month = (int)(J + 2 - 12 * L);
            int Year = 2021;// (int)(100 * (N - 49) + I + L);

            DateTime dt = new DateTime(Year, Month, Day);
            return dt;
        }

        public static DateTime JulianToDateTime(double julianDate)
        {
            double unixTime = (julianDate - 2440587.5) * 86400;

            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTime).ToLocalTime();

            return dtDateTime;
        }
    }
}
