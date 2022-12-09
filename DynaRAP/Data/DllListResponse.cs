using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class DllListResponse : JsonData
    {
        public List<ResponseDLL> response { get; set; }

        public DllListResponse(int code, string message) : base(code, message) { }
    }

    public class ResponseDLL
    {
        public string seq { get; set; }
        public string dataSetCode { get; set; }
        public string dataSetName { get; set; }
        public string dataVersion { get; set; }
        public string registerUid { get; set; }
        public CreatedAt createdAt { get; set; }
    }

    public class DllData
    {
        public DllData()
        {
        }
        public DllData(string seq, string dataSetCode, string dataSetName, string dataVersion, string regTime, int del, int view)
        {
            Seq = seq;
            DataSetCode = dataSetCode;
            DataSetName = dataSetName;
            DataVersion = dataVersion;
            RegTime = regTime;
            Del = del;
            View = view;
        }
        public DllData(string seq, string dataSetCode, string dataSetName, string dataVersion, string regTime, int del, int view, int modify)
        {
            Seq = seq;
            DataSetCode = dataSetCode;
            DataSetName = dataSetName;
            DataVersion = dataVersion;
            RegTime = regTime;
            Del = del;
            View = view;
            Modify = modify;
        }
        public string Seq { get; set; }
        public string DataSetCode { get; set; }
        public string DataSetName { get; set; }
        public string DataVersion { get; set; }
        public string RegTime { get; set; }
        public int Del { get; set; }
        public int View { get; set; }
        public int Modify { get; set; }
    }
}
