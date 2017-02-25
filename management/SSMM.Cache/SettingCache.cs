using SSMM.Entity;
using SSMM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMM.Cache
{
    public class SettingCache : BaseCache<string, SettingDto>
    {
        private static SettingCache _cache;
        public static SettingCache Cache
        {
            get
            {
                if (_cache == null)
                {
                    lock (synObj)
                    {
                        if (_cache == null)
                        {
                            _cache = new SettingCache();
                        }
                    }
                }
                return _cache;
            }
        }

        public override SettingDto GetItem(string key)
        {
            using (var DB = new SSMMEntities())
            {
                var setting = DB.Setting.SingleOrDefault(x => x.Key == key);
                if (setting != null)
                {
                    var dto = new SettingDto()
                    {
                        Key = setting.Key,
                        Value = setting.Value
                    };
                    return dto;
                }
            }
            return null;
        }

        public override Dictionary<string, SettingDto> Init()
        {
            var dic = new Dictionary<string, SettingDto>();
            using (var DB = new SSMMEntities())
            {
                dic = DB.Setting.Where(x => true).ToDictionary(key => key.Key, value => new SettingDto()
                {
                    Key = value.Key,
                    Value = value.Value
                });
            }
            return dic;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        public string Get(SettingFlag flag)
        {
            var key = flag.ToString();
            var dto = GetValue(key);
            if (dto != null)
                return dto.Value;
            else
                return "";
        }

    }
}
