using NTech.Xm.Commons.Defines;
using NTech.Xm.Station.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NTech.Xm.Station.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public static SettingsViewModel Instance { get; private set; }
        public LoginViewModel LoginViewModel { get; private set; }
        public LineViewModel LineViewModel { get; private set; }
        public PrinterViewModel PrinterViewModel { get; private set; }
        public TroughViewModel TroughViewModel { get; private set; }
        public SettingsViewModel(LoginViewModel loginViewModel, LineViewModel lineViewModel, PrinterViewModel printerViewModel, TroughViewModel troughViewModel)
        {
            if (Instance is null)
                Instance = this;
            else
                return;
            this.LoginViewModel = loginViewModel;

            this.LoginViewModel.LoginEvent += LoginViewModel_LoginEvent;
            this.LineViewModel = lineViewModel;
            this.PrinterViewModel = printerViewModel;
            this.TroughViewModel = troughViewModel;

            this.CheckAllStatusLEDTableFixedCmd = new CheckAllStatusLEDTableFixedCmd(this.TroughViewModel);
            this.CheckSingleStatusLEDTableFixedCmd = new CheckSingleStatusLEDTableFixedCmd(this.TroughViewModel);
            this.SaveMsgParamCmd = new SaveMsgParamCmd(this.PrinterViewModel);
        }

        private void LoginViewModel_LoginEvent(object sender, Xm.Commons.Defines.LOGIN_STATE loginState, Xm.Commons.Defines.ROLE role)
        {
            switch (loginState)
            {
                case Xm.Commons.Defines.LOGIN_STATE.LOGOUT:
                    this.IsLoginSuccess = false;
                    this.IsAdmin = false;
                    break;
                case Xm.Commons.Defines.LOGIN_STATE.LOGIN_SUCCESS:
                    this.IsLoginSuccess = true;
                    if (role == ROLE.ADMIN)
                        this.IsAdmin = true;
                    else
                        this.IsAdmin = false;
                    break;
                case Xm.Commons.Defines.LOGIN_STATE.LOGIN_FAILED:
                    this.IsLoginSuccess = false;
                    this.IsAdmin = false;
                    break;
                default:
                    this.IsLoginSuccess = false;
                    break;
            }
        }

        private bool _isLoginSuccess;
        public bool IsLoginSuccess
        {
            get => _isLoginSuccess;
            set
            {
                Set(ref _isLoginSuccess, value);
            }
        }
        private bool _isAdmin;
        public bool IsAdmin
        {
            get => _isAdmin;
            set
            {
                Set(ref _isAdmin, value);
            }
        }

        public ICommand CheckAllStatusLEDTableFixedCmd { get; }
        public ICommand CheckSingleStatusLEDTableFixedCmd { get; }
        public ICommand SaveMsgParamCmd { get; }
    }
}
