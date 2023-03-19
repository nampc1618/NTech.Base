using NTech.Base.Commons.BaseCommand;
using NTech.Xm.Station.ViewModels;
using NTech.Xm.Station.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Xm.Station.Command
{
    public class EditMessagePrintedCmd : CommandBase
    {
        private readonly EditMessagePrintedViewModel _editMessagePrintedViewModel;
        public EditMessagePrintedCmd(EditMessagePrintedViewModel editMessagePrintedViewModel)
        {
            this._editMessagePrintedViewModel = editMessagePrintedViewModel;
        }
        public override void Execute(object parameter)
        {
            EditMessagePrintedView editMessagePrintedView = new EditMessagePrintedView();
            _editMessagePrintedViewModel.EditMessagePrintedView = editMessagePrintedView;
            _editMessagePrintedViewModel.EditMessagePrintedView.Title = $"{_editMessagePrintedViewModel.MessagesDetailModel.Printer} - Bản tin khách hàng {_editMessagePrintedViewModel.MessagesDetailModel.CustomerName}";
            _editMessagePrintedViewModel.EditMessagePrintedView.ShowDialog();
        }
    }
}
