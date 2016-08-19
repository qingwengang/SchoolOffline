using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchoolOffline.Service;
using SchoolOffline.Entity;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolOffline.Controllers
{
    public class MuluController : Controller
    {
        private MuluService muluService = new MuluService();
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetMuluType()
        {
            List<Mulu> muluList = muluService.GetAll();
            List<string> result = new List<string>();
            foreach (var item in muluList)
            {
                if (!result.Contains(item.TypeName))
                {
                    result.Add(item.TypeName);
                }
            }
            return new JsonResult(result);
        }
        public string AddMulu(string typeName,string muluName,int sortNum)
        {
            Mulu mulu = new Mulu
            {
                TypeName = typeName,
                MuluName = muluName,
                SortNum = sortNum
            };
            muluService.AddMulu(mulu);
            return "success";
        }
        public JsonResult GetMulu()
        {
            List<Mulu> muluList = muluService.GetAll();
            return new JsonResult(muluList);
        }
    }
}