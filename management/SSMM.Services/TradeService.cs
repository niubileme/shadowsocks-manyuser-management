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
        /// 支付宝转账
        /// </summary>
        public static bool AlipayTransfer(string email, string tradeno, out string info)
        {
            info = "";
            //验证是否已记录
            if (TradeNoIsExist(tradeno, PaymentType.支付宝转账))
            {
                info = "该交易号已充值！";
                return false;
            }  
            //查询转账记录
            var remark = "";
            decimal amount = 0;
            if (!QueryAlipayTradeRecord(tradeno, out amount, out remark))
            {
                info = "没有找到相关充值记录！请确定交易号是否正确！或系统有延迟，稍后再试。";
                return false;
            }
            var CurrentUser = UserCache.Cache.GetValue(email);
            if (CurrentUser == null)
            {
                info = "请确保账号正确，且正常登录！";
                return false;
            }
            //插入记录
            using (var DB = new SSMMEntities())
            {
                //增加余额
                var user = DB.User.Find(CurrentUser.Id);
                user.Balance += amount;
                //插入订单
                DB.Order.Add(new Order()
                {
                    TradeNumber = GenerateOutTradeNo(),
                    ProductId = "",
                    ProductName = "",
                    Amount = amount,
                    CreateTime = DateTime.Now,
                    Status = 1,
                    Type = PaymentType.支付宝转账.ToString(),
                    UserId = user.Id
                });
                if (DB.SaveChanges() > 0)
                {
                    info = $"充值成功！金额：{amount.ToString("0.00")}元";
                    return true;
                }
                else
                {
                    info = "出现异常！请稍后再试！";
                    return false;
                }
            }
        }

        /// <summary>
        /// 生成唯一订单号
        /// </summary>
        public static string GenerateOutTradeNo()
        {
            var ran = new Random();
            return string.Format("{0}{1}{2}", DateTime.Now.ToString("yyyyMMddHHmmssfff"), ran.Next(1000, 9999), ran.Next(1000, 9999));
        }

        /// <summary>
        /// 通过Api接口查询支付宝转账
        /// </summary>
        private static bool QueryAlipayTradeRecord(string tradeno, out decimal amount, out string remark)
        {
            amount = 0;
            remark = "";
            var code = "";
            var apiurl = SettingCache.Cache.Get(SettingFlag.AlipaySearchApi);
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
        /// 订单列表
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="totalcount"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<OrderDto> GetOrderList(int offset, int limit, out int totalcount, string key = null)
        {
            var models = new List<OrderDto>();
            using (var DB = new SSMMEntities())
            {
                totalcount = DB.Order.Count();

                var list = (from T1 in DB.Order
                            join T2 in DB.User
                            on T1.UserId equals T2.Id
                            select new OrderDto
                            {
                                Id = T1.Id,
                                TradeNumber = T1.TradeNumber,
                                ProductId = T1.ProductId,
                                ProductName = T1.ProductName,
                                Amount = T1.Amount,
                                CreateTime = T1.CreateTime,
                                Status = T1.Status,
                                Type = T1.Type,
                                UserId = T1.UserId,
                                UserName = T2.UserName,
                                Email = T2.Email
                            }
                            );
                if (!string.IsNullOrEmpty(key))
                    list = list.Where(x => x.TradeNumber.Contains(key) || x.ProductName.Contains(key));

                var result = list.OrderByDescending(x => x.CreateTime)
                                  .Skip(offset)
                                  .Take(limit)
                                  .ToList();
                result.ForEach(x =>
                {
                    models.Add(new OrderDto()
                    {
                        Id = x.Id,
                        TradeNumber = x.TradeNumber,
                        ProductId = x.ProductId,
                        ProductName = x.ProductName,
                        Amount = x.Amount,
                        CreateTime = x.CreateTime,
                        Status = x.Status,
                        Type = x.Type,
                        UserId = x.UserId
                    });
                });
            }
            return models;
        }

        /// <summary>
        /// 提现列表
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="totalcount"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<BillDto> GetBillList(int offset, int limit, out int totalcount, string key = null)
        {
            var models = new List<BillDto>();
            using (var DB = new SSMMEntities())
            {
                var list = DB.Bills.Where(x => true);
                if (!string.IsNullOrEmpty(key))
                    list = list.Where(x => x.TransferAccount.Contains(key));
                totalcount = list.Count();
                var result = list.OrderByDescending(x => x.CreateTime)
                                  .Skip(offset)
                                  .Take(limit)
                                  .ToList();
                result.ForEach(x =>
                {
                    models.Add(new BillDto()
                    {
                        Id = x.Id,
                        Amount = x.Amount,
                        CreateTime = x.CreateTime,
                        Type = x.Type,
                        UserId = x.UserId,
                        Status = x.Status,
                        TransferAccount = x.TransferAccount
                    });
                });
            }
            return models;
        }
    }
}
