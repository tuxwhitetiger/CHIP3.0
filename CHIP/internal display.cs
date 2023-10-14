using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Drawing;
using SixLabors.ImageSharp.Processing.Overlays;
using SixLabors.ImageSharp.Processing.Text;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.Threading;
using Ssd130;
using Iot.Device.Max7219;


namespace CHIP
{
    internal class internal_display
    {
        
        public internal_display(Logger mylogger)
        {
            mylogger.Log("lcd test");
            start(mylogger);
            mylogger.Log("lcd done");
        }

        private void start(Logger mylogger) {
            using (var image = new Image<Rgba32>(128, 32))

            using (SSD130 Ssd130 = new SSD130())
            {
                mylogger.Log("Ssd130 Begin");
                Ssd130.Begin();
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
