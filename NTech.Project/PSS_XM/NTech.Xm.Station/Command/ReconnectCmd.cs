using NTech.Base.Commons.BaseCommand;
using NTech.Xm.Station.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Xm.Station.Command
{
    public class ReconnectCmd : CommandBase
    {
        private readonly MainViewModel _mainViewModel;
        public ReconnectCmd(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
        public override void Execute(object parameter)
        {
            _mainViewModel.Reconnect();
            //_mainViewModel.SelectMessagesDetail_NewAndAll(_mainViewModel.StrDateNow);
            _mainViewModel.SelectMessagesDetail_All2Day();
            //_mainViewModel.SelectMessagesDetail_Printing(_mainViewModel.StrDateNow);
            _mainViewModel.SelectMessagesDetail_AllPrinting();
        }
    }
}
