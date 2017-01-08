using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMM.Model
{
    public class AlipayTradeResult
    {
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 交易号
        /// </summary>
        public string TradeNo { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        [DefaultValue(0)]
        public decimal Amount1 { get; set; }
        /// <summary>
        /// 服务费
        /// </summary>
        [DefaultValue(0)]
        public decimal PostalFee { get; set; }
        /// <summary>
        /// 应付金额
        /// </summary>
        [DefaultValue(0)]
        public decimal Amount2 { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateOn { get; set; }
        /// <summary>
        /// 付款时间
        /// </summary>
        public DateTime PaymentOn { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndOn { get; set; }
        public string Info { get; set; }
    }
}
