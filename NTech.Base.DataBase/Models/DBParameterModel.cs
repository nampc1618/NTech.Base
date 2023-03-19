using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Base.DataBase.Models
{
    public class DBParameterModel
    {
        /// <summary>
        /// Parameter Name
        /// </summary>
        public string ParameterName { get; set; }
        /// <summary>
        /// Parameter Value
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// Data type
        /// </summary>
        public string DbType { get; set; }
        /// <summary>
        /// Parameter Direction
        /// </summary>
        public ParameterDirection Direction { get; set; } = ParameterDirection.Input;
        /// <summary>
        /// Data Size
        /// </summary>
        public int Size { get; set; }
    }
}
