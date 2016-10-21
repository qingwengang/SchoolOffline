using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchoolOffline.Util;
using SchoolOffline.Service;
using SchoolOffline.Configs;
using Microsoft.Extensions.Options;
using SchoolOffline.Entity;
using DoctorOffline.Service;
using System.Text;
using SchoolOffline.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolOffline.Controllers
{
    public class InitController : Controller
    {   
        private QuestionService questionService = new QuestionService();
        private QuestionContentService questionContentService = new QuestionContentService();
        private QuestionTypeRelationService questionTypeRelationService = new QuestionTypeRelationService();
        private CourseService courseService = new CourseService();
        private MenuService menuService = new MenuService();
        private TiyContentService tiyService = new TiyContentService();
        private SchoolMuluService schoolMuluService = new SchoolMuluService();
        private TiyContentService tiycontentService = new TiyContentService();
        private SchoolContentService contentService = new SchoolContentService();
        private SchoolMuluService muluService = new SchoolMuluService();
        public ApplicationConfiguration config;
        public InitController(IOptions<ApplicationConfiguration> option)
        {
            config = option.Value;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public string ReplaceStatic()
        {
            //FileTool.RenameStaticFolder();
            return "success";
        }

        public string InitQuestionPageAll()
        {
            InitQuestionListStaticPageAll();
            return "success";
        }
        #region 初始化笔试题库
        /// <summary>
        /// 初始化所有笔试题库的列表页面
        /// </summary>
        private void InitQuestionListStaticPageAll()
        {
            List<String> types = questionService.GetDistinct("select Distinct type as col from question");
            foreach (string type in types)
            {
                InitQuestionListByType(type);
            }
        }
        /// <summary>
        /// 根据Type字段初始化笔试题库列表页
        /// </summary>
        /// <param name="type"></param>
        private void InitQuestionListByType(string type)
        {
            string countStr = questionService.QueryOne(string.Format("select count(id) as col from question where type='{0}'", type));
            int pageCount = (int.Parse(countStr)+9) / 10;
            for(int i = 1; i <= pageCount; i++)
            {
                string url = String.Format(config.pageUrl + "Home/QuestionList?type={0}&page={1}", type, i);
                string responseBodyAsText = HttpTool.GetHtmlContent(url);
                FileTool.Write(type, i, responseBodyAsText,"QuestionList");
            }
        }
        /// <summary>
        /// 初始化笔试题详细页面
        /// </summary>
        private void InitQuestionStaticPageAll()
        {
            List<QuestionContent> questonContentList = questionContentService.QueryBySql("select rootid,pageid from questioncontent ");
            foreach(var item in questonContentList)
            {
                string url = String.Format(config.pageUrl + "Home/Question?rootId={0}&pageId={1}", item.RootId,item.PageId);
                string responseBodyAsText = HttpTool.GetHtmlContent(url);
                FileTool.Write(item.RootId.ToString(), item.PageId, responseBodyAsText, "Question");
            }
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
            InitQuestionListStaticPageAll();
            InitQuestionStaticPageAll();
            //FileTool.RenameStaticFolder();
            return "success";
        }
        /// <summary>
        /// 给首页等其他页面静态化
        /// </summary>
        /// <returns></returns>
        public void InitStaticPageSuper()
        {
            string[] pageNames = { "Index", "About" };
            foreach (string item in pageNames)
            {
                string url = String.Format(config.pageUrl + "SuperPage/{0}", item);
                string responseBodyAsText = HttpTool.GetHtmlContent(url);
                FileTool.Write(item, responseBodyAsText);
            }
        }

        public string InitStaticPageByTypeName(string type)
        {
            if (type == "ALL")
            {
                string s = InitStaticPageAll();
            }
            else
            {
                if (type == "DIY")
                {
                    InitDIYPage();
                }
                else
                {
                    List<Course> courseList = new CourseService().GetCourseByTypeName(type);
                    foreach (var item in courseList)
                    {
                        string url = String.Format(config.pageUrl + "Home/Index?type={0}&id={1}", type, item.Id);
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
            foreach (var item in idList)
            {
                string url = String.Format(config.pageUrl + "Home/DIY?id={0}", item);
                string responseBodyAsText = HttpTool.GetHtmlContent(url);
                FileTool.Write("DIY", long.Parse(item), responseBodyAsText);
            }
        }

        public string InitMenu(string type)
        {
            if (type == "ALL")
            {
                InitMenuAll();
            }
            else
            {
                string menuContent = string.Empty;
                if (type == "tuijian")
                {
                    menuContent = InitTuijian();
                } else if (type=="questionmenu")
                {
                    menuContent = InitQuestionMenu();
                }
                else if (type == "questioncoursemenu")
                {
                    menuContent = InitQuestionCourseMenu();
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
                        sbHtml.Append(String.Format("<a target=\"_top\" title=\"{0}\" id=\"{1}\" href=\"/{2}/{1}.html\">{0}</a>", course.Title, course.Id, type));
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
            List<string> types = new BaseSchoolService().GetDistinct("select distinct typename as col from course where typename!='AboutUs' ORDER BY id desc LIMIT 0,4");
            types.Add("AboutUs");
            StringBuilder sbHtml = new StringBuilder();
            foreach (string type in types)
            {
                string sql = string.Format("select title,id,SortNum from course where TypeName='{0}' ORDER BY id desc LIMIT 0,5", type);
                List<CourseSortModel> courseModels = courseService.GetBySql(sql);
                sbHtml.Append("<dl>");
                if (type != "AboutUs")
                {
                    sbHtml.AppendFormat("<dt>{0}</dt>", type + "最新课程");
                }
                else
                {
                    sbHtml.Append("关于我们");
                }

                foreach (var item in courseModels)
                {
                    sbHtml.AppendFormat("<dd>·<a href = \"/{0}/{1}.html\"> {2} </a ></dd > ", type, item.Id, item.Title);
                }
                sbHtml.Append("</dl>");
            }
            return sbHtml.ToString();
        }
        private string InitQuestionMenu()
        {
            List<QuestionTypeRelation> relationList = questionTypeRelationService.GetAll();
            StringBuilder sbHtml = new StringBuilder();
            foreach(var item in relationList)
            {
                sbHtml.AppendFormat("<a href=\"/QuestionList/{0}/1.html\" target =\"_blank\" title=\"{1}\">{1}</a>", item.QuestionType, item.QuestionTypeDesc);
            }
            return sbHtml.ToString();
        }
        private string InitQuestionCourseMenu()
        {
            StringBuilder sbhtml = new StringBuilder();
            List<string> muluTypeList=new MuluService().GetDistinctTypeName();
            foreach(var muluType in muluTypeList)
            {
                if (muluType != "AboutUs")
                {
                    var course = courseService.GetMinCourseByType(muluType);
                    if (course != null)
                    {
                        sbhtml.AppendFormat("<a href=\"/{0}/{1}.html\" title=\"{0}教程\">{0} 教程</a>", muluType, course.Id);
                    }
                }
            }
            return sbhtml.ToString();
        }
        
        public void InitMenuAll()
        {
            List<String> types = new BaseSchoolService().GetDistinct("select DISTINCT typename as col from course ");
            foreach (string type in types)
            {
                string s = InitMenu(type);
            }
            InitMenu("tuijian");
            InitMenu("questionmenu");
            InitMenu("questioncoursemenu");
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
                result = InitMenu(type);
                if (result == "success")
                {
                    result = InitStaticPageByTypeName(type);
                }
            }
            return "success";
        }
        #endregion
    }
}
