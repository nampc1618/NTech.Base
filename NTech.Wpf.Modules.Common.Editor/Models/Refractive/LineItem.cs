using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NTech.Base.Commons.BaseModel;
using NTech.Base.Wpf.Controls.ObjectEditor.Models;

namespace NTech.Wpf.Modules.Common.Editor.Models.Refractive
{
    /// <summary>
    /// [NCS-3103] : Refractive index 창에서 scan image zoom in/out 기능 구현되지 않음
    /// </summary>
    public class LineItem : BaseModel
    {
        private UIEditableBounds _layoutBounds;
        /// <summary>
        /// LayoutBounds
        /// </summary>
        public UIEditableBounds LayoutBounds
        {
            get
            {
                if (this._layoutBounds == null) { this._layoutBounds = new UIEditableBounds(); }
                return this._layoutBounds;
            }
            set
            {
                this._layoutBounds = value;
                this.RaisePropertyChanged();
            }
        }

        private Visibility _visibility = Visibility.Collapsed;
        /// <summary>
        /// Visibility
        /// </summary>
        public Visibility Visibility
        {
            get { return this._visibility; }
            set
            {
                this._visibility = value;
                this.RaisePropertyChanged();
            }
        }
    }
}
