using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SSMM.Services.Core
{
    public class BusinessManager
    {
        public static void Init()
        {
            try
            {
                //UseAutoMapper.Init();

                //BackgroundThread thread = new BackgroundThread();
                //thread.Start();

                //Session过期时间
                HttpContext.Current.Session.Timeout = 60;
            }
            catch (Exception ex)
            {

                LogService.Error("BusinessManager.Init()初始化错误！", ex);
            }

        }
    }
}
