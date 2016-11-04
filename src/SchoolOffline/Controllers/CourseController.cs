using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchoolOffline.Service;
using DoctorOffline.Service;
using SchoolOffline.Entity;
using DoctorOffline.Entity;
using Dao.Service;
using System.Text;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolOffline.Controllers
{
    public class CourseController : Controller
    {
        #region 初始化service
        private CourseService courseService = new CourseService();
        private MenuService menuService = new MenuService();
        private TiyContentService tiyService = new TiyContentService();
        private SchoolMuluService schoolMuluService = new SchoolMuluService();
        private TiyContentService tiycontentService = new TiyContentService();
        private SchoolContentService contentService = new SchoolContentService();
        private SchoolMuluService muluService = new SchoolMuluService();
        private CourseTitleService titleService = new CourseTitleService();
        private CourseDraftService draftService = new CourseDraftService();
        #endregion
        /// <summary>
        /// 推送数据到线上
        /// </summary>
        /// <param name="muluId"></param>
        /// <param name="typeId"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public JsonResult Pass(long muluId, long typeId, string title, string content)
        {
            try
            {
                if (muluId > 0)
                {
                    List<Course> courseList = courseService.QueryBySql("select * from course where outerid=" + muluId);
                    if (courseList != null && courseList.Count > 0)
                    {
                        return Json("已经推送过了");
                    }
                }
                Mulu m = new MuluService().GetByMuluId(typeId);
                int maxSortNum = courseService.GetMaxSortNumByMuluName(m.MuluName);
                Course course = new Course(m.TypeName, m.MuluName, title, content, maxSortNum + 1);
                course.OuterId = muluId;
                new CourseService().Add(course);
                if (muluId > 0)
                {
                    SchoolMulu mulu = new SchoolMuluService().GetByMuluId(muluId);
                    mulu.IfPassed = 1;
                    new SchoolMuluService().Update(mulu);
                }
            }
            catch (Exception e)
            {
                return Json("fail");
            }
            return Json("success");
        }
        public JsonResult Push(long draftId)
        {
            var titleList = titleService.GetByDraftId(draftId);
            var draft = draftService.GetById(draftId);
            StringBuilder sbcontent = new StringBuilder();
            foreach (var title in titleList)
            {
                if (!string.IsNullOrEmpty(title.Content.Trim().TrimEnd("<p><br/></p>".ToCharArray())))
                {
                    sbcontent.AppendFormat("<h2>{0}</h2>{1}<hr>", title.TitleName, title.Content.TrimEnd().TrimEnd("<p><br/></p>".ToCharArray()));
                }
            }
            List<Course> courseList = courseService.QueryBySql("select * from course where draftid=" + draftId);
            if(courseList!=null && courseList.Count > 0)
            {
                var course = courseList.FirstOrDefault();
                course.Content = sbcontent.ToString();
                course.MuluName = draft.MuluName;
                course.Title = draft.Title;
                course.TypeName = draft.TypeName;
                new CourseService().Update(course, true);
            }
            else
            {
                int maxSortNum = courseService.GetMaxSortNumByMuluName(draft.MuluName);
                Course course = new Course
                {
                    Content = sbcontent.ToString(),
                    DraftId = draftId,
                    MuluName = draft.MuluName,
                    Title = draft.Title,
                    TypeName = draft.TypeName,
                    SortNum=maxSortNum+1
                };
                new CourseService().Add(course);
            }
            return Json("success");
        }
        public JsonResult Update(long muluId, long typeId, string title, string content)
        {
            List<Course> courseList = courseService.QueryBySql("select * from course where outerid=" + muluId);
            if (courseList != null && courseList.Count > 0)
            {
                var course = courseList.FirstOrDefault();
                course.Content = content;
                courseService.Update(course, true);
                return Json("success");
            }
            return Json("fial");
        }
    }
}
