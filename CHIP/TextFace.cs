using RPiRgbLEDMatrix;
using System;
using System.Diagnostics;

namespace CHIP
{
    class TextFace
    {
        string text;
        int textsize = 0;
        Stopwatch timer = new Stopwatch();
        static Random rand = new Random();
        Logger mylogger;
        bool play = true;
        int hight = 32;
        int width = 128;

        int x=0;
        int x2=0;
        int y=16;
        Color col = new Color(255,255,255);
        int speed = 200;//ms delay per pixel shift
        public TextFace(Logger mylogger,string text)
        {
            this.text = text;
        }
        internal void SetText(string text)
        {
            this.text = text;
        }
        internal void update(RGBLedCanvas canvas, RGBLedMatrix matrix, RGBLedFont font)
        {
            canvas.Clear();
            textsize= canvas.DrawText(font, x, y, col, text);
            canvas.DrawText(font, x2, y, col, text);
            matrix.SwapOnVsync(canvas);
            Move();
        }

        public void Move() {
            bool tick = false;
            if(timer.IsRunning == true)
            {
                if (timer.ElapsedMilliseconds >= speed) { 
                    tick = true;
                    timer.Restart();
                }
            }else
            {
                timer.Stop();
                timer.Start();
            }

            if(tick)
            {
                if (textsize < width)
                {
                    x++;
                    x2++;
                    if (x == (width - textsize))
                    {
                        x2 = 0 - textsize;
                    }
                    if (x2 == (width - textsize))
                    {
                        x = 0 - textsize;
                    }
                }else
                {
                    x++;
                    x2 = 0 - textsize;
                    if (x == width)
                    {
                        x = 0 - textsize;
                    }
                }
            }
        }

        internal void SetTextFacespeed(string speed)
        {
            this.speed = int.Parse(speed);
        }

        internal void SetTextFaceColour(string colour)
        {
            col = new Color(Convert.ToInt32(colour.Substring(0, 2), 16), Convert.ToInt32(colour.Substring(2, 2), 16), Convert.ToInt32(colour.Substring(4, 2), 16));
        }
    }
}