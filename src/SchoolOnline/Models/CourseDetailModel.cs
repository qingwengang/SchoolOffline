using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolOffline.Models
{
    /// <summary>
    /// 课程详细页面专用model
    /// </summary>
    public class CourseDetailModel
    {
        public string title { get; set; }
        public string menuHtml { get; set; }
        public string lastPageHref { get; set; }
        public string nextPageHref { get; set; }
        public long pageId { get; set; }
        public string content { get; set; }
        public string tuijianmenuHtml { get; set; }
        public string desc { get; set; }
        public string canonical { get; set; }
        public string tuijian { get; set; }
    }
}
