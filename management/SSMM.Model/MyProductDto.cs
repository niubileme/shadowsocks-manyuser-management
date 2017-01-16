using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMM.Model
{
    public class MyProductDto
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDes { get; set; }
        public int ProductTraffic { get; set; }
        public int ProductExpirationDay { get; set; }
        public decimal ProductPrice { get; set; }
        public sbyte ProductIsRest { get; set; }

        public long d { get; set; }
        public long transfer_enable { get; set; }
        public int port { get; set; }
        public string password { get; set; }
        public bool status { get; set; }
        public sbyte isrest { get; set; }
        public int last_rest_time { get; set; }
        public int expiration_time { get; set; }
        public int create_time { get; set; }
        public int userid { get; set; }
    }
}
