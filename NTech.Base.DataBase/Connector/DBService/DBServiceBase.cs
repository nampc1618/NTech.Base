using Microsoft.Practices.EnterpriseLibrary.Data;
using NTech.Base.Commons.Logger;
using NTech.Base.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Base.DataBase.Connector.DBService
{
    public abstract class DBServiceBase : IDBService
    {
        /// <summary>
        /// ConnectionString
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// Database
        /// </summary>
        public Database Database { get; set; }

        #region [ =============== ExecuteDataSet =============== ]
        /// <summary>
        /// ExecuteDataSet
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public abstract DBResultModel ExecuteDataSet(DBRequestModel requestModel);

        /// <summary>
        /// ExecuteDataSet
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        DBResultModel IDBService.ExecuteDataSet(DBRequestModel requestModel)
        {
            this.WriteLog(requestModel);

            return ExecuteDataSet(requestModel);
        }
        #endregion // [ =============== ExecuteDataSet =============== ]


        #region [ =============== ExecuteScalar =============== ]
        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public abstract DBResultModel ExecuteScalar(DBRequestModel requestModel);

        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        DBResultModel IDBService.ExecuteScalar(DBRequestModel requestModel)
        {
            this.WriteLog(requestModel);

            return ExecuteScalar(requestModel);
        }
        #endregion // [ =============== ExecuteScalar =============== ]


        #region [ =============== ExecuteNonQuery =============== ]
        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public abstract DBResultModel ExecuteNonQuery(DBRequestModel requestModel);

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        DBResultModel IDBService.ExecuteNonQuery(DBRequestModel requestModel)
        {
            this.WriteLog(requestModel);

            return ExecuteNonQuery(requestModel);
        }
        #endregion // [ =============== ExecuteNonQuery =============== ]


        #region [ =============== ExecuteNonQueryMulti =============== ]
        /// <summary>
        /// ExecuteNonQueryMulti
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public abstract DBResultModel ExecuteNonQueryMulti(DBRequestModel requestModel);

        /// <summary>
        /// ExecuteNonQueryMulti
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        DBResultModel IDBService.ExecuteNonQueryMulti(DBRequestModel requestModel)
        {
            this.WriteLog(requestModel);

            return ExecuteNonQueryMulti(requestModel);
        }
        #endregion // [ =============== ExecuteNonQueryMulti =============== ]


        #region [ =============== Log / Exception =============== ]
        /// <summary>
        /// Add QueryInfo in Excetion Data
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        public void AddExceptionData(Exception ex, string query, List<DBParameterModel> parameters, string userID)
        {
            ex.Data.Add("DBServiceException", this.GetLogText(query, parameters, userID));
        }

        /// <summary>
        /// Add QueryInfo in Excetion Data
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="requestModel"></param>
        public void AddExceptionData(Exception ex, DBRequestModel requestModel)
        {
            ex.Data.Add("DBServiceException", requestModel != null ? this.GetLogText(requestModel) : "DBRequest is null.");
        }

        ///// <summary>
        ///// Logging Query Info
        ///// </summary>
        ///// <param name="query"></param>
        ///// <param name="parameters"></param>
        //public void WriteLog(string query, List<DBParameterModel> parameters)
        //{
        //    LogHub.Write(this.GetLogText(query, parameters), LogTypes.CCIDBConnector);
        //}
        public void WriteLog(DBRequestModel requestModel)
        {
            if (requestModel != null)
            {
                LogHub.Write(this.GetLogText(requestModel), LogTypes.NTechDBConnector);
            }
        }

        /// <summary>
        /// Get Log Text
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public string GetLogText(DBRequestModel requestModel)
        {
            string userInfo = string.Empty;
            if (!string.IsNullOrEmpty(requestModel.UserID))
            {
                userInfo = $"[USERID]:{requestModel.UserID}{Environment.NewLine}";
            }

            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(requestModel.Query))
            {
                sb.Append(this.GetLogText(requestModel.Query, requestModel.Parameters));
            }

            if (requestModel.QueryInfos != null)
            {
                foreach (var info in requestModel.QueryInfos)
                {
                    sb.AppendLine(this.GetLogText(info.Item1, info.Item2));
                }
            }

            return $"{userInfo}{sb.ToString()}";
        }

        /// <summary>
        /// Get Log Text
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string GetLogText(string query, List<DBParameterModel> parameters, string userID = "")
        {
            string userInfo = string.Empty;
            if (!string.IsNullOrEmpty(userID))
            {
                userInfo = $"[USERID]:{userID}{Environment.NewLine}";
            }

            string paramInfo = string.Empty;
            if (parameters != null && parameters.Any())
            {
                paramInfo = $" [PARAMS]:{string.Join(",", parameters.Select(x => $"{x.ParameterName}={x.Value}").ToArray())}";
            }

            return $"{userInfo}[QUERY]:{query}{paramInfo}";
        }
        #endregion // [ =============== Log / Exception =============== ]


        #region [ =============== Delete =============== ]
        /// <summary>
        /// ExecuteDataSet - MS SQL
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <param name="isStoredProcedure"></param>
        /// <returns></returns>
        //public abstract DBResultModel ExecuteDataSet(string query, List<DBParameterModel> parameters, bool isStoredProcedure = false);

        /// <summary>
        /// ExecuteScalar - MS SQL
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <param name="isStoredProcedure"></param>
        /// <returns></returns>
        //public abstract DBResultModel ExecuteScalar(string query, List<DBParameterModel> parameters, bool isStoredProcedure = false);

        /// <summary>
        /// ExecuteNonQuery - MS SQL
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <param name="isStoredProcedure"></param>
        /// <returns></returns>
        //public abstract DBResultModel ExecuteNonQuery(string query, List<DBParameterModel> parameters, bool isStoredProcedure = false);

        /// <summary>
        /// Multi ExecuteNonQuery - MS SQL
        /// </summary>
        /// <param name="queryInfos"></param>
        /// <param name="isStoredProcedure"></param>
        /// <param name="takeCount"></param>
        /// <returns></returns>
        //public abstract DBResultModel ExecuteNonQueryMulti(List<Tuple<string, List<DBParameterModel>>> queryInfos, bool isStoredProcedure = false, int takeCount = 100);

        /// <summary>
        /// ExecuteDataSet - MS SQL
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <param name="isStoredProcedure"></param>
        /// <returns></returns>
        //DBResultModel IDBService.ExecuteDataSet(string query, List<DBParameterModel> parameters, bool isStoredProcedure = false)
        //{
        //    this.WriteLog(query, parameters);

        //    return ExecuteDataSet(query, parameters, isStoredProcedure);
        //}

        /// <summary>
        /// ExecuteScalar - MS SQL
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <param name="isStoredProcedure"></param>
        /// <returns></returns>
        //DBResultModel IDBService.ExecuteScalar(string query, List<DBParameterModel> parameters, bool isStoredProcedure = false)
        //{
        //    this.WriteLog(query, parameters);

        //    return ExecuteScalar(query, parameters, isStoredProcedure);
        //}

        /// <summary>
        /// ExecuteNonQuery - MS SQL
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <param name="isStoredProcedure"></param>
        /// <returns></returns>
        //DBResultModel IDBService.ExecuteNonQuery(string query, List<DBParameterModel> parameters, bool isStoredProcedure = false)
        //{
        //    this.WriteLog(query, parameters);

        //    return ExecuteNonQuery(query, parameters, isStoredProcedure);
        //}

        /// <summary>
        /// Multi ExecuteNonQuery - MS SQL
        /// </summary>
        /// <param name="queryInfos"></param>
        /// <param name="isStoredProcedure"></param>
        /// <param name="takeCount"></param>
        /// <returns></returns>
        //DBResultModel IDBService.ExecuteNonQueryMulti(List<Tuple<string, List<DBParameterModel>>> queryInfos, bool isStoredProcedure = false, int takeCount = 100)
        //{
        //    this.WriteLog(queryInfos);

        //    return ExecuteNonQueryMulti(queryInfos, isStoredProcedure, takeCount);
        //}

        ///// <summary>
        ///// Logging Query Info
        ///// </summary>
        ///// <param name="queryInfos"></param>
        //public void WriteLog(List<Tuple<string, List<DBParameterModel>>> queryInfos)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    foreach (var info in queryInfos)
        //    {
        //        sb.AppendLine(this.GetLogText(info.Item1, info.Item2));
        //    }

        //    LogHub.Write(sb.ToString(), LogTypes.CCIDBConnector);
        //}
        #endregion // [ =============== Delete =============== ]

    }
}
