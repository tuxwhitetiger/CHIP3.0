using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using rpi_rgb_led_matrix_sharp;
using System.Diagnostics;

namespace CHIP
{
    public class MainClass
    {
        
        public static void Main(string[] args)
        {
            Logger mylogger = new Logger();
            Face face = new Face();
            face.load(mylogger);
            mylogger.Log("start update loop");
            while (true) {
                face.update();
            }

        }
    }
}