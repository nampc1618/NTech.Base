using NTech.Base.Commons.BaseCommand;
using NTech.Xm.Station.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Xm.Station.Command
{
    public class DeleteMessageDataCmd : CommandBase
    {
        private readonly PrinterViewModel _printerViewModel;
        public DeleteMessageDataCmd(PrinterViewModel printerViewModel)
        {
            this._printerViewModel = printerViewModel;
        }
        public override void Execute(object parameter)
        {
            _printerViewModel.StepsPerform(_printerViewModel.PRINTERSelected, Commons.Defines.STEPS_PERFORM.DELETE_MESSAGE_DATA);
            //MainViewModel.Instance.MainView.popupImgPrinter.IsOpen = false;
        }
    }
}
