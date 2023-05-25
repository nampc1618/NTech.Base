using NTech.Base.Commons.BaseCommand;
using PLCCommunication.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PLCCommunication.Commands
{
    public class StopCmd : CommandBase
    {
        private readonly PLCModel _plcModel;
        public StopCmd(PLCModel plcModel)
        {
            this._plcModel = plcModel;
        }
        public override void Execute(object parameter)
        {
            _plcModel.StopProcess();
        }
    }
}
