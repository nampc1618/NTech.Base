using NTech.Base.Commons.BaseViewModel;
using NTech.Base.Wpf.Utils;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace NTech.Base.Wpf.Controls.ObjectEditor.Models
{
    //public interface IEditableUIObject
    //{
    //    // Positions
    //    double X { get; set; }
    //    double Y { get; set; }
    //    double Width { get; set; }
    //    double Height { get; set; }

    //    // Editable
    //    double EditedX { get; set; }
    //    double EditedY { get; set; }
    //    double EditedWidth { get; set; }
    //    double EditedHeight { get; set; }
    //    bool IsEditing { get; }

    //    //// Binding Position
    //    //double ProjectionX { get; }
    //    //double ProjectionY { get; }
    //    //double ProjectionWidth { get; }
    //    //double ProjectionHeight { get; }

    //    //bool IsEditMode { get; set; }
    //    void BeginEdit();
    //    void EndEdit(bool isCancel);


    //    //
    //    int ZIndex { get; set; }
    //    Rect GetBounds();

    //    // Selections
    //    bool IsSelected { get; set; }

    //    // Visible
    //}

    public interface IUIEditableObject
    {
        UIEditableBounds LayoutBounds { get; set; }
    }

    public class UIEditableBounds : ViewModelBase, ICloneable
    {
        public static UIEditableBounds GetUIEditableBoundsFromCenterPoint(double centerX, double centerY, double width, double height, double angle = 0d)
        {
            UIEditableBounds result = new UIEditableBounds();

            result.X = centerX - width * 0.5;
            result.Y = centerY - height * 0.5;
            result.Width = width;
            result.Height = height;
            result.Angle = angle;

            return result;
        }

        /// <summary>
        /// [ NCS-336 : CreateJob from kypcb ] : Board 좌표를 Array 좌표로 변경 (Item 의 좌상단 포인트 리턴)
        /// </summary>
        /// <param name="itemGlobalCenter">Item 의 Board 좌표 정보 (Center)</param>
        /// <param name="array">Item 이 속한 Array 의 좌표 정보</param>
        /// <param name="isNeedAngle">Item Angle 유지 여부</param>
        /// <returns>Item 의 좌상단 포인트 리턴 (Array 기준)</returns>
        public static UIEditableBounds GetArrayLayoutBounds(UIEditableBounds itemGlobalCenter, UIEditableBounds array, bool isNeedAngle = true)
        {
            UIEditableBounds itemGlobalLayoutBounds = UIEditableBounds.GetUIEditableBoundsFromCenterPoint(itemGlobalCenter.X, itemGlobalCenter.Y, itemGlobalCenter.Width, itemGlobalCenter.Height);
            UIEditableBounds arrayLayoutBounds = new UIEditableBounds(array.X, array.Y, array.Width, array.Height, array.Angle * -1);
            UIEditableBounds itemLayoutBounds = UIEditableBounds.TranslateRotation(itemGlobalLayoutBounds - arrayLayoutBounds, arrayLayoutBounds.Angle, arrayLayoutBounds.GetBoundsCenter());

            return new UIEditableBounds(itemLayoutBounds.X, itemLayoutBounds.Y, itemLayoutBounds.Width, itemLayoutBounds.Height, isNeedAngle ? (itemGlobalCenter.Angle - arrayLayoutBounds.Angle) : 0);
        }



        public Point GetCenter()
        {
            return new Point(this.X + this.Width * 0.5, this.Y + this.Height * 0.5);
        }
        public Point GetEditedCenter()
        {
            return new Point(this.EditedX + this.EditedWidth * 0.5, this.EditedY + this.EditedHeight * 0.5);
        }

        public Point GetBoundsCenter()
        {
            return new Point(this.Width * 0.5, this.Height * 0.5);
        }

        public static UIEditableBounds TranslateRotation(UIEditableBounds item, double angle, Point point)
        {
            var center = item.GetCenter();
            var rotated = BasicMath.CalcRotatePoint(center, point, angle);

            UIEditableBounds result = new UIEditableBounds();
            result.X = rotated.X - (item.Width * 0.5);
            result.Y = rotated.Y - (item.Height * 0.5);
            result.Width = item.Width;
            result.Height = item.Height;
            result.Angle = BasicMath.DegreeNormalize(item.Angle + angle);

            var editedCenter = item.GetEditedCenter();
            var editedRotated = BasicMath.CalcRotatePoint(editedCenter, point, angle);

            result.EditedX = editedRotated.X - (item.EditedWidth * 0.5);
            result.EditedY = editedRotated.Y - (item.EditedHeight * 0.5);
            result.EditedWidth = item.EditedWidth;
            result.EditedHeight = item.EditedHeight;
            result.EditedAngle = BasicMath.DegreeNormalize(item.EditedAngle + angle);

            return result;
        }


        public static UIEditableBounds operator +(UIEditableBounds a, UIEditableBounds b)
        {
            UIEditableBounds result = new UIEditableBounds();

            result.X = a.X + b.X;
            result.Y = a.Y + b.Y;
            result.Width = a.Width;
            result.Height = a.Height;

            return result;
        }

        public static UIEditableBounds operator -(UIEditableBounds a, UIEditableBounds b)
        {
            UIEditableBounds result = new UIEditableBounds();

            result.X = a.X - b.X;
            result.Y = a.Y - b.Y;
            result.Width = a.Width;
            result.Height = a.Height;

            return result;
        }


        public UIEditableBounds()
        {
        }
        public UIEditableBounds(double x, double y, double width, double height, double angle = 0d)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.Angle = angle;
        }

        public event Action CoordinateChanged;

        private void OnCoordinateChanged()
        {
            if (CoordinateChanged != null)
            {
                CoordinateChanged();
            }
        }

        public double Angle
        {
            get
            {
                return _angle;
            }
            set
            {
                var oldValue = _angle;

                _angle = value;
                RaisePropertyChanged();

                if (oldValue != value) { OnCoordinateChanged(); }
            }
        }
        private double _angle;

        public double X
        {
            get
            {
                return _x;
            }
            set
            {
                var oldValue = _x;

                _x = value;
                RaisePropertyChanged();

                if (oldValue != value) { OnCoordinateChanged(); }
            }
        }
        private double _x;

        public double Y
        {
            get
            {
                return _y;
            }
            set
            {
                var oldValue = _y;

                _y = value;
                RaisePropertyChanged();

                if (oldValue != value) { OnCoordinateChanged(); }
            }
        }
        private double _y;

        public double Width
        {
            get
            {
                return _width;
            }
            set
            {
                var oldValue = _width;

                _width = value;
                RaisePropertyChanged();
                RaisePropertyChanged("LiveWidth");

                if (oldValue != value) { OnCoordinateChanged(); }
            }
        }
        private double _width;

        public double Height
        {
            get
            {
                return _height;
            }
            set
            {
                var oldValue = _height;

                _height = value;
                RaisePropertyChanged();
                RaisePropertyChanged("LiveHeight");

                if (oldValue != value) { OnCoordinateChanged(); }
            }
        }
        private double _height;

        public double LiveWidth
        {
            get
            {
                return this.IsEditing == true ? this.EditedWidth : this.Width;
            }
        }
        public double LiveHeight
        {
            get
            {
                return this.IsEditing == true ? this.EditedHeight : this.Height;
            }
        }

        public void UpdateLiveSize()
        {
            RaisePropertyChanged("LiveWidth");
            RaisePropertyChanged("LiveHeight");
        }

        public int ZIndex
        {
            get
            {
                return _zIndex;
            }
            set
            {
                _zIndex = value;
                RaisePropertyChanged();
            }
        }
        private int _zIndex;

        public virtual Rect GetBounds()
        {
            return new Rect(X, Y, Width, Height);
        }

        public virtual void SetBounds(Rect bounds)
        {
            this.X = bounds.X;
            this.Y = bounds.Y;
            this.Width = bounds.Width;
            this.Height = bounds.Height;
        }

        public virtual void SetValue(UIEditableBounds value)
        {
            this.X = value.X;
            this.Y = value.Y;
            this.Width = value.Width;
            this.Height = value.Height;
            this.Angle = value.Angle;

            this.EditedX = value.EditedX;
            this.EditedY = value.EditedY;
            this.EditedWidth = value.EditedWidth;
            this.EditedHeight = value.EditedHeight;
            this.EditedAngle = value.EditedAngle;

            this.IsEditing = value.IsEditing;
        }

        public double EditedAngle
        {
            get
            {
                return _editedAngle;
            }
            set
            {
                var oldValue = _editedAngle;

                _editedAngle = value;
                RaisePropertyChanged();

                if (oldValue != value) { OnCoordinateChanged(); }
            }
        }
        private double _editedAngle;

        public double EditedX
        {
            get
            {
                return _editedX;
            }
            set
            {
                var oldValue = _editedX;

                _editedX = value;
                if (oldValue != value) { OnCoordinateChanged(); }
                RaisePropertyChanged();
            }
        }
        private double _editedX;

        public double EditedY
        {
            get
            {
                return _editedY;
            }
            set
            {
                var oldValue = _editedY;

                _editedY = value;
                RaisePropertyChanged();

                if (oldValue != value) { OnCoordinateChanged(); }
            }
        }
        private double _editedY;

        public double EditedWidth
        {
            get
            {
                return _editedWidth;
            }
            set
            {
                var oldValue = _editedWidth;

                _editedWidth = value;
                RaisePropertyChanged();
                RaisePropertyChanged("LiveWidth");

                if (oldValue != value) { OnCoordinateChanged(); }
            }
        }
        private double _editedWidth;

        public double EditedHeight
        {
            get
            {
                return _editedHeight;
            }
            set
            {
                var oldValue = _editedHeight;

                _editedHeight = value;
                RaisePropertyChanged();
                RaisePropertyChanged("LiveHeight");

                if (oldValue != value) { OnCoordinateChanged(); }
            }
        }
        private double _editedHeight;

        public int EditedZIndex
        {
            get
            {
                return _editedZIndex;
            }
            set
            {
                _editedZIndex = value;
                RaisePropertyChanged();
            }
        }
        private int _editedZIndex;

        /// <summary>
        /// BoardSetting 팝업에서 Array 이동시 Local Fiducial 도 함께 이동하기 위해 사용
        /// </summary>
        public bool IsTempEditing { get; set; }

        public bool IsEditing
        {
            get
            {
                return _isEditing;
            }
            private set
            {
                var oldValue = _isEditing;

                _isEditing = value;
                RaisePropertyChanged();

                if (oldValue != value) { OnCoordinateChanged(); }
            }
        }
        private bool _isEditing;

        public void BeginEdit()
        {
            this.EditedX = this.X;
            this.EditedY = this.Y;
            this.EditedWidth = this.Width;
            this.EditedHeight = this.Height;
            this.EditedAngle = this.Angle;

            this.IsEditing = true;
        }

        public void EndEdit(bool isCancel)
        {
            if (isCancel == false)
            {
                this.X = this.EditedX;
                this.Y = this.EditedY;
                this.Width = this.EditedWidth;
                this.Height = this.EditedHeight;
                this.Angle = this.EditedAngle;
            }
            this.IsEditing = false;
        }

        //public double X
        //{
        //    get
        //    {
        //        return _x;
        //    }
        //    set
        //    {
        //        _x = value;
        //        RaisePropertyChanged();

        //        _unitX = value * PixelToUnitWidth;
        //        RaisePropertyChanged("UnitX");
        //    }
        //}
        //private double _x;

        //public double Y { get; set; }
        //public double Width { get; set; }
        //public double Height { get; set; }

        //public double UnitX
        //{
        //    get
        //    {
        //        return _unitX;
        //    }
        //    set
        //    {
        //        _unitX = value;
        //        RaisePropertyChanged();

        //        _x = value * UnitToPixelWidth;
        //        RaisePropertyChanged("_x");
        //    }
        //}
        //private double _unitX;

        //public double UnitY { get; set; }
        //public double UnitWidth { get; set; }
        //public double UnitHeight { get; set; }


        //public double UnitToPixelWidth { get; set; } = 1d;
        //public double PixelToUnitWidth { get; set; } = 1d;
        //public double UnitToPixelHeight { get; set; } = 1d;
        //public double PixelToUnitHeight { get; set; } = 1d;

        //public void SetUnitInfo(double unitToPixelWidth, double unitToPixelHeight)
        //{
        //    UnitToPixelWidth = unitToPixelWidth;
        //    PixelToUnitWidth = 1d / unitToPixelWidth;

        //    UnitToPixelHeight = unitToPixelHeight;
        //    PixelToUnitHeight = 1d / unitToPixelHeight;
        //}
        //public const string EXPORT_ELEMENT_NAME = "UIEditableBounds";
        //public XElement Export(string name = null)
        //{
        //    XElement xDoc = new XElement(EXPORT_ELEMENT_NAME);
        //    if (string.IsNullOrWhiteSpace(name) == false)
        //    {
        //        xDoc.SetAttributeValue("Name", name);
        //    }
            
        //    xDoc.SetAttributeValue("X", this.X);
        //    xDoc.SetAttributeValue("Y", this.Y);
        //    xDoc.SetAttributeValue("Width", this.Width);
        //    xDoc.SetAttributeValue("Height", this.Height);
        //    xDoc.SetAttributeValue("Angle", this.Angle);

        //    return xDoc;
        //}

        //public void Import(XElement xDoc)
        //{
        //    this.X = xDoc.SafeGetAttributeByDouble("X");
        //    this.Y = xDoc.SafeGetAttributeByDouble("Y");
        //    this.Width = xDoc.SafeGetAttributeByDouble("Width");
        //    this.Height = xDoc.SafeGetAttributeByDouble("Height");
        //    this.Angle = xDoc.SafeGetAttributeByDouble("Angle");
        //}

        public object Clone()
        {
            UIEditableBounds clone = new UIEditableBounds();
            clone.X = this.X;
            clone.Y = this.Y;
            clone.Width = this.Width;
            clone.Height = this.Height;
            clone.Angle = this.Angle;
            return clone;
        }

        /// <summary>
        /// 원본에서 해당 % 만큼 줄인 LayoutBounds를 구함
        /// </summary>
        /// <param name="sizeDownPercentage"></param>
        /// <returns></returns>
        public UIEditableBounds GetResizedLayoutBounds(double percentage)
        {
            UIEditableBounds clone = new UIEditableBounds();

            double width = this.Width * (percentage / 100);
            double height = this.Height * (percentage / 100);
            if (width < 0) width = 0;
            if (height < 0) height = 0;

            double xGap = (this.Width - width) / 2;
            double yGap = (this.Height - height) / 2;
            if (xGap < 0) xGap = 0;
            if (yGap < 0) yGap = 0;

            clone.X = this.X + xGap;
            clone.Y = this.Y + yGap;
            clone.Width = width;
            clone.Height = height;
            clone.Angle = this.Angle;
            return clone;
        }

        public static Rect GetRectFromLayoutBounds(UIEditableBounds layoutBounds)
        {
            if (layoutBounds == null)
                return Rect.Empty;

            Rect rect = new Rect();
            rect.X = layoutBounds.X;
            rect.Y = layoutBounds.Y;
            rect.Width = layoutBounds.Width;
            rect.Height = layoutBounds.Height;

            return rect;
        }

        public static UIEditableBounds GetLayoutBoundsFromRect(Rect rect)
        {
            if (rect == Rect.Empty)
                return null;

            return new UIEditableBounds(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public bool IsEqualValue(UIEditableBounds layoutBounds)
        {
            if (this.X == layoutBounds.X && this.Y == layoutBounds.Y &&
                this.Width == layoutBounds.Width && this.Height == layoutBounds.Height &&
                this.Angle == layoutBounds.Angle)
                return true;

            return false;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4}", this.X, this.Y, this.Width, this.Height, this.Angle);
        }

        public static UIEditableBounds Parse(string data)
        {
            UIEditableBounds result = new UIEditableBounds();
            if (string.IsNullOrWhiteSpace(data) == true)
            {
                return result;
            }
            data = data.Replace(" ", "");

            int i = 0;
            foreach (var value in data.Split(','))
            {
                double n = Convert.ToDouble(value);

                switch (i++)
                {
                    case 0:
                        result.X = n;
                        break;
                    case 1:
                        result.Y = n;
                        break;
                    case 2:
                        result.Width = n;
                        break;
                    case 3:
                        result.Height = n;
                        break;
                    case 4:
                        result.Angle = n;
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// LeftTop Point
        /// [NCS-3103] : Refractive index 창에서 scan image zoom in/out 기능 구현되지 않음
        /// </summary>
        public Point Location
        {
            get
            {
                return new Point(this.X, this.Y);
            }
        }
    }
}
