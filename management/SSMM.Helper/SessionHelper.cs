using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SSMM.Helper
{
    public class SessionHelper
    {

        public static object GetValue(string keyName)
        {
            return HttpContext.Current.Session[keyName];
        }

        public static void SetValue(string keyName, object val)
        {
            if (val == null || string.IsNullOrEmpty(val.ToString()))
            {
                HttpContext.Current.Session.Remove(keyName);
            }
            else
            {
                HttpContext.Current.Session[keyName] = val;
            }
        }

    }
}
