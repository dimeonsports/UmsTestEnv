using System.IO.Ports;

namespace TestBase.Protocols.Rs485
{
    // Single-instance lifecycle manager for the RS-485 reader.
    public static class Rs485OnOff
    {
        private static Rs485Core? _reader;
        
        // Current RS-485 reader instance (throws if not initialized).
        public static Rs485Core Reader => _reader ?? throw new InvalidOperationException(
            "RS-485 is not connected. Call InitUmsRS485Com(...) first.");
        
        // True if the reader has been initialized and not closed yet.
        public static bool IsConnected => _reader is not null;

        // Initializes RS-485 reader on the given COM port.
        public static void InitUmsRS485Com(
            string portName,
            int baud = 38400,
            Parity parity = Parity.None,
            int dataBits = 8,
            StopBits stopBits = StopBits.One,
            int readTimeoutMs = 200)
        {
            if (_reader is not null) CloseUmsRS485Com(); // ensure clean re-init

            _reader = new Rs485Core(
                portName: portName,
                baud: baud,
                parity: parity,
                dataBits: dataBits,
                stopBits: stopBits,
                readTimeoutMs: readTimeoutMs);
        }

        // Closes and disposes the current RS-485 reader (if any).
        public static void CloseUmsRS485Com()
        {
            var r = _reader;
            if (r is null) return;
            try { r.Dispose(); } catch { }
            _reader = null;
        }
    }
}
