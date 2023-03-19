using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace NTech.Xm.Gate.Views
{
    /// <summary>
    /// Interaction logic for TestView.xaml
    /// </summary>
    public partial class TestView : MetroWindow, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        //List<InfoCustomer> lstCus = null;

        public TestView()
        {
            InitializeComponent();

            this.DataContext = this;

            //lstCus = new List<InfoCustomer>();
            //lstCus.Add(new InfoCustomer() { CustomerName = "Khach A", CustomerCode = "001" });
            //lstCus.Add(new InfoCustomer() { CustomerName = "Khach B", CustomerCode = "002" });
            //lstCus.Add(new InfoCustomer() { CustomerName = "Khach C", CustomerCode = "003" });

            //ListInfoCus = lstCus;
            //dgTest.ItemsSource = ListInfoCus;
            //dgTest1.ItemsSource = ListInfoCus;
        }
        //public List<InfoCustomer> ListInfoCus
        //{
        //    get
        //    {
        //        return lstCus;
        //    }
        //    set
        //    {
        //        lstCus = value;
        //        OnPropertyChanged(nameof(ListInfoCus));
        //    }
        //}
        //public class InfoCustomer
        //{
        //    public string CustomerName { get; set; }
        //    public string CustomerCode { get; set; }
        //}

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            //ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            //if (toggleSwitch != null)
            //{
            //    if (toggleSwitch.IsOn == true)
            //    {
            //        progress.IsActive = true;
            //        progress.Visibility = Visibility.Visible;
            //    }
            //    else
            //    {
            //        progress.IsActive = false;
            //        progress.Visibility = Visibility.Collapsed;
            //    }
            //}
        }
    }
}
