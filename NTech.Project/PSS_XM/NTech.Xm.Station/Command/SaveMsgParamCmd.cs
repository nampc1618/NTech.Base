using NTech.Base.Commons.BaseCommand;
using NTech.Xm.Station.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Xm.Station.Command
{
    public class SaveMsgParamCmd : CommandBase
    {
        private readonly PrinterViewModel _printerViewModel;
        public SaveMsgParamCmd(PrinterViewModel printerViewModel)
        {
            this._printerViewModel = printerViewModel;
        }
        public override void Execute(object parameter)
        {
            _printerViewModel.SaveMsgParam(_printerViewModel.PRINTERSelected.LenghtMsgInRaster,
                                           _printerViewModel.PRINTERSelected.HeightMsg,
                                           _printerViewModel.PRINTERSelected.WidthMsg,
                                           _printerViewModel.PRINTERSelected.Delay,
                                           _printerViewModel.PRINTERSelected.FieldOrientation);
        }
    }
}
