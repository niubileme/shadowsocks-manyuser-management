using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMM.Model
{
    /// <summary>
    /// 优惠码
    /// </summary>
    public class CouponCodeDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Des { get; set; }
        public decimal Amount { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.DateTime ExpirationTime { get; set; }
        public int MaxCount { get; set; }
        public sbyte Status { get; set; }
    }
}
