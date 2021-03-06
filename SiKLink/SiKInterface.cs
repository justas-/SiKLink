﻿/*
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
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;
using System.Text.Json;
using System.IO;
using System;

namespace SiKLink
{
    public class SiKInterface
    {
        /// <summary>
        /// Get a list of serial ports available in the system.
        /// </summary>
        /// <returns>List of port names available</returns>
        public static List<string> GetSerialPorts()
        {
            try
            {
                return new List<string>(SerialPort.GetPortNames());
            }
            catch (System.ComponentModel.Win32Exception)
            {
                return new List<string>();
            }
        }
        /// <summary>
        /// Serial port connect status
        /// </summary>
        public bool PortConnected
        {
            get
            {
                if (_serialPort == null)
                    return false;

                return _serialPort.IsOpen;
            }
        }
        /// <summary>
        /// SiK radio in Command mode.
        /// </summary>
        public bool CommandMode { get; private set; } = false;
        /// <summary>
        /// Streaming of RSSI data is enabled
        /// </summary>
        public bool RssiStreamEnabled { get; private set; }
        /// <summary>
        /// Configuration parameters of the local SiK board.
        /// </summary>
        public SiKConfig SiKConfig = new SiKConfig();
        /// <summary>
        /// Configuration parameters of the remote SiK board.
        /// </summary>
        public SiKConfig SiKConfigRemote => throw new NotImplementedException();
        /// <summary>
        /// RSSI data received from the radio
        /// </summary>
        public event RssiDataReceived OnRssiData;

        protected SerialPort _serialPort;
        protected List<byte> _rssiBytesBuffer = new List<byte>(512);

        public SiKInterface() : this(new SerialPort()) { }

        public SiKInterface(SerialPort serial)
        {
            _serialPort = serial;
            _serialPort.NewLine = "\n";
            _serialPort.DataReceived += _serialPort_DataReceived;
        }
        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Process only when streaming data is enabled
            if (!RssiStreamEnabled)
                return;

            string str_sentence = _serialPort.ReadLine();
            if (str_sentence.StartsWith("L/R RSSI:"))
            {
                _processStreamingRssiData(str_sentence);
            }
        }
        private void _processStreamingRssiData(string data)
        {
            RssiDataEventArgs rssi_data;
            try
            {
                rssi_data = new RssiDataEventArgs(data);
            }
            catch
            {
                return;
            }

            OnRssiData?.Invoke(this, rssi_data);
        }
        /// <summary>
        /// Connect to Serial Port and enter the command mode
        /// </summary>
        /// <param name="port">Port to connect to</param>
        /// <param name="baudrate">Baud rate</param>
        /// <returns>true on success</returns>
        public bool Connect(string port, int baudrate)
        {
            if (PortConnected)
                return true;

            _serialPort.BaudRate = baudrate;
            _serialPort.PortName = port;

            try
            {
                _serialPort.Open();
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check if the connected radio is in the command mode
        /// </summary>
        /// <returns>true if radio is in the command mode</returns>
        public bool CheckCommandMode()
        {
            if (!PortConnected)
                throw new PortNotConnectedException();

            // Write a single plus char
            _serialPort.Write("+");
            Thread.Sleep(100);

            // Expect an echo
            var ret = _serialPort.BytesToRead;
            if (ret == 0)
                return false;

            // Try to read data
            var ret_char = _serialPort.ReadChar();
            if (ret_char == '+')
            {
                _serialPort.WriteLine("AT\r");  // Send NOP
                _serialPort.ReadLine();         // Consume NOP echo
                CommandMode = true;
                return true;
            }

            return false;
        }
        /// <summary>
        /// Enter command mode.
        /// </summary>
        /// <returns>true on success</returns>
        public bool EnterCommandMode()
        {
            if (!PortConnected)
                throw new PortNotConnectedException();

            string reply;

            try
            {
                _serialPort.Write("+++");
                Thread.Sleep(1000);
                reply = _serialPort.ReadLine();
            }
            catch
            {
                return false;
            }

            if (!reply.StartsWith("OK"))
            {
                CommandMode = false;
                return false;
            }

            CommandMode = true;
            return true;
        }
        /// <summary>
        /// Disconnect from the SiK device.
        /// </summary>
        public void Disconnect()
        {
            if (!PortConnected)
                return;

            if (CommandMode)
            {
                SendAtCommand("O", false);
                CommandMode = false;
            }

            _serialPort.Close();
        }
        /// <summary>
        /// Read SiK board identification parameters.
        /// </summary>
        /// <returns>true on success, false otherwise</returns>
        public bool ReadIdentificationData()
        {
            if (!PortConnected)
                throw new PortNotConnectedException();

            if (!CommandMode)
                throw new NotInCommandModeException();

            try
            {
                SiKConfig.RadioBanner = SendAtCommand("I0");
                SiKConfig.RadioVersion = SendAtCommand("I1");
                SiKConfig.BoardId = SendAtCommand("I2");
                SiKConfig.BoardFrequency = Constants.BoardFreqStr[int.Parse(SendAtCommand("I3"))];
                SiKConfig.BootloaderVersion = SendAtCommand("I4");
            }
            catch
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// Reboot the radio
        /// </summary>
        /// <returns>true on success</returns>
        public bool RebootRadio()
        {
            if (!PortConnected)
                throw new PortNotConnectedException();

            if (!CommandMode)
                throw new NotInCommandModeException();

            try
            {
                SendAtCommand("Z", false);
                return true;
            }
            catch
            {
                return false;
            }

        }
        /// <summary>
        /// Save current parameters to the EEPROM
        /// </summary>
        /// <returns>true on success</returns>
        public bool SaveToEEPROM()
        {
            if (!PortConnected)
                throw new PortNotConnectedException();

            if (!CommandMode)
                throw new NotInCommandModeException();

            try
            {
                return SendAtCommand("&W") == "OK";
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Read all EEPROM configuration parameters
        /// </summary>
        /// <returns>true on success, false otherwise</returns>
        public bool ReadEEPROMData()
        {
            if (!PortConnected)
                throw new PortNotConnectedException();

            if (!CommandMode)
                throw new NotInCommandModeException();

            var params_list = new List<string>();

            try
            {
                // Send request and consume echo
                SendAtCommand("I5", false);

                for (int i = 16; i > 0; i--)
                {
                    string param = _serialPort.ReadLine();
                    params_list.Add(param);
                }
            }
            catch
            {
                return false;
            }

            foreach (var line in params_list)
            {
                var tokens = line.Split(':');
                var param_id = tokens[0];
                var param_val = tokens[1].Split("=")[1].Trim('\r');

                switch (param_id)
                {
                    case "S0":
                        SiKConfig.ParameterFormat = int.Parse(param_val);
                        break;
                    case "S1":
                        SiKConfig.SerialSpeed = int.Parse(param_val);
                        break;
                    case "S2":
                        SiKConfig.AirSpeed = int.Parse(param_val);
                        break;
                    case "S3":
                        SiKConfig.NetworkID = int.Parse(param_val);
                        break;
                    case "S4":
                        SiKConfig.TxPower = int.Parse(param_val);
                        break;
                    case "S5":
                        SiKConfig.ECC = param_val == "1" ? true : false;
                        break;
                    case "S6":
                        SiKConfig.MavlinkMode = int.Parse(param_val);
                        break;
                    case "S7":
                        SiKConfig.OpportunisticResend = param_val == "1" ? true : false;
                        break;
                    case "S8":
                        SiKConfig.MinFrequency = int.Parse(param_val);
                        break;
                    case "S9":
                        SiKConfig.MaxFrequency = int.Parse(param_val);
                        break;
                    case "S10":
                        SiKConfig.NumChannels = int.Parse(param_val);
                        break;
                    case "S11":
                        SiKConfig.DutyCycle = int.Parse(param_val);
                        break;
                    case "S12":
                        SiKConfig.LbtRssiThreshold = int.Parse(param_val);
                        break;
                    case "S13":
                        SiKConfig.ManchesterEncoding = param_val == "1" ? true : false;
                        break;
                    case "S14":
                        SiKConfig.UseRtsCts = param_val == "1" ? true : false;
                        break;
                    case "S15":
                        SiKConfig.MaxWindowSize = int.Parse(param_val);
                        break;
                    default:
                        continue;
                }
            }

            return true;
        }
        /// <summary>
        /// Transfer all parameter values to the radio.
        /// </summary>
        /// <remarks>This function does not save parameters to the EEPROM!</remarks>
        /// <returns>true on success</returns>
        public bool SaveParameters()
        {
            if (!PortConnected)
                throw new PortNotConnectedException();

            if (!CommandMode)
                throw new NotInCommandModeException();

            try
            {
                // Params format is immutable.
                //if (!WriteParameter(Constants.SikParameters.FORMAT, SiKConfig.ParameterFormat))
                //    return false;
                if (!WriteParameter(Constants.SikParameters.SERIAL_SPEED, SiKConfig.SerialSpeed))
                    return false;
                if (!WriteParameter(Constants.SikParameters.AIR_SPEED, SiKConfig.AirSpeed))
                    return false;
                if (!WriteParameter(Constants.SikParameters.NETID, SiKConfig.NetworkID))
                    return false;
                if (!WriteParameter(Constants.SikParameters.TXPOWER, SiKConfig.TxPower))
                    return false;
                if (!WriteParameter(Constants.SikParameters.ECC, SiKConfig.ECC))
                    return false;
                if (!WriteParameter(Constants.SikParameters.MAVLINK, SiKConfig.MavlinkMode))
                    return false;
                if (!WriteParameter(Constants.SikParameters.OPPRESEND, SiKConfig.OpportunisticResend))
                    return false;
                if (!WriteParameter(Constants.SikParameters.MIN_FREQ, SiKConfig.MinFrequency))
                    return false;
                if (!WriteParameter(Constants.SikParameters.MAX_FREQ, SiKConfig.MaxFrequency))
                    return false;
                if (!WriteParameter(Constants.SikParameters.NUM_CHANNELS, SiKConfig.NumChannels))
                    return false;
                if (!WriteParameter(Constants.SikParameters.DUTY_CYCLE, SiKConfig.DutyCycle))
                    return false;
                if (!WriteParameter(Constants.SikParameters.LBT_RSSI, SiKConfig.LbtRssiThreshold))
                    return false;
                if (!WriteParameter(Constants.SikParameters.MANCHESTER, SiKConfig.ManchesterEncoding))
                    return false;
                if (!WriteParameter(Constants.SikParameters.RTSCTS, SiKConfig.UseRtsCts))
                    return false;
                if (!WriteParameter(Constants.SikParameters.MAX_WINDOW, SiKConfig.MaxWindowSize))
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Save current configuration values to a JSON backup file.
        /// </summary>
        /// <param name="filename">Filename to save</param>
        /// <returns>true on success</returns>
        public bool SaveParamsToFile(string filename)
        {
            string jsonString = JsonSerializer.Serialize(SiKConfig);

            using (StreamWriter outputFile = new StreamWriter(filename))
            {
                try
                {
                    outputFile.WriteLine(jsonString);
                    return true;
                }
                catch
                {
                    // Consume errors if any and return failure
                    return false;
                }
            }
        }
        /// <summary>
        /// Load configuration parameters from a JSON file
        /// </summary>
        /// <param name="filename">path to a parameters file</param>
        /// <returns>true on success</returns>
        public bool LoadParamsFromFile(string filename)
        {
            string jsonString;

            using (StreamReader inputFile = new StreamReader(filename))
            {
                try
                {
                    jsonString = inputFile.ReadToEnd();
                }
                catch
                {
                    return false;
                }
            }

            try
            {
                var new_data = JsonSerializer.Deserialize<SiKConfig>(jsonString);
                SiKConfig = new_data;
            }
            catch
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// Enable/disable receiving and processing RSSI and related data.
        /// </summary>
        public void ToggleRssiDebug()
        {
            // Important! Stop reading data before the next line which will block trying to consume the echo
            RssiStreamEnabled = !RssiStreamEnabled;
            SendAtCommand("&T=RSSI", false);
        }
        /// <summary>
        /// Set SiK radio parameter value
        /// </summary>
        /// <param name="paramNum">Parameter number</param>
        /// <param name="value">Parameter integer value</param>
        /// <returns>true on success</returns>
        protected bool WriteParameter(int paramNum, int value)
        {
            string op_result = SendAtCommand($"S{paramNum}={value}");
            return op_result == "OK";
        }
        /// <summary>
        /// Set Sik radio parameter value
        /// </summary>
        /// <param name="paramNum">Parameter number</param>
        /// <param name="value">Parameter boolean value</param>
        /// <returns></returns>
        protected bool WriteParameter(int paramNum, bool value)
        {
            string bool_value = value ? "1" : "0";
            string op_result = SendAtCommand($"S{paramNum}={bool_value}");
            return op_result == "OK";
        }
        public bool WriteParameter(Constants.SikParameters parameter, int value)
        {
            if (!PortConnected)
                throw new PortNotConnectedException();

            if (!CommandMode)
                throw new NotInCommandModeException();

            return WriteParameter((int)parameter, value);
        }
        public bool WriteParameter(Constants.SikParameters parameter, bool value)
        {
            if (!PortConnected)
                throw new PortNotConnectedException();

            if (!CommandMode)
                throw new NotInCommandModeException();

            return WriteParameter((int)parameter, value);
        }
        /// <summary>
        /// Send AT command which expects one line return.
        /// </summary>
        /// <param name="command">Command (without AT)</param>
        /// <param name="with_reply">Expect a reply</param>
        /// <returns>reply from radio or empty str</returns>
        protected string SendAtCommand(string command, bool with_reply = true)
        {
            string at_command = $"AT{command}";
            _serialPort.Write($"{at_command}\r");   // \r send command for processing

            string echo = _serialPort.ReadLine();
            echo = echo.Trim('\r');
            if (echo != at_command)
            {
                Debug.WriteLine($"Got something else than echo: {echo}");
            }

            // Return an empty str if no reply expected
            if (!with_reply)
                return "";

            string reply = _serialPort.ReadLine();
            reply = reply.Trim('\r');
            return reply;
        }

        public delegate void RssiDataReceived(object sender, RssiDataEventArgs rssi);
    }
}
