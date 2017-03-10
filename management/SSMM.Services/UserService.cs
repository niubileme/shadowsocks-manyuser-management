using AutoMapper;
using SSMM.Cache;
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
    public class UserService
    {
        /// <summary>
        /// 注册用户
        /// </summary>
        public static bool Register(string username, string email, string password, string affcode, out string msg)
        {
            msg = "";
            using (var DB = new SSMMEntities())
            {
                //检测Email是否存在
                var serverUser = DB.User.FirstOrDefault(x => x.Email == email);
                if (serverUser != null)
                {
                    msg = "Email已存在！";
                    return false;
                }
                //aff
                var pid = 0;
                if (!string.IsNullOrEmpty(affcode))
                {
                    var parent = DB.User.FirstOrDefault(x => x.AffCode == affcode);
                    if (parent != null)
                        pid = parent.Id;
                }
                //添加
                DB.User.Add(new User()
                {
                    UserName = username,
                    Email = email,
                    Password = FormatHelper.GetMD5ByString(password),
                    QQ = "",
                    Address = "",
                    Status = 1,
                    Balance = 0,
                    CreateTime = DateTime.Now,
                    IsManager = 0,
                    AffCode = Guid.NewGuid().ToString("n"),
                    ParentId = pid
                });
                if (DB.SaveChanges() > 0)
                {
                    msg = "注册成功！";
                    UserCache.Cache.UpdateCacheValue(email);
                    return true;
                }
                else
                {
                    msg = "注册失败了！";
                    return false;
                }
            }
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool Login(string email, string password, out string msg, out UserDto currentuser)
        {
            msg = "";
            currentuser = null;
            //通过缓存判断
            //var user = UserCache.Cache.GetValue(email);
            //不使用缓存，防止数据库修改后，cache更新不及时
            var user = UserCache.Cache.GetOriginalValue(email);
            if (user == null)
            {
                msg = "该用户不存在！";
                return false;
            }
            if (FormatHelper.GetMD5ByString(password) != user.Password)
            {
                //更新
                user = UserCache.Cache.UpdateCacheValue(email);
                if (FormatHelper.GetMD5ByString(password) != user.Password)
                {
                    msg = "输入的密码不正确！";
                    return false;
                }
            }
            currentuser = user;
            return true;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="totalcount"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<UserDto> GetList(int offset, int limit, out int totalcount, string key = null)
        {
            var users = new List<UserDto>();
            using (var DB = new SSMMEntities())
            {
                var list = DB.User.Where(x => true);
                if (!string.IsNullOrEmpty(key))
                    list = list.Where(x => x.Email.Contains(key) || x.UserName.Contains(key));
                totalcount = list.Count();
                var result = list.OrderByDescending(x => x.CreateTime)
                                  .Skip(offset)
                                  .Take(limit)
                                  .ToList();
                result.ForEach(x =>
                {
                    users.Add(new UserDto()
                    {
                        Id = x.Id,
                        UserName = x.UserName,
                        Email = x.Email,
                        Password = x.Email,
                        QQ = x.QQ,
                        Address = x.Address,
                        Status = x.Status,
                        Balance = x.Balance,
                        CreateTime = x.CreateTime,
                        IsManager = x.IsManager,
                        AffCode = x.AffCode,
                        ParentId = x.ParentId.HasValue ? x.ParentId.Value : 0
                    });
                });
            }
            return users;
        }

        /// <summary>
        /// 更新用户状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool UpdateStatus(int id, int status)
        {
            using (var DB = new SSMMEntities())
            {
                var user = DB.User.Find(id);
                if (user == null)
                    return false;
                if (user.Status == status)
                    return false;
                user.Status = (sbyte)status;
                var result = DB.SaveChanges() > 0;
                if (result)
                {
                    UserCache.Cache.UpdateCacheValue(user.Email);
                    return true;
                }
                else
                    return false;
            }

        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static UserDto Query(string email)
        {
            using (var DB = new SSMMEntities())
            {
                var user = DB.User.Single(x => x.Email == email);
                return new UserDto()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Password = user.Email,
                    QQ = user.QQ,
                    Address = user.Address,
                    Status = user.Status,
                    Balance = user.Balance,
                    CreateTime = user.CreateTime,
                    IsManager = user.IsManager,
                    AffCode = user.AffCode,
                    ParentId = user.ParentId.HasValue ? user.ParentId.Value : 0
                };
            }
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        public static bool RestPassword(string email, string oldpwd, string newpwd, out string info)
        {
            info = "";
            using (var DB = new SSMMEntities())
            {
                var user = DB.User.SingleOrDefault(x => x.Email == email);
                if (user == null)
                {
                    info = "该用户不存在！";
                    return false;
                }
                oldpwd = FormatHelper.GetMD5ByString(oldpwd);
                if (oldpwd != user.Password)
                {
                    info = "原始密码不正确！";
                    return false;
                }
                user.Password = FormatHelper.GetMD5ByString(newpwd);
                if (DB.SaveChanges() > 0)
                {
                    info = "修改密码成功！";
                    UserCache.Cache.UpdateCacheValue(email);
                    return true;
                }
                else
                {
                    info = "修改密码失败！";
                    return false;
                }
            }
        }

        /// <summary>
        /// 总用户数
        /// </summary>
        /// <returns></returns>
        public static int TotalCount()
        {
            using (var DB = new SSMMEntities())
            {
                return DB.User.Count();
            }
        }


        /// <summary>
        /// 获取邀请客户列表
        /// </summary>
        public static List<UserDto> GetCustomers(int userid)
        {
            var users = new List<UserDto>();
            using (var DB = new SSMMEntities())
            {
                var list = DB.User.Where(x => x.ParentId == userid).OrderByDescending(x => x.CreateTime).ToList();
                list.ForEach(x =>
                {
                    users.Add(new UserDto()
                    {
                        Id = x.Id,
                        UserName = x.UserName,
                        Email = x.Email,
                        Password = x.Email,
                        QQ = x.QQ,
                        Address = x.Address,
                        Status = x.Status,
                        Balance = x.Balance,
                        CreateTime = x.CreateTime,
                        IsManager = x.IsManager,
                        AffCode = x.AffCode,
                        ParentId = x.ParentId.HasValue ? x.ParentId.Value : 0
                    });
                });
            }
            return users;
        }
    }
}
