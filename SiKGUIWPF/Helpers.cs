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
using System.Collections.ObjectModel;
using System.Text;

namespace SiKGUIWPF
{
    class Helpers
    {
        public static ReadOnlyCollection<int> SerialRates = new ReadOnlyCollection<int>(new int[] { 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200, 230400 });
    }
}
