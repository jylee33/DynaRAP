using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class EquationResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public List<EquationSourceResponse> response { get; set; }
        public ResponseAt responseAt { get; set; }
        public int resultCount { get; set; }
    }
    public class EquationSourceResponse
    {
        public string seq { get; set; }
        public string moduleSeq { get; set; }
        public string eqName { get; set; }
        public string equation { get; set; }
        public int eqOrder { get; set; }
        public string julianStartAt { get; set; }
        public string julianEndAt { get; set; }
        public double offsetStartAt { get; set; }
        public double offsetEndAt { get; set; }
        public string dataCount { get; set; }
        public DataProps dataProp { get; set; }
        public string eqNo { get; set; }
    }

    public class EquationSaveRequest
    {
        public string command { get; set; }
        public string moduleSeq { get; set; }
        public List<Equations> equations { get; set; }
    }

    public class Equations
    {
        public string seq { get; set; }
        public string eqName { get; set; }
        public string equation { get; set; }
        public string julianStartAt { get; set; }
        public string julianEndAt { get; set; }
        public double offsetStartAt { get; set; }
        public double offsetEndAt { get; set; }
        public DataProps dataProp { get; set; }
        public Equations()
        {

        }

        public Equations(string eqName, string equation, string julianStartAt, string julianEndAt, double offsetStartAt, double offsetEndAt, DataProps dataProp, string seq = null)
        {
            this.seq = seq;
            this.eqName = eqName;
            this.equation = equation;
            this.julianStartAt = julianStartAt;
            this.julianEndAt = julianEndAt;
            this.offsetStartAt = offsetStartAt;
            this.offsetEndAt = offsetEndAt;
            this.dataProp = dataProp;
        }
    }

    public class EquationGridData
    {
        string seq = null;
        bool eqFalg = false;
        public string eqName { get; set; }
        public string equation { get; set; }
        public string SelectionStart { get; set; }
        public string SelectionEnd { get; set; }
        public string julianStartAt { get; set; }
        public string julianEndAt { get; set; }
        public double offsetStartAt { get; set; }
        public double offsetEndAt { get; set; }
        public int Del { get; set; }
        public int View { get; set; }
        public string DataCnt { get; set; }
        public string tags { get; set; }
        public string Seq { get => seq; set => seq = value; }
        public bool EqFalg { get => eqFalg; set => eqFalg = value; }
        public EquationGridData()
        {
            this.Del = 1;
            this.View = 1;
        }
        public EquationGridData(EquationSourceResponse response)
        {
            this.seq = response.seq;
            this.eqName = response.eqName;
            this.equation = response.equation;
            this.SelectionStart = response.offsetStartAt.ToString();
            this.SelectionEnd = response.offsetEndAt.ToString();
            this.julianStartAt = response.julianStartAt;
            this.julianEndAt = response.julianEndAt;
            this.offsetStartAt = response.offsetStartAt;
            this.offsetEndAt = response.offsetEndAt;
            this.Del = 1;
            this.View = 1;
            this.DataCnt = response.dataCount;
            this.tags = response.dataProp.tags;
            this.EqFalg = true;
        }
      
    }

    public class EvaluationResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public List<double> response { get; set; }
        public ResponseAt responseAt { get; set; }
        public int resultCount { get; set; }
    }
}
