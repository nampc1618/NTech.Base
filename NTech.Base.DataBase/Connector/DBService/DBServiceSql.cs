using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using NTech.Base.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Base.DataBase.Connector.DBService
{
    public class DBServiceSql : DBServiceBase
    {
        public DBServiceSql(string connectionString)
        {
            this.ConnectionString = connectionString;
            this.Database = new SqlDatabase(connectionString);
        }
        public override DBResultModel ExecuteDataSet(DBRequestModel requestModel)
        {
            DBResultModel resultModel = null;
            try
            {
                resultModel = new DBResultModel(-1, this.Database.ExecuteDataSet(this.GenerateCommand(requestModel)), null);
            }
            catch (Exception)
            {
                return null;
            }
            return resultModel;
        }
        public override DBResultModel ExecuteScalar(DBRequestModel requestModel)
        {
            DBResultModel resultModel = null;
            try
            {
                var result = this.Database.ExecuteScalar(this.GenerateCommand(requestModel));
                resultModel = new DBResultModel(-1, null, result != DBNull.Value ? result : null);
            }
            catch (Exception ex)
            {
                this.AddExceptionData(ex, requestModel);
                throw ex;
            }
            return resultModel;
        }
        public override DBResultModel ExecuteNonQuery(DBRequestModel requestModel)
        {
            DBResultModel resultModel = null;
            try
            {
                resultModel = new DBResultModel(this.Database.ExecuteNonQuery(this.GenerateCommand(requestModel)), null, null);
            }
            catch (Exception ex)
            {
                this.AddExceptionData(ex, requestModel);
                throw ex;
            }
            return resultModel;
        }
        public override DBResultModel ExecuteNonQueryMulti(DBRequestModel requestModel)
        {
            DBResultModel resultModel = null;

            if (requestModel.QueryInfos != null && requestModel.QueryInfos.Any())
            {
                try
                {
                    using (DbConnection connection = this.Database.CreateConnection())
                    {
                        int count = 0;

                        connection.Open();

                        var queryList = requestModel.QueryInfos.Where(x => !string.IsNullOrWhiteSpace(x.Item1));
                        while (queryList.Count() > 0)
                        {
                            DbTransaction transaction = connection.BeginTransaction();

                            try
                            {
                                string query = requestModel.QueryInfos.First().Item1;
                                using (DbCommand command = (requestModel.IsStoredProcedure == false ? Database.GetSqlStringCommand(query) : Database.GetStoredProcCommand(query)))
                                {
                                    foreach (var queryInfo in queryList.Take(requestModel.TakeCount).ToList())
                                    {
                                        try
                                        {
                                            command.CommandText = queryInfo.Item1;

                                            command.Parameters.Clear();
                                            if (queryInfo.Item2 != null)
                                            {
                                                foreach (var item in queryInfo.Item2)
                                                {
                                                    SqlParameter param = new SqlParameter();
                                                    param.ParameterName = item.ParameterName;
                                                    param.Value = item.Value == null ? null : item.Value;
                                                    param.SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), item.DbType);
                                                    param.Direction = item.Direction;
                                                    if (item.Size > 0) param.Size = item.Size;

                                                    command.Parameters.Add(param);
                                                }
                                            }

                                            count += this.Database.ExecuteNonQuery(command, transaction);
                                        }
                                        catch (Exception ex)
                                        {
                                            this.AddExceptionData(ex, queryInfo.Item1, queryInfo.Item2, requestModel.UserID);
                                            throw ex;
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                transaction.Rollback();
                                throw;
                            }

                            transaction.Commit();
                            queryList = queryList.Skip(requestModel.TakeCount);
                        }

                        resultModel = new DBResultModel(count, null, null);
                    }
                }
                catch
                {
                    throw;
                }
            }

            return resultModel;
        }

        public DbCommand GenerateCommand(DBRequestModel requestModel)
        {
            var command = requestModel.IsStoredProcedure == false ? Database.GetSqlStringCommand(requestModel.Query) : Database.GetSqlStringCommand(requestModel.Query);
            if (requestModel.Parameters != null)
            {
                foreach (var item in requestModel.Parameters)
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = item.ParameterName;
                    param.Value = item.Value == null ? null : item.Value;
                    param.SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), item.DbType);
                    param.Direction = item.Direction;
                    if(item.Size > 0) param.Size = item.Size;

                    command.Parameters.Add(param);
                }
            }
            return command;
        }

        #region [ =============== Delete =============== ]
        /// <summary>
        /// ExecuteDataSet - MS SQL
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <param name="isStoredProcedure"></param>
        /// <returns></returns>
        //public override DBResultModel ExecuteDataSet(string query, List<DBParameterModel> parameters, bool isStoredProcedure = false)
        //{
        //    DBResultModel resultModel = null;
        //    try
        //    {
        //        resultModel = new DBResultModel(-1, this.Database.ExecuteDataSet(this.GenerateCommand(query, parameters, isStoredProcedure)), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        this.AddExceptionData(ex, query, parameters);
        //        throw ex;
        //    }
        //    return resultModel;
        //}

        /// <summary>
        /// ExecuteScalar - MS SQL
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <param name="isStoredProcedure"></param>
        /// <returns></returns>
        //public override DBResultModel ExecuteScalar(string query, List<DBParameterModel> parameters, bool isStoredProcedure = false)
        //{
        //    DBResultModel resultModel = null;
        //    try
        //    {
        //        var result = this.Database.ExecuteScalar(this.GenerateCommand(query, parameters, isStoredProcedure));
        //        resultModel =  new DBResultModel(-1, null, result != DBNull.Value ? result : null);
        //    }
        //    catch (Exception ex)
        //    {
        //        this.AddExceptionData(ex, query, parameters);
        //        throw ex;
        //    }
        //    return resultModel;
        //}

        /// <summary>
        /// ExecuteNonQuery - MS SQL
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <param name="isStoredProcedure"></param>
        /// <returns></returns>
        //public override DBResultModel ExecuteNonQuery(string query, List<DBParameterModel> parameters, bool isStoredProcedure = false)
        //{
        //    DBResultModel resultModel = null;
        //    try
        //    {
        //        resultModel = new DBResultModel(this.Database.ExecuteNonQuery(this.GenerateCommand(query, parameters, isStoredProcedure)), null, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        this.AddExceptionData(ex, query, parameters);
        //        throw ex;
        //    }
        //    return resultModel;
        //}

        /// <summary>
        /// Multi ExecuteNonQuery - MS SQL
        /// </summary>
        /// <param name="queryInfos"></param>
        /// <param name="isStoredProcedure"></param>
        /// <param name="takeCount"></param>
        /// <returns></returns>
        //public override DBResultModel ExecuteNonQueryMulti(List<Tuple<string, List<DBParameterModel>>> queryInfos, bool isStoredProcedure = false, int takeCount = 100)
        //{
        //    DBResultModel resultModel = null;

        //    if (queryInfos != null && queryInfos.Any())
        //    {
        //        try
        //        {
        //            using (DbConnection connection = this.Database.CreateConnection())
        //            {
        //                int count = 0;

        //                connection.Open();

        //                var queryList = queryInfos.Where(x => !string.IsNullOrWhiteSpace(x.Item1));
        //                while (queryList.Count() > 0)
        //                {
        //                    DbTransaction transaction = connection.BeginTransaction();

        //                    try
        //                    {
        //                        string query = queryInfos.First().Item1;
        //                        using (DbCommand command = (isStoredProcedure == false ? Database.GetSqlStringCommand(query) : Database.GetStoredProcCommand(query)))
        //                        {
        //                            foreach (var queryInfo in queryList.Take(takeCount).ToList())
        //                            {
        //                                try
        //                                {
        //                                    command.CommandText = queryInfo.Item1;

        //                                    command.Parameters.Clear();
        //                                    if (queryInfo.Item2 != null)
        //                                    {
        //                                        foreach (var item in queryInfo.Item2)
        //                                        {
        //                                            SqlParameter param = new SqlParameter();
        //                                            param.ParameterName = item.ParameterName;
        //                                            param.Value = item.Value == null ? DBNull.Value : item.Value;
        //                                            param.SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), item.DbType);
        //                                            param.Direction = item.Direction;
        //                                            if (item.Size > 0) param.Size = item.Size;

        //                                            command.Parameters.Add(param);
        //                                        }
        //                                    }

        //                                    count += this.Database.ExecuteNonQuery(command, transaction);
        //                                }
        //                                catch (Exception ex)
        //                                {
        //                                    this.AddExceptionData(ex, queryInfo.Item1, queryInfo.Item2);
        //                                    throw ex;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    catch
        //                    {
        //                        transaction.Rollback();
        //                        throw;
        //                    }

        //                    transaction.Commit();
        //                    queryList = queryList.Skip(takeCount);
        //                }

        //                resultModel = new DBResultModel(count, null, null);
        //            }
        //        }
        //        catch
        //        {
        //            throw;
        //        }
        //    }

        //    return resultModel;
        //}
        #endregion // [ =============== Delete =============== ]
    }
}
