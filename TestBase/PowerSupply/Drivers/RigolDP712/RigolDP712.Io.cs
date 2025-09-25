using System.Globalization;

namespace TestBase.PowerSupply.Drivers
{
    // Sends a single SCPI command (no response expected).
    public sealed partial class RigolDp712
    {
        private void Write(string cmd)
        {
            EnsureOpen();
            _port.WriteLine(cmd);
        }

        // Sends a query and reads a single line response.
        private string Query(string cmd)
        {
            EnsureOpen();
            _port.DiscardInBuffer(); // avoid mixing previous unread data with this response
            _port.WriteLine(cmd);
            var resp = _port.ReadLine();
            return resp.Trim();
        }

        // Parses a double using invariant culture. Throws a descriptive error on failure.
        private static double ParseInvariant(string s)
        {
            if (double.TryParse(s?.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var v))
                return v;
            throw new FormatException($"Cannot parse numeric response: '{s}'");
        }
    }
}