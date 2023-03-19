using Microsoft.Xaml.Behaviors.Media;
using Newtonsoft.Json;
using NTech.Base.Commons.BaseCommand;
using NTech.Xm.Database.Models;
using NTech.Xm.Gate.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NTech.Xm.Gate.Command
{
    public class InsertMessageDetailCmd : CommandBase
    {
        private readonly MainViewModel _mainViewModel;
        public InsertMessageDetailCmd(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        public override void Execute(object parameter)
        {
            if (MessageBox.Show("Tạo bản tin mới?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (_mainViewModel.MessagesDetailModel != null && _mainViewModel.CheckExistGuid(_mainViewModel.MessagesDetailModel.Guid)
                    && _mainViewModel.IsEditMsg == true)
                {
                    _mainViewModel.DeleteMessagesDetail();
                    _mainViewModel.IsEditMsg = false;
                }

                _mainViewModel.MessagesDetailModel.Guid = Guid.NewGuid();
                _mainViewModel.MessagesDetailModel.ManufacturingDate = DateTime.Now.ToString("dd-MM-yyyy");
                _mainViewModel.MessagesDetailModel.ManufacturingTime = DateTime.Now.ToString("HH:mm:ss");
                _mainViewModel.MessagesDetailModel.CustomerCode = _mainViewModel.CustomerModel.CustomerCode;
                _mainViewModel.MessagesDetailModel.CustomerName = _mainViewModel.CustomerModel.CustomerName;
                _mainViewModel.MessagesDetailModel.Line = string.Empty;
                _mainViewModel.MessagesDetailModel.Printer = string.Empty;
                _mainViewModel.MessagesDetailModel.Trough = string.Empty;
                _mainViewModel.MessagesDetailModel.MessageState = "Chưa in";
                _mainViewModel.MessagesDetailModel.Note = string.Empty;

                _mainViewModel.InsertMessageDetail(_mainViewModel.MessagesDetailModel);
                _mainViewModel.SelectMessagesDetail_New(DateTime.Now.ToString("dd-MM-yyyy"));

                _mainViewModel.ServerSendToClient("NeM");


                //Reset info msg
                _mainViewModel.MessagesDetailModel.Guid = Guid.Empty;
                _mainViewModel.MessagesDetailModel.ManufacturingDate = string.Empty;
                _mainViewModel.MessagesDetailModel.ManufacturingTime = string.Empty;
                _mainViewModel.MessagesDetailModel.ManufacturingShift = null;
                _mainViewModel.MessagesDetailModel.CouponCode = string.Empty;
                _mainViewModel.MessagesDetailModel.LicensePlate = string.Empty;
                _mainViewModel.MessagesDetailModel.CustomerCode = string.Empty;
                _mainViewModel.MessagesDetailModel.CustomerName = string.Empty;
                _mainViewModel.MessagesDetailModel.WeightTon = 0;
                _mainViewModel.MessagesDetailModel.NumberBags = 0;
                _mainViewModel.MessagesDetailModel.NumberBagsPrinted = 0;
                _mainViewModel.MessagesDetailModel.CementType = null;
                _mainViewModel.MessagesDetailModel.BagCoverType = null;
                _mainViewModel.MessagesDetailModel.Brand = null;
                _mainViewModel.MessagesDetailModel.ArrangeType = null;
                _mainViewModel.MessagesDetailModel.Line = string.Empty;
                _mainViewModel.MessagesDetailModel.Printer = string.Empty;
                _mainViewModel.MessagesDetailModel.Trough = string.Empty;
                _mainViewModel.MessagesDetailModel.MessageState = string.Empty;
                _mainViewModel.MessagesDetailModel.Note = string.Empty;
                _mainViewModel.CustomerModel = null;
            }
        }
    }
}
