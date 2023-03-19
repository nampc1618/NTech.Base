using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Base.DataBase.Models
{
    public class DBResultModel
    {
        /// <summary>
        /// Applied row count
        /// </summary>
        public int RowCount { get; set; }
        /// <summary>
        /// The result set returned by query
        /// </summary>
        public DataSet DataSet { get; set; }
        /// <summary>
        /// The first column of the first row in the result set returned by the query
        /// </summary>
        public object ReturnValue { get; set; }

        public DBResultModel()
        {

        }
        public DBResultModel(int rowCount, DataSet dataSet, object returnValue)
        {
            this.RowCount = rowCount;
            this.DataSet = dataSet;
            this.ReturnValue = returnValue;
        }
    }
}
