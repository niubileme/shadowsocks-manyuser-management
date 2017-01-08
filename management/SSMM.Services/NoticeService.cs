using SSMM.Entity;
using SSMM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMM.Services
{
    public class NoticeService
    {
        public static NoticeDto Query(string id)
        {
            using (var DB = new SSMMEntities())
            {
                var notice = DB.Notice.Find(id);
                if (notice == null)
                    return null;
                return new NoticeDto()
                {
                    Id = notice.Id,
                    CreateTime = notice.CreateTime,
                    Title = notice.Title,
                    Contents = notice.Contents,
                    IsDelete = notice.IsDelete
                };
            }
        }

        public static List<NoticeDto> GetList(int num)
        {
            var models = new List<NoticeDto>();
            using (var DB = new SSMMEntities())
            {
                var result = DB.Notice.OrderByDescending(x => x.CreateTime)
                                  .Take(num)
                                  .ToList();
                result.ForEach(x =>
                {
                    models.Add(new NoticeDto()
                    {
                        Id = x.Id,
                        CreateTime = x.CreateTime,
                        Title = x.Title,
                        Contents = x.Contents,
                        IsDelete = x.IsDelete
                    });
                });
            }
            return models;
        }

        /// <summary>
        /// 公告列表
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="totalcount"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<NoticeDto> GetList(int offset, int limit, out int totalcount, string key = null)
        {
            var models = new List<NoticeDto>();
            using (var DB = new SSMMEntities())
            {
                var list = DB.Notice.Where(x => true);
                if (!string.IsNullOrEmpty(key))
                    list = list.Where(x => x.Title.Contains(key));
                totalcount = list.Count();
                var result = list.OrderByDescending(x => x.CreateTime)
                                  .Skip(offset)
                                  .Take(limit)
                                  .ToList();
                result.ForEach(x =>
                {
                    models.Add(new NoticeDto()
                    {
                        Id = x.Id,
                        CreateTime = x.CreateTime,
                        Title = x.Title,
                        Contents = x.Contents,
                        IsDelete = x.IsDelete
                    });
                });
            }
            return models;
        }

        /// <summary>
        /// 添加公告
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static bool Add(NoticeDto dto)
        {
            using (var DB = new SSMMEntities())
            {
                DB.Notice.Add(new Notice()
                {
                    Id = dto.Id,
                    CreateTime = dto.CreateTime,
                    Title = dto.Title,
                    Contents = dto.Contents,
                    IsDelete = dto.IsDelete
                });
                return DB.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 更新公告
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static bool Update(NoticeDto dto)
        {
            using (var DB = new SSMMEntities())
            {
                var model = DB.Notice.Find(dto.Id);
                if (model != null)
                {
                    model.CreateTime = DateTime.Now;
                    model.Title = dto.Title;
                    model.Contents = dto.Contents;
                    model.IsDelete = dto.IsDelete;
                }
                return DB.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 删除公告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Delete(string id)
        {
            using (var DB = new SSMMEntities())
            {
                var model = DB.Notice.Find(id);
                if (model != null)
                {
                    DB.Notice.Remove(model);
                }
                return DB.SaveChanges() > 0;
            }
        }
    }
}
