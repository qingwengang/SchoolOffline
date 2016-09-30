using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchoolOffline.Configs;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolOffline.Controllers
{
    public class SuperPageController : Controller
    {
        public ApplicationConfiguration config;
        public SuperPageController(IOptions<ApplicationConfiguration> option)
        {
            config = option.Value;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About()
        {
            ViewData["b"] = config.pageUrl;
            return View();
        }
    }
}
