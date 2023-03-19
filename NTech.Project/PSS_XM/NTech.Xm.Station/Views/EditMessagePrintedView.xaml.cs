using MahApps.Metro.Controls;
using NTech.Xm.Station.ViewModels;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for EditMessagePrintedView.xaml
    /// </summary>
    public partial class EditMessagePrintedView : MetroWindow
    {
        public EditMessagePrintedView()
        {
            InitializeComponent();
            this.DataContext = EditMessagePrintedViewModel.Instance;
        }

        private void chk1_Checked(object sender, RoutedEventArgs e)
        {
            EditMessagePrintedViewModel.Instance.Offset1 = EditMessagePrintedViewModel.Instance.MessagesDetailModel.NumberBagsPrinted - EditMessagePrintedViewModel.Instance.MessagesDetailModel.NumberBags;
        }

        private void chk2_Checked(object sender, RoutedEventArgs e)
        {
            EditMessagePrintedViewModel.Instance.Offset2 = EditMessagePrintedViewModel.Instance.MessagesDetailModel.NumberBags - EditMessagePrintedViewModel.Instance.MessagesDetailModel.NumberBagsPrinted;
        }

        private void wdEditMsg_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            EditMessagePrintedViewModel.Instance.UseCase1 = false;
            EditMessagePrintedViewModel.Instance.UseCase2= false;
            EditMessagePrintedViewModel.Instance.Offset1 = 0;
            EditMessagePrintedViewModel.Instance.Offset2 = 0;
            EditMessagePrintedViewModel.Instance.ReasonCase1 = null;
            EditMessagePrintedViewModel.Instance.ReasonCase2= null;
            EditMessagePrintedViewModel.Instance.EditMessagePrintedView= null;
        }
    }
}
