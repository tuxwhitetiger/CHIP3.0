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
        top,bottom,left,right,none,
            topleft, topright,
            bottomleft, bottomright
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
        

        internal void generatebounds() {
            bounds = new Rectangle(x,y,width,height);
        }

        internal deaColitionDirection colitionDetection() {
            generatebounds();

            bool top = false;
            bool bottom = false;
            bool left = false;
            bool right = false;
            
            if (bounds.IntersectsWith(boundTop)) { top=true; }
            if (bounds.IntersectsWith(boundBottom)) { bottom = true ; }
            if (bounds.IntersectsWith(boundLeft)) { left = true; }
            if (bounds.IntersectsWith(boundRight)) { right = true; }

            if (top && right)
            {
                return deaColitionDirection.topright;
            }
            if (top && left)
            {
                return deaColitionDirection.topleft;
            }
            if (bottom && left)
            {
                return deaColitionDirection.bottomleft;
            }
            if (bottom && right)
            {
                return deaColitionDirection.bottomright;
            }
            if (top)
            {
                return deaColitionDirection.top;
            }
            if (bottom)
            {
                return deaColitionDirection.bottom;
            }
            if (left)
            {
                return deaColitionDirection.left;
            }
            if (right)
            {
                return deaColitionDirection.right;
            }

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

                case deaColitionDirection.topleft: deltaX = deltaX * -1; deltaY = deltaY * -1; updateColor(); break;
                case deaColitionDirection.topright: deltaX = deltaX * -1; deltaY = deltaY * -1; updateColor(); break;
                case deaColitionDirection.bottomleft: deltaX = deltaX * -1; deltaY = deltaY * -1; updateColor(); break;
                case deaColitionDirection.bottomright: deltaX = deltaX * -1; deltaY = deltaY * -1; updateColor(); break;
            }
            //increment
            x += deltaX;
            y += deltaY;
        }

       
    }
}
