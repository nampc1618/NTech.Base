using NTech.Base.Commons.BaseCommand;
using NTech.Xm.Station.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Xm.Station.Command
{
    public class CheckSingleStatusLEDTableFixedCmd : CommandBase
    {
        private readonly TroughViewModel _troughViewModel;
        public CheckSingleStatusLEDTableFixedCmd(TroughViewModel troughViewModel)
        {
            this._troughViewModel = troughViewModel;
        }
        public async override void Execute(object parameter)
        {
            switch (int.Parse((string)parameter))
            {
                case 1:
                    await _troughViewModel.CheckSingleStatusLEDTableFixed(_troughViewModel.LED_FIXED_1, 0);
                    break;
                case 2:
                    await _troughViewModel.CheckSingleStatusLEDTableFixed(_troughViewModel.LED_FIXED_2, 0);
                    break;
                case 3:
                    await _troughViewModel.CheckSingleStatusLEDTableFixed(_troughViewModel.LED_FIXED_3, 0);
                    break;
                case 4:
                    await _troughViewModel.CheckSingleStatusLEDTableFixed(_troughViewModel.LED_FIXED_4, 0);
                    break;
                case 5:
                    await _troughViewModel.CheckSingleStatusLEDTableFixed(_troughViewModel.LED_FIXED_5, 0);
                    break;
                case 6:
                    await _troughViewModel.CheckSingleStatusLEDTableFixed(_troughViewModel.LED_FIXED_6, 0);
                    break;
                case 7:
                    await _troughViewModel.CheckSingleStatusLEDTableFixed(_troughViewModel.LED_FIXED_7, 0);
                    break;
                case 8:
                    await _troughViewModel.CheckSingleStatusLEDTableFixed(_troughViewModel.LED_FIXED_8, 0);
                    break;
                case 9:
                    await _troughViewModel.CheckSingleStatusLEDTableFixed(_troughViewModel.LED_FIXED_9, 0);
                    break;
                case 10:
                    await _troughViewModel.CheckSingleStatusLEDTableFixed(_troughViewModel.LED_FIXED_10, 0);
                    break;
                case 11:
                    await _troughViewModel.CheckSingleStatusLEDTableFixed(_troughViewModel.LED_FIXED_11, 0);
                    break;
                case 12:
                    await _troughViewModel.CheckSingleStatusLEDTableFixed(_troughViewModel.LED_FIXED_12, 0);
                    break;
                case 13:
                    await _troughViewModel.CheckSingleStatusLEDTableFixed(_troughViewModel.LED_FIXED_13, 0);
                    break;
                case 14:
                    await _troughViewModel.CheckSingleStatusLEDTableFixed(_troughViewModel.LED_FIXED_14, 0);
                    break;
                default:
                    break;
            }
        }
    }
}
