namespace TestBase.PowerSupply.Interfaces
{
    public interface IPowerSupply
    {
        // Connection lifecycle
        void Open();    // Open port/connection
        void Close();   // Close
        
        // Identification
        string Idn();   // *IDN
        
        // Set points
        void SetVoltage(double volts);  // Set voltage
        void SetCurrent(double amps);   // Set current
        
        // Output control
        void SetOutput(bool on);    // Output On/Off
        bool GetOutputState();      // Get Output state (On/Off)
        
        // Measurement
        double MeasureVoltage();
        double MeasureCurrent();
        double MeasurePower();
        (double Voltage, double Current, double Power) MeasureAll();
    }
}