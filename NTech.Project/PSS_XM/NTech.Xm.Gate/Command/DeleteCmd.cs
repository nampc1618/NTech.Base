using NTech.Base.Commons.BaseCommand;
using NTech.Xm.Gate.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace NTech.Xm.Gate.Command
{
    public class DeleteCmd : CommandBase
    {
        private readonly SettingsViewModel _settingsViewModel;
        public DeleteCmd(SettingsViewModel settingsViewModel)
        {
            this._settingsViewModel = settingsViewModel;
        }
        public override void Execute(object parameter)
        {
            switch ((DB_TABLE_LIST)parameter)
            {
                case DB_TABLE_LIST.Customers:
                    if (_settingsViewModel.CustomerModel != null)
                    {
                        _settingsViewModel.DeleteCustomer(_settingsViewModel.CustomerModel);
                        _settingsViewModel.SelectCustomers();
                        MainViewModel.Instance.MainView.settingsView.popupOption.IsOpen = false;
                    }
                    break;
                case DB_TABLE_LIST.ArrangeProducts:
                    if (_settingsViewModel.ArrangeProductsModel != null)
                    {
                        _settingsViewModel.DeleteArrangeProduct(_settingsViewModel.ArrangeProductsModel);
                        _settingsViewModel.SelectArrangeProducts();
                        MainViewModel.Instance.MainView.settingsView.popupOption.IsOpen = false;
                    }
                    break;
                case DB_TABLE_LIST.BagCover:
                    if (_settingsViewModel.BagCoverModel != null)
                    {
                        _settingsViewModel.DeleteBagCover(_settingsViewModel.BagCoverModel);
                        _settingsViewModel.SelectBagCover();
                        MainViewModel.Instance.MainView.settingsView.popupOption.IsOpen = false;
                    }
                    break;
                case DB_TABLE_LIST.CementBrand:
                    if (_settingsViewModel.CementBrandModel != null)
                    {
                        _settingsViewModel.DeleteCementBrand(_settingsViewModel.CementBrandModel);
                        _settingsViewModel.SelectCementBrand();
                        MainViewModel.Instance.MainView.settingsView.popupOption.IsOpen = false;
                    }
                    break;
                case DB_TABLE_LIST.Products:
                    if (_settingsViewModel.ProductsModel != null)
                    {
                        _settingsViewModel.DeleteProduct(_settingsViewModel.ProductsModel);
                        _settingsViewModel.SelectProducts();
                        MainViewModel.Instance.MainView.settingsView.popupOption.IsOpen = false;
                    }
                    break;
                case DB_TABLE_LIST.Roles:
                    break;
                default:
                    break;
            }
        }
    }
}
