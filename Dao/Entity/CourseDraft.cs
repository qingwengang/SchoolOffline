using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dao.Entity
{
    public class CourseDraft
    {
        public long Id { get; set; }
        public string TypeName { get; set; }
        public string MuluName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int SortNum { get; set; }
    }
}
