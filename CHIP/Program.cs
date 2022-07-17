using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            List<Gif> gifs = new List<Gif>();

            Gif happy = new Gif(64, 32, 300, "happy");
            happy.loadData(net.GetGifData("happy.gif"));
            Gif neomatrix = new Gif(64, 32, 300, "matrix");
            neomatrix.loadData(net.GetGifData("matrix-spin.gif"));
            Gif cwoods = new Gif(64, 32, 300, "CWOODSDEAN");
            cwoods.loadData(net.GetGifData("CWOODSDEAN-full.gif"));
            Gif flag = new Gif(64, 32, 300, "flag");
            flag.loadData(net.GetGifData("flag.gif"));
            Gif lowbatt = new Gif(64, 32, 300, "lowbatt");
            lowbatt.loadData(net.GetGifData("lowbatt.gif"));
            Gif overheat = new Gif(64, 32, 300, "overheat");
            overheat.loadData(net.GetGifData("overheat.gif"));
            Gif pacman = new Gif(64, 32, 300, "pacman");
            pacman.loadData(net.GetGifData("pacman.gif"));

            gifs.Add(pacman);
            gifs.Add(cwoods);
            gifs.Add(neomatrix);

            

            while (true) {
                switch (net.getFace()) {
                    case "Sad face": missingfile(matrix, canvas, font); break;
                    case "Happy face": happy.playGif(matrix, canvas, 40); break;
                    case "Angry face": missingfile(matrix, canvas, font); break;
                    case "What face": missingfile(matrix, canvas, font); break;
                    case "Flag face": flag.playGif(matrix, canvas, 40); break;
                    case "Oh face": missingfile(matrix, canvas, font); break;
                    case "Gif face": randomGiff(matrix, canvas, gifs);  break;
                    case "lowbatt face": lowbatt.playGif(matrix, canvas, 40); break;
                    case "overheat face": overheat.playGif(matrix, canvas, 40); break;
                    case "snake face": missingfile(matrix, canvas, font); break;
                    case "matrix face": neomatrix.playGif(matrix, canvas, 40); break;
                    case "pacman face": pacman.playGif(matrix, canvas, 40); break;
                    case "cwoods face": cwoods.playGif(matrix, canvas, 40); break;
                }
            }
        }

        private static void randomGiff(RGBLedMatrix matrix, RGBLedCanvas canvas, List<Gif> gifs)
        {
            Random rand = new Random();
            int pick = rand.Next(0, gifs.Count-1);
            gifs[pick].playGif(matrix, canvas, 40);
        }

        private static void missingfile(RGBLedMatrix matrix, RGBLedCanvas canvas, RGBLedFont font)
        {
            canvas.DrawText(font, 7, 10, new Color(255, 255, 255), "missing");
            canvas.DrawText(font, 7, 23, new Color(255, 255, 255), "Gif");
            canvas = matrix.SwapOnVsync(canvas);
        }
    }

}