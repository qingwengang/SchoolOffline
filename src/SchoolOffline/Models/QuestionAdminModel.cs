using SchoolOffline.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolOffline.Models
{
    public class QuestionAdminModel
    {
        public string html { get; set; }
        public List<QuestionTypeRelation> relationList { get; set; }
    }
}
