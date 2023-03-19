using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Core.Camera
{
    public interface ICamera : IDisposable
    {
        string Manufacturing { get; }
        string Seri { get;}
        string CameraName { get; }
        string CameraType { get; }

        void Open();
        void Close();
    }
}
