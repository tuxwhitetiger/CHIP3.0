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

        public bool setup = false;

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

        public Rectangle boundTop = new Rectangle(-1,-1,130,1);
        public Rectangle boundLeft = new Rectangle(-1,-1,1,34);
        public Rectangle boundRight= new Rectangle(129,-1,1,34);
        public Rectangle boundBottom = new Rectangle(-1,33,130,1);

        private HSV hue = new HSV(0,100,100);

        private HSVSystem HSVS = new HSVSystem();


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

                RGB rgb = HSVS.HSVToRGB(hue);
                hue.H += 1;
                
                color = new Color(rgb.R, rgb.G, rgb.B);
                if (hue.H > 360) { hue.H = 0; }
            }
        }
       
    }
}
