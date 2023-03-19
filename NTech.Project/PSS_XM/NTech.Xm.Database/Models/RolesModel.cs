using NTech.Base.Commons.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Xm.Database.Models
{
    public class RolesModel : BaseModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string Username { get; set; }
        public string Passcode { get; set; }
    }
}
