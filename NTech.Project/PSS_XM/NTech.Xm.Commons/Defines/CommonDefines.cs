using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Net.NetworkInformation;
using System.Windows.Input;

namespace NTech.Xm.Commons.Defines
{
    public enum LOGIN_STATE
    {
        LOGOUT,
        LOGIN_SUCCESS,
        LOGIN_FAILED
    }
    public enum ROLE
    {
        NO_PERMISSION,
        OPERATOR,
        ADMIN,
        SUPERADMIN
    }
    public static class CommonDefines
    {
        public static readonly string IP_PC_GATE = "192.168.0.1";
        public static readonly string IP_PC_LINE2 = "192.168.0.2";
        public static readonly string IP_PC_LINE3 = "192.168.0.3";

        public static readonly SolidColorBrush SolidColorOK = (SolidColorBrush)new BrushConverter().ConvertFromString("#2E7D32");
        public static readonly SolidColorBrush SolidColorFail = (SolidColorBrush)new BrushConverter().ConvertFromString("#DD2C00");
        public static string GetEnumDescription(Enum enumObj)
        {
            FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());
            if (fieldInfo != null)
            {
                object[] attribArray = fieldInfo.GetCustomAttributes(false);
                if (attribArray != null && attribArray.Length > 0 && attribArray[0] is DescriptionAttribute attrib)
                {
                    return attrib.Description;
                }
            }
            return enumObj.ToString();
        }

        public static string[] GetAllLocalIPv4(NetworkInterfaceType _type)
        {
            List<string> ipAddrList = new List<string>();
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipAddrList.Add(ip.Address.ToString());
                        }
                    }
                }
            }
            return ipAddrList.ToArray();
        }
        public static string GetLocalIPv4Addresses()
        {
            try
            {
                string returnAddress = String.Empty;

                // Get a list of all network interfaces (usually one per network card, dialup, and VPN connection)
                NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

                foreach (NetworkInterface network in networkInterfaces)
                {
                    // Read the IP configuration for each network
                    IPInterfaceProperties properties = network.GetIPProperties();

                    if (network.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                           network.OperationalStatus == OperationalStatus.Up &&
                           !network.Description.ToLower().Contains("virtual") &&
                           !network.Description.ToLower().Contains("pseudo"))
                    {
                        // Each network interface may have multiple IP addresses
                        foreach (IPAddressInformation address in properties.UnicastAddresses)
                        {
                            // We're only interested in IPv4 addresses for now
                            if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                                continue;

                            // Ignore loopback addresses (e.g., 127.0.0.1)
                            if (IPAddress.IsLoopback(address.Address))
                                continue;

                            returnAddress = address.Address.ToString();
                        }
                    }
                }

                return returnAddress;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

    }
    public static class Secure
    {
        private static readonly DependencyProperty PasswordInitializedProperty =
            DependencyProperty.RegisterAttached("PasswordInitialized", typeof(bool), typeof(Secure), new PropertyMetadata(false));

        private static readonly DependencyProperty SettingPasswordProperty =
            DependencyProperty.RegisterAttached("SettingPassword", typeof(bool), typeof(Secure), new PropertyMetadata(false));

        public static string GetPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(PasswordProperty);
        }
        public static void SetPassword(DependencyObject obj, string value)
        {
            obj.SetValue(PasswordProperty, value);
        }
        // We play a trick here. If we set the initial value to something, it'll be set to something else when the binding kicks in,
        // and HandleBoundPasswordChanged will be called, which allows us to set up our event subscription.
        // If the binding sets us to a value which we already are, then this doesn't happen. Therefore start with a value that's
        // definitely unique.
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password", typeof(string), typeof(Secure),
                new FrameworkPropertyMetadata(Guid.NewGuid().ToString(), HandleBoundPasswordChanged)
                {
                    BindsTwoWayByDefault = true,
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.LostFocus // Match the default on Binding
                });

        private static void HandleBoundPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = dp as PasswordBox;
            if (passwordBox == null)
                return;

            // If we're being called because we set the value of the property we're bound to (from inside 
            // HandlePasswordChanged, then do nothing - we already have the latest value).
            if ((bool)passwordBox.GetValue(SettingPasswordProperty))
                return;

            // If this is the initial set (see the comment on PasswordProperty), set ourselves up
            if (!(bool)passwordBox.GetValue(PasswordInitializedProperty))
            {
                passwordBox.SetValue(PasswordInitializedProperty, true);
                passwordBox.PasswordChanged += HandlePasswordChanged;
            }

            passwordBox.Password = e.NewValue as string;
        }

        private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;
            passwordBox.SetValue(SettingPasswordProperty, true);
            SetPassword(passwordBox, passwordBox.Password);
            passwordBox.SetValue(SettingPasswordProperty, false);
        }
    }
    public static class DataGridTextSearch
    {
        // Using a DependencyProperty as the backing store for SearchValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchValueProperty =
            DependencyProperty.RegisterAttached("SearchValue", typeof(string), typeof(DataGridTextSearch),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.Inherits));

        public static string GetSearchValue(DependencyObject obj)
        {
            return (string)obj.GetValue(SearchValueProperty);
        }

        public static void SetSearchValue(DependencyObject obj, string value)
        {
            obj.SetValue(SearchValueProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsTextMatch.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsTextMatchProperty =
            DependencyProperty.RegisterAttached("IsTextMatch", typeof(bool), typeof(DataGridTextSearch), new UIPropertyMetadata(false));

        public static bool GetIsTextMatch(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsTextMatchProperty);
        }

        public static void SetIsTextMatch(DependencyObject obj, bool value)
        {
            obj.SetValue(IsTextMatchProperty, value);
        }
    }
    public static class FocusAdvancement
    {
        public static bool GetAdvancesByEnterKey(DependencyObject obj)
        {
            return (bool)obj.GetValue(AdvancesByEnterKeyProperty);
        }

        public static void SetAdvancesByEnterKey(DependencyObject obj, bool value)
        {
            obj.SetValue(AdvancesByEnterKeyProperty, value);
        }

        public static readonly DependencyProperty AdvancesByEnterKeyProperty =
            DependencyProperty.RegisterAttached("AdvancesByEnterKey", typeof(bool), typeof(FocusAdvancement),
            new UIPropertyMetadata(OnAdvancesByEnterKeyPropertyChanged));

        static void OnAdvancesByEnterKeyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as UIElement;
            if (element == null) return;

            if ((bool)e.NewValue) element.KeyDown += Keydown;
            else element.KeyDown -= Keydown;
        }

        static void Keydown(object sender, KeyEventArgs e)
        {
            if (!e.Key.Equals(Key.Enter)) return;

            var element = sender as UIElement;
            //if (element == null || !(element is TextBox) || !(element is ComboBox)) return;
            if (element != null) element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
    }
}
