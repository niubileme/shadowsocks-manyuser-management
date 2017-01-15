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
        public static bool CreateOrder(string productid, string productname, decimal price, decimal amount, string type, int userid)
        {
            using (var DB = new SSMMEntities())
            {
                DB.Order.Add(new Order()
                {
                    TradeNumber = FormatHelper.GenerateOutTradeNo(),
                    ProductId = productid,
                    ProductName = productname,
                    Price = price,
                    Amount = amount,
                    CreateTime = DateTime.Now,
                    Status = 1,
                    Type = type,
                    UserId = userid
                });
            }
            return false;
        }
    }
}
