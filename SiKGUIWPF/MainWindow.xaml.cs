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

using SiKLink;

namespace SiKGUIWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private SiKInterface _sikInterface = new SiKInterface();
        private string _statusMessage;

        public event PropertyChangedEventHandler PropertyChanged;

        public SiKConfig SiKConfig
        {
            get; private set;
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
            foreach (var item in SiKInterface.GetSerialPorts())
                SerialPort.Items.Add(item);

            foreach (var item in SiKLink.Constants.SerialRates)
                SerialSpeed.Items.Add(item);

            SerialSpeed.SelectedItem = 57600;

            foreach (var item in SiKLink.Constants.SiKSerialRates)
                SiKSerialSpeed.Items.Add(item);

            SiKConfig = _sikInterface.SiKConfig;
        }

        private void Button_ConnectClick(object sender, RoutedEventArgs e)
        {
            if (SerialPort.SelectedItem == null || SerialSpeed.SelectedItem == null)
                return;

            if (!_sikInterface.Connect((string)SerialPort.SelectedItem, (int)SerialSpeed.SelectedItem))
            {
                StatusMessage = "Failed to open port.";
                return;
            }

            if (!_sikInterface.EnterCommandMode())
            {
                StatusMessage = "Failes to enter command mode.";
                return;
            }

            StatusMessage = "Connected!";

            _sikInterface.ReadIdentificationData();
        }

        private void Button_DisconnectClick(object sender, RoutedEventArgs e)
        {
            _sikInterface.Disconnect();
        }

        private void Button_ReadValuesClick(object sender, RoutedEventArgs e)
        {
            if (!_sikInterface.ReadEEPROMData())
            {
                StatusMessage = "Failure reading EEPROM parameters";
            }

            StatusMessage = "EEPROM parameters read";
        }
    }
}
