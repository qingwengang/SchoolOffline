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
            return string.Format("<a href='/{0}/{1}.html' style='color:red'>{2}</a>", typeName, id, name);
        }
        public static string GeneratPageHref(string typeName,string nextPage)
        {
            if (!string.IsNullOrEmpty(nextPage))
            {
                string[] nextArr = nextPage.Split('~');
                return GeneratHref(typeName, nextArr[1], nextArr[0]);
            }
            return string.Empty;
        }
    }
}
