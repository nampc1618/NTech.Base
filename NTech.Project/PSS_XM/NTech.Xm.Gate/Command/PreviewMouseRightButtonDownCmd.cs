using NTech.Base.Commons.BaseCommand;
using NTech.Xm.Gate.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NTech.Xm.Gate.Command
{
    public class PreviewMouseRightButtonDownCmd : CommandBase
    {
        private readonly SettingsViewModel _settingsViewModel;
        public PreviewMouseRightButtonDownCmd(SettingsViewModel settingsViewModel)
        {
            _settingsViewModel = settingsViewModel;
        }
        public override void Execute(object parameter)
        {
            if(((DataGrid)parameter).Name == "dgNewMessages")
            {
                MainViewModel.Instance.MainView.popupDeleteMsgNew.IsOpen= true;
            }
            else
            {
                MainViewModel.Instance.MainView.settingsView.popupOption.IsOpen = true;
            }
        }
    }
}
