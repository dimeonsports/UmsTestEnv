using NUnit.Framework;
using Allure.NUnit;
using Allure.NUnit.Attributes;

using static TestBase.TestKit.Psu.PsuInit;
using static TestBase.TestKit.Psu.PsuMain;
using static TestBase.Protocols.Rs485.Rs485OnOff;
using static TestBase.TestFunc.Rs485.Rs485Voltage;
using static TestCases.PowerManagement.Assertions.AnalogInput;

namespace TestCases.PowerManagement
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("Power Management: UMS Analog Input O1 Voltage random drop")]
    [AllureFeature("UMS Tests")]
    [AllureOwner("Aleksei Birjukov")]
    [NonParallelizable]
    public class TсPm0003
    {
        [OneTimeSetUp]
        [AllureBefore("Rig setup: PSU & RS-485 init")]
        public void OneTimeSetUp()
        {
            InitPsuCom(
                "RigolDP712", 
                "COM5", 
                5000);
            
            InitUmsRS485Com(
                "COM4", 
                38400);
            
            SetPsuCurrentLimit(0.25);
            SetPsuVoltage(0.00);
            SetPsuOutputOn();
            
            SetAverageVoltSamples(
                samples: 15,
                msBetween: 100,
                timeoutMs: 5000,
                unitsPerVolt: 1000.0);
        }

        [TestCase(10.00, 0.15, TestName = "Analog Input 1: Set and validate 10.0V")]
        [TestCase(1.00, 0.15, TestName = "Analog Input 1: Set and validate 1.0V")]
        [TestCase(5.00, 0.15, TestName = "Analog Input 1: Set and validate 5.0V")]
        [TestCase(2.00, 0.15, TestName = "Analog Input 1: Set and validate 2.0V")]
        [TestCase(9.00, 0.15, TestName = "Analog Input 1: Set and validate 9.0V")]
        [TestCase(3.00, 0.15, TestName = "Analog Input 1: Set and validate 3.0V")]
        [TestCase(8.00, 0.15, TestName = "Analog Input 1: Set and validate 8.0V")]
        [TestCase(4.00, 0.15, TestName = "Analog Input 1: Set and validate 4.0V")]
        [TestCase(7.00, 0.15, TestName = "Analog Input 1: Set and validate 7.0V")]
        [TestCase(0.00, 0.15, TestName = "Analog Input 1: Set and validate 0.0V")]
        [TestCase(10.00, 0.15, TestName = "Analog Input 1: Set and validate 10.0V")]
        [TestCase(0.50, 0.15, TestName = "Analog Input 1: Set and validate 0.5V")]
        [TestCase(9.00, 0.15, TestName = "Analog Input 1: Set and validate 9.0V")]
        [TestCase(0.00, 0.15, TestName = "Analog Input 1: Set and validate 0.0V")]
        public void AnalogInput_O1_StepUp(double targetV, double tolV)
        {
            SetPsuVoltage(targetV);
            
            AssertAnalogInputVoltage(
                1, 
                targetV,
                targetV,
                tolV);
        }

        [OneTimeTearDown]
        [AllureAfter("Rig teardown: PSU & RS-485 close")]
        public void OneTimeTearDown()
        {
            SetPsuVoltage(0.00);
            SetPsuOutputOff();
            ClosePsuCom();
            CloseUmsRS485Com();
        }
    }
}