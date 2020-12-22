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
using System.Linq;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;

namespace SiKGUIWPF
{
    /// <summary>
    /// Interaction logic for RssiFigure.xaml
    /// </summary>
    public partial class RssiFigure : UserControl, INotifyPropertyChanged
    {
        public SeriesCollection SeriesCollection { get; set; }
        public ChartValues<RssiObservation> ChartValues { get; set; }
        public double AxisStep { get; set; }
        public double AxisUnit { get; set; }
        public double AxisXMax
        {
            get { return _axisXMax; }
            set
            {
                _axisXMax = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AxisXMax"));
            }
        }
        public double AxisXMin
        {
            get { return _axisXMin; }
            set
            {
                _axisXMin = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AxisXMin"));
            }
        }

        private double _axisXMax;
        private double _axisXMin;

        public event PropertyChangedEventHandler PropertyChanged;

        public RssiFigure()
        {
            InitializeComponent();

            var local_rssi_mapper = Mappers.Xy<RssiObservation>().X(r => r.Id).Y(r => r.LocalRssi);
            var local_noise_mapper = Mappers.Xy<RssiObservation>().X(r => r.Id).Y(r => r.LocalNoise);
            var remote_rssi_mapper = Mappers.Xy<RssiObservation>().X(r => r.Id).Y(r => r.RemoteRssi);
            var remote_noise_mapper = Mappers.Xy<RssiObservation>().X(r => r.Id).Y(r => r.RemoteNoise);

            SeriesCollection = new SeriesCollection
            {
                new LineSeries(local_rssi_mapper)
                {
                    Title = "Local RSSI",
                    Values = new ChartValues<RssiObservation>()
                },
                new LineSeries(local_noise_mapper)
                {
                    Title = "Local Noise",
                    Values = new ChartValues<RssiObservation>()
                },
                new LineSeries(remote_rssi_mapper)
                {
                    Title = "Remote RSSI",
                    Values = new ChartValues<RssiObservation>()
                },
                new LineSeries(remote_noise_mapper)
                {
                    Title = "Remote Noise",
                    Values = new ChartValues<RssiObservation>()
                },
            };

            AxisStep = 10;
            AxisUnit = 1;

            SetAxisLimits(RssiObservation.NextId);

            DataContext = this;
        }
        public void AddValue(RssiObservation rssiData)
        {
            foreach (var series in SeriesCollection)
            {
                series.Values.Add(rssiData);
                if (series.Values.Count > 100) series.Values.RemoveAt(0);
            }
            SetAxisLimits(RssiObservation.NextId);
        }
        public void ClearValues()
        {
            foreach (var series in SeriesCollection)
            {
                series.Values.Clear();
            }
        }
        private void SetAxisLimits(int currentId)
        {
            AxisXMax = currentId + 1;
            AxisXMin = currentId - 100;
        }
    }
}
