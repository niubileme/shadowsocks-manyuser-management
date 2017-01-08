using SSMM.Services.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMM.Services
{
    public class LogService
    {

        /// <summary>
        /// 写入调试信息
        /// </summary>
        public static void Debug(string msg, Exception ex=null)
        {
            if (Config.LOG_LEVENL >= 3)
            {
                WriteLog("DEBUG", msg, ex);
            }
        }

        /// <summary>
        /// 写入运行时信息
        /// </summary>
        public static void Info(string msg, Exception ex = null)
        {
            if (Config.LOG_LEVENL >= 2)
            {
                WriteLog("INFO", msg, ex);
            }
        }

        /// <summary>
        /// 写入错误信息
        /// </summary>
        public static void Error(string msg, Exception ex = null)
        {
            if (Config.LOG_LEVENL >= 1)
            {
                WriteLog("ERROR", msg, ex);
            }
        }

        private static void WriteLog(string type, string msg, Exception ex = null)
        {
           
        }
    }
}
