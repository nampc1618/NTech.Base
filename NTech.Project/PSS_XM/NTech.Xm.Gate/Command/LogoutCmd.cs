using NTech.Base.Commons.BaseCommand;
using NTech.Xm.Commons.Defines;
using NTech.Xm.Gate.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NTech.Xm.Gate.Command
{
    public class LogoutCmd : CommandBase
    {
        private readonly LoginViewModel _loginViewModel;
        public LogoutCmd(LoginViewModel loginViewModel)
        {
            this._loginViewModel = loginViewModel;
        }
        public override void Execute(object parameter)
        {
            _loginViewModel.LOGIN_STATE = LOGIN_STATE.LOGOUT;
            _loginViewModel.ROLE = ROLE.NO_PERMISSION;
        }
    }
}
