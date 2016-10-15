using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolOffline.Entity
{
    public class Question
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Des { get; set; }
        public DateTime CreateTime { get; set; }

    }
}
