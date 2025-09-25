namespace TestBase.PowerSupply.Drivers
{
    public sealed partial class RigolDp712
    {
        // Measures output voltage via :MEASure:VOLTage?
        public double MeasureVoltage() => ParseInvariant(Query(":MEASure:VOLTage?"));
        // Measures output current via :MEASure:CURRent?
        public double MeasureCurrent() => ParseInvariant(Query(":MEASure:CURRent?"));
        // Measures output power via :MEASure:POWEr?
        public double MeasurePower()   => ParseInvariant(Query(":MEASure:POWEr?"));

        // Measures all three values (V,I,P) via :MEASure:ALL?
        public (double Voltage, double Current, double Power) MeasureAll()
        {
            var resp  = Query(":MEASure:ALL?");
            var parts = resp.Split(',');
            double v = ParseInvariant(parts[0]);
            double i = ParseInvariant(parts.Length > 1 ? parts[1] : "0");
            double p = ParseInvariant(parts.Length > 2 ? parts[2] : "0");
            return (v, i, p);
        }
    }
}