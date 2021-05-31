using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineAttendance.Models
{
    public partial class zclass
    {
        public List<zclass> zclasses { get; set; }
    }
    public partial class faculty
    {
        public List<faculty> faculties { get; set; }
        public String type { get; set; }
    }
}