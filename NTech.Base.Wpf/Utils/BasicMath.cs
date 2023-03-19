using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NTech.Base.Wpf.Utils
{
    public static class BasicMath
    {
        // 도 -> 라디안
        public static double DegreeToRadian(double degree) { return Math.PI * degree / 180.0d; }

        public static double DegreeNormalize(double degree)
        {
            var m = degree % 360.0d;

            if (m < 0.0d)
            {
                m += 360.0d;
            }

            return m;
        }

        /// <summary>
        /// KJH : 두점 사이의 거리.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double CalcDistanceOfTwoPoints(Point start, Point end)
        {
            return Math.Sqrt(Math.Pow((end.X - start.X), 2) + Math.Pow((end.Y - start.Y), 2));
        }

        /// <summary>
        /// KJH : 두점 사이의 거리
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double CalcDistanceOfTwoPoints(double start, double end)
        {
            return Math.Sqrt(Math.Pow(start, 2) + Math.Pow(end, 2));
        }

        // 두 점의 각도를 계산
        public static double CalcDegreeOfTwoPoints(Point start, Point end)
        {
            var deltaY = Math.Pow((end.Y - end.Y), 2d);

            var radian = Math.Atan2((end.Y - start.Y), (end.X - start.X));

            return (radian * (180d / Math.PI) + 360d) % 360d;
        }

        // 점을 회전
        public static Point CalcRotatePoint(Point pointToRotate, Point centerPoint, double degrees)
        {
            double angleInRadians = degrees * (Math.PI / 180.0d);
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);
            return new Point
            {
                X =
                    (double)
                    (cosTheta * (pointToRotate.X - centerPoint.X) -
                    sinTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.X),
                Y =
                    (double)
                    (sinTheta * (pointToRotate.X - centerPoint.X) +
                    cosTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.Y)
            };
        }

        public static Rect CalcRotateRect(Rect rectToRotate, double degrees)
        {
            return CalcRotateRect(rectToRotate, GetCenter(rectToRotate), degrees);
        }

        public static Rect CalcRotateRect(Rect rectToRotate, Point centerPoint, double degrees)
        {
            var a = CalcRotatePoint(rectToRotate.TopLeft, centerPoint, degrees);
            var b = CalcRotatePoint(rectToRotate.BottomRight, centerPoint, degrees);
            return new Rect(a, b);
        }

        /// <summary>
        /// 사각형을 회전한 후 전체를 포함하는 Rect를 구함
        /// </summary>
        /// <param name="rectToRotate"></param>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static Rect CalcRotateRectOuter(Rect rectToRotate, double degrees)
        {
            Point centerPoint = GetCenter(rectToRotate);
            return CalcRotateRectOuter(rectToRotate, centerPoint, degrees);
        }

        /// <summary>
        /// 사각형을 회전한 후 전체를 포함하는 Rect를 구함
        /// </summary>
        /// <param name="rectToRotate"></param>
        /// <param name="centerPoint"></param>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static Rect CalcRotateRectOuter(Rect rectToRotate, Point centerPoint, double degrees)
        {
            if (rectToRotate == Rect.Empty)
                return Rect.Empty;

            //모서리 네개의 점을 회전함
            Point topLeft = rectToRotate.TopLeft;
            Point topRight = rectToRotate.TopRight;
            Point bottomLeft = rectToRotate.BottomLeft;
            Point bottomRight = rectToRotate.BottomRight;

            Point topLeftRotate = BasicMath.CalcRotatePoint(topLeft, centerPoint, degrees);
            Point topRightRotate = BasicMath.CalcRotatePoint(topRight, centerPoint, degrees);
            Point bottomLeftRotate = BasicMath.CalcRotatePoint(bottomLeft, centerPoint, degrees);
            Point bottomRightRotate = BasicMath.CalcRotatePoint(bottomRight, centerPoint, degrees);

            //회전한 네개의 점들의 min, max값을 계산해서 Rect 값을 구함
            double minX = Math.Min(bottomRightRotate.X, Math.Min(bottomLeftRotate.X, Math.Min(topLeftRotate.X, topRightRotate.X)));
            double maxX = Math.Max(bottomRightRotate.X, Math.Max(bottomLeftRotate.X, Math.Max(topLeftRotate.X, topRightRotate.X)));
            double minY = Math.Min(bottomRightRotate.Y, Math.Min(bottomLeftRotate.Y, Math.Min(topLeftRotate.Y, topRightRotate.Y)));
            double maxY = Math.Max(bottomRightRotate.Y, Math.Max(bottomLeftRotate.Y, Math.Max(topLeftRotate.Y, topRightRotate.Y)));

            Rect result = new Rect(minX, minY, maxX - minX, maxY - minY);

            return result;
        }

        /// <summary>
        /// 사각형을 center 기준으로 Resize함
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="ratio">확대 비율 (ex. 10% 확대인 경우 값이 1.1임. 이때 left, top, right, bottom 각각 절반인 5%씩 확대됨)</param>
        /// <returns></returns>
        public static Rect ResizeRect(Rect rect, double ratio)
        {
            if (ratio == 1.0)
                return rect;

            double resultWidth = rect.Width * ratio;
            double gapWidth = Math.Abs(rect.Width - resultWidth) / 2;

            double resultHeight = rect.Height * ratio;
            double gapHeight = Math.Abs(rect.Height - resultHeight) / 2;

            Rect resultRect = new Rect();
            if (ratio < 1.0)
            {
                resultRect.X = rect.X + gapWidth;
                resultRect.Y = rect.Y + gapHeight;
                resultRect.Width = rect.Width - (gapWidth * 2);
                resultRect.Height = rect.Height - (gapHeight * 2);
            }
            else
            {
                resultRect.X = rect.X - gapWidth;
                resultRect.Y = rect.Y - gapHeight;
                resultRect.Width = rect.Width + (gapWidth * 2);
                resultRect.Height = rect.Height + (gapHeight * 2);
            }

            return resultRect;
        }

        public static Point GetCenter(Rect rect)
        {
            return new Point(rect.X + rect.Width * 0.5d, rect.Y + rect.Height * 0.5d);
        }

        public static Point GetLeftTopPoint(Rect rect)
        {
            return new Point(rect.X - rect.Width * 0.5d, rect.Y - rect.Height * 0.5d);
        }

        public static Point PointTryParse(string data)
        {
            Point result = new Point();
            if (string.IsNullOrWhiteSpace(data) == true)
            {
                return result;
            }

            data = data.Replace(" ", "");
            string[] arr = data.Split(',');

            double x;
            double y;

            if (arr.Length == 2 && double.TryParse(arr[0], out x) && double.TryParse(arr[1], out y))
            {
                result.X = x;
                result.Y = y;
            }

            return result;
        }

        /// <summary>
        /// [ NCS-520 : Progress Notify Merge ]
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Size SizeTryParse(string data)
        {
            Size result = new Size();
            if (string.IsNullOrWhiteSpace(data) == true)
            {
                return result;
            }

            data = data.Replace(" ", "");
            string[] arr = data.Split(',');

            double x;
            double y;

            if (arr.Length == 2 && double.TryParse(arr[0], out x) && double.TryParse(arr[1], out y))
            {
                result.Width = x;
                result.Height = y;
            }

            return result;
        }
    }
}
