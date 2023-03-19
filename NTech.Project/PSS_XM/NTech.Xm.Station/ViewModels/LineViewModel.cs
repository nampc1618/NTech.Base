using NTech.Xm.Commons.Defines;
using NTech.Xm.Station.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Xm.Station.ViewModels
{
    public class LineViewModel : ViewModelBase
    {
        #region Lines
        public LINE LINE1 { get; }
        public LINE LINE2 { get; }
        public LINE LINE3 { get; }
        #endregion

        public static LineViewModel Instance { get; private set; }
        public LineViewModel()
        {
            if (Instance is null)
            {
                Instance = this;
            }
            else
            {
                return;
            }
            
            this.LINE1 = new LINE() { LineName = "LINE 1", Tag = 1 };
            this.LINE2 = new LINE() { LineName = "LINE 2", Tag = 2 };
            this.LINE3 = new LINE() { LineName = "LINE 3", Tag = 3 };

            this.IPLocalMachine = CommonDefines.GetLocalIPv4Addresses();
            this.AddLINEList();
            this.AddLinesName();
        }

        #region Properties
        List<LINE> _lineList;
        public List<LINE> LINEList
        {
            get { return _lineList; }
            set
            {
                Set(ref _lineList, value);
            }
        }

        private LINE _lineSelected;
        public LINE LINESelected
        {
            get { return _lineSelected; }
            set
            {
                Set(ref _lineSelected, value);
            }
        }

        private List<string> _linesName;
        public List<string> LINESName
        {
            get { return _linesName; }
            set
            {
                Set(ref _linesName, value);
            }
        }
        public string IPLocalMachine { get; set; }
        public LINE LINEByUse { get; set; }
        #endregion

        #region Methods
        void AddLINEList()
        {
            this._lineList = new List<LINE>();

            if (this.IPLocalMachine.Equals(CommonDefines.IP_PC_LINE2))
            {
                this.LINEList.Add(LINE2);
                this.LINEByUse = this.LINE2;
            }
            else if(this.IPLocalMachine.Equals(CommonDefines.IP_PC_LINE3))
            {
                this.LINEList.Add(LINE3);
                this.LINEByUse = this.LINE3;
            }
            //this.LINEList.Add(LINE1);
        }
        void AddLinesName()
        {
            this._linesName = new List<string>();

            this.LINESName.Add(LINE1.LineName);
            this.LINESName.Add(LINE2.LineName);
            this.LINESName.Add(LINE3.LineName);
        }
        #endregion
    }
}
