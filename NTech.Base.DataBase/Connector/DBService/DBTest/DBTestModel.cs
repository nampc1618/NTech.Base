using NTech.Base.Commons.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Base.DataBase.Connector.DBService.DBTest
{
    public class DBTestModel : BaseModel
    {
        public DBTestModel() : base()
        {
            List = new Dictionary<string, object>();
        }
        public int? Seq { get; set; }
        public string Title { get; set; }
        public Dictionary<string, object> List { get; set; }
    }
}
