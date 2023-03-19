using BespokeFusion;
using NTech.Base.Commons.BaseCommand;
using NTech.Xm.Station.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NTech.Xm.Station.Command
{
    public class RequestPrintStatusCmd : CommandBase
    {
        private readonly PrinterViewModel _printerViewModel;
        public RequestPrintStatusCmd(PrinterViewModel printerViewModel)
        {
            _printerViewModel = printerViewModel;
        }
        public override void Execute(object parameter)
        {
            if (_printerViewModel.PRINTERSelected.PRINTER_STATES == Commons.Defines.PRINTER_STATES.PRINT_STOP)
            {
                if (MaterialMessageBox.ShowWithCancel($"{_printerViewModel.PRINTERSelected.PrinterName} đang chỉ TẠM DỪNG IN\n Bạn có CHẮC CHẮN muốn in một bản tin mới?", "THÔNG BÁO") == MessageBoxResult.OK)
                    _printerViewModel.StepsPerform(_printerViewModel.PRINTERSelected, Commons.Defines.STEPS_PERFORM.REQUEST_PRINTER_STATUS);
                //MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
            }
            else
            {
                _printerViewModel.StepsPerform(_printerViewModel.PRINTERSelected, Commons.Defines.STEPS_PERFORM.REQUEST_PRINTER_STATUS);
            }
        }
    }
}
