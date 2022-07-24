using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using rpi_rgb_led_matrix_sharp;
using System.Diagnostics;

namespace CHIP
{
    enum faces
    {
        happy,
        sad,
        cwood,
        pacman,
        snake,
        matrix,
        lowbatt,
        Angry,
        What,
        Flag,
        Overheat,
        Gif,
        Oh
    }

    class Face
    {
        mynetwork net;
        controller_network cnet;
        List<Controller> controllerstokill;
        snake snakegame;
        bool playingsnake = false;
        RGBLedFont font;

        RGBLedMatrixOptions options = new RGBLedMatrixOptions();
        RGBLedMatrix matrix;
        RGBLedCanvas canvas;
        Stopwatch timer;

        faces runningface = faces.happy;

        //setup gif faces
        List<Gif> gifs = new List<Gif>();
        Gif happy;
        Gif neomatrix;
        Gif cwoods;
        Gif flag;
        Gif lowbatt;
        Gif overheat;
        Gif pacman;


        public void load()
        {
            net = new mynetwork();
            cnet = new controller_network();
            controllerstokill = new List<Controller>();
            net.connect();
            options.Rows = 32;
            options.Cols = 64;
            options.ChainLength = 1;
            options.Parallel = 1;
            options.GpioSlowdown = 3;
            options.HardwareMapping = "regular";
            matrix = new RGBLedMatrix(options);
            canvas = matrix.CreateOffscreenCanvas();
            Console.WriteLine(net.getFace());
            snakegame = new snake();
            font = new RGBLedFont("./fonts/7x13.bdf");
            canvas.DrawText(font, 7, 10, new Color(255, 255, 255), "Loading");
            canvas.DrawText(font, 7, 23, new Color(255, 255, 255), "Gifs");
            canvas = matrix.SwapOnVsync(canvas);


            happy = new Gif("happy");
            happy.loadData(net.GetGifData("happy.gif"), 40);
            neomatrix = new Gif("matrix");
            neomatrix.loadData(net.GetGifData("matrix-spin.gif"),40);
            cwoods = new Gif("CWOODSDEAN");
            cwoods.loadData(net.GetGifData("CWOODSDEAN-full.gif"),1000);
            flag = new Gif("flag");
            flag.loadData(net.GetGifData("flag.gif"),40);
            lowbatt = new Gif("lowbatt");
            lowbatt.loadData(net.GetGifData("lowbatt.gif"),40);
            overheat = new Gif("overheat");
            overheat.loadData(net.GetGifData("overheat.gif"),40);
            pacman = new Gif("pacman");
            pacman.loadData(net.GetGifData("pacman.gif"),40);
            
            gifs.Add(cwoods);
            gifs.Add(neomatrix);
            gifs.Add(pacman);
            timer = new Stopwatch();
            
        }
        public void update() {
            timer.Start();
            canvas.Clear();
            while (true)
            {
                if (timer.ElapsedMilliseconds > 1000)
                {
                    timer.Restart();
                    switch (net.getFace()) // need to make this async
                    {
                        case "Sad face": playingsnake = false; runningface = faces.sad; break;
                        case "Happy face": playingsnake = false; runningface = faces.happy; break;
                        case "Angry face": playingsnake = false; runningface = faces.Angry;  break;
                        case "What face": playingsnake = false; runningface = faces.What;  break;
                        case "Flag face":   playingsnake = false; runningface = faces.Flag;  break;
                        case "Gif face": playingsnake = false; runningface = faces.Gif; break;
                        case "Oh face": playingsnake = false; runningface = faces.Oh; break;
                        case "Snake face": playingsnake = true; runningface = faces.snake; break;
                        case "Overheat face": playingsnake = false; runningface = faces.Overheat;  break;
                        case "Cwood face":  playingsnake = false; runningface = faces.cwood;  break;
                        case "Lowbatt face": playingsnake = false; runningface = faces.lowbatt;  break;
                        case "Pacman face": playingsnake = false; runningface = faces.pacman;  break;
                        case "Matrix face": playingsnake = false; runningface = faces.matrix;  break;
                    }
                }
                switch (runningface) {
                    case faces.Angry: missingfile(); break;
                    case faces.cwood: cwoodTick(); break;
                    case faces.Flag: FlagTick(); break;
                    case faces.Gif: randomGiff(); break;
                    case faces.happy: happyTick(); break;
                    case faces.lowbatt: lowbattTick(); break;
                    case faces.matrix: matrixTick(); break;
                    case faces.Oh: missingfile(); break;
                    case faces.Overheat: overheatTick(); break;
                    case faces.pacman: pacmanTick(); break;
                    case faces.sad: missingfile(); break;
                    case faces.snake: snakeTick(); break;
                    case faces.What: missingfile(); break;
                }
            }
        }

        private void happyTick() {
            happy.playGif(matrix, canvas);
        }
        private void snakeTick()
        {
            if (!snakegame.running)
            {
                cnet.controllers = new List<Controller>();
                snakegame.startNewGame();
                snakegame.running = true;
            }
            canvas.Clear();
            foreach (Controller c in cnet.controllers)
            {
                if (c.killme) { controllerstokill.Add(c); }
            }
            if (cnet.controllers.Count == 0)
            {
                canvas.DrawText(font, 7, 10, new Color(255, 255, 255), "connect");
                canvas.DrawText(font, 0, 20, new Color(255, 255, 255), "controller");
                canvas = matrix.SwapOnVsync(canvas);
            }
            else
            {
                snakegame.update(cnet.controllers.Last());
                snakegame.printframe(matrix, canvas);
            }
            foreach (Controller c in controllerstokill)
            {
                cnet.controllers.Remove(c);
            }
        }
        private void randomGiff()
        {
            Random rand = new Random();
            int pick = rand.Next(0, gifs.Count);
            gifs[pick].playGif(matrix, canvas);
        }
        private void FlagTick() {
            flag.playGif(matrix, canvas);
        }
        private void overheatTick() {
            overheat.playGif(matrix, canvas);
        }
        private void cwoodTick() {
            cwoods.playGif(matrix, canvas);
        }
        private void lowbattTick() {
            lowbatt.playGif(matrix, canvas);
        }
        private void matrixTick() {
            neomatrix.playGif(matrix, canvas);
        }
        private void pacmanTick() {
            pacman.playGif(matrix, canvas);
        }

        private void missingfile()
        {
            Thread.Sleep(40);
            canvas.Fill(new Color(0, 0, 0));
            canvas.DrawText(font, 7, 10, new Color(255, 255, 255), "missing");
            canvas.DrawText(font, 7, 23, new Color(255, 255, 255), "Gif");
            canvas = matrix.SwapOnVsync(canvas);
        }

    }
}
