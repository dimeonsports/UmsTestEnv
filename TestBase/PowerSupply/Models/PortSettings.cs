namespace TestBase.PowerSupply.Models;

public sealed record PortSettings(
    string PortName,                                         // Port name (f.e. "COM5")
    int BaudRate = 9600,                                     // Baudrate (By Default: 9600)
    System.IO.Ports.Parity Parity = System.IO.Ports.Parity.None, // Parity: None
    int DataBits = 8,                                        // Data bit (By Default: 8)
    System.IO.Ports.StopBits StopBits = System.IO.Ports.StopBits.One, // Stop-bit: (By Default: 1)
    int ReadTimeoutMs = 3000,                                // Read Timeout in ms
    int WriteTimeoutMs = 3000                                // Write Timeout in ms
);