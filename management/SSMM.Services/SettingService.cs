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
    public class SettingService
    {

        public static bool Set(SettingFlag flag, string value)
        {
            var key = flag.ToString();
            using (var DB = new SSMMEntities())
            {
                var val = DB.Setting.FirstOrDefault(x => x.Key == key);
                if (val != null)
                {
                    if (val.Value == value) return true;
                    val.Value = value;
                }
                else
                {
                    DB.Setting.Add(new Setting()
                    {
                        Key = key,
                        Value = value
                    });
                }
                if (DB.SaveChanges() > 0)
                {
                    SettingCache.Cache.UpdateCacheValue(key);
                    return true;
                }
                return false;
            }
        }

        public static bool WebSite(string url, string title, string keywords, string des, string code)
        {
            return Set(SettingFlag.WebSiteUrl, url) && Set(SettingFlag.WebSiteTitle, title) && Set(SettingFlag.WebSiteKeywords, keywords) && Set(SettingFlag.WebSiteDescription, des) && Set(SettingFlag.StatisticalCode, code);

        }

        public static bool Basic(string alipayaccount, string alipayremarksearchapi, string portrange, int rebatenum, string websiteinfo)
        {
            return Set(SettingFlag.AlipayAccount, alipayaccount) && Set(SettingFlag.AlipayRemarkSearchApi, alipayremarksearchapi) && Set(SettingFlag.SSPortRange, portrange) && Set(SettingFlag.RebateNum, rebatenum.ToString()) && Set(SettingFlag.WebSiteInfo, websiteinfo);

        }

    }
}
