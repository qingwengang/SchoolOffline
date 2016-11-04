using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolOffline.Util
{
    public class OnlinePageHelper
    {
        public static string GeneratHref(string typeName,string id,string name)
        {
            //return string.Format("<a href='/{0}/{1}.html' style='color:red'>{2}</a>", typeName, id, name);
            return string.Format("<a id=\"ctl00_lnkPrev1\" href='/{0}/{1}.html'>{2}</a>", typeName, id, name);
        }
        public static string GeneratPageHref(string typeName,string nextPage,string before="")
        {
            if (!string.IsNullOrEmpty(nextPage))
            {
                string[] nextArr = nextPage.Split('~');
                return GeneratHref(typeName, nextArr[1], before+nextArr[0]);
            }
            return string.Empty;
        }
    }
}
