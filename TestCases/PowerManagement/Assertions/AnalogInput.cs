using NUnit.Framework;
using static TestBase.TestFunc.Rs485.Rs485Voltage;

namespace TestCases.PowerManagement.Assertions
{
    // Assertion helpers for analog input validation over RS-485.
    public static class AnalogInput
    {
        /* Measures averaged voltage for the channel (using configured sampling)
         and asserts it equals `expected` within `tolerance`.*/
        public static void AssertAnalogInputVoltage(
            int channel,
            double appliedVoltage,
            double expected,
            double tolerance)
        {
            var vIn = SampleAverageVoltsConfigured(channel);

            Assert.That(
                vIn,
                Is.EqualTo(expected).Within(tolerance),
                $"o{channel}: target={appliedVoltage:F3}V, expected={expected:F3}V ±{tolerance:F3}V, measured={vIn:F3}V");
        }
    }
}