using NTech.Base.DataBase.Connector.DBService.DBTest;
using NTech.Base.DataBase.Models;
using NTech.Xm.Database.Models;
using NTech.Xm.Database.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Xm.Database
{
    public class TaskManagerDB
    {
        private DBTestTask _task = null;
        
        public TaskManagerDB()
        {
            this._task = new DBTestTask();
        }

        #region Select Cmd
        public DBResultModel SelectCustomers(string dbName)
        {
            DBResultModel result = _task.SelectCustomers(new DBRequestModel() { DBName = dbName});
            return result;
        }
        public DBResultModel SelectArrangeProducts(string dbName)
        {
            DBResultModel result = _task.SelectArrangeProducts(new DBRequestModel() { DBName = dbName });
            return result;
        }
        public DBResultModel SelectBagCover(string dbName)
        {
            DBResultModel result = _task.SelectBagCover(new DBRequestModel() { DBName = dbName});
            return result;
        }
        public DBResultModel SelectCementBrand(string dbName)
        {
            DBResultModel result = _task.SelectCementBrand(new DBRequestModel() { DBName = dbName });
            return result;
        }
        public DBResultModel SelectProducts(string dbName)
        {
            DBResultModel result = _task.SelectProducts(new DBRequestModel() { DBName = dbName });
            return result;
        }
        public DBResultModel SelectRoles(string dbName)
        {
            DBResultModel result = _task.SelectRoles(new DBRequestModel() { DBName = dbName });
            return result;
        }
        public DBResultModel SelectMessagesDetailByDate(string dbName, string[] day)
        {
            DBResultModel result = _task.SelectMessagesDetailByDate(new DBRequestModel() { DBName = dbName,  CustomInputData = day});
            return result;
        }
        public DBResultModel SelectMessageDetailByGuidAndDateNow(string dbName, params object[] data)
        {
            DBResultModel result = _task.SelectMessageDetailByGuidAndDateNow(new DBRequestModel() { DBName = dbName, CustomInputData = data });
            return result;
        }
        public DBResultModel SelectMessagesDetailBetween2Date(string dbName, DateTime? fromdt, DateTime? todt)
        {
            var para = new DBTestModel();
            para.List.Add("@fromdt", fromdt?.ToString("dd-MM-yyyy"));
            para.List.Add("@todt", todt?.ToString("dd-MM-yyyy"));

            DBResultModel result = _task.SelectMessagesDetailBetween2Date(new DBRequestModel() { DBName = dbName, InputData = para });
            return result;
        }
        public DBResultModel SelectGuidMsgNewest(string dbName)
        {
            DBResultModel result = _task.SelectGuidMsgNewest(new DBRequestModel() { DBName = dbName});
            return result;
        }
        #endregion

        #region Insert Cmd
        public DBResultModel InsertTest(string dbName, Test test)
        {
            return _task.InsertTest(new DBRequestModel() { DBName = dbName, InputData = test });
        }
        public DBResultModel InsertMessageDetail(string dbName, MessagesDetailModel messagesDetailModel)
        {
            return _task.InsertMessageDetail(new DBRequestModel() { DBName = dbName, InputData = messagesDetailModel });
        }
        public DBResultModel InsertCustomer(string dbName, CustomersModel customersModel)
        {
            return _task.InsertCustomer(new DBRequestModel() { DBName = dbName, InputData = customersModel });
        }
        public DBResultModel InsertArrangeProduct(string dbName, ArrangeProductsModel arrangeProductsModel)
        {
            return _task.InsertArrangeProduct(new DBRequestModel() { DBName = dbName, InputData = arrangeProductsModel });
        }
        public DBResultModel InsertBagCover(string dbName, BagCoverModel bagCoverModel)
        {
            return _task.InsertBagCover(new DBRequestModel() { DBName = dbName, InputData = bagCoverModel });
        }
        public DBResultModel InsertCementBrand(string dbName, CementBrandModel cementBrandModel)
        {
            return _task.InsertCementBrand(new DBRequestModel() { DBName = dbName, InputData = cementBrandModel });
        }
        public DBResultModel InsertProduct(string dbName, ProductsModel productsModel)
        {
            return _task.InsertProduct(new DBRequestModel() { DBName = dbName, InputData = productsModel });
        }
        #endregion

        #region Update Cmd
        public DBResultModel UpdateMessageDetail(string dbName, MessagesDetailModel messagesDetailModel)
        {
            return _task.UpdateMessageDetail(new DBRequestModel() { DBName = dbName, InputData = messagesDetailModel });
        }
        public DBResultModel UpdateNumberBagPrintedAndMessageState(string dbName, MessagesDetailModel messagesDetailModel)
        {
            return _task.UpdateNumberBagPrintedAndMessageState(new DBRequestModel() { DBName = dbName, InputData = messagesDetailModel });
        }
        public DBResultModel UpdateNoteForMessageDetail(string dbName, MessagesDetailModel messagesDetailModel)
        {
            return _task.UpdateNoteForMessageDetail(new DBRequestModel() { DBName = dbName, InputData = messagesDetailModel });
        }
        public DBResultModel UpdateCustomer(string dbName, CustomersModel customersModel)
        {
            return _task.UpdateCustomer(new DBRequestModel() { DBName = dbName, InputData = customersModel });
        }
        public DBResultModel UpdateArrangeProduct(string dbName, ArrangeProductsModel arrangeProductsModel)
        {
            return _task.UpdateArrangeProduct(new DBRequestModel() { DBName = dbName, InputData = arrangeProductsModel });
        }
        public DBResultModel UpdateBagCover(string dbName, BagCoverModel bagCoverModel)
        {
            return _task.UpdateBagCover(new DBRequestModel() { DBName = dbName, InputData = bagCoverModel });
        }
        public DBResultModel UpdateCementBrand(string dbName, CementBrandModel cementBrandModel)
        {
            return _task.UpdateCementBrand(new DBRequestModel() { DBName = dbName, InputData = cementBrandModel });
        }
        public DBResultModel UpdateProduct(string dbName, ProductsModel productsModel)
        {
            return _task.UpdateProduct(new DBRequestModel() { DBName = dbName, InputData = productsModel });
        }
        public DBResultModel UpdateRole(string dbName, RolesModel rolesModel)
        {
            return _task.UpdateRole(new DBRequestModel() { DBName = dbName, InputData = rolesModel });
        }
        public DBResultModel UpdateGuidMsgNewest(string dbName, params object[] data)
        {
            return _task.UpdateGuidMsgNewest(new DBRequestModel() { DBName = dbName, CustomInputData = data });
        }
            #endregion

            #region ------------DELETE-------------
        public DBResultModel DeleteCustomer(string dbName, CustomersModel customersModel)
        {
            return _task.DeleteCustomer(new DBRequestModel() { DBName = dbName, InputData = customersModel });
        }
        public DBResultModel DeleteArrangeProduct(string dbName, ArrangeProductsModel arrangeProductsModel)
        {
            return _task.DeleteArrangeProduct(new DBRequestModel() { DBName = dbName, InputData = arrangeProductsModel });
        }
        public DBResultModel DeleteBagCover(string dbName, BagCoverModel bagCoverModel)
        {
            return _task.DeleteBagCover(new DBRequestModel() { DBName = dbName, InputData = bagCoverModel });
        }
        public DBResultModel DeleteCementBrand(string dbName, CementBrandModel cementBrandModel)
        {
            return _task.DeleteCementBrand(new DBRequestModel() { DBName = dbName, InputData = cementBrandModel });
        }
        public DBResultModel DeleteProduct(string dbName, ProductsModel productsModel)
        {
            return _task.DeleteProduct(new DBRequestModel() { DBName = dbName, InputData = productsModel });
        }
        public DBResultModel DeleteMessageDetail(string dbName, MessagesDetailModel messagesDetailModel)
        {
            return _task.DeleteMessageDetail(new DBRequestModel() { DBName = dbName, InputData = messagesDetailModel });
        }
        #endregion
    }
}
