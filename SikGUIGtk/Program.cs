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
using Gtk;

namespace SiKGuiGtk
{
    class Program
    {
        public static Application App;
        public static Window Win;

        [STAThread]
        public static void Main(string[] args)
        {
            Application.Init();

            App = new Application("local.SiKLink.SiKGuiGtk", GLib.ApplicationFlags.None);
            App.Register(GLib.Cancellable.Current);

            Win = new MainWindow();
            App.AddWindow(Win);

            var menu = new GLib.Menu();
            menu.AppendItem(new GLib.MenuItem("About", "app.about"));
            menu.AppendItem(new GLib.MenuItem("Quit", "app.quit"));
            App.AppMenu = menu;

            var aboutAction = new GLib.SimpleAction("about", null);
            aboutAction.Activated += AboutActivated;
            App.AddAction(aboutAction);

            var quitAction = new GLib.SimpleAction("quit", null);
            quitAction.Activated += QuitActivated;
            App.AddAction(quitAction);

            Win.ShowAll();
            Application.Run();
        }
        private static void AboutActivated(object sender, EventArgs e)
        {
            var dialog = new AboutDialog
            {
                TransientFor = Win,
                ProgramName = "SiK GUI Gtk",
                Version = "1.0.0",
                Comments = "A Gtk-based GUI application for SiK Link.",
                LogoIconName = "system-run-symbolic",
                License = "The application is licensed under LGPL.",
                Website = "https://github.com/justas-/SiKLink",
                WebsiteLabel = "SiK Link and SiK GUI @ GitHub"
            };
            dialog.Run();
            dialog.Hide();
        }

        private static void QuitActivated(object sender, EventArgs e)
        {
            Application.Quit();
        }
    }
}
