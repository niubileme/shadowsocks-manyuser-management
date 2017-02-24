using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMM.Model
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string TradeNumber { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreateTime { get; set; }
        public sbyte Status { get; set; }
        public string Type { get; set; }
        public int UserId { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
