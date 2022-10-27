using System;
using System.Collections.Generic;
using System.Text;

namespace CHIP
{
    [Serializable()]
    class GifData
    {
        public int[,,,] data; //x, y, framecount, color(0=r,1=g,2=b)
        public int x = 0;
        public int y = 0;
        public int newFrameCount;
        public string name;
        public int mstick;
        public bool mirror = false;
    }
}
