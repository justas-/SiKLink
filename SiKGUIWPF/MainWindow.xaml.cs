/*
SiK Link - GUI and control library for SiK radios.
Copyright(C) 2020  J. Poderys

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with this program.If not, see<http://www.gnu.org/licenses/>.
*/
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;

using SiKLink;

namespace SiKGUIWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private static readonly int SMALLWNDHEIGHT = 280;
        private static readonly int FULLWNDHEIGHT = 580;

        private SiKInterface _sikInterface = new SiKInterface();
        private string _statusMessage;
        private bool _rssiStreaming = false;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Find a parent based on a parent's Type.
        /// </summary>
        /// <typeparam name="T">Type of the parent</typeparam>
        /// <param name="child">Child element</param>
        /// <remarks>https://www.infragistics.com/community/blogs/b/blagunas/posts/find-the-parent-control-of-a-specific-type-in-wpf-and-silverlight</remarks>
        /// <returns>Parent or null</returns>
        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }

        public string StatusMessage
        {
            get
            {
                return _statusMessage;
            }
            set
            {
                if (value != _statusMessage)
                {
                    _statusMessage = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StatusMessage"));
                }
            }
        }
        public MainWindow()
        {
            InitializeComponent();

            Height = SMALLWNDHEIGHT;
            RssiFig.Visibility = Visibility.Collapsed;

            SerialPort.DropDownOpened += SerialPort_DropDownOpened;

            foreach (var item in SiKInterface.GetSerialPorts())
                SerialPort.Items.Add(item);

            foreach (var item in Helpers.SerialRates)
                SerialSpeed.Items.Add(item);

            SerialSpeed.SelectedItem = 57600;

            foreach (var item in SiKLink.Constants.SiKSerialRates)
                SiKSerialSpeed.Items.Add(item);

            foreach (var item in Helpers.MavVersions)
                MavlinkFrame.Items.Add(item);

            foreach (var item in SiKLink.Constants.AirPower)
                AirPower.Items.Add(item);

            foreach (var item in Enumerable.Range(1, 100))
                DutyCycle.Items.Add(item);

            foreach (var item in Enumerable.Range(0, 255))
                LbtRssi.Items.Add(item);

            foreach (var item in Enumerable.Range(33, 99))
                MaxWnd.Items.Add(item);

            DataContext = _sikInterface.SiKConfig;
        }
        /// <summary>
        /// Populate Drop Down with Serial Port names.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialPort_DropDownOpened(object sender, EventArgs e)
        {
            SerialPort.Items.Clear();

            foreach (var item in SiKInterface.GetSerialPorts())
                SerialPort.Items.Add(item);
        }

        /// <summary>
        /// Close connection nicely when closing.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            _sikInterface.Disconnect();
            base.OnClosing(e);
        }

        private void Button_ConnectClick(object sender, RoutedEventArgs e)
        {
            if (SerialPort.SelectedItem == null)
            {
                StatusMessage = "Select COM port!";
                return;
            }

            if (SerialSpeed.SelectedItem == null)
            {
                StatusMessage = "Select COM baudrate";
                return;
            }

            if (!_sikInterface.Connect((string)SerialPort.SelectedItem, (int)SerialSpeed.SelectedItem))
            {
                StatusMessage = "Failed to open the COM port.";
                return;
            }

            // We are connected now. Update tab
            var parent = FindParent<TabControl>(sender as Button);
            if (parent != null)
            {
                var tab = parent.Items[0] as TabItem;
                if (tab != null)
                {
                    tab.Header = $"{SerialPort.SelectedItem}({SerialSpeed.SelectedItem})";
                }
            }

            // Are we in the command mode already?
            var in_command = _sikInterface.CheckCommandMode();
            if (!in_command)
            {
                if (!_sikInterface.EnterCommandMode())
                {
                    StatusMessage = "Failed to enter the command mode.";
                    return;
                }
            }

            StatusMessage = "Connected!";
            _sikInterface.ReadIdentificationData();
        }

        private void Button_DisconnectClick(object sender, RoutedEventArgs e)
        {
            _sikInterface.Disconnect();

            // We are connected now. Update tab
            var parent = FindParent<TabControl>(sender as Button);
            if (parent != null)
            {
                var tab = parent.Items[0] as TabItem;
                if (tab != null)
                {
                    tab.Header = "Disconnected";
                }
            }

            StatusMessage = "Disconnected.";
        }

        private void Button_ReadValuesClick(object sender, RoutedEventArgs e)
        {
            if (!_sikInterface.PortConnected)
            {
                StatusMessage = "Not connected to the radio!";
                return;
            }

            if (!_sikInterface.ReadEEPROMData())
            {
                StatusMessage = "Failure reading EEPROM parameters";
            }

            StatusMessage = "EEPROM parameters read";
        }

        private void Button_WriteValuesClick(object sender, RoutedEventArgs e)
        {
            if (!_sikInterface.PortConnected)
            {
                StatusMessage = "Not connected to the radio!";
                return;
            }

            if (!_sikInterface.SaveParameters())
            {
                StatusMessage = "Failed to save the parameters";
            }
            else
            {
                StatusMessage = "Parameters saved.";
            }
        }
        private void Button_SaveEepromClick(object sender, RoutedEventArgs e)
        {
            if (!_sikInterface.PortConnected)
            {
                StatusMessage = "Not connected to the radio!";
                return;
            }

            if (_sikInterface.SaveToEEPROM())
            {
                StatusMessage = "Configuration saved to EEPROM.";
            }
            else
            {
                StatusMessage = "Failure while writing EEPROM.";
            }
        }
        private void Button_RestartRadioClick(object sender, RoutedEventArgs e)
        {
            if (!_sikInterface.PortConnected)
            {
                StatusMessage = "Not connected to the radio!";
                return;
            }

            _sikInterface.RebootRadio();
            StatusMessage = "Radio reboot requested.";
        }
        private void Button_SaveClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON file (*.json)|*.json";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (saveFileDialog.ShowDialog() == true)
            {
                if (_sikInterface.SaveParamsToFile(saveFileDialog.FileName))
                {
                    StatusMessage = "Parameters saved to a backup file.";
                }
                else
                {
                    StatusMessage = "Failed to save the parameters file.";
                }
            }
        }
        private void Button_LoadClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON file (*.json)|*.json";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                if (_sikInterface.LoadParamsFromFile(openFileDialog.FileName))
                {
                    DataContext = _sikInterface.SiKConfig;
                    StatusMessage = "Parameters loaded from a config file.";
                }
                else
                {
                    StatusMessage = "Failed to load the parameters.";
                }
            }
        }
        private void Button_RssiClick(object sender, RoutedEventArgs e)
        {
            if (!_sikInterface.PortConnected)
            {
                StatusMessage = "Not connected to the radio!";
                return;
            }

            _sikInterface.ToggleRssiDebug();
            if (!_rssiStreaming)
            {
                EnableControlButtons(false);
                BtnShowFigures.Background = Brushes.LightGreen;
                _sikInterface.OnRssiData += _sikInterface_OnRssiData;
                _rssiStreaming = true;
                Height = FULLWNDHEIGHT;
                RssiFig.Visibility = Visibility.Visible;
            }
            else
            {
                EnableControlButtons(true);
                BtnShowFigures.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDDDDDD"));
                _sikInterface.OnRssiData -= _sikInterface_OnRssiData;
                _rssiStreaming = false;
                Height = SMALLWNDHEIGHT;
                RssiFig.Visibility = Visibility.Collapsed;
                RssiFig.ClearValues();
            }
        }
        /// <summary>
        /// Enable or disable function buttons.
        /// </summary>
        /// <param name="enable">true to enable</param>
        private void EnableControlButtons(bool enable)
        {
            if (enable)
            {
                BtnReadValues.IsEnabled = true;
                BtnWriteValues.IsEnabled = true;
                BtnSaveEeprom.IsEnabled = true;
            }
            else
            {
                BtnReadValues.IsEnabled = false;
                BtnWriteValues.IsEnabled = false;
                BtnSaveEeprom.IsEnabled = false;
            }
        }
        private void _sikInterface_OnRssiData(object sender, RssiDataEventArgs rssi)
        {
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    RssiFig.AddValue(new RssiObservation
                    {
                        LocalRssi = rssi.LocalRssi,
                        LocalNoise = rssi.LocalNoise,
                        RemoteRssi = rssi.RemoteRssi,
                        RemoteNoise = rssi.RemoteNoise,
                        Id = RssiObservation.NextId++
                    });
                });
            }
        }
    }
}
