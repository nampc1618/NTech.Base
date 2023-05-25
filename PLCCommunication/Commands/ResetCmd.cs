using NTech.Base.Commons.BaseCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PLCCommunication.Commands
{
    public class ResetCmd : CommandBase
    {
        private readonly PLCModel _plc;
        public ResetCmd(PLCModel plc)
        {
            _plc = plc;
        }
        public override void Execute(object parameter)
        {
            if (MessageBox.Show("Bạn chắc chắn muốn RESET bộ đếm PLC", "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                string para = parameter as string;
                if (para != null)
                {
                    switch (para)
                    {
                        case "1":
                            _plc.SetBit(_plc.DicBitReset["1"]);
                            break;
                        case "2":
                            _plc.SetBit(_plc.DicBitReset["2"]);
                            break;
                        case "3":
                            _plc.SetBit(_plc.DicBitReset["3"]);
                            break;
                        case "4":
                            _plc.SetBit(_plc.DicBitReset["4"]);
                            break;
                        case "5":
                            _plc.SetBit(_plc.DicBitReset["5"]);
                            break;
                        case "6":
                            _plc.SetBit(_plc.DicBitReset["6"]);
                            break;
                    }
                }
            }
        }
    }
}
