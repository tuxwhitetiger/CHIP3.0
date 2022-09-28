using rpi_rgb_led_matrix_sharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CHIP
{
    class textSpam
    {
        int counter = 0;
        Stopwatch timer = new Stopwatch();
        Random rand = new Random();
        internal void start(RGBLedCanvas canvas, RGBLedMatrix matrix) {
            timer.Restart();
            canvas.Clear();
            canvas = matrix.SwapOnVsync(canvas);
        }

        internal void Tick(string text, int count, int time, RGBLedCanvas canvas, RGBLedMatrix matrix, RGBLedFont font)
        {
            counter++;
            if (timer.ElapsedMilliseconds > (time - Math.Pow(2, counter)))
            {
                if (counter == count) {
                    start(canvas, matrix);
                    counter = 0;
                }
                if (counter < count) {
                    canvas.DrawText(font, rand.Next(0,128), rand.Next(0,32), new Color(rand.Next(0,255), rand.Next(0, 255), rand.Next(0, 255)), text);
                    canvas = matrix.SwapOnVsync(canvas);
                }
                timer.Restart();
            }
        }

        internal void PrintTextBothSides(string text, int posX,int posY, Color col,RGBLedCanvas canvas, RGBLedMatrix matrix, RGBLedFont font) {
            canvas.DrawText(font, posX, posY, col, text);
            canvas.DrawText(font, 64+posX, posY, col, text);
            canvas = matrix.SwapOnVsync(canvas);
        }
    }
}
