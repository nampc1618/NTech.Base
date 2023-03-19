using NTech.Base.DataBase.Utils;
using NTech.Xm.Commons.Defines;
using NTech.Xm.Database;
using NTech.Xm.Database.Models;
using NTech.Xm.Gate.Command;
using NTech.Xm.Gate.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace NTech.Xm.Gate.ViewModels
{
    public enum DB_TABLE_LIST
    {
        [Description("Danh sách khách hàng")]
        Customers,
        [Description("Cách xếp hàng")]
        ArrangeProducts,
        [Description("Loại vỏ bao")]
        BagCover,
        [Description("Thương hiệu xi măng")]
        CementBrand,
        [Description("Danh sách sản phẩm")]
        Products,
        [Description("Quản trị")]
        Roles
    }
    public class SettingsViewModel : ViewModelBase
    {
        private readonly TaskManagerDB _taskManagerDB;
        private readonly string DBName = "XmDb";

        #region Variables
        CustomersModel _customerModel;
        ArrangeProductsModel _arrangeProductsModel;
        BagCoverModel _bagCoverModel;
        CementBrandModel _cementBrandModel;
        ProductsModel _productsModel;
        RolesModel _rolesModel;

        List<CustomersModel> _customerModelList;
        List<ArrangeProductsModel> _arrangeProductsModelList;
        List<BagCoverModel> _bagCoverModelList;
        List<CementBrandModel> _cementBrandModelList;
        List<ProductsModel> _productsModelList;
        List<RolesModel> _rolesModelList;

        ObservableCollection<string> _arrangeProductList;
        ObservableCollection<string> _bagCoverList;
        ObservableCollection<string> _cementBrandList;
        ObservableCollection<string> _productList;
        List<string> _shiftList;
        #endregion

        public static SettingsViewModel Instance { get; private set; }
        public SettingsView SettingsView { get; private set; }
        public LoginViewModel LoginViewModel { get; private set; }
        public SettingsViewModel(SettingsView settingsView, LoginViewModel loginViewModel)
        {
            if (Instance is null)
                Instance = this;
            else
                return;

            this._taskManagerDB = new TaskManagerDB();
            this.SettingsView = settingsView;
            this.LoginViewModel = loginViewModel;

            this.Init();

            SelectCustomers();
            SelectArrangeProducts();
            SelectBagCover();
            SelectCementBrand();
            SelectProducts();
            SelectRoles();

            this.SelectionChangedCmd = new SelectionChangedCmd(this);
            this.SaveChangedCmd = new SaveChangedCmd(this);
            this.PreviewMouseRightButtonDownCmd = new PreviewMouseRightButtonDownCmd(this);
            this.CreateNewCmd = new CreateNewCmd(this);
            this.DeleteCmd = new DeleteCmd(this);

            this.LoginViewModel.LoginEvent += LoginViewModel_LoginEvent;
        }

        private void LoginViewModel_LoginEvent(object sender, LOGIN_STATE loginState, ROLE role)
        {
            switch (loginState)
            {
                case LOGIN_STATE.LOGOUT:
                    this.VisibilitySettingsView = Visibility.Collapsed;
                    break;
                case LOGIN_STATE.LOGIN_SUCCESS:
                    if (role == ROLE.ADMIN || role == ROLE.SUPERADMIN)
                        this.VisibilitySettingsView = Visibility.Visible;
                    else
                        this.VisibilitySettingsView = Visibility.Collapsed;
                    break;
                case LOGIN_STATE.LOGIN_FAILED:
                    this.VisibilitySettingsView = Visibility.Collapsed;
                    break;
            }
        }
        private Visibility _visibilitySettingsView = Visibility.Collapsed;
        public Visibility VisibilitySettingsView
        {
            get { return _visibilitySettingsView; }
            set
            {
                Set(ref _visibilitySettingsView, value);
            }
        }

        #region Methods
        void Init()
        {
            this._dbTableList = new List<string>();
            this._dbTableList.Add(CommonDefines.GetEnumDescription(DB_TABLE_LIST.Customers));
            this._dbTableList.Add(CommonDefines.GetEnumDescription(DB_TABLE_LIST.ArrangeProducts));
            this._dbTableList.Add(CommonDefines.GetEnumDescription(DB_TABLE_LIST.BagCover));
            this._dbTableList.Add(CommonDefines.GetEnumDescription(DB_TABLE_LIST.CementBrand));
            this._dbTableList.Add(CommonDefines.GetEnumDescription(DB_TABLE_LIST.Products));
            this._dbTableList.Add(CommonDefines.GetEnumDescription(DB_TABLE_LIST.Roles));

            this._customerModel = new CustomersModel();
            this._arrangeProductsModel = new ArrangeProductsModel();
            this._bagCoverModel = new BagCoverModel();
            this._cementBrandModel = new CementBrandModel();
            this._productsModel = new ProductsModel();
            this._rolesModel = new RolesModel();

            this._customerModelList = new List<CustomersModel>();
            this._arrangeProductsModelList = new List<ArrangeProductsModel>();
            this._bagCoverModelList = new List<BagCoverModel>();
            this._cementBrandModelList = new List<CementBrandModel>();
            this._productsModelList = new List<ProductsModel>();
            this._rolesModelList = new List<RolesModel>();

            this._arrangeProductList = new ObservableCollection<string>();
            this._bagCoverList = new ObservableCollection<string>();
            this._cementBrandList = new ObservableCollection<string>();
            this._productList = new ObservableCollection<string>();
            this._shiftList = new List<string>() { "Ca 1", "Ca 2", "Ca 3" };
        }
        #region Database: Select
        public void SelectCustomers()
        {
            var database = _taskManagerDB.SelectCustomers(DBName);
            var listCustomer = database?.DataSet?.Tables[0].ToList<CustomersModel>();
            if (listCustomer != null)
                CustomerModelList = listCustomer;
        }

        public void SelectArrangeProducts()
        {
            var database = _taskManagerDB.SelectArrangeProducts(DBName);
            var listArrangeProduct = database?.DataSet?.Tables[0].ToList<ArrangeProductsModel>();
            if (listArrangeProduct != null)
            {
                ArrangeProductsModelList = listArrangeProduct;
                this._arrangeProductList.Clear();
                foreach (var item in listArrangeProduct)
                {
                    _arrangeProductList.Add(item.ArrangeType);
                }
            }
        }
        public void SelectBagCover()
        {
            var database = _taskManagerDB.SelectBagCover(DBName);
            var listBagCover = database?.DataSet?.Tables[0].ToList<BagCoverModel>();
            if (listBagCover != null)
            {
                BagCoverModelList = listBagCover;
                this._bagCoverList.Clear();
                foreach (var item in listBagCover)
                {
                    _bagCoverList.Add(item.BagCoverType);
                }
            }
        }
        public void SelectCementBrand()
        {
            var database = _taskManagerDB.SelectCementBrand(DBName);
            var listCementBrand = database?.DataSet?.Tables[0].ToList<CementBrandModel>();
            if (listCementBrand != null)
            {
                CementBrandModelList = listCementBrand;
                this._cementBrandList.Clear();
                foreach (var item in listCementBrand)
                {
                    _cementBrandList.Add(item.Brand);
                }
            }
        }
        public void SelectProducts()
        {
            var database = _taskManagerDB.SelectProducts(DBName);
            var listProduct = database?.DataSet?.Tables[0].ToList<ProductsModel>();
            if (listProduct != null)
            {
                ProductsModelList = listProduct;
                this._productList.Clear();
                foreach (var item in listProduct)
                {
                    _productList.Add(item.ProductName);
                }
            }
        }
        public void SelectRoles()
        {
            var database = _taskManagerDB.SelectRoles(DBName);
            var listRole = database?.DataSet?.Tables[0].ToList<RolesModel>();
            if (listRole != null)
            {
                RolesModelList = listRole;
            }
        }
        #endregion

        #region Database: Insert
        public void InsertCustomer(CustomersModel customersModel)
        {
            _taskManagerDB.InsertCustomer(DBName, customersModel);
        }
        public void InsertArrangeProduct(ArrangeProductsModel arrangeProductsModel)
        {
            _taskManagerDB.InsertArrangeProduct(DBName, arrangeProductsModel);
        }
        public void InsertBagCover(BagCoverModel bagCoverModel)
        {
            _taskManagerDB.InsertBagCover(DBName, bagCoverModel);
        }
        public void InsertCementBrand(CementBrandModel cementBrandModel)
        {
            _taskManagerDB.InsertCementBrand(DBName, cementBrandModel);
        }
        public void InsertProduct(ProductsModel productsModel)
        {
            _taskManagerDB.InsertProduct(DBName, productsModel);
        }
        #endregion

        #region Database: Update
        public void UpdateCustomer(CustomersModel customersModel)
        {
            _taskManagerDB.UpdateCustomer(DBName, customersModel);
        }
        public void UpdateArrangeProduct(ArrangeProductsModel arrangeProductsModel)
        {
            _taskManagerDB.UpdateArrangeProduct(DBName, arrangeProductsModel);
        }
        public void UpdateBagCover(BagCoverModel bagCoverModel)
        {
            _taskManagerDB.UpdateBagCover(DBName, bagCoverModel);
        }
        public void UpdateCementBrand(CementBrandModel cementBrandModel)
        {
            _taskManagerDB.UpdateCementBrand(DBName, cementBrandModel);
        }
        public void UpdateProduct(ProductsModel productsModel)
        {
            _taskManagerDB.UpdateProduct(DBName, productsModel);
        }
        public void UpdateRole(RolesModel rolesModel)
        {
            _taskManagerDB.UpdateRole(DBName, rolesModel);
        }
        #endregion

        #region Database : Delete
        public void DeleteCustomer(CustomersModel customersModel)
        {
            _taskManagerDB.DeleteCustomer(DBName, customersModel);
        }
        public void DeleteArrangeProduct(ArrangeProductsModel arrangeProductsModel)
        {
            _taskManagerDB.DeleteArrangeProduct(DBName, arrangeProductsModel);
        }
        public void DeleteBagCover(BagCoverModel bagCoverModel)
        {
            _taskManagerDB.DeleteBagCover(DBName, bagCoverModel);
        }
        public void DeleteCementBrand(CementBrandModel cementBrandModel)
        {
            _taskManagerDB.DeleteCementBrand(DBName, cementBrandModel);
        }
        public void DeleteProduct(ProductsModel productsModel)
        {
            _taskManagerDB.DeleteProduct(DBName, productsModel);
        }
        #endregion

        public bool CheckExistGuid(DB_TABLE_LIST dbTableSelected, Guid guid)
        {
            switch (dbTableSelected)
            {
                case DB_TABLE_LIST.Customers:
                    var database1 = _taskManagerDB.SelectCustomers(DBName);
                    var listCustomer = database1?.DataSet?.Tables[0].ToList<CustomersModel>();
                    var customer = listCustomer?.Where(x => x.Guid == guid).ToList();
                    if (customer.Count == 0)
                        return false;
                    else
                        return true;

                case DB_TABLE_LIST.ArrangeProducts:
                    var database2 = _taskManagerDB.SelectArrangeProducts(DBName);
                    var listArrangeProducts = database2?.DataSet?.Tables[0].ToList<CustomersModel>();
                    var arrangeProduct = listArrangeProducts?.Where(x => x.Guid == guid).ToList();
                    if (arrangeProduct.Count == 0)
                        return false;
                    else
                        return true;
                case DB_TABLE_LIST.BagCover:
                    var database3 = _taskManagerDB.SelectBagCover(DBName);
                    var listBagCover = database3?.DataSet?.Tables[0].ToList<CustomersModel>();
                    var bagCover = listBagCover?.Where(x => x.Guid == guid).ToList();
                    if (bagCover.Count == 0)
                        return false;
                    else
                        return true;
                case DB_TABLE_LIST.CementBrand:
                    var database4 = _taskManagerDB.SelectCementBrand(DBName);
                    var listBrand = database4?.DataSet?.Tables[0].ToList<CustomersModel>();
                    var brand = listBrand?.Where(x => x.Guid == guid).ToList();
                    if (brand.Count == 0)
                        return false;
                    else
                        return true;
                case DB_TABLE_LIST.Products:
                    var database5 = _taskManagerDB.SelectProducts(DBName);
                    var listProduct = database5?.DataSet?.Tables[0].ToList<CustomersModel>();
                    var product = listProduct?.Where(x => x.Guid == guid).ToList();
                    if (product.Count == 0)
                        return false;
                    else
                        return true;
                case DB_TABLE_LIST.Roles:
                    var database6 = _taskManagerDB.SelectRoles(DBName);
                    var listRole = database6?.DataSet?.Tables[0].ToList<CustomersModel>();
                    var role = listRole?.Where(x => x.Guid == guid).ToList();
                    if (role.Count == 0)
                        return false;
                    else
                        return true;
                default: return false;
            }
        }
        #endregion

        #region Properties
        public List<CustomersModel> CustomerModelList
        {
            get { return _customerModelList; }
            set
            {
                if (Set(ref _customerModelList, value))
                {

                }
            }
        }
        public List<ArrangeProductsModel> ArrangeProductsModelList
        {
            get { return _arrangeProductsModelList; }
            set
            {
                if (Set(ref _arrangeProductsModelList, value))
                {

                }
            }
        }
        public List<BagCoverModel> BagCoverModelList
        {
            get { return _bagCoverModelList; }
            set
            {
                if (Set(ref _bagCoverModelList, value))
                {

                }
            }
        }
        public List<CementBrandModel> CementBrandModelList
        {
            get { return _cementBrandModelList; }
            set
            {
                if (Set(ref _cementBrandModelList, value))
                {

                }
            }
        }
        public List<ProductsModel> ProductsModelList
        {
            get { return _productsModelList; }
            set
            {
                if (Set(ref _productsModelList, value))
                {

                }
            }
        }
        public List<RolesModel> RolesModelList
        {
            get { return _rolesModelList; }
            set
            {
                if (Set(ref _rolesModelList, value))
                {

                }
            }
        }

        public ObservableCollection<string> ArrangeProductList
        {
            get { return _arrangeProductList; }
            set
            {
                if (Set(ref _arrangeProductList, value))
                {

                }
            }
        }
        public ObservableCollection<string> BagCoverList
        {
            get { return _bagCoverList; }
            set
            {
                if (Set(ref _bagCoverList, value))
                {

                }
            }
        }
        public ObservableCollection<string> CementBrandList
        {
            get { return _cementBrandList; }
            set
            {
                if (Set(ref _cementBrandList, value))
                {

                }
            }
        }
        public ObservableCollection<string> ProductList
        {
            get { return _productList; }
            set
            {
                if (Set(ref _productList, value))
                {

                }
            }
        }
        public List<string> ShiftList
        {
            get { return _shiftList; }
            set
            {
                Set(ref _shiftList, value);
            }
        }

        public List<string> _dbTableList;
        public List<string> DbTableList
        {
            get => _dbTableList;
            set
            {
                Set(ref _dbTableList, value);
            }
        }
        private DB_TABLE_LIST _dbTableSelected = DB_TABLE_LIST.Customers;
        public DB_TABLE_LIST DB_TABLE_SELECTED
        {
            get => _dbTableSelected;
            set
            {
                if (Set(ref _dbTableSelected, value))
                {

                }
            }
        }
        private string _dbTableSelectedItem = CommonDefines.GetEnumDescription(DB_TABLE_LIST.Customers);
        public string DBTableSelectedItem
        {
            get => _dbTableSelectedItem;
            set
            {
                Set(ref _dbTableSelectedItem, value);
            }
        }

        public CustomersModel CustomerModel
        {
            get { return _customerModel; }
            set
            {
                Set(ref _customerModel, value);
            }
        }
        public ArrangeProductsModel ArrangeProductsModel
        {
            get => _arrangeProductsModel;
            set
            {
                Set(ref _arrangeProductsModel, value);
            }
        }
        public BagCoverModel BagCoverModel
        {
            get => _bagCoverModel;
            set
            {
                Set(ref _bagCoverModel, value);
            }
        }
        public CementBrandModel CementBrandModel
        {
            get => _cementBrandModel;
            set
            {
                Set(ref _cementBrandModel, value);
            }
        }
        public ProductsModel ProductsModel
        {
            get => _productsModel;
            set
            {
                Set(ref _productsModel, value);
            }
        }
        public RolesModel RolesModel
        {
            get => _rolesModel;
            set
            {
                Set(ref _rolesModel, value);
            }
        }

        private bool _isCreateNew;
        public bool IsCreateNew
        {
            get => _isCreateNew;
            set
            {
                Set(ref _isCreateNew, value);
            }
        }
        #endregion

        #region Command
        public ICommand SelectionChangedCmd { get; }
        public ICommand SaveChangedCmd { get; }
        public ICommand PreviewMouseRightButtonDownCmd { get; }
        public ICommand CreateNewCmd { get; }
        public ICommand DeleteCmd { get; }
        #endregion
    }
}
