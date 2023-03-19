using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Core.VisionTool
{
    public interface ITool
    {
        string Id { get; set; }
        string Type { get; set; }
        string Name { get; set; }
    }
}
