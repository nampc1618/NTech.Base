using BespokeFusion;
using NTech.Base.Commons.BaseCommand;
using NTech.Xm.Station.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace NTech.Xm.Station.Command
{
    public class PreviewMouseLeftButtonDownCmd : CommandBase
    {
        private readonly PrinterViewModel _printerViewModel;
        public PreviewMouseLeftButtonDownCmd(PrinterViewModel printerViewModel)
        {
            this._printerViewModel= printerViewModel;
        }
        public override void Execute(object parameter)
        {
            if(parameter is ToggleButton)
            {
                if (_printerViewModel.PRINTERSelected.PRINTER_STATES == Commons.Defines.PRINTER_STATES.PRINTING)
                {
                    if (MaterialMessageBox.ShowWithCancel($"Bạn chắc chắn muốn DỪNG IN {_printerViewModel.PRINTERSelected.PrinterName}?", true, false) == MessageBoxResult.OK)
                    {
                        _printerViewModel.PRINTERSelected.ToggleUncheckedCmd.Execute(true);
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
    }
}
