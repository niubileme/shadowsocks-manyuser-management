using SSMM.Entity;
using SSMM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMM.Services
{
    public class ProductService
    {
        public static ProductDto Query(string id)
        {
            using (var DB = new SSMMEntities())
            {
                var product = DB.Product.Find(id);
                if (product == null)
                    return null;
                return new ProductDto()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Traffic = product.Traffic,
                    ExpirationDay = product.ExpirationDay,
                    Price = product.Price,
                    IsRest = product.IsRest,
                    SortNum = product.SortNum
                };
            }
        }

        public static List<ProductDto> GetAll()
        {
            var models = new List<ProductDto>();
            using (var DB = new SSMMEntities())
            {
                var result = DB.Product.OrderByDescending(x => x.SortNum)
                                  .ToList();
                result.ForEach(x =>
                {
                    models.Add(new ProductDto()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Traffic = x.Traffic,
                        ExpirationDay = x.ExpirationDay,
                        Price = x.Price,
                        IsRest = x.IsRest,
                        SortNum = x.SortNum
                    });
                });
            }
            return models;
        }


        /// <summary>
        /// 产品列表
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="totalcount"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<ProductDto> GetList(int offset, int limit, out int totalcount, string key = null)
        {
            var models = new List<ProductDto>();
            using (var DB = new SSMMEntities())
            {
                var list = DB.Product.Where(x => true);
                if (!string.IsNullOrEmpty(key))
                    list = list.Where(x => x.Name.Contains(key));
                totalcount = list.Count();
                var result = list.OrderByDescending(x => x.SortNum)
                                  .Skip(offset)
                                  .Take(limit)
                                  .ToList();
                result.ForEach(x =>
                {
                    models.Add(new ProductDto()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Traffic = x.Traffic,
                        ExpirationDay = x.ExpirationDay,
                        Price = x.Price,
                        IsRest = x.IsRest,
                        SortNum = x.SortNum
                    });
                });
            }
            return models;
        }

        /// <summary>
        /// 添加产品
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static bool Add(ProductDto dto)
        {
            using (var DB = new SSMMEntities())
            {
                DB.Product.Add(new Product()
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Description = dto.Description,
                    Traffic = dto.Traffic,
                    ExpirationDay = dto.ExpirationDay,
                    Price = dto.Price,
                    IsRest = dto.IsRest,
                    SortNum = dto.SortNum
                });
                return DB.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 更新产品
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static bool Update(ProductDto dto)
        {
            using (var DB = new SSMMEntities())
            {
                var model = DB.Product.Find(dto.Id);
                if (model != null)
                {
                    model.Name = dto.Name;
                    model.Description = dto.Description;
                    model.Traffic = dto.Traffic;
                    model.ExpirationDay = dto.ExpirationDay;
                    model.Price = dto.Price;
                    model.IsRest = dto.IsRest;
                    model.SortNum = dto.SortNum;
                }
                return DB.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Delete(string id)
        {
            using (var DB = new SSMMEntities())
            {
                var model = DB.Product.Find(id);
                if (model != null)
                {
                    DB.Product.Remove(model);
                }
                return DB.SaveChanges() > 0;
            }
        }
    }
}
