using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class OverviewDataPoint
    {
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public OverviewDataPoint(DateTime date, double value)
        {
            this.Date = date;
            this.Value = value;
        }
        public static List<OverviewDataPoint> GetDataPoints()
        {
            List<OverviewDataPoint> data = new List<OverviewDataPoint> {
                new OverviewDataPoint(new DateTime(2019, 6, 1, 0, 0, 0), 56.1226),
                new OverviewDataPoint(new DateTime(2019, 6, 1, 3, 0, 0), 50.18432),
                new OverviewDataPoint(new DateTime(2019, 6, 1, 6, 0, 0), 51.51443),
                new OverviewDataPoint(new DateTime(2019, 6, 1, 9, 0, 0), 60.2624),
                new OverviewDataPoint(new DateTime(2019, 6, 1, 12, 0, 0), 64.04412),
                new OverviewDataPoint(new DateTime(2019, 6, 1, 15, 0, 0), 66.56123),
                new OverviewDataPoint(new DateTime(2019, 6, 1, 18, 0, 0), 65.48127),
                new OverviewDataPoint(new DateTime(2019, 6, 1, 21, 0, 0), 60.4412),
                new OverviewDataPoint(new DateTime(2019, 6, 2, 0, 0, 0), 57.2341),
                new OverviewDataPoint(new DateTime(2019, 6, 2, 3, 0, 0), 52.3469),
                new OverviewDataPoint(new DateTime(2019, 6, 2, 6, 0, 0), 51.82341),
                new OverviewDataPoint(new DateTime(2019, 6, 2, 9, 0, 0), 61.532),
                new OverviewDataPoint(new DateTime(2019, 6, 2, 12, 0, 0), 63.8641),
                new OverviewDataPoint(new DateTime(2019, 6, 2, 15, 0, 0), 65.12374),
                new OverviewDataPoint(new DateTime(2019, 6, 2, 18, 0, 0), 65.6321)};

            //List<OverviewDataPoint> data = new List<OverviewDataPoint>();

            //Random rand = new Random();
            //int randValue = 0;

            //for (int i = 1; i < 2022; i++)
            //{
            //    randValue = rand.Next();

            //    data.Add(new OverviewDataPoint(new DateTime(i, 6, 1, 0, 0, 0), randValue));
            //}

            return data;
        }
    }
}
