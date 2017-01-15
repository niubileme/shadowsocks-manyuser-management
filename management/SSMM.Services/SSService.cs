using SSMM.Cache;
using SSMM.Entity;
using SSMM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMM.Services
{
    public class SSService
    {
        public static object obj = new object();
        /// <summary>
        /// 获取ss端口
        /// </summary>
        public static int GetNextPort()
        {
            lock (obj)
            {
                var port = 0;
                var range = SettingCache.Cache.Get(SettingFlag.SSPortRange);
                if (range == null)
                    throw new Exception("请先后台配置ss连接端口范围");
                var min = Convert.ToInt32(range.Split('&')[0]);
                var max = Convert.ToInt32(range.Split('&')[1]);
                using (var DB = new SSMMEntities())
                {
                    var current = DB.SS.OrderByDescending(x => x.port).SingleOrDefault();
                    if (current == null)
                        port = min;
                    else
                        port = current.port + 1;
                }
                if (port > max)
                    throw new Exception("端口已超过最大范围");
                return port;
            }
        }
    }
}
