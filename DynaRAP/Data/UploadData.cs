using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class UploadDataResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public List<UploadData> response { get; set; }

        public ResponseAt responseAt { get; set; }
        public int resultCount { get; set; }
    }
    
    public class UploadData
    {
        public string seq { get; set; }
        public string uploadId { get; set; }
        public string dataType { get; set; }
        public string uploadName { get; set; }
        public string storePath { get; set; }
        public int Delete { get; set; }
    }

}
