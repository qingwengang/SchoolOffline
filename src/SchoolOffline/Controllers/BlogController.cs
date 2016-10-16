using Microsoft.AspNetCore.Mvc;
using SchoolOffline.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolOffline.Controllers
{
    public class BlogController : Controller
    {
        private CourseService courseService = new CourseService();
        public IActionResult Index()
        {
            return View();
        }
        public string GetPageUrl(string type)
        {
            var list=courseService.GetCourseByTypeName(type);
            StringBuilder sbhtml = new StringBuilder();
            foreach(var item in list)
            {
                sbhtml.AppendFormat("<p><a title=\"霹雳猿教程-{1}教程-{0}\" href=\"http://www.piliyuan.com/{1}/{2}.html\" target=\"_blank\">霹雳猿教程-{1}教程-{0}</a></p>", item.Title, type, item.Id);
            }
            return sbhtml.ToString();
        }
    }
}
