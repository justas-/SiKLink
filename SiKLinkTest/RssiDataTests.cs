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
using NUnit.Framework;
using SiKLink;

namespace SiKLinkTest
{
    class RssiDataTests
    {
        [Test]
        public void ParseRssiDataString()
        {
            var valuestr = "L/R RSSI: 208/217  L/R noise: 49/30 pkts: 5  txe=1 rxe=2 stx=3 srx=4 ecc=5/6 temp=42 dco=7";
            var rss_data = new RssiDataEventArgs(valuestr);

            Assert.AreEqual(rss_data.LocalRssi, 208);
            Assert.AreEqual(rss_data.RemoteRssi, 217);
            Assert.AreEqual(rss_data.LocalNoise, 49);
            Assert.AreEqual(rss_data.RemoteNoise, 30);
            Assert.AreEqual(rss_data.PacketsReceived, 5);
            Assert.AreEqual(rss_data.TransmitErrors, 1);
            Assert.AreEqual(rss_data.ReceiveErrors, 2);
            Assert.AreEqual(rss_data.SerialTxOverflow, 3);
            Assert.AreEqual(rss_data.SerialRxOverflow, 4);
            Assert.AreEqual(rss_data.CorrectedErrors, 5);
            Assert.AreEqual(rss_data.CorrectedPackets, 6);
            Assert.AreEqual(rss_data.RadioTemperature, 42);
            Assert.AreEqual(rss_data.DutyCycleOffset, 7);
        }
    }
}
