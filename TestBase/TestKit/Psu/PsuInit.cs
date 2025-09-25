using TestBase.PowerSupply.Interfaces;
using TestBase.PowerSupply.Models;
using TestBase.PowerSupply.Drivers;

namespace TestBase.TestKit.Psu
{
    // Power Supply (PSU) lifetime + access to the current driver instance.
    public static class PsuInit
    {
        private static IPowerSupply? _psu;

        // Current PSU (throws if InitPsuCom(...) wasn't called yet).
        public static IPowerSupply PSU =>
            _psu ?? throw new System.InvalidOperationException("PSU is not initialized. Call InitPsuCom(...) first.");
        
        // Initializes (idempotent) and opens PSU, then forces safe state (Output OFF).
        public static void InitPsuCom(string modelName, string portName, int readTimeoutMs, int writeTimeoutMs = 3000)
        {
            if (_psu is null)
            {
                var settings = new PortSettings(portName, ReadTimeoutMs: readTimeoutMs, WriteTimeoutMs: writeTimeoutMs);
                _psu = CreateDriver(modelName, settings);
                _psu.Open();
            }

            TryOutputOff(_psu);
            Thread.Sleep(1000);
        }
        
        // Full shutdown: Output OFF and dispose the driver. Safe to call multiple times.
        public static void ClosePsuCom()
        {
            var psu = _psu;
            if (psu is null) return;

            TryOutputOff(psu);
            (psu as System.IDisposable)?.Dispose(); // RigolDP712.Dispose() внутри сам закрывает порт

            _psu = null;
        }

        // Best-effort output OFF (swallows any exceptions on tear-down).
        private static void TryOutputOff(IPowerSupply psu)
        {
            try { psu.SetOutput(false); } catch { }
        }

        // Factory mapping from model name to concrete driver Type.
        private static IPowerSupply CreateDriver(string modelName, PortSettings settings) =>
            (modelName?.Trim()) switch
            {
                "RigolDP712" or nameof(RigolDp712) => new RigolDp712(settings),
                _ => throw new System.NotSupportedException($"Unknown power supply model '{modelName}'.")
            };
    }
}