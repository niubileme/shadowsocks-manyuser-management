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
    public class OrderService
    {

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
    }
}
