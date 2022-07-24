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
            Face face = new Face();
            face.load();
            while (true) {
                face.update();
            }

        }
    }
}