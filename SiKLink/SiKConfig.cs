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
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace SiKLink
{
    public class SiKConfig : INotifyPropertyChanged
    {
        private int _format;
        private int _serialSpeed;
        private int _airSpeed;
        private int _netId;
        private int _txPower;
        private bool _ecc;
        private int _mavlink;
        private bool _opprsend;
        private int _minFreq;
        private int _maxFreq;
        private int _numChannels;
        private int _dutyCycle;
        private int _lbtRssi;
        private bool _manchester;
        private bool _rtscts;
        private int _maxWindow;

        private string _radioBanner;
        private string _radioVersion;
        private string _boardId;
        private string _boardFrequency;
        private string _bootloaderVersion;

        // ATS parameters https://github.com/ArduPilot/SiK/blob/master/Firmware/radio/parameters.h
        // Defaults:
        //{"FORMAT",         PARAM_FORMAT_CURRENT},
        //{"SERIAL_SPEED",   57}, // match APM default of 57600
        //{"AIR_SPEED",      64}, // relies on MAVLink flow control
        //{"NETID",          25},
        //{"TXPOWER",        20},
        //{"ECC",             0},
        //{"MAVLINK",         1},
        //{"OPPRESEND",       0},
        //{"MIN_FREQ",        0},
        //{"MAX_FREQ",        0},
        //{"NUM_CHANNELS",    0},
        //{"DUTY_CYCLE",    100},
        //{"LBT_RSSI",        0},
        //{"MANCHESTER",      0},
        //{"RTSCTS",          0},
        //{"MAX_WINDOW",    131},

        public int ParameterFormat
        {
            get
            {
                return _format;
            }
            set
            {
                if (_format != value)
                {
                    _format = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ParameterFormat"));
                }
            }
        }
        public int SerialSpeed
        {
            get
            {
                return _serialSpeed;
            }
            set
            {
                if (_serialSpeed != value)
                {
                    _serialSpeed = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SerialSpeed"));
                }
            }
        }
        public int AirSpeed
        {
            get
            {
                return _airSpeed;
            }
            set
            {
                if (_airSpeed != value)
                {
                    _airSpeed = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AirSpeed"));
                }
            }
        }
        public int NetworkID
        {
            get
            {
                return _netId;
            }
            set
            {
                if (_netId != value)
                {
                    _netId = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NetworkID"));
                }
            }
        }
        public int TxPower
        {
            get
            {
                return _txPower;
            }
            set
            {
                if (_txPower != value)
                {
                    _txPower = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TxPower"));
                }
            }
        }
        public bool ECC
        {
            get
            {
                return _ecc;
            }
            set
            {
                if (_ecc != value)
                {
                    _ecc = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ECC"));
                }
            }
        }
        public int MavlinkMode
        {
            get
            {
                return _mavlink;
            }
            set
            {
                if (_mavlink != value)
                {
                    _mavlink = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MavlinkMode"));
                }
            }
        }
        public bool OpportunisticResend
        {
            get
            {
                return _opprsend;
            }
            set
            {
                if (_opprsend != value)
                {
                    _opprsend = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OpportunisticResend"));
                }
            }
        }
        public int MinFrequency
        {
            get
            {
                return _minFreq;
            }
            set
            {
                if (_minFreq != value)
                {
                    _minFreq = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MinFrequency"));
                }
            }
        }
        public int MaxFrequency
        {
            get
            {
                return _maxFreq;
            }
            set
            {
                if (_maxFreq != value)
                {
                    _maxFreq = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MaxFrequency"));
                }
            }
        }
        public int NumChannels
        {
            get
            {
                return _numChannels;
            }
            set
            {
                if (_numChannels != value)
                {
                    _numChannels = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NumChannels"));
                }
            }
        }
        public int DutyCycle
        {
            get
            {
                return _dutyCycle;
            }
            set
            {
                if (_dutyCycle != value)
                {
                    _dutyCycle = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DutyCycle"));
                }
            }
        }
        public int LbtRssiThreshold
        {
            get
            {
                return _lbtRssi;
            }
            set
            {
                if (_lbtRssi != value)
                {
                    _lbtRssi = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LbtRssiThreshold"));
                }
            }
        }
        public bool ManchesterEncoding
        {
            get
            {
                return _manchester;
            }
            set
            {
                if (_manchester != value)
                {
                    _manchester = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LbtRssiThreshold"));
                }
            }
        }
        public bool UseRtsCts
        {
            get
            {
                return _rtscts;
            }
            set
            {
                if (_rtscts != value)
                {
                    _rtscts = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UseRtsCts"));
                }
            }
        }
        public int MaxWindowSize
        {
            get
            {
                return _maxWindow;
            }
            set
            {
                if (_maxWindow != value)
                {
                    _maxWindow = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MaxWindowSize"));
                }
            }
        }

        //
        // AT(I) command https://github.com/ArduPilot/SiK/blob/master/Firmware/radio/at.c Ln 322
        //

        /// <summary>
        /// Startup Banner String (ATI / ATI0 / g_banner_string)
        /// "RFD SiK " + APP_VERSION_HIGH + "." + APP_VERSION_LOW + " on " + BOARD_NAME;
        /// </summary>
        [JsonIgnore]
        public string RadioBanner
        {
            get
            {
                return _radioBanner;
            }
            set
            {
                if (_radioBanner != value)
                {
                    _radioBanner = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RadioBanner"));
                }
            }
        }
        /// <summary>
        /// Version String (ATI1 / g_version_string)
        /// </summary>
        [JsonIgnore]
        public string RadioVersion
        {
            get
            {
                return _radioVersion;
            }
            set
            {
                if (_radioVersion != value)
                {
                    _radioVersion = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RadioVersion"));
                }
            }
        }
        /// <summary>
        /// Board ID (ATI2 / BOARD_ID)
        /// </summary>
        [JsonIgnore]
        public string BoardId
        {
            get
            {
                return _boardId;
            }
            set
            {
                if (_boardId != value)
                {
                    _boardId = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BoardId"));
                }
            }
        }
        /// <summary>
        /// Board Frequency (ATI3 / g_board_frequency)
        /// </summary>
        [JsonIgnore]
        public string BoardFrequency
        {
            get
            {
                return _boardFrequency;
            }
            set
            {
                if (_boardFrequency != value)
                {
                    _boardFrequency = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BoardFrequency"));
                }
            }
        }
        /// <summary>
        /// Board Bootloader Version (ATI4 / g_board_bl_version)
        /// </summary>
        [JsonIgnore]
        public string BootloaderVersion
        {
            get
            {
                return _bootloaderVersion;
            }
            set
            {
                if (_bootloaderVersion != value)
                {
                    _bootloaderVersion = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BootloaderVersion"));
                }
            }
        }


        public SiKConfig() { }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
