using PLCCommunication.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PLCCommunication
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
            MainWindow mainview = new MainWindow();
            PLCViewModel plcViewModel = new PLCViewModel(mainview.Dispatcher);

            MainViewModel mainViewModel = new MainViewModel(mainview.Dispatcher, mainview, plcViewModel);
            mainview.DataContext = mainViewModel;

            mainview.Show();
        }
    }
}
