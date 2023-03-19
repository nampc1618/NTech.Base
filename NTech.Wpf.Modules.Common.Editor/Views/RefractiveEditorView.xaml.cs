using NTech.Wpf.Control.RefractiveZoom;
using NTech.Wpf.Modules.Common.Editor.Models.Refractive;
using NTech.Wpf.Modules.Common.Editor.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NTech.Wpf.Modules.Common.Editor.Views
{
    /// <summary>
    /// Interaction logic for RefractiveEditorView.xaml
    /// </summary>
    public partial class RefractiveEditorView : UserControl
    {
        private RefractiveEditorViewModel _viewModel = null;
        public RefractiveEditorView()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            if (this.DataContext == null)
            {
                this._viewModel = new RefractiveEditorViewModel(this.zc);
                this.DataContext = this._viewModel;
            }

            _viewModel.SetImage("D:\\ENTRY\\@NTECH\\NTECHBASE\\NTech.Base\\NTech.Base.Wpf\\Images\\BannerNTech-large.jpg");
        }
        private void zc_ZoomControlMouseUpEvent(object sender, Point point, bool isClicked, MouseEventArgs e)
        {
            if (this._viewModel != null)
            {
                this._viewModel.ApplyMousePoint(point, isClicked, e);
            }
        }

        private void EditorPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            var eventArg = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton);
            eventArg.RoutedEvent = UIElement.MouseDownEvent;
            this.zc.RaiseEvent(eventArg);
        }

        private void EditorPanel_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            var eventArg = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton);
            eventArg.RoutedEvent = UIElement.MouseUpEvent;
            this.zc.RaiseEvent(eventArg);

            if (e.ChangedButton == MouseButton.Right)
            {
                if (this._viewModel != null)
                {
                    this._viewModel.Undo();
                }
            }
        }

        private void EditorPanel_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArg.RoutedEvent = UIElement.MouseWheelEvent;
            this.zc.RaiseEvent(eventArg);
        }

        private void EditorPanel_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;

            var eventArg = new MouseEventArgs(e.MouseDevice, e.Timestamp, e.StylusDevice);
            eventArg.RoutedEvent = UIElement.MouseMoveEvent;
            this.zc.RaiseEvent(eventArg);
        }

        public void SetImage(string path)
        {
            if (this._viewModel != null)
            {
                this._viewModel.SetImage(path);
            }
        }

        public void SetImage(ImageSource imageSource)
        {
            if (this._viewModel != null)
            {
                this._viewModel.SetImage(imageSource);
            }
        }

        public void ResetAll()
        {
            if (this._viewModel != null)
            {
                this._viewModel.ResetAll();
            }
        }

        public void ChangeMode(Modes mode)
        {
            if (this._viewModel != null)
            {
                this.zc.Mode = RefractiveZoomControlModes.Custom;
                this.zc.Mode = RefractiveZoomControlModes.Fill;

                this._viewModel.ChangeMode(mode);
            }
        }

        public bool EndPointClicked()
        {
            bool endPointClicked = false;

            if (this._viewModel != null)
            {
                endPointClicked = this._viewModel.RefractiveSection.IsAddedHE;
            }

            return endPointClicked;
        }

        public bool TopCoatingPointClicked()
        {
            bool topCoatingPointClicked = false;

            if (this._viewModel != null)
            {
                topCoatingPointClicked = this._viewModel.RefractiveSection.IsAddedVS;
            }

            return topCoatingPointClicked;
        }

        public bool BottomCoatingPointClicked()
        {
            bool bottomCoatingPointClicked = false;

            if (this._viewModel != null)
            {
                bottomCoatingPointClicked = this._viewModel.RefractiveSection.IsAddedVE;
            }

            return bottomCoatingPointClicked;
        }

        public float RefractiveIndexValue()
        {
            float refractiveIndexValue = default(float);

            if (this._viewModel != null)
            {
                refractiveIndexValue = this._viewModel.RefractiveSection.RefractiveIndexValue;
            }

            return refractiveIndexValue;
        }
    }
}
