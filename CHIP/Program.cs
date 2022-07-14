﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using rpi_rgb_led_matrix_sharp;
using SharpDX.*;

namespace CHIP
{
    class MainClass
    {
        [DllImport("libtest.so", EntryPoint = "print")]
        static extern void print(string message);

        public static void Main(string[] args)
        {
            print("Hello World C# => C++");
            var matrix = new RGBLedMatrix(32, 2, 1);
            var canvas = matrix.CreateOffscreenCanvas();

            mynetwork net = new mynetwork();
            net.connect();
            Console.WriteLine(net.getFace());


            




            /*
            for (var i = 0; i < 1000; ++i)
            {
                for (var y = 0; y < canvas.Height; ++y)
                {
                    for (var x = 0; x < canvas.Width; ++x)
                    {
                        canvas.SetPixel(x, y, new Color(i & 0xff, x, y));
                    }
                }
                canvas.DrawCircle(canvas.Width / 2, canvas.Height / 2, 6, new Color(0, 0, 255));
                canvas.DrawLine(canvas.Width / 2 - 3, canvas.Height / 2 - 3, canvas.Width / 2 + 3, canvas.Height / 2 + 3, new Color(0, 0, 255));
                canvas.DrawLine(canvas.Width / 2 - 3, canvas.Height / 2 + 3, canvas.Width / 2 + 3, canvas.Height / 2 - 3, new Color(0, 0, 255));

                canvas = matrix.SwapOnVsync(canvas);
            }
            */

        }
    }

}