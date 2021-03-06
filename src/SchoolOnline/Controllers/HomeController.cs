﻿using System;
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
using SchoolOffline.Util;
using SchoolOffline.Models;
using Dao.Service;

namespace SchoolOffline.Controllers
{
    public class HomeController : Controller
    {
        private MenuService menuService = new MenuService();
        private CourseService courseService = new CourseService();
        private TiyContentService tiycontentService = new TiyContentService();
        private QuestionService questionService = new QuestionService();
        private QuestionContentService questionContentService = new QuestionContentService();
        private QuestionTypeRelationService questionTypeRelationService = new QuestionTypeRelationService();
        private MuluExtendService extendService = new MuluExtendService();
        /// <summary>
        /// 课程详细页面
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Index(string type,long id)
        {
            CourseDetailModel model = new CourseDetailModel();
            if (String.IsNullOrEmpty(type))
            {
                type = "HTML";
            }
            if (id <= 0)
            {
                id = 1;
            }
            Course course = new CourseService().GetById(id);
            //ViewData["content"] = course.Content;
            model.content = course.DraftId>0?"<h1>"+course.Title+"</h1>"+course.Content: course.Content;
            Menu menu = new MenuService().GetMenuByTypeName(course.TypeName);
            //ViewData["menuHtml"] = menu!=null?menu.Content:"";
            model.menuHtml= menu != null ? menu.Content : "";
            Menu menutuijian = new MenuService().GetMenuByTypeName("tuijian");
            //ViewData["tuijianmenuHtml"] = menutuijian.Content;
            model.tuijianmenuHtml= menutuijian.Content;
            //ViewData["lastPageHref"] = OnlinePageHelper.GeneratPageHref(type, course.LastPage);
            //ViewData["nextPageHref"] = OnlinePageHelper.GeneratPageHref(type, course.NextPage);
            model.lastPageHref= OnlinePageHelper.GeneratPageHref(type, course.LastPage);
            model.nextPageHref= OnlinePageHelper.GeneratPageHref(type, course.NextPage);
            //ViewBag.aa = course.Title;
            model.title = course.Title;
            StringBuilder sbDesc = new StringBuilder();
            sbDesc.Append(course.Title).Append(",").Append(course.MuluName).Append(",").Append(course.TypeName).Append(",").Append("霹雳猿教程");
            StringBuilder sbCanonical = new StringBuilder();
            sbCanonical.AppendFormat("{0}/{1}/{2}.html", OnlineConfig.HomeUrl, course.TypeName, course.Id);
            //ViewBag.bb = sbDesc.ToString();
            //ViewBag.canonical = sbCanonical.ToString();
            model.desc = sbDesc.ToString();
            model.canonical = sbCanonical.ToString();
            var ext= extendService.Get(type, "shuji");
            model.tuijian = ext!=null ? ext.Content : "";
            model.comment = course.Comment;
            //ViewData["pageId"] = id;
            model.pageId = id;
            return View(model);
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
        
        public IActionResult QuestionList(string type,int page)
        {
            QuestionPageModel pageDo = questionService.GetQuestionPage(type, page);
            pageDo.questionTypeDesc = questionTypeRelationService.GetTypeDesc(type);
            ViewData["url"] = GetPageUrl(type, pageDo.pageCount, page);
            pageDo.canonical = string.Format("{0}/QuestionList/{1}/{2}.html", OnlineConfig.HomeUrl, type, page);
            Menu menutuijian = new MenuService().GetMenuByTypeName("tuijian");
            Menu menuquestion = new MenuService().GetMenuByTypeName("questionmenu");
            Menu menuquestioncourse = new MenuService().GetMenuByTypeName("questioncoursemenu");
            ViewData["tuijianmenuHtml"] = menutuijian.Content;
            ViewData["questionmenu"] = menuquestion.Content;
            ViewData["menuquestioncourse"] = menuquestioncourse.Content;
            return View(pageDo);
        }

        public IActionResult Question(long rootId,int pageId)
        {
            var questioncontent = questionContentService.GetContent(rootId, pageId);
            QuestionModel model = new QuestionModel { questionContent = questioncontent };
            ViewData["url"] = GetQuestionPageUrl(rootId, questioncontent.PageCount, pageId);
            var questionList = questionService.QueryBySql(string.Format("select * from question where id={0}", rootId));
            if(questionList!=null && questionList.Count > 0)
            {
                var question = questionList.FirstOrDefault();
                model.title = question.Title;
                model.type = question.Type;
            }
            model.canonical = string.Format("{0}/Question/{1}/{2}.html", OnlineConfig.HomeUrl,rootId,pageId);
            model.questionTypeDesc = questionTypeRelationService.GetTypeDesc(model.type);
            Menu menutuijian = new MenuService().GetMenuByTypeName("tuijian");
            Menu menuquestion = new MenuService().GetMenuByTypeName("questionmenu");
            Menu menuquestioncourse = new MenuService().GetMenuByTypeName("questioncoursemenu");
            ViewData["tuijianmenuHtml"] = menutuijian.Content;
            ViewData["questionmenu"] = menuquestion.Content;
            ViewData["menuquestioncourse"] = menuquestioncourse.Content;
            return View(model);
        }

        public IActionResult Detail(string type,long id)
        {
            CourseDetailModel model = new CourseDetailModel();
            if (String.IsNullOrEmpty(type))
            {
                type = "HTML";
            }
            if (id <= 0)
            {
                id = 1;
            }
            Course course = new CourseService().GetById(id);
            model.content = course.DraftId > 0 ? "<h1>" + course.Title + "</h1>" + course.Content : course.Content;
            Menu menu = new MenuService().GetMenuByTypeName(course.TypeName);
            model.menuHtml = menu != null ? menu.Content : "";
            Menu menutuijian = new MenuService().GetMenuByTypeName("tuijian");
            model.tuijianmenuHtml = menutuijian.Content;
            model.lastPageHref = OnlinePageHelper.GeneratPageHref(type, course.LastPage,"上一章节：");
            model.nextPageHref = OnlinePageHelper.GeneratPageHref(type, course.NextPage,"下一章节：");
            model.lastPageHref1 = model.lastPageHref.Replace("ctl00_lnkPrev1", "ctl00_lnkPrev2");
            model.nextPageHref1 = model.nextPageHref.Replace("ctl00_lnkNext1", "ctl00_lnkNext2");
            model.title = course.Title;
            StringBuilder sbDesc = new StringBuilder();
            sbDesc.Append(course.Title).Append(",").Append(course.MuluName).Append(",").Append(course.TypeName).Append(",").Append("霹雳猿教程");
            StringBuilder sbCanonical = new StringBuilder();
            sbCanonical.AppendFormat("{0}/{1}/{2}.html", OnlineConfig.HomeUrl, course.TypeName, course.Id);
            model.desc = sbDesc.ToString();
            model.canonical = sbCanonical.ToString();
            model.currentMenu = "menu"+type.ToLower();
            var ext = extendService.Get(type, "shuji");
            model.tuijian = ext != null ? ext.Content : "";
            model.pageId = id;
            model.comment = course.Comment;
            return View(model);
        }
        
        public IActionResult Error()
        {
            return Index("HTML", 1);
        }
        #region 生成题库列表分页标签
        private string GetPageUrl(string type, int pageCount, int currentPage)
        {
            StringBuilder sbUrl = new StringBuilder();
            if (currentPage > 1)
            {
                sbUrl.AppendFormat("<li class=\"next - page\"><a href=\"/QuestionList/{0}/{1}.html\" title =\"第 {1} 页\">上一页</a></li>", type, currentPage - 1);
            }
            if (pageCount <= 15)
            {
                for (int i = 1; i <= pageCount; i++)
                {
                    sbUrl.Append(GenereatePageUrl(type, i, currentPage));
                }
            }
            else
            {
                if (currentPage <= 5)
                {
                    for (int i = 1; i < currentPage + 3; i++)
                    {
                        sbUrl.Append(GenereatePageUrl(type, i, currentPage));
                    }
                    for (int i = pageCount - 4; i <= pageCount; i++)
                    {
                        sbUrl.Append(GenereatePageUrl(type, i, currentPage));
                    }
                }
                else if (currentPage >= pageCount - 5)
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        sbUrl.Append(GenereatePageUrl(type, i, currentPage));
                    }
                    for (int i = currentPage - 4; i <= pageCount; i++)
                    {
                        sbUrl.Append(GenereatePageUrl(type, i, currentPage));
                    }
                }
                else
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        sbUrl.Append(GenereatePageUrl(type, i, currentPage));
                    }
                    for (int i = currentPage - 3; i < currentPage + 3; i++)
                    {
                        sbUrl.Append(GenereatePageUrl(type, i, currentPage));
                    }
                    for (int i = pageCount - 4; i <= pageCount; i++)
                    {
                        sbUrl.Append(GenereatePageUrl(type, i, currentPage));
                    }
                }
            }
            if (currentPage < pageCount)
            {
                sbUrl.AppendFormat("<li class=\"next - page\"><a href=\"/QuestionList/{0}/{1}.html\" title =\"第 {1} 页\"></li> 下一页</a></li>", type, currentPage + 1);
            }
            sbUrl.AppendFormat("<li><span>共 {0} 页</span></li>", pageCount);
            return sbUrl.ToString();
        }

        private string GenereatePageUrl(string type, int pageNo, int currentPage)
        {
            if (pageNo != currentPage)
            {
                return string.Format("<li><a href=\"/QuestionList/{0}/{1}.html\" title =\"第 {1} 页\">{1}</a></li>", type, pageNo);
            }
            else
            {
                return string.Format("<li class=\"active\"><span>{0}</span></li>", pageNo);
            }
        }
        #endregion
        #region 题库详细页分页
        private string GetQuestionPageUrl(long rootId, int pageCount, int currentPage)
        {
            StringBuilder sbUrl = new StringBuilder();
            if (currentPage > 1)
            {
                sbUrl.AppendFormat("<li class=\"next - page\"><a href=\"/Question/{0}/{1}.html\" title =\"第 {1} 页\">上一页</a></li>", rootId, currentPage - 1);
            }
            for (int i = 1; i <= pageCount; i++)
            {
                sbUrl.Append(GenereateQuestionPageUrl(rootId, i, currentPage));
            }
            if (currentPage < pageCount)
            {
                sbUrl.AppendFormat("<li class=\"next - page\"><a href=\"/Question/{0}/{1}.html\" title =\"第 {1} 页\">下一页</a></li>", rootId, currentPage + 1);
            }
            return sbUrl.ToString();
        }
        private string GenereateQuestionPageUrl(long rootId, int pageNo, int currentPage)
        {
            if (pageNo != currentPage)
            {
                return string.Format("<li><a href=\"/Question/{0}/{1}.html\" title =\"第 {1} 页\">{1}</a></li>", rootId, pageNo);
            }
            else
            {
                return string.Format("<li class=\"active\"><span>{0}</span></li>", pageNo);
            }
        }
        #endregion

    }
}
