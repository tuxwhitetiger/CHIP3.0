using rpi_rgb_led_matrix_sharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace CHIP
{
    class Gif
    {
        int[,,,] data; //x, y, framecount, color(0=r,1=g,2=b)
        int x=0;
        int y=0;
        int framecount=0;
        public int newFrameCount;
        Stopwatch timer = new Stopwatch();
        string name;

        public Gif(string name) {
            this.name = name;
        }

        public void loadData(String rawdata) {
            gifstruct newgif = new gifstruct();
            //trim fat from the end of the message ,FRAMEDONE
            string[] fixedData = rawdata.Split("DONE");
            //split out the indevidual frames
            string[] frames = fixedData[0].Split('F', StringSplitOptions.RemoveEmptyEntries);
            foreach (String frame in frames) {
                newgif.newframe();
                Console.WriteLine("processing frame:" + newgif.frames.Count);
                string[] rows = frame.Split('R', StringSplitOptions.RemoveEmptyEntries);
                foreach (String row in rows)
                {
                    newgif.newrow();
                    string[] pixels = row.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    if (pixels.Length <= 1)
                    {//pritty sure that we at the end of the row here
                        break;
                    }
                    foreach (String Pixel in pixels)
                    {
                        String trimmed = Pixel.Trim('[');
                        String trimmed2 = trimmed.Trim(']');
                        string[] colors = trimmed2.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        newgif.pumpData(Int32.Parse(colors[0]), Int32.Parse(colors[1]), Int32.Parse(colors[2]));
                    }
                }
            }
            newFrameCount = newgif.frames.Count;
            Console.WriteLine("shunt data");
            data = newgif.toArray();
        }

        internal void printFrame(RGBLedMatrix matrix, RGBLedCanvas canvas,int myFrame)
        {
            int maxy = y;
            if (canvas.Height < maxy) {
                maxy = canvas.Height;
            }

            int maxx = x;
            if (canvas.Width < maxx) {
                maxx = canvas.Width;
            }

            for (int myy = 0; myy < maxy; myy++)
            {
                for (int myx = 0; myx < maxx; myx++)
                {//data x, y, framecount, color(0=r,1=g,2=b)
                    Console.WriteLine("x:" + myx + " y:" + myy + " r:" + data[myx, myy, myFrame, 0] + " g:" + data[myx, myy, myFrame, 1] + " b:" + data[myx, myy, myFrame, 2]);
                    canvas.SetPixel(myx, myy, new Color(data[myx, myy, myFrame,0], data[myx, myy, myFrame, 1], data[myx, myy, myFrame, 2]));
                }
            }
            canvas = matrix.SwapOnVsync(canvas);
        }
        internal void playGif(RGBLedMatrix matrix, RGBLedCanvas canvas, int mstick) {
            Console.WriteLine("gif name:"+name);
            int leftpos = Console.CursorLeft;
            int toppos = Console.CursorTop;
            timer.Reset();
            timer.Start();
            for (int i = 0; i < newFrameCount; i++) {
                Console.WriteLine("frame:" + i);
                printFrame(matrix,canvas, i);
                Thread.Sleep(mstick);
            }
            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;
            double fps = (1000 / timeTaken.TotalMilliseconds) * newFrameCount;
            Console.SetCursorPosition(leftpos, toppos);
            Console.Write("FPS:" + fps);
        }
    }
}
