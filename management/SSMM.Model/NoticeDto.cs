using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMM.Model
{
    public class NoticeDto
    {
        public string Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public sbyte IsDelete { get; set; }
    }
}
