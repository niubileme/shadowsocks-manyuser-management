using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMM.Model.Charts
{
    public class AmountChartsModel
    {
        public AmountChartsModel()
        {
            xAxis = new List<string>();
            amountsSeries = new List<string>();
            numSeries = new List<string>();
        }
        public List<string> xAxis { get; set; }
        public List<string> amountsSeries { get; set; }
        public List<string> numSeries { get; set; }
    }
}
