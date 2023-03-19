using NTech.Base.Commons.BaseCommand;
using NTech.Xm.Gate.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Xm.Gate.Command
{
    public class CreateNewCmd : CommandBase
    {
        private readonly SettingsViewModel _settingsViewModel;
        public CreateNewCmd(SettingsViewModel settingsViewModel)
        {
            this._settingsViewModel = settingsViewModel;
        }
        public override void Execute(object parameter)
        {
            _settingsViewModel.IsCreateNew = true;
            MainViewModel.Instance.MainView.settingsView.popupOption.IsOpen = false;
        }
    }
}
