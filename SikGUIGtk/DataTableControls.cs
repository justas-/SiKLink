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
using System.Linq;

namespace SiKGuiGtk
{
    /// <summary>
    /// HMI elements and data bindings of the data table.
    /// </summary>
    public class DataTableControls
    {
        // Line 1
        public ComboBoxText SerialSpeedCombo;
        public Entry AirSpeedEntry;
        public CheckButton EccCheck;
        public ComboBoxText MavLinkVerCombo;

        // Line 2
        public Entry MinFreqEntry;
        public Entry MaxFreqEntry;
        public Entry NumChanEntry;
        public ComboBoxText TxPowerCombo;

        // Line 3
        public Entry NetIdEntry;
        public ComboBoxText DutyCycleCombo;
        public ComboBoxText LbtRssiCombo;
        public ComboBoxText MaxWndCombo;

        // Line 4
        public CheckButton RtsCtsCheck;
        public CheckButton ManchesterCheck;
        public CheckButton OpportunisticCheck;
        public Entry EepromFmtEntry;

        public DataTableControls()
        {
            SerialSpeedCombo = new ComboBoxText();
            foreach (var item in SiKLink.Constants.SiKSerialRates)
                SerialSpeedCombo.Append(item.ToString(), item.ToString());
            AirSpeedEntry = new Entry();
            EccCheck = new CheckButton("ECC");
            MavLinkVerCombo = new ComboBoxText();
            foreach (var item in Helpers.MavVersions)
                MavLinkVerCombo.Append(
                    Helpers.MavVersions.IndexOf(item).ToString(),
                    item);

            MinFreqEntry = new Entry();
            MaxFreqEntry = new Entry();
            NumChanEntry = new Entry();
            TxPowerCombo = new ComboBoxText();
            foreach (var item in SiKLink.Constants.AirPower)
                TxPowerCombo.Append(item.ToString(), item.ToString());

            NetIdEntry = new Entry();
            DutyCycleCombo = new ComboBoxText();
            foreach (var item in Enumerable.Range(1, 100))
                DutyCycleCombo.Append(item.ToString(), item.ToString());
            LbtRssiCombo = new ComboBoxText();
            foreach (var item in Enumerable.Range(0, 255))
                LbtRssiCombo.Append(item.ToString(), item.ToString());
            MaxWndCombo = new ComboBoxText();
            foreach (var item in Enumerable.Range(33, 99))
                MaxWndCombo.Append(item.ToString(), item.ToString());

            RtsCtsCheck = new CheckButton("RTS/CTS");
            ManchesterCheck = new CheckButton("Manchester");
            OpportunisticCheck = new CheckButton("Opp. Send");
            EepromFmtEntry = new Entry();
            EepromFmtEntry.IsEditable = false;
        }
        /// <summary>
        /// Create HMI to Data Model bindings
        /// </summary>
        public void CreateBindings(SiKConfig sik_config)
        {
            // Subscribe to Data Model -> HMI direction
            sik_config.PropertyChanged += SiKConfig_PropertyChanged;

            // Implement HMI -> Data Model direction
            SerialSpeedCombo.Changed += (s, e) => { sik_config.SerialSpeed = int.Parse(SerialSpeedCombo.ActiveText); };
            AirSpeedEntry.Changed += (s, e) => { sik_config.AirSpeed = int.Parse(AirSpeedEntry.Text); };
            EccCheck.Toggled += (s, e) => { sik_config.ECC = EccCheck.Active; };
            MavLinkVerCombo.Changed += (s, e) => { sik_config.MavlinkMode = Helpers.MavVersions.IndexOf(MavLinkVerCombo.ActiveText); };

            MinFreqEntry.Changed += (s, e) => { sik_config.MinFrequency = int.Parse(MinFreqEntry.Text); };
            MaxFreqEntry.Changed += (s, e) => { sik_config.MaxFrequency = int.Parse(MaxFreqEntry.Text); };
            NumChanEntry.Changed += (s, e) => { sik_config.NumChannels = int.Parse(NumChanEntry.Text); };
            TxPowerCombo.Changed += (s, e) => { sik_config.TxPower = int.Parse(TxPowerCombo.ActiveText); };

            NetIdEntry.Changed += (s, e) => { sik_config.NetworkID = int.Parse(NetIdEntry.Text); };
            DutyCycleCombo.Changed += (s, e) => { sik_config.DutyCycle = int.Parse(DutyCycleCombo.ActiveText); };
            LbtRssiCombo.Changed += (s, e) => { sik_config.LbtRssiThreshold = int.Parse(LbtRssiCombo.ActiveText); };
            MaxWndCombo.Changed += (s, e) => { sik_config.MaxWindowSize = int.Parse(MaxWndCombo.ActiveText); };

            RtsCtsCheck.Toggled += (s, e) => { sik_config.UseRtsCts = RtsCtsCheck.Active; };
            ManchesterCheck.Toggled += (s, e) => { sik_config.ManchesterEncoding = ManchesterCheck.Active; };
            OpportunisticCheck.Toggled += (s, e) => { sik_config.OpportunisticResend = OpportunisticCheck.Active; };
        }
        /// <summary>
        /// Implement Data Model to HMI binding
        /// </summary>
        public void SiKConfig_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var sik_conf = sender as SiKConfig;
            switch (e.PropertyName)
            {
                case "ParameterFormat":
                    EepromFmtEntry.Text = sik_conf.ParameterFormat.ToString();
                    break;
                case "SerialSpeed":
                    SerialSpeedCombo.SetActiveId(sik_conf.SerialSpeed.ToString());
                    break;
                case "AirSpeed":
                    AirSpeedEntry.Text = sik_conf.AirSpeed.ToString();
                    break;
                case "NetworkID":
                    NetIdEntry.Text = sik_conf.NetworkID.ToString();
                    break;
                case "TxPower":
                    TxPowerCombo.SetActiveId(sik_conf.TxPower.ToString());
                    break;
                case "ECC":
                    EccCheck.Active = sik_conf.ECC;
                    break;
                case "MavlinkMode":
                    MavLinkVerCombo.SetActiveId(sik_conf.MavlinkMode.ToString());
                    break;
                case "OpportunisticResend":
                    OpportunisticCheck.Active = sik_conf.OpportunisticResend;
                    break;
                case "MinFrequency":
                    MinFreqEntry.Text = sik_conf.MinFrequency.ToString();
                    break;
                case "MaxFrequency":
                    MaxFreqEntry.Text = sik_conf.MaxFrequency.ToString();
                    break;
                case "NumChannels":
                    NumChanEntry.Text = sik_conf.NumChannels.ToString();
                    break;
                case "DutyCycle":
                    DutyCycleCombo.SetActiveId(sik_conf.DutyCycle.ToString());
                    break;
                case "LbtRssiThreshold":
                    LbtRssiCombo.SetActiveId(sik_conf.LbtRssiThreshold.ToString());
                    break;
                case "ManchesterEncoding":
                    ManchesterCheck.Active = sik_conf.ManchesterEncoding;
                    break;
                case "UseRtsCts":
                    RtsCtsCheck.Active = sik_conf.UseRtsCts;
                    break;
                case "MaxWindowSize":
                    MaxWndCombo.SetActiveId(sik_conf.MaxWindowSize.ToString());
                    break;
                default:
                    break;
            }
        }
    }
}
