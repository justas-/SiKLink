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
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;

namespace SiKGUIWPF
{
    /// <summary>
    /// Interaction logic for RssiFigure.xaml
    /// </summary>
    public partial class RssiFigure : UserControl
    {
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public RssiFigure()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Local RSSI",
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,4 }
                },
                new LineSeries
                {
                    Title = "Remote RSSI",
                    Values = new ChartValues<double> { 6, 7, 3, 4 ,6 },
                },
                new LineSeries
                {
                    Title = "Local Noise",
                    Values = new ChartValues<double> { 4,2,7,2,7 },
                },
                new LineSeries
                {
                    Title = "Remote Noise",
                    Values = new ChartValues<double> { 4,2,7,2,7 },
                }
            };

            var labels = new string[120];
            foreach (var item in Enumerable.Range(1, 120))
                labels[item - 1] = item.ToString();

            Labels = labels;

            DataContext = this;
        }
    }
}
