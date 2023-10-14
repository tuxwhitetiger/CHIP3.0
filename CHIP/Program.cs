using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RPiRgbLEDMatrix;
using System.Diagnostics;

namespace CHIP
{
    public class MainClass
    {
        
        public static void Main(string[] args)
        {
            Logger mylogger = new Logger();
            internal_display internal_Display = new internal_display(mylogger);
            Face face = new Face();
            face.load(mylogger);
            mylogger.Log("start update loop");
            while (true) {
                face.update();
            }

        }
    }
}
