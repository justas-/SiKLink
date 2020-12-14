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
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace SiKGUIWPF
{
    [ValueConversion(typeof(int), typeof(ComboBoxItem))]
    class MavVerToIdConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int numeric_id = (int)value;
            return Helpers.MavVersions[numeric_id];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ComboBoxItem item = (ComboBoxItem)value;
            return Helpers.MavVersions.IndexOf(item);
        }
    }
}
