using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchoolOffline.Service;
using SchoolOffline.Entity;
using System.Text;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolOffline.Controllers
{
    public class QuestionAdminController : Controller
    {
        private QuestionService questionService = new QuestionService();
        private QuestionContentService questionContentServie = new QuestionContentService();
        // GET: /<controller>/
        public IActionResult Index()
        {
            StringBuilder sbHtml = new StringBuilder();
            List<Question> questionList = questionService.QueryBySql("select type,title from question order  by type ,CreateTime desc");
            List<string> typeList = new List<string>();
            foreach(var item in questionList)
            {
                if (!typeList.Contains(item.Type))
                {
                    typeList.Add(item.Type);
                }
            }
            foreach(var type in typeList)
            {
                sbHtml.AppendFormat("<li><span>{0}</span>", type);
                if (questionList.Where(x => x.Type == type).Count() > 0)
                {
                    sbHtml.Append("<ul>");
                    foreach (var question in questionList.Where(x => x.Type == type))
                    {
                        sbHtml.AppendFormat("<li><span id='{1}' class='mulu' onclick='GetContent({1})'>{0}</span></li>", question.Title,question.Id);
                    }
                    sbHtml.Append("</ul>");
                }
                sbHtml.AppendFormat("</li>");
            }
            ViewData["html"] = sbHtml.ToString();
            return View();
        }

        public JsonResult CreateQuestion(string type,string title,string desc,string content)
        {
            Question question = new Question { Type = type, Title = title, Des = desc, CreateTime = DateTime.Now };
            long rootId=questionService.Add(question);
            if (rootId > 0)
            {
                List<string> contentList = content.Split(new string[] { "<hr>","<hr/>" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                for(int i = 1; i <= contentList.Count; i++)
                {
                    QuestionContent questionContent = new QuestionContent { RootId = rootId, Content = contentList[i - 1], PageCount = contentList.Count, PageId = i };
                    questionContentServie.Add(questionContent);
                }        
            }
            return Json("success");
        }
    }
}
