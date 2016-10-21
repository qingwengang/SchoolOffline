using Dao.Entity;
using Dao.Service;
using Microsoft.AspNetCore.Mvc;
using SchoolOffline.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolOffline.Controllers
{
    public class TuijianController : Controller
    {
        private MuluService muluService = new MuluService();
        private MuluExtendService muluExtendService = new MuluExtendService();
        public IActionResult Index()
        {
            ViewData["typeList"] = muluService.GetDistinctTypeName();
            return View();
        }
        public string Create(string type,string content)
        {
            MuluExtend ext = new MuluExtend
            {
                 TypeName=type,Content=content,RelationType="shuji"
            };
            muluExtendService.UpdateOrAdd(ext);
            return "success";
        }
    }
}
