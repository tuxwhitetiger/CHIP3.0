using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using rpi_rgb_led_matrix_sharp;

namespace CHIP
{
    public class MainClass
    {
        
        public static void Main(string[] args)
        {
            var timer = new Stopwatch();

            mynetwork net = new mynetwork();
            net.connect();
            Console.WriteLine(net.getFace());
            RGBLedMatrixOptions options = new RGBLedMatrixOptions();
            options.Rows = 32;
            options.Cols = 64;
            options.ChainLength = 1;
            options.Parallel = 1;
            options.GpioSlowdown = 3;
            options.HardwareMapping = "regular";
            var matrix = new RGBLedMatrix(options);
            var canvas = matrix.CreateOffscreenCanvas();

            RGBLedFont font = new RGBLedFont("./fonts/7x13.bdf");
            canvas.DrawText(font, 7, 10, new Color(255, 255, 255), "Loading");
            canvas.DrawText(font, 7, 23, new Color(255, 255, 255), "Gifs");
            canvas = matrix.SwapOnVsync(canvas);

            Gif neomatrix = new Gif(64, 32, 300);
            neomatrix.loadData(net.GetGifData("matrix-spin.gif"));


            int leftpos = Console.CursorLeft;
            int toppos = Console.CursorTop;

            while (true) {
                timer.Reset();
                timer.Start();
                neomatrix.playGif(matrix,canvas, 40);
                
                timer.Stop();
                TimeSpan timeTaken = timer.Elapsed;
                double fps = (1000 / timeTaken.TotalMilliseconds)*neomatrix.newFrameCount;
                Console.SetCursorPosition(leftpos, toppos);
                Console.Write("FPS:"+ fps);
            }
            
            

        }
    }

}