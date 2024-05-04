using RPiRgbLEDMatrix;
using System;
using System.Collections.Generic;
using System.Text;

namespace CHIP
{
    internal class DrawFace
    {
        Logger mylogger;
        public DrawFace(Logger mylogger) { 
         this.mylogger = mylogger;
        }

        public Color[,] DrawFaceData;


        internal void update(RGBLedCanvas canvas, RGBLedMatrix matrix)
        {
            int maxy = 32;
            if (canvas.Height < maxy)
            {
                maxy = canvas.Height;
            }

            int maxx = 64;
            if (canvas.Width < maxx)
            {
                maxx = canvas.Width;
            }
            for (int myy = 0; myy < maxy; myy++)
            {
                for (int myx = 0; myx < maxx; myx++)
                {//data x, y, framecount, color(0=r,1=g,2=b)
                    canvas.SetPixel(64 - myx, myy, DrawFaceData[myx, myy]);
                }
            }
            matrix.SwapOnVsync(canvas);
        }
    }
}
