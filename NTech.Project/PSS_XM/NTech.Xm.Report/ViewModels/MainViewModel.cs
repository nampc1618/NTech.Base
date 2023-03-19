using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Xm.Report.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public static MainViewModel Instance { get; private set; }
        public MainWindow MainWindow { get; private set; }
        public MainViewModel(MainWindow mainWindow)
        {
            if (Instance is null)
                Instance = this;
            else
                return;

            this.MainWindow = mainWindow;
        }
    }
}
