namespace TestBase.PowerSupply.Drivers
{
    public sealed partial class RigolDp712
    {
        // Turns output ON/OFF for channel 1.
        public void SetOutput(bool on)
            => Write($":OUTPut:STATe CH1,{(on ? "ON" : "OFF")}");

        // Reads output state. Accepts either textual "ON"/"OFF" or numeric "1"/"0".
        public bool GetOutputState()
        {
            var resp = Query(":OUTPut:STATe?");
            return resp.Equals("ON", StringComparison.OrdinalIgnoreCase) || resp == "1";
        }
    }
}