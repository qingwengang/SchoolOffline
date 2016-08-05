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
using SchoolOffline.Configs;

namespace SchoolOffline.Controllers
{
    public class HomeController : Controller
    {
        private MenuService menuService = new MenuService();
        private CourseService courseService = new CourseService();
        private TiyContentService tiycontentService = new TiyContentService();
        /// <summary>
        /// 课程详细页面
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Index(string type,long id)
        {
            Course course = new CourseService().GetById(id);
            ViewData["content"] = course.Content;
            Menu menu = new MenuService().GetMenuByTypeName(course.TypeName);
            ViewData["menuHtml"] = menu.Content;
            Menu menutuijian = new MenuService().GetMenuByTypeName("tuijian");
            ViewData["tuijianmenuHtml"] = menutuijian.Content;
            ViewBag.aa = course.Title;
            StringBuilder sbDesc = new StringBuilder();
            sbDesc.Append(course.Title).Append(",").Append(course.MuluName).Append(",").Append(course.TypeName).Append(",").Append("霹雳猿教程");
            StringBuilder sbCanonical = new StringBuilder();
            sbCanonical.AppendFormat("{0}/{1}/{2}.html", OnlineConfig.HomeUrl, course.TypeName, course.Id);
            ViewBag.bb = sbDesc.ToString();
            ViewBag.canonical = sbCanonical.ToString();
            ViewData["pageId"] = id;
            return View();
        }
        /// <summary>
        /// 在线测试页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult DIY(long id)
        {
            ViewData["tiyContent"] = tiycontentService.GetById(id);
            return View();
        }
        
        public IActionResult Error()
        {
            return Index("HTML", 1);
        }
    }
}
