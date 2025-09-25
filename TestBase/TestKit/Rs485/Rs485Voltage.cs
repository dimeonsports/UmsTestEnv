using System.Diagnostics;
using TestBase.Protocols.Rs485;

namespace TestBase.TestFunc.Rs485
{
    // Averaged voltage acquisition over RS-485 using last-known RAW samples.
    public static class Rs485Voltage
    {
        private static int _samples = 10;
        private static int _msBetween = 100;
        private static int _timeoutMs = 5000;
        private static double _unitsPerV = 1000.0;

        private const int WaitNoDataMs = 10;
        
        // Sets default averaging config (call once in SetUp/OneTimeSetUp).
        public static void SetAverageVoltSamples(
            int samples = 10,
            int msBetween = 100,
            int timeoutMs = 5000,
            double unitsPerVolt = 1000.0)
        {
            if (samples <= 0) throw new ArgumentOutOfRangeException(nameof(samples));
            if (msBetween < 0) throw new ArgumentOutOfRangeException(nameof(msBetween));
            if (timeoutMs <= 0) throw new ArgumentOutOfRangeException(nameof(timeoutMs));
            if (unitsPerVolt <= 0) throw new ArgumentOutOfRangeException(nameof(unitsPerVolt));

            _samples = samples;
            _msBetween = msBetween;
            _timeoutMs = timeoutMs;
            _unitsPerV = unitsPerVolt;
        }
        
        // Takes an average using the previously configured parameters.
        public static double SampleAverageVoltsConfigured(int channel) =>
            SampleAverageVolts(channel, _samples, _msBetween, _timeoutMs, _unitsPerV);

        // Averages the last-known RAW values (converted to volts) for the given channel.
        public static double SampleAverageVolts(
            int channel,
            int samples,
            int msBetween,
            int timeoutMs,
            double unitsPerVolt)
        {
            if (samples <= 0) throw new ArgumentOutOfRangeException(nameof(samples));
            if (msBetween < 0) throw new ArgumentOutOfRangeException(nameof(msBetween));
            if (timeoutMs <= 0) throw new ArgumentOutOfRangeException(nameof(timeoutMs));
            if (unitsPerVolt <= 0) throw new ArgumentOutOfRangeException(nameof(unitsPerVolt));

            var sw = Stopwatch.StartNew();
            double sum = 0;
            int n = 0;

            while (n < samples)
            {
                if (sw.ElapsedMilliseconds > timeoutMs)
                    throw new TimeoutException($"Timeout sampling o{channel}");

                if (Rs485OnOff.Reader.TryGetLastRaw(channel, out var raw))
                {
                    sum += raw / unitsPerVolt;
                    n++;
                    Thread.Sleep(msBetween);
                    continue;
                }

                Thread.Sleep(WaitNoDataMs); // wait for first/next data to appear
            }

            return sum / n;
        }
    }
}