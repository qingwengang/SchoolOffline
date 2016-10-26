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
        //private static string staticPathNew = "E:\\StaticFilesNew";
        public static void Write(string type, long id, string content,string rootFolderName="")
        {
            String dicPath = String.Format("{0}\\{2}{1}", staticPath, type,string.IsNullOrEmpty(rootFolderName)?"":rootFolderName+"\\");
            if (!Directory.Exists(dicPath))
            {
                Directory.CreateDirectory(dicPath);
            }
            FileStream fs = new FileStream(String.Format("{0}\\{1}.html", dicPath, id), FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.Write(content);
            sw.Dispose();
            fs.Dispose();
        }
        public static void Write(string pageName, string content)
        {
            String dicPath = String.Format(staticPath);
            if (!Directory.Exists(dicPath))
            {
                Directory.CreateDirectory(dicPath);
            }
            FileStream fs = new FileStream(String.Format("{0}\\{1}.html", staticPath, pageName), FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.Write(content);
            sw.Dispose();
            fs.Dispose();
        }
        public static void WriteWithHouzui(string pageName, string content)
        {
            String dicPath = String.Format(staticPath);
            if (!Directory.Exists(dicPath))
            {
                Directory.CreateDirectory(dicPath);
            }
            FileStream fs = new FileStream(String.Format("{0}\\{1}", staticPath, pageName), FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.Write(content);
            sw.Dispose();
            fs.Dispose();
        }
        //public static void RenameStaticFolder()
        //{
        //    String dicPath = String.Format(staticPath);
        //    if (Directory.Exists(dicPath))
        //    {
        //        Directory.Move(staticPath, staticPath+ DateTime.Now.ToString("yyyyMMddHHMMss"));
        //        Directory.Move(staticPathNew, staticPath);
        //        //RenameDirectories(staticPath, staticPath + DateTime.Now.ToString("yyyyMMddHHMMss"));
        //        //RenameDirectories(staticPath+"New", staticPath);
        //    }
        //}
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
        public static void WriteLog(string logPath, string content)
        {
            FileStream fs = new FileStream(logPath, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("UTF-8"));
            sw.Write(content);
            sw.Dispose();
            fs.Dispose();
        }
        public static string Read(string path)
        {
            string result="";
            string[] arr=File.ReadAllLines(path, Encoding.GetEncoding("UTF-8"));
            foreach(string item in arr)
            {
                result += item + "<br>";
            }
            //FileStream fs = new FileStream(path, FileMode.Open);
            //using (StreamReader streamReader = new StreamReader(fs, Encoding.GetEncoding("UTF-8")))
            //{
            //    string text = streamReader.ReadToEnd();
            //    streamReader.Dispose();
            //    result = text;
            //}
            return result;
        }
    }
}
