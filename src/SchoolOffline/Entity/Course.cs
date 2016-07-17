using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolOffline.Entity
{
    public class Course
    {
        public long Id { get; set; }
        public string TypeName { get; set; }
        public string MuluName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int SortNum { get; set; }

        public Course() { }

        public Course(string typeName,string muluName,string title,string content,int sortNum)
        {
            this.TypeName = typeName;
            this.MuluName = muluName;
            this.Title = title;
            this.Content = content;
            this.SortNum = sortNum;
        }
    }
}
