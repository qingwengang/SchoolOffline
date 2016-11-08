using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dao.Entity
{
    public class CsdnContent
    {
        public long Id { get; set; }
        public long MuluId { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
    }
}
