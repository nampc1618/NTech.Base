using BespokeFusion;
using NTech.Base.Commons.BaseCommand;
using NTech.Xm.Gate.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Xm.Gate.Command
{
    public class DeleteMsgNewCmd : CommandBase
    {
        private readonly MainViewModel _mainViewModel;
        public DeleteMsgNewCmd(MainViewModel mainViewModel)
        {
            this._mainViewModel = mainViewModel;
        }

        public override void Execute(object parameter)
        {
            if(MaterialMessageBox.ShowWithCancel("Chắc chắn xóa bản tin này", true, false) == System.Windows.MessageBoxResult.OK)
            {
                _mainViewModel.DeleteMessagesDetail();
                _mainViewModel.SelectMessagesDetail_New(DateTime.Now.ToString("dd-MM-yyyy"));
                _mainViewModel.ServerSendToClient("DeM");
            }
        }
    }
}
