using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dao.Entity
{
    public class CourseTitle
    {
        public long Id { get; set; }
        public long DraftId { get; set; }
        public string TitleName { get; set; }
        public string Content { get; set; }
        public int SortNum { get; set; }
    }
}
