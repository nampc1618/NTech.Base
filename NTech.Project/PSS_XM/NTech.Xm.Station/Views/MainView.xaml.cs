using MahApps.Metro.Controls;
using NTech.Xm.Database.Models;
using NTech.Xm.Station.Commons.Defines;
using NTech.Xm.Station.Models;
using NTech.Xm.Station.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace NTech.Xm.Station.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : MetroWindow
    {
        public MainView()
        {
            InitializeComponent();
        }


        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void dgPrintedMessages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainViewModel.Instance.IsReprint = false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.popupDG.IsOpen = false;
        }

        private void MetroTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
                MainViewModel.Instance.IsReprint = false;
        }

        private void Flyout_IsOpenChanged(object sender, RoutedEventArgs e)
        {

        }

        private void popupImgPrinter_Closed(object sender, EventArgs e)
        {
            MainViewModel.Instance.PrinterViewModel.IsManual = false;
        }

        private void txtSearchNewMsg_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(txtSearchNewMsg.Text == string.Empty)
            {
                MainViewModel.Instance.NewMessagesDetailList = MainViewModel.Instance.NewMessagesDetailListClone;
                return;
            }
            var filter = MainViewModel.Instance.NewMessagesDetailListClone.Where(x => x.CouponCode.StartsWith(txtSearchNewMsg.Text)).ToList();
            ObservableCollection<MessagesDetailModel> obMsg = new ObservableCollection<MessagesDetailModel>(filter);
            MainViewModel.Instance.NewMessagesDetailList = obMsg;
        }

        private void txtSearchPrintedMsg_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSearchPrintedMsg.Text == string.Empty)
            {
                MainViewModel.Instance.AllMessagesDetailList = MainViewModel.Instance.AllMessagesDetailListClone;
                return;
            }
            var filter = MainViewModel.Instance.AllMessagesDetailListClone.Where(x => x.CouponCode.StartsWith(txtSearchPrintedMsg.Text)).ToList();
            ObservableCollection<MessagesDetailModel> obMsg = new ObservableCollection<MessagesDetailModel>(filter);
            MainViewModel.Instance.AllMessagesDetailList = obMsg;
        }
    }
}
