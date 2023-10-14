using Iot.Device.CharacterLcd;
using Iot.Device.Mcp23xxx;
using Iot.Device.Ssd1351;
using Iot.Device.Ssd13xx;
using System.Device.Gpio;
using System.Device.I2c;

namespace CHIP
{
    internal class internal_display
    {
        I2cDevice i2cDevice;
        Ssd1306 serialDriver;
        Lcd1602 lcd;
        public internal_display(Logger mylogger)
        {
            mylogger.Log("internal display boot");
            mylogger.Log("i2cDevice create");
            i2cDevice = I2cDevice.Create(new I2cConnectionSettings(1, 0x20));
            mylogger.Log("i2cDevice done");
            mylogger.Log("serialDriver create");
            serialDriver = new Ssd1306(i2cDevice);
            mylogger.Log("serialDriver done");
            mylogger.Log("lcd create");
            lcd = new Lcd1602(dataPins: new int[] { 0, 1, 2, 3 },
                        registerSelectPin: 4,
                        readWritePin: 5,
                        enablePin: 6,
                        controller: new GpioController(PinNumberingScheme.Logical));
            mylogger.Log("lcd done");
            mylogger.Log("lcd test");
            testText();
            mylogger.Log("lcd test done");
        }

        private void testText() {
            lcd.Clear();
            lcd.SetCursorPosition(0, 0);
            lcd.Write($"OH Hi there");
        }
    }
}
