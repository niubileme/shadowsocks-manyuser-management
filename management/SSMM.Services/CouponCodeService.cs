using SSMM.Entity;
using SSMM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMM.Services
{
    public class CouponCodeService
    {
        public static bool Query(string code, out CouponCodeDto couponCode)
        {
            couponCode = null;
            using (var DB = new SSMMEntities())
            {
                var model = DB.CouponCode.SingleOrDefault(x => x.Code == code);
                if (model == null)
                    return false;
                //判断是否过期
                if (model.Status != 1 || DateTime.Now > model.ExpirationTime)
                    return false;
                couponCode = new CouponCodeDto()
                {
                    Id = model.Id,
                    Code = model.Code,
                    Des = model.Des,
                    Amount = model.Amount,
                    CreateTime = model.CreateTime,
                    ExpirationTime = model.ExpirationTime,
                    MaxCount = model.MaxCount,
                    Status = model.Status
                };
                return true;
            }
        }




    }
}
