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
    public class ConnectPrinterCmd : CommandBase
    {
        private readonly PrinterViewModel _printerViewModel;
        public ConnectPrinterCmd(PrinterViewModel printerViewModel)
        {
            _printerViewModel = printerViewModel;   
        }
        public override async void Execute(object parameter)
        {
            _printerViewModel.StepsPerform(_printerViewModel.PRINTERSelected, STEPS_PERFORM.CONNECT_PRINTER);
            await TroughViewModel.Instance.CheckSingleStatusLEDTableFixed(_printerViewModel.PRINTERSelected.LEDTableFixed, 0);
        }
    }
}
