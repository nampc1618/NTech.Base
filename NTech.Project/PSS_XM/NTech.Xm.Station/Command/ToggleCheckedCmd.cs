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
    public class ToggleCheckedCmd : CommandBase
    {
        private readonly PrinterViewModel _printerViewModel;
        public ToggleCheckedCmd(PrinterViewModel printerViewModel)
        {
            _printerViewModel = printerViewModel;
        }
        public override void Execute(object parameter)
        {
            //if (MaterialMessageBox.ShowWithCancel($"Bạn muốn in tiếp {_printerViewModel.PRINTERSelected.PrinterName}?", false, false) == MessageBoxResult.OK)
            //{
                if ((_printerViewModel.PRINTERSelected.PRINTER_STATES == Commons.Defines.PRINTER_STATES.PRINT_STOP))
                    _printerViewModel.StepsPerform(_printerViewModel.PRINTERSelected, Commons.Defines.STEPS_PERFORM.START_PRINT);
            //}
        }
    }
}
