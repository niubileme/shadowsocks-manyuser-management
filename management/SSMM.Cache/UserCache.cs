using SSMM.Cache;
using SSMM.Entity;
using SSMM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMM.Cache
{
    public class UserCache : BaseCache<string, UserDto>
    {
        private static UserCache _cache;
        public static UserCache Cache
        {
            get
            {
                if (_cache == null)
                {
                    lock (synObj)
                    {
                        if (_cache == null)
                        {
                            _cache = new UserCache();
                        }
                    }
                }
                return _cache;
            }
        }

        public UserCache()
        { }

        public override UserDto GetItem(string email)
        {
            using (var DB = new SSMMEntities())
            {
                var user = DB.User.SingleOrDefault(x => x.Email == email);
                if (user != null)
                {
                    var dto = new UserDto()
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        Password = user.Password,
                        QQ = user.QQ,
                        Address = user.Address,
                        Status = user.Status,
                        Balance = user.Balance,
                        CreateTime = user.CreateTime,
                        IsManager = user.IsManager,
                        AffCode = user.AffCode,
                        ParentId = user.ParentId.HasValue ? user.ParentId.Value : 0
                    };
                    return dto;
                }
            }
            return null;
        }

        public override Dictionary<string, UserDto> Init()
        {
            var dic = new Dictionary<string, UserDto>();
            using (var DB = new SSMMEntities())
            {
                dic = DB.User.Where(x => true).ToDictionary(key => key.Email, value => new UserDto()
                {
                    Id = value.Id,
                    UserName = value.UserName,
                    Email = value.Email,
                    Password = value.Password,
                    QQ = value.QQ,
                    Address = value.Address,
                    Status = value.Status,
                    Balance = value.Balance,
                    CreateTime = value.CreateTime,
                    IsManager = value.IsManager,
                    AffCode = value.AffCode,
                    ParentId = value.ParentId.HasValue ? value.ParentId.Value : 0
                });
            }
            return dic;
        }


    }
}
