using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchoolOffline.Service;
using SchoolOffline.Entity;
using DoctorOffline.Service;
using System.Text;
using System.Net.Http;
using System.Net;
using System.IO;

namespace SchoolOffline.Controllers
{
    public class HomeController : Controller
    {
        private MenuService menuService = new MenuService();
        public IActionResult Index(string type,long id)
        {
            Course course = new CourseService().GetById(id);
            ViewData["content"] = course.Content;
            Menu menu = new MenuService().GetMenuByTypeName(course.TypeName);
            ViewData["menuHtml"] = menu.Content;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
        public async Task<string> DisplayValue()
        {
            HttpClient cl = new HttpClient();
            return await cl.GetStringAsync("http://www.baidu.com");

        }

        public string Contact()
        {
            var httpClient = new HttpClient(); 
            var task = httpClient.GetAsync(new Uri("http://localhost:42742/HTML/15.html"));

            task.Result.EnsureSuccessStatusCode();
            HttpResponseMessage response = task.Result;
            var result = response.Content.ReadAsStringAsync();
            string responseBodyAsText = result.Result;
            return responseBodyAsText;
        }
        public string InitStaticPageAll()
        {
            List<string> types = new MenuService().GetDistinct("select DISTINCT typename as col  from menu");
            foreach(var item in types)
            {
                InitStaticPageByTypeName(item);
            }
            return "success";
        }

        public string InitStaticPageByTypeName(string type)
        {
            List<Course> courseList = new CourseService().GetCourseByTypeName(type);
            foreach(var item in courseList)
            {
                var httpClient = new HttpClient();
                var task = httpClient.GetAsync(new Uri(String.Format("http://localhost:42742/{0}/{1}.html",type,item.Id)));

                task.Result.EnsureSuccessStatusCode();
                HttpResponseMessage response = task.Result;
                var result = response.Content.ReadAsStringAsync();
                string responseBodyAsText = result.Result;
                Write(type,item.Id,responseBodyAsText);
            }
            return "success";
        }
        public void Write(string type,long id,string content)
        {
            String dicPath = String.Format("E:\\StaticFiles\\{0}", type);
            if (!Directory.Exists(dicPath))
            {
                Directory.CreateDirectory(dicPath);
            }
            FileStream fs = new FileStream(String.Format("E:\\StaticFiles\\{0}\\{1}.html",type,id), FileMode.Append);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.Write(content);
            sw.Dispose();
            fs.Dispose();
        }
        public JsonResult InitMenu(string type)
        {
            List<String> types = new BaseSchoolService().GetDistinct("select DISTINCT typename as col from mulu ");
            List<Course> courseList = new CourseService().GetCourseByTypeName(type);
            StringBuilder sbHtml = new StringBuilder();
            foreach(Course course in courseList)
            {

                sbHtml.Append(String.Format("<a target=\"_top\" title=\"{0}\" href=\"{1}.html\">{0}</a>",course.Title,course.Id));
            }
            string menuContent = sbHtml.ToString();
            Menu menu = new MenuService().GetMenuByTypeName(type);
            if (menu == null)
            {
                menu = new Menu();
                menu.TypeName = type;
                menu.Content = menuContent;
                menuService.Add(menu);
            }else
            {
                if(menuContent != menu.Content)
                {
                    menu.Content = menuContent;
                    menuService.Update(menu);
                }
            }
            return Json("success");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
