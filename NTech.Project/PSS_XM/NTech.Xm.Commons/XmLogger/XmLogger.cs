using NLog;
using NTech.Base.Commons.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Xm.Commons.XmLogger
{
    public class XmLogger : ILog
    {
        Dictionary<LogTypes, Logger> _loggers = new Dictionary<LogTypes, Logger>();

        public void GenerateLogger()
        {
            string[] logTypes = Enum.GetNames(typeof(LogTypes));
            foreach (var item in logTypes)
            {
                LogTypes logType = (LogTypes)Enum.Parse(typeof(LogTypes), item);
                if(this._loggers.ContainsKey(logType) == false)
                {
                    this._loggers.Add(logType, LogManager.GetLogger(item));
                }
            }
        }
        private Logger FindLogger(LogTypes logType)
        {
            return this._loggers.Where(f => f.Key == logType).Select(f => f.Value).FirstOrDefault();
        }
        public void Write(string message, LogTypes logType = LogTypes.NTechInfo)
        {
            var logger = this.FindLogger(logType);
            if (logger != null)
            {
                switch (logType)
                {
                    case LogTypes.NTechInfo:
                        logger.Info(message);
                        this.WriteLogViewPopup("INFO", message);
                        break;
                    case LogTypes.NTechError:
                        logger.Error(message);
                        this.WriteLogViewPopup("ERROR", message);
                        break;
                    case LogTypes.NTechWarning:
                        logger.Warn(message);
                        this.WriteLogViewPopup("WARN", message);
                        break;
                    case LogTypes.NTechDBConnector:
                        logger.Info(message);
                        this.WriteLogViewPopup("DBCONNECTOR", message);
                        break;
                    case LogTypes.NTechDebug:
#if DEBUG
                        logger.Debug(message);
#endif
                        this.WriteLogViewPopup("DEBUG", message);
                        break;
                }
            }
        }
        public void Write(Exception ex, LogTypes logType = LogTypes.NTechInfo)
        {
            var logger = this.FindLogger(logType);
            var exLogger = this.FindLogger(LogTypes.NTechError);
            if (logger != null)
            {
                switch (logType)
                {
                    case LogTypes.NTechInfo:
                        logger.Error(ex);
                        exLogger.Error(ex);
                        this.WriteLogViewPopup("INFO", ex);
                        break;
                    case LogTypes.NTechError:
                        logger.Error(ex);
                        this.WriteLogViewPopup("ERROR", ex);
                        break;
                    case LogTypes.NTechWarning:
                        logger.Error(ex);
                        exLogger.Error(ex);
                        this.WriteLogViewPopup("GUICONNECTOR", ex);
                        break;
                    case LogTypes.NTechDBConnector:
                        logger.Error(ex);
                        exLogger.Error(ex);
                        this.WriteLogViewPopup("DBCONNECTOR", ex);
                        break;
                    case LogTypes.NTechDebug:
#if DEBUG
                        logger.Error(ex);
                        exLogger.Error(ex);
#endif
                        this.WriteLogViewPopup("DEBUG", ex);
                        break;
                }
            }
        }
        public void Write(Exception ex, string message, LogTypes logType = LogTypes.NTechInfo)
        {
            var logger = this.FindLogger(logType);
            if (logger != null)
            {
                switch (logType)
                {
                    case LogTypes.NTechInfo:
                        logger.Info(ex, message);
                        this.WriteLogViewPopup("INFO", ex, message);
                        break;
                    case LogTypes.NTechError:
                        logger.Error(ex, message);
                        this.WriteLogViewPopup("ERROR", ex, message);
                        break;
                    case LogTypes.NTechWarning:
                        logger.Info(ex, message);
                        this.WriteLogViewPopup("GUICONNECTOR", ex, message);
                        break;
                    case LogTypes.NTechDBConnector:
                        logger.Info(ex, message);
                        this.WriteLogViewPopup("DBCONNECTOR", ex, message);
                        break;
                    case LogTypes.NTechDebug:
#if DEBUG
                        logger.Debug(ex, message);
#endif
                        this.WriteLogViewPopup("DEBUG", ex, message);
                        break;
                }
            }
        }

        public void WriteLogViewPopup(string level, string message)
        {
        }

        public void WriteLogViewPopup(string level, Exception ex)
        {
        }

        public void WriteLogViewPopup(string level, Exception ex, string message)
        {
        }
    }
}
