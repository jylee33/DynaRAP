using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.EventData
{
    public class SelectedRangeEventArgs : EventArgs
    {
        object minValue;
        object maxValue;

        public object MinValue { get => minValue; set => minValue = value; }
        public object MaxValue { get => maxValue; set => maxValue = value; }

        public SelectedRangeEventArgs(object min, object max)
        {
            this.minValue = min;
            this.maxValue = max;
        }
    }
}
