using NTech.Base.Commons.BaseCommand;
using NTech.Xm.Database.Models;
using NTech.Xm.Gate.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Xm.Gate.Command
{
    public class SaveChangedCmd : CommandBase
    {
        private readonly SettingsViewModel _settingsViewModel;
        public SaveChangedCmd(SettingsViewModel settingsViewModel)
        {
            this._settingsViewModel = settingsViewModel;
        }
        public override void Execute(object parameter)
        {
            switch (_settingsViewModel.DB_TABLE_SELECTED)
            {
                case DB_TABLE_LIST.Customers:
                    if (_settingsViewModel.CustomerModel == null)
                        return;
                    if(!_settingsViewModel.CheckExistGuid(DB_TABLE_LIST.Customers, _settingsViewModel.CustomerModel.Guid))
                    {
                        //Insert
                        _settingsViewModel.InsertCustomer(_settingsViewModel.CustomerModel);
                        _settingsViewModel.SelectCustomers();
                    }
                    else
                    {
                        //Update
                        _settingsViewModel.UpdateCustomer(_settingsViewModel.CustomerModel);
                        _settingsViewModel.SelectCustomers();
                    }
                    break;
                case DB_TABLE_LIST.ArrangeProducts:
                    if (_settingsViewModel.ArrangeProductsModel == null)
                        return;
                    if (!_settingsViewModel.CheckExistGuid(DB_TABLE_LIST.ArrangeProducts, _settingsViewModel.ArrangeProductsModel.Guid))
                    {
                        //Insert
                        _settingsViewModel.InsertArrangeProduct(_settingsViewModel.ArrangeProductsModel);
                        _settingsViewModel.SelectArrangeProducts();
                    }
                    else
                    {
                        //Update
                        _settingsViewModel.UpdateArrangeProduct(_settingsViewModel.ArrangeProductsModel);
                        _settingsViewModel.SelectArrangeProducts();
                    }
                    break;
                case DB_TABLE_LIST.BagCover:
                    if (_settingsViewModel.BagCoverModel == null)
                        return;
                    if (!_settingsViewModel.CheckExistGuid(DB_TABLE_LIST.BagCover, _settingsViewModel.BagCoverModel.Guid))
                    {
                        //Insert
                        _settingsViewModel.InsertBagCover(_settingsViewModel.BagCoverModel);
                        _settingsViewModel.SelectBagCover();
                    }
                    else
                    {
                        //Update
                        _settingsViewModel.UpdateBagCover(_settingsViewModel.BagCoverModel);
                        _settingsViewModel.SelectBagCover();
                    }
                    break;
                case DB_TABLE_LIST.CementBrand:
                    if (_settingsViewModel.CementBrandModel == null)
                        return;
                    if (!_settingsViewModel.CheckExistGuid(DB_TABLE_LIST.CementBrand, _settingsViewModel.CementBrandModel.Guid))
                    {
                        //Insert
                        _settingsViewModel.InsertCementBrand(_settingsViewModel.CementBrandModel);
                        _settingsViewModel.SelectCementBrand();
                    }
                    else
                    {
                        //Update
                        _settingsViewModel.UpdateCementBrand(_settingsViewModel.CementBrandModel);
                        _settingsViewModel.SelectCementBrand();
                    }
                    break;
                case DB_TABLE_LIST.Products:
                    if (_settingsViewModel.ProductsModel == null)
                        return;
                    if (!_settingsViewModel.CheckExistGuid(DB_TABLE_LIST.Products, _settingsViewModel.ProductsModel.Guid))
                    {
                        //Insert
                        _settingsViewModel.InsertProduct(_settingsViewModel.ProductsModel);
                        _settingsViewModel.SelectProducts();
                    }
                    else
                    {
                        //Update
                        _settingsViewModel.UpdateProduct(_settingsViewModel.ProductsModel);
                        _settingsViewModel.SelectProducts();
                    }
                    break;
                case DB_TABLE_LIST.Roles:
                    if (_settingsViewModel.RolesModel == null)
                        return;
                    if (_settingsViewModel.CheckExistGuid(DB_TABLE_LIST.Roles, _settingsViewModel.RolesModel.Guid))
                    {
                        //Insert
                        _settingsViewModel.UpdateRole(_settingsViewModel.RolesModel);
                        _settingsViewModel.SelectRoles();
                    }
                    break;
            }
        }
    }
}
