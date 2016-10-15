using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorOffline.Entity
{
    public class SchoolMulu
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Sort { get; set; }
        public string OutUrl { get; set; }
        public string Type1 { get; set; }
        public string Type2 { get; set; }
        public int SpiderFlag { get; set; }
        public int IfPassed { get; set; }
    }
}
