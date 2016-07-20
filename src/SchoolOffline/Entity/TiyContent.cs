using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolOffline.Entity
{
    public class TiyContent
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string CourseTitle { get; set; }
        public TiyContent() { }
        public TiyContent(string title,string courseTitle,string content)
        {
            this.Title = title;
            this.CourseTitle = courseTitle;
            this.Content = content;
        }
    }
}
