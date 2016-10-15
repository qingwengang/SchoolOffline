using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolOffline.Entity
{
    public class QuestionTypeRelation
    {
        public long Id { get; set; }
        public string QuestionType { get; set; }
        public string QuestionTypeDesc { get; set; }
    }
}
