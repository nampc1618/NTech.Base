using NTech.Base.Commons.BaseCommand;
using NTech.Xm.Station.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Xm.Station.Command
{
    public class StopPrintCmd : CommandBase
    {
        private readonly PrinterViewModel _printerViewModel;
        public StopPrintCmd(PrinterViewModel printerViewModel)
        {
            _printerViewModel = printerViewModel;
        }
        public override void Execute(object parameter)
        {
            if (_printerViewModel.PRINTERSelected.PRINTER_STATES != Commons.Defines.PRINTER_STATES.PRINTING
                || _printerViewModel.PRINTERSelected.PRINTER_STATES != Commons.Defines.PRINTER_STATES.DISCONNECTED)
                _printerViewModel.StepsPerform(_printerViewModel.PRINTERSelected, Commons.Defines.STEPS_PERFORM.STOP_PRINT);
            //MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
        }
    }
}
