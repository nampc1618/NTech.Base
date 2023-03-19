using Jai_FactoryDotNET;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NTech.Wpf.TestForm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CFactory cFac = new CFactory();
        CCamera cCamera;

        // GenICam nodes
        CNode myWidthNode;
        CNode myHeightNode;
        CNode myGainNode;

        public int iOldSelectedIndex; // Selected camera entry in the ListBox
        ColorPalette myMonoColorPalette = null;
        public MainWindow()
        {
            InitializeComponent();

            scrollViewerEx.ImgEx = imageEx;
            imageEx.Parent = scrollViewerEx;
            scrollViewerEx.GridChild = grid;

            SearchCamera();
        }

        private void btnStartLive_Click(object sender, RoutedEventArgs e)
        {
            //imageEx.Visibility = Visibility.Collapsed;
            //picShow.Visible = true;
            if (cCamera != null)
            {
                //HwndSource hwndSource = PresentationSource.FromVisual(imageEx) as HwndSource;
                //IntPtr handle = new IntPtr();
                //if (hwndSource != null)
                //{
                //    handle = hwndSource.Handle;
                //}

                cCamera.StretchLiveVideo = true;
                // Start the image acquisition with the picturebox windows handle. If the handle is IntPtr.Zero then a new window will be created
                cCamera.StartImageAcquisition(false, 5);

                //Jai_FactoryWrapper.RECT newRectSize;
                //if (cCamera.StretchLiveVideo)
                //    newRectSize = new Jai_FactoryWrapper.RECT(0, 0, (int)imageEx.Width, (int)imageEx.Height);
                //else
                //    newRectSize = new Jai_FactoryWrapper.RECT(0, 0, Convert.ToInt32(cCamera.GetNode("Width").Max), Convert.ToInt32(cCamera.GetNode("Height").Max));

                //Jai_FactoryWrapper.J_Image_ResizeChildWindow(cCamera.WindowHandle, ref newRectSize);
            }
        }

        private void btnSwTrigger_Click(object sender, RoutedEventArgs e)
        {

            // But we have 2 ways of sending a software trigger: JAI and GenICam SNC
            // The GenICam SFNC software trigger is available if a node called
            // TriggerSoftware is available
            if (cCamera.GetNode("TriggerSoftware") != null)
            {
                // Here we assume that this is the GenICam SFNC way of setting up the trigger
                // To switch to Continuous the following is required:
                // TriggerSelector=FrameStart
                // Execute TriggerSoftware[TriggerSelector] command
                cCamera.GetNode("TriggerSelector").Value = "FrameStart";
                cCamera.GetNode("TriggerSoftware").ExecuteCommand();
            }
            else
            {
                // We need to "pulse" the Software Trigger feature in order to trigger the camera!
                cCamera.GetNode("SoftwareTrigger0").Value = 0;
                cCamera.GetNode("SoftwareTrigger0").Value = 1;
                cCamera.GetNode("SoftwareTrigger0").Value = 0;
            }
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                imageEx.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }
        void SearchCamera()
        {
            Jai_FactoryWrapper.EFactoryError error = Jai_FactoryWrapper.EFactoryError.Success;

            // Open the Factory
            error = cFac.Open("");

            stbItem.Content = "Searching for cameras...";

            if (error == Jai_FactoryWrapper.EFactoryError.Success)
            {
                cbbDevice.Items.Clear(); // delete camera list from combo box

                // Search for any new GigE cameras
                cFac.UpdateCameraList(CFactory.EDriverType.Undefined);

                if (cFac.CameraList.Count > 0)
                {
                    for (int i = 0; i < cFac.CameraList.Count; i++)
                    {
                        string sList = cFac.CameraList[i].ModelName;

                        if (cFac.CameraList[i].CameraID.Contains("INT=>FD"))
                            sList += " (Filter Driver)";
                        else if (cFac.CameraList[i].CameraID.Contains("INT=>SD"))
                            sList += " (Socket Driver)";

                        cbbDevice.Items.Add(sList);
                    }
                }
                else
                {
                    MessageBox.Show("No camera found");
                }

                stbItem.Content = "Found " + cFac.CameraList.Count.ToString() + " camera(s). Select camera from the ListBox to open it.";
            }
            else
            {
                showErrorMsg(error);
                //error handling?
            }
        }
        private void showErrorMsg(Jai_FactoryWrapper.EFactoryError error)
        {
            String sErrorMsg = "Error = " + error.ToString();

            MessageBox.Show(sErrorMsg);
        }
        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (null != cCamera)
            {
                if (cCamera.IsOpen)
                {
                    cCamera.Close();
                }

                cCamera = null;
            }

            // Discover GigE and/or generic GenTL devices using myFactory.UpdateCameraList(in this case specifying Filter Driver for GigE cameras).
            cFac.UpdateCameraList(Jai_FactoryDotNET.CFactory.EDriverType.FilterDriver);

            // Open the camera - first check for GigE devices
            for (int i = 0; i < cFac.CameraList.Count; i++)
            {
                cCamera = cFac.CameraList[i];
                if (Jai_FactoryWrapper.EFactoryError.Success == cCamera.Open())
                {
                    break;
                }
            }

            if (null != cCamera && cCamera.IsOpen)
            {

                int currentValue = 0;

                // Get the Width GenICam Node
                myWidthNode = cCamera.GetNode("Width");
                if (myWidthNode != null)
                {
                    currentValue = int.Parse(myWidthNode.Value.ToString());
                }
                else

                    SetFramegrabberValue("Width", (Int64)myWidthNode.Value);

                // Get the Height GenICam Node
                myHeightNode = cCamera.GetNode("Height");
                if (myHeightNode != null)
                {
                    currentValue = int.Parse(myHeightNode.Value.ToString());


                }
                SetFramegrabberValue("Height", (Int64)myHeightNode.Value);

                // Get the GainRaw GenICam Node
                myGainNode = cCamera.GetNode("GainRaw");
                if (myGainNode != null)
                {
                    currentValue = int.Parse(myGainNode.Value.ToString());
                }

                SetFramegrabberPixelFormat();
            }
            else
            {
                MessageBox.Show("No Cameras Found!");
            }
        }
        private void SetFramegrabberValue(String nodeName, Int64 int64Val)
        {
            if (null == cCamera)
            {
                return;
            }

            IntPtr hDevice = IntPtr.Zero;
            Jai_FactoryWrapper.EFactoryError error = Jai_FactoryWrapper.J_Camera_GetLocalDeviceHandle(cCamera.CameraHandle, ref hDevice);
            if (Jai_FactoryWrapper.EFactoryError.Success != error)
            {
                return;
            }

            if (IntPtr.Zero == hDevice)
            {
                return;
            }

            IntPtr hNode;
            error = Jai_FactoryWrapper.J_Camera_GetNodeByName(hDevice, nodeName, out hNode);
            if (Jai_FactoryWrapper.EFactoryError.Success != error)
            {
                return;
            }

            if (IntPtr.Zero == hNode)
            {
                return;
            }

            error = Jai_FactoryWrapper.J_Node_SetValueInt64(hNode, false, int64Val);
            if (Jai_FactoryWrapper.EFactoryError.Success != error)
            {
                return;
            }

            //Special handling for Active Silicon CXP boards, which also has nodes prefixed
            //with "Incoming":
            if ("Width" == nodeName || "Height" == nodeName)
            {
                string strIncoming = "Incoming" + nodeName;
                IntPtr hNodeIncoming;
                error = Jai_FactoryWrapper.J_Camera_GetNodeByName(hDevice, strIncoming, out hNodeIncoming);
                if (Jai_FactoryWrapper.EFactoryError.Success != error)
                {
                    return;
                }

                if (IntPtr.Zero == hNodeIncoming)
                {
                    return;
                }

                error = Jai_FactoryWrapper.J_Node_SetValueInt64(hNodeIncoming, false, int64Val);
            }
        }
        private void SetFramegrabberPixelFormat()
        {
            String nodeName = "PixelFormat";

            if (null == cCamera)
            {
                return;
            }

            IntPtr hDevice = IntPtr.Zero;
            Jai_FactoryWrapper.EFactoryError error = Jai_FactoryWrapper.J_Camera_GetLocalDeviceHandle(cCamera.CameraHandle, ref hDevice);
            if (Jai_FactoryWrapper.EFactoryError.Success != error)
            {
                return;
            }

            if (IntPtr.Zero == hDevice)
            {
                return;
            }

            long pf = 0;
            error = Jai_FactoryWrapper.J_Camera_GetValueInt64(cCamera.CameraHandle, nodeName, ref pf);
            if (Jai_FactoryWrapper.EFactoryError.Success != error)
            {
                return;
            }
            UInt64 pixelFormat = (UInt64)pf;

            UInt64 jaiPixelFormat = 0;
            error = Jai_FactoryWrapper.J_Image_Get_PixelFormat(cCamera.CameraHandle, pixelFormat, ref jaiPixelFormat);
            if (Jai_FactoryWrapper.EFactoryError.Success != error)
            {
                return;
            }

            StringBuilder sbJaiPixelFormatName = new StringBuilder(512);
            uint iSize = (uint)sbJaiPixelFormatName.Capacity;
            error = Jai_FactoryWrapper.J_Image_Get_PixelFormatName(cCamera.CameraHandle, jaiPixelFormat, sbJaiPixelFormatName, iSize);
            if (Jai_FactoryWrapper.EFactoryError.Success != error)
            {
                return;
            }

            IntPtr hNode;
            error = Jai_FactoryWrapper.J_Camera_GetNodeByName(hDevice, nodeName, out hNode);
            if (Jai_FactoryWrapper.EFactoryError.Success != error)
            {
                return;
            }

            if (IntPtr.Zero == hNode)
            {
                return;
            }

            error = Jai_FactoryWrapper.J_Node_SetValueString(hNode, false, sbJaiPixelFormatName.ToString());
            if (Jai_FactoryWrapper.EFactoryError.Success != error)
            {
                return;
            }

            //Special handling for Active Silicon CXP boards, which also has nodes prefixed
            //with "Incoming":
            string strIncoming = "Incoming" + nodeName;
            IntPtr hNodeIncoming;
            error = Jai_FactoryWrapper.J_Camera_GetNodeByName(hDevice, strIncoming, out hNodeIncoming);
            if (Jai_FactoryWrapper.EFactoryError.Success != error)
            {
                return;
            }

            if (IntPtr.Zero == hNodeIncoming)
            {
                return;
            }

            error = Jai_FactoryWrapper.J_Node_SetValueString(hNodeIncoming, false, sbJaiPixelFormatName.ToString());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void cbbDevice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Jai_FactoryWrapper.EFactoryError error = Jai_FactoryWrapper.EFactoryError.Success;

            // do nothing if the selected camera is the same as the previous one
            if (cCamera != null && iOldSelectedIndex != cbbDevice.SelectedIndex)
            {
                // Close any other opened camera so we can open a new one...
                cCamera.Close();
                cCamera = null;
            }

            if (cCamera == null)
            {
                // open the selected camera from the list
                cCamera = cFac.CameraList[cbbDevice.SelectedIndex];

                stbItem.Content = cCamera.CameraID;
                error = cCamera.Open();
                if (error != Jai_FactoryWrapper.EFactoryError.Success)
                {
                    showErrorMsg(error);
                    return;
                }
                else
                {
                    stbItem.Content = stbItem.Content.ToString();

                    // save the selected camera index
                    iOldSelectedIndex = cbbDevice.SelectedIndex;

                    // initialze controls
                    CNode myNode;
                    // width
                    myNode = cCamera.GetNode("Width");

                    SetFramegrabberValue("Width", (Int64)myNode.Value);

                    // height
                    myNode = cCamera.GetNode("Height");

                    SetFramegrabberValue("Height", (Int64)myNode.Value);

                    SetFramegrabberPixelFormat();

                    // gain
                    myNode = cCamera.GetNode("GainRaw");

                    // .. and remember to set the trigger accordingly

                    // But we have 2 ways of setting up triggers: JAI and GenICam SNC
                    // The GenICam SFNC trigger setup is available if a node called
                    // TriggerSelector is available
                    if (cCamera.GetNode("TriggerSelector") != null)
                    {
                        // Here we assume that this is the GenICam SFNC way of setting up the trigger
                        // To switch to Continuous the following is required:
                        // TriggerSelector=FrameStart
                        // TriggerMode[TriggerSelector]=Off
                        cCamera.GetNode("TriggerSelector").Value = "FrameStart";
                        cCamera.GetNode("TriggerMode").Value = "Off";

                        // Does this camera have a "Software Trigger" feature available?
                        myNode = cCamera.GetNode("TriggerSoftware");

                    }
                    else
                    {
                        // Here we assume that this is the JAI of setting up the trigger
                        // To switch to Continuous the following is required:
                        // ExposureMode=Continuous
                        cCamera.GetNode("ExposureMode").Value = "Continuous";

                        // Does this camera have a "Software Trigger" feature available?
                        myNode = cCamera.GetNode("SoftwareTrigger0");

                    }

                    radContinous.IsChecked = true;

                }
            }
        }

        void HandleImage(ref Jai_FactoryWrapper.ImageInfo ImageInfo)
        {
            // Jai_FactoryWrapper.EFactoryError error = Jai_FactoryWrapper.EFactoryError.Success;

            // This is in fact a callback, so we would need to handle the data as fast as possible and the frame buffer 
            // we get as a parameter will be recycled when we terminate.
            // This leaves us with two choises:
            // 1) Get the work we need to do done ASAP and return
            // 2) Make a copy of the image data and process this afterwards
            //
            // We have the access to the buffer directly via the ImageInfo.ImageBuffer variable
            // 
            // We can access the raw frame buffer bytes if we use "unsafe" code and pointer
            // To do this we need to set the "Allow unsafe code" in the project properties and then access the data like:
            //
            // unsafe
            // {
            //     // Cast IntPtr to pointer to byte
            //     byte* pArray = (byte*)ImageInfo.ImageBuffer;
            //     // Do something with the data
            //     // Read values
            //     byte value = pArray[10];
            //     // Write values
            //     for (int i = 0; i < 1000; i++)
            //         pArray[i] = (byte)(i % 255);
            // }
            //
            // // If we want to copy the data instead we can do like this without Unsafe code:
            // byte[] array = null;
            //
            // if (ImageInfo.ImageBuffer != IntPtr.Zero)
            // {
            //     // Allocate byte array that can contain the copy of data
            //     array = new byte[ImageInfo.ImageSize];
            //     // Do the copying
            //     Marshal.Copy(ImageInfo.ImageBuffer, array, 0, (int)ImageInfo.ImageSize);
            //
            //     // Do something with the raw data
            //     byte val = array[10];
            //}

            unsafe
            {
                // Cast IntPtr to pointer to byte
                //byte* pArray = (byte*)ImageInfo.ImageBuffer;
                // Do something with the data
                Bitmap bmpSrc = new Bitmap((int)ImageInfo.SizeX, (int)ImageInfo.SizeY, (int)ImageInfo.SizeX, System.Drawing.Imaging.PixelFormat.Format8bppIndexed, ImageInfo.ImageBuffer);
                // Create a Monochrome palette (only once)
                if (myMonoColorPalette == null)
                {
                    Bitmap monoBitmap = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
                    myMonoColorPalette = monoBitmap.Palette;

                    for (int i = 0; i < 256; i++)
                        myMonoColorPalette.Entries[i] = System.Drawing.Color.FromArgb(i, i, i);
                }
                // Set the Monochrome Color Palette
                bmpSrc.Palette = myMonoColorPalette;

                imageEx.Dispatcher.BeginInvoke(new Action(() =>
                {
                    imageEx.Source = ConvertBmpToBmpSrc(bmpSrc);
                }));
            }
            return;
        }
        private void btnStopLive_Click(object sender, RoutedEventArgs e)
        {
            if (cCamera != null)
            {
                cCamera.StopImageAcquisition();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (cCamera != null)
            {
                cCamera.StopImageAcquisition();

                if (null != cCamera)
                {
                    CNode node = cCamera.GetNode("TriggerMode");
                    if (null != node)
                    {
                        node.Value = "Off";
                    }
                }
                cFac.Close();
                cCamera.Close();
            }
        }

        private void radSwTrig_Click(object sender, RoutedEventArgs e)
        {
            cCamera.NewImageDelegate += new Jai_FactoryWrapper.ImageCallBack(HandleImage);
            imageEx.Visibility = Visibility.Visible;
            //picShow.Visible = false;

            // Prepare for software trigger:

            // But we have 2 ways of setting up triggers: JAI and GenICam SNC
            // The GenICam SFNC trigger setup is available if a node called
            // TriggerSelector is available
            if (cCamera.GetNode("TriggerSelector") != null)
            {
                // Here we assume that this is the GenICam SFNC way of setting up the trigger
                // To switch to Continuous the following is required:
                // TriggerSelector=FrameStart
                // TriggerMode[TriggerSelector]=On
                // TriggerSource[TriggerSelector]=Software
                cCamera.GetNode("TriggerSelector").Value = "FrameStart";
                cCamera.GetNode("TriggerMode").Value = "On";
                cCamera.GetNode("TriggerSource").Value = "Software";
            }
            else
            {
                // Select triggered mode (not continuous mode)

                // Here we assume that this is the JAI of setting up the trigger
                // To switch to Continuous the following is required:
                // ExposureMode=EdgePreSelect
                // LineSelector=CameraTrigger0
                // LineSource=SoftwareTrigger0
                // LineInverter=ActiveHigh
                cCamera.GetNode("ExposureMode").Value = "EdgePreSelect";

                // Set Line Selector to "Camera Trigger 0"
                cCamera.GetNode("LineSelector").Value = "CameraTrigger0";

                // Set Line Source to "Software Trigger 0"
                cCamera.GetNode("LineSource").Value = "SoftwareTrigger0";

                // .. and finally set the Line Polarity (LineInverter) to "Active High"
                cCamera.GetNode("LineInverter").Value = "ActiveHigh";
            }
        }

        private void radContinous_Click(object sender, RoutedEventArgs e)
        {
            // But we have 2 ways of setting up triggers: JAI and GenICam SNC
            // The GenICam SFNC trigger setup is available if a node called
            // TriggerSelector is available
            if (cCamera.GetNode("TriggerSelector") != null)
            {
                // Here we assume that this is the GenICam SFNC way of setting up the trigger
                // To switch to Continuous the following is required:
                // TriggerSelector=FrameStart
                // TriggerMode[TriggerSelector]=Off
                cCamera.GetNode("TriggerSelector").Value = "FrameStart";
                cCamera.GetNode("TriggerMode").Value = "Off";
            }
            else
            {
                // Here we assume that this is the JAI of setting up the trigger
                // To switch to Continuous the following is required:
                // ExposureMode=Continuous
                cCamera.GetNode("ExposureMode").Value = "Continuous";
            }
        }
        private BitmapImage BitmapToImageSource(System.Drawing.Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }
        public static BitmapSource ConvertBmpToBmpSrc(System.Drawing.Bitmap bitmap)
        {
            var bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height,
                bitmap.HorizontalResolution, bitmap.VerticalResolution,
                PixelFormats.Gray8, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);

            return bitmapSource;
        }
    }
}
