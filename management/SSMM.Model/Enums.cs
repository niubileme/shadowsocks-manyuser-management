using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMM.Model
{
    public enum SettingFlag
    {
        /// <summary>
        /// 网站标题
        /// </summary>
        WebSiteTitle,
        /// <summary>
        /// 网站关键字
        /// </summary>
        WebSiteKeywords,
        /// <summary>
        /// 网站描述
        /// </summary>
        WebSiteDescription,
        /// <summary>
        /// 统计代码
        /// </summary>
        StatisticalCode,


        /// <summary>
        /// 支付宝账号
        /// </summary>
        AlipayAccount,
        /// <summary>
        /// 支付宝查询接口
        /// </summary>
        AlipaySearchApi,

    };


    public enum PaymentType
    {
       支付宝转账,
       微信转账,
       支付宝支付,
       微信支付,
    };


}
