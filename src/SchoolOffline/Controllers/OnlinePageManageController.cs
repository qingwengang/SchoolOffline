using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchoolOffline.Entity;
using SchoolOffline.Service;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolOffline.Controllers
{
    public class OnlinePageManageController : Controller
    {
        private CourseService courseService = new CourseService();
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public string InitPageHref(string type)
        {
            if (type == "ALL")
            {
                InitPageHrefAll();
            }else
            {
                try
                {
                    List<Course> courseList = new CourseService().GetCourseByTypeName(type);
                    for (int i = 0; i < courseList.Count; i++)
                    {
                        bool flag = false;
                        if (i != 0)
                        {
                            if (courseList[i].LastPage != courseList[i - 1].Title + "~" + courseList[i - 1].Id)
                            {
                                courseList[i].LastPage = courseList[i - 1].Title + "~" + courseList[i - 1].Id;
                                flag = true;
                            }
                        }
                        if (i != courseList.Count - 1)
                        {
                            if (courseList[i].NextPage != courseList[i + 1].Title + "~" + courseList[i + 1].Id)
                            {
                                courseList[i].NextPage = courseList[i + 1].Title + "~" + courseList[i + 1].Id;
                                flag = true;
                            }
                        }
                        if (flag)
                        {
                            courseService.UpdatePageHref(courseList[i]);
                        }
                    }
                }
                catch (Exception e)
                {
                    return "faile";
                }
            }
            return "success";
        }
        public string InitPageHrefAll()
        {
            List<string> types = new MenuService().GetDistinct("select DISTINCT typename as col  from menu");
            foreach (var item in types)
            {
                InitPageHref(item);
            }
            return "success";
        }
    }
}
