using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SiKGUIWPF
{
    class Helpers
    {
        public static ReadOnlyCollection<int> SerialRates = new ReadOnlyCollection<int>(new int[] { 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200, 230400 });
    }
}
