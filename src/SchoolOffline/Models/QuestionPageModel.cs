using SchoolOffline.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolOffline.Models
{
    public class QuestionPageModel
    {
        public int currentPage { get; set; }
        public int pageCount { get; set; }
        public List<Question> questionList { get; set; }
        public string questionTypeDesc { get; set; }
        public string canonical { get; set; }
    }
}
