using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NTech.Base.Wpf.Controls.ObjectEditor.Adorners
{
    public class SelectionAdorner : Adorner
    {
        public bool IsCancel { get; set; }

        private Point _startPoint;
        private Point _movedPoint;

        public Brush SelectionBrush { get; set; }
        public Pen SelectionBorder { get; set; }


        public SelectionAdorner(UIElement adornerElement, Point startPoint)
            : base(adornerElement)
        {
            _startPoint = startPoint;
        }

        public void UpdatePosition(Point position)
        {
            _movedPoint = position;
            this.InvalidateVisual();
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            this.SelectionRect = new Rect(_startPoint, _movedPoint);
            dc.DrawRectangle(SelectionBrush, SelectionBorder, SelectionRect);
        }

        public Rect SelectionRect { get; private set; }
    }
}
