using RPiRgbLEDMatrix;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CHIP
{
    class magic8Ball
    {
        Dictionary<int, string> predictions = new Dictionary<int, string>();
        int currentPrediction = -1;
        public bool shake = false;
        Stopwatch stopwatch = new Stopwatch();
        RGBLedFont font = new RGBLedFont("./fonts/5x8.bdf");

        public magic8Ball() {
            predictions.Add(-1, "boop to shake");//show 8 ball
            predictions.Add(0, "It is certain.");
            predictions.Add(1, "It is decidedly so.");
            predictions.Add(2, "Without a doubt.");
            predictions.Add(3, "Yes definitely.");
            predictions.Add(4, "You may rely on it.");
            predictions.Add(5, "As I see it, yes.");
            predictions.Add(6, "Most likely.");
            predictions.Add(7, "Outlook good.");
            predictions.Add(8, "Yes.");
            predictions.Add(9, "Signs point to yes.");
            predictions.Add(10, "Reply hazy, try again.");
            predictions.Add(11, "Ask again later.");
            predictions.Add(12, "Better not tell you now.");
            predictions.Add(13, "Cannot predict now.");
            predictions.Add(14, "Concentrate and ask again.");
            predictions.Add(15, "Don't count on it.");
            predictions.Add(16, "My reply is no.");
            predictions.Add(17, "My sources say no.");
            predictions.Add(18, "Outlook not so good.");
            predictions.Add(19, "Very doubtful.");
        }

        public void updateTick() {
            if (shake) {
                Random rand = new Random();
                currentPrediction = rand.Next(0, 19);
                shake = false;
                stopwatch.Restart();
            }
            if (stopwatch.ElapsedMilliseconds > 8000) {
                currentPrediction = -1;
                stopwatch.Stop();
            }
        }

        public void drawFace(RGBLedMatrix matrix, RGBLedCanvas canvas) {
            string[]  text = WordWrap(predictions[currentPrediction]);
            canvas.Clear();
            int y = 8;
            foreach (String s in text) {
                canvas.DrawText(font, 7, y, new Color(255, 255, 255), s);
                y += 8;
            }
            matrix.SwapOnVsync(canvas);
        }

        private string[] WordWrap(string str)
        {
            int width = 12;
            string[] words = str.Split(" ");
            List<String> lines = new List<string>();
            int currentLineLength = 0;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < words.Length; i += 1)
            {
                string word = words[i];
                if (currentLineLength + word.Length > width)
                {
                    sb.Append(word);
                }
                else {
                    if (currentLineLength == 0) {
                        sb.Append(word);
                    }
                    String line = sb.ToString();
                    lines.Add(line);
                    currentLineLength = 0;
                    sb = new StringBuilder();
                }
            }
            return lines.ToArray();
        }


    }
}
