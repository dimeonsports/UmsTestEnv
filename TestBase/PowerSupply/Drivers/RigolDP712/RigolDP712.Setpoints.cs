using System.Globalization;

namespace TestBase.PowerSupply.Drivers
{
    public sealed partial class RigolDp712
    {
        // Sets the output voltage setpoint (channel 1) using SCPI immediate amplitude Path.
        public void SetVoltage(double volts)
        {
            EnsureOpen();
            var v = volts.ToString("G", CultureInfo.InvariantCulture);
            Write($":SOURce:VOLTage:LEVel:IMMediate:AMPLitude {v}");
        }

        // Sets the current limit setpoint (channel 1) using SCPI immediate amplitude Path.
        public void SetCurrent(double amps)
        {
            EnsureOpen();
            var a = amps.ToString("G", CultureInfo.InvariantCulture);
            Write($":SOURce:CURRent:LEVel:IMMediate:AMPLitude {a}");
        }
    }
}