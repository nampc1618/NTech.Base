using NTech.Base.Commons.BaseCommand;
using NTech.Xm.Gate.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Xm.Gate.Command
{
    public class SearchCmd : CommandBase
    {
        private readonly MainViewModel _mainViewModel;
        public SearchCmd(MainViewModel mainViewModel)
        {
            this._mainViewModel = mainViewModel;
        }
        public override void Execute(object parameter)
        {
            
        }
    }
}
