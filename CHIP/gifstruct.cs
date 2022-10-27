using System;
using System.Collections.Generic;
using System.Text;

namespace CHIP
{
    class gifstruct
    {
        public List<frame> frames = new List<frame>();
        public gifstruct() {
            
        }
        public void newframe()
        {
            frames.Add(new frame());
        }
        public void newrow()
        {
            frames[frames.Count - 1].rows.Add(new row());
        }

        public void pumpData(int r, int g, int b) {
            int framecount = frames.Count -1;
            int rowcount = frames[framecount].rows.Count -1;
            frames[framecount].rows[rowcount].pixels.Add(new pixel());
            int pixelcount = frames[framecount].rows[rowcount].pixels.Count -1;
            frames[framecount].rows[rowcount].pixels[pixelcount].colors.Add(r);
            frames[framecount].rows[rowcount].pixels[pixelcount].colors.Add(g);
            frames[framecount].rows[rowcount].pixels[pixelcount].colors.Add(b);
        }

        public int[,,,] toArray()
        {
            //build corrrect size array
            //x(cols), y(rows), framecount, color(0=r,1=g,2=b)
            int rows = frames[frames.Count - 1].rows.Count;
            int cols = frames[frames.Count - 1].rows[rows - 1].pixels.Count;
            int framecount = frames.Count;
            int[,,,] output = new int[cols+1,rows + 1, framecount + 1, 3];
            //shove the data into it
            int x=0, y=0, z =0;
            foreach (frame f in frames) {
                y = 0;
                foreach (row r in f.rows) {
                    x = 0;
                    foreach (pixel p in r.pixels) {
                        output[x, y, z, 0] = p.colors[0];
                        output[x, y, z, 1] = p.colors[1];
                        output[x, y, z, 2] = p.colors[2];
                        x++;
                    }
                    y++;
                }
                z++;
            }

            return output;
        }
    }
    public class frame
    {
        public List<row> rows = new List<row>();
        public void newrow() {
            rows.Add(new row());
        }

        public void fixrows() {
            List<row> newrows = new List<row>();
            newrows.AddRange(rows.GetRange(0, 4));
            newrows.AddRange(rows.GetRange(8, 4));
            newrows.AddRange(rows.GetRange(4, 4));
            newrows.AddRange(rows.GetRange(12, 4));
            newrows.AddRange(rows.GetRange(16, 4));
            newrows.AddRange(rows.GetRange(24, 4));
            newrows.AddRange(rows.GetRange(20, 4));
            newrows.AddRange(rows.GetRange(28, 4));
            rows = newrows;
        }

    }
    public class row
    {
        public List<pixel> pixels = new List<pixel>();
        public void newpixel()
        {
            pixels.Add(new pixel());
        }
    }
    public class pixel
    {
        public List<int> colors = new List<int>();
    }
    
}
