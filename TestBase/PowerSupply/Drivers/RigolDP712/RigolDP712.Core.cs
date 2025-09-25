using System.IO.Ports;
using System.Text;
using TestBase.PowerSupply.Interfaces;
using TestBase.PowerSupply.Models;

namespace TestBase.PowerSupply.Drivers  
{
    // SCPI (Standard Command for Programmable Instruments) driver for Rigol DP712 over a SerialPort.
    public sealed partial class RigolDp712 : IPowerSupply, IDisposable
    {
        private readonly SerialPort _port;

        // Creates a SerialPort configured from PortSettings.
        public RigolDp712(PortSettings set)
        {
            _port = new SerialPort(set.PortName, set.BaudRate, set.Parity, set.DataBits, set.StopBits)
            {
                NewLine = "\n",                     // SCPI replies are typically LF-terminated
                Encoding = Encoding.ASCII,          // DP712 is ASCII-based
                ReadTimeout = set.ReadTimeoutMs,
                WriteTimeout = set.WriteTimeoutMs,
                Handshake = Handshake.None
            };
        }

        // Opens the serial port if not already open.
        public void Open()
        {
            if (!_port.IsOpen) _port.Open();
            Thread.Sleep(50);
        }

        // Closes the serial port if it's open.
        public void Close()
        {
            if (_port.IsOpen) _port.Close();
        }

        // Indicates whether the port is currently open.
        public bool IsOpen => _port.IsOpen;

        // Throws if the port is not open. Call before any I/O.
        private void EnsureOpen()
        {
            if (!_port.IsOpen)
                throw new InvalidOperationException("Serial port is not open. Call Open() first.");
        }

        // Ensures a clean shutdown: attempts to close, then disposes the SerialPort.
        public void Dispose()
        {
            try { Close(); } catch { }
            _port.Dispose();
        }
    }
}