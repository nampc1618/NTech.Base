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
    public class CancelSelectionsCmd : CommandBase
    {
        private readonly MainViewModel _mainViewModel;
        public CancelSelectionsCmd(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
        public override void Execute(object parameter)
        {
            _mainViewModel.MessagesDetailModel = null;
           //await _mainViewModel.PrinterViewModel.PRINTERSelected.Stop();
        }
    }
}
