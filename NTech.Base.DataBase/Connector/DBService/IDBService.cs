using Microsoft.Practices.EnterpriseLibrary.Data;
using NTech.Base.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Base.DataBase.Connector.DBService
{
    public interface IDBService
    {
        /// <summary>
        /// Connection string
        /// </summary>
        string ConnectionString { get; set; }
        /// <summary>
        /// Database
        /// </summary>
        Database Database { get; set; }
        /// <summary>
        /// ExecuteDataSet - MS SQL
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        DBResultModel ExecuteDataSet(DBRequestModel requestModel);
        /// <summary>
        /// ExecuteScalar - MS SQL
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        DBResultModel ExecuteScalar(DBRequestModel requestModel);
        /// <summary>
        /// ExecuteNonQuery - MS SQL
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        DBResultModel ExecuteNonQuery(DBRequestModel requestModel);
        /// <summary>
        /// Mutil ExecuteNonQuery - MS SQL
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        DBResultModel ExecuteNonQueryMulti(DBRequestModel requestModel);

        #region [ =============== Delete =============== ]
        /// <summary>
        /// ExecuteDataSet - MS SQL
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <param name="isStoredProcedure"></param>
        /// <returns></returns>
        //DBResultModel ExecuteDataSet(string query, List<DBParameterModel> parameters, bool isStoredProcedure = false);

        /// <summary>
        /// ExecuteScalar - MS SQL
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <param name="isStoredProcedure"></param>
        /// <returns></returns>
        //DBResultModel ExecuteScalar(string query, List<DBParameterModel> parameters, bool isStoredProcedure = false);

        /// <summary>
        /// ExecuteNonQuery - MS SQL
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <param name="isStoredProcedure"></param>
        /// <returns></returns>
        //DBResultModel ExecuteNonQuery(string query, List<DBParameterModel> parameters, bool isStoredProcedure = false);

        /// <summary>
        /// Multi ExecuteNonQuery - MS SQL
        /// </summary>
        /// <param name="queryInfos"></param>
        /// <param name="isStoredProcedure"></param>
        /// <param name="takeCount"></param>
        /// <returns></returns>
        //DBResultModel ExecuteNonQueryMulti(List<Tuple<string, List<DBParameterModel>>> queryInfos, bool isStoredProcedure = false, int takeCount = 100);
        #endregion // [ =============== Delete =============== ]
    }
}
