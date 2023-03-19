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
    public class ClosingCmd : CommandBase
    {
        private readonly MainViewModel _mainViewModel;
        public ClosingCmd(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
        public override void Execute(object parameter)
        {
            
        }
    }
}
