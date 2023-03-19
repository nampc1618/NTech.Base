using NTech.Base.Wpf.Controls.ObjectEditor.Adorners;
using NTech.Base.Wpf.Controls.ObjectEditor.Models;
using NTech.Base.Wpf.Helper;
using NTech.Base.Wpf.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace NTech.Base.Wpf.Controls.ObjectEditor
{
    public static class ResizerTypesExtension
    {
        public static bool IsComponentResizer(this ResizerTypes resizer)
        {
            switch (resizer)
            {
                case ResizerTypes.Component_All:
                    return true;
            }
            return false;
        }
    }
    public enum ResizerTypes
    {
        None,

        TopLeft,
        TopCenter,
        TopRight,

        MiddleLeft,
        MiddleRight,

        BottomLeft,
        BottomCenter,
        BottomRight,

        Rotation,
        
        Width,
        Height,
        OffsetX,
        StartX,
        Component_All,
        Ignore_Frame,
        Ignore_Ring,

        #region Underfill (Div)
        //Underfill
        Underfill_T,
        Underfill_R,
        Underfill_B,
        Underfill_L,

        // UnderfillDiv - HeightItem - BaseRegion
        UnderfillDiv_BaseRegionMiddleLeft,
        UnderfillDiv_BaseRegionMiddleRight,
        UnderfillDiv_BaseRegionTopCenter,
        UnderfillDiv_BaseRegionBottomCenter,
        UnderfillDiv_BaseRegionTopLeft,
        UnderfillDiv_BaseRegionBottomRight,
        UnderfillDiv_BaseRegionTopRight,
        UnderfillDiv_BaseRegionBottomLeft,
        UnderfillDiv_BaseRegionRotation,
        BaseRegionRotation,

        UnderfillDivHeight_T,
        UnderfillDivHeight_R,
        UnderfillDivHeight_B,
        UnderfillDivHeight_L,
        UnderfillDivWidth_T,
        UnderfillDivWidth_R,
        UnderfillDivWidth_B,
        UnderfillDivWidth_L,

        UnderfillDivHeight_Line,
        UnderfillDivHeight_Angle,

        UnderfillRotate,
        UnderfillTopLeft,
        UnderfillTopCenter,
        UnderfillTopRight,

        UnderfillMiddleLeft,
        UnderfillMiddleRight,

        UnderfillBottomLeft,
        UnderfillBottomCenter,
        UnderfillBottomRight,
        #endregion

        #region Drop
        Drop_T,
        Drop_R,
        Drop_B,
        Drop_L,

        // Drop - HeightItem - BaseRegion
        Drop_BaseRegionMiddleLeft,
        Drop_BaseRegionMiddleRight,
        Drop_BaseRegionTopCenter,
        Drop_BaseRegionBottomCenter,
        Drop_BaseRegionTopLeft,
        Drop_BaseRegionBottomRight,
        Drop_BaseRegionTopRight,
        Drop_BaseRegionBottomLeft,
        Drop_BaseRegionRotation,

        DropHeight_T,
        DropHeight_R,
        DropHeight_B,
        DropHeight_L,

        DropHeight_Line,
        DropHeight_Angle,

        DropRotate,
        DropTopLeft,
        DropTopCenter,
        DropTopRight,

        DropMiddleLeft,
        DropMiddleRight,

        DropBottomLeft,
        DropBottomCenter,
        DropBottomRight,
        #endregion

    }

    public class ObjectEditableItemsControl : ListBox
    {
        public double OuterZoomRatio
        {
            get { return (double)GetValue(OuterZoomRatioProperty); }
            set { SetValue(OuterZoomRatioProperty, value); }
        }
        public static readonly DependencyProperty OuterZoomRatioProperty =
            DependencyProperty.Register("OuterZoomRatio", typeof(double), typeof(ObjectEditableItemsControl), new PropertyMetadata(1.0d));


        public Panel OuterPanel
        {
            get { return (Panel)GetValue(OuterPanelProperty); }
            set { SetValue(OuterPanelProperty, value); }
        }
        public static readonly DependencyProperty OuterPanelProperty =
            DependencyProperty.Register("OuterPanel", typeof(Panel), typeof(ObjectEditableItemsControl), new PropertyMetadata(new PropertyChangedCallback(OnOuterPanelChanged)));

        private static void OnOuterPanelChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs e)
        {
            ObjectEditableItemsControl view = dobj as ObjectEditableItemsControl;
            if (view != null && e.NewValue is Panel)
            {
                view.OnOuterPanelChanged(e.OldValue as Panel, e.NewValue as Panel);
            }
        }

        private void OnOuterPanelChanged(Panel oldPanel, Panel newPanel)
        {
            if (oldPanel != null)
            {
                oldPanel.MouseLeftButtonDown -= NewPanel_PreviewMouseLeftButtonDown;
            }
            newPanel.MouseLeftButtonDown += NewPanel_PreviewMouseLeftButtonDown;
        }

        private void NewPanel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (VisualTreeHelperEx.FindAncestorByType<ObjectEditableItemsControl>(e.OriginalSource as FrameworkElement) != this)
            {
                if (this.IsHitTestVisible == true)
                {
                    this.OnPreviewMouseLeftButtonDown(e);
                }
            }
        }

        #region Drag Drop
        private Point _downPoint;
        private Point _donwPointOffset;
        private FrameworkElement _downContainer;
        private DateTime _downContainerCapturedTime;

        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        

        ResizerTypes _downResizer;

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            _downResizer = ResizerTypes.None;
            var ofe = e.OriginalSource as FrameworkElement;
            if (ofe != null)
            {
                Enum.TryParse<ResizerTypes>((ofe.Tag ?? "").ToString(), out _downResizer);
            }

            var container = CaptureContainer(e);

            if (container == null)
            {
                this.SelectedItem = null;
            }
            else if (this.SelectedItems.Count > 0 && container != null && this.SelectedItems.Contains(container.DataContext) == true)
            {
                e.Handled = true;
            }
            //if (container != null)
            //{
            //    if (Keyboard.IsKeyDown(Key.LeftCtrl) == true)
            //    {
            //        container.SetValue(ListBoxItem.IsSelectedProperty, true);
            //    }
            //    else
            //    {

            //        if (this.SelectedItems.Contains(container.DataContext) == false)
            //        {
            //            this.SelectedItem = container.DataContext;
            //        }
            //    }

            //}
            //else
            //{
            //    this.SelectedItem = null;
            //}

            //if (container == null)
            //{
            //    List<object> selectedTargets = new List<object>();
            //    var pos = e.GetPosition(this);
            //    var rx = pos.X / this.ActualWidth;
            //    var ry = pos.Y / this.ActualHeight;
            //    Rect selectionRange = new Rect(new Point(rx,ry), new Point(rx, ry));
            //    RangeSelection(selectionRange, selectedTargets);

            //    var find = selectedTargets.LastOrDefault();
            //    this.SelectedItem = find;
            //}

            //e.Handled = true;
        }



        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            this.ReleaseMouseCapture();
            _downContainer = null;
        }

        MoveAdorner _moveAdorner;
        SelectionAdorner _selectionAdorner;
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if (_downContainer != null)
                {
                    if (Mouse.Captured == this)
                    {
                        var gap = e.GetPosition(this) - _downPoint;
                        if ((DateTime.Now - _downContainerCapturedTime).TotalSeconds > 0.1d || gap.Length > 5)
                        {
                            try
                            {
                                var layoutPanel = GetItemsPanel();

                                List<FrameworkElement> selectedContainers = new List<FrameworkElement>();
                                foreach (var item in this.SelectedItems)
                                {
                                    var container = this.ItemContainerGenerator?.ContainerFromItem(item) as FrameworkElement;
                                    if (container != null)
                                    {
                                        selectedContainers.Add(container);
                                    }
                                }
                                var targetItems = selectedContainers.Select(f => f.DataContext).OfType<IUIEditableObject>().ToList();
                                _moveAdorner = new MoveAdorner(layoutPanel, targetItems, _downPoint, _downResizer, OuterZoomRatio);
                                _moveAdorner.BeginMoves();

                                switch (_downResizer)
                                {
                                    case ResizerTypes.BottomRight:
                                    case ResizerTypes.TopLeft:
                                        Mouse.OverrideCursor = Cursors.SizeNWSE;
                                        break;
                                    case ResizerTypes.MiddleRight:
                                    case ResizerTypes.MiddleLeft:
                                        Mouse.OverrideCursor = Cursors.SizeWE;
                                        break;
                                    case ResizerTypes.TopRight:
                                    case ResizerTypes.BottomLeft:
                                        Mouse.OverrideCursor = Cursors.SizeNESW;
                                        break;
                                    case ResizerTypes.TopCenter:
                                    case ResizerTypes.BottomCenter:
                                        Mouse.OverrideCursor = Cursors.SizeNS;
                                        break;
                                }

                                AdornerLayer.GetAdornerLayer(layoutPanel as Visual).Add(_moveAdorner);

                                DragDrop.AddGiveFeedbackHandler(layoutPanel, new GiveFeedbackEventHandler(DragDrop_Feedback));
                                DragDrop.AddQueryContinueDragHandler(layoutPanel, new QueryContinueDragEventHandler(DragDrop_QueryContinueDrag));

                                DragDrop.DoDragDrop(layoutPanel, new object(), DragDropEffects.None);

                                DragDrop.RemoveQueryContinueDragHandler(layoutPanel, new QueryContinueDragEventHandler(DragDrop_QueryContinueDrag));
                                DragDrop.RemoveGiveFeedbackHandler(layoutPanel, new GiveFeedbackEventHandler(DragDrop_Feedback));

                                AdornerLayer.GetAdornerLayer(layoutPanel as Visual).Remove(_moveAdorner);

                                if (_moveAdorner.IsCancel == false)
                                {
                                    if (ItemsMoving != null)
                                    {
                                        _moveAdorner.IsCancel = !ItemsMoving(targetItems);
                                    }
                                }
                                _moveAdorner.EndMoves();
                            }
                            catch
                            {
                            }
                            finally
                            {
                                _moveAdorner = null;
                                Mouse.OverrideCursor = null;
                            }
                        }
                    }
                }
                else if (Mouse.Captured == this)
                {
                    if ((DateTime.Now - _downContainerCapturedTime).TotalSeconds > 0.1d)
                    {
                        // range selection
                        var layoutPanel = GetItemsPanel();
                        _selectionAdorner = new SelectionAdorner(layoutPanel, _downPoint);
                        _selectionAdorner.SelectionBrush = new SolidColorBrush(Color.FromArgb(0xEE, 0x77, 0x77, 0x77));
                        _selectionAdorner.SelectionBorder = new Pen(Brushes.Blue, 1);

                        AdornerLayer.GetAdornerLayer(layoutPanel as Visual).Add(_selectionAdorner);

                        DragDrop.AddGiveFeedbackHandler(layoutPanel, new GiveFeedbackEventHandler(SelectionDragDrop_Feedback));
                        DragDrop.AddQueryContinueDragHandler(layoutPanel, new QueryContinueDragEventHandler(SelectionDragDrop_QueryContinueDrag));

                        DragDrop.DoDragDrop(layoutPanel, new object(), DragDropEffects.None);

                        DragDrop.RemoveQueryContinueDragHandler(layoutPanel, new QueryContinueDragEventHandler(SelectionDragDrop_QueryContinueDrag));
                        DragDrop.RemoveGiveFeedbackHandler(layoutPanel, new GiveFeedbackEventHandler(SelectionDragDrop_Feedback));

                        AdornerLayer.GetAdornerLayer(layoutPanel as Visual).Remove(_selectionAdorner);

                        if (RangeSelection != null)
                        {
                            List<object> selectedTargets = new List<object>();
                            RangeSelection(_selectionAdorner.SelectionRect, selectedTargets);

                            if (selectedTargets.Count > 0)
                            {
                                if (Keyboard.IsKeyDown(Key.LeftCtrl) == true)
                                {
                                    var union = this.SelectedItems.OfType<object>().ToList().Union(selectedTargets).Distinct().ToList();
                                    SetSelectedItems(union);
                                }
                                else
                                {
                                    SetSelectedItems(selectedTargets);
                                }
                            }
                            else
                            {
                                if (Keyboard.IsKeyDown(Key.LeftCtrl) == false)
                                {
                                    this.SelectedItem = null;
                                }
                            }
                        }
                        else
                        {
                            this.SelectedItem = null;
                        }
                    }
                    else
                    {
                        this.SelectedItem = null;
                    }
                    
                }
            }
        }

        public event Action<Rect, List<object>> RangeSelection;
        public event Func<List<IUIEditableObject>, bool> ItemsMoving;

        private void DragDrop_Feedback(object sender, GiveFeedbackEventArgs e)
        {
            if (_moveAdorner != null)
            {
                var point = this.PointFromScreen(User32Util.GetMousePosition());
                _moveAdorner.UpdatePosition(point);
            }
            e.Handled = true;
        }
        private void DragDrop_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (e.EscapePressed == true)
            {
                _moveAdorner.IsCancel = true;
            }
        }

        private void SelectionDragDrop_Feedback(object sender, GiveFeedbackEventArgs e)
        {
            if (_selectionAdorner != null)
            {
                var point = this.PointFromScreen(User32Util.GetMousePosition());
                _selectionAdorner.UpdatePosition(point);
            }
            e.Handled = true;
        }
        private void SelectionDragDrop_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            //if (e.EscapePressed == true)
            //{
            //    _isAdornerCancel = true;
            //}
        }

        private ListBoxItem CaptureContainer(MouseButtonEventArgs e)
        {
            _downContainer = null;
            _downPoint = e.GetPosition(GetItemsPanel());

            var fe = e.OriginalSource as FrameworkElement;
            if (fe != null)
            {
                var container = this.ItemContainerGenerator?.ContainerFromItem(fe.DataContext) as ListBoxItem;
                if (container != null)
                {
                    _downContainer = container;
                    var pos = e.GetPosition(_downContainer);
                    _donwPointOffset = new Point(-pos.X, -pos.Y);
                    _downContainerCapturedTime = DateTime.Now;
                    
                }
                //this.CaptureMouse();
                Mouse.Capture(this, CaptureMode.SubTree);
                return container;
            }
            return null;
        }


        #endregion


        private Panel GetItemsPanel()
        {
            ItemsPresenter itemsPresenter = VisualTreeHelperEx.FindDescendantByType<ItemsPresenter>(this);
            Panel itemsPanel = VisualTreeHelper.GetChild(itemsPresenter, 0) as Panel;
            return itemsPanel;
        }
    }
}
