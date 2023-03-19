using BespokeFusion;
using NTech.Base.Commons.BaseCommand;
using NTech.Xm.Station.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Xm.Station.Command
{
    public class SaveEditPrintedCmd : CommandBase
    {
        private readonly EditMessagePrintedViewModel _editMessagePrintedViewModel;
        public SaveEditPrintedCmd(EditMessagePrintedViewModel editMessagePrintedViewModel) 
        { 
            this._editMessagePrintedViewModel = editMessagePrintedViewModel;
        }
        public override void Execute(object parameter)
        {
            string note = string.Empty;

            if ((bool)parameter)
            {
                note = $"[Đã chỉnh sửa lúc: {DateTime.Now.ToString("HH:mm:ss dd-MM-yyyy")}] - [TH1: Số đếm THỪA so với đơn hàng] - [Lý do: {_editMessagePrintedViewModel.ReasonCase1?.ToUpper()}] - [Số lượng bù: {_editMessagePrintedViewModel.Offset1} bao]";
            }
            else
            {
                note = $"[Đã chỉnh sửa lúc: {DateTime.Now.ToString("HH:mm:ss dd-MM-yyyy")}] - [TH2: Số đếm THIẾU so với đơn hàng] - [Lý do: {_editMessagePrintedViewModel.ReasonCase2?.ToUpper()}] - [Số lượng bù: {_editMessagePrintedViewModel.Offset2} bao]";
            }

            _editMessagePrintedViewModel.MessagesDetailModel.Note = note;
            
            _editMessagePrintedViewModel.UpdateNoteForMessageDetail();
            MainViewModel.Instance.SelectMessagesDetail_NewAndAll(DateTime.Now.ToString("dd-MM-yyyy"));
            MaterialMessageBox.Show("Chỉnh sửa thành công!");
            _editMessagePrintedViewModel.EditMessagePrintedView.Close();
        }
    }
}
