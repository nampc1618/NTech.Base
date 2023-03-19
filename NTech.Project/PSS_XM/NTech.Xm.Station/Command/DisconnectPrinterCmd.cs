using NTech.Base.Commons.BaseCommand;
using NTech.Xm.Station.Commons.Defines;
using NTech.Xm.Station.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NTech.Xm.Station.Command
{
    public class DisconnectPrinterCmd : CommandBase
    {
        private readonly PrinterViewModel _printerViewModel;
        public DisconnectPrinterCmd(PrinterViewModel printerViewModel)
        {
            _printerViewModel = printerViewModel;   
        }
        public override void Execute(object parameter)
        {
            _printerViewModel.StepsPerform(_printerViewModel.PRINTERSelected, STEPS_PERFORM.DISCONNECT_PRINTER);
        }
    }
}
