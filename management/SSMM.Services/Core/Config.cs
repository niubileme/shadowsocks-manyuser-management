using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMM.Services.Core
{
    public class Config
    {
        /// <summary>
        ///【日志级别】
        ///===================================
        /// 0.不输出日志；
        /// 1.只输出错误信息;
        /// 2.输出错误和正常信息;
        /// 3.输出错误信息、正常信息和调试信息
        /// ===================================
        /// </summary>
        public const int LOG_LEVENL = 1;


        public static string GetAppSetting(string key)
        {
            var value = "";
            try
            {
                value = ConfigurationManager.AppSettings[key];
            }
            catch (Exception)
            {
                value = "";
            }
            return value;
        }
    }
}
