using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NTech.Base.Commons.BaseViewModel;
using NTech.Base.Resources.NUtils;
using NTech.Wpf.Control.RefractiveZoom;
using NTech.Wpf.Modules.Common.Editor.Models.Refractive;


namespace NTech.Wpf.Modules.Common.Editor.ViewModels
{
    /// <summary>
    /// [NCS-3103] : Refractive index 창에서 scan image zoom in/out 기능 구현되지 않음
    /// </summary>
    public class RefractiveEditorViewModel : ViewModelBase
    {
        #region [ =============== Field =============== ]
        private RefractiveZoomControl _zoomControl = null;

        // [NCS-3556] : Vision변경 사항 검증
        private PixelColor[,] _pixelColors;
        private bool _correctMouseDownPosition = true;
        #endregion // [ =============== Field =============== ]


        #region [ =============== Property =============== ]
        private RefractiveSectionItem _refractiveSection;
        public RefractiveSectionItem RefractiveSection
        {
            get
            {
                if (this._refractiveSection == null) { this._refractiveSection = new RefractiveSectionItem(); }
                return this._refractiveSection;
            }
            set
            {
                this._refractiveSection = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion // [ =============== Property =============== ]


        #region [ =============== Constructor / Initialization =============== ]
        public RefractiveEditorViewModel(RefractiveZoomControl zoomControl) : base()
        {
            this._zoomControl = zoomControl;

            this.AttachEvents();
        }

        /// <summary>
        /// Attach EventAggregate
        /// </summary>
        private void AttachEvents()
        {
        }

        /// <summary>
        /// Event 삭제
        /// </summary>
        private void DetachEvents()
        {
        }
        #endregion // [ =============== Constructor / Initialization =============== ]


        #region [ =============== Method =============== ]
        /// <summary>
        /// SetImage
        /// </summary>
        /// <param name="path"></param>
        public void SetImage(string path)
        {
            this.RefractiveSection.LoadImage(path);

            // [NCS-3556] : Vision변경 사항 검증
            this.SetPixelColors();
        }

        /// <summary>
        /// SetImage
        /// </summary>
        /// <param name="path"></param>
        public void SetImage(ImageSource imageSource)
        {
            this.RefractiveSection.LoadImage(imageSource);

            // [NCS-3556] : Vision변경 사항 검증
            this.SetPixelColors();
        }

        // [NCS-3556] : Vision변경 사항 검증
        private void SetPixelColors()
        {
            this._pixelColors = null;

            if (this.RefractiveSection.Image != null)
            {
                if (this.RefractiveSection.Image is BitmapImage bitmapImage)
                {
                    this._pixelColors = ImageUtil.GetPixelColorsFromImageSource(bitmapImage);
                }
                else if (this.RefractiveSection.Image is CachedBitmap cachedBitmap)
                {
                    this._pixelColors = ImageUtil.GetPixelColorsFromImageSource(cachedBitmap);
                }
            }
        }

        /// <summary>
        /// ChangeMode
        /// </summary>
        /// <param name="mode"></param>
        public void ChangeMode(Modes mode)
        {
            if ((this.RefractiveSection.Mode == Modes.V && mode == Modes.H) || (this.RefractiveSection.Mode == Modes.H && mode == Modes.None))
            {
                this.RefractiveSection.Clear();
            }

            this.RefractiveSection.Mode = mode;
        }

        public void ApplyMousePoint(Point p, bool isClicked, MouseEventArgs e)
        {
            if (p != null)
            {
                var w = this.RefractiveSection.ImageWidth;
                var h = this.RefractiveSection.ImageHeight;

                if (isClicked == true && e is MouseButtonEventArgs arg && arg.ChangedButton == MouseButton.Left)
                {
                    if (p.X < 0 || p.Y < 0 || p.X > w || p.Y > h)
                    {
                        return;
                    }

                    // [NCS-3556] : Vision변경 사항 검증
                    // [NCS-3605] : 비전 소스 검증 2
                    if (this._correctMouseDownPosition && this._pixelColors != null && this._pixelColors.Length > 0)
                    {
                        int nWindowXSize = 2;
                        int nWimdpwYSize = 5;

                        int minSearchXWindow = (int)p.X - nWindowXSize;
                        int maxSearchXWindow = (int)p.X + nWindowXSize;
                        if (minSearchXWindow < 0) minSearchXWindow = 0;
                        if (maxSearchXWindow > w) maxSearchXWindow = (int)w;

                        int minSearchYWindow = (int)p.Y - nWimdpwYSize;
                        int maxSearchYWindow = (int)p.Y + nWimdpwYSize;
                        if (minSearchYWindow < 0) minSearchYWindow = 0;
                        if (maxSearchYWindow > h) maxSearchYWindow = (int)h;

                        // gray scale의 경우 RGB값이 모두동일하게 들어온다.
                        int nMaxIntensity = _pixelColors[(int)p.Y, (int)p.X].Blue;
                        int nMaxIntensityYPos = (int)p.Y;

                        //begin debug code
                        //List<PixelColor> listPixelColor = new List<PixelColor>(20);
                        //for (int dy = minSearchYWindow; dy < maxSearchYWindow; dy++)
                        //{
                        //    listPixelColor.Add(_pixelColors[dy, (int)p.X]);
                        //}

                        //end debug code

                        for (int dy = minSearchYWindow; dy < maxSearchYWindow; dy++)
                        {
                            int nCurrentIntensity = _pixelColors[dy, (int)p.X].Blue;
                            if (nCurrentIntensity > nMaxIntensity)
                            {
                                nMaxIntensity = nCurrentIntensity;
                                nMaxIntensityYPos = dy;
                            }
                        }

                        int nMaxRowIntensitySum = 0;
                        for (int dy = nMaxIntensityYPos-1; dy <= nMaxIntensityYPos+1; dy++)
                        {
                            if (dy > 0 && dy < h)
                            {
                                int nCurrentIntensitySum = 0;
                                for (int dx = minSearchXWindow; dx < maxSearchXWindow; dx++)
                                {
                                    nCurrentIntensitySum += _pixelColors[dy, dx].Blue;
                                }

                                if (nMaxRowIntensitySum < nCurrentIntensitySum)
                                {
                                    nMaxIntensityYPos = dy;
                                    nMaxRowIntensitySum = nCurrentIntensitySum;
                                }
                            }

                        }
                        

                        p.Y = nMaxIntensityYPos;
                    }

                    this.RefractiveSection.AddMarker(p);

                }
                else if (isClicked == false)
                {
                    this.RefractiveSection.AddLine(p);
                }
            }
        }

        public void ResetAll()
        {
            this.RefractiveSection.Clear();

         
        }

        public void Undo()
        {
            this.RefractiveSection.Clear(false);

        }
        #endregion // [ =============== Method =============== ]


        #region [ =============== Dispose =============== ]
        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                }

                this.DetachEvents();
                this.disposed = true;
            }

            base.Dispose(disposing);
        }
        #endregion // [ =============== Dispose =============== ]
    }
}
