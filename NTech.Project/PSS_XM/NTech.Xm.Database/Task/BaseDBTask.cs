using NTech.Base.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Xm.Database.Task
{
    public class BaseDBTask
    {
        public BaseDBTask()
        {

        }
        public virtual DBResultModel Select(DBRequestModel requestModel)
        {
            return null;
        }
        public virtual DBResultModel Insert(DBRequestModel requestModel)
        {
            return null;
        }
        public virtual DBResultModel Update(DBRequestModel requestModel)
        {
            return null;
        }
        public virtual DBResultModel MultiUpdate(DBRequestModel requestModel)
        {
            return null;
        }
    }
}
