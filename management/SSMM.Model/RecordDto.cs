using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMM.Model
{
    public class RecordDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string Type { get; set; }
        public string Info { get; set; }
        public int UserId { get; set; }
        public string Remark { get; set; }
    }
}
