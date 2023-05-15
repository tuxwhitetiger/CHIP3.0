using rpi_rgb_led_matrix_sharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace CHIP
{
    
    class Gif 
    {
        Logger mylogger;
        public GifData data = new GifData();
        Stopwatch timer = new Stopwatch();
        
        public Gif(string name,bool mirror)
        {
            data.mirror = mirror;
            data.name = name;
        }

        public Gif(string name)
        {
            data.mirror = true;
            data.name = name;
        }
        public Gif() { 
            //only to be used by deserialiser
        }

        public void SetLogger(Logger mylogger) { 
            this.mylogger = mylogger;
        }
        public void loadData(String rawdata, int tick) {
            data.mstick = tick;
            gifstruct newgif = new gifstruct();
            //trim fat from the end of the message ,FRAMEDONE
            string[] fixedData = rawdata.Split("DONE");
            //split out the indevidual frames

            string[] frames = fixedData[0].Split('F', StringSplitOptions.RemoveEmptyEntries);
            foreach (String frame in frames) {
                newgif.newframe();
                string[] rows = frame.Split('R', StringSplitOptions.RemoveEmptyEntries);
                foreach (String row in rows)
                {
                    newgif.newrow();
                    string[] pixels = row.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    if (pixels.Length <= 1)
                    {//pritty sure that we at the end of the row here
                        break;
                    }
                    data.x = pixels.Length;
                    foreach (String Pixel in pixels)
                    {
                        String trimmed = Pixel.Trim('[');
                        String trimmed2 = trimmed.Trim(']');
                        string[] colors = trimmed2.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        newgif.pumpData(Int32.Parse(colors[0]), Int32.Parse(colors[1]), Int32.Parse(colors[2]));
                    }
                    data.y = rows.Length;
                }
                newgif.frames[newgif.frames.Count - 1].fixrows();


            }
            data.newFrameCount = newgif.frames.Count;
            data.data = newgif.toArray();
        }

        internal void printFrame(RGBLedMatrix matrix, RGBLedCanvas canvas, int myFrame)
        {
            int maxy = data.y;
            if (canvas.Height < maxy)
            {
                maxy = canvas.Height;
            }

            int maxx = data.x;
            if (canvas.Width < maxx)
            {
                maxx = canvas.Width;
            }
            for (int myy = 0; myy < maxy; myy++)
            {
                for (int myx = 0; myx < maxx; myx++)
                {//data x, y, framecount, color(0=r,1=g,2=b)
                    canvas.SetPixel(64 - myx, myy, new Color(data.data[myx, myy, myFrame, 0], data.data[myx, myy, myFrame, 1], data.data[myx, myy, myFrame, 2]));
                }
            }
            canvas = matrix.SwapOnVsync(canvas);
        }
        internal void printmirroredFrame(RGBLedMatrix matrix, RGBLedCanvas canvas, int myFrame)
        {
            int maxy = data.y;
            if (canvas.Height < maxy)
            {
                maxy = canvas.Height;
            }

            int maxx = data.x;
            if (canvas.Width < maxx)
            {
                maxx = canvas.Width;
            }
            for (int myy = 0; myy < maxy; myy++)
            {
                for (int myx = 0; myx < maxx; myx++)
                {//data x, y, framecount, color(0=r,1=g,2=b)
                    Color c = new Color(data.data[myx, myy, myFrame, 0], data.data[myx, myy, myFrame, 1], data.data[myx, myy, myFrame, 2]);
                    canvas.SetPixel(64 - myx, myy, c);
                    canvas.SetPixel(64 + myx, myy, c);
                }
            }
            canvas = matrix.SwapOnVsync(canvas);
        }

        internal void printPiviotFrame(RGBLedMatrix matrix, RGBLedCanvas canvas, int myFrame)
        {
            int maxy = data.y;
            if (canvas.Height < maxy)
            {
                maxy = canvas.Height;
            }

            int maxx = data.x;
            if (canvas.Width < maxx)
            {
                maxx = canvas.Width;
            }
            for (int myy = 0; myy < maxy; myy++)
            {
                for (int myx = 0; myx < maxx; myx++)
                {//data x, y, framecount, color(0=r,1=g,2=b)
                    Color c = new Color(data.data[myx, myy, myFrame, 0], data.data[myx, myy, myFrame, 1], data.data[myx, myy, myFrame, 2]);
                    canvas.SetPixel(myx, myy, c);
                    canvas.SetPixel(64 + myx, myy, c);
                }
            }
            canvas = matrix.SwapOnVsync(canvas);
        }

        internal void playGif(RGBLedMatrix matrix, RGBLedCanvas canvas) {
            mylogger.Log("gif name:" + data.name);
            Console.WriteLine("gif name:"+ data.name);
            int leftpos = Console.CursorLeft;
            int toppos = Console.CursorTop;
            timer.Reset();
            timer.Start();
            for (int i = 0; i < data.newFrameCount; i++) {
                if (data.mirror)
                {
                    //Console.WriteLine("data.mirror:true");
                    printmirroredFrame(matrix, canvas, i);
                }
                else if (data.piviot) {
                    //Console.WriteLine("data.piviot:true");
                    printPiviotFrame(matrix, canvas, i);
                }
                else
                {
                    //Console.WriteLine("standard:true");
                    printFrame(matrix, canvas, i);
                }
                
                Thread.Sleep(data.mstick);
            }
            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;
            double fps = (1000 / timeTaken.TotalMilliseconds) * data.newFrameCount;
            Console.SetCursorPosition(leftpos, toppos);
            Console.Write("FPS:" + fps);
            mylogger.Log("FPS:" + fps);
        }
    
    }
}
