using Microsoft.AspNetCore.Mvc;
using SchoolOffline.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolOffline.Controllers
{
    public class LogController : Controller
    {
        public IActionResult Index()
        {
            var content= FileTool.Read("D:\\u_ex161025.log");
            content = content.Replace("\\n", "<br>");
            ViewData["data"] = content;
            return View();
        }
    }
}
