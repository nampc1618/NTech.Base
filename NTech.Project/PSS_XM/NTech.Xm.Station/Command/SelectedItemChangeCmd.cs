using NTech.Base.Commons.BaseCommand;
using NTech.Xm.Station.Commons.Defines;
using NTech.Xm.Station.Models;
using NTech.Xm.Station.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace NTech.Xm.Station.Command
{
    public class SelectedItemChangeCmd : CommandBase
    {
        private readonly MainViewModel _mainViewModel;
        public SelectedItemChangeCmd(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        public async override void Execute(object parameter)
        {
            var result = _mainViewModel.MainView.tvMain.SelectedItem;

            if (result is LINE)
            {
                _mainViewModel.PrinterViewModel.PRINTERSName.Clear();
                var line = result as LINE;
                line.IsSelected = true;
                _mainViewModel.LineViewModel.LINESelected = line;
                _mainViewModel.LineViewModel.LINEList.Where(x => x.Tag != line.Tag).ToList().ForEach(a => a.IsSelected = false);
                _mainViewModel.PrinterViewModel.PRINTERList.ForEach(a => a.IsSelected = false);
                _mainViewModel.TroughViewModel.TROUGHList.ForEach(a => a.IsSelected = false);
                //_mainViewModel.MainView.cbbLINE.SelectedItem = line.LineName;
                //_mainViewModel.MainView.cbbPRINTER.SelectedItem = null;
                //_mainViewModel.MainView.cbbTROUGH.SelectedItem = null;
                foreach (var item in line.Printers)
                {
                    _mainViewModel.PrinterViewModel.PRINTERSName.Add(item.PrinterName);
                }
            }
            else if (result is PRINTER)
            {
                var printer = result as PRINTER;
                printer.IsSelected = true;
                _mainViewModel.PrinterViewModel.PRINTERSelected = printer;
                if (printer.PRINTER_STATES == Commons.Defines.PRINTER_STATES.PRINTING || printer.PRINTER_STATES == Commons.Defines.PRINTER_STATES.PRINT_STOP)
                    return;
                _mainViewModel.TroughViewModel.TROUGHSName.Clear();
                foreach (var line in _mainViewModel.LineViewModel.LINEList)
                {
                    var res = line.Printers.Where(x => x.Tag == printer.Tag).Select(x => x).ToList();
                    if (res.Count > 0)
                    {
                        line.IsSelected = true;
                        _mainViewModel.LineViewModel.LINESelected = line;
                        //_mainViewModel.MainView.cbbLINE.SelectedItem = line.LineName;
                    }
                    else
                    {
                        line.IsSelected = false;
                    }
                }
               
                _mainViewModel.PrinterViewModel.PRINTERList.Where(x => x.Tag != printer.Tag)
                                                           .Where(x => x.PRINTER_STATES != PRINTER_STATES.PRINTING)
                                                           .Where(x => x.PRINTER_STATES != PRINTER_STATES.PRINT_STOP)
                                                           .ToList().ForEach(a => a.IsSelected = false);
                _mainViewModel.TroughViewModel.TROUGHList.Where(x => x.TROUGH_STATES != TROUGH_STATES.IS_ACTIVE).ToList()
                                                         .ForEach(a => a.IsSelected = false);
                _mainViewModel.TroughViewModel.TROUGHSelected = null;
                _mainViewModel.MessagesDetailModel = null;
                _mainViewModel.MessagesDetailModelHistory = null;

                await _mainViewModel.TroughViewModel.CheckSingleStatusLEDTableFixed(_mainViewModel.TroughViewModel.TROUGHSelected, 0);

                foreach (var item in printer.Troughs)
                {
                    _mainViewModel.TroughViewModel.TROUGHSName.Add(item.TroughName);
                }
            }
            else if (result is TROUGH)
            {
                if(_mainViewModel.TroughViewModel.TROUGHSelected != null && _mainViewModel.TroughViewModel.TROUGHSelected.TROUGH_STATES == TROUGH_STATES.CHECKING)
                {
                    MessageBox.Show($"Đang kiểm tra {_mainViewModel.TroughViewModel.TROUGHSelected.TroughName}, xin vui lòng chờ!");
                    return;
                }
                var trough = result as TROUGH;
                trough.IsSelected = true;
                _mainViewModel.TroughViewModel.TROUGHSelected = trough;
                if (trough.TROUGH_STATES == TROUGH_STATES.IS_ACTIVE)
                    return;
                foreach (var printer in _mainViewModel.PrinterViewModel.PRINTERList)
                {
                    var res = printer.Troughs.Where(x => x.Tag == trough.Tag && x.PrinterLead == trough.PrinterLead).Select(x => x).ToList();
                    if (res.Count > 0)
                    {
                        printer.IsSelected = true;
                        _mainViewModel.PrinterViewModel.PRINTERSelected = printer;
                        //_mainViewModel.MainView.cbbPRINTER.SelectedItem = printer.PrinterName;
                        foreach (var item in printer.Troughs)
                        {
                            _mainViewModel.TroughViewModel.TROUGHSName.Add(item.TroughName);
                        }
                    }
                    else
                    {
                        if (!(printer.PRINTER_STATES is PRINTER_STATES.PRINTING) && !(printer.PRINTER_STATES is PRINTER_STATES.PRINT_STOP))
                            printer.IsSelected = false;
                    }
                }
                _mainViewModel.TroughViewModel.TROUGHList.Where(x => x.Tag == trough.Tag).ToList()
                                                         .ForEach(a => { a.IsSelected = true; });
                _mainViewModel.TroughViewModel.TROUGHList.Where(x => x.Tag != trough.Tag && x.TROUGH_STATES != TROUGH_STATES.IS_ACTIVE).ToList()
                                                         .ForEach(a => a.IsSelected = false);
                //_mainViewModel.MainView.cbbTROUGH.SelectedItem = trough.TroughName;
                //await _mainViewModel.TroughViewModel.SetTextInitalForLEDTable(_mainViewModel.TroughViewModel.TROUGHSelected, "SẴN SÀNG");
                _mainViewModel.PrinterViewModel.PRINTERSelected.IsUseTrough= true;
                await _mainViewModel.TroughViewModel.CheckSingleStatusLEDTableFixed(_mainViewModel.TroughViewModel.TROUGHSelected, 0);
            }
        }
    }
}
