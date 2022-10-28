﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using rpi_rgb_led_matrix_sharp;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CHIP
{
    class Face
    {
        mynetwork net;
        controller_network cnet;
        List<Controller> controllerstokill;


        snake snakegame;
        magic8Ball eightball = new magic8Ball();

        textSpam Tspam = new textSpam();

        RGBLedFont font;

        RGBLedMatrixOptions options = new RGBLedMatrixOptions();
        
        RGBLedMatrix matrix;
        RGBLedCanvas canvas;
        Stopwatch timer;

        faces runningface = faces.happy;

        //setup gif faces
        IDictionary<string, Gif> allGifs = new Dictionary<string, Gif>();


        public void load()
        {
            net = new mynetwork();
            cnet = new controller_network();
            controllerstokill = new List<Controller>();
            net.connect();
            options.Rows = 32;
            options.Cols = 128;
            options.ChainLength = 1;
            options.Parallel = 1;
            options.GpioSlowdown = 3;
            options.HardwareMapping = "regular";
            matrix = new RGBLedMatrix(options);
            canvas = matrix.CreateOffscreenCanvas();
            Console.WriteLine(net.getFace());
            snakegame = new snake();
            font = new RGBLedFont("./fonts/7x13.bdf");

            canvas.Clear();
            Tspam.PrintTextBothSides("Loading", 7, 10, new Color(255, 255, 255), canvas, matrix, font);
            Tspam.PrintTextBothSides("Gifs", 7, 23, new Color(255, 255, 255), canvas, matrix, font);
            canvas = matrix.SwapOnVsync(canvas);

            // check if i have serialized data if not ask python to do it
            DirectoryInfo facesFolder = new DirectoryInfo("./faces");
            FileInfo[] faces = facesFolder.GetFiles("*.gif");
            DirectoryInfo serialDataFolder = new DirectoryInfo("./serial");
            FileInfo[] serialfaces = serialDataFolder.GetFiles("*.serial");
            List<FileInfo> toserialize = new List<FileInfo>();

            //remove from faces list if in serialized list
            foreach (FileInfo fi in faces) {
                Console.WriteLine(fi.Name);
                bool removed = false;
                foreach (FileInfo fi2 in serialfaces) {
                    Console.WriteLine(fi2.Name);
                    if (fi.Name.Contains(fi2.Name.Trim().Substring(0, fi2.Name.Trim().Length - 7))){
                        //do nothing its a match
                        removed = true;
                    }
                }
                if (!removed) {
                    toserialize.Add(fi);
                }
            }


            //load from serial serialfaces
            foreach (FileInfo fi in serialfaces)
            {
                Console.WriteLine("deserialising :" + fi.Name);
                BinaryFormatter formatter = new BinaryFormatter();
                String source = "./serial/";
                source += fi.Name;
                Stream reader = new FileStream(source, FileMode.Open, FileAccess.Read);
                Gif g = new Gif();
                g.data = (GifData)formatter.Deserialize(reader);

                allGifs.Add(g.data.name, g);
            }

            //load from python
            foreach (FileInfo fi in toserialize)
            {
                Console.WriteLine("loading :" + fi.Name);
                Gif g = new Gif(fi.Name.Split('.')[0]);
                g.loadData(net.GetGifData(fi.Name), 40);
                //now its loaded need to serialize and save for next time
                BinaryFormatter formatter = new BinaryFormatter();
                String destination = "./serial/";
                destination += fi.Name.Trim().Substring(0, fi.Name.Trim().Length - 4);
                destination += ".serial";
                Stream writer = new FileStream(destination, FileMode.Create, FileAccess.Write);
                Console.WriteLine("writing :" + fi.Name);
                formatter.Serialize(writer, g.data);
                writer.Close();
                allGifs.Add(g.data.name, g);
            }
            

            timer = new Stopwatch();
        }
        public void update() {
            Console.WriteLine("dictonery count:" + allGifs.Count.ToString());
            Console.WriteLine("dictionary:");
            foreach (String s in allGifs.Keys)
            {
                Console.WriteLine(s);
            }
            timer.Start();
            canvas.Clear();
            while (true)
            {
                if (timer.ElapsedMilliseconds > 1000)
                {
                    timer.Restart();
                    switch (net.getFace()) // need to make this async
                    {
                        case "Sad face": snakegame.running = false; runningface = faces.sad; break;
                        case "Happy face": snakegame.running = false; runningface = faces.happy; break;
                        case "Angry face": snakegame.running = false; runningface = faces.Angry;  break;
                        case "What face": snakegame.running = false; runningface = faces.What; Tspam.start(canvas, matrix); break;
                        case "Flag face": snakegame.running = false; runningface = faces.Flag;  break;
                        case "Gif face": snakegame.running = false; runningface = faces.Gif; break;
                        case "Oh face": snakegame.running = false; runningface = faces.Oh; break;
                        case "Snake face": runningface = faces.snake; break;
                        case "Overheat face": snakegame.running = false; runningface = faces.Overheat;  break;
                        case "Cwood face": snakegame.running = false; runningface = faces.cwood;  break;
                        case "Lowbatt face": snakegame.running = false; runningface = faces.lowbatt;  break;
                        case "Pacman face": snakegame.running = false; runningface = faces.pacman;  break;
                        case "Matrix face": snakegame.running = false; runningface = faces.matrix; break;
                        case "8 Ball Face": snakegame.running = false; runningface = faces.eightball; break;
                        case "SHAKE BALL": snakegame.running = false; runningface = faces.eightball; eightball.shake = true; break;
                    }
                }
                switch (runningface) {
                    case faces.Angry: AngryTick(); break;
                    case faces.cwood: cwoodTick(); break;
                    case faces.Flag: FlagTick(); break;
                    case faces.Gif: randomGiff(); break;
                    case faces.happy: happyTick(); break;
                    case faces.lowbatt: lowbattTick(); break;
                    case faces.matrix: matrixTick(); break;
                    case faces.Oh: OhTick(); break;
                    case faces.Overheat: overheatTick(); break;
                    case faces.pacman: pacmanTick(); break;
                    case faces.sad: sadTick(); break;
                    case faces.snake: snakeTick(); break;
                    case faces.What: WhatTick(); break;
                    case faces.eightball: eightballTick(); break;
                    case faces.Halloween: HalloweenTick(); break;
                }
            }
        }
        private void sadTick() {
            allGifs["Sad-Face"].playGif(matrix, canvas);
        }
        private void OhTick() {
            allGifs["Shocked-Face"].playGif(matrix, canvas);
        }
        private void AngryTick()
        {
            allGifs["Angry-Face"].playGif(matrix, canvas);
        }
        private void HalloweenTick()
        {
            allGifs["Halloween-Face"].playGif(matrix, canvas);
        }
        public void amungusImposterFace() //you are the imposter
        {

        }
        public void amungussusKillFace() //imposter killing 
        { 

        }
        public void amunguscrewmateFace() // you are the crewmate
        {

        }
        public void WhatTick() {
            Tspam.Tick("?",50,10,canvas, matrix,font);
        }

        private void happyTick() {
            allGifs["happy"].playGif(matrix, canvas);
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
                canvas.Clear();
                Tspam.PrintTextBothSides("connect", 7, 10, new Color(255, 255, 255), canvas, matrix, font);
                Tspam.PrintTextBothSides("controller", 0, 20, new Color(255, 255, 255), canvas, matrix, font);
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
            int pick = rand.Next(0, allGifs.Values.Count);
            allGifs.Values.ElementAt(pick).playGif(matrix, canvas);
        }
        private void FlagTick() {
            allGifs["flag"].playGif(matrix, canvas);
        }
        private void overheatTick() {
            allGifs["overheat"].playGif(matrix, canvas);
        }
        private void cwoodTick() {
            allGifs["CWOODSDEAN-full"].playGif(matrix, canvas);
        }
        private void lowbattTick() {
            allGifs["lowbatt"].playGif(matrix, canvas);
        }
        private void matrixTick() {
            allGifs["matrix-spin"].playGif(matrix, canvas);
        }
        private void pacmanTick() {
            allGifs["pacman"].playGif(matrix, canvas);
        }
        private void eightballTick() {
            eightball.updateTick();
            eightball.drawFace(matrix, canvas);
        }
        private void missingfile()
        {
            Thread.Sleep(40);
            canvas.Clear();
            Tspam.PrintTextBothSides("missing", 7, 10, new Color(255, 255, 255), canvas, matrix, font);
            Tspam.PrintTextBothSides("Gif", 7, 23, new Color(255, 255, 255), canvas, matrix, font);
            canvas = matrix.SwapOnVsync(canvas);
        }

    }
}
