namespace TestBase.TestKit.Psu
{
    // High-level PSU actions with built-in settle delays.
    public static class PsuMain
    {
        private const int DelayMs = 1200; // delay after PSU set

        // Sets the current limit and waits for the instrument to settle.
        public static void SetPsuCurrentLimit(double amps)
        {
            PsuInit.PSU.SetCurrent(amps);
            Thread.Sleep(DelayMs);
        }

        // Sets output voltage and waits for the instrument to settle.
        public static void SetPsuVoltage(double volts)
        {
            PsuInit.PSU.SetVoltage(volts);
            Thread.Sleep(DelayMs);
        }

        // Turns PSU output ON and waits a moment for stabilization.
        public static void SetPsuOutputOn()
        {
            PsuInit.PSU.SetOutput(true);
            Thread.Sleep(DelayMs);
        }

        // Turns PSU output OFF and waits a moment for stabilization.
        public static void SetPsuOutputOff()
        {
            PsuInit.PSU.SetOutput(false);
            Thread.Sleep(DelayMs);
        }
    }
}