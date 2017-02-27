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
        /// 网站Url
        /// </summary>
        WebSiteUrl,
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
        /// 返佣比例
        /// </summary>
        RebateNum,


        /// <summary>
        /// 支付宝账号
        /// </summary>
        AlipayAccount,
        /// <summary>
        /// 支付宝查询接口[交易号]
        /// </summary>
        AlipayTradeNoSearchApi,
        /// <summary>
        /// 支付宝查询接口[备注]
        /// </summary>
        AlipayRemarkSearchApi,

        /// <summary>
        /// 支付过期时间 分钟
        /// </summary>
        PayExpiredMinutes,

        /// <summary>
        /// SS连接的端口范围 分隔符 &
        /// </summary>
        SSPortRange,
    };


    public enum PaymentType
    {
        账户余额,
        支付宝转账,
        微信转账,
        支付宝支付,
        微信支付,
    };


    public enum RecordType
    {
        充值,
        提现,
        返佣,
    };
}
