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
        private CourseService courseService = new CourseService();
        private TiyContentService tiycontentService = new TiyContentService();
        public IActionResult Index(string type,long id)
        {
            if (type == "DIY")
            {
                return RedirectToAction("DIY", new { id = id });
            }
            Course course = new CourseService().GetById(id);
            ViewData["content"] = course.Content;
            Menu menu = new MenuService().GetMenuByTypeName(course.TypeName);
            ViewData["menuHtml"] = menu.Content;
            return View();
        }
        public IActionResult TIY(long id)
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult DIY(long id)
        {
            ViewData["tiyContent"] = tiycontentService.GetById(id);
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

        
        

        public IActionResult Error()
        {
            return View();
        }
    }
}
