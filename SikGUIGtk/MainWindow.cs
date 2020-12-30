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
using Gtk;
using System;
using System.Diagnostics;

using SiKLink;

namespace SiKGuiGtk
{
    class MainWindow : Window
    {
        private static readonly string NOT_CONN_LBL = "Not Connected";

        private HeaderBar _headerBar;
        private Notebook _notebook;
        private Box _pageContainer;

        protected BoardIdentifierControls _boardIdentifiers;
        protected DataTableControls _dataTableControls;

        private Label _pageLabel;

        private ComboBoxText _portNameCombo;
        private ComboBoxText _baudRateCombo;

        private Statusbar _statusBar;
        private uint _cxt;

        protected SiKInterface _sikInterface;

        // Impersonate WPF control to enable code sharing
        public string StatusMessage
        {
            set
            {
                _statusBar.RemoveAll(_cxt);
                _statusBar.Push(_cxt, value);
            }
        }

        public MainWindow() : base(WindowType.Toplevel)
        {
            // Setup GUI
            WindowPosition = WindowPosition.Center;
            DefaultSize = new Gdk.Size(600, 300);

            _sikInterface = new SiKInterface();

            _headerBar = new HeaderBar();
            _headerBar.ShowCloseButton = true;
            _headerBar.Title = "SiK Radio Configurator";

            _boardIdentifiers = new BoardIdentifierControls();
            _dataTableControls = new DataTableControls();
            _sikInterface.SiKConfig.PropertyChanged += _boardIdentifiers.SiKConfig_PropertyChanged;
            _dataTableControls.CreateBindings(_sikInterface.SiKConfig);

            _pageLabel = new Label(NOT_CONN_LBL);

            Titlebar = _headerBar;
            Destroyed += (sender, e) => Application.Quit();

            _notebook = new Notebook();
            _pageContainer = new Box(Orientation.Vertical, 1);
            _notebook.AppendPage(_pageContainer, _pageLabel);
            _notebook.AppendPage(new Box(Orientation.Vertical, 5), new Label("TBD"));

            Add(_notebook);

            CreatePortSelLine();
            CreateDataTable();
            CreateBoardIdLine();
            CreateButtonsLine();

            // TODO: Fix Statusbar at the bottom
            _statusBar = new Statusbar();
            _pageContainer.Add(_statusBar);
            _cxt = _statusBar.GetContextId("Main page");

            ShowAll();
        }
        private void CreateDataTable()
        {
            Grid data_grid = new Grid();
            _pageContainer.Add(data_grid);

            Box cbox;

            // Line 1
            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(new Label("Serial Speed:"));
            cbox.Add(_dataTableControls.SerialSpeedCombo);
            data_grid.Attach(cbox, 0, 0, 1, 1);

            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(new Label("Air Speed:"));
            cbox.Add(_dataTableControls.AirSpeedEntry);
            data_grid.Attach(cbox, 1, 0, 1, 1);

            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(_dataTableControls.EccCheck);
            data_grid.Attach(cbox, 2, 0, 1, 1);

            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(new Label("MavLink:"));
            cbox.Add(_dataTableControls.MavLinkVerCombo);
            data_grid.Attach(cbox, 3, 0, 1, 1);

            // Line 2
            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(new Label("Min Freq:"));
            cbox.Add(_dataTableControls.MinFreqEntry);
            data_grid.Attach(cbox, 0, 1, 1, 1);

            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(new Label("Max Freq:"));
            cbox.Add(_dataTableControls.MaxFreqEntry);
            data_grid.Attach(cbox, 1, 1, 1, 1);

            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(new Label("Num Chan:"));
            cbox.Add(_dataTableControls.NumChanEntry);
            data_grid.Attach(cbox, 2, 1, 1, 1);

            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(new Label("Tx Power:"));
            cbox.Add(_dataTableControls.TxPowerCombo);
            data_grid.Attach(cbox, 3, 1, 1, 1);

            // Line 3
            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(new Label("Net ID:"));
            cbox.Add(_dataTableControls.NetIdEntry);
            data_grid.Attach(cbox, 0, 2, 1, 1);

            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(new Label("Duty Cycle:"));
            cbox.Add(_dataTableControls.DutyCycleCombo);
            data_grid.Attach(cbox, 1, 2, 1, 1);

            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(new Label("LBT RSSI:"));
            cbox.Add(_dataTableControls.LbtRssiCombo);
            data_grid.Attach(cbox, 2, 2, 1, 1);

            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(new Label("Max Wnd:"));
            cbox.Add(_dataTableControls.MaxWndCombo);
            data_grid.Attach(cbox, 3, 2, 1, 1);

            // Line 4
            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(_dataTableControls.RtsCtsCheck);
            data_grid.Attach(cbox, 0, 3, 1, 1);

            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(_dataTableControls.ManchesterCheck);
            data_grid.Attach(cbox, 1, 3, 1, 1);

            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(_dataTableControls.OpportunisticCheck);
            data_grid.Attach(cbox, 2, 3, 1, 1);

            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(new Label("EEPROM Fmt:"));
            cbox.Add(_dataTableControls.EepromFmtEntry);
            data_grid.Attach(cbox, 3, 3, 1, 1);
        }
        private void CreatePortSelLine()
        {
            Box conn_box = new Box(Orientation.Horizontal, 2);
            _pageContainer.Add(conn_box);

            conn_box.Add(new Label("Serial Port:"));

            _portNameCombo = new ComboBoxText();
            _portNameCombo.Changed += Portname_Changed;
            conn_box.Add(_portNameCombo);
            PopulateSerialPortCombo();

            conn_box.Add(new Label("Port baudrate:"));

            _baudRateCombo = new ComboBoxText();
            conn_box.Add(_baudRateCombo);
            foreach (var bd_rate in Helpers.SerialRates)
                _baudRateCombo.PrependText(bd_rate);

            Button conn_btn = new Button();
            conn_btn.Label = "Connect";
            conn_btn.Clicked += BtnConnect_Click;
            conn_box.Add(conn_btn);

            Button disconn_btn = new Button();
            disconn_btn.Label = "Disconnect";
            disconn_btn.Clicked += BtnDisconnect_Click;
            conn_box.Add(disconn_btn);
        }
        private void PopulateSerialPortCombo()
        {
            var ports = SiKInterface.GetSerialPorts();
            ports.Add("<refresh>");

            _portNameCombo.RemoveAll();
            foreach (var pname in ports)
                _portNameCombo.PrependText(pname);
        }
        private void CreateBoardIdLine()
        {
            Box ident_line = new Box(Orientation.Horizontal, 1);
            _pageContainer.Add(ident_line);

            ident_line.Add(new Label("Radio:"));
            ident_line.Add(_boardIdentifiers.RadioIdEntry);

            ident_line.Add(new Label("Radio Ver:"));
            ident_line.Add(_boardIdentifiers.RadioVerEntry);

            ident_line.Add(new Label("Board Id:"));
            ident_line.Add(_boardIdentifiers.BoardIdEntry);

            ident_line.Add(new Label("Board Freq:"));
            ident_line.Add(_boardIdentifiers.BoardFreqEntry);

            ident_line.Add(new Label("BL Ver:"));
            ident_line.Add(_boardIdentifiers.BootloaderVerEntry);
        }
        private void CreateButtonsLine()
        {
            Box buttons_line = new Box(Orientation.Horizontal, 5);
            _pageContainer.Add(buttons_line);

            Button read_btn = new Button();
            read_btn.Label = "Read Values";
            read_btn.Clicked += BtnReadValues_Click;
            buttons_line.Add(read_btn);

            Button write_btn = new Button();
            write_btn.Label = "Write Values";
            write_btn.Clicked += BtnWriteValues_Click;
            buttons_line.Add(write_btn);

            Button save_eeprom_btn = new Button();
            save_eeprom_btn.Label = "Save to EEPROM";
            save_eeprom_btn.Clicked += BtnSaveEeprom_Click;
            buttons_line.Add(save_eeprom_btn);

            Button restart_btn = new Button();
            restart_btn.Label = "Restart Radio";
            restart_btn.Clicked += BtnRestart_Click;
            buttons_line.Add(restart_btn);

            Button save_btn = new Button();
            save_btn.Label = "Save";
            save_btn.Clicked += BtnSave_Click;
            buttons_line.Add(save_btn);

            Button load_btn = new Button();
            load_btn.Label = "Load";
            load_btn.Clicked += BtnLoad_Click;
            buttons_line.Add(load_btn);

            Button rssi_btn = new Button();
            rssi_btn.Label = "RSSI";
            rssi_btn.Clicked += BtnRssi_Click;
            buttons_line.Add(rssi_btn);
        }
        private void Portname_Changed(object sender, EventArgs e)
        {
            var port = _portNameCombo.ActiveText;
            // Handle only <refresh> request
            if (port != "<refresh>")
                return;

            PopulateSerialPortCombo();
        }
        private void BtnConnect_Click(object sender, EventArgs e)
        {
            var port = _portNameCombo.ActiveText;
            if (port == null || port == "<refresh>")
            {
                StatusMessage = "Select Serial port!";
                return;
            }

            var baud_str = _baudRateCombo.ActiveText;
            if (baud_str == null)
            {
                StatusMessage = "Select Baud rate";
                return;
            }

            int baud;
            if (!int.TryParse(baud_str, out baud))
            {
                StatusMessage = "Failure parsing Baud rate";
                return;
            }

            if (!_sikInterface.Connect(port, baud))
            {
                StatusMessage = "Failed to open the COM port.";
                return;
            }

            // Are we in the command mode already?
            if (!_sikInterface.CheckCommandMode())
            {
                if (!_sikInterface.EnterCommandMode())
                {
                    StatusMessage = "Failed to enter the command mode.";
                    return;
                }
            }

            _pageLabel.Text = $"{port}({baud})";
            StatusMessage = "Connected!";
            _sikInterface.ReadIdentificationData();
        }
        private void BtnDisconnect_Click(object sender, EventArgs e)
        {
            _sikInterface.Disconnect();
            _pageLabel.Text = NOT_CONN_LBL;

            StatusMessage = "Disconnected";
        }
        private void BtnReadValues_Click(object sender, EventArgs e)
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
        private void BtnWriteValues_Click(object sender, EventArgs e)
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
        private void BtnSaveEeprom_Click(object sender, EventArgs e)
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
        private void BtnRestart_Click(object sender, EventArgs e)
        {
            if (!_sikInterface.PortConnected)
            {
                StatusMessage = "Not connected to the radio!";
                return;
            }

            _sikInterface.RebootRadio();
            StatusMessage = "Radio reboot requested.";
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            var dialog = new FileChooserDialog(
                "Save configuration",
                this,
                FileChooserAction.Save);
            dialog.AddButton(Stock.Cancel, ResponseType.Cancel);
            dialog.AddButton(Stock.Save, ResponseType.Ok);
            dialog.SetCurrentFolder(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            dialog.SelectMultiple = false;

            var response = (ResponseType)dialog.Run();
            if (response == ResponseType.Ok)
            {
                if (_sikInterface.SaveParamsToFile(dialog.Filename))
                {
                    StatusMessage = "Parameters saved to a backup file.";
                }
                else
                {
                    StatusMessage = "Failed to save the parameters file.";
                }
            }
            dialog.Dispose();
        }
        private void BtnLoad_Click(object sender, EventArgs e)
        {
            var dialog = new FileChooserDialog(
                    "Load configuration",
                    this,
                    FileChooserAction.Open);
            dialog.AddButton(Stock.Cancel, ResponseType.Cancel);
            dialog.AddButton(Stock.Open, ResponseType.Ok);
            dialog.SetCurrentFolder(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            dialog.SelectMultiple = false;

            var response = (ResponseType)dialog.Run();
            if (response == ResponseType.Ok)
            {
                if (_sikInterface.LoadParamsFromFile(dialog.Filename))
                {
                    _sikInterface.SiKConfig.PropertyChanged += _boardIdentifiers.SiKConfig_PropertyChanged;
                    _dataTableControls.CreateBindings(_sikInterface.SiKConfig);
                    StatusMessage = "Parameters loaded from a config file.";
                }
                else
                {
                    StatusMessage = "Failed to load the parameters.";
                }
            }
            dialog.Dispose();
        }
        private void BtnRssi_Click(object sender, EventArgs e)
        {
            // TODO
        }
    }
}
