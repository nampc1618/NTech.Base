using NTech.Base.Commons.BaseCommand;
using NTech.Xm.Station.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NTech.Xm.Station.Command
{
    public class PressF5KeyCmd : CommandBase
    {
        private readonly PrinterViewModel _printerViewModel;
        public PressF5KeyCmd(PrinterViewModel printerViewModel)
        {
            this._printerViewModel = printerViewModel;
        }
        public override void Execute(object parameter)
        {
            if(_printerViewModel.PRINTERSelected != null)
            {
                if (_printerViewModel.PRINTERSelected.PRINTER_STATES == Commons.Defines.PRINTER_STATES.PRINT_STOP)
                {
                    if (MessageBox.Show($"{_printerViewModel.PRINTERSelected.PrinterName} đang chỉ TẠM DỪNG IN\n Bạn có CHẮC CHẮN muốn in một bản tin mới?", "", MessageBoxButton.YesNo,
                        MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        _printerViewModel.StepsPerform(_printerViewModel.PRINTERSelected, Commons.Defines.STEPS_PERFORM.REQUEST_PRINTER_STATUS);
                }
                else
                {
                    _printerViewModel.StepsPerform(_printerViewModel.PRINTERSelected, Commons.Defines.STEPS_PERFORM.REQUEST_PRINTER_STATUS);
                }
            }
        }
    }
}
