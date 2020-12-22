# SiKLink and SiKGUI

A C# interface library and a GUI application to configure SiK radio transceivers commonly used in hobby applications.

## SiKLink

SiKLink in a C# interface library to controll the SiK radio. It abstracts all serial communication into method calls. The library is written using .NET Core framework and is licensed under LGPL.

## SiKGUI

SiKGUI is a set of graphical application providing user interface to SiKLink. The goals of SiKGUI are to make SiKLink user-friendly, and to try the portability of .NET between the different operating systems and GUI frameworks.

_*TLDR*_ - Find the latest binary in [Github releases page](https://github.com/justas-/SiKLink/releases/).

### SiKGUIWPF

SiKGUIWPF is a GUI application written in C# and using WPF. Due to the use of WPF, it only runs on MS Windows.

![WPF GUI](https://raw.githubusercontent.com/justas-/SiKLink/main/.github/SikLinkWpfMain.jpg "WPF GUI")

To connect to the radio, select the serial port and the baudrate. Once the connection is established, board identification fields will be filled with the information provided by the radio.

User controls:
- Read Values - read the current configuration and display in the GUI
- Write Values - send the values from the GUI to the radio memory. Values are not yet saved to EEPROM.
- Save to EEPROM - save the values *currently in the radio's memory* to the EEPROM.
- Restart Radio - restart the radio. Required for most parameters to take effect.
- Load / Save - Load / Save the currently displayed parameters to / from the JSON file. You need to "Write Value" and "Save to EEPROM" after loading the JSON file to transfer the parameters permanently to the radio.
- RSSI - Show the local and remote received signal and noise levels. All other functions are disabled while RSSI display is active. Click the RSSI once more to close the display and re-enable other functions.

![WPF GUI with RSSI display](https://raw.githubusercontent.com/justas-/SiKLink/main/.github/SikLinkWpfRssi.jpg "WPF GUI with RSSI display")

> Note that for two radios to communicate, parameters with blue background must match on both ends!

### SiKGUIGtk

SiKGUIGtk is TBD GUI application written in C# uing Gtk framework. It should be usable on MS Windows, Mac, and Linux operating systems.
