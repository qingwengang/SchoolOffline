using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolOffline.Util
{
    public class FileTool
    {
        private static string staticPath= "E:\\StaticFiles";
        private static string staticPathNew = "E:\\StaticFilesNew";
        public static void Write(string type, long id, string content)
        {
            String dicPath = String.Format("{0}\\{1}",staticPathNew, type);
            if (!Directory.Exists(dicPath))
            {
                Directory.CreateDirectory(dicPath);
            }
            FileStream fs = new FileStream(String.Format("{0}\\{1}\\{2}.html", staticPathNew,type, id), FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.Write(content);
            sw.Dispose();
            fs.Dispose();
        }
        public static void Write(string pageName, string content)
        {
            String dicPath = String.Format(staticPathNew);
            if (!Directory.Exists(dicPath))
            {
                Directory.CreateDirectory(dicPath);
            }
            FileStream fs = new FileStream(String.Format("{0}\\{1}.html",staticPathNew, pageName), FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.Write(content);
            sw.Dispose();
            fs.Dispose();
        }
        public static void RenameStaticFolder()
        {
            String dicPath = String.Format(staticPath);
            if (Directory.Exists(dicPath))
            {
                Directory.Move(staticPath, staticPath+ DateTime.Now.ToString("yyyyMMddHHMMss"));
                Directory.Move(staticPathNew, staticPath);
                //RenameDirectories(staticPath, staticPath + DateTime.Now.ToString("yyyyMMddHHMMss"));
                //RenameDirectories(staticPath+"New", staticPath);
            }
        }
        public static void RenameDirectories(string directoryName, string newDirectoryName)
        {
            int i = 1;
            string[] sDirectories = Directory.GetDirectories(directoryName);
            foreach (string sDirectory in sDirectories)
            {
                string sDirectoryName = Path.GetFileName(sDirectory);
                string sNewDirectoryName = string.Format(newDirectoryName, i++);
                string sNewDirectory = Path.Combine(directoryName, sNewDirectoryName);
                Directory.Move(sDirectory, sNewDirectory);
            }
        }
    }
}
