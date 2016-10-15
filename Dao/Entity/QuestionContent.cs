using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolOffline.Entity
{
    public class QuestionContent
    {
        public long Id { get; set; }
        public long RootId { get; set; }
        public int PageId { get; set; }
        public String Content { get; set; }
        public int PageCount { get; set; }
    }
}
