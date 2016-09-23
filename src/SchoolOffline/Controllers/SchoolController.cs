﻿using DoctorOffline.Entity;
using DoctorOffline.Models;
using DoctorOffline.Service;
using Microsoft.AspNetCore.Mvc;
using SchoolOffline.Entity;
using SchoolOffline.Models;
using SchoolOffline.Service;
using SchoolOffline.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DoctorOffline.Controllers
{
    public class SchoolController : Controller
    {
        #region 初始化service
        private CourseService courseService = new CourseService();
        private MenuService menuService = new MenuService();
        private TiyContentService tiyService = new TiyContentService();
        private SchoolMuluService schoolMuluService = new SchoolMuluService();
        private TiyContentService tiycontentService = new TiyContentService();
        private SchoolContentService contentService = new SchoolContentService();
        private SchoolMuluService muluService = new SchoolMuluService();
        #endregion
        #region 核心操作部分
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
                        sbHtml.AppendFormat("<li><span id='{1}' class='mulu {2}' onclick='GetContent({1})'>{0}</span></li>", mulu.Name,mulu.Id,mulu.IfPassed==1?"green": mulu.IfPassed == 2?"blue":"");
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
            ViewData["typeList"] = courseService.GetDistinctTypeName();
            return View();
        }
        public JsonResult GetContent(long muluId)
        {
            SchoolContentModel model = new SchoolContentModel();
            SchoolContent content = new SchoolContentService().GetByMuluId(muluId);
            SchoolMulu mulu = muluService.GetByMuluId(muluId);
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
            model.title = mulu.Name;
            StringBuilder sbLis = new StringBuilder();
            String[] titles = content.Titles.Split('|');
            foreach(var title in titles)
            {
                sbLis.AppendFormat("<li><a onclick=\"a('{0}')\">{0}</a><a onclick=\"b('{0}')\">bing</a>|<a onclick=\"s360('{0}')\">360</a>|<a onclick=\"c('{0}')\">google</a></li>", title);
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
        public string Complete(long muluId)
        {
            SchoolMulu schoolMulu = schoolMuluService.GetByMuluId(muluId);
            schoolMulu.IfPassed = 2;
            schoolMuluService.Update(schoolMulu);
            return "success";
        }
        public JsonResult Pass(long muluId,long typeId,string title,string content)
        {
            try
            {
                List<Course> courseList = courseService.QueryBySql("select * from course where outerid=" + muluId);
                if(courseList!=null && courseList.Count > 0)
                {
                    return Json("已经推送过了");
                }
                Mulu m = new MuluService().GetByMuluId(typeId);
                int maxSortNum = courseService.GetMaxSortNumByMuluName(m.MuluName);
                Course course = new Course(m.TypeName, m.MuluName, title, content, maxSortNum+1);
                course.OuterId = muluId;
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
        public JsonResult GetSortModelByMuluName(long muluId)
        {
            List<CourseSortModel> courseList = new CourseService().GetCourseSortModelByMuluName(muluId);
            return Json(courseList);
        }

        public IActionResult CompareShow(long muluId)
        {
            ViewData["muluId"] = muluId;
            return View();
        }
        public string AddMulu(string type1,string name,string content)
        {
            SchoolMulu mulu = new SchoolMulu
            {
                Name = name,
                Type1 = type1
            };
            long muluId=schoolMuluService.Add(mulu);
            SchoolContent scontent = new SchoolContent();
            scontent.MuluId = muluId;
            scontent.Content = content;
            contentService.Add(scontent);
            return "success";
        }
        #endregion
        #region 前台页面初始化
        public string InitStaticPageAll()
        {
            List<string> types = new MenuService().GetDistinct("select DISTINCT typename as col  from menu");
            foreach (var item in types)
            {
                InitStaticPageByTypeName(item);
            }
            InitDIYPage();
            InitStaticPageSuper();
            FileTool.RenameStaticFolder();
            return "success";
        }
        /// <summary>
        /// 给首页等其他页面静态化
        /// </summary>
        /// <returns></returns>
        public void InitStaticPageSuper()
        {
            string[] pageNames = { "Index", "About" };
            foreach(string item in pageNames){
                string url = String.Format("http://localhost:42742/SuperPage/{0}", item);
                string responseBodyAsText = HttpTool.GetHtmlContent(url);
                FileTool.Write(item, responseBodyAsText);
            }
        }
       
        public string InitStaticPageByTypeName(string type)
        {
            if (type == "ALL")
            {
                string s=InitStaticPageAll();
            }
            else
            {
                if (type == "DIY")
                {
                    InitDIYPage();
                }else
                {
                    List<Course> courseList = new CourseService().GetCourseByTypeName(type);
                    foreach (var item in courseList)
                    {
                        string url = String.Format("http://localhost:42742/Home/Index?type={0}&id={1}", type, item.Id);
                        string responseBodyAsText = HttpTool.GetHtmlContent(url);
                        FileTool.Write(type, item.Id, responseBodyAsText);
                    }
                }
            }
            return "success";
        }
        public void InitDIYPage()
        {
            var tiyContentList = tiycontentService.GetAll();
            var idList = tiycontentService.GetDistinct("select id as col from tiycontent");
            foreach(var item in idList)
            {
                string url = String.Format("http://localhost:42742/Home/DIY?id={0}", item);
                string responseBodyAsText = HttpTool.GetHtmlContent(url);
                FileTool.Write("DIY", long.Parse(item), responseBodyAsText);
            }
        }
        
        public string InitMenu(string type)
        {
            if (type == "ALL")
            {
                InitMenuAll();
            }else
            {
                string menuContent = string.Empty;
                if (type == "tuijian")
                {
                    menuContent = InitTuijian();
                }
                else
                {
                    List<Course> courseList = new CourseService().GetCourseByTypeName(type);
                    StringBuilder sbHtml = new StringBuilder();
                    string muluName = String.Empty;
                    foreach (Course course in courseList)
                    {
                        if (muluName != course.MuluName)
                        {
                            muluName = course.MuluName;
                            sbHtml.AppendFormat("<h2 class=\"left\"><span class=\"left_h2\">{0}</span></h2>", muluName);
                        }
                        sbHtml.Append(String.Format("<a target=\"_top\" title=\"{0}\" id=\"{1}\" href=\"{1}.html\">{0}</a>", course.Title, course.Id));
                    }
                    menuContent = sbHtml.ToString();
                }
                Menu menu = new MenuService().GetMenuByTypeName(type);
                if (menu == null)
                {
                    menu = new Menu();
                    menu.TypeName = type;
                    menu.Content = menuContent;
                    menuService.Add(menu);
                }
                else
                {
                    if (menuContent != menu.Content)
                    {
                        menu.Content = menuContent;
                        menuService.Update(menu);
                    }
                }
            }
            return "success";
        }
        public string InitTuijian()
        {
            List<string> types = new BaseSchoolService().GetDistinct("select distinct typename as col from course ORDER BY id desc LIMIT 0,5");
            StringBuilder sbHtml = new StringBuilder();
            foreach (string type in types)
            {
                string sql = string.Format("select title,id,SortNum from course where TypeName='{0}' ORDER BY id desc LIMIT 0,5", type);
                List<CourseSortModel> courseModels = courseService.GetBySql(sql);
                sbHtml.Append("<dl>");
                sbHtml.AppendFormat("<dt>{0}</dt>", type + "最新课程");
                foreach(var item in courseModels)
                {
                    sbHtml.AppendFormat("<dd>·<a href = \"/{0}/{1}.html\"> {2} </a ></dd > ",item.Id,item.Id,item.Title);
                }
                sbHtml.Append("</dl>");
            }
            return sbHtml.ToString();
        }
        public string setSortNum(int sortNum,long currentCourseId)
        {
            Course course = courseService.GetById(currentCourseId);
            course.SortNum = sortNum;
            courseService.Update(course);
            return "success";
        }
        public void InitMenuAll()
        {
            List<String> types = new BaseSchoolService().GetDistinct("select DISTINCT typename as col from course ");
            foreach(string type in types)
            {
                string s=InitMenu(type);
            }
            InitMenu("tuijian");
        }
        public string InitAllOnline(string type)
        {
            string result = string.Empty;
            if (type == "ALL")
            {
                InitMenuAll();
                InitStaticPageAll();
            }
            else
            {
                result=InitMenu(type);
                if (result == "success")
                {
                    result= InitStaticPageByTypeName(type);
                }
            }
            return "success";
        }
        #endregion
        #region 课程或测试内容编辑
        public IActionResult DIYEdit(string type)
        {
            var tiyList = tiyService.GetByCourseTitle(type);
            ViewData["tiyList"] = tiyList;
            return View();
        }
        public IActionResult EditCourse(long id)
        {
            Course course = courseService.GetById(id);
            ViewData["course"] = course;
            return View();
        }
        public string UpdateCourse(long id,string title,string typename,string muluname,string content,int sortnum)
        {
            Course course = new Course(typename, muluname, title, content, sortnum);
            course.Id = id;
            courseService.Update(course);
            return "success";
        }
        public JsonResult GetCourse(long id)
        {
            Course course = courseService.GetById(id);
            return Json(course);
        }
        public JsonResult GetTiyContentById(long id)
        {
            var content = tiyService.GetById(id);
            return Json(content);
        }
        public string SaveTiyContent(long id,string title,string coursetitle,string content,string type)
        {
            if (id == 0)
            {
                TiyContent tiycontent = new TiyContent(title, coursetitle, content,type);
                var newid=tiyService.Add(tiycontent);
                return string.Format("/Home/DIY/{0}.html",newid);
            }
            else
            {
                TiyContent tiycontent = new TiyContent(title, coursetitle, content,type);
                tiycontent.Id = id;
                tiyService.Update(tiycontent);
            }
            return "success";
        }
        #endregion
    }
}
