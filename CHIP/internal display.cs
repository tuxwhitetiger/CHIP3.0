using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Drawing;
using SixLabors.ImageSharp.Processing.Text;
using SixLabors.Primitives;
using Ssd130;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace CHIP
{
    internal class internal_display
    {
        /*
        public internal_display(Logger mylogger)
        {
            mylogger.Log("lcd test");
            mylogger.Log("create Oled");

            mylogger.Log("Creating test bitmap ...");
            try
            {

                new Bitmap(128, 128).Save("test.bmp");
            }
            catch (Exception ex)
            {
                mylogger.Log(ex.Message);
                mylogger.Log(ex.StackTrace);
                mylogger.Log(ex.InnerException.ToString());

            }
            mylogger.Log("... done!");
            UxSSD1306 _1306 = new UxSSD1306(mylogger);
            mylogger.Log("create complete");


            mylogger.Log("Initilize Oled");
            _1306.Initialize();
            mylogger.Log("Initilize complete");
            mylogger.Log("GetGraphics");
            Graphics display = _1306.GetGraphics();
            mylogger.Log("done");
            display.DrawRectangle(Pens.Black, 0, 0, _1306.DisplayWidth - 1, _1306.DisplayHeight - 1);
            display.DrawEllipse(Pens.Black, 10, 10, _1306.DisplayWidth - 20, _1306.DisplayHeight - 20);
            mylogger.Log("Draw Oled");
            _1306.Update(display);
            mylogger.Log("Wait 10 sec");
            Thread.Sleep(10000);
            display = _1306.GetGraphics();
            _1306.Update(display);
            mylogger.Log("Clear display");
            mylogger.Log("Wait 1 sec");
            Thread.Sleep(1000);

            
            mylogger.Log("lcd done");
        }

        */
        
        public internal_display(Logger mylogger) {
            using (var image = new Image<Rgba32>(128, 32))

            using (SSD130 Ssd130 = new SSD130())
            {
                mylogger.Log("Ssd130 Begin");
                Ssd130.Begin(1);
                mylogger.Log("Ssd130 Clear");
                Ssd130.Clear();
                mylogger.Log("Ssd130 Display");
                Ssd130.Display();
                mylogger.Log("List");
                List<string> fuentes = new List<string>(){ "DejaVu Sans Light", "DejaVu Sans Mono","Piboto Thin", "Noto Mono" };
                mylogger.Log("List replace");
                fuentes = new List<string>() { "Noto Mono" };
                mylogger.Log("List loop");
                foreach (var fonts in fuentes)
                {
                    mylogger.Log("set vars");
                    var fontSize = 25;
                    var time = DateTime.Now;
                    var font = SystemFonts.CreateFont(fonts, fontSize, FontStyle.Regular);
                    mylogger.Log("Mutate");
                    image.Mutate(ctx => ctx.Fill(Rgba32.Black).DrawText($"{time.Hour.ToString("D2")}:{time.Minute.ToString("D2")}:{time.Second.ToString("D2")}",font, Rgba32.White, new PointF(0, 0)));
                    mylogger.Log("Ssd130 Image");
                    Ssd130.Image(image);
                    mylogger.Log("Ssd130 Display");
                    Ssd130.Display();
                }
            }
        }
    
        
    }
}
