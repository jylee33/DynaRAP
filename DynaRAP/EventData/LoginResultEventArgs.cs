using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.EventData
{
    public class LoginResultEventArgs
    {
        string resultString;
        string reason;

        public string ResultString
        {
            get { return resultString; }
        }
        public string Reason
        {
            get { return reason; }
        }
        public LoginResultEventArgs(string result, string reason)
        {
            this.resultString = result;
            this.reason = reason;
        }
    }
}
