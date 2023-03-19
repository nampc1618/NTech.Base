using NTech.Base.Commons.BaseCommand;
using NTech.Xm.Station.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Xm.Station.Command
{
    public class CheckAllStatusLEDTableFixedCmd : CommandBase
    {
        private readonly TroughViewModel _troughViewModel;
        public CheckAllStatusLEDTableFixedCmd(TroughViewModel troughViewModel)
        {
            this._troughViewModel = troughViewModel;
        }
        public async override void Execute(object parameter)
        {
           await _troughViewModel.CheckAllCardStatusForLEDTableFixed();
        }
    }
}
