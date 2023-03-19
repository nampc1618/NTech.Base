using NTech.Base.Commons.BaseCommand;
using NTech.Xm.Database.Models;
using NTech.Xm.Station.Commons.Defines;
using NTech.Xm.Station.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Xm.Station.Command
{
    public class PushMessageAndContentLEDCmd : CommandBase
    {
        private readonly LineViewModel _lineViewModel;
        private readonly PrinterViewModel _printerViewModel;
        private readonly TroughViewModel _troughViewModel;

        public PushMessageAndContentLEDCmd(LineViewModel lineViewModel, PrinterViewModel printerViewModel, TroughViewModel troughViewModel)
        {
            _lineViewModel = lineViewModel;
            _printerViewModel = printerViewModel;
            _troughViewModel = troughViewModel;
        }
        public async override void Execute(object parameter)
        {
            _printerViewModel.IsManual = false;
            _printerViewModel.PRINTERSelected.TroughByUse = _troughViewModel.TROUGHSelected;

            MainViewModel.Instance.MessagesDetailModel.Line = _lineViewModel.LINESelected.LineName;
            MainViewModel.Instance.MessagesDetailModel.Printer = _printerViewModel.PRINTERSelected.PrinterName;
            MainViewModel.Instance.MessagesDetailModel.Trough = _printerViewModel.PRINTERSelected.TroughByUse.TroughName;
            MainViewModel.Instance.MessagesDetailModel.MessageState = Define.GetEnumDescription(MESSAGE_STATE.PRINTING);
            MainViewModel.Instance.IsCanStart = false;
            _printerViewModel.PRINTERSelected.MessagesDetailModel =  (MessagesDetailModel)MainViewModel.Instance.MessagesDetailModel.Clone();
            _printerViewModel.PRINTERSelected.MessageName = $"THANHTHANGMSG";

            
            await _troughViewModel.SetTextToAllLEDPanel(_printerViewModel.PRINTERSelected,
                                                        _printerViewModel.PRINTERSelected.LEDTableFixed,
                                                        _printerViewModel.PRINTERSelected.TroughByUse,
                                                        false,
                                                        _printerViewModel.PRINTERSelected.LEDTableFixed.Text1Info,
                                                        _printerViewModel.PRINTERSelected.LEDTableFixed.Text2Info,
                                                        _printerViewModel.PRINTERSelected.LEDTableFixed.Text3Info);

            _printerViewModel.StepsPerform(_printerViewModel.PRINTERSelected, STEPS_PERFORM.DELETE_MESSAGE_DATA);
        }
    }
}
