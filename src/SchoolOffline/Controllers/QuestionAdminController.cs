using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchoolOffline.Service;
using SchoolOffline.Entity;
using System.Text;
using SchoolOffline.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolOffline.Controllers
{
    public class QuestionAdminController : Controller
    {
        private QuestionService questionService = new QuestionService();
        private QuestionContentService questionContentServie = new QuestionContentService();
        private QuestionTypeRelationService relationService = new QuestionTypeRelationService();
        // GET: /<controller>/
        public IActionResult Index()
        {
            QuestionAdminModel model = new QuestionAdminModel();
            StringBuilder sbHtml = new StringBuilder();
            List<Question> questionList = questionService.QueryBySql("select type,title,id from question order  by type ,CreateTime desc");
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
                sbHtml.AppendFormat("<li><span>{0}</span>&nbsp;&nbsp;<a target='_blank' href='/QuestionList/{0}/1.html'>Online</a>", type);
                if (questionList.Where(x => x.Type == type).Count() > 0)
                {
                    sbHtml.Append("<ul>");
                    foreach (var question in questionList.Where(x => x.Type == type))
                    {
                        sbHtml.AppendFormat("<li><span id='{1}' class='mulu' onclick='GetContent({1})'>{0}</span><a target='_blank' href='/Question/{1}/1.html'>online</a></li>", question.Title,question.Id);
                    }
                    sbHtml.Append("</ul>");
                }
                sbHtml.AppendFormat("</li>");
            }
            ViewData["html"] = sbHtml.ToString();
            model.html = sbHtml.ToString();
            model.relationList = relationService.GetAll();
            return View(model);
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
        public JsonResult GetQuestionDetail(long rootId)
        {
            OffQuestionModel model = new OffQuestionModel();
            StringBuilder sbcontent = new StringBuilder();
            var question = questionService.QueryBySql(string.Format("select * from question where id={0}", rootId)).FirstOrDefault();
            var contentList = questionContentServie.QueryBySql(string.Format("select * from questioncontent where rootid={0} order by pageid",rootId));
            contentList.ForEach(x => sbcontent.Append(x.Content).Append("<hr>"));
            model.question = question;
            model.content = sbcontent.ToString();
            return Json(model);
        }
    }
}
