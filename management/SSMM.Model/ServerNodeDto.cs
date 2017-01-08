using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMM.Model
{
    public class ServerNodeDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string IP { get; set; }
        public string CNAME { get; set; }
        public string Description { get; set; }
        public sbyte Status { get; set; }
        public int SortNum { get; set; }
    }
}
