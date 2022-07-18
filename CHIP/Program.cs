using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using rpi_rgb_led_matrix_sharp;

namespace CHIP
{
    public class MainClass
    {
        
        public static void Main(string[] args)
        {
            mynetwork net = new mynetwork();
            controller_network cnet = new controller_network();
            List<Controller> controllerstokill = new List<Controller>();
            snake snakegame = new snake();
            bool playingsnake = false;
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

            Gif happy = new Gif( "happy");
            happy.loadData(net.GetGifData("happy.gif"),40);
            /*
            Gif neomatrix = new Gif("matrix");
            neomatrix.loadData(net.GetGifData("matrix-spin.gif"),40);
            Gif cwoods = new Gif("CWOODSDEAN");
            cwoods.loadData(net.GetGifData("CWOODSDEAN-full.gif"),1000);
            Gif flag = new Gif("flag");
            flag.loadData(net.GetGifData("flag.gif"),40);
            Gif lowbatt = new Gif("lowbatt");
            lowbatt.loadData(net.GetGifData("lowbatt.gif"),40);
            Gif overheat = new Gif("overheat");
            overheat.loadData(net.GetGifData("overheat.gif"),40);
            Gif pacman = new Gif("pacman");
            pacman.loadData(net.GetGifData("pacman.gif"),40);
            
            
            gifs.Add(cwoods);
            gifs.Add(neomatrix);
            gifs.Add(pacman);

            */

            while (true) {
                
                switch (net.getFace()) {
                    case "Sad face":    playingsnake = false; missingfile(matrix, canvas, font); break;
                    case "Happy face":  playingsnake = false; happy.playGif(matrix, canvas); break;
                    case "Angry face":  playingsnake = false; missingfile(matrix, canvas, font); break;
                    case "What face":   playingsnake = false; missingfile(matrix, canvas, font); break;
//                    case "Flag face":   playingsnake = false; flag.playGif(matrix, canvas); break;
                    case "Gif face":    playingsnake = false; randomGiff(matrix, canvas, gifs);  break;
                    case "Oh face":     playingsnake = false; missingfile(matrix, canvas, font); break;
                    case "Snake face":  playingsnake = true; break;
//                    case "Overheat face": playingsnake = false; overheat.playGif(matrix, canvas); break;
//                    case "Cwood face":  playingsnake = false; cwoods.playGif(matrix, canvas); break;
//                    case "Lowbatt face": playingsnake = false; lowbatt.playGif(matrix, canvas); break;
//                    case "Pacman face": playingsnake = false; pacman.playGif(matrix, canvas); break;
//                    case "Matrix face": playingsnake = false; neomatrix.playGif(matrix, canvas); break;
                    
                }
                if (playingsnake)
                {
                    Console.WriteLine("controllers:" + cnet.controllers.Count);
                    foreach (Controller c in cnet.controllers)
                    {
                        if (c.killme)
                        {
                            controllerstokill.Add(c);
                        }
                        Console.WriteLine("player:" + c.player);
                        c.getupdate();
                    }
                    if (cnet.controllers.Count == 0)
                    {
                        canvas.DrawText(font, 7, 23, new Color(255, 255, 255), "connect controller");
                        canvas = matrix.SwapOnVsync(canvas);
                    }
                    else
                    {
                        Console.WriteLine("update with controller:" + cnet.controllers[0].player);
                        snakegame.update(cnet.controllers[0]);
                        snakegame.printframe(matrix, canvas);
                    }
                    foreach (Controller c in controllerstokill)
                    {
                        cnet.controllers.Remove(c);
                    }
                }
            }
        }

        private static void randomGiff(RGBLedMatrix matrix, RGBLedCanvas canvas, List<Gif> gifs)
        {
            Random rand = new Random();
            int pick = rand.Next(0, gifs.Count);
            gifs[pick].playGif(matrix, canvas);
        }

        private static void missingfile(RGBLedMatrix matrix, RGBLedCanvas canvas, RGBLedFont font)
        {
            Thread.Sleep(40);
            canvas.Fill(new Color(0, 0, 0));
            canvas.DrawText(font, 7, 10, new Color(255, 255, 255), "missing");
            canvas.DrawText(font, 7, 23, new Color(255, 255, 255), "Gif");
            canvas = matrix.SwapOnVsync(canvas);
        }
    }

}