using NTech.Xm.Commons.Defines;
using NTech.Xm.Database.Models;
using NTech.Xm.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NTech.Base.DataBase.Utils;
using NTech.Xm.Station.Command;

namespace NTech.Xm.Station.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly string DBName = "XmDb";

        public delegate void LoginHandler(object sender, LOGIN_STATE loginState, ROLE role);
        public event LoginHandler LoginEvent;

        private readonly TaskManagerDB _taskManagerDB;
        public static LoginViewModel Instance { get; private set; }
        public LoginViewModel()
        {
            if (Instance is null)
            {
                Instance = this;
            }
            else
                return;

            this._taskManagerDB = new TaskManagerDB();
            this.RolesModelList = new List<RolesModel>();
            this._roles = new List<string>();
            this.SelectRoles();

            this.LoginCmd = new LoginCmd(this);
            this.LogoutCmd = new LogoutCmd(this);
        }
        public List<RolesModel> RolesModelList { get; set; }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                Set(ref _userName, value);
            }
        }
        private string _passCode;
        public string Passcode
        {
            get => _passCode;
            set
            {
                Set(ref _passCode, value);
            }
        }
        private List<string> _roles;
        public List<string> Roles
        {
            get => _roles;
            set
            {
                Set(ref _roles, value);
            }
        }

        private LOGIN_STATE _loginState;
        public LOGIN_STATE LOGIN_STATE
        {
            get => _loginState;
            set
            {
                Set(ref _loginState, value);
                switch (_loginState)
                {
                    case LOGIN_STATE.LOGOUT:
                        this.LoginEvent?.Invoke(this, LOGIN_STATE.LOGOUT, ROLE.NO_PERMISSION);
                        break;
                    case LOGIN_STATE.LOGIN_SUCCESS:
                        this.LoginEvent?.Invoke(this, _loginState, ROLE);
                        this.UserName = null;
                        this.Passcode = string.Empty;
                        break;
                    case LOGIN_STATE.LOGIN_FAILED:
                        this.LoginEvent?.Invoke(this, LOGIN_STATE.LOGIN_FAILED, ROLE.NO_PERMISSION);
                        break;
                }

            }
        }
        public ROLE ROLE { get; set; }

        public void SelectRoles()
        {
            var database = _taskManagerDB.SelectRoles(DBName);
            var listRole = database?.DataSet?.Tables[0].ToList<RolesModel>();
            if (listRole != null)
            {
                RolesModelList = listRole;
                foreach (var item in listRole)
                {
                    Roles.Add(item.Username);
                }
            }
        }
        public bool CanLogin()
        {
            return !string.IsNullOrEmpty(UserName);
        }
        public ICommand LoginCmd { get; }
        public ICommand LogoutCmd { get; }
    }
}
