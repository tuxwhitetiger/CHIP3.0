using rpi_rgb_led_matrix_sharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace CHIP
{
    class Gif
    {
        int[,,,] data; //x, y, framecount, color(0=r,1=g,2=b)
        int x;
        int y;
        int framecount;
        public int newFrameCount;
        public Gif(int x, int y, int framecount) {
            this.x = x;
            this.y = y;
            this.framecount = framecount;
            data = new int[x, y, framecount, 3];
        }

        public void loadData(String rawdata) {
            //trim fat from the end of the message ,FRAMEDONE
            string[] fixedData = rawdata.Split("DONE");
            //split out the indevidual frames
            string[] frames = fixedData[0].Split(",FRAME");
            char[] charsToTrim = {'[',']'};
            foreach (String frame in frames) {
                Console.WriteLine("processing frame:" + newFrameCount);
                int xcounter = 0;
                int ycounter = 0;
                //split to pixels
                string[] pixels = frame.Split(',');
                Console.WriteLine("pixel count:" + pixels.Length);
                if (pixels.Length <= 1)
                {//pritty sure that we at the end of the file here
                    break;
                }
                foreach (String Pixel in pixels)
                {
                    //split into color
                    String trimmed = Pixel.Trim(charsToTrim);
                    //Console.WriteLine(trimmed);
                    string[] colors = trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    //if there more then 3 colors in pixel let me know
                    if (colors.Length != 3)
                    {
                        Console.WriteLine("we done fucked up with colors");
                    }
                    //put the color data into the array
                    //this bit might change to make it easyer to push to display
                    data[xcounter, ycounter, newFrameCount, 0] = Int32.Parse(colors[0]);
                    data[xcounter, ycounter, newFrameCount, 1] = Int32.Parse(colors[1]);
                    data[xcounter, ycounter, newFrameCount, 2] = Int32.Parse(colors[2]);
                    //increment x counter
                    xcounter++;
                    //if we are at the end of the row go to start of next one
                    if (xcounter == x)
                    {
                        xcounter = 0;
                        //Console.WriteLine("next row");
                        ycounter++;
                    }
                    //if there are more rows then expected let me know
                    if (ycounter == y)
                    {
                        Console.WriteLine("we done fucked up with row counting");
                    }
                }

                //move to the next frame
                newFrameCount++;
                //if there are more frames then expected let me know
                if (newFrameCount == framecount)
                {
                    Console.WriteLine("we done fucked up with frame count");
                }
            }
        }

        internal void printFrame(RGBLedCanvas canvas,int myFrame)
        {
            for (int myy = 0; myy < y; myy++)
            {
                for (int myx = 0; myx < x; myx++)
                {//data x, y, framecount, color(0=r,1=g,2=b)
                    canvas.SetPixel(myx, myy, new Color(data[myx, myy, myFrame,0], data[myx, myy, myFrame, 1], data[myx, myy, myFrame, 2]));
                }
            }
        }
    }
}
