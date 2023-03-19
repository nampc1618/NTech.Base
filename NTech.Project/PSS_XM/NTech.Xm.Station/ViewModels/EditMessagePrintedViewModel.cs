using NTech.Xm.Database;
using NTech.Xm.Database.Models;
using NTech.Xm.Station.Command;
using NTech.Xm.Station.Commons.Defines;
using NTech.Xm.Station.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace NTech.Xm.Station.ViewModels
{
    public class EditMessagePrintedViewModel : ViewModelBase
    {
        private readonly string DBName = "XmDb";
        private readonly TaskManagerDB _taskManagerDB;
        MessagesDetailModel _messagesDetailModel;
        public static EditMessagePrintedViewModel Instance { get; private set; }
        public EditMessagePrintedView EditMessagePrintedView { get; set; }
        public MainViewModel MainViewModel { get; }
        public EditMessagePrintedViewModel()
        {
            if (Instance is null)
            {
                Instance = this;
            }
            else
            {
                return;
            }
            
            this._taskManagerDB = new TaskManagerDB();
            this.SaveEditPrintedCmd = new SaveEditPrintedCmd(this);
            this._messagesDetailModel = new MessagesDetailModel();
            this._listState = new List<string>();

            _listState.Add(Define.GetEnumDescription(MESSAGE_STATE.NOT_YET_PRINT));
            _listState.Add(Define.GetEnumDescription(MESSAGE_STATE.PRINTING));
            _listState.Add(Define.GetEnumDescription(MESSAGE_STATE.NOT_PRINT_DONE));
            _listState.Add(Define.GetEnumDescription(MESSAGE_STATE.PRINT_DONE));

            this.ListReason1 = new List<string>()
            {
            "Bù rách vỡ",
            "Bù theo quy chế",
            "Bộ đếm lỗi",
            "Cấp đi làm mẫu"
            };
            this.ListReason2 = new List<string>()
            {
            "Bộ đếm lỗi"
            };

        }
        public List<string> ListReason1 { get; set; }
        public List<string> ListReason2 { get; set; }

        public List<string> _listState;
        public List<string> ListState
        {
            get { return _listState; }
            set
            {
                Set(ref _listState, value);
            }
        }
        private int _offet1;
        public int Offset1
        {
            get => _offet1;
            set
            {
                Set(ref _offet1, value);
            }
        }
        private int _offet2;
        public int Offset2
        {
            get => _offet2;
            set
            {
                Set(ref _offet2, value);
            }
        }
        private string _reasonCase1;
        public string ReasonCase1
        {
            get => _reasonCase1;
            set => Set(ref _reasonCase1, value);
        }
        private string _reasonCase2;
        public string ReasonCase2
        {
            get => _reasonCase2;
            set => Set(ref _reasonCase2, value);
        }

        public MessagesDetailModel MessagesDetailModel
        {
            get { return _messagesDetailModel; }
            set
            {
                if (Set(ref _messagesDetailModel, value))
                {
                    if (this._messagesDetailModel != null)
                    {
                        if (this._messagesDetailModel.NumberBags < this._messagesDetailModel.NumberBagsPrinted)
                        {
                            this.UseCase1 = true;
                        }
                        else
                        {
                            this.UseCase2 = true;
                        }
                    }
                }

            }
        }
        private bool _useCase1;
        public bool UseCase1
        {
            get => _useCase1;
            set
            {
                Set(ref _useCase1, value);
            }
        }
        private bool _useCase2;
        public bool UseCase2
        {
            get => _useCase2;
            set
            {
                Set(ref _useCase2, value);
            }
        }
        public void UpdateNoteForMessageDetail()
        {
            _taskManagerDB.UpdateNoteForMessageDetail(DBName, this.MessagesDetailModel);
        }
        public ICommand SaveEditPrintedCmd { get; }
    }
}
