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
    public class PreviewMouseRightButtonDownCmd : CommandBase
    {
        private readonly MainViewModel _mainViewModel;
        public PreviewMouseRightButtonDownCmd(MainViewModel mainViewModel)
        {
            this._mainViewModel = mainViewModel;
        }
        public override void Execute(object parameter)
        {
            if (parameter is TreeView || parameter is Image)
            {
                //ContextMenu context = new ContextMenu();

                //MenuItem menuItemRequestStatus = new MenuItem();
                //menuItemRequestStatus.Header = "Kiểm tra trạng thái máy in";
                //menuItemRequestStatus.Command = _mainViewModel.RequestPrintStatusCmd;
                //menuItemRequestStatus.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");
                //menuItemRequestStatus.FontWeight = FontWeights.SemiBold;
                //menuItemRequestStatus.FontSize = 16;

                //context.Items.Add(menuItemRequestStatus);
                //context.PlacementTarget = _mainViewModel.MainView;
                //context.IsOpen = true;

                _mainViewModel.MainView.popupImgPrinter.IsOpen = true;
                _mainViewModel.PrinterViewModel.IsManual = true;
            }
           
            else
            {
                _mainViewModel.MainView.popupDG.IsOpen = true;
            }
        }
    }
}
