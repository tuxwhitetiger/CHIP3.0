using Iot.Device.CharacterLcd;
using Iot.Device.Mcp23xxx;
using Iot.Device.Ssd1351;
using Iot.Device.Ssd13xx;
using System.Device;
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
            scani2c(0, mylogger);
            scani2c(1, mylogger);
            scani2c(2, mylogger);
            mylogger.Log("internal display boot");
            mylogger.Log("i2cDevice create");
            i2cDevice = I2cDevice.Create(new I2cConnectionSettings(1, 0x10));
            mylogger.Log("i2cDevice done");
            mylogger.Log("QueryComponentInformation");
            ComponentInformation CI = i2cDevice.QueryComponentInformation();
            mylogger.Log("QueryComponentInformation done");
            mylogger.Log("Name:"+CI.Name);
            mylogger.Log("Description:" + CI.Description);
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
        private void scani2c(int x,Logger mylogger) {
            for (int i=0x00; i<0xff;i++) {
                mylogger.Log("testing interface:"+ x +":"+ i);
                i2cDevice = I2cDevice.Create(new I2cConnectionSettings(x, i));
                ComponentInformation CI = i2cDevice.QueryComponentInformation();
                if (!CI.Description.Equals("Unix I2C device"))
                {
                    mylogger.Log("Name:" + CI.Name);
                    mylogger.Log("Description:" + CI.Description);
                }
            }
        }
        private void testText() {
            lcd.Clear();
            lcd.SetCursorPosition(0, 0);
            lcd.Write($"OH Hi there");
        }
    }
}
