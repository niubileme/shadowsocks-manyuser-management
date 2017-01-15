using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMM.Model
{
    public class SSDto
    {
        public string id { get; set; }
        public int t { get; set; }
        public long u { get; set; }
        public long d { get; set; }
        public long transfer_enable { get; set; }
        public int port { get; set; }
        public string password { get; set; }
        public sbyte @switch { get; set; }
        public sbyte enable { get; set; }
        public sbyte isrest { get; set; }
        public int last_rest_time { get; set; }
        public int expiration_time { get; set; }
        public int create_time { get; set; }
        public int userid { get; set; }
    }
}
