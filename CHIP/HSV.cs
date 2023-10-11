using RPiRgbLEDMatrix;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CHIP
{
    public struct RGB
    {
        private byte _r;
        private byte _g;
        private byte _b;

        public RGB(byte r, byte g, byte b)
        {
            this._r = r;
            this._g = g;
            this._b = b;
        }

        public byte R
        {
            get { return this._r; }
            set { this._r = value; }
        }

        public byte G
        {
            get { return this._g; }
            set { this._g = value; }
        }

        public byte B
        {
            get { return this._b; }
            set { this._b = value; }
        }

        public bool Equals(RGB rgb)
        {
            return (this.R == rgb.R) && (this.G == rgb.G) && (this.B == rgb.B);
        }
    }

    public struct HSV
    {
        private double _h;
        private double _s;
        private double _v;

        public HSV(double h, double s, double v)
        {
            this._h = h;
            this._s = s;
            this._v = v;
        }

        public double H
        {
            get { return this._h; }
            set { this._h = value; }
        }

        public double S
        {
            get { return this._s; }
            set { this._s = value; }
        }

        public double V
        {
            get { return this._v; }
            set { this._v = value; }
        }

        public bool Equals(HSV hsv)
        {
            return (this.H == hsv.H) && (this.S == hsv.S) && (this.V == hsv.V);
        }
    }

    public class HSVSystem
    {
        public HSV hue = new HSV(0, 100, 100);
        
        private double speed = 1.0;//numbers of seconds to loop 
        private double tickIncrement = 33.3;//tick per ms
        Stopwatch timer = new Stopwatch();
        public HSVSystem() {
            timer.Start();
        }

        public void SetSpeed(double speed) { 
            this.speed = speed;
            //360 to loop
            //1000 ms 
            //(speed/360) / 1000 = increment per ms
            tickIncrement = ((speed/1000.0)/ 360.0);
        }
        

        public double GetSpeed()
        {
            return this.speed;
        }

        public void Tick() {
            hue.H += (Double)(timer.ElapsedMilliseconds * tickIncrement);
            if (hue.H < 0) { hue.H = 0.0; }
            if (hue.H > 1) { hue.H = 0.0; }
            timer.Restart();
        }

        private double scaler(double degrees) {

            return ((degrees / 100.0) * 60.0);
        }

        public RGB GetRGB()
        {
            double r = 0;
            double g = 0;
            double b = 0;
            if (hue.H > 0 || hue.H < 60)
            {
                //r=1
                //g=var
                //b=0
                r = 1;
                g = scaler(hue.H);
                b = 0;
            }
            else if (hue.H > 60 || hue.H < 120)
            {
                //r=var
                //g=1
                //b=0
                r = scaler(hue.H);
                g = 1;
                b = 0;
            }
            else if (hue.H > 120 || hue.H < 180)
            {
                //r=0
                //g=1
                //b=var
                r = 0;
                g = 1;
                b = scaler(hue.H);
            }
            else if (hue.H > 180 || hue.H < 240)
            {
                //r=0
                //g=var
                //b=1
                r = 0;
                g = scaler(hue.H);
                b = 1;
            }
            else if (hue.H > 240 || hue.H < 300)
            {
                //r=var
                //g=0
                //b=1
                r = scaler(hue.H);
                g = 1;
                b = 0;
            }
            else if (hue.H > 300 || hue.H < 360)
            {
                //r=1
                //g=0
                //b=var
                r = 1;
                g = 0;
                b = scaler(hue.H);
            }

            return new RGB((byte)(r*255), (byte)(g*255), (byte)(b*255));
        }

        public RGB GetRGB2()
        {
            double kr = ((5 + (hue.H * 6)) % 6);
            double kg = ((3 + (hue.H * 6)) % 6);
            double kb = ((1 + (hue.H * 6)) % 6);

            double r = 1 - Math.MaxMagnitude(Math.MinMagnitude(kr, Math.MinMagnitude(4 - kr, 1)), 0);
            double g = 1 - Math.MaxMagnitude(Math.MinMagnitude(kg, Math.MinMagnitude(4 - kg, 1)), 0);
            double b = 1 - Math.MaxMagnitude(Math.MinMagnitude(kb, Math.MinMagnitude(4 - kb, 1)), 0);

            return new RGB((byte)(r*255), (byte)(g*255), (byte)(b*255));
        }

        public RGB GetRGB3()
        {
            HSV hsv = hue;
            double r = 0, g = 0, b = 0;

            if (hsv.S == 0)
            {
                r = hsv.V;
                g = hsv.V;
                b = hsv.V;
            }
            else
            {
                int i;
                double f, p, q, t;

                if (hsv.H == 360)
                    hsv.H = 0;
                else
                    hsv.H = hsv.H / 60;

                i = (int)Math.Truncate(hsv.H);
                f = hsv.H - i;

                p = hsv.V * (1.0 - hsv.S);
                q = hsv.V * (1.0 - (hsv.S * f));
                t = hsv.V * (1.0 - (hsv.S * (1.0 - f)));

                switch (i)
                {
                    case 0:
                        r = hsv.V;
                        g = t;
                        b = p;
                        break;

                    case 1:
                        r = q;
                        g = hsv.V;
                        b = p;
                        break;

                    case 2:
                        r = p;
                        g = hsv.V;
                        b = t;
                        break;

                    case 3:
                        r = p;
                        g = q;
                        b = hsv.V;
                        break;

                    case 4:
                        r = t;
                        g = p;
                        b = hsv.V;
                        break;

                    default:
                        r = hsv.V;
                        g = p;
                        b = q;
                        break;
                }
            }
            return new RGB((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
        }

        internal Color GetColor()
        {
            RGB rgb = GetRGB();
            return new Color(rgb.R, rgb.G, rgb.B);
        }

    }
}
