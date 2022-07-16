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

            Gif neomatrix = new Gif(64, 32, 300);
            neomatrix.loadData(net.GetGifData("matrix-spin.gif"));

            var matrix = new RGBLedMatrix(32, 2, 1);
            var canvas = matrix.CreateOffscreenCanvas();
            neomatrix.printFrame(canvas,0);
            canvas = matrix.SwapOnVsync(canvas);

            int leftpos = Console.CursorLeft;
            int toppos = Console.CursorTop;

            while (true) {
                neomatrix.playGif(canvas, 40);
                timer.Reset();
                timer.Start();
                neomatrix.printFrame(canvas, 0);
                canvas = matrix.SwapOnVsync(canvas);
                timer.Stop();
                TimeSpan timeTaken = timer.Elapsed;
                double fps = 1000 / timeTaken.TotalMilliseconds;
                Console.SetCursorPosition(leftpos, toppos);
                Console.Write("FPS:"+ fps);
            }
            
            

        }
    }

}