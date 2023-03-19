using ControlzEx.Theming;
using NTech.Xm.Gate.Controls;
using NTech.Xm.Gate.ViewModels;
using NTech.Xm.Gate.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace NTech.Xm.Gate
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var theme = ThemeManager.Current.AddTheme(new Theme("CustomLightMidnightBlue", "CustomLightMidnightBlue", "Light", "MidnightBlue", Colors.MidnightBlue, Brushes.MidnightBlue, true, false)); ;
            ThemeManager.Current.ChangeTheme(this, theme);
            //ThemeManager.Current.AddTheme(RuntimeThemeGenerator.Current.GenerateRuntimeTheme("Light", Colors.Red));

            //TestView testView = new TestView();
            //testView.Show();

            LoginViewModel loginViewModel = new LoginViewModel();

            SettingsView settingsView = new SettingsView();
            SettingsViewModel settingsViewModel = new SettingsViewModel(settingsView, loginViewModel);

            MainView mainView = new MainView();
            MainViewModel mainViewModel = new MainViewModel(mainView.Dispatcher, mainView, loginViewModel, settingsViewModel);

            mainView.DataContext = mainViewModel;
            
            mainView.Show();
        }
    }
}
