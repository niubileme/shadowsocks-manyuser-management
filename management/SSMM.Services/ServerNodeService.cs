using SSMM.Entity;
using SSMM.Helper;
using SSMM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMM.Services
{
    public class ServerNodeService
    {

        public static List<ServerNodeDto> GetAll(MyProductDto my)
        {
            var nodes = new List<ServerNodeDto>();
            using (var DB = new SSMMEntities())
            {
                var result = DB.ServerNode.OrderByDescending(x => x.SortNum).ToList();
                result.ForEach(x =>
                {
                    nodes.Add(new ServerNodeDto()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        IP = x.IP,
                        CNAME = x.CNAME,
                        Description = x.Description,
                        Status = x.Status,
                        SortNum = x.SortNum,
                        QRCode = $"ss://{FormatHelper.EncodeBase64ByUTF8($"rc4-md5:{my.password}@{x.IP}:{my.port}")}"
                    });
                });
            }
            return nodes;

        }



        /// <summary>
        /// 节点列表
        /// </summary>
        public static List<ServerNodeDto> GetList(int offset, int limit, out int totalcount, string key = null)
        {
            var nodes = new List<ServerNodeDto>();
            using (var DB = new SSMMEntities())
            {
                var list = DB.ServerNode.Where(x => true);
                if (!string.IsNullOrEmpty(key))
                    list = list.Where(x => x.CNAME.Contains(key) || x.IP.Contains(key) || x.Name.Contains(key));
                totalcount = list.Count();
                var result = list.OrderByDescending(x => x.SortNum)
                                  .Skip(offset)
                                  .Take(limit)
                                  .ToList();
                result.ForEach(x =>
                {
                    nodes.Add(new ServerNodeDto()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        IP = x.IP,
                        CNAME = x.CNAME,
                        Description = x.Description,
                        Status = x.Status,
                        SortNum = x.SortNum
                    });
                });
            }
            return nodes;
        }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static bool Add(ServerNodeDto dto)
        {
            using (var DB = new SSMMEntities())
            {
                DB.ServerNode.Add(new ServerNode()
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    IP = dto.IP,
                    CNAME = dto.CNAME,
                    Description = dto.Description,
                    Status = dto.Status,
                    SortNum = dto.SortNum,
                });
                return DB.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 更新节点
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static bool Update(ServerNodeDto dto)
        {
            using (var DB = new SSMMEntities())
            {
                var node = DB.ServerNode.Find(dto.Id);
                if (node != null)
                {
                    node.Name = dto.Name;
                    node.IP = dto.IP;
                    node.CNAME = dto.CNAME;
                    node.Description = dto.Description;
                    node.Status = dto.Status;
                    node.SortNum = dto.SortNum;
                }
                return DB.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Delete(string id)
        {
            using (var DB = new SSMMEntities())
            {
                var node = DB.ServerNode.Find(id);
                if (node != null)
                {
                    DB.ServerNode.Remove(node);
                }
                return DB.SaveChanges() > 0;
            }
        }

    }
}
