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

namespace SiKLink
{
    public class RssiDataEventArgs : EventArgs
    {
        public int LocalRssi { get; }
        public int RemoteRssi { get; }
        public int LocalNoise { get; }
        public int RemoteNoise { get; }
        public int PacketsReceived { get; }
        public int TransmitErrors { get; }
        public int ReceiveErrors { get; }
        public int SerialTxOverflow { get; }
        public int SerialRxOverflow { get; }
        public int CorrectedErrors { get; }
        public int CorrectedPackets { get; }
        public int RadioTemperature { get; }
        public int DutyCycleOffset { get; }

        public RssiDataEventArgs(string valuestring)
        {
            var tokens = valuestring.Split(' ');

            var lr_rssi = tokens[2].Split('/');
            LocalRssi = int.Parse(lr_rssi[0]);
            RemoteRssi = int.Parse(lr_rssi[1]);

            var lr_noise = tokens[6].Split('/');
            LocalNoise = int.Parse(lr_noise[0]);
            RemoteNoise = int.Parse(lr_noise[1]);

            PacketsReceived = int.Parse(tokens[8]);

            var txe = tokens[10].Split('=');
            TransmitErrors = int.Parse(txe[1]);

            var rxe = tokens[11].Split('=');
            ReceiveErrors = int.Parse(rxe[1]);

            var stx = tokens[12].Split('=');
            SerialTxOverflow = int.Parse(stx[1]);

            var srx = tokens[13].Split('=');
            SerialRxOverflow = int.Parse(srx[1]);

            var ecc = tokens[14].Split('=');
            var sub_ecc = ecc[1].Split('/');
            CorrectedErrors = int.Parse(sub_ecc[0]);
            CorrectedPackets = int.Parse(sub_ecc[1]);

            var temp = tokens[15].Split('=');
            RadioTemperature = int.Parse(temp[1]);

            var dco = tokens[16].Split('=');
            DutyCycleOffset = int.Parse(dco[1]);
        }
    }
}
