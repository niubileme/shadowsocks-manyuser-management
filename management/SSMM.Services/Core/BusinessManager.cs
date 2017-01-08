using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


            }
            catch (Exception ex)
            {

                LogService.Error("BusinessManager.Init()初始化错误！", ex);
            }

        }
    }
}
