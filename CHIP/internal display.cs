using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Drawing;
using SixLabors.ImageSharp.Processing.Text;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace CHIP
{
    internal class internal_display
    {
        Image<Rgba32> image = new Image<Rgba32>(128, 32);
        SSD130 Ssd130 = new SSD130();
        Font font = SystemFonts.CreateFont("Noto Mono", 16, FontStyle.Regular);
        Logger mylogger;

        public internal_display(Logger mylogger) {
            this.mylogger = mylogger;
            mylogger.Log("Ssd130 Begin");
            Ssd130.Begin(mylogger,1);
            mylogger.Log("Ssd130 Clear");
            Ssd130.Clear();
            mylogger.Log("Ssd130 Display");
            Ssd130.Display();
            mylogger.Log("List");
            PrintTime();
        }

        public void PrintTime() {
            var time = DateTime.Now;
            mylogger.Log("Mutate");
            image.Mutate(ctx => ctx.Fill(Rgba32.Black).DrawText($"{time.Hour.ToString("D2")}:{time.Minute.ToString("D2")}:{time.Second.ToString("D2")}", font, Rgba32.White, new PointF(16, 0)));
            mylogger.Log("Ssd130 Image");
            Ssd130.Image(image);
            mylogger.Log("Ssd130 Display");
            Ssd130.Display();
        }

        public void printText(String text) {
            image.Mutate(ctx => ctx.Fill(Rgba32.Black).DrawText(text, font, Rgba32.White, new PointF(0, 0)));
            Ssd130.Image(image);
            Ssd130.Display();
        }

        internal void update(string face)
        {
            PrintTime();
            printText(face);
        }
    }
}
