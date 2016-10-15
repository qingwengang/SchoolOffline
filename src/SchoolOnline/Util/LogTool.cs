using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolOffline.Util
{
    public class LogTool
    {
        public static void Error(string type,string message,Exception e)
        {
            string content = String.Format("{0} type:{1},message:{2},exception content:{3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), type, message, e.StackTrace);
            FileTool.WriteLog("E:\\log\\Error\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt", content+ "<br>\r\n");
        }
    }
}
