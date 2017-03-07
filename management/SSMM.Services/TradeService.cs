using SSMM.Cache;
using SSMM.Entity;
using SSMM.Helper;
using SSMM.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SSMM.Services
{
    public class TradeService
    {

        /// <summary>
        /// 交易号是否存在
        /// </summary>
        public static bool TradeNoIsExist(string tradeno, PaymentType type)
        {
            using (var DB = new SSMMEntities())
            {
                var trade = DB.Order.SingleOrDefault(x => x.TradeNumber == tradeno && x.Type == type.ToString());
                return trade != null;
            }
        }
        
        /// <summary>
        /// 生成支付宝转账唯一code
        /// </summary>
        public static string GetAlipayTransferCode(string email, int minlength = 6)
        {
            var code = "";
            var user = UserCache.Cache.GetValue(email);
            using (var DB = new SSMMEntities())
            {
                var count = DB.Order.Count(x => x.UserId == user.Id && x.Type == PaymentType.支付宝转账.ToString() && x.Status == 1);//已转账的次数
                code = $"{user.Id}{count}";
            }
            if (code.Length < minlength)
                code = FormatHelper.RandomNum(minlength - code.Length) + code;
            return code;
        }

        /// <summary>
        /// 通过Api查询支付宝转账[交易号]
        /// </summary>
        private static bool QueryAlipayTradeNoSearchApi(string tradeno, out decimal amount, out string remark)
        {
            amount = 0;
            remark = "";
            var code = "";
            var apiurl = SettingCache.Cache.Get(SettingFlag.AlipayTradeNoSearchApi);
            if (string.IsNullOrEmpty(apiurl))
                return false;
            apiurl += tradeno;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(apiurl);
                request.Method = "get";
                var response = (HttpWebResponse)request.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    code = sr.ReadToEnd();
                }
            }
            catch (Exception)
            {
                code = "";
            }
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }
            var result = FormatHelper.JsonDeserializer<AlipayTradeResult>(code);
            if (result == null || result.IsSuccess == false)
            {
                return false;
            }
            amount = result.Amount1;
            remark = result.Remark;
            return true;
        }

        /// <summary>
        /// 通过Api查询支付宝转账[备注]
        /// </summary>
        public static bool QueryAlipayRemarkSearchApi(string info, out decimal amount, out string remark)
        {
            amount = 0;
            remark = "";
            var code = "";
            var apiurl = SettingCache.Cache.Get(SettingFlag.AlipayRemarkSearchApi);
            if (string.IsNullOrEmpty(apiurl))
                return false;
            apiurl += info;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(apiurl);
                request.Method = "get";
                request.Timeout = 1000 * 5;
                var response = (HttpWebResponse)request.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    code = sr.ReadToEnd();
                }
            }
            catch (Exception)
            {
                code = "";
                remark = "支付服务响应失败！请稍后再试！";
            }
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }
            var result = FormatHelper.JsonDeserializer<AlipayTradeResult>(code);
            if (result == null || result.IsSuccess == false)
            {
                return false;
            }
            amount = result.Amount1;
            remark = result.Remark;
            return true;
        }

        /// <summary>
        /// 支付参数加密签名
        /// </summary>
        /// <returns></returns>
        public static string ParameterSign(string uid, string pid, string pwd, string pcode, int timestamp)
        {
            var Key = "AutumnSS";
            return FormatHelper.GetMD5ByString($"uid={uid}&pid={pid}&pwd={pwd}&pcode={pcode}&timestamp={timestamp}&key={Key}");
        }

        /// <summary>
        /// 校验参数签名，返回最终订单金额
        /// </summary>
        public static bool CheckParameterSign(string uid, string pid, string pwd, string pcode, int timestamp, string sign, PaymentType type, out string info, out decimal amount)
        {
            info = "";
            amount = 0;
            //校验参数
            var ServerSign = ParameterSign(uid, pid, pwd, pcode, timestamp);
            if (sign != ServerSign)
            {
                info = "签名错误！请重新支付！";
                return false;
            }
            var time = FormatHelper.GetTime(timestamp);
            if ((DateTime.Now - time).TotalMinutes > 30)
            {
                info = "支付已过期！请重新支付！";
                return false;
            }
            using (var DB = new SSMMEntities())
            {
                var user = DB.User.SingleOrDefault(x => x.Email == uid);
                if (user == null)
                {
                    info = "该用户不存在！请重新支付！";
                    return false;
                }
                var product = DB.Product.Find(pid);
                if (product == null)
                {
                    info = "该产品不存在！请重新支付！";
                    return false;
                }
                amount = product.Price;
                //优惠码
                var couponcode = new CouponCodeDto();
                if (CouponCodeService.Query(pcode, out couponcode))
                {
                    amount = product.Price - couponcode.Amount;
                    amount = amount < 0 ? 0 : amount;
                }
                else
                {
                    if (!string.IsNullOrEmpty(pcode))
                    {
                        info = "优惠码已过期或已超过最大使用次数！请重新支付！";
                        return false;
                    }
                }
                //支付方式为 支付宝转账 优惠后的价格为0
                if (type == PaymentType.支付宝转账)
                {
                    if (amount == 0)
                    {
                        info = "支付成功！";
                        return true;
                    }
                }
                //支付方式为 账户余额支付 需要校验余额
                if (type == PaymentType.账户余额)
                {
                    if (user.Balance < amount)
                    {
                        info = "账户余额不足！";
                        return false;
                    }
                }

                return true;
            }
        }


        #region 账户余额支付

        /// <summary>
        /// 账户余额支付
        /// </summary>
        public static bool PayMentForAccountBalance(string uid, string pid, string pwd, string pcode, int timestamp, string sign, out string info)
        {
            info = "";
            decimal amount = 0;
            if (!CheckParameterSign(uid, pid, pwd, pcode, timestamp, sign, PaymentType.账户余额, out info, out amount))
                return false;
            using (var DB = new SSMMEntities())
            {
                var user = DB.User.SingleOrDefault(x => x.Email == uid);
                var product = DB.Product.Find(pid);
                //扣除账户余额
                user.Balance -= amount;
                //生成订单
                var tradeno = FormatHelper.GenerateOutTradeNo();
                DB.Order.Add(new Order()
                {
                    TradeNumber = tradeno,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Amount = amount,
                    CreateTime = DateTime.Now,
                    Status = 1,
                    Type = PaymentType.账户余额.ToString(),
                    UserId = user.Id
                });
                //ss信息
                var ss = DB.SS.SingleOrDefault(x => x.userid == user.Id);
                if (ss == null)
                {
                    DB.SS.Add(new SS()
                    {
                        id = tradeno,
                        t = FormatHelper.ConvertDateTimeInt(DateTime.Now),//ss服务端更新时间
                        u = 0,//上传流量
                        d = 0,//下载流量
                        transfer_enable = FormatHelper.GB2B(product.Traffic),//long 总套餐流量
                        port = SSService.GetNextPort(),//端口
                        password = pwd,//密码
                        @switch = 1,
                        enable = 1,
                        isrest = product.IsRest,//是否重置流量
                        last_rest_time = FormatHelper.ConvertDateTimeInt(DateTime.Now.AddDays(30)),//重置流量时间
                        expiration_time = FormatHelper.ConvertDateTimeInt(DateTime.Now.AddDays(product.ExpirationDay)),//过期时间
                        create_time = FormatHelper.ConvertDateTimeInt(DateTime.Now),
                        userid = user.Id
                    });
                }
                else
                {
                    ss.t = FormatHelper.ConvertDateTimeInt(DateTime.Now);//ss服务端更新时间
                    ss.u = 0;//上传流量
                    ss.d = 0;//下载流量
                    ss.transfer_enable = FormatHelper.GB2B(product.Traffic);//long 总套餐流量
                    ss.password = pwd;//密码
                    ss.@switch = 1;
                    ss.enable = 1;
                    ss.isrest = product.IsRest;//是否重置流量
                    ss.last_rest_time = FormatHelper.ConvertDateTimeInt(DateTime.Now.AddDays(30));//重置流量时间
                    ss.expiration_time = FormatHelper.ConvertDateTimeInt(DateTime.Now.AddDays(product.ExpirationDay));//过期时间
                    ss.create_time = FormatHelper.ConvertDateTimeInt(DateTime.Now);
                }
                //返佣
                if (user.ParentId.Value != 0)
                {
                    var parent = DB.User.Find(user.ParentId.Value);
                    if (parent != null)
                    {
                        var num = Convert.ToInt32(SettingCache.Cache.Get(SettingFlag.RebateNum)) * 0.01;
                        var agentamounts = amount * (decimal)num;//返佣金额为最终优惠后的实际交易价格的返佣百分比
                        parent.Balance += agentamounts;
                        DB.Record.Add(new Record()
                        {
                            Amount = agentamounts,
                            CreateTime = DateTime.Now,
                            Type = RecordType.返佣.ToString(),
                            Info = $"用户[{user.Id}]{user.Email}购买了{product.Name},产品价格{product.Price.ToString("0.00")}元,最终交易价格{amount.ToString("0.00")}元,返佣百分比{num * 100}%,返佣金额{agentamounts.ToString("0.00")}元",
                            UserId = parent.Id,
                            Remark = PaymentType.账户余额.ToString()
                        });
                    }
                }
                if (DB.SaveChanges() > 0)
                {
                    info = "支付成功！";
                    return true;
                }
                else
                {
                    info = "发现错误！支付失败！";
                    return false;
                }
            }

        }

        #endregion


        #region 支付宝转账支付


        /// <summary>
        /// 支付宝转账支付
        /// </summary>
        public static bool PayMentForAlipayTransfer(string uid, string pid, string pwd, string pcode, out string info)
        {
            info = "";
            decimal amount = 0;
            using (var DB = new SSMMEntities())
            {
                var user = DB.User.SingleOrDefault(x => x.Email == uid);
                var product = DB.Product.Find(pid);
                if (product == null)
                {
                    info = "该产品不存在！请重新支付！";
                    return false;
                }
                //优惠码
                var couponcode = new CouponCodeDto();
                if (CouponCodeService.Query(pcode, out couponcode))
                {
                    amount = product.Price - couponcode.Amount;
                    amount = amount < 0 ? 0 : amount;
                }
                else
                {
                    if (!string.IsNullOrEmpty(pcode))
                    {
                        info = "优惠码已过期或已超过最大使用次数！请重新支付！";
                        return false;
                    }
                }
                var tradeno = FormatHelper.GenerateOutTradeNo();
                //生成订单
                DB.Order.Add(new Order()
                {
                    TradeNumber = tradeno,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Amount = amount,
                    CreateTime = DateTime.Now,
                    Status = 1,
                    Type = PaymentType.支付宝转账.ToString(),
                    UserId = user.Id
                });
                //ss信息
                var ss = DB.SS.SingleOrDefault(x => x.userid == user.Id);
                if (ss == null)
                {
                    DB.SS.Add(new SS()
                    {
                        id = tradeno,
                        t = FormatHelper.ConvertDateTimeInt(DateTime.Now),//ss服务端更新时间
                        u = 0,//上传流量
                        d = 0,//下载流量
                        transfer_enable = FormatHelper.GB2B(product.Traffic),//long 总套餐流量
                        port = SSService.GetNextPort(),//端口
                        password = pwd,//密码
                        @switch = 1,
                        enable = 1,
                        isrest = product.IsRest,//是否重置流量
                        last_rest_time = FormatHelper.ConvertDateTimeInt(DateTime.Now.AddDays(30)),//重置流量时间
                        expiration_time = FormatHelper.ConvertDateTimeInt(DateTime.Now.AddDays(product.ExpirationDay)),//过期时间
                        create_time = FormatHelper.ConvertDateTimeInt(DateTime.Now),
                        userid = user.Id
                    });
                }
                else
                {
                    ss.t = FormatHelper.ConvertDateTimeInt(DateTime.Now);//ss服务端更新时间
                    ss.u = 0;//上传流量
                    ss.d = 0;//下载流量
                    ss.transfer_enable = FormatHelper.GB2B(product.Traffic);//long 总套餐流量
                    ss.password = pwd;//密码
                    ss.@switch = 1;
                    ss.enable = 1;
                    ss.isrest = product.IsRest;//是否重置流量
                    ss.last_rest_time = FormatHelper.ConvertDateTimeInt(DateTime.Now.AddDays(30));//重置流量时间
                    ss.expiration_time = FormatHelper.ConvertDateTimeInt(DateTime.Now.AddDays(product.ExpirationDay));//过期时间
                    ss.create_time = FormatHelper.ConvertDateTimeInt(DateTime.Now);
                }
                //返佣
                if (user.ParentId.Value != 0)
                {
                    var parent = DB.User.Find(user.ParentId.Value);
                    if (parent != null)
                    {
                        var num = Convert.ToInt32(SettingCache.Cache.Get(SettingFlag.RebateNum)) * 0.01;
                        var agentamounts = amount * (decimal)num;//返佣金额为最终优惠后的实际交易价格的返佣百分比
                        parent.Balance += agentamounts;
                        DB.Record.Add(new Record()
                        {
                            Amount = agentamounts,
                            CreateTime = DateTime.Now,
                            Type = RecordType.返佣.ToString(),
                            Info = $"用户[{user.Id}]{user.Email}购买了{product.Name},产品价格{product.Price.ToString("0.00")}元,最终交易价格{amount.ToString("0.00")}元,返佣百分比{num * 100}%,返佣金额{agentamounts.ToString("0.00")}元",
                            UserId = parent.Id,
                            Remark = PaymentType.支付宝转账.ToString()
                        });
                    }
                }
                if (DB.SaveChanges() > 0)
                {
                    info = "支付成功！";
                    return true;
                }
                else
                {
                    info = "发现错误！支付失败！";
                    return false;
                }
            }

        }

        #endregion


        public static List<RecordDto> GetRecordList(int offset, int limit, out int totalcount, string key = null)
        {
            var records = new List<RecordDto>();
            using (var DB = new SSMMEntities())
            {
                var list = DB.Record.Where(x => true);
                if (!string.IsNullOrEmpty(key))
                    list = list.Where(x => x.Type.Contains(key) || x.Remark.Contains(key));
                totalcount = list.Count();
                var result = list.OrderByDescending(x => x.CreateTime)
                                  .Skip(offset)
                                  .Take(limit)
                                  .ToList();
                result.ForEach(x =>
                {
                    records.Add(new RecordDto()
                    {
                        Id = x.Id,
                        Amount = x.Amount,
                        CreateTime = x.CreateTime,
                        Type = x.Type,
                        Info = x.Info,
                        UserId = x.UserId,
                        Remark = x.Remark
                    });
                });
            }
            return records;
        }

    }
}
