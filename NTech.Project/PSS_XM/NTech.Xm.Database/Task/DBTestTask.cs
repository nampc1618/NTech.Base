using NTech.Base.DataBase.Connector;
using NTech.Base.DataBase.Connector.DBService.DBTest;
using NTech.Base.DataBase.Models;
using NTech.Xm.Database.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Xm.Database.Task
{
    public class DBTestTask : BaseDBTask
    {
        public DBTestTask() : base()
        {

        }

        #region Select Cmds
        public DBResultModel SelectCustomers(DBRequestModel requestModel)
        {
            StringBuilder query = new StringBuilder();
            query.Append("Select * from dbo.Customers");

            requestModel.IsStoredProcedure = false;
            requestModel.Query = query.ToString();

            var dbService = DBConnector.Create(requestModel.DBName);
            var result = dbService?.ExecuteDataSet(requestModel);

            return result;
        }
        public DBResultModel SelectArrangeProducts(DBRequestModel requestModel)
        {
            StringBuilder query = new StringBuilder();
            query.Append("Select * from dbo.ArrangeProducts");

            requestModel.IsStoredProcedure = false;
            requestModel.Query = query.ToString();

            var dbService = DBConnector.Create(requestModel.DBName);
            var result = dbService?.ExecuteDataSet(requestModel);
            return result;
        }
        public DBResultModel SelectBagCover(DBRequestModel requestModel)
        {
            StringBuilder query = new StringBuilder();
            query.Append("Select * from dbo.BagCover");

            requestModel.IsStoredProcedure = false;
            requestModel.Query = query.ToString();

            var dbService = DBConnector.Create(requestModel.DBName);
            var result = dbService?.ExecuteDataSet(requestModel);
            return result;
        }
        public DBResultModel SelectCementBrand(DBRequestModel requestModel)
        {
            StringBuilder query = new StringBuilder();
            query.Append("Select * from dbo.CementBrand");

            requestModel.IsStoredProcedure = false;
            requestModel.Query = query.ToString();

            var dbService = DBConnector.Create(requestModel.DBName);
            var result = dbService?.ExecuteDataSet(requestModel);
            return result;
        }
        public DBResultModel SelectProducts(DBRequestModel requestModel)
        {
            StringBuilder query = new StringBuilder();
            query.Append("Select * from dbo.Products");

            requestModel.IsStoredProcedure = false;
            requestModel.Query = query.ToString();

            var dbService = DBConnector.Create(requestModel.DBName);
            var result = dbService?.ExecuteDataSet(requestModel);
            return result;
        }
        public DBResultModel SelectRoles(DBRequestModel requestModel)
        {
            StringBuilder query = new StringBuilder();
            query.Append("Select * from dbo.Roles");

            requestModel.IsStoredProcedure = false;
            requestModel.Query = query.ToString();

            var dbService = DBConnector.Create(requestModel.DBName);
            var result = dbService?.ExecuteDataSet(requestModel);
            return result;
        }
        public DBResultModel SelectMessagesDetailByDate(DBRequestModel requestModel)
        {
            StringBuilder query = new StringBuilder();
            query.Append("Select * from dbo.MessagesDetail where ManufacturingDate = '" + requestModel.CustomInputData[0] + "' ");

            requestModel.IsStoredProcedure = false;
            requestModel.Query = query.ToString();

            var dbService = DBConnector.Create(requestModel.DBName);
            var result = dbService?.ExecuteDataSet(requestModel);
            return result;
        }
        public DBResultModel SelectMessagesDetailAllPrinting(DBRequestModel requestModel)
        {
            StringBuilder query = new StringBuilder();
            query.Append("Select * from dbo.MessagesDetail where MessageState = N'" + requestModel.CustomInputData[0] + "' ");

            requestModel.IsStoredProcedure = false;
            requestModel.Query = query.ToString();

            var dbService = DBConnector.Create(requestModel.DBName);
            var result = dbService?.ExecuteDataSet(requestModel);
            return result;
        }
        public DBResultModel SelectMessageDetailByGuidAndDateNow(DBRequestModel requestModel)
        {
            var data = requestModel.CustomInputData;
            string date = (string)data[0];
            Guid guid= (Guid)data[1];
            
            StringBuilder query = new StringBuilder();
            query.Append($"Select * from dbo.MessagesDetail where ManufacturingDate = '{date}' and Guid = '{guid}'");

            requestModel.IsStoredProcedure = false;
            requestModel.Query = query.ToString();

            var dbService = DBConnector.Create(requestModel.DBName);
            var result = dbService?.ExecuteDataSet(requestModel);
            return result;
        }
        public DBResultModel SelectMessagesDetailBetween2Date(DBRequestModel requestModel)
        {
            var inputdata = requestModel.InputData as DBTestModel;
            List<DBParameterModel> paramList = new List<DBParameterModel>();
            foreach (var p in inputdata.List)
            {
                paramList.Add(new DBParameterModel() { ParameterName = p.Key, Value = p.Value, DbType = SqlDbType.VarChar.ToString() });
            }
            StringBuilder query = new StringBuilder();
            query.Append("Select * from dbo.MessagesDetail where ManufacturingDate BETWEEN @fromdt AND @todt");

            requestModel.IsStoredProcedure = false;
            requestModel.Query = query.ToString();
            requestModel.Parameters = paramList;

            var dbService = DBConnector.Create(requestModel.DBName);
            var result = dbService?.ExecuteDataSet(requestModel);
            return result;

        }

        public DBResultModel SelectGuidMsgNewest(DBRequestModel requestModel)
        {
        
            StringBuilder query = new StringBuilder();
            query.Append($"Select *from dbo.GuidMsgNewest");

            requestModel.IsStoredProcedure = false;
            requestModel.Query = query.ToString();

            var dbService = DBConnector.Create(requestModel.DBName);
            var result = dbService?.ExecuteDataSet(requestModel);
            return result;
        }

        #endregion

        #region Insert Cmds
        public DBResultModel InsertTest(DBRequestModel requestModel)
        {
            var inputData = requestModel.InputData as Test;

            List<DBParameterModel> paramList = new List<DBParameterModel>();
            paramList.Add(new DBParameterModel() { ParameterName = "@MyName", Value = inputData.MyName, DbType = SqlDbType.NVarChar.ToString() });
            paramList.Add(new DBParameterModel() { ParameterName = "@MyCode", Value = inputData.MyCode, DbType = SqlDbType.NVarChar.ToString() });

            StringBuilder query = new StringBuilder();
            query.Append("Insert into Test (MyName, MyCode) values (@MyName, @MyCode)");

            requestModel.Query = query.ToString();
            requestModel.Parameters = paramList;

            var dbService = DBConnector.Create(requestModel.DBName);
            return dbService?.ExecuteDataSet(requestModel);
        }

        public DBResultModel InsertMessageDetail(DBRequestModel requestModel)
        {
            var inputdata = requestModel.InputData as MessagesDetailModel;
            List<DBParameterModel> paramlist = new List<DBParameterModel>();
            paramlist.Add(new DBParameterModel() { ParameterName = "@Guid", Value = inputdata.Guid, DbType = SqlDbType.UniqueIdentifier.ToString() });                     //1
            paramlist.Add(new DBParameterModel() { ParameterName = "@ManufacturingDate", Value = inputdata.ManufacturingDate, DbType = SqlDbType.VarChar.ToString() });    //2
            paramlist.Add(new DBParameterModel() { ParameterName = "@ManufacturingTime", Value = inputdata.ManufacturingTime, DbType = SqlDbType.VarChar.ToString() });    //3
            paramlist.Add(new DBParameterModel() { ParameterName = "@ManufacturingShift", Value = inputdata.ManufacturingShift, DbType = SqlDbType.VarChar.ToString() });  //4
            paramlist.Add(new DBParameterModel() { ParameterName = "@CouponCode", Value = inputdata.CouponCode, DbType = SqlDbType.VarChar.ToString() });                  //5
            paramlist.Add(new DBParameterModel() { ParameterName = "@LicensePlate", Value = inputdata.LicensePlate, DbType = SqlDbType.VarChar.ToString() });              //6
            paramlist.Add(new DBParameterModel() { ParameterName = "@CustomerCode", Value = inputdata.CustomerCode, DbType = SqlDbType.VarChar.ToString() });              //7
            paramlist.Add(new DBParameterModel() { ParameterName = "@CustomerName", Value = inputdata.CustomerName, DbType = SqlDbType.NVarChar.ToString() });             //8
            paramlist.Add(new DBParameterModel() { ParameterName = "@CementType", Value = inputdata.CementType, DbType = SqlDbType.VarChar.ToString() });                  //9
            paramlist.Add(new DBParameterModel() { ParameterName = "@WeightTon", Value = inputdata.WeightTon, DbType = SqlDbType.Float.ToString() });                        //10
            paramlist.Add(new DBParameterModel() { ParameterName = "@NumberBags", Value = inputdata.NumberBags, DbType = SqlDbType.Int.ToString() });                      //11
            paramlist.Add(new DBParameterModel() { ParameterName = "@NumberBagsPrinted", Value = inputdata.NumberBagsPrinted, DbType = SqlDbType.Int.ToString() });        //12
            paramlist.Add(new DBParameterModel() { ParameterName = "@BagCoverType", Value = inputdata.BagCoverType, DbType = SqlDbType.VarChar.ToString() });              //13
            paramlist.Add(new DBParameterModel() { ParameterName = "@Brand", Value = inputdata.Brand, DbType = SqlDbType.NVarChar.ToString() });                           //14
            paramlist.Add(new DBParameterModel() { ParameterName = "@ArrangeType", Value = inputdata.ArrangeType, DbType = SqlDbType.NVarChar.ToString() });               //15
            paramlist.Add(new DBParameterModel() { ParameterName = "@Line", Value = inputdata.Line, DbType = SqlDbType.NVarChar.ToString() });                              //16
            paramlist.Add(new DBParameterModel() { ParameterName = "@Printer", Value = inputdata.Printer, DbType = SqlDbType.NVarChar.ToString() });                        //17
            paramlist.Add(new DBParameterModel() { ParameterName = "@Trough", Value = inputdata.Trough, DbType = SqlDbType.NVarChar.ToString() });                          //18
            paramlist.Add(new DBParameterModel() { ParameterName = "@MessageState", Value = inputdata.MessageState, DbType = SqlDbType.NVarChar.ToString() });             //19

            StringBuilder query = new StringBuilder();
            query.Append("Insert into dbo.MessagesDetail (Guid, ManufacturingDate, ManufacturingTime, ManufacturingShift, " +
                                                         "CouponCode, LicensePlate, CustomerCode, CustomerName, CementType, " +
                                                         "WeightTon, NumberBags, NumberBagsPrinted, BagCoverType, Brand, " +
                                                         "ArrangeType, Line, Printer, Trough, MessageState) values " +
                                                         "(@Guid, @ManufacturingDate, @ManufacturingTime, @ManufacturingShift, " +
                                                         "@CouponCode, @LicensePlate, @CustomerCode, @CustomerName, @CementType, " +
                                                         "@WeightTon, @NumberBags, @NumberBagsPrinted, @BagCoverType, @Brand, " +
                                                         "@ArrangeType, @Line, @Printer, @Trough, @MessageState)");
            requestModel.Query = query.ToString();
            requestModel.Parameters = paramlist;

            var dbService = DBConnector.Create(requestModel.DBName);
            return dbService?.ExecuteDataSet(requestModel);
        }

        public DBResultModel InsertCustomer(DBRequestModel requestModel)
        {
            var inputdata = requestModel.InputData as CustomersModel;
            List<DBParameterModel> paramList = new List<DBParameterModel>();
            //paramList.Add(new DBParameterModel() { ParameterName = "@Guid", Value = inputdata.Guid, DbType = SqlDbType.UniqueIdentifier.ToString() });
            paramList.Add(new DBParameterModel() { ParameterName = "@CustomerCode", Value = inputdata.CustomerCode, DbType = SqlDbType.VarChar.ToString() });
            paramList.Add(new DBParameterModel() { ParameterName = "@CustomerName", Value = inputdata.CustomerName, DbType = SqlDbType.NVarChar.ToString() });

            StringBuilder query = new StringBuilder();
            query.Append("Insert into dbo.Customers (CustomerCode, CustomerName) Values (@CustomerCode, @CustomerName)");

            requestModel.Query = query.ToString();
            requestModel.Parameters = paramList;

            var dbService = DBConnector.Create(requestModel.DBName);
            return dbService?.ExecuteDataSet(requestModel);
        }
        public DBResultModel InsertArrangeProduct(DBRequestModel requestModel)
        {
            var inputdata = requestModel.InputData as ArrangeProductsModel;
            List<DBParameterModel> paramList = new List<DBParameterModel>();
            paramList.Add(new DBParameterModel() { ParameterName = "@Id", Value = inputdata.Id, DbType = SqlDbType.Int.ToString() });
            paramList.Add(new DBParameterModel() { ParameterName = "@ArrangeType", Value = inputdata.ArrangeType, DbType = SqlDbType.NVarChar.ToString() });

            StringBuilder query = new StringBuilder();
            query.Append("Insert into dbo.ArrangeProducts (Id, ArrangeType) Values (@Id, @ArrangeType)");

            requestModel.Query = query.ToString();
            requestModel.Parameters = paramList;

            var dbService = DBConnector.Create(requestModel.DBName);
            return dbService?.ExecuteDataSet(requestModel);
        }
        public DBResultModel InsertBagCover(DBRequestModel requestModel)
        {
            var inputdata = requestModel.InputData as BagCoverModel;
            List<DBParameterModel> paramList = new List<DBParameterModel>();
            paramList.Add(new DBParameterModel() { ParameterName = "@Id", Value = inputdata.Id, DbType = SqlDbType.Int.ToString() });
            paramList.Add(new DBParameterModel() { ParameterName = "@BagCoverType", Value = inputdata.BagCoverType, DbType = SqlDbType.VarChar.ToString() });

            StringBuilder query = new StringBuilder();
            query.Append("Insert into dbo.BagCover (Id, BagCoverType) Values (@Id, @BagCoverType)");

            requestModel.Query = query.ToString();
            requestModel.Parameters = paramList;

            var dbService = DBConnector.Create(requestModel.DBName);
            return dbService?.ExecuteDataSet(requestModel);
        }
        public DBResultModel InsertCementBrand(DBRequestModel requestModel)
        {
            var inputdata = requestModel.InputData as CementBrandModel;
            List<DBParameterModel> paramList = new List<DBParameterModel>();
            paramList.Add(new DBParameterModel() { ParameterName = "@Id", Value = inputdata.Id, DbType = SqlDbType.Int.ToString() });
            paramList.Add(new DBParameterModel() { ParameterName = "@Brand", Value = inputdata.Brand, DbType = SqlDbType.NVarChar.ToString() });

            StringBuilder query = new StringBuilder();
            query.Append("Insert into dbo.CementBrand (Id, Brand) Values (@Id, @Brand)");

            requestModel.Query = query.ToString();
            requestModel.Parameters = paramList;

            var dbService = DBConnector.Create(requestModel.DBName);
            return dbService?.ExecuteDataSet(requestModel);
        }
        public DBResultModel InsertProduct(DBRequestModel requestModel)
        {
            var inputdata = requestModel.InputData as ProductsModel;
            List<DBParameterModel> paramList = new List<DBParameterModel>();
            paramList.Add(new DBParameterModel() { ParameterName = "@Id", Value = inputdata.Id, DbType = SqlDbType.Int.ToString() });
            paramList.Add(new DBParameterModel() { ParameterName = "@ProductCode", Value = inputdata.ProductCode, DbType = SqlDbType.VarChar.ToString() });
            paramList.Add(new DBParameterModel() { ParameterName = "@ProductName", Value = inputdata.ProductName, DbType = SqlDbType.NVarChar.ToString() });

            StringBuilder query = new StringBuilder();
            query.Append("Insert into dbo.Products (Id, ProductCode, ProductName) Values (@Id, @ProductCode, @ProductName)");

            requestModel.Query = query.ToString();
            requestModel.Parameters = paramList;

            var dbService = DBConnector.Create(requestModel.DBName);
            return dbService?.ExecuteDataSet(requestModel);
        }
        #endregion

        #region Update Cmds
        public DBResultModel UpdateMessageDetail(DBRequestModel requestModel)
        {
            var inputdata = requestModel.InputData as MessagesDetailModel;
            List<DBParameterModel> paramlist = new List<DBParameterModel>();

            paramlist.Add(new DBParameterModel() { ParameterName = "@Guid", Value = inputdata.Guid, DbType = SqlDbType.UniqueIdentifier.ToString() });
            paramlist.Add(new DBParameterModel() { ParameterName = "@Line", Value = inputdata.Line, DbType = SqlDbType.NVarChar.ToString() });
            paramlist.Add(new DBParameterModel() { ParameterName = "@Printer", Value = inputdata.Printer, DbType = SqlDbType.NVarChar.ToString() });
            paramlist.Add(new DBParameterModel() { ParameterName = "@Trough", Value = inputdata.Trough, DbType = SqlDbType.NVarChar.ToString() });
            paramlist.Add(new DBParameterModel() { ParameterName = "@MessageState", Value = inputdata.MessageState, DbType = SqlDbType.NVarChar.ToString() });
            StringBuilder query = new StringBuilder();
            query.Append("Update dbo.MessagesDetail set Line = @Line, Printer = @Printer, Trough = @Trough, MessageState = @MessageState where Guid = @Guid");
            requestModel.Query = query.ToString();
            requestModel.Parameters = paramlist;

            var dbService = DBConnector.Create(requestModel.DBName);
            return dbService?.ExecuteDataSet(requestModel);
        }

        public DBResultModel UpdateNumberBagPrintedAndMessageState(DBRequestModel requestModel)
        {
            var inputdata = requestModel.InputData as MessagesDetailModel;
            List<DBParameterModel> paramlist = new List<DBParameterModel>();

            paramlist.Add(new DBParameterModel() { ParameterName = "@Guid", Value = inputdata.Guid, DbType = SqlDbType.UniqueIdentifier.ToString() });
            paramlist.Add(new DBParameterModel() { ParameterName = "@NumberBagsPrinted", Value = inputdata.NumberBagsPrinted, DbType = SqlDbType.Int.ToString() });
            paramlist.Add(new DBParameterModel() { ParameterName = "@MessageState", Value = inputdata.MessageState, DbType = SqlDbType.NVarChar.ToString() });

            StringBuilder query = new StringBuilder();
            query.Append("Update dbo.MessagesDetail set NumberBagsPrinted = @NumberBagsPrinted, MessageState = @MessageState where Guid = @Guid");
            requestModel.Query = query.ToString();
            requestModel.Parameters = paramlist;

            var dbService = DBConnector.Create(requestModel.DBName);
            return dbService?.ExecuteDataSet(requestModel);
        }
        public DBResultModel UpdateNoteForMessageDetail(DBRequestModel requestModel)
        {
            var inputdata = requestModel.InputData as MessagesDetailModel;
            List<DBParameterModel> paramlist = new List<DBParameterModel>();

            paramlist.Add(new DBParameterModel() { ParameterName = "@Guid", Value = inputdata.Guid, DbType = SqlDbType.UniqueIdentifier.ToString() });
            paramlist.Add(new DBParameterModel() { ParameterName = "@NumberBagsPrinted", Value = inputdata.NumberBagsPrinted, DbType = SqlDbType.Int.ToString() });
            paramlist.Add(new DBParameterModel() { ParameterName = "@MessageState", Value = inputdata.MessageState, DbType = SqlDbType.NVarChar.ToString() });
            paramlist.Add(new DBParameterModel() { ParameterName = "@Note", Value = inputdata.Note, DbType = SqlDbType.NVarChar.ToString() });

            StringBuilder query = new StringBuilder();
            query.Append("Update dbo.MessagesDetail set NumberBagsPrinted = @NumberBagsPrinted, MessageState = @MessageState, Note=@Note where Guid = @Guid");
            requestModel.Query = query.ToString();
            requestModel.Parameters = paramlist;

            var dbService = DBConnector.Create(requestModel.DBName);
            return dbService?.ExecuteDataSet(requestModel);
        }

        public DBResultModel UpdateCustomer(DBRequestModel requestModel)
        {
            var inputdata = requestModel.InputData as CustomersModel;
            List<DBParameterModel> paramList = new List<DBParameterModel>();
            paramList.Add(new DBParameterModel() { ParameterName = "@Guid", Value = inputdata.Guid, DbType = SqlDbType.UniqueIdentifier.ToString() });
            paramList.Add(new DBParameterModel() { ParameterName = "@CustomerCode", Value = inputdata.CustomerCode, DbType = SqlDbType.VarChar.ToString() });
            paramList.Add(new DBParameterModel() { ParameterName = "@CustomerName", Value = inputdata.CustomerName, DbType = SqlDbType.NVarChar.ToString() });

            StringBuilder query = new StringBuilder();
            query.Append("Update dbo.Customers Set CustomerCode = @CustomerCode, CustomerName = @CustomerName Where Guid = @Guid");
            requestModel.Query = query.ToString();
            requestModel.Parameters = paramList;

            var dbService = DBConnector.Create(requestModel.DBName);
            return dbService?.ExecuteDataSet(requestModel);
        }
        public DBResultModel UpdateArrangeProduct(DBRequestModel requestModel)
        {
            var inputdata = requestModel.InputData as ArrangeProductsModel;
            List<DBParameterModel> paramList = new List<DBParameterModel>();
            paramList.Add(new DBParameterModel() { ParameterName = "@Guid", Value = inputdata.Guid, DbType = SqlDbType.UniqueIdentifier.ToString() });
            paramList.Add(new DBParameterModel() { ParameterName = "@Id", Value = inputdata.Id, DbType = SqlDbType.Int.ToString() });
            paramList.Add(new DBParameterModel() { ParameterName = "@ArrangeType", Value = inputdata.ArrangeType, DbType = SqlDbType.NVarChar.ToString() });

            StringBuilder query = new StringBuilder();
            query.Append("Update dbo.ArrangeProducts Set Id = @Id, ArrangeType = @ArrangeType Where Guid = @Guid");
            requestModel.Query = query.ToString();
            requestModel.Parameters = paramList;

            var dbService = DBConnector.Create(requestModel.DBName);
            return dbService?.ExecuteDataSet(requestModel);
        }
        public DBResultModel UpdateBagCover(DBRequestModel requestModel)
        {
            var inputdata = requestModel.InputData as BagCoverModel;
            List<DBParameterModel> paramList = new List<DBParameterModel>();
            paramList.Add(new DBParameterModel() { ParameterName = "@Guid", Value = inputdata.Guid, DbType = SqlDbType.UniqueIdentifier.ToString() });
            paramList.Add(new DBParameterModel() { ParameterName = "@Id", Value = inputdata.Id, DbType = SqlDbType.Int.ToString() });
            paramList.Add(new DBParameterModel() { ParameterName = "@BagCoverType", Value = inputdata.BagCoverType, DbType = SqlDbType.VarChar.ToString() });

            StringBuilder query = new StringBuilder();
            query.Append("Update dbo.BagCover Set Id = @Id, BagCoverType = @BagCoverType Where Guid = @Guid");
            requestModel.Query = query.ToString();
            requestModel.Parameters = paramList;

            var dbService = DBConnector.Create(requestModel.DBName);
            return dbService?.ExecuteDataSet(requestModel);
        }
        public DBResultModel UpdateCementBrand(DBRequestModel requestModel)
        {
            var inputdata = requestModel.InputData as CementBrandModel;
            List<DBParameterModel> paramList = new List<DBParameterModel>();
            paramList.Add(new DBParameterModel() { ParameterName = "@Guid", Value = inputdata.Guid, DbType = SqlDbType.UniqueIdentifier.ToString() });
            paramList.Add(new DBParameterModel() { ParameterName = "@Id", Value = inputdata.Id, DbType = SqlDbType.Int.ToString() });
            paramList.Add(new DBParameterModel() { ParameterName = "@Brand", Value = inputdata.Brand, DbType = SqlDbType.NVarChar.ToString() });

            StringBuilder query = new StringBuilder();
            query.Append("Update dbo.CementBrand Set Id = @Id, Brand = @Brand Where Guid = @Guid");
            requestModel.Query = query.ToString();
            requestModel.Parameters = paramList;

            var dbService = DBConnector.Create(requestModel.DBName);
            return dbService?.ExecuteDataSet(requestModel);
        }
        public DBResultModel UpdateProduct(DBRequestModel requestModel)
        {
            var inputdata = requestModel.InputData as ProductsModel;
            List<DBParameterModel> paramList = new List<DBParameterModel>();
            paramList.Add(new DBParameterModel() { ParameterName = "@Guid", Value = inputdata.Guid, DbType = SqlDbType.UniqueIdentifier.ToString() });
            paramList.Add(new DBParameterModel() { ParameterName = "@Id", Value = inputdata.Id, DbType = SqlDbType.Int.ToString() });
            paramList.Add(new DBParameterModel() { ParameterName = "@ProductCode", Value = inputdata.ProductCode, DbType = SqlDbType.VarChar.ToString() });
            paramList.Add(new DBParameterModel() { ParameterName = "@ProductName", Value = inputdata.ProductName, DbType = SqlDbType.NVarChar.ToString() });

            StringBuilder query = new StringBuilder();
            query.Append("Update dbo.Products Set Id = @Id, ProductCode = @ProductCode, ProductName = @ProductName Where Guid = @Guid");
            requestModel.Query = query.ToString();
            requestModel.Parameters = paramList;

            var dbService = DBConnector.Create(requestModel.DBName);
            return dbService?.ExecuteDataSet(requestModel);
        }
        public DBResultModel UpdateRole(DBRequestModel requestModel)
        {
            var inputdata = requestModel.InputData as RolesModel;
            List<DBParameterModel> paramList = new List<DBParameterModel>();
            paramList.Add(new DBParameterModel() { ParameterName = "@Guid", Value = inputdata.Guid, DbType = SqlDbType.UniqueIdentifier.ToString() });
            paramList.Add(new DBParameterModel() { ParameterName = "@Id", Value = inputdata.Id, DbType = SqlDbType.Int.ToString() });
            paramList.Add(new DBParameterModel() { ParameterName = "@Username", Value = inputdata.Username, DbType = SqlDbType.VarChar.ToString() });
            paramList.Add(new DBParameterModel() { ParameterName = "@Passcode", Value = inputdata.Passcode, DbType = SqlDbType.VarChar.ToString() });

            StringBuilder query = new StringBuilder();
            query.Append("Update dbo.Roles Set Id = @Id, Username = @Username, Passcode = @Passcode Where Guid = @Guid");
            requestModel.Query = query.ToString();
            requestModel.Parameters = paramList;

            var dbService = DBConnector.Create(requestModel.DBName);
            return dbService?.ExecuteDataSet(requestModel);
        }
        public DBResultModel UpdateGuidMsgNewest(DBRequestModel requestModel)
        {
            try
            {
                var inputdata = requestModel.CustomInputData;
                string colName = (string)inputdata[0];
                Guid guid = (Guid)inputdata[1];

                StringBuilder query = new StringBuilder();
                query.Append($"Update dbo.GuidMsgNewest Set {colName} = '{guid}'");
                requestModel.Query = query.ToString();

                var dbService = DBConnector.Create(requestModel.DBName);
                return dbService?.ExecuteDataSet(requestModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        #endregion

        #region -----DELETE-----
        public DBResultModel DeleteCustomer(DBRequestModel requestModel)
        {
            var inputdata = requestModel.InputData as CustomersModel;
            if (inputdata == null)
                return null;
            List<DBParameterModel> paramList = new List<DBParameterModel>();
            paramList.Add(new DBParameterModel() { ParameterName = "@Guid", Value = inputdata.Guid, DbType = SqlDbType.UniqueIdentifier.ToString() });

            StringBuilder query = new StringBuilder();
            query.Append("Delete from dbo.Customers Where Guid = @Guid");
            requestModel.Query = query.ToString();
            requestModel.Parameters = paramList;

            var dbService = DBConnector.Create(requestModel.DBName);
            return dbService?.ExecuteDataSet(requestModel);
        }
        public DBResultModel DeleteArrangeProduct(DBRequestModel requestModel)
        {
            var inputdata = requestModel.InputData as ArrangeProductsModel;
            if (inputdata == null)
                return null;
            List<DBParameterModel> paramList = new List<DBParameterModel>();
            paramList.Add(new DBParameterModel() { ParameterName = "@Guid", Value = inputdata.Guid, DbType = SqlDbType.UniqueIdentifier.ToString() });

            StringBuilder query = new StringBuilder();
            query.Append("Delete from dbo.ArrangeProducts Where Guid = @Guid");
            requestModel.Query = query.ToString();
            requestModel.Parameters = paramList;

            var dbService = DBConnector.Create(requestModel.DBName);
            return dbService?.ExecuteDataSet(requestModel);
        }
        public DBResultModel DeleteBagCover(DBRequestModel requestModel)
        {
            var inputdata = requestModel.InputData as BagCoverModel;
            if (inputdata == null)
                return null;
            List<DBParameterModel> paramList = new List<DBParameterModel>();
            paramList.Add(new DBParameterModel() { ParameterName = "@Guid", Value = inputdata.Guid, DbType = SqlDbType.UniqueIdentifier.ToString() });

            StringBuilder query = new StringBuilder();
            query.Append("Delete from dbo.BagCover Where Guid = @Guid");
            requestModel.Query = query.ToString();
            requestModel.Parameters = paramList;

            var dbService = DBConnector.Create(requestModel.DBName);
            return dbService?.ExecuteDataSet(requestModel);
        }
        public DBResultModel DeleteCementBrand(DBRequestModel requestModel)
        {
            var inputdata = requestModel.InputData as CementBrandModel;
            if (inputdata == null)
                return null;
            List<DBParameterModel> paramList = new List<DBParameterModel>();
            paramList.Add(new DBParameterModel() { ParameterName = "@Guid", Value = inputdata.Guid, DbType = SqlDbType.UniqueIdentifier.ToString() });

            StringBuilder query = new StringBuilder();
            query.Append("Delete from dbo.CementBrand Where Guid = @Guid");
            requestModel.Query = query.ToString();
            requestModel.Parameters = paramList;

            var dbService = DBConnector.Create(requestModel.DBName);
            return dbService?.ExecuteDataSet(requestModel);
        }
        public DBResultModel DeleteProduct(DBRequestModel requestModel)
        {
            var inputdata = requestModel.InputData as ProductsModel;
            if (inputdata == null)
                return null;
            List<DBParameterModel> paramList = new List<DBParameterModel>();
            paramList.Add(new DBParameterModel() { ParameterName = "@Guid", Value = inputdata.Guid, DbType = SqlDbType.UniqueIdentifier.ToString() });

            StringBuilder query = new StringBuilder();
            query.Append("Delete from dbo.Products Where Guid = @Guid");
            requestModel.Query = query.ToString();
            requestModel.Parameters = paramList;

            var dbService = DBConnector.Create(requestModel.DBName);
            return dbService?.ExecuteDataSet(requestModel);
        }
        public DBResultModel DeleteMessageDetail(DBRequestModel requestModel)
        {
            var inputdata = requestModel.InputData as MessagesDetailModel;
            if (inputdata == null)
                return null;
            List<DBParameterModel> paramList = new List<DBParameterModel>();
            paramList.Add(new DBParameterModel() { ParameterName = "@Guid", Value = inputdata.Guid, DbType = SqlDbType.UniqueIdentifier.ToString() });

            StringBuilder query = new StringBuilder();
            query.Append("Delete from dbo.MessagesDetail Where Guid = @Guid");
            requestModel.Query = query.ToString();
            requestModel.Parameters = paramList;

            var dbService = DBConnector.Create(requestModel.DBName);
            return dbService?.ExecuteDataSet(requestModel);
        }
        #endregion
    }
}
