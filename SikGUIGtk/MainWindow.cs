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

namespace SiKGuiGtk
{
    class MainWindow : Window
    {
        private HeaderBar _headerBar;
        private Notebook _notebook;
        private Box _pageContainer;

        public MainWindow() : base(WindowType.Toplevel)
        {
            // Setup GUI
            WindowPosition = WindowPosition.Center;
            DefaultSize = new Gdk.Size(600, 300);

            _headerBar = new HeaderBar();
            _headerBar.ShowCloseButton = true;
            _headerBar.Title = "SiK Radio Configurator";

            Titlebar = _headerBar;
            Destroyed += (sender, e) => Application.Quit();

            _notebook = new Notebook();
            _pageContainer = new Box(Orientation.Vertical, 1);
            _notebook.AppendPage(_pageContainer, null);

            _notebook.AppendPage(new Box(Orientation.Vertical, 5), null);

            Add(_notebook);

            CreatePortSelLine();
            CreateDataTable();
            CreateBoardIdLine();
            CreateButtonsLine();

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
            cbox.Add(new ComboBox());
            data_grid.Attach(cbox, 0, 0, 1, 1);

            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(new Label("Air Speed:"));
            cbox.Add(new Entry());
            data_grid.Attach(cbox, 1, 0, 1, 1);

            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(new CheckButton("ECC"));
            data_grid.Attach(cbox, 2, 0, 1, 1);

            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(new Label("MavLink:"));
            cbox.Add(new ComboBox());
            data_grid.Attach(cbox, 3, 0, 1, 1);

            // Line 2
            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(new Label("Min Freq:"));
            cbox.Add(new Entry());
            data_grid.Attach(cbox, 0, 1, 1, 1);

            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(new Label("Max Freq::"));
            cbox.Add(new Entry());
            data_grid.Attach(cbox, 1, 1, 1, 1);

            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(new Label("Num Chan:"));
            cbox.Add(new Entry());
            data_grid.Attach(cbox, 2, 1, 1, 1);

            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(new Label("Tx Power:"));
            cbox.Add(new ComboBox());
            data_grid.Attach(cbox, 3, 1, 1, 1);

            // Line 3
            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(new Label("Net ID:"));
            cbox.Add(new Entry());
            data_grid.Attach(cbox, 0, 2, 1, 1);

            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(new Label("Duty Cycle:"));
            cbox.Add(new ComboBox());
            data_grid.Attach(cbox, 1, 2, 1, 1);

            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(new Label("LBT RSSI:"));
            cbox.Add(new ComboBox());
            data_grid.Attach(cbox, 2, 2, 1, 1);

            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(new Label("Max Wnd:"));
            cbox.Add(new ComboBox());
            data_grid.Attach(cbox, 3, 2, 1, 1);

            // Line 4
            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(new CheckButton("RTS/CTS"));
            data_grid.Attach(cbox, 0, 3, 1, 1);

            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(new CheckButton("Manchester"));
            data_grid.Attach(cbox, 1, 3, 1, 1);

            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(new CheckButton("Opp Send"));
            data_grid.Attach(cbox, 2, 3, 1, 1);

            cbox = new Box(Orientation.Horizontal, 1);
            cbox.Add(new Label("EEPROM Fmt:"));
            cbox.Add(new Entry());
            data_grid.Attach(cbox, 3, 3, 1, 1);
        }
        private void CreatePortSelLine()
        {
            Box conn_box = new Box(Orientation.Horizontal, 2);
            _pageContainer.Add(conn_box);

            conn_box.Add(new Label("Serial Port:"));
            var comports = SiKLink.SiKInterface.GetSerialPorts();
            comports.Add("<refresh>");

            ComboBox portname = new ComboBox(comports.ToArray());
            portname.Changed += Portname_Changed;
            conn_box.Add(portname);

            conn_box.Add(new Label("Port baudrate:"));
            ComboBox baudrate = new ComboBox(Helpers.SerialRates);
            conn_box.Add(baudrate);

            Button conn_btn = new Button();
            conn_btn.Label = "Connect";
            conn_btn.Clicked += (sender, e) => Debug.WriteLine("Connected clicked!");
            conn_box.Add(conn_btn);

            Button disconn_btn = new Button();
            disconn_btn.Label = "Disconnect";
            disconn_btn.Clicked += (sender, e) => Debug.WriteLine("Disconnected clicked!");
            conn_box.Add(disconn_btn);
        }
        private void CreateBoardIdLine()
        {
            Box ident_line = new Box(Orientation.Horizontal, 1);
            _pageContainer.Add(ident_line);

            ident_line.Add(new Label("Radio:"));
            Entry _radioIdEntry = new Entry();
            _radioIdEntry.IsEditable = false;
            ident_line.Add(_radioIdEntry);

            ident_line.Add(new Label("Radio Ver:"));
            Entry _radioVerEntry = new Entry();
            _radioVerEntry.IsEditable = false;
            ident_line.Add(_radioVerEntry);

            ident_line.Add(new Label("Board Id:"));
            Entry _boardIdEntry = new Entry();
            _boardIdEntry.IsEditable = false;
            ident_line.Add(_boardIdEntry);

            ident_line.Add(new Label("Board Freq:"));
            Entry _boardFreqEntry = new Entry();
            _boardFreqEntry.IsEditable = false;
            ident_line.Add(_boardFreqEntry);

            ident_line.Add(new Label("BL Ver:"));
            Entry _bootloadVerEntry = new Entry();
            _bootloadVerEntry.IsEditable = false;
            ident_line.Add(_bootloadVerEntry);
        }
        private void CreateButtonsLine()
        {
            Box buttons_line = new Box(Orientation.Horizontal, 5);
            _pageContainer.Add(buttons_line);

            Button read_btn = new Button();
            read_btn.Label = "Read Values";
            buttons_line.Add(read_btn);

            Button write_btn = new Button();
            write_btn.Label = "Write Values";
            buttons_line.Add(write_btn);

            Button save_eeprom_btn = new Button();
            save_eeprom_btn.Label = "Save to EEPROM";
            buttons_line.Add(save_eeprom_btn);

            Button restart_btn = new Button();
            restart_btn.Label = "Restart Radio";
            buttons_line.Add(restart_btn);

            Button save_btn = new Button();
            save_btn.Label = "Save";
            buttons_line.Add(save_btn);

            Button load_btn = new Button();
            load_btn.Label = "Load";
            buttons_line.Add(load_btn);

            Button rssi_btn = new Button();
            rssi_btn.Label = "RSSI";
            buttons_line.Add(rssi_btn);
        }
        private void Portname_Changed(object sender, EventArgs e)
        {
            Debug.WriteLine("Portname_Changed");
        }
    }
}
