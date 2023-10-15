using RPiRgbLEDMatrix;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace CHIP
{
    class textSpam
    {
        bool startted = false;
        int counter = 0;
        Stopwatch timer = new Stopwatch();
        Random rand = new Random();
        tpart[] tparts;
        int tpartscount = 0;
        int poition = 0;

        internal void start(RGBLedCanvas canvas, RGBLedMatrix matrix) {
            timer.Restart();
            canvas.Clear();
            matrix.SwapOnVsync(canvas);
        }
        internal void Tick(string text, int count, int time, RGBLedCanvas canvas, RGBLedMatrix matrix, RGBLedFont font)
        { 
            //check if this new command
            if(tpartscount!=count)
            {
                tpartscount = count;
                tparts = new tpart[tpartscount];
                poition = 0;
            }
            if (poition == tpartscount-1) {
                poition = 0;
            }
            tparts[poition] = generatetpart();
            poition++;
            canvas.Clear();
            foreach (tpart part in tparts)
            {
                canvas.DrawText(font, part.x, part.y, part.col, text);
            }
            timer.Reset();
            timer.Start();
            while (timer.ElapsedMilliseconds < time) { 
            
            }
            matrix.SwapOnVsync(canvas);
        }

        private tpart generatetpart() {
            tpart t = new tpart();
            t.x = rand.Next(0, 128);
            t.y = rand.Next(0, 32);
            t.col = new Color(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
            return t;
        }

        internal void oldTick(string text, int count, int time, RGBLedCanvas canvas, RGBLedMatrix matrix, RGBLedFont font)
        {
            if (startted) {
                if (timer.ElapsedMilliseconds > 2000) {
                    start(canvas, matrix);
                }
                if (timer.ElapsedMilliseconds > (time - Math.Pow(2, counter)))
                {
                    counter++;
                    if (counter > count)
                    {
                        start(canvas, matrix);
                        canvas.Clear();
                        counter = 0;
                    }
                    if (counter < count)
                    {
                        canvas.DrawText(font, rand.Next(0, 128), rand.Next(0, 32), new Color(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)), text);
                    }
                    matrix.SwapOnVsync(canvas);
                    timer.Restart();
                }
            }else{
                start(canvas,matrix);
                startted = true;
            }
        }

        internal void PrintTextBothSides(string text, int posX,int posY, Color col,RGBLedCanvas canvas, RGBLedMatrix matrix, RGBLedFont font) {
            canvas.DrawText(font, posX, posY, col, text);
            canvas.DrawText(font, 64+posX, posY, col, text);
        }

        
    }

    struct tpart {
        public int x;
        public int y;
        public Color col;
    }
}
