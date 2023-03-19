using NTech.Base.Commons.BaseCommand;
using NTech.Xm.Commons.Defines;
using NTech.Xm.Gate.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Xm.Gate.Command
{
    public class SelectionChangedCmd : CommandBase
    {
        private readonly SettingsViewModel _settingsViewModel;
        public SelectionChangedCmd(SettingsViewModel settingsViewModel)
        {
            this._settingsViewModel = settingsViewModel;
        }
        public override void Execute(object parameter)
        {
            switch (_settingsViewModel.DBTableSelectedItem)
            {
                case "Danh sách khách hàng":
                    _settingsViewModel.DB_TABLE_SELECTED = DB_TABLE_LIST.Customers;
                    _settingsViewModel.IsCreateNew = false;
                    _settingsViewModel.SelectCustomers();
                    break;
                case "Cách xếp hàng":
                    _settingsViewModel.DB_TABLE_SELECTED = DB_TABLE_LIST.ArrangeProducts;
                    _settingsViewModel.IsCreateNew = false;
                    _settingsViewModel.SelectArrangeProducts();
                    break;
                case "Loại vỏ bao":
                    _settingsViewModel.DB_TABLE_SELECTED = DB_TABLE_LIST.BagCover;
                    _settingsViewModel.IsCreateNew = false;
                    _settingsViewModel.SelectBagCover();
                    break;
                case "Thương hiệu xi măng":
                    _settingsViewModel.DB_TABLE_SELECTED = DB_TABLE_LIST.CementBrand;
                    _settingsViewModel.IsCreateNew = false;
                    _settingsViewModel.SelectCementBrand();
                    break;
                case "Danh sách sản phẩm":
                    _settingsViewModel.DB_TABLE_SELECTED = DB_TABLE_LIST.Products;
                    _settingsViewModel.IsCreateNew = false;
                    _settingsViewModel.SelectProducts();
                    break;
                case "Quản trị":
                    _settingsViewModel.DB_TABLE_SELECTED = DB_TABLE_LIST.Roles;
                    _settingsViewModel.IsCreateNew = false;
                    _settingsViewModel.SelectRoles();
                    break;
                default:
                    _settingsViewModel.DB_TABLE_SELECTED = DB_TABLE_LIST.Customers;
                    _settingsViewModel.IsCreateNew = false;
                    _settingsViewModel.SelectCustomers();
                    break;
            }
        }
    }
}
