
using NTech.Base.Wpf.Controls.ObjectEditor.Models;
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
    public class MoveAdorner : Adorner
    {
        private Point _startPoint;
        private List<IUIEditableObject> _items;
        private double _zoomRatio;

        public bool IsCancel { get; set; }
        ResizerTypes _resizer;

        public MoveAdorner(UIElement adornerElement, List<IUIEditableObject> items, Point startPoint, ResizerTypes resizer, double zoomRatio)
            : base(adornerElement)
        {
            _startPoint = startPoint;
            _items = items;
            _zoomRatio = zoomRatio;
            _resizer = resizer;
        }

        public void BeginMoves()
        {
            if (_items != null)
            {
                foreach (var item in _items)
                {
                    item.LayoutBounds.BeginEdit();
                }
            }
        }

        public void EndMoves()
        {
            if (_items != null)
            {
                foreach (var item in _items)
                {
                    item.LayoutBounds.EndEdit(this.IsCancel);
                }
            }
        }

        public void UpdatePosition(Point position)
        {
            if (_items != null)
            {
                var gap = position - _startPoint;

                foreach (var item in _items)
                {
                    switch (_resizer)
                    {
                        case ResizerTypes.None:
                            {
                                item.LayoutBounds.EditedX = item.LayoutBounds.X + (gap.X / _zoomRatio);
                                item.LayoutBounds.EditedY = item.LayoutBounds.Y + (gap.Y / _zoomRatio);
                            }
                            break;
                        case ResizerTypes.TopLeft:
                        case ResizerTypes.MiddleLeft:
                        case ResizerTypes.TopCenter:
                            {
                                var x1 = item.LayoutBounds.X + (gap.X / _zoomRatio);
                                var y1 = item.LayoutBounds.Y + (gap.Y / _zoomRatio);

                                var x2 = item.LayoutBounds.X + item.LayoutBounds.Width; 
                                var y2 = item.LayoutBounds.Y + item.LayoutBounds.Height;

                                if (_resizer == ResizerTypes.MiddleLeft)
                                {
                                    y1 = item.LayoutBounds.Y;
                                }
                                if (_resizer == ResizerTypes.TopCenter)
                                {
                                    x1 = item.LayoutBounds.X;
                                }

                                var rect = new Rect(new Point(x1, y1), new Point(x2, y2));

                                item.LayoutBounds.EditedX = rect.X;
                                item.LayoutBounds.EditedY = rect.Y;
                                item.LayoutBounds.EditedWidth = rect.Width;
                                item.LayoutBounds.EditedHeight = rect.Height;
                            }
                            break;
                        case ResizerTypes.BottomRight:
                        case ResizerTypes.MiddleRight:
                        case ResizerTypes.BottomCenter:
                            {
                                var x1 = item.LayoutBounds.X;
                                var y1 = item.LayoutBounds.Y;

                                var x2 = item.LayoutBounds.X + item.LayoutBounds.Width + (gap.X / _zoomRatio);
                                var y2 = item.LayoutBounds.Y + item.LayoutBounds.Height + (gap.Y / _zoomRatio);

                                if (_resizer == ResizerTypes.BottomCenter)
                                {
                                    x2 = item.LayoutBounds.X + item.LayoutBounds.Width;
                                }
                                if (_resizer == ResizerTypes.MiddleRight)
                                {
                                    y2 = item.LayoutBounds.Y + item.LayoutBounds.Height;
                                }

                                var rect = new Rect(new Point(x1, y1), new Point(x2, y2));

                                item.LayoutBounds.EditedX = rect.X;
                                item.LayoutBounds.EditedY = rect.Y;
                                item.LayoutBounds.EditedWidth = rect.Width;
                                item.LayoutBounds.EditedHeight = rect.Height;
                            }
                            break;

                        case ResizerTypes.TopRight:
                            {
                                var x1 = item.LayoutBounds.X;
                                var y1 = item.LayoutBounds.Y + (gap.Y / _zoomRatio);

                                var x2 = item.LayoutBounds.X + item.LayoutBounds.Width + (gap.X / _zoomRatio);
                                var y2 = item.LayoutBounds.Y + item.LayoutBounds.Height;

                                var rect = new Rect(new Point(x1, y1), new Point(x2, y2));

                                item.LayoutBounds.EditedX = rect.X;
                                item.LayoutBounds.EditedY = rect.Y;
                                item.LayoutBounds.EditedWidth = rect.Width;
                                item.LayoutBounds.EditedHeight = rect.Height;
                            }
                            break;

                        case ResizerTypes.BottomLeft:
                            {
                                var x1 = item.LayoutBounds.X + (gap.X / _zoomRatio);
                                var y1 = item.LayoutBounds.Y;

                                var x2 = item.LayoutBounds.X + item.LayoutBounds.Width;
                                var y2 = item.LayoutBounds.Y + item.LayoutBounds.Height + (gap.Y / _zoomRatio);

                                var rect = new Rect(new Point(x1, y1), new Point(x2, y2));

                                item.LayoutBounds.EditedX = rect.X;
                                item.LayoutBounds.EditedY = rect.Y;
                                item.LayoutBounds.EditedWidth = rect.Width;
                                item.LayoutBounds.EditedHeight = rect.Height;
                            }
                            break;
                    }
                }
            }
        }
    }

    public class DragAndDropLocationAdorner : Adorner
    {
        public bool IsCancel { get; set; }

        public Point StartPoint { get; private set; }

        public DragAndDropLocationAdorner(UIElement adornerElement, Point startPoint)
            : base(adornerElement)
        {
            this.StartPoint = startPoint;
        }
    }
}
