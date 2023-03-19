using NTech.Core.VisionTool.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Core.VisionTool.DefaultTool
{
    public class Inspection
    {
        public Inspection()
        {
        }
        public Snapshot Snapshot { get; set; }
        public bool LogicforInspPass { get; set; }
        public bool Result { get; set; }
    }
}
