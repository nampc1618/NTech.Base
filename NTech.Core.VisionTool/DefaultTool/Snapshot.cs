using NTech.Core.VisionTool.DefaultTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Core.VisionTool.Tools
{
    public class Snapshot
    {
        public Snapshot()
        {
        }
        public AcquireImage Acquire { get; set; }
        public List<Recipe> Recipes { get; set; }
    }
}
