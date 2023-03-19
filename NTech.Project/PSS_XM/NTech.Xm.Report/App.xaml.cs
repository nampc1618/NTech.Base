using ControlzEx.Theming;
using NTech.Xm.Report.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace NTech.Xm.Report
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
            var theme = ThemeManager.Current.AddTheme(new Theme("CustomLightMidnightBlue", "CustomLightMidnightBlue", "Dark", "CustomColor", Color.FromRgb(26, 31, 43), (SolidColorBrush)new BrushConverter().ConvertFromString("#1A1F2B"), true, false));
            ThemeManager.Current.ChangeTheme(this, theme);

            MainWindow mainView = new MainWindow();
            MainViewModel mainViewModel = new MainViewModel(mainView);

            mainView.DataContext = mainViewModel;
            mainView.Show();
        }
    }
}
