using Kis.Toolkit;
using NTech.Xm.Commons.Defines;
using NTech.Xm.Station.Command;
using NTech.Xm.Station.Commons.Defines;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;

namespace NTech.Xm.Station.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        XmlManagement xmlManagement;
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

            xmlManagement = new XmlManagement();
            xmlManagement.Load(Define.PlcPath);
            UsePLC = xmlManagement.SelectSingleNode("//PLC/UsePLC").InnerText.Trim();
            if (UsePLC.Equals("true"))
                _isUsePLC = true;
            else
                _isUsePLC = false;

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

        private string _usePLC;
        public string UsePLC
        {
            get => _usePLC;
            set
            {
                if (Set(ref _usePLC, value))
                {

                }
            }
        }
        private bool _isUsePLC;
        public bool IsUsePLC
        {
            get => _isUsePLC;
            set
            {
                if (Set(ref _isUsePLC, value))
                {
                    XmlNode nodeUsePLC = xmlManagement.SelectSingleNode("//PLC/UsePLC");
                    if (_isUsePLC)
                    {
                        xmlManagement.SetNodeValueFromNode(nodeUsePLC, "true");
                    }
                    else
                    {
                        xmlManagement.SetNodeValueFromNode(nodeUsePLC, "false");
                    }
                    if (xmlManagement.Save(Define.PlcPath))
                    {
                        MessageBox.Show("Done!");
                    }
                }
            }
        }
        public void RunScript(string path)
        {
            Process.Start(new ProcessStartInfo("Powershell.exe", path) { UseShellExecute = true });
        }
        public ICommand CheckAllStatusLEDTableFixedCmd { get; }
        public ICommand CheckSingleStatusLEDTableFixedCmd { get; }
        public ICommand SaveMsgParamCmd { get; }
    }
}
