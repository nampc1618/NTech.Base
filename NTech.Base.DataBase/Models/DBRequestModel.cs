using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTech.Base.Commons.BaseModel;

namespace NTech.Base.DataBase.Models
{
    public class DBRequestModel
    {
        /// <summary>
        /// DB Instance Name
        /// </summary>
        public string DBName { get; set; }
        /// <summary>
        /// Login UserInfo
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// Input Data
        /// </summary>
        public BaseModel InputData { get; set; }
        /// <summary>
        /// Custom Input Data
        /// </summary>
        public object[] CustomInputData { get; set; }
        /// <summary>
        /// Query
        /// </summary>
        public string Query { get; set; }
        /// <summary>
        /// DBParameterModel List
        /// </summary>
        public List<DBParameterModel> Parameters { get; set; }
        /// <summary>
        /// Check whether is StoreProcedure or no?
        /// </summary>
        public bool IsStoredProcedure { get; set; } = false;
        /// <summary>
        /// QueryInfo List (Query, DBParameterModel List)
        /// </summary>
        public List<Tuple<string, List<DBParameterModel>>> QueryInfos { get; set; }
        /// <summary>
        /// Take Count (ExecuteNonQueryMulti)
        /// </summary>
        public int TakeCount { get; set; } = 100;
    }
}
