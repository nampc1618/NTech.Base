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
    public class LoginCmd : CommandBase
    {
        private readonly LoginViewModel _loginViewModel;
        public LoginCmd(LoginViewModel loginViewModel)
        {
            this._loginViewModel = loginViewModel;
        }
        public override void Execute(object parameter)
        {
            if(_loginViewModel.CanLogin())
            {
                var Operator = _loginViewModel.RolesModelList.Where(x=>x.Username.Equals("Operator")).ToList().FirstOrDefault();
                var Admin = _loginViewModel.RolesModelList.Where(x => x.Username.Equals("Admin")).ToList().FirstOrDefault();
                var SuperAdmin = _loginViewModel.RolesModelList.Where(x => x.Username.Equals("SuperAdmin")).ToList().FirstOrDefault();

                if(_loginViewModel.UserName == Operator.Username && _loginViewModel.Passcode == Operator.Passcode)
                {
                    _loginViewModel.ROLE = ROLE.OPERATOR;
                    _loginViewModel.LOGIN_STATE = LOGIN_STATE.LOGIN_SUCCESS;
                    
                }
                else if(_loginViewModel.UserName == Admin.Username && _loginViewModel.Passcode == Admin.Passcode)
                {
                    _loginViewModel.ROLE = ROLE.ADMIN;
                    _loginViewModel.LOGIN_STATE = LOGIN_STATE.LOGIN_SUCCESS;
                   
                }
                else if (_loginViewModel.UserName == SuperAdmin.Username && _loginViewModel.Passcode == SuperAdmin.Passcode)
                {
                    _loginViewModel.ROLE = ROLE.SUPERADMIN;
                    _loginViewModel.LOGIN_STATE = LOGIN_STATE.LOGIN_SUCCESS;
                }
                else
                {
                    _loginViewModel.ROLE = ROLE.NO_PERMISSION;
                    _loginViewModel.LOGIN_STATE = LOGIN_STATE.LOGIN_FAILED;
                    MessageBox.Show("Sai mật khẩu");
                }
            }
            else
            {
                MessageBox.Show("Chưa chọn tên đăng nhập");
            }
        }
    }
}
