using DoctorOffline.Entity;
using DoctorOffline.Models;
using DoctorOffline.Service;
using Microsoft.AspNetCore.Mvc;
using SchoolOffline.Entity;
using SchoolOffline.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorOffline.Controllers
{
    public class SchoolController : Controller
    {
        public IActionResult Index(long muluId)
        {
            if (muluId <= 0)
            {
                muluId = 1;
            }
            StringBuilder sbHtml = new StringBuilder();
            List<SchoolMulu> muluList = new SchoolMuluService().GetAll();
            List<Mulu> mulus = new MuluService().GetAll();
            List<String> types = new List<string>();
            foreach(SchoolMulu mulu in muluList)
            {
                if (!types.Contains(mulu.Type1))
                {
                    types.Add(mulu.Type1);
                }
            }
            foreach(String type in types)
            {
                sbHtml.AppendFormat("<li><span>{0}</span>", type);
                if(muluList.Where(x => x.Type1 == type).Count() > 0)
                {
                    sbHtml.Append("<ul>");
                    foreach (var mulu in muluList.Where(x => x.Type1 == type))
                    {
                        sbHtml.AppendFormat("<li><span id='{1}' class='mulu {2}' onclick='GetContent({1})'>{0}</span></li>", mulu.Name,mulu.Id,mulu.IfPassed==1?"green":"");
                    }
                    sbHtml.Append("</ul>");
                }
                sbHtml.AppendFormat("</li>");
            }
            List<String> typeList = new List<string>();
            foreach (Mulu m in mulus)
            {
                if (!typeList.Contains(m.TypeName))
                {
                    typeList.Add(m.TypeName);
                }
            }
            StringBuilder sbType = new StringBuilder();
            foreach(var typeName in typeList)
            {
                sbType.AppendFormat("<li><span>{0}</span>", typeName);
                sbType.Append("<ul>");
                foreach(var mulu in mulus.Where(x => x.TypeName == typeName))
                {
                    sbType.AppendFormat("<li><span id='type_{1}' class='mulutype' onclick='selectType({1})'>{0}</span>", mulu.MuluName,mulu.Id);
                }
                sbType.Append("</ul>");
            }

            SchoolContent content = new SchoolContentService().GetByMuluId(muluId);
            if (String.IsNullOrEmpty(content.Titles))
            {
                content.Titles = "";
            }
            ViewData["titles"] = content.Titles.Split('|');
            ViewData["content"] = content;
            ViewData["html"] = sbHtml.ToString();
            ViewData["typeHtml"] = sbType.ToString();
            ViewData["muluId"] = muluId;
            return View();
        }
        public JsonResult GetContent(long muluId)
        {
            SchoolContentModel model = new SchoolContentModel();
            SchoolContent content = new SchoolContentService().GetByMuluId(muluId);
            model.outerContent = content.OutContent;
            if (content != null)
            {
                if (!string.IsNullOrEmpty(content.Content))
                {
                    model.content = content.Content;
                }else
                {
                    model.content = content.OutContent;
                }
            }
            if(model.content==null)
            {
                model.content = "";
            }
            model.title = content.Titles;
            StringBuilder sbLis = new StringBuilder();
            String[] titles = content.Titles.Split('|');
            foreach(var title in titles)
            {
                sbLis.AppendFormat("<li><a onclick=\"a('{0}')\">{0}</a><a onclick=\"b('{0}')\">bing</a>|<a onclick=\"c('{0}')\">google</a></li>", title);
            }
            model.hs = sbLis.ToString();
            return Json(model);
        }
        public JsonResult SaveContent(long muluId,string content,string title)
        {
            try
            {
                SchoolContent schoolContent = new SchoolContentService().GetByMuluId(muluId);
                schoolContent.Titles = title;
                schoolContent.Content = content;
                new SchoolContentService().Save(schoolContent);
            }catch(Exception e)
            {
                return Json("fail");
            }
            return Json("success");
        }
        public JsonResult Pass(long muluId,long typeId)
        {
            try
            {
                SchoolContent content = new SchoolContentService().GetByMuluId(muluId);
                Mulu m = new MuluService().GetByMuluId(typeId);
                Course course = new Course(m.TypeName, m.MuluName, content.Titles, content.Content, 0);
                new CourseService().Add(course);
                SchoolMulu mulu = new SchoolMuluService().GetByMuluId(muluId);
                mulu.IfPassed = 1;
                new SchoolMuluService().Update(mulu);
            }
            catch (Exception e)
            {
                return Json("fail");
            }
            return Json("success");
        }
    }
}
