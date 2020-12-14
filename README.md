# SiKLink and SiKGUI

A C# interface library and a GUI application to configure SiK radio transceivers commonly used in hobby applications.

## SiKLink

SiKLink in a C# interface library to controll the SiK radio. It abstracts serial communication using method calls. The library is writeen using .NET Core and is licensed under LGPL.

## SiKGUI

SiKGUI is a set of GUI interfaces for SiKLink. The goal is to make SiKLink user-friendly and to try portability of .NET between different operating systems and GUI frameworks.

### SiKGUIWPF

SiKGUIWPF is a GUI application written in C# using WPF. Due to the use of WPF, it can run on MS Windows only.

![WPF GUI](https://raw.githubusercontent.com/justas-/SiKLink/main/.github/SiKWPFGUI.png "WPF GUI")

To connect to the radio, select the serial port and the baudrate. Once the connection is established, board identification fields will be filled out with information provided by the radio.

User controls:
- Read Values - read the current configuration and display in the GUI
- Write Values - send the values from the GUI to the radio memory, but do not save them.
- Save to EEPROM - save the values *in the radio memory* to the EEPROM.
- Restart Radio - restart the radio into the data mode. Required for most parameters to take effect.

> Note that for 2 radios to communicate, parameters with blue background must match on both ends!

### SiKGUIGtk

SiKGUIGtk is TBD GUI application written in C# uing Gtk framework. It should be usable on MS Windows, Mac, and Linux operating systems.
