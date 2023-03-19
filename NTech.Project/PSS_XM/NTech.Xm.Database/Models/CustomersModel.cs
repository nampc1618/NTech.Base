using NTech.Base.Commons.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Xm.Database.Models
{
    public class CustomersModel : BaseModel
    {
        public Guid Guid { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
    }
}
