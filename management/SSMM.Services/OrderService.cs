using SSMM.Cache;
using SSMM.Entity;
using SSMM.Helper;
using SSMM.Model;
using SSMM.Model.Charts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
namespace SSMM.Services
{
    public class OrderService
    {

        /// <summary>
        /// 判断是否为新用户
        /// 没有购买服务的
        /// </summary>
        public static bool IsNew(int userid)
        {
            using (var DB = new SSMMEntities())
            {
                return DB.Order.Count(x => x.UserId == userid) == 0;
            }
        }

        public static decimal GetAmount(DateTime starttime, DateTime endtime)
        {
            using (var DB = new SSMMEntities())
            {
                try
                {
                    return DB.Order.Where(x => x.CreateTime >= starttime && x.CreateTime < endtime && x.Type == PaymentType.支付宝转账.ToString()).Sum(x => x.Amount);

                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }


        public static AmountChartsModel GetAmountCharts(DateTime starttime, DateTime endtime)
        {
            using (var DB = new SSMMEntities())
            {
                var list = DB.Order.Where(x => x.CreateTime >= starttime && x.CreateTime < endtime && x.Type == PaymentType.支付宝转账.ToString()).ToList();
                var list2 = list.GroupBy(x => x.CreateTime.ToString("MM-dd")).OrderBy(x => x.Key).Select(x => new
                {
                    date = x.Key,
                    num = x.Count(),
                    amounts = x.Sum(g => g.Amount)
                });
                var result = new AmountChartsModel();
                foreach (var item in list2)
                {
                    result.xAxis.Add(item.date);
                    result.numSeries.Add(item.num.ToString());
                    result.amountsSeries.Add(item.amounts.ToString());
                }
                return result;
            }
        }

        public static PaymentChartsModel GetOrderPaymentTypeCharts(DateTime starttime, DateTime endtime)
        {
            using (var DB = new SSMMEntities())
            {
                var list = DB.Order.Where(x => x.CreateTime >= starttime && x.CreateTime < endtime);
                var result = new PaymentChartsModel();
                result.AliPayNum = list.Where(x => x.Type == PaymentType.支付宝转账.ToString()).Count();
                result.AccountBalanceNum = list.Where(x => x.Type == PaymentType.账户余额.ToString()).Count();
                return result;
            }
        }

        public static OrderDto Query(string tradeno)
        {
            using (var DB = new SSMMEntities())
            {
                var order = DB.Order.SingleOrDefault(x => x.TradeNumber == tradeno);
                if (order == null)
                    return null;
                return new OrderDto()
                {
                    Id = order.Id,
                    TradeNumber = order.TradeNumber,
                    ProductId = order.ProductId,
                    ProductName = order.ProductName,
                    Price = order.Price,
                    Amount = order.Amount,
                    CreateTime = order.CreateTime,
                    Status = order.Status,
                    Type = order.Type,
                    UserId = order.UserId

                };
            }
        }

        /// <summary>
        /// 订单列表
        /// </summary>
        public static List<OrderDto> GetOrderList(int offset, int limit, out int totalcount, string key = null)
        {
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
                                Price = T1.Price,
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
                    list = list.Where(x => x.TradeNumber.Contains(key) || x.Email.Contains(key) || x.UserId.Equals(key));

                var result = list.OrderByDescending(x => x.CreateTime)
                                  .Skip(offset)
                                  .Take(limit)
                                  .ToList();
                return result;
            }
        }

        /// <summary>
        /// 我的账单
        /// </summary>
        public static List<OrderDto> GetMyOrder(int userid, int offset, int limit, out int totalcount)
        {
            var result = new List<OrderDto>();
            using (var DB = new SSMMEntities())
            {
                totalcount = DB.Order.Count(x => x.UserId == userid);

                var list = DB.Order.Where(x => x.UserId == userid)
                                  .OrderByDescending(x => x.CreateTime)
                                  .Skip(offset)
                                  .Take(limit)
                                  .ToList();
                list.ForEach(x =>
                {
                    result.Add(new OrderDto()
                    {
                        Id = x.Id,
                        TradeNumber = x.TradeNumber,
                        ProductId = x.ProductId,
                        ProductName = x.ProductName,
                        Price = x.Price,
                        Amount = x.Amount,
                        CreateTime = x.CreateTime,
                        Status = x.Status,
                        Type = x.Type,
                        UserId = x.UserId
                    });
                });
                return result;
            }
        }
    }
}
