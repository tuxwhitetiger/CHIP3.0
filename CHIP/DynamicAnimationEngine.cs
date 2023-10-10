using System;
using System.Collections.Generic;
using RPiRgbLEDMatrix;
using System.Text;
using System.Drawing;
using Color = RPiRgbLEDMatrix.Color;
using SharpDX.Direct2D1.Effects;
using SharpDX.Direct2D1;

namespace CHIP
{
    public enum daemode { 
        bounce,
        rainbowBounce
    }
    public enum deaColitionDirection { 
        top,bottom,left,right,none
    }
    public class DynamicAnimationEngine
    {
        Random rand = new Random();

        public daemode mode;
        //pos
        public int x;
        public int y;
        public int width;
        public int height;

        //changes
        public int deltaX;
        public int deltaY;
        public int deltaWidth;
        public int deltaHeight;

        public Color color;

        //colition bounds
        public Rectangle bounds;

        public Rectangle boundTop = new Rectangle(0,-1,128,1);
        public Rectangle boundLeft = new Rectangle(-1,0,1,32);
        public Rectangle boundRight= new Rectangle(128,0,1,32);
        public Rectangle boundBottom = new Rectangle(0,32,128,1);

        private int hue = 0;

        internal void generatebounds() {
            bounds = new Rectangle(x,y,width,height);
        }

        internal deaColitionDirection colitionDetection() {
            generatebounds();
            if (bounds.IntersectsWith(boundTop)) { return deaColitionDirection.top; }
            if (bounds.IntersectsWith(boundBottom)) { return deaColitionDirection.bottom; }
            if (bounds.IntersectsWith(boundLeft)) { return deaColitionDirection.left; }
            if (bounds.IntersectsWith(boundRight)) { return deaColitionDirection.right; }

            return deaColitionDirection.none;
        }
        internal void updateColor() {
            if (mode == daemode.bounce)
            {
                color = new Color(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
            }
        }
        internal void tick()
        {
            //check if coliding
            switch (colitionDetection()) {
                //reverse based on face and new colour
                case deaColitionDirection.top: deltaY = deltaY * -1; updateColor(); break;
                case deaColitionDirection.bottom: deltaY = deltaY * -1; updateColor(); break;
                case deaColitionDirection.left: deltaX = deltaX * -1; updateColor(); break;
                case deaColitionDirection.right: deltaX = deltaX * -1; updateColor(); break;
            }
            
            //increment
            x += deltaX;
            y += deltaY;
        }
        internal void ColorTick()
        {
            if (mode == daemode.rainbowBounce)
            {
                int r = 0;
                int g = 0;
                int b = 0;

                HsvToRgb(hue, out r, out g, out b);
                hue += 1;
                color = new Color(r, g, b);
            }
        }
        void HsvToRgb(double h, out int r, out int g, out int b)
        {
            double S = 100;
            double V = 100;

            double H = h;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            double R, G, B;
            if (V <= 0)
            { R = G = B = 0; }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i)
                {

                    // Red is the dominant color

                    case 0:
                        R = V;
                        G = tv;
                        B = pv;
                        break;

                    // Green is the dominant color

                    case 1:
                        R = qv;
                        G = V;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = V;
                        B = tv;
                        break;

                    // Blue is the dominant color

                    case 3:
                        R = pv;
                        G = qv;
                        B = V;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = V;
                        break;

                    // Red is the dominant color

                    case 5:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                    case 6:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // The color is not defined, we should throw an error.

                    default:
                        //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                        R = G = B = V; // Just pretend its black/white
                        break;
                }
            }
            r = Clamp((int)(R * 255.0));
            g = Clamp((int)(G * 255.0));
            b = Clamp((int)(B * 255.0));
        }
        int Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }

    }
}
