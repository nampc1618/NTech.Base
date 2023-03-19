using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Brush = System.Drawing.Brush;

namespace NTech.Base.Resources.NUtils
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PixelColor
    {
        public byte Blue;
        public byte Green;
        public byte Red;
        public byte Alpha;
    }

    public class ImageUtil
    {
        #region [Interop]

        // System.Drawing.Bitmap.GetHbitmap() 호출로 생성된 GDI 비트맵 사용 후 해제 필요
        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject(IntPtr hObject);

        #endregion

        /// <summary>
        /// ImageSource에서 Pixel Color값들을 복사함
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pixels"></param>
        /// <param name="stride"></param>
        /// <param name="offset"></param>
        public static void CopyPixels(BitmapSource source, PixelColor[,] pixels, int stride, int offset)
        {
            var height = source.PixelHeight;
            var width = source.PixelWidth;
            var pixelBytes = new byte[height * width * 4];
            source.CopyPixels(pixelBytes, stride, 0);
            int y0 = offset / width;
            int x0 = offset - width * y0;
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    pixels[y + y0, x + x0] = new PixelColor
                    {
                        Blue = pixelBytes[(y * width + x) * 4 + 0],
                        Green = pixelBytes[(y * width + x) * 4 + 1],
                        Red = pixelBytes[(y * width + x) * 4 + 2],
                        Alpha = pixelBytes[(y * width + x) * 4 + 3],
                    };
        }

        /// <summary>
        /// ImageSource에서 Pixel Color값들을 추출함
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static PixelColor[,] GetPixelColorsFromImageSource(BitmapSource source)
        {
            // [NCS-3556] : Vision변경 사항 검증
            if (source == null) { return null; }

            if (source.Format != PixelFormats.Bgra32)
                source = new FormatConvertedBitmap(source, PixelFormats.Bgra32, null, 0);

            int width = source.PixelWidth;
            int height = source.PixelHeight;
            PixelColor[,] result = new PixelColor[height, width];

            CopyPixels(source, result, width * 4, 0);
            return result;
        }

        /// <summary>
        /// Pixel당 1 Byte인 Data에서 PixelColor List를 구함
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="pixelWidth"></param>
        /// <param name="pixelHeight"></param>
        /// <returns></returns>
        public static PixelColor[,] GetPixelColorsFromOneColorBytes(byte[] bytes, int pixelWidth, int pixelHeight)
        {
            PixelColor[,] pixelColorList = new PixelColor[pixelHeight, pixelWidth];
            int index = -1;
            for (int row = 0; row < pixelHeight; row++)
            {
                for (int col = 0; col < pixelWidth; col++)
                {
                    index++;
                    pixelColorList[row, col].Blue = bytes[index];
                    pixelColorList[row, col].Green = bytes[index];
                    pixelColorList[row, col].Red = bytes[index];
                    pixelColorList[row, col].Alpha = 255;
                }
            }

            return pixelColorList;
        }

        /// <summary>
        /// Pixel당 3 Byte인 Data에서 PixelColor List를 구함
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="pixelWidth"></param>
        /// <param name="pixelHeight"></param>
        /// <returns></returns>
        public static PixelColor[,] GetPixelColorsFromThreeColorBytes(byte[] bytes, int pixelWidth, int pixelHeight)
        {
            PixelColor[,] pixelColorList = new PixelColor[pixelHeight, pixelWidth];
            int index = -1;
            for (int row = 0; row < pixelHeight; row++)
            {
                for (int col = 0; col < pixelWidth; col++)
                {
                    index++;

                    if (index * 3 + 2 < bytes.Length)
                    {
                        pixelColorList[row, col].Blue = bytes[index * 3];
                        pixelColorList[row, col].Green = bytes[index * 3 + 1];
                        pixelColorList[row, col].Red = bytes[index * 3 + 2];
                        pixelColorList[row, col].Alpha = 255;
                    }
                }
            }

            return pixelColorList;
        }

        /// <summary>
        /// Pixel당 4 Byte인 Data에서 PixelColor List를 구함
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="pixelWidth"></param>
        /// <param name="pixelHeight"></param>
        /// <returns></returns>
        public static PixelColor[,] GetPixelColorsFromFourColorBytes(byte[] bytes, int pixelWidth, int pixelHeight)
        {
            PixelColor[,] pixelColorList = new PixelColor[pixelHeight, pixelWidth];
            int index = -1;
            for (int row = 0; row < pixelHeight; row++)
            {
                for (int col = 0; col < pixelWidth; col++)
                {
                    index++;
                    pixelColorList[row, col].Blue = bytes[index * 4];
                    pixelColorList[row, col].Green = bytes[index * 4 + 1];
                    pixelColorList[row, col].Red = bytes[index * 4 + 2];
                    pixelColorList[row, col].Alpha = bytes[index * 4 + 3];
                }
            }

            return pixelColorList;
        }

        ///// <summary>
        ///// ARGB Color값을 가진 Data에서 PixelColor의 List를 구함
        ///// </summary>
        ///// <param name="bytes"></param>
        ///// <param name="pixelWidth"></param>
        ///// <param name="pixelHeight"></param>
        ///// <returns></returns>
        //public static PixelColor[,] GetPixelColorsFromARGBBytes(byte[] bytes, int pixelWidth, int pixelHeight)
        //{
        //    PixelColor[,] pixelColorList = new PixelColor[pixelHeight, pixelWidth];
        //    int index = -1;
        //    for (int row = 0; row < pixelHeight; row++)
        //    {
        //        for (int col = 0; col < pixelWidth; col++)
        //        {
        //            index = (row * pixelWidth * 4) + (col * 4);
        //            pixelColorList[row, col].Blue = bytes[index];
        //            pixelColorList[row, col].Green = bytes[index + 1];
        //            pixelColorList[row, col].Red = bytes[index + 2];
        //            pixelColorList[row, col].Alpha = bytes[index + 3];
        //        }
        //    }

        //    return pixelColorList;
        //}

        /// <summary>
        /// PixelColor Array를 ImageSource로 변환
        /// </summary>
        /// <param name="pixels"></param>
        /// <param name="pixelWidth"></param>
        /// <param name="pixelHeight"></param>
        /// <returns></returns>
        public static ImageSource GetImageSourceFromPixelColors(PixelColor[,] pixels, int pixelWidth, int pixelHeight)
        {
            if (pixelWidth <= 0 || pixelHeight <= 0)
                return null;

            // Define parameters used to create the BitmapSource.
            System.Windows.Media.PixelFormat pf = PixelFormats.Bgra32;
            int rawStride = (pixelWidth * pf.BitsPerPixel + 7) / 8;

            byte[] rgbByteData = new byte[pixelWidth * 4 * pixelHeight];

            int index = -1;
            for (int row = 0; row < pixelHeight; row++)
            {
                for (int col = 0; col < pixelWidth; col++)
                {
                    index++;
                    rgbByteData[index * 4] = (byte)pixels[row, col].Blue;
                    rgbByteData[index * 4 + 1] = (byte)pixels[row, col].Green;
                    rgbByteData[index * 4 + 2] = (byte)pixels[row, col].Red;
                    rgbByteData[index * 4 + 3] = (byte)pixels[row, col].Alpha;
                }
            }

            // Create a BitmapSource.
            BitmapSource bitmapSource = BitmapSource.Create(pixelWidth, pixelHeight, 96, 96, pf, null, rgbByteData, rawStride);
            bitmapSource?.Freeze();

            return bitmapSource;
        }


        /// <summary>
        /// 단색 Image를 생성함 (1Pixel).
        /// ImageControl에 Binding 한 후 Drag시 속도가 빠름
        /// </summary>
        /// <param name="pixelColor"></param>
        /// <returns></returns>
        public static ImageSource GetOneColorImageSource(PixelColor pixelColor)
        {
            //var bitmap = BitmapImage.Create(2, 2, 96, 96, PixelFormats.Indexed1, new BitmapPalette(new List<System.Windows.Media.Color> { Colors.Transparent }), new byte[] { 0, 0, 0, 0 }, 1);
            var bitmap = BitmapSource.Create(1, 1, 96, 96, PixelFormats.Bgr24, null, new byte[3] { pixelColor.Blue, pixelColor.Green, pixelColor.Red }, 3);
            bitmap?.Freeze();
            //var result = new TransformedBitmap(bitmap, new ScaleTransform(pixelWidth, pixelHeight));
            return bitmap;
        }

        /// <summary>
        /// 단색 Image를 생성함 (Pixel 넓이, 높이 적용)
        /// ImageControl에 Binding 한 후 Drag시 속도가 느림
        /// </summary>
        /// <param name="pixelWidth"></param>
        /// <param name="pixelHeight"></param>
        /// <param name="pixelColor"></param>
        /// <returns></returns>
        public static ImageSource GetOneColorImageSource(int pixelWidth, int pixelHeight, PixelColor pixelColor)
        {
            //var bitmap = BitmapImage.Create(2, 2, 96, 96, PixelFormats.Indexed1, new BitmapPalette(new List<System.Windows.Media.Color> { Colors.Transparent }), new byte[] { 0, 0, 0, 0 }, 1);
            var bitmap = BitmapSource.Create(1, 1, 96, 96, PixelFormats.Bgr24, null, new byte[3] { pixelColor.Blue, pixelColor.Green, pixelColor.Red }, 3);
            var result = new TransformedBitmap(bitmap, new ScaleTransform(pixelWidth, pixelHeight));
            result.Freeze();

            return result;
        }

        /// <summary>
        /// 단색 Image를 생성함
        /// </summary>
        /// <param name="pixelWidth"></param>
        /// <param name="pixelHeight"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static ImageSource GetOneColorImageSource_Old(int pixelWidth, int pixelHeight, PixelColor pixelColor)
        {
            // Define parameters used to create the BitmapSource.
            System.Windows.Media.PixelFormat pf = PixelFormats.Bgra32;
            int rawStride = (pixelWidth * pf.BitsPerPixel + 7) / 8;

            byte[] rgbByteData = new byte[pixelWidth * 4 * pixelHeight];

            int index = -1;
            for (int row = 0; row < pixelHeight; row++)
            {
                for (int col = 0; col < pixelWidth; col++)
                {
                    index++;

                    rgbByteData[index * 4] = (byte)pixelColor.Blue;
                    rgbByteData[index * 4 + 1] = (byte)pixelColor.Green;
                    rgbByteData[index * 4 + 2] = (byte)pixelColor.Red;
                    rgbByteData[index * 4 + 3] = (byte)pixelColor.Alpha;

                    ////테두리색
                    ////if (row == 0 || row == pixelHeight - 1 || col == 0 || col == pixelWidth - 1)
                    ////{
                    ////    rgbByteData[index * 4] = (byte)255;
                    ////    rgbByteData[index * 4 + 1] = (byte)255;
                    ////    rgbByteData[index * 4 + 2] = (byte)255;
                    ////    rgbByteData[index * 4 + 3] = (byte)255;
                    ////}
                    //////바탕색
                    ////else
                    //{
                    //    //rgbByteData[index * 4] = (byte)pixels[row, col].Blue;
                    //    //rgbByteData[index * 4 + 1] = (byte)pixels[row, col].Green;
                    //    //rgbByteData[index * 4 + 2] = (byte)pixels[row, col].Red;
                    //    //rgbByteData[index * 4 + 3] = (byte)pixels[row, col].Alpha;
                    //    rgbByteData[index * 4] = (byte)30;
                    //    rgbByteData[index * 4 + 1] = (byte)30;
                    //    rgbByteData[index * 4 + 2] = (byte)30;
                    //    rgbByteData[index * 4 + 3] = (byte)255;
                    //}
                }
            }

            // Create a BitmapSource.
            BitmapSource bitmapSource = BitmapSource.Create(pixelWidth, pixelHeight, 96, 96, pf, null, rgbByteData, rawStride);

            return bitmapSource;
        }

        /// <summary>
        /// PixelColor Array를 Gray ImageSource로 변환
        /// </summary>
        /// <param name="pixels"></param>
        /// <param name="pixelWidth"></param>
        /// <param name="pixelHeight"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static ImageSource GrayScaleFromPixelColors(PixelColor[,] pixels, int pixelWidth, int pixelHeight, int threshold)
        {
            // Define parameters used to create the BitmapSource.
            System.Windows.Media.PixelFormat pf = PixelFormats.Bgra32;
            int rawStride = (pixelWidth * pf.BitsPerPixel + 7) / 8;

            byte[] rgbByteData = new byte[pixelWidth * 4 * pixelHeight];

            int index = -1;
            for (int row = 0; row < pixelHeight; row++)
            {
                for (int col = 0; col < pixelWidth; col++)
                {
                    index++;

                    byte b = (byte)pixels[row, col].Blue;
                    byte g = (byte)pixels[row, col].Green;
                    byte r = (byte)pixels[row, col].Red;
                    byte a = (byte)pixels[row, col].Alpha;

                    int grayScale = (int)(r * 0.299 + g * 0.578 + b * 0.114);

                    rgbByteData[index * 4] = (byte)grayScale;
                    rgbByteData[index * 4 + 1] = (byte)grayScale;
                    rgbByteData[index * 4 + 2] = (byte)grayScale;
                    rgbByteData[index * 4 + 3] = 255;
                }
            }

            // Create a BitmapSource.
            BitmapSource bitmapSource = BitmapSource.Create(pixelWidth, pixelHeight, 96, 96, pf, null, rgbByteData, rawStride);

            return bitmapSource;
        }

        /// <summary>
        /// PixelColor Array를 이진화함 (Threshold 이하 값은 흰색으로, 나머지 값은 parameter로 넘어온 Color로 변경)
        /// </summary>
        /// <param name="pixels"></param>
        /// <param name="pixelWidth"></param>
        /// <param name="pixelHeight"></param>
        /// <param name="threshold"></param>
        /// <param name="changeColor"></param>
        /// <returns></returns>
        public static ImageSource BinarizationRedFromPixelColors(PixelColor[,] pixels, int pixelWidth, int pixelHeight, int threshold, System.Windows.Media.Color changeColor)
        {
            // Define parameters used to create the BitmapSource.
            System.Windows.Media.PixelFormat pf = PixelFormats.Bgra32;
            int rawStride = (pixelWidth * pf.BitsPerPixel + 7) / 8;

            byte[] rgbByteData = new byte[pixelWidth * 4 * pixelHeight];

            int index = -1;
            for (int row = 0; row < pixelHeight; row++)
            {
                for (int col = 0; col < pixelWidth; col++)
                {
                    index++;

                    byte b = (byte)pixels[row, col].Blue;
                    byte g = (byte)pixels[row, col].Green;
                    byte r = (byte)pixels[row, col].Red;
                    byte a = (byte)pixels[row, col].Alpha;

                    int grayScale = (int)(r * 0.299 + g * 0.578 + b * 0.114);
                    // [NCS-3181] FIducial threshold image do not update according brightness and threshold parameters
                    // CWR: threshold 초과에서 이상으로 변경.
                    if (grayScale <= threshold)
                    {
                        rgbByteData[index * 4] = 255;
                        rgbByteData[index * 4 + 1] = 255;
                        rgbByteData[index * 4 + 2] = 255;
                        rgbByteData[index * 4 + 3] = 255;
                    }
                    else
                    {
                        rgbByteData[index * 4] = changeColor.B;
                        rgbByteData[index * 4 + 1] = changeColor.G;
                        rgbByteData[index * 4 + 2] = changeColor.R;
                        rgbByteData[index * 4 + 3] = changeColor.A;
                    }
                }
            }

            // Create a BitmapSource.
            BitmapSource bitmapSource = BitmapSource.Create(pixelWidth, pixelHeight, 96, 96, pf, null, rgbByteData, rawStride);

            return bitmapSource;
        }

        /// <summary>
        /// PixelColor Array를 이진화함 (Threshold 이하 값은 흰색으로, 나머지 값은 그대로 유지)
        /// </summary>
        /// <param name="pixels"></param>
        /// <param name="pixelWidth"></param>
        /// <param name="pixelHeight"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static ImageSource BinarizationFromPixelColors(PixelColor[,] pixels, int pixelWidth, int pixelHeight, int threshold)
        {
            // Define parameters used to create the BitmapSource.
            System.Windows.Media.PixelFormat pf = PixelFormats.Bgra32;
            int rawStride = (pixelWidth * pf.BitsPerPixel + 7) / 8;

            byte[] rgbByteData = new byte[pixelWidth * 4 * pixelHeight];

            int index = -1;
            for (int row = 0; row < pixelHeight; row++)
            {
                for (int col = 0; col < pixelWidth; col++)
                {
                    index++;

                    byte b = (byte)pixels[row, col].Blue;
                    byte g = (byte)pixels[row, col].Green;
                    byte r = (byte)pixels[row, col].Red;
                    byte a = (byte)pixels[row, col].Alpha;

                    int grayScale = (int)(r * 0.299 + g * 0.578 + b * 0.114);

                    if (grayScale < threshold)
                    {
                        b = 255;
                        g = 255;
                        r = 255;
                        a = 255;
                    }
                    else
                    {
                        //do nothing..
                    }


                    rgbByteData[index * 4] = b;
                    rgbByteData[index * 4 + 1] = g;
                    rgbByteData[index * 4 + 2] = r;
                    rgbByteData[index * 4 + 3] = a;
                }
            }

            // Create a BitmapSource.
            BitmapSource bitmapSource = BitmapSource.Create(pixelWidth, pixelHeight, 96, 96, pf, null, rgbByteData, rawStride);

            return bitmapSource;
        }

        /// <summary>
        /// Image 위에 Text 표시
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="position"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static BitmapImage WriteTextOverImage(string inputFile, System.Windows.Point position, string text, int fontSize = 500)
        {
            BitmapImage bitmap = new BitmapImage(new Uri(inputFile)); // inputFile must be absolute path
            DrawingVisual visual = new DrawingVisual();

            FormattedText formattedText = new FormattedText(text, CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight, new Typeface("Tahoma"), fontSize, System.Windows.Media.Brushes.Red,
                VisualTreeHelper.GetDpi(visual).PixelsPerDip);

            using (DrawingContext dc = visual.RenderOpen())
            {
                dc.DrawImage(bitmap, new Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));
                dc.DrawText(formattedText, position);
            }

            RenderTargetBitmap target = new RenderTargetBitmap(bitmap.PixelWidth, bitmap.PixelHeight,
                                                               bitmap.DpiX, bitmap.DpiY, PixelFormats.Default);
            target.Render(visual);
            target.Freeze();

            return ConvertRenderTargetBitmapToBitmapImage(target);
        }

        /// <summary>
        /// RenderTargetBitmap를 BitmapImage로 변환
        /// </summary>
        /// <param name="renderTargetBitmap"></param>
        /// <returns></returns>
        private static BitmapImage ConvertRenderTargetBitmapToBitmapImage(RenderTargetBitmap renderTargetBitmap)
        {
            var bitmapImage = new BitmapImage();
            var bitmapEncoder = new PngBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            using (var stream = new MemoryStream())
            {
                bitmapEncoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);

                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
            }
            bitmapImage.Freeze();

            return bitmapImage;
        }



        #region OpenCV

        /// <summary>
        /// FOV Bayer Image를 RGB 형식으로 변환함
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static ImageSource ConvertBayerImageToRGB(string bayerImageFilePath)
        {
            try
            {
                //string filePath = @"C:\Kohyoung\AOI\FOV_IMG\LiveImg\1\Img_1.bmp";
                if (!File.Exists(bayerImageFilePath))
                {
                    //MessageBox.Show("이미지 파일이 존재하지 않음 !!");
                    return null;
                }

                //Bayer Image (Source)
                OpenCvSharp.CPlusPlus.Mat src = OpenCvSharp.CPlusPlus.Cv2.ImRead(bayerImageFilePath, LoadMode.AnyDepth);

                //RGB 형식
                OpenCvSharp.CPlusPlus.Mat dst = new OpenCvSharp.CPlusPlus.Mat(src.Size(), OpenCvSharp.CPlusPlus.MatType.CV_8UC3);
                //Mat dst = new Mat(src.Size(), MatType.CV_32FC1);

                //Image 변환
                OpenCvSharp.CPlusPlus.Cv2.CvtColor(src, dst, ColorConversion.BayerGrToRgb);

                //변환 Image
                using (Bitmap bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(dst))
                {
                    //if (Keyboard.IsKeyDown(Key.LeftShift) && Keyboard.IsKeyDown(Key.LeftCtrl))
                    //    return BitmapToImageSource(bitmap);
                    //return BitmapToBitmapImage(bitmap);

                    return BitmapToImageSource(bitmap);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                //throw;
                //LogHub.Write(string.Format("[CCI - OpenCV] ImageUtil.ConvertBayerImageToRGB() 실패. path = {0}", bayerImageFilePath));
            }

            return null;
        }

        /// <summary>
        /// OpenCV IplImage를 BitmapImage로 변환
        /// </summary>
        /// <param name="ipl"></param>
        /// <returns></returns>
        private static ImageSource ConvertIplToBitmapImage(IplImage ipl)
        {
            using (Bitmap bitmap = BitmapConverter.ToBitmap(ipl))
            {
                return BitmapToImageSource(bitmap);
            }
        }

        /// <summary>
        /// IplImage에서 r,g,b를 뽑아냄
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public IplImage Split(IplImage src)
        {
            IplImage b = new IplImage(src.Size, BitDepth.U8, 1);
            IplImage g = new IplImage(src.Size, BitDepth.U8, 1);
            IplImage r = new IplImage(src.Size, BitDepth.U8, 1);

            Cv.Split(src, b, g, r, null);

            return r;
            //return g;
            //return b;                
        }

        /// <summary>
        /// IplImage에서 r,g,b값을 뽑아내기, 합치기 Sample (Split/Merge)
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public IplImage Merge(IplImage src)
        {
            IplImage merge = new IplImage(src.Size, BitDepth.U8, 3);
            IplImage b = new IplImage(src.Size, BitDepth.U8, 1);
            IplImage g = new IplImage(src.Size, BitDepth.U8, 1);
            IplImage r = new IplImage(src.Size, BitDepth.U8, 1);

            Cv.Split(src, b, g, r, null);
            Cv.Merge(b, g, r, null, merge);


            return merge;
        }

        #endregion OpenCV


        //==========================================

        public static Bitmap Binarization(Bitmap bitmap, int threshold)
        {
            //Bitmap 새로 생성
            Bitmap resultBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    //Pixel Color 획득
                    System.Drawing.Color color = bitmap.GetPixel(i, j);

                    //RGB 평균값
                    int average = (int)((color.R + color.G + color.B) / 3);

                    //RGB 평균치가 T값 이하인 경우 백색으로 설정
                    if (average <= threshold)
                    {
                        resultBitmap.SetPixel(i, j, System.Drawing.Color.White);
                    }
                    //RGB 평균치가 T값보다 큰 경우 흑색으로 설정
                    else
                    {
                        resultBitmap.SetPixel(i, j, System.Drawing.Color.Black);
                    }
                }
            }

            return resultBitmap;
        }

        public static Bitmap Grayscale(Bitmap bitmap)
        {
            //Bitmap 새로 생성
            Bitmap resultBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    //Get the Pixel
                    System.Drawing.Color bitmapColor = bitmap.GetPixel(x, y);

                    //Declare grayScale as the Grayscale Pixel
                    //int grayScale = (int)((bitmapColor.R * 0.3) + (bitmapColor.G * 0.59) + (bitmapColor.B * 0.11));

                    // Transform RGB to Y (gray scale)
                    int grayScale = (int)(bitmapColor.R * 0.299 + bitmapColor.G * 0.578 + bitmapColor.B * 0.114);

                    //Declare myColor as a Grayscale Color
                    System.Drawing.Color myColor = System.Drawing.Color.FromArgb(grayScale, grayScale, grayScale);

                    //Set the Grayscale Pixel
                    resultBitmap.SetPixel(x, y, myColor);
                }
            }

            return resultBitmap;
        }

        /// <summary>
        /// BitmapSource를 BitmapImage로 변환
        /// </summary>
        /// <param name="bitmapSource"></param>
        /// <returns></returns>
        public static BitmapImage BitmapSourceToBitmapImage(BitmapSource bitmapSource)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            MemoryStream memoryStream = new MemoryStream();
            BitmapImage bImg = new BitmapImage();

            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            encoder.Save(memoryStream);

            memoryStream.Position = 0;
            bImg.BeginInit();
            bImg.StreamSource = memoryStream;
            bImg.EndInit();
            bImg.Freeze();

            memoryStream.Close();

            return bImg;
        }

        public static System.Drawing.Bitmap BitmapSourceToBitmap2(BitmapSource srs)
        {
            int width = srs.PixelWidth;
            int height = srs.PixelHeight;
            int stride = width * ((srs.Format.BitsPerPixel + 7) / 8);
            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.AllocHGlobal(height * stride);
                srs.CopyPixels(new Int32Rect(0, 0, width, height), ptr, height * stride, stride);
                using (var btm = new System.Drawing.Bitmap(width, height, stride, System.Drawing.Imaging.PixelFormat.Format1bppIndexed, ptr))
                {
                    // Clone the bitmap so that we can dispose it and
                    // release the unmanaged memory at ptr
                    return new System.Drawing.Bitmap(btm);
                }
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                    Marshal.FreeHGlobal(ptr);
            }
        }

        //BitmapImage를 WPF Image.Source에 Binding할수 있음 (BitmapToImageSource 보다 속도가 느림)
        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            if (null == bitmap)
            {
                return null;
            }

            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                bitmapimage.Freeze();

                return bitmapimage;
            }
        }

        //BitmapImage를 WPF Image.Source에 Binding할수 있음 (BitmapToBitmapImage 보다 속도 빠름)
        public static ImageSource BitmapToImageSource(Bitmap bitmap)
        {
            if(null == bitmap)
            {
                return null;
            }

            var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height,
                bitmap.HorizontalResolution, bitmap.VerticalResolution,
                PixelFormats.Bgr24, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);

            return bitmapSource;
        }

        /// <summary>
        /// [NCS-1867] BoardSetting 팝업의 ClipBoard 기능오류
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static ImageSource BitmapToImageSource2(Bitmap src)
        {
            BitmapImage image = new BitmapImage();
            using (MemoryStream ms = new MemoryStream())
            {
                ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                image.BeginInit();
                ms.Seek(0, SeekOrigin.Begin);
                image.StreamSource = ms;
                image.EndInit();
            }
            image.Freeze();

            return image;
        }

        public static byte[] BitmapImageToByte(BitmapImage bitmapImage)
        {
            Stream stream = bitmapImage.StreamSource;
            byte[] buffer = null;

            if (stream != null && stream.Length > 0)
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    buffer = br.ReadBytes((Int32)stream.Length);
                }
            }

            return buffer;
        }

        public static BitmapImage ByteToBitmapImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length <= 0)
                return null;

            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();

            return image;
        }


        //public static byte[] BitmapImageToByte2(BitmapImage bitmapImage)
        //{
        //    byte[] buffer = null;

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        bitmapImage.Save(ms);
        //        buffer = ms.ToArray();
        //    }

        //    return buffer;
        //}

        public static System.Windows.Media.ImageSource ByteToImageSource(byte[] buffer)
        {
            var width = 100;
            var height = 100;
            var dpiX = 96d;
            var dpiY = 96d;
            var pixelFormat = System.Windows.Media.PixelFormats.Pbgra32;
            var bytesPerPixel = (pixelFormat.BitsPerPixel + 7) / 8;
            var stride = bytesPerPixel * width;

            var imageSource = BitmapSource.Create(width, height, dpiX, dpiY, pixelFormat, null, buffer, stride);

            return imageSource;
        }

        public static RenderTargetBitmap ElementToBitmapImage(FrameworkElement element)
        {
            System.Windows.Media.DrawingVisual drawingVisual = new System.Windows.Media.DrawingVisual();
            System.Windows.Media.DrawingContext drawingContext = drawingVisual.RenderOpen();
            Rect rect = new Rect(0, 0, element.ActualWidth, element.ActualHeight);
            drawingContext.DrawRectangle(new System.Windows.Media.VisualBrush(element), null, rect);
            drawingContext.Close();

            RenderTargetBitmap target = new RenderTargetBitmap((int)element.ActualWidth, (int)element.ActualHeight, 96, 96, System.Windows.Media.PixelFormats.Pbgra32);
            target.Render(drawingVisual);
            target.Freeze();

            return target;
        }

        /// <summary>
        /// FrameworkElement to RenderTargetBitmap
        /// [NCS-983] : [Intro] PCB Preview 창 대응
        /// </summary>
        /// <param name="element">soure element</param>
        /// <param name="margin">Left/Top/Right/Bottom</param>
        /// <param name="background">Background Color</param>
        /// <returns>RenderTargetBitmap</returns>
        public static ImageSource ElementToBitmapImage(FrameworkElement element, System.Drawing.Size renderSize, System.Windows.Media.Stretch stretch, Tuple<double, double, double, double> margin, System.Windows.Media.SolidColorBrush background)
        {
            if (element == null) { return null; }

            Rect rectBackground = new Rect();
            Rect rectElement = new Rect();

            // Element Size
            if (renderSize == null || stretch == Stretch.None)
            {
                rectBackground = new Rect(0, 0, element.ActualWidth + margin.Item1 + margin.Item3, element.ActualHeight + margin.Item2 + margin.Item4);
                rectElement = new Rect(margin.Item1, margin.Item2, element.ActualWidth, element.ActualHeight);
            }
            else
            {
                switch (stretch)
                {
                    case Stretch.Fill:
                        {
                            rectBackground = new Rect(0, 0, renderSize.Width + margin.Item1 + margin.Item3, renderSize.Height + margin.Item2 + margin.Item4);
                            rectElement = new Rect(margin.Item1, margin.Item2, renderSize.Width, renderSize.Height);
                        }
                        break;
                    case Stretch.Uniform:
                        {
                            // W/H 비율
                            var ratioX = renderSize.Width / element.ActualWidth;
                            var ratioY = renderSize.Height / element.ActualHeight;
                            var ratio = ratioX< ratioY ? ratioX : ratioY;

                            double renderWidth = element.ActualWidth * ratio;
                            double renderHeight = element.ActualHeight * ratio;

                            double x = margin.Item1;
                            double y = margin.Item2;
                            if (renderSize.Width > renderWidth)
                            {
                                x += (renderSize.Width - renderWidth) * 0.5;
                            }
                            if (renderSize.Height > renderHeight)
                            {
                                y += (renderSize.Height - renderHeight) * 0.5;
                            }

                            rectBackground = new Rect(0, 0, renderSize.Width + margin.Item1 + margin.Item3, renderSize.Height + margin.Item2 + margin.Item4);
                            rectElement = new Rect(x, y, renderWidth, renderHeight);
                        }
                        break;
                    case Stretch.UniformToFill:
                        {
                            rectBackground = new Rect(0, 0, renderSize.Width + margin.Item1 + margin.Item3, renderSize.Height + margin.Item2 + margin.Item4);
                            rectElement = new Rect(margin.Item1, margin.Item2, element.ActualWidth, element.ActualHeight);
                        }
                        break;
                }
            }

            System.Windows.Media.DrawingVisual drawingVisual = new System.Windows.Media.DrawingVisual();
            System.Windows.Media.DrawingContext drawingContext = drawingVisual.RenderOpen();
            if (background != null) { drawingContext.DrawRectangle(background, null, rectBackground); }         // Background
            drawingContext.DrawRectangle(new System.Windows.Media.VisualBrush(element), null, rectElement);     // Element
            drawingContext.Close();
            
            RenderTargetBitmap target = new RenderTargetBitmap((int)rectBackground.Width, (int)rectBackground.Height, 96, 96, System.Windows.Media.PixelFormats.Pbgra32);
            target.Render(drawingVisual);
            target.Freeze();

            // [NCS-1867] BoardSetting 팝업의 ClipBoard 기능오류
            MemoryStream stream = new MemoryStream();
            BitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(target));
            encoder.Save(stream);

            using (Bitmap bitmap = new Bitmap(stream))
            {
                return ImageUtil.BitmapToImageSource2(bitmap);
            }
        }

        public static byte[] UriToByte(Uri uri)
        {
            byte[] imageBytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                //encoder.Frames.Add(BitmapFrame.Create(new Uri(img.Source.ToString(), UriKind.RelativeOrAbsolute)));
                encoder.Frames.Add(BitmapFrame.Create(uri));
                encoder.Save(ms);
                imageBytes = ms.ToArray();
            }

            return imageBytes;
        }

        #region CropImage

        /// <summary>
        /// 이미지를 Crop 한 결과를 반환합니다.
        /// </summary>
        /// <param name="imageSource">원본이미지입니다.</param>
        /// <param name="x">잘라낼 x 축 값입니다.</param>
        /// <param name="y">잘라낼 y 축 값입니다.</param>
        /// <param name="width">잘라낼 너비값입니다.</param>
        /// <param name="height">잘라낼 높이값입니다.</param>
        /// <returns>잘라낸 이미지입니다.</returns>
        public static ImageSource CropImage(ImageSource imageSource, double x, double y, double width, double height)
        {
            var cropBitmap = new CroppedBitmap((BitmapSource)imageSource, new Int32Rect((int)x, (int)y, (int)width, (int)height));
            cropBitmap.Freeze();

            return cropBitmap;
        }

        /// <summary>
        /// 이미지를 Crop 한 결과를 반환합니다.
        /// </summary>
        /// <param name="imageSource">원본이미지입니다.</param>
        /// <param name="x">잘라낼 x 축 값입니다.</param>
        /// <param name="y">잘라낼 y 축 값입니다.</param>
        /// <param name="width">잘라낼 너비값입니다.</param>
        /// <param name="height">잘라낼 높이값입니다.</param>
        /// <returns>잘라낸 이미지입니다.</returns>
        public static ImageSource CropImage(ImageSource imageSource, int x, int y, int width, int height)
        {
            var cropBitmap = new CroppedBitmap((BitmapSource)imageSource, new Int32Rect(x, y, width, height));
            cropBitmap.Freeze();

            return cropBitmap;
        }

        #endregion

        /// <summary>
        /// Hexa값을 Color로 변경
        /// </summary>
        /// <param name="strColor"></param>
        /// <returns></returns>
        public static SolidColorBrush GetColorFromString(string strColor)
        {
            //return new BrushConverter().ConvertFromString("#FFFFFF") as SolidColorBrush;
            return new BrushConverter().ConvertFromString(strColor) as SolidColorBrush;
        }

        /// <summary>
        /// Hexa값을 SolidColorBrush 로 변경 
        /// [NCS-898] ColorUtil 로 이동해야할거 같음 GetColorFromString 이 여기 있어 일단 여기로 처리
        /// </summary>
        /// <param name="strColor"></param>
        /// <param name="opacity"></param>
        /// <returns></returns>
        public static SolidColorBrush GetColorFromString(string strColor, double opacity)
        {
            SolidColorBrush solidColorBrush = new BrushConverter().ConvertFromString(strColor) as SolidColorBrush;
            solidColorBrush.Opacity = opacity;

            return solidColorBrush;
        }

        /// <summary>
        /// rgb 값을 SolidColorBrush 로 변경 [NCS-898]
        /// </summary>
        /// <param name="strColor"></param>
        /// <param name="opacity"></param>
        /// <returns></returns>
        public static SolidColorBrush GetColorFromRgb(byte red, byte green, byte blue, double opacity)
        {
            SolidColorBrush solidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, (byte)red, (byte)green, (byte)blue));
            solidColorBrush.Opacity = opacity;

            return solidColorBrush;
        }

        private static ObservableCollection<OpenCvSharp.CPlusPlus.Mat> _currentMat=new ObservableCollection<OpenCvSharp.CPlusPlus.Mat>();
        public static void setCurrentMat(string bayerImageFilePath,int index)
        {
            if (index>_currentMat.Count)
                _currentMat.Add(OpenCvSharp.CPlusPlus.Cv2.ImRead(bayerImageFilePath, LoadMode.Color));
            else
                _currentMat[index-1] = OpenCvSharp.CPlusPlus.Cv2.ImRead(bayerImageFilePath, LoadMode.Color);
        }
        public static ImageSource ApplyBrightnessAndConstrast(double brightness, double constrast,int index)
        {
            // 해당 인덱스가 없으면 리턴
            if (_currentMat.Count == 0 || _currentMat.Count < index - 1)
                return null;

            brightness = brightness * 255 * 0.01;
            constrast = constrast / 50.0 + 1;
            using (OpenCvSharp.CPlusPlus.Mat resultBrightnessImage = new OpenCvSharp.CPlusPlus.Mat())
            {
                _currentMat[index - 1].ConvertTo(resultBrightnessImage, -1, constrast, brightness);
                using (Bitmap bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(resultBrightnessImage))
                {
                    return BitmapToImageSource(bitmap);
                }
            }
        }
        
        //public static ImageSource ReplaceColor(BitmapSource bitmapSource, double fovWidth, double fovHeight, String colorString)
        //{
        //    if (bitmapSource is null)
        //        return null;

        //    int withROI = (int)(FoundationConstants.sizeROI.Width / fovWidth * bitmapSource.PixelWidth);
        //    int heightROI = (int)(FoundationConstants.sizeROI.Height / fovHeight * bitmapSource.PixelHeight);
        //    int startX = 0;
        //    int startY = 0;
        //    if (fovWidth != 0)
        //        startX = (int)(((FoundationConstants.posROI.X - FoundationConstants.posFOV.X )/  fovWidth) * bitmapSource.PixelWidth) ;
        //    if (fovHeight != 0)
        //        startY = (int)(((FoundationConstants.posROI.Y - FoundationConstants.posFOV.Y )/  fovHeight) * bitmapSource.PixelHeight);
        //    int endX = startX + withROI;
        //    int endY = startY + heightROI;
        //    if (startX < 0)
        //    {
        //        endX = withROI + startX;
        //        startX = 0;
        //    }
                
        //    if (startY < 0)
        //    {
        //        endY = heightROI + startY;
        //        startY = 0;
        //    }
        //    if (endX > bitmapSource.PixelWidth)
        //        endX = bitmapSource.PixelWidth;
        //    if (endY > bitmapSource.PixelHeight)
        //        endY = bitmapSource.PixelHeight;

        //    System.Drawing.Bitmap scrBitmap = BitmapFromSource(bitmapSource);
        //    System.Drawing.Bitmap newBitmap = BitmapFromSource(bitmapSource);
        //    System.Drawing.Color actualColor;
        //    double[] svValue = { 0, 0 };
        //    int hue = 0;
        //    int saturation = 0;
        //    int lightValue;
        //    var changeColor = System.Drawing.ColorTranslator.FromHtml(colorString);
        //    for (int i = startX; i < endX; i++)
        //    {
        //        for (int j = startY; j < endY; j++)
        //        {
        //            actualColor = scrBitmap.GetPixel(i, j);

        //            svValue = ImageUtil.ConvertRGBtoHSV(actualColor.R, actualColor.G, actualColor.B);
        //            hue = (int)(actualColor.GetHue());
        //            saturation = (int)(svValue[0] * 100);
        //            lightValue = (int)(svValue[1] * 100);
        //            if (CompareColorInRange(hue, FoundationConstants.HueMin, FoundationConstants.HueMax, FoundationConstants.HueReversed)
        //                && CompareColorInRange(saturation, FoundationConstants.SaturationMin, FoundationConstants.SaturationMax, FoundationConstants.SaturationReversed)
        //                && CompareColorInRange(lightValue, FoundationConstants.LightnessMin, FoundationConstants.LightnessMax, FoundationConstants.GreyValueReversed)
        //                && String.IsNullOrWhiteSpace(colorString) == false)
        //                newBitmap.SetPixel(i, j, changeColor);
        //            else
        //                newBitmap.SetPixel(i, j, actualColor);
        //        }
        //    }

        //    var resultBitmap = ConvertBitmap(newBitmap);

        //    // Release Bitmap
        //    scrBitmap?.Dispose();
        //    newBitmap?.Dispose();

        //    return resultBitmap;
        //}

        public void caculateDestination(double startPointX, double startPointY, double endPointX, double endPointY, double width, double height)
        {
            double ax = (endPointX - startPointX) / width;
            double aY = (endPointY - startPointY) / height;
        }

        //public Color[][] GetBitMapColorMatrix(string bitmapFilePath)
        //{
        //    bitmapFilePath = @"C:\9673780.jpg";
        //    Bitmap b1 = new Bitmap(bitmapFilePath);

        //    int hight = b1.Height;
        //    int width = b1.Width;

        //    Color[][] colorMatrix = new Color[width][];
        //    for (int i = 0; i < width; i++)
        //    {
        //        colorMatrix[i] = new Color[hight];
        //        for (int j = 0; j < hight; j++)
        //        {
        //            colorMatrix[i][j] = b1.GetPixel(i, j);
        //        }
        //    }
        //    return colorMatrix;
        //}

        public static System.Drawing.Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);
            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));
            if (hi == 0) return System.Drawing.Color.FromArgb(255, v, t, p);
            else if (hi == 1) return System.Drawing.Color.FromArgb(255, q, v, p);
            else if (hi == 2) return System.Drawing.Color.FromArgb(255, p, v, t);
            else if (hi == 3) return System.Drawing.Color.FromArgb(255, p, q, v);
            else if (hi == 4) return System.Drawing.Color.FromArgb(255, t, p, v);
            else
                return System.Drawing.Color.FromArgb(255, v, p, q);
        }

        public static bool CompareColorInRange(int actualcolor, int min, int max,bool reverse)
        {
            bool result = false;
            if((actualcolor>=min) && (actualcolor <=max) && (reverse==false))
                result = true;
            if (((actualcolor < min) || (actualcolor > max)) && (reverse == true))
                result = true;
            return result;
        }

        public static BitmapSource ConvertBitmap(Bitmap source)
        {
            var hBitmap = source.GetHbitmap();
            var bitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                            hBitmap,
                            IntPtr.Zero,
                            Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());

            bitmap?.Freeze();
            DeleteObject(hBitmap);

            return bitmap;
        }

        /// <summary>
        /// WPF System.Windows.Media.Imaging.BitmapSource를 System.Drawing.Bitmap으로 변환
        /// 사용 후, Bitmap.Dispose() 호출 필요함.
        /// </summary>
        /// <param name="bitmapsource"></param>
        /// <returns></returns>
        public static System.Drawing.Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            System.Drawing.Bitmap bitmap;
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new System.Drawing.Bitmap(outStream);
            }
            return bitmap;
        }

        public static System.Windows.Media.Color SetAvgHSV(double sumHue, double sumbSaturation, double sumLightness, int Count)
        {
            if(0 == Count)
            {
                return new System.Windows.Media.Color();
            }

            double Avghue = sumHue / Count;
            double Avgsaturation = sumbSaturation / Count;
            double Avglightness = sumLightness / Count;
            double H = Avghue;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            double R, G, B;
            if (Avglightness <= 0)
            { R = G = B = 0; }
            else if (Avgsaturation <= 0)
            {
                R = G = B = Avglightness;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = Avglightness * (1 - Avgsaturation);
                double qv = Avglightness * (1 - Avgsaturation * f);
                double tv = Avglightness * (1 - Avgsaturation * (1 - f));
                switch (i)
                {
                    // Red is the dominant color
                    case 0:
                        R = Avglightness;
                        G = tv;
                        B = pv;
                        break;
                    // Green is the dominant color
                    case 1:
                        R = qv;
                        G = Avglightness;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = Avglightness;
                        B = tv;
                        break;
                    // Blue is the dominant color
                    case 3:
                        R = pv;
                        G = qv;
                        B = Avglightness;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = Avglightness;
                        break;
                    // Red is the dominant color
                    case 5:
                        R = Avglightness;
                        G = pv;
                        B = qv;
                        break;
                    // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.
                    case 6:
                        R = Avglightness;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = Avglightness;
                        G = pv;
                        B = qv;
                        break;
                    default:
                        R = G = B = Avglightness; // Just pretend its black/white
                        break;
                }
            }
            int r, g, b;
            r = Clamp((int)(R * 255.0));
            g = Clamp((int)(G * 255.0));
            b = Clamp((int)(B * 255.0));
            return System.Windows.Media.Color.FromArgb(255, (byte)r, (byte)g, (byte)b);

        }
        public static int Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }

        public static double[] ConvertRGBtoHSV(int Red, int Green, int Blue)
        {
            double[] svValue = { 0, 0 };
            double r1 = (double)Red / 255;
            double g1 = (double)Green / 255;
            double b1 = (double)Blue / 255;
            double Cmax = Math.Max(r1, Math.Max(g1, b1));
            double Cmin = Math.Min(r1, Math.Min(g1, b1));
            double delta = Cmax - Cmin;
            // Saturation Calculation
            if (Cmax == 0)
            {
                svValue[0] = 0;
            }
            else
                svValue[0] = delta / Cmax;
            svValue[1] = Cmax;
            return svValue;

        }

        public static bool doOverlap(System.Windows.Point l1, System.Windows.Point r1,
                         System.Windows.Point l2, System.Windows.Point r2)
        {
            // If one rectangle is on left side of other  
            if (l1.X >= r2.X || l2.X >= r1.X)
            {
                return false;
            }

            // If one rectangle is above other  
            if (l1.Y >= r2.Y || l2.Y >= r1.Y)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// BitmapSource - Rotate and Crop
        /// </summary>
        /// <param name="bitmapSource"></param>
        /// <param name="rect"></param>
        /// <param name="angle"></param>
        /// <param name="HighQuality"></param>
        /// <param name="isSaveToLocalFile"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Bitmap CropRotatedSource(BitmapSource bitmapSource, Rectangle rect, float angle, bool HighQuality, bool isSaveToLocalFile, string filePath = "")
        {
            using (var bitmap = BitmapFromSource(bitmapSource))
            {
                return CropRotatedBitmap(bitmap, rect, angle, HighQuality, isSaveToLocalFile, filePath);
            }
        }

        /// <summary>
        /// Bitmap - Rotate and Crop
        /// </summary>
        /// <param name="source"></param>
        /// <param name="rect"></param>
        /// <param name="angle"></param>
        /// <param name="HighQuality"></param>
        /// <param name="isSaveToLocalFile"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Bitmap CropRotatedBitmap(Bitmap source, Rectangle rect, float angle, bool HighQuality, bool isSaveToLocalFile, string filePath = "")
        {
            int[] offsets = { -1, 1, 0 };
            Bitmap result = new Bitmap(rect.Width, rect.Height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.InterpolationMode = HighQuality ? System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic : System.Drawing.Drawing2D.InterpolationMode.Default;
                foreach (int x in offsets)
                {
                    foreach (int y in offsets)
                    {
                        using (System.Drawing.Drawing2D.Matrix mat = new System.Drawing.Drawing2D.Matrix())
                        {
                            mat.Translate(-rect.Location.X - rect.Width * x, -rect.Location.Y - rect.Height * y);
                            mat.RotateAt(angle, new PointF((float)source.Width / 2, (float)source.Height / 2));

                            g.Transform = mat;
                            g.DrawImage(source, new System.Drawing.Point(0, 0));
                        }
                    }
                }
            }

            if (isSaveToLocalFile == true && string.IsNullOrWhiteSpace(filePath) == false)
            {
                var dir = System.IO.Path.GetDirectoryName(filePath);
                if (Directory.Exists(dir) == false)
                {
                    Directory.CreateDirectory(dir);
                }

                result.Save(filePath);
            }

            return result;
        }

        /// <summary>
        /// DrawingVisual을 사용해, Source 비트맵에 target 비트맵를 drawPoint 위치에 덧붙인다.
        /// [NCS-1355]
        /// </summary>
        /// <param name="source">원본 이미지</param>
        /// <param name="target">덧붙일 이미지</param>
        /// <param name="drawPixelPoint">그려질 위치</param>
        /// <param name="drawPixelSize">그려질 크기</param>
        /// <param name="angle">덧붙일 이미지 회전</param>
        /// <returns></returns>
        public static BitmapSource AttachBitmap(BitmapSource source, BitmapSource target, System.Windows.Point drawPixelPoint, System.Windows.Size drawPixelSize, double angle = 0d)
        {
            if (source == null || target == null)
            {
                return null;
            }

            var centerX = drawPixelPoint.X + drawPixelSize.Width / 2;
            var centerY = drawPixelPoint.Y + drawPixelSize.Height / 2;

            DrawingVisual visual = new DrawingVisual();
            using (DrawingContext dc = visual.RenderOpen())
            {
                dc.DrawImage(source, new Rect(0, 0, source.PixelWidth, source.PixelHeight));
                dc.PushTransform(new RotateTransform(angle, centerX, centerY));
                dc.DrawImage(target, new Rect(drawPixelPoint.X, drawPixelPoint.Y, drawPixelSize.Width, drawPixelSize.Height));
            }

            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(source.PixelWidth, source.PixelHeight, source.DpiX, source.DpiY, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(visual);
            renderTargetBitmap.Freeze();

            return renderTargetBitmap;
        }

        /// <summary>
        /// Source 비트맵에 target 비트맵를 drawPoint 위치에 덧붙인다.
        /// PixelFormat이 다른 경우, Source 비트맵 기준으로 변환한다.
        /// 범위를 초과하는 경우, Crop 된다.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="drawPoint"></param>
        /// <returns></returns>
        public static BitmapSource AttachBitmap(BitmapSource source, BitmapSource target, System.Windows.Point drawPoint)
        {
            if(source == null || target == null)
            {
                return null;
            }

            // Convert Format
            if (source.Format != target.Format)
            {
                target = ConvertPixelFormat(target, source.Format) as BitmapSource;
            }

            // Crop Over Range
            if(drawPoint.X + target.PixelWidth > source.PixelWidth)
            {
                target = (BitmapSource)CropImage(target, 0, 0, source.PixelWidth - drawPoint.X, target.Height);
            }
            if (drawPoint.Y + target.PixelHeight > source.PixelHeight)
            {
                target = (BitmapSource)CropImage(target, 0, 0, target.PixelWidth, source.PixelHeight - drawPoint.Y);
            }
            if (drawPoint.X  < 0)
            {
                var drawPointCropX = -drawPoint.X;
                target = (BitmapSource)CropImage(target, drawPointCropX, 0, target.PixelWidth - drawPointCropX, target.PixelHeight);
                drawPoint.X = 0;
            }
            if (drawPoint.Y < 0)
            {
                var drawPointCropY = -drawPoint.Y;
                target = (BitmapSource)CropImage(target, 0, drawPointCropY, target.PixelWidth, target.PixelHeight - drawPointCropY);
                drawPoint.Y = 0;
            }

            // Attach
            var targetStride = (target.PixelWidth * target.Format.BitsPerPixel + 7) / 8;
            var buffer = new byte[target.PixelHeight * targetStride];
            target.CopyPixels(buffer, targetStride, 0);

            var wb = new WriteableBitmap(source);
            wb.WritePixels(new Int32Rect((int)drawPoint.X, (int)drawPoint.Y, target.PixelWidth, target.PixelHeight), buffer, targetStride, 0);
            wb.Freeze();

            return wb;
        }

        /// <summary>
        /// 이미지 포맷을 변경한다.
        /// </summary>
        /// <param name="imageSource"></param>
        /// <param name="pixelFormat"></param>
        /// <returns></returns>
        public static ImageSource ConvertPixelFormat(ImageSource imageSource, System.Windows.Media.PixelFormat pixelFormat)
        {
            BitmapPalette palette = null;

            // 8bit Gray 이미지 팔레트
            if (pixelFormat == PixelFormats.Indexed8)
            {
                palette = BitmapPalettes.Gray256;
            }
            
            FormatConvertedBitmap newFormatedBitmapSource = new FormatConvertedBitmap(imageSource as BitmapSource, pixelFormat, palette, 0);
            newFormatedBitmapSource.Freeze();

            return newFormatedBitmapSource;
        }

        /// <summary>
        /// 이미지 사이즈를 변경한다.
        /// </summary>
        /// <param name="imageSource"></param>
        /// <param name="pixelWidth"></param>
        /// <param name="pixelHeight"></param>
        /// <returns></returns>
        //public static ImageSource ResizeImage(ImageSource imageSource, double pixelWidth, double pixelHeight)
        //{
        //    var scaleX = Foundation.Math.IEEE754Util.DivisionofTwoDouble(pixelWidth, imageSource.Width, 20);
        //    var scaleY = Foundation.Math.IEEE754Util.DivisionofTwoDouble(pixelHeight, imageSource.Height, 20);
        //    var targetBitmap = new TransformedBitmap(imageSource as BitmapSource, new ScaleTransform(scaleX, scaleY));
        //    targetBitmap.Freeze();

        //    return targetBitmap;
        //}

        /// <summary>
        /// 이미지를 회전시켜 저장한다.
        /// </summary>
        /// <param name="imageSource"></param>
        /// <param name="angle"></param>
        /// <param name="center"></param>
        /// <returns></returns>
        public static ImageSource RotateImage(ImageSource imageSource, double angle, System.Windows.Point center)
        {
            var bitmapSource = imageSource as BitmapSource;

            DrawingVisual visual = new DrawingVisual();
            visual.Opacity = 0;

            using (DrawingContext dc = visual.RenderOpen())
            {
                dc.PushTransform(new RotateTransform(angle, center.X, center.Y));
                dc.DrawImage(bitmapSource, new Rect(0, 0, bitmapSource.PixelWidth, bitmapSource.PixelHeight));
            }

            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(bitmapSource.PixelWidth, bitmapSource.PixelHeight, bitmapSource.DpiX, bitmapSource.DpiY, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(visual);
            renderTargetBitmap.Freeze();

            return renderTargetBitmap;
        }
    }
}
