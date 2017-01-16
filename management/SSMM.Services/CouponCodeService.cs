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



        /// <summary>
        /// 列表
        /// </summary>
        public static List<CouponCodeDto> GetList(int offset, int limit, out int totalcount, string key = null)
        {
            var models = new List<CouponCodeDto>();
            using (var DB = new SSMMEntities())
            {
                var list = DB.CouponCode.Where(x => true);
                if (!string.IsNullOrEmpty(key))
                    list = list.Where(x => x.Code.Contains(key));
                totalcount = list.Count();
                var result = list.OrderByDescending(x => x.CreateTime)
                                  .Skip(offset)
                                  .Take(limit)
                                  .ToList();
                result.ForEach(x =>
                {
                    models.Add(new CouponCodeDto()
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Des = x.Des,
                        Amount = x.Amount,
                        CreateTime = x.CreateTime,
                        ExpirationTime = x.ExpirationTime,
                        MaxCount = x.MaxCount,
                        Status = x.Status
                    });
                });
            }
            return models;
        }

        /// <summary>
        /// 添加
        /// </summary>
        public static bool Add(CouponCodeDto dto)
        {
            using (var DB = new SSMMEntities())
            {
                DB.CouponCode.Add(new CouponCode()
                {
                    Id = dto.Id,
                    Code = dto.Code,
                    Des = dto.Des,
                    Amount = dto.Amount,
                    CreateTime = dto.CreateTime,
                    ExpirationTime = dto.ExpirationTime,
                    MaxCount = dto.MaxCount,
                    Status = dto.Status
                });
                return DB.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        public static bool Update(int id, int status)
        {
            using (var DB = new SSMMEntities())
            {
                var model = DB.CouponCode.Find(id);
                if (model != null)
                {
                    model.Status = (sbyte)status;
                }
                return DB.SaveChanges() > 0;
            }
        }




    }
}
