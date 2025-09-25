using System.Collections.Concurrent;
using System.IO.Ports;
using System.Text;
using System.Text.RegularExpressions;

namespace TestBase.Protocols.Rs485
{
    /* 
        Low-level RS-485 reader:
        Opens SerialPort and spawns a background thread.
        - Parses lines like "o1: 1990, o2: 5".
        - Keeps last RAW value per channel in-memory.
    */
    public sealed class Rs485Core : IDisposable
    {
        private readonly SerialPort _port;
        private readonly CancellationTokenSource _cts = new();
        private readonly Thread _thread;
        private readonly ConcurrentDictionary<int, int> _lastRaw = new();

        private static readonly Regex Re = new(@"o(?<ch>\d+):\s*(?<val>-?\d+)", RegexOptions.Compiled);
        private const int IdleReadMs = 5;       // small pause when nothing to read
        private const int OnErrorPauseMs = 20;  // backoff on unexpected read errors

        
        // Opens the port and starts a background reader thread immediately.
        public Rs485Core(
            string portName,
            int baud = 115200,
            Parity parity = Parity.None,
            int dataBits = 8,
            StopBits stopBits = StopBits.One,
            int readTimeoutMs = 200)
        {
            _port = new SerialPort(portName, baud, parity, dataBits, stopBits)
            {
                NewLine = "\n",
                ReadTimeout = readTimeoutMs,
                Encoding = Encoding.ASCII
            };
            _port.Open();

            _thread = new Thread(ReadLoop) { IsBackground = true, Name = "RS485-Reader" };
            _thread.Start();
        }

        // Tries to get the last known RAW value for channel o{channel}.
        public bool TryGetLastRaw(int channel, out int value) => _lastRaw.TryGetValue(channel, out value);
        
        // Returns the last known RAW value or null if nothing has been received yet.
        public int? GetLastRawOrNull(int channel) => _lastRaw.TryGetValue(channel, out var v) ? v : (int?)null;
        
        /// <summary>
        /// Background loop: collects text from the port and splits by '\n',
        /// parsing any "oX: value" pairs it finds per line.
        /// Swallows TimeoutException and briefly sleeps on other errors to stay alive.
        /// </summary>
        private void ReadLoop()
        {
            var ct = _cts.Token;
            var sb = new StringBuilder(1024);

            while (!ct.IsCancellationRequested)
            {
                try
                {
                    var chunk = _port.ReadExisting(); // non-blocking; may be empty
                    if (string.IsNullOrEmpty(chunk))
                    {
                        Thread.Sleep(IdleReadMs);
                        continue;
                    }

                    sb.Append(chunk);
                    while (true)
                    {
                        var text = sb.ToString();
                        int nl = text.IndexOf('\n');
                        if (nl < 0) break;

                        var line = text[..nl].TrimEnd('\r');
                        sb.Remove(0, nl + 1);

                        foreach (Match m in Re.Matches(line))
                        {
                            if (int.TryParse(m.Groups["ch"].Value, out var ch) &&
                                int.TryParse(m.Groups["val"].Value, out var val))
                            {
                                _lastRaw[ch] = val;
                            }
                        }
                    }
                }
                catch (TimeoutException) { /* ignore: normal on idle */ }
                catch
                {
                    Thread.Sleep(OnErrorPauseMs); // // backoff to avoid busy-error loop
                }
            }
        }

        // Graceful shutdown: cancel loop, wait a bit, close and dispose everything.
        public void Dispose()
        {
            try { _cts.Cancel(); } catch { }
            try { _thread.Join(200); } catch { }
            try { _port.Close(); } catch { }
            _port.Dispose();
            _cts.Dispose();
        }
    }
}