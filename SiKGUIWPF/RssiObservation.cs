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
namespace SiKGUIWPF
{
    public class RssiObservation
    {
        public static int NextId = 1;
        public int Id { get; set; }
        public int LocalRssi { get; set; }
        public int RemoteRssi { get; set; }
        public int LocalNoise { get; set; }
        public int RemoteNoise { get; set; }
    }
}
