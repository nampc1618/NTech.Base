using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NTech.Base.Commons.BaseModel;
using NTech.Base.Wpf.Controls.ObjectEditor.Models;

namespace NTech.Wpf.Modules.Common.Editor.Models.Refractive
{
    /// <summary>
    /// [NCS-3103] : Refractive index 창에서 scan image zoom in/out 기능 구현되지 않음
    /// </summary>
    public enum Modes
    {
        H,
        V,
        None,
    }

    /// <summary>
    /// [NCS-3103] : Refractive index 창에서 scan image zoom in/out 기능 구현되지 않음
    /// </summary>
    public class RefractiveSectionItem : BaseModel
    {
        /// <summary>
        /// Margin
        /// </summary>
        private const int MARGIN = 1;

        /// <summary>
        /// Mode
        /// </summary>
        public Modes Mode { get; set; } = Modes.H;

        private ImageSource _image;
        /// <summary>
        /// Image
        /// </summary>
        public ImageSource Image
        {
            get { return this._image; }
            set
            {
                this._image = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Image Width
        /// </summary>
        public double ImageWidth { get; set; }

        /// <summary>
        /// Image Height
        /// </summary>
        public double ImageHeight { get; set; }

        /// <summary>
        /// Image Path
        /// </summary>
        public string ImagePath { get; private set; }

        /// <summary>
        /// Image Name
        /// </summary>
        public string ImageName { get; private set; }

        private bool _isAddedHS = false;
        /// <summary>
        /// Is Added Horizontal Start Point
        /// </summary>
        public bool IsAddedHS
        {
            get { return this._isAddedHS; }
            set
            {
                this._isAddedHS = value;
                this.RaisePropertyChanged();
            }
        }

        private bool _isAddedHE = false;
        /// <summary>
        /// Is Added Horizontal End Point
        /// </summary>
        public bool IsAddedHE
        {
            get { return this._isAddedHE; }
            set
            {
                this._isAddedHE = value;
                this.RaisePropertyChanged();
            }
        }

        private bool _isAddedVS = false;
        /// <summary>
        /// Is Added Vertical Start Point
        /// </summary>
        public bool IsAddedVS
        {
            get { return this._isAddedVS; }
            set
            {
                this._isAddedVS = value;
                this.RaisePropertyChanged();
            }
        }

        private bool _isAddedVE = false;
        /// <summary>
        /// Is Added Vertical End Point
        /// </summary>
        public bool IsAddedVE
        {
            get { return this._isAddedVE; }
            set
            {
                this._isAddedVE = value;
                this.RaisePropertyChanged();
            }
        }

        private PointMarkerItem _markerHS;
        /// <summary>
        /// Horizontal Start Point Marker
        /// </summary>
        public PointMarkerItem MarkerHS
        {
            get
            {
                if (this._markerHS == null) { this._markerHS = new PointMarkerItem(); }
                return this._markerHS;
            }
            set
            {
                this._markerHS = value;
                this.RaisePropertyChanged();
            }
        }

        private PointMarkerItem _markerHE;
        /// <summary>
        /// Horizontal End Point Marker
        /// </summary>
        public PointMarkerItem MarkerHE
        {
            get
            {
                if (this._markerHE == null) { this._markerHE = new PointMarkerItem(); }
                return this._markerHE;
            }
            set
            {
                this._markerHE = value;
                this.RaisePropertyChanged();
            }
        }

        private PointMarkerItem _markerVS;
        /// <summary>
        /// Vertical Start Point Marker
        /// </summary>
        public PointMarkerItem MarkerVS
        {
            get
            {
                if (this._markerVS == null) { this._markerVS = new PointMarkerItem(); }
                return this._markerVS;
            }
            set
            {
                this._markerVS = value;
                this.RaisePropertyChanged();
            }
        }

        private PointMarkerItem _markerVE;
        /// <summary>
        /// Vertical End Point Marker
        /// </summary>
        public PointMarkerItem MarkerVE
        {
            get
            {
                if (this._markerVE == null) { this._markerVE = new PointMarkerItem(); }
                return this._markerVE;
            }
            set
            {
                this._markerVE = value;
                this.RaisePropertyChanged();
            }
        }

        private LineItem _lineH;
        /// <summary>
        /// Horizontal Line
        /// </summary>
        public LineItem LineH
        {
            get
            {
                if (this._lineH == null) { this._lineH = new LineItem(); }
                return this._lineH;
            }
            set
            {
                this._lineH = value;
                this.RaisePropertyChanged();
            }
        }

        private LineItem _lineV;
        /// <summary>
        /// Vertical Line
        /// </summary>
        public LineItem LineV
        {
            get
            {
                if (this._lineV == null) { this._lineV = new LineItem(); }
                return this._lineV;
            }
            set
            {
                this._lineV = value;
                this.RaisePropertyChanged();
            }
        }

        private float _refractiveIndexValue;
        /// <summary>
        /// RefractiveIndexValue
        /// </summary>
        public float RefractiveIndexValue
        {
            get { return this._refractiveIndexValue; }
            set
            {
                this._refractiveIndexValue = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RefractiveSectionItem()
        {
        }

        /// <summary>
        /// Load Image
        /// </summary>
        /// <param name="path"></param>
        public void LoadImage(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                this.Image = null;

                this.ImageWidth = 0;
                this.ImageHeight = 0;

                this.ImagePath = string.Empty;
                this.ImageName = string.Empty;

                return;
            }

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.CacheOption = BitmapCacheOption.OnLoad;
            bi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bi.UriSource = new Uri(path, UriKind.Absolute);
            bi.EndInit();
            bi.Freeze();

            this.ImageWidth = bi.PixelWidth;
            this.ImageHeight = bi.PixelHeight;

            this.Image = bi;
            this.ImagePath = path;
            this.ImageName = Path.GetFileName(path);
        }

        /// <summary>
        /// Load Image
        /// </summary>
        /// <param name="imageSource"></param>
        public void LoadImage(ImageSource imageSource)
        {
            this.ImageWidth = imageSource.Width;
            this.ImageHeight = imageSource.Height;

            this.Image = imageSource;
        }

        /// <summary>
        /// Clear Item
        /// </summary>
        /// <param name="isAll"></param>
        public void Clear(bool isAll = true)
        {
            this.RefractiveIndexValue = float.NaN;
            if (isAll)
            {
                switch (this.Mode)
                {
                    case Modes.H:
                        {
                            this.MarkerHS.Visibility = Visibility.Collapsed;
                            this.MarkerHE.Visibility = Visibility.Collapsed;
                            this.LineH.Visibility = Visibility.Collapsed;

                            this.IsAddedHS = false;
                            this.IsAddedHE = false;
                        }
                        break;
                    case Modes.V:
                        {
                            this.MarkerVS.Visibility = Visibility.Collapsed;
                            this.MarkerVE.Visibility = Visibility.Collapsed;
                            this.LineV.Visibility = Visibility.Collapsed;

                            this.IsAddedVS = false;
                            this.IsAddedVE = false;
                        }
                        break;
                }
            }
            else
            {
                switch (this.Mode)
                {
                    case Modes.H:
                        {
                            if (this.IsAddedHS && this.IsAddedHE)
                            {
                                this.IsAddedHE = false;

                                this.MarkerHS.Visibility = Visibility.Visible;
                            }
                            else if (this.IsAddedHS)
                            {
                                this.IsAddedHS = false;
                                this.MarkerHS.Visibility = Visibility.Collapsed;
                                this.LineH.Visibility = Visibility.Collapsed;
                            }
                        }
                        break;
                    case Modes.V:
                        {
                            if (this.IsAddedVS && this.IsAddedVE)
                            {
                                this.IsAddedVS = false;
                                this.IsAddedVE = false;

                                this.AddMarker(this.MarkerVS.LayoutBounds.Location);
                            }
                            else if (this.IsAddedVS)
                            {
                                this.IsAddedVS = false;
                                this.MarkerVS.Visibility = Visibility.Collapsed;
                                this.LineV.Visibility = Visibility.Collapsed;
                            }
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Add Point Marker
        /// </summary>
        /// <param name="p"></param>
        /// <param name="isVisible"></param>
        public void AddMarker(Point p, bool isVisible = true)
        {
            switch (this.Mode)
            {
                case Modes.H:
                    {
                        if (this.IsAddedHS == false)
                        {
                            this.AddMarkerHS(p);
                        }
                        else if (this.IsAddedHE == false)
                        {
                            this.AddMarkerHE(p);
                            this.AddLineH();
                        }
                    }
                    break;
                case Modes.V:
                    {
                        if (this.IsAddedVS == false)
                        {
                            this.AddMarkerVS(p);

                            var ve = CalculateSecondPoint(this.MarkerHS.LayoutBounds.Location, this.MarkerHE.LayoutBounds.Location, p);
                            this.AddMarkerVE(ve, false);

                            var bounds = this.CalculateLineLayoutBouds(p, ve);
                            this.AddLineV(bounds.X, bounds.Y, bounds.Width, bounds.Height);
                        }
                        else if (this.IsAddedVE == false)
                        {
                            bool checkedPoint = this.CheckBottomPointAccepted(this.MarkerHS.LayoutBounds.Location, this.MarkerHE.LayoutBounds.Location, this.MarkerVS.LayoutBounds.Location, p);
                            if (checkedPoint)
                            {
                                this.MarkerVE.Visibility = true ? Visibility.Visible : Visibility.Collapsed;
                                this.IsAddedVE = true;

                                this.AddLineV(this.MarkerVS.LayoutBounds.X, this.MarkerVS.LayoutBounds.Y, this.MarkerVE.LayoutBounds.X, this.MarkerVE.LayoutBounds.Y);

                                this.CalculateRefractiveIndex();
                            }
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Add Line
        /// </summary>
        /// <param name="p"></param>
        public void AddLine(Point p)
        {
            switch (this.Mode)
            {
                case Modes.H:
                    {
                        if (this.IsAddedHS == true && this.IsAddedHE == false)
                        {
                            this.AddMarkerHE(p, false);
                            this.AddLineH();
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Add Horizontal Start Point Marker
        /// </summary>
        /// <param name="p"></param>
        /// <param name="isVisible"></param>
        public void AddMarkerHS(Point p, bool isVisible = true)
        {
            this.Mode = Modes.H;
            this.MarkerHS.LayoutBounds = new UIEditableBounds(p.X, p.Y, 0, 0);
            this.MarkerHS.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;

            this.IsAddedHS = true;
        }

        /// <summary>
        /// Add Horizontal End Point Marker
        /// </summary>
        /// <param name="p"></param>
        /// <param name="isVisible"></param>
        public void AddMarkerHE(Point p, bool isVisible = true)
        {
            this.MarkerHE.LayoutBounds = new UIEditableBounds(p.X, p.Y, 0, 0);

            this.MarkerHE.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            this.IsAddedHE = isVisible;
        }

        /// <summary>
        /// Add Vertical Start Point Marker
        /// </summary>
        /// <param name="p"></param>
        /// <param name="isVisible"></param>
        public void AddMarkerVS(Point p, bool isVisible = true)
        {
            this.Mode = Modes.V;
            this.MarkerVS.LayoutBounds = new UIEditableBounds(p.X, p.Y, 0, 0);
            this.MarkerVS.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;

            this.IsAddedVS = isVisible;
        }

        /// <summary>
        /// Add Vertical End Point Marker
        /// </summary>
        /// <param name="p"></param>
        /// <param name="isVisible"></param>
        public void AddMarkerVE(Point p, bool isVisible = true)
        {
            this.MarkerVE.LayoutBounds = new UIEditableBounds(p.X, p.Y, 0, 0);
            this.MarkerVE.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;

            this.IsAddedVE = isVisible;
        }

        /// <summary>
        /// Add Horizontal Line
        /// </summary>
        public void AddLineH()
        {
            this.LineH.LayoutBounds = this.CalculateLineLayoutBouds(this.MarkerHS.LayoutBounds.Location, this.MarkerHE.LayoutBounds.Location);
            this.LineH.Visibility = Visibility.Visible;

            if (this.IsAddedHE)
            {
                this.MarkerHS.Visibility = Visibility.Collapsed;
                this.MarkerHE.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Add Vertical Line
        /// </summary>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <param name="ex"></param>
        /// <param name="ey"></param>
        public void AddLineV(double sx, double sy, double ex, double ey)
        {
            this.LineV.LayoutBounds = new UIEditableBounds(sx, sy, ex, ey);
            this.LineV.Visibility = Visibility.Visible;

            if (this.IsAddedVE)
            {
                this.MarkerVS.Visibility = Visibility.Collapsed;
                this.MarkerVE.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Calculate Line LayoutBouds
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private UIEditableBounds CalculateLineLayoutBouds(Point s, Point e)
        {
            UIEditableBounds layoutBounds = null;

            if (s.X == e.X)
            {
                layoutBounds = new UIEditableBounds(s.X, 0, s.X, this.ImageHeight);
            }
            else if (s.Y == e.Y)
            {
                layoutBounds = new UIEditableBounds(0, s.Y, this.ImageWidth, s.Y);
            }
            else
            {
                double sX = 0;
                double sY = 0;
                double eX = 0;
                double eY = 0;

                double a = (s.Y - e.Y) / (s.X - e.X);
                double b = s.Y - ((s.Y - e.Y) * s.X) / (s.X - e.X);

                double positionLeft = b;
                if ((positionLeft >= 0) && (positionLeft <= this.ImageHeight))
                {
                    sX = 0;
                    sY = positionLeft;
                }
                else if (positionLeft < 0)
                {
                    sX = -b / a;
                    sY = 0;
                }
                else if (positionLeft > this.ImageHeight)
                {
                    sX = (this.ImageHeight - b) / a;
                    sY = this.ImageHeight;
                }

                double positionRight = a * this.ImageWidth + b;
                if ((positionRight >= 0) && (positionRight <= this.ImageHeight))
                {
                    eX = this.ImageWidth;
                    eY = positionRight;
                }
                else if (positionRight < 0)
                {
                    eX = -b / a;
                    eY = 0;
                }
                else if (positionRight > this.ImageHeight)
                {
                    eX = (this.ImageHeight - b) / a;
                    eY = this.ImageHeight;
                }

                layoutBounds = new UIEditableBounds(sX, sY, eX, eY);
            }

            return layoutBounds;
        }

        /// <summary>
        /// Calculate Second Point
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <param name="vs"></param>
        /// <returns></returns>
        private Point CalculateSecondPoint(Point s, Point e, Point vs)
        {
            Point p = new Point();
            if (s.X == e.X)
            {
                p.X = vs.X + 1;
                p.Y = vs.Y;
            }
            else if (s.Y == e.Y)
            {
                p.X = vs.X;
                p.Y = vs.Y + 1;
            }
            else
            {
                double a = (s.Y - e.Y) / (s.X - e.X);
                double b = (1 / a) * vs.X + vs.Y;
                p.Y = b;
            }
            return p;
        }

        /// <summary>
        /// Check Vertical End Point Accepted
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <param name="vs"></param>
        /// <param name="ve"></param>
        /// <returns></returns>
        private bool CheckBottomPointAccepted(Point s, Point e, Point vs, Point ve)
        {
            if (s.X == e.X)
            {
                if ((ve.Y <= vs.Y + MARGIN) && (ve.Y >= vs.Y - MARGIN))
                {
                    this.MarkerVE.LayoutBounds = new UIEditableBounds(vs.Y, ve.X, 0, 0);
                    return true;
                }
            }
            else if (s.Y == e.Y)
            {
                if ((ve.X <= vs.X + MARGIN) && (ve.X >= vs.X - MARGIN))
                {
                    this.MarkerVE.LayoutBounds = new UIEditableBounds(vs.X, ve.Y, 0, 0);
                    return true;
                }
            }
            else
            {
                double a = (s.Y - e.Y) / (s.X - e.X);
                double b = vs.Y + (1 / a) * vs.X;
                double yvalue = (-1 / a) * ve.X + b;
                double xvalue = (-ve.Y + b) * a;
                if ((ve.Y <= yvalue + MARGIN) && (ve.Y >= yvalue - MARGIN))
                {
                    this.MarkerVE.LayoutBounds = new UIEditableBounds(ve.X, yvalue, 0, 0);
                    return true;
                }
                else if ((ve.X <= xvalue + MARGIN) && (ve.X >= xvalue - MARGIN))
                {
                    this.MarkerVE.LayoutBounds = new UIEditableBounds(xvalue, ve.Y, 0, 0);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Calculate RefractiveIndex
        /// </summary>
        private void CalculateRefractiveIndex()
        {
            Point crossPoint = CalculatePerpendicularPoint();
            double topCoatingtoBaseLineDistance = Math.Sqrt(Math.Pow((this.MarkerVS.LayoutBounds.X - crossPoint.X), 2) + Math.Pow((this.MarkerVS.LayoutBounds.Y - crossPoint.Y), 2));
            double bottomCoatingtoBaseLineDistance = Math.Sqrt(Math.Pow((this.MarkerVE.LayoutBounds.X - crossPoint.X), 2) + Math.Pow((this.MarkerVE.LayoutBounds.Y - crossPoint.Y), 2));
            double refractiveIndexValue = 1;
            if (topCoatingtoBaseLineDistance != 0)
            {
                refractiveIndexValue = (topCoatingtoBaseLineDistance + bottomCoatingtoBaseLineDistance) / topCoatingtoBaseLineDistance;
                this.RefractiveIndexValue = (float)refractiveIndexValue;
            }
        }

        /// <summary>
        /// Calculate Perpendicular Point
        /// </summary>
        /// <returns></returns>
        private Point CalculatePerpendicularPoint()
        {
            var startPoint = this.MarkerHS.LayoutBounds.Location;
            var endPoint = this.MarkerHE.LayoutBounds.Location;
            var elementPoint = this.MarkerVS.LayoutBounds.Location;

            Point ptPointCenter = new Point();
            double fTemp = ((endPoint.X - startPoint.X) * (elementPoint.X - startPoint.X) +
                (endPoint.Y - startPoint.Y) * (elementPoint.Y - startPoint.Y)) / (Math.Pow((endPoint.Y - startPoint.Y), 2) +
                Math.Pow((endPoint.X - startPoint.X), 2));
            ptPointCenter.X = startPoint.X + fTemp * (endPoint.X - startPoint.X);
            ptPointCenter.Y = startPoint.Y + fTemp * (endPoint.Y - startPoint.Y);
            return ptPointCenter;
        }
    }
}
