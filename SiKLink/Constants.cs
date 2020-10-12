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
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SiKLink
{
    public class Constants
    {
        public static ReadOnlyCollection<int> AirRates = new ReadOnlyCollection<int>(new int[]{ 2, 4, 8, 16, 19, 24, 32, 48, 64, 96, 128, 192, 250 });
        public static ReadOnlyCollection<int> MavlinkFrame = new ReadOnlyCollection<int>(new int[] { 0, 1, 2 });
        public static ReadOnlyCollection<int> SiKSerialRates = new ReadOnlyCollection<int>(new int[] { 1, 2, 4, 9, 19, 38, 57, 115, 230 });
        public static ReadOnlyCollection<int> AirPower = new ReadOnlyCollection<int>(new int[] { 1, 2, 5, 8, 11, 14, 17, 20 });
        public static ReadOnlyDictionary<int, string> BoardFreqStr = new ReadOnlyDictionary<int, string>(new Dictionary<int, string>() { { 0x43, "433" }, { 0x47, "470" }, { 0x86, "868" }, { 0x91, "915" }, { 0xf0, "NONE" } });
    }   
}
