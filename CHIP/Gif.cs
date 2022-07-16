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
        int x;
        int y;
        int framecount;
        public int newFrameCount;
        Stopwatch timer = new Stopwatch();
        public Gif(int x, int y, int framecount) {
            this.x = x;
            this.y = y;
            this.framecount = framecount;
            data = new int[x, y, framecount, 3];
        }

        public void loadData(String rawdata) {
            gifstruct newgif = new gifstruct();
            //trim fat from the end of the message ,FRAMEDONE
            string[] fixedData = rawdata.Split("DONE");
            //split out the indevidual frames
            string[] frames = fixedData[0].Split('!', StringSplitOptions.RemoveEmptyEntries);
            char[] charsToTrim = {'[',']'};
            
            foreach (String frame in frames) {
                newgif.newframe();
                Console.WriteLine("processing frame:" + newgif.frames.Count);
                string[] rows = frame.Split('#', StringSplitOptions.RemoveEmptyEntries);
                foreach (String row in rows)
                {
                    newgif.newrow();
                    string[] pixels = frame.Split(',');
                    if (pixels.Length <= 1)
                    {//pritty sure that we at the end of the row here
                        break;
                    }
                    foreach (String Pixel in pixels)
                    {
                        String trimmed = Pixel.Trim(charsToTrim);
                        string[] colors = trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        Console.WriteLine("count:" + colors.Length);
                        Console.WriteLine("r:" + colors[0]);
                        Console.WriteLine("g:" + colors[1]);
                        Console.WriteLine("b:" + colors[2]);
                        newgif.pumpData(Int32.Parse(colors[0]), Int32.Parse(colors[1]), Int32.Parse(colors[2]));
                    }
                }
            }
            newFrameCount = newgif.frames.Count;
            data = newgif.toArray();
        }

        internal void printFrame(RGBLedMatrix matrix, RGBLedCanvas canvas,int myFrame)
        {
            for (int myy = 0; myy < y; myy++)
            {
                for (int myx = 0; myx < x; myx++)
                {//data x, y, framecount, color(0=r,1=g,2=b)
                    canvas.SetPixel(myx, myy, new Color(data[myx, myy, myFrame,0], data[myx, myy, myFrame, 1], data[myx, myy, myFrame, 2]));
                }
            }
            canvas = matrix.SwapOnVsync(canvas);
        }
        internal void playGif(RGBLedMatrix matrix, RGBLedCanvas canvas, int mstick) {
            int leftpos = Console.CursorLeft;
            int toppos = Console.CursorTop;
            timer.Reset();
            timer.Start();
            for (int i = 0; i < newFrameCount; i++) {
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
