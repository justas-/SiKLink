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
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Threading;

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
        public bool CommandMode { get; private set;  } = false;
        /// <summary>
        /// Configuration parameters of the local SiK board.
        /// </summary>
        public SiKConfig SiKConfig = new SiKConfig();
        /// <summary>
        /// Configuration parameters of the remote SiK board.
        /// </summary>
        public SiKConfig SiKConfigRemote; // TODO: Implement

        private SerialPort _serialPort;

        public SiKInterface()
        {
            _serialPort = new SerialPort();
            _serialPort.NewLine = "\r\n";
        }
        /// <summary>
        /// Connect to Serial Port and enter the command mode
        /// </summary>
        /// <param name="port">Port to connect to</param>
        /// <param name="baudrate">Baud rate</param>
        /// <returns>true on success and in command mode</returns>
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
        /// Enter command mode.
        /// </summary>
        /// <returns>true on success</returns>
        public bool EnterCommandMode()
        {
            if (!PortConnected)
                return false;

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
                return false;

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
                SendATNoRep("O");
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
            if (!PortConnected || !CommandMode)
                throw new IOException();

            try
            {
                SiKConfig.RadioBanner = SendATOneLineRep("I0");
                SiKConfig.RadioVersion = SendATOneLineRep("I1");
                SiKConfig.BoardId = SendATOneLineRep("I2");
                SiKConfig.BoardFrequency = Constants.BoardFreqStr[int.Parse(SendATOneLineRep("I3"))];
                SiKConfig.BootloaderVersion = SendATOneLineRep("I4");
            }
            catch
            {
                // Consume exception.
                // TODO: Make a log
                return false;
            }

            return true;
        }
        /// <summary>
        /// Read all EEPROM configuration parameters
        /// </summary>
        /// <returns>true on success, false otherwise</returns>
        public bool ReadEEPROMData()
        {
            if (!PortConnected || !CommandMode)
                throw new IOException();

            var params_list = new List<string>();

            try
            {
                _serialPort.WriteLine("ATI5");

                for (int i = 17; i > 0; i--)
                {
                    var param = _serialPort.ReadLine();
                    params_list.Add(param);
                    Debug.WriteLine(param);
                }
            }
            catch
            {
                return false;
            }

            foreach(var line in params_list)
            {
                // Skip echo
                if (line.StartsWith("AT"))
                    continue;

                var tokens = line.Split(':');
                var param_id = tokens[0];
                var param_val = tokens[1].Split("=")[1];
                
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
        /// Set SiK radio parameter value
        /// </summary>
        /// <param name="paramNum">Parameter number</param>
        /// <param name="value">Parameter integer value</param>
        /// <returns>true on success</returns>
        public bool WriteParameter(int paramNum, int value)
        {
            return true;
        }
        /// <summary>
        /// Set Sik radio parameter value
        /// </summary>
        /// <param name="paramNum">Parameter number</param>
        /// <param name="value">Parameter boolean value</param>
        /// <returns></returns>
        public bool WriteParameter(int paramNum, bool value)
        {
            return true;
        }
        public bool WriteParameter(Constants.SikParameters parameter, int value)
        {
            return WriteParameter((int)parameter, value);
        }
        public bool WriteParameter(Constants.SikParameters parameter, bool value)
        {
            return WriteParameter((int)parameter, value);
        }
        /// <summary>
        /// Send AT command which expects OK as reply.
        /// </summary>
        /// <param name="command">Comman (without AT)</param>
        /// <returns>true on OK</returns>
        private void SendATNoRep(string command)
        {
            _serialPort.WriteLine($"AT{command}");
            // Consume echo if any
            _serialPort.ReadLine();
        }
        /// <summary>
        /// Send AT command which expects one line return.
        /// </summary>
        /// <param name="command">Command (without AT)</param>
        /// <returns>SiK returned data</returns>
        private string SendATOneLineRep(string command)
        {
            _serialPort.WriteLine($"AT{command}");
            Thread.Sleep(100);
            _serialPort.ReadLine(); // Consume echo
            Thread.Sleep(100);
            var rep = _serialPort.ReadLine();
            return rep;
        }

    }
}
