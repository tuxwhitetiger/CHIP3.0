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
            for (int myy = 0; myy < 32; myy++)
            {
                for (int myx = 0; myx < 64; myx++)
                {//data x, y, framecount, color(0=r,1=g,2=b)
                    canvas.SetPixel(64 - myx, myy, DrawFaceData[myx, myy]);
                }
            }
            matrix.SwapOnVsync(canvas);
        }
    }
}
