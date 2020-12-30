# SiKLink

A C# interface library and a Graphical User Interface (GUI) application to configure SiK radio transceivers commonly used in hobby applications.

## SiKLink

SiKLink in a C# interface library to controll the SiK radio. It abstracts all serial communication into method calls. The library is written using .NET Core framework and is licensed under LGPL.

## SiKGUI

SiKGUI is an application providing graphical user interface to the SiKLink library. SiKGUI comes in two flavours. SiKGUIWPF is a Windows-only WPF application. SiKGUIGtk is a multi-platform (Windows, Linux, Mac) Gtk3-based application.

_*TLDR*_ - Find the latest binaries in [Github releases page](https://github.com/justas-/SiKLink/releases/).

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

SiKGUIGtk is a GUI application written in C# uing Gtk3 framework. It should be usable on MS Windows, Mac, and Linux operating systems.

![Gtk3 GUI](https://raw.githubusercontent.com/justas-/SiKLink/main/.github/SiKLinkGtk.jpg "Gtk3 GUI")

User controls of SiKGUIGtk are exactly the same as in WPF version. 

_N.B._ 

- RSSI figure drawing is not (yet) implemented.
- Serial port drop-down is populated when the app starts. If you connect the SiK radio while the app is running, select the "&lt;refresh&gt;" option to re-populate the list.
