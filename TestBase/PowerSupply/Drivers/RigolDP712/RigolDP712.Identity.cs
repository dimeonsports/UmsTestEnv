namespace TestBase.PowerSupply.Drivers
{
    // Queries instrument identity string (*IDN?). Returns vendor, model, serial, firmware as a single CSV string.
    public sealed partial class RigolDp712
    {
        public string Idn() => Query("*IDN?");
    }
}