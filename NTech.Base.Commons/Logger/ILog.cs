using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Base.Commons.Logger
{
    public interface ILog
    {
        void GenerateLogger();
        void Write(string message, LogTypes logType = LogTypes.NTechInfo);
        void Write(Exception ex, LogTypes logType = LogTypes.NTechInfo);
        void Write(Exception ex, string message, LogTypes logType = LogTypes.NTechInfo);
    }
    public enum LogTypes
    {
        NTechInfo,
        NTechError,
        NTechWarning,
        NTechDebug,
        NTechDBConnector
    }
}
