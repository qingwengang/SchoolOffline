using SchoolOffline.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolOffline.Models
{
    public class QuestionModel
    {
        public QuestionContent questionContent { get; set; }
        public string title { get; set; }
        public string canonical { get; set; }
        public string type { get; set; }
        public string questionTypeDesc { get; set; }
    }
}
