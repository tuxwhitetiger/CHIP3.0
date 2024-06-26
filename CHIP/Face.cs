﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RPiRgbLEDMatrix;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CHIP
{
    class Face
    {
        Logger mylogger;
        //controller_network cnet;
        List<GameController> controllerstokill;

        CalanderClock clock;

        TextFace tface;
        DrawFace dface;

        snake snakegame;
        magic8Ball eightball = new magic8Ball();
        cmatrix cmatrix ;
        textSpam Tspam = new textSpam();
        DynamicAnimationEngine dae = new DynamicAnimationEngine();
        private HSVSystem HSVS = new HSVSystem();

        RGBLedFont font;

        RGBLedMatrixOptions options = new RGBLedMatrixOptions();
        
        RGBLedMatrix matrix;
        RGBLedCanvas canvas;
        Stopwatch timer;
        Stopwatch faceAnimationTimer = new Stopwatch();
        int faceAnimationDelay = 0;
        Random rand = new Random();

        faces runningface = faces.happy;
        

        //setup gif faces
        IDictionary<string, Gif> allGifs = new Dictionary<string, Gif>();

        public void load(Logger mynewlogger, mynetwork net)
        {
            mylogger = mynewlogger;
            mylogger.Log("starting loader");
            

            options.Rows = 32;
            options.Cols = 64;
            options.ChainLength = 2;
            options.Parallel = 1;
            options.GpioSlowdown = 4;
            options.HardwareMapping = "regular-CD-Flip"; //regular	adafruit-hat	adafruit-hat-pwm	regular-pi1	classic	classic-pi1	compute-module
            options.ScanMode = ScanModes.Progressive; //can be 0 or 1
            options.RowAddressType= 0;//can be 0-4
            options.Multiplexing= 0;//TEST LINE!!!
            // Multiplexing can only be one of 0=normal; 1=Stripe; 2=Checkered; 3=Spiral; 4=ZStripe; 5=ZnMirrorZStripe; 6=coreman; 7=Kaler2Scan; 8=ZStripeUneven; 9=P10-128x4-Z; 10=QiangLiQ8; 11=InversedZStripe; 12=P10Outdoor1R1G1-1; 13=P10Outdoor1R1G1-2; 14=P10Outdoor1R1G1-3; 15=P10CoremanMapper; 16=P8Outdoor1R1G1; 17=FlippedStripe; 18=P10Outdoor32x16HalfScan
            

            mylogger.Log("matrix pop");
            matrix = new RGBLedMatrix(options);
            mylogger.Log("canvas pop");
            canvas = matrix.CreateOffscreenCanvas();
            mylogger.Log("net test");
           // Console.WriteLine(net.getFace());
            //mylogger.Log("net test result" + net.getFace());
            mylogger.Log("load snake");
            snakegame = new snake();
            mylogger.Log("load font");
            font = new RGBLedFont("./fonts/7x14.bdf");

            canvas.Clear();
            mylogger.Log("push text");
            Tspam.PrintTextBothSides("Loading", 7, 10, new Color(255, 255, 255), canvas, matrix, font);
            Tspam.PrintTextBothSides("Gifs", 7, 23, new Color(255, 255, 255), canvas, matrix, font);
            matrix.SwapOnVsync(canvas);


            mylogger.Log("check serialized data if not ask python to do it");
            // check if i have serialized data if not ask python to do it
            DirectoryInfo facesFolder = new DirectoryInfo("./faces");
            FileInfo[] faces = facesFolder.GetFiles("*.gif");
            DirectoryInfo serialDataFolder = new DirectoryInfo("./serial");
            FileInfo[] serialfaces = serialDataFolder.GetFiles("*.serial");
            List<FileInfo> toserialize = new List<FileInfo>();

            mylogger.Log("remove from faces list if in serialized list");
            //remove from faces list if in serialized list
            foreach (FileInfo fi in faces) {
                mylogger.Log("file name : " + fi.Name);
                Console.WriteLine(fi.Name);
                bool removed = false;
                foreach (FileInfo fi2 in serialfaces) {
                    mylogger.Log("file2 name : " + fi2.Name);
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

            mylogger.Log("load from serial serialfaces");
            //load from serial serialfaces
            foreach (FileInfo fi in serialfaces)
            {
                mylogger.Log("deserialising :" + fi.Name);
                Console.WriteLine("deserialising :" + fi.Name);
                BinaryFormatter formatter = new BinaryFormatter();
                String source = "./serial/";
                source += fi.Name;
                Stream reader = new FileStream(source, FileMode.Open, FileAccess.Read);
                Gif g = new Gif();
                g.data = (GifData)formatter.Deserialize(reader);

                allGifs.Add(g.data.name, g);
            }

            mylogger.Log("//load from python");
            //load from python
            foreach (FileInfo fi in toserialize)
            {
                mylogger.Log("loading :" + fi.Name);
                Console.WriteLine("loading :" + fi.Name);
                Gif g = new Gif(fi.Name.Split('.')[0]);
                mylogger.Log("data fetch");
                String data = net.GetGifData(fi.Name);
                mylogger.Log("data fetch complete");
                mylogger.Log("data load");
                g.loadData(data, 40);
                mylogger.Log("data load Complete");
                mylogger.Log("Build Serializer");
                //now its loaded need to serialize and save for next time
                mylogger.Log("Build Serializer : starting formatter");
                BinaryFormatter formatter = new BinaryFormatter();
                mylogger.Log("Build Serializer : starting destination");
                String destination = "./serial/";
                mylogger.Log("Build Serializer : starting destination1");
                destination += fi.Name.Trim().Substring(0, fi.Name.Trim().Length - 4);
                mylogger.Log("Build Serializer : starting destination2");
                destination += ".serial";
                mylogger.Log("Build Serializer : starting stream");
                
                try
                {
                    Stream writer = new FileStream(destination, FileMode.Create, FileAccess.Write);
                    mylogger.Log("Build Serializer complete");
                    mylogger.Log("writing :" + fi.Name);
                    formatter.Serialize(writer, g.data);
                    writer.Close();
                }catch( Exception ex)
                {
                    if (ex is FileNotFoundException)
                    {
                        mylogger.Log("File Not Found Exception");
                        mylogger.Log("message: " + ex.Message);
                        mylogger.Log("StackTrace: " + ex.StackTrace);
                    }
                    else { 
                        mylogger.Log("well shit some thing gone wrong");
                        mylogger.Log("message: " + ex.Message);
                        mylogger.Log("StackTrace: " + ex.StackTrace);
                    }
                }
                
                allGifs.Add(g.data.name, g);
            }
            mylogger.Log("manual fixes to gif data");
            //force inject mistake corrections
            allGifs["overheat"].data.piviot = true;
            allGifs["overheat"].data.mirror = false;
            mylogger.Log("setting logger to each gif");
            foreach (Gif g in allGifs.Values)
            {
                g.SetLogger(mylogger);
            }

            timer = new Stopwatch();
            timer.Start();
            canvas.Clear();
            mylogger.Log("load cmatrix");
            cmatrix = new cmatrix(mylogger);

            mylogger.Log("load textface");
            tface = new TextFace(mylogger, " TEST ");
            dface = new DrawFace(mylogger);

            mylogger.Log("completed loader");
            this.runningface = runningface;
            mylogger.Log("dictonery count:" + allGifs.Count.ToString());
            Console.WriteLine("dictonery count:" + allGifs.Count.ToString());
            mylogger.Log("dictionary:");
            Console.WriteLine("dictionary:");
            foreach (String s in allGifs.Keys)
            {
                mylogger.Log(s);
                Console.WriteLine(s);
            }
        }
        public void update(faces runningface) {
            HSVS.Tick();
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
                case faces.Love: loveTick();break;
                case faces.textTest: textTest();break;
                case faces.DvDBounce: DvDTick(); break;
                case faces.matrixRain: matrixRain();break;
                case faces.textFace: textFace();break;
                case faces.DrawFace: drawface();break;
            }
        }

        private void drawface()
        {
            dface.update(canvas, matrix);
        }

        private void textFace()
        {
            tface.update(canvas, matrix, font);
        }

        private void matrixRain() {
            cmatrix.Frame(canvas, matrix);
        }
        private void textTest()
        {
            canvas.Clear();
            Tspam.PrintTextBothSides("TEST", 7, 10, new Color(255, 255, 255), canvas, matrix, font);
            Tspam.PrintTextBothSides("TEXT", 7, 23, new Color(255, 255, 255), canvas, matrix, font);
        }
        private void loveTick()
        {
            allGifs["Love-Face"].playGif(matrix, canvas);
        }
        private void sadTick() {
            allGifs["Sad-Face"].playGif(matrix, canvas);
        }
        private void OhTick() {
            allGifs["Shocked-Face"].playGif(matrix, canvas);
        }
        private void AngryTick()
        {
            allGifs["Angy-Face"].playGif(matrix, canvas);
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

            Tspam.Tick("?",50,25,canvas, matrix,font);
        }
        private void happyTick() {
            if (faceAnimationTimer.Elapsed.TotalSeconds >= faceAnimationDelay)
            {
                allGifs["happy"].playGif(matrix, canvas);
                faceAnimationTimer.Restart();
                faceAnimationDelay = rand.Next(0, 5);
            }
            else {
                allGifs["happy"].printmirroredFrame(matrix, canvas,1);
            }
        }
        private void setupDvD() {
            if (!dae.setup)
            {
                dae.mode = daemode.rainbowBounce;
                dae.x = rand.Next(1, 5);
                dae.y = rand.Next(1, 5);
                dae.deltaX = 1;
                dae.deltaY = 1;
                dae.color = HSVS.GetColor();
                dae.width = 35;
                dae.height = 16;
                faceAnimationDelay = 33;

                HSVS.SetSpeed(1);

                dae.setup= true;
            }
        }
        private void DvDTick() {
            setupDvD();
            allGifs["DVD_logo"].printColorGrayscaleFrame(matrix, canvas, 0, HSVS.GetColor(), dae.x, dae.y);
            if (faceAnimationTimer.Elapsed.TotalMilliseconds >= faceAnimationDelay)
            {
                dae.tick();
                faceAnimationTimer.Restart();
            }
            //16x35
        }
        private void snakeTick()
        {
            /*
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
                matrix.SwapOnVsync(canvas);
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
            */
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
            //need to fix moves to fast
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
            //broken
        }
        private void missingfile()
        {
            Thread.Sleep(40);
            canvas.Clear();
            Tspam.PrintTextBothSides("missing", 7, 10, new Color(255, 255, 255), canvas, matrix, font);
            Tspam.PrintTextBothSides("Gif", 7, 23, new Color(255, 255, 255), canvas, matrix, font);
            matrix.SwapOnVsync(canvas);
        }
        private void RequestHandeler(string getRequest)
        {
            switch (getRequest)
            {
                //case "GET TIME": net.speak("time is " + DateTime.Now.Hour.ToString() + " " + DateTime.Now.Minute.ToString()); break;

            }
        }

        internal void setText(string text)
        {
            tface.SetText(text);
        }

        internal void SetTextFacespeed(string speed)
        {
            tface.SetTextFacespeed(speed);
        }

        internal void SetTextFaceColour(string colour)
        {
            tface.SetTextFaceColour(colour);
        }
        internal void SetTextFaceScroll(bool scroll)
        {
            tface.SetTextFaceScroll(scroll);
        }

        internal void setDrawFaceData(Color[,] colors)
        {
            dface.DrawFaceData = colors; 
        }
    }
}
