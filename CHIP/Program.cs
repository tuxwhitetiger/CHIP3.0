using System;
using System.Runtime.InteropServices;
using rpi_rgb_led_matrix_sharp;

namespace CHIP
{
    public class MainClass
    {
        
        public static void Main(string[] args)
        {
            

            mynetwork net = new mynetwork();
            net.connect();
            Console.WriteLine(net.getFace());

            Gif neomatrix = new Gif(64, 32, 100);
            neomatrix.loadData(net.GetGifData("matrix-spin.gif"));

            var matrix = new RGBLedMatrix(32, 2, 1);
            var canvas = matrix.CreateOffscreenCanvas();
            neomatrix.printFrame(canvas,0);
            canvas = matrix.SwapOnVsync(canvas);

            while (true) {
                neomatrix.printFrame(canvas, 0);
                canvas = matrix.SwapOnVsync(canvas);
            }
            
            

        }
    }

}