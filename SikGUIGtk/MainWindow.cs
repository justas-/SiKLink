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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SiKGuiGtk
{
    class MainWindow : Window
    {
        private HeaderBar _headerBar;
        private Notebook _notebook;

        public MainWindow() : base(WindowType.Toplevel)
        {
            // Setup GUI
            WindowPosition = WindowPosition.Center;
            DefaultSize = new Gdk.Size(800, 600);

            _headerBar = new HeaderBar();
            _headerBar.ShowCloseButton = true;
            _headerBar.Title = "GtkSharp Sample Application";

            Titlebar = _headerBar;
            Destroyed += (sender, e) => Application.Quit();

            VBox vbox1 = new VBox(false, 2);
            vbox1.Orientation = Orientation.Horizontal;
            VBox vbox2 = new VBox(false, 2);

            _notebook = new Notebook();
            _notebook.AppendPage(vbox1, null);
            _notebook.AppendPage(vbox2, null);
            Add(_notebook);

            vbox1.Add(new Label("Serial Port:"));
            var comports = SiKLink.SiKInterface.GetSerialPorts();
            comports.Add("<refresh>");

            ComboBox portname = new ComboBox(comports.ToArray());
            portname.Changed += Portname_Changed;
            vbox1.Add(portname);

            vbox1.Add(new Label("Port baudrate:"));
            ComboBox baudrate = new ComboBox(Helpers.SerialRates);
            vbox1.Add(baudrate);
        }

        private void Portname_Changed(object sender, EventArgs e)
        {
            Debug.WriteLine("Portname_Changed");
        }
    }
}
