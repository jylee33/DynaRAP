using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class SalesData
    {
        static int UniqueID = 37;
        public SalesData()
        {
            ID = UniqueID++;
        }
        public SalesData(int id, int regionId, string region, decimal marchSales, decimal septemberSales, decimal marchSalesPrev, decimal septermberSalesPrev, double marketShare)
        {
            ID = id;
            RegionID = regionId;
            Region = region;
            MarchSales = marchSales;
            SeptemberSales = septemberSales;
            MarchSalesPrev = marchSalesPrev;
            SeptemberSalesPrev = septermberSalesPrev;
            MarketShare = marketShare;
        }
        public int ID { get; set; }
        public int RegionID { get; set; }
        public string Region { get; set; }
        public decimal MarchSales { get; set; }
        public decimal SeptemberSales { get; set; }
        public decimal MarchSalesPrev { get; set; }
        public decimal SeptemberSalesPrev { get; set; }

        [DisplayFormat(DataFormatString = "p0")]
        public double MarketShare { get; set; }
    }

    public class SalesDataGenerator
    {
        public static List<SalesData> CreateData()
        {
            List<SalesData> sales = new List<SalesData>();
            sales.Add(new SalesData(0, -1, "Western Europe", 30540, 33000, 32220, 35500, .70));
            sales.Add(new SalesData(1, 0, "Austria", 22000, 28000, 26120, 28500, .92));
            sales.Add(new SalesData(2, 0, "France", 23020, 27000, 20120, 29200, .51));
            sales.Add(new SalesData(3, 0, "Germany", 30540, 33000, 32220, 35500, .93));
            sales.Add(new SalesData(4, 0, "Spain", 12900, 10300, 14300, 9900, .82));
            sales.Add(new SalesData(5, 0, "Switzerland", 9323, 10730, 7244, 9400, .14));
            sales.Add(new SalesData(6, 0, "United Kingdom", 14580, 13967, 15200, 16900, .91));

            sales.Add(new SalesData(17, -1, "Eastern Europe", 22500, 24580, 21225, 22698, .62));
            sales.Add(new SalesData(18, 17, "Belarus", 7315, 18800, 8240, 17480, .34));
            sales.Add(new SalesData(19, 17, "Bulgaria", 6300, 2821, 5200, 10880, .8));
            sales.Add(new SalesData(20, 17, "Croatia", 4200, 3890, 3880, 4430, .29));
            sales.Add(new SalesData(21, 17, "Russia", 22500, 24580, 21225, 22698, .85));

            sales.Add(new SalesData(26, -1, "North America", 31400, 32800, 30300, 31940, .84));
            sales.Add(new SalesData(27, 26, "USA", 31400, 32800, 30300, 31940, .87));
            sales.Add(new SalesData(28, 26, "Canada", 25390, 27000, 5200, 29880, .64));

            sales.Add(new SalesData(29, -1, "South America", 16380, 17590, 15400, 16680, .32));
            sales.Add(new SalesData(30, 29, "Argentina", 16380, 17590, 15400, 16680, .88));
            sales.Add(new SalesData(31, 29, "Brazil", 4560, 9480, 3900, 6100, .10));

            sales.Add(new SalesData(32, -1, "Asia", 20388, 22547, 22500, 25756, .52));
            sales.Add(new SalesData(34, 32, "India", 4642, 5320, 4200, 6470, .44));
            sales.Add(new SalesData(35, 32, "Japan", 9457, 12859, 8300, 8733, .70));
            sales.Add(new SalesData(36, 32, "China", 20388, 22547, 22500, 25756, .82));
            return sales;
        }
    }

}
