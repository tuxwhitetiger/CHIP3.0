using SharpDX.WIC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using RPiRgbLEDMatrix;

namespace CHIP
{
    internal class cmatrix
    {
        // properties
        
        static Random rand = new Random();
        Logger mylogger;
        bool play = true;
        int hight = 32;
        int width = 128;
        Color[,] color;
        Color[] toprow;
        bool[] freecol;

        public cmatrix(Logger mylogger) {
            this.mylogger = mylogger;
            mylogger.Log("start cmatrix");
            color = new Color[width, hight];
            toprow = new Color[width];
            freecol = new bool[width];
            for(int i =0;i<freecol.Length;i++) {
                freecol[i] = true;
            }
        }
        public void Frame(RGBLedCanvas canvas, RGBLedMatrix matrix)
        {
            mylogger.Log("Frame");
            shiftDown(canvas);
            matrix.SwapOnVsync(canvas);
        }
        private void createNewRow()
        {
            for(int i= 0; i < freecol.Length; i++)
            {
                if (freecol[i])
                {
                    if (rand.Next(0, 3) == 0) {
                        toprow[i] = new Color(255, 255, 255);
                        freecol[i] = false;
                    }
                }
            }
        }
        private void shiftDown(RGBLedCanvas canvas) {
            Color[,] newcolor = new Color[width, hight];
            for (int y=hight-1 ; y>=1; y--) {
                for (int x=0; x<width ; x++)
                {
                    //shift down
                    mylogger.Log(x + "," + (y - 1) + " moved to " + x + "," + y);
                    newcolor[x,y] = color[x,y-1];
                    //update colour
                    if (newcolor[x, y].R > 0)
                    {
                        newcolor[x, y] = new Color(0, 255, 0);
                    }else
                    {
                        newcolor[x, y].G = (byte)(newcolor[x, y].G / 2);
                    }
                    if (newcolor[x, y].G < 8) {
                            freecol[x] = true;
                    }
                    //draw new color to screen
                    canvas.SetPixel(x,y, newcolor[x, y]);
                }
            }
            color = newcolor;
            createNewRow();
            //drop top row into canvas
            
            for (int i =0; i< width; i++)
            {
                canvas.SetPixel(i, 0, toprow[i]);
            }
        }
    }
}
