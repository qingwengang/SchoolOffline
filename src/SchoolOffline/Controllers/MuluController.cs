using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchoolOffline.Service;
using SchoolOffline.Entity;
using Dao.Service;
using Dao.Entity;
using System.Text;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolOffline.Controllers
{
    public class MuluController : Controller
    {
        private MuluService muluService = new MuluService();
        private CourseDraftService draftService = new CourseDraftService();
        private CourseTitleService titleService = new CourseTitleService();
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult EditTitleContent(long titleId)
        {
            ViewData["titleId"] = titleId;
            CourseTitle courseTitle = titleService.GetByTitleId(titleId);
            ViewData["content"] = courseTitle.Content;
            return View();
        }
        public IActionResult EditDraft(long draftId)
        {
            var titleList=titleService.GetByDraftId(draftId);
            var draft = draftService.GetById(draftId);
            StringBuilder sbcontent = new StringBuilder();
            foreach(var title in titleList)
            {
                if (!string.IsNullOrEmpty(title.Content.Trim()))
                {
                    sbcontent.AppendFormat("<h2>{0}</h2>{1}<hr>", title.TitleName, title.Content);
                }
            }
            ViewData["content"] = sbcontent.ToString();
            ViewData["title"] = draft.Title;
            ViewData["draftid"] = draftId;
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
        public JsonResult GetMulu(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                List<Mulu> muluList = muluService.GetAll();
                return new JsonResult(muluList);
            }
            else
            {
                List<Mulu> muluList = muluService.GetByTypeName(type);
                return new JsonResult(muluList);
            }
        }
        public JsonResult GetCourseDraftByMuluName(string muluName)
        {
            var result = draftService.GetByMuluName(muluName);
            return new JsonResult(result);
        }
        public string CreateCourseDraft(string typeName,string muluName,string title)
        {
            CourseDraft draft = new CourseDraft { TypeName = typeName, MuluName = muluName, Title = title };
            draftService.Add(draft);
            return "success";
        }
        public string UpdateCourseDraft(long draftId,string title,int sortNum)
        {
            var draft = draftService.GetById(draftId);
            draft.Title = title;
            draft.SortNum = sortNum;
            draftService.Update(draft);
            return "success";
        }
        public string UpdateCourseTitle(long id,string titleName,int sortNum)
        {
            var courseTitle = titleService.GetByTitleId(id);
            courseTitle.TitleName = titleName;
            courseTitle.SortNum = sortNum;
            titleService.Update(courseTitle);
            return "success";
        }
        public string CreateCourseTitle(long draftId,string title)
        {
            CourseTitle courseTitle = new CourseTitle { DraftId = draftId, TitleName = title };
            titleService.Add(courseTitle);
            return "success";
        }
        public JsonResult GetCourseTitle(long id)
        {
            CourseTitle courseTitle = titleService.GetByTitleId(id);
            return new JsonResult(courseTitle);
        }
        public JsonResult GetCourseTitleList(long draftId)
        {
            var result = titleService.GetByDraftId(draftId);
            return new JsonResult(result);
        }
        public string EditCourseTitleContent(long titleId,string content)
        {
            var courseTitle = titleService.GetByTitleId(titleId);
            courseTitle.Content = content;
            titleService.Update(courseTitle);
            return "success";
        }
    }
}