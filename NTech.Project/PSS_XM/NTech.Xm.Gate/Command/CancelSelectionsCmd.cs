using NTech.Base.Commons.BaseCommand;
using NTech.Xm.Gate.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NTech.Xm.Gate.Command
{
    public class CancelSelectionsCmd : CommandBase
    {
        private readonly MainViewModel _mainViewModel;
        public CancelSelectionsCmd(MainViewModel mainViewModel)
        {
            this._mainViewModel = mainViewModel;
        }
        public override void Execute(object parameter)
        {
            _mainViewModel.CustomerModel = null;
            _mainViewModel.MainView.txtCouponCode.Text = string.Empty;
            _mainViewModel.MainView.txtLicensePlate.Text = string.Empty;
            _mainViewModel.MainView.txtWeightTon.Text = string.Empty;
            _mainViewModel.MainView.lblNumberBag.Content = string.Empty;
            _mainViewModel.MainView.cbbArrangeType.SelectedItem = null;
            _mainViewModel.MainView.cbbBagCoverType.SelectedItem = null;
            _mainViewModel.MainView.cbbBrand.SelectedItem = null;
            _mainViewModel.MainView.cbbCementType.SelectedItem = null;
            _mainViewModel.MainView.cbbManufacturingShift.SelectedItem = null;
        }
    }
}
