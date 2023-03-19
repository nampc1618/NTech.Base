using ControlzEx.Theming;
using NTech.Xm.Station.ViewModels;
using NTech.Xm.Station.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace NTech.Xm.Station
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
            //var theme = ThemeManager.Current.AddTheme(new Theme("CustomLightMidnightBlue", "CustomLightMidnightBlue", "Light", "MidnightBlue", Colors.MidnightBlue, Brushes.MidnightBlue, true, false));
            var theme = ThemeManager.Current.AddTheme(new Theme("CustomLightMidnightBlue", "CustomLightMidnightBlue", "Light", "CustomColor", Color.FromRgb(26, 31, 43), (SolidColorBrush)new BrushConverter().ConvertFromString("#1A1F2B"), true, false));
            //var theme = ThemeManager.Current.AddTheme(new Theme("CustomLightMidnightBlue", "CustomLightMidnightBlue", "Light", "CustomColor", Color.FromRgb(27, 79, 114), (SolidColorBrush)new BrushConverter().ConvertFromString("#1B4F72"), true, false));
            ThemeManager.Current.ChangeTheme(this, theme);

            //ThemeManager.Current.AddTheme(RuntimeThemeGenerator.Current.GenerateRuntimeTheme("Light", Colors.Red));

            MainView mainView = new MainView();
            LoginViewModel loginViewModel = new LoginViewModel();

            #region initialzation order
            LineViewModel lineViewModel = new LineViewModel(); //Step 1
            PrinterViewModel printerViewModel = new PrinterViewModel(lineViewModel, mainView.Dispatcher); // Step 2
            TroughViewModel troughViewModel = new TroughViewModel(printerViewModel); // Step 3
            SettingsViewModel settingsViewModel = new SettingsViewModel(loginViewModel, lineViewModel, printerViewModel, troughViewModel);
            EditMessagePrintedViewModel editMessagePrintedViewModel = new EditMessagePrintedViewModel();
            MainViewModel mainViewModel = new MainViewModel(mainView.Dispatcher, mainView, loginViewModel,
                                                            lineViewModel, printerViewModel, troughViewModel, settingsViewModel, editMessagePrintedViewModel); // Step 4
            
            #endregion

            mainView.DataContext = mainViewModel;
            

            mainView.Show();
        }
    }
}
