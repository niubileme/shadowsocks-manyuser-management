using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMM.Model
{
    public class BillDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreateTime { get; set; }
        public string Type { get; set; }
        public int UserId { get; set; }
        public sbyte Status { get; set; }
        public string TransferAccount { get; set; }
    }
}
