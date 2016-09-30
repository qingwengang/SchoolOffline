using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SchoolOffline.Util
{
    public class HttpTool
    {
        public static string GetHtmlContent(string url)
        {
            try
            {
                var httpClient = new HttpClient();
                var task = httpClient.GetAsync(new Uri(url));
                task.Result.EnsureSuccessStatusCode();
                HttpResponseMessage response = task.Result;
                var result = response.Content.ReadAsStringAsync();
                return result.Result;
            }catch(Exception e)
            {
                LogTool.Error("HttpTool.GetHtmlContent|"+url, e.Message, e);
                throw e;
            }
        }
    }
}
