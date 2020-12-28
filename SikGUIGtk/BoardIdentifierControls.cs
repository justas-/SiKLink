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
using SiKLink;

namespace SiKGuiGtk
{
    public class BoardIdentifierControls
    {
        public Entry RadioIdEntry;
        public Entry RadioVerEntry;
        public Entry BoardIdEntry;
        public Entry BoardFreqEntry;
        public Entry BootloaderVerEntry;

        public BoardIdentifierControls()
        {
            RadioIdEntry = new Entry();
            RadioIdEntry.IsEditable = false;
            RadioVerEntry = new Entry();
            RadioVerEntry.IsEditable = false;
            BoardIdEntry = new Entry();
            BoardIdEntry.IsEditable = false;
            BoardFreqEntry = new Entry();
            BoardFreqEntry.IsEditable = false;
            BootloaderVerEntry = new Entry();
            BootloaderVerEntry.IsEditable = false;
        }

        public void SiKConfig_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var sik_conf = sender as SiKConfig;
            switch (e.PropertyName)
            {
                case "RadioBanner":
                    RadioIdEntry.Text = sik_conf.RadioBanner;
                    break;
                case "RadioVersion":
                    RadioVerEntry.Text = sik_conf.RadioVersion;
                    break;
                case "BoardId":
                    BoardIdEntry.Text = sik_conf.BoardId;
                    break;
                case "BoardFrequency":
                    BoardFreqEntry.Text = sik_conf.BoardFrequency;
                    break;
                case "BootloaderVersion":
                    BootloaderVerEntry.Text = sik_conf.BootloaderVersion;
                    break;
                default:
                    break;
            }
        }
    }
}
