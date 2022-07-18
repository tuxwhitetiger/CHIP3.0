using rpi_rgb_led_matrix_sharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace CHIP
{
    class snake
    {
        food food;
        List<snakeSegment> Snake = new List<snakeSegment>();
        bool started = false;
        bool dead = false;
        RGBLedFont font;
        public snake() {
            startNewGame();
            font = new RGBLedFont("./fonts/7x13.bdf");
        }

        public void startNewGame() {
            started = false;
            dead = false;
            food = newfood();
            snakeSegment head = new snakeSegment(8, 16, true, direction.right);
            head.moving = true;
            Snake.Add(head);
            snakeSegment tail = new snakeSegment(7, 16, false, direction.right, head);
            tail.moving = true;
            Snake.Add(tail);
            snakeSegment tail2 = new snakeSegment(6, 16, false, direction.right, tail);
            tail2.moving = true;
            Snake.Add(tail2);
            snakeSegment tail3 = new snakeSegment(5, 16, false, direction.right, tail2);
            tail3.moving = true;
            Snake.Add(tail3);
        }

        public void movesnake() {
            if (started)
            {
                Console.WriteLine("snake segments:" + Snake.Count);
                Snake.Reverse();
                foreach (snakeSegment ss in Snake)
                {
                    Console.WriteLine("update:" + Snake.IndexOf(ss));
                    ss.update();
                    //head colition with tail check
                }
                Snake.Reverse();
                Console.WriteLine("checkColition");
                checkColition();
            }
        }
        public void checkColition() {
            foreach (snakeSegment ss in Snake)
            {
                if (Snake[0] != ss)
                {
                    if ((Snake[0].x == ss.x) && (Snake[0].y == ss.y))
                    {
                        dead = true;
                    }
                }
            }
            if (((Snake[0].x == 0) || (Snake[0].x == 64)) && ((Snake[0].y == 0) || (Snake[0].y == 32)))
            {
                dead = true;
            }
        }
        public void checkForFood() {
            if ((Snake[0].x == food.x) && (Snake[0].y == food.y)) {
                //nom nom
                snakeSegment extraTail = new snakeSegment(food.x, food.y, false, Snake[0].direction, Snake[Snake.Count-1]);
                Snake.Add(extraTail);
                food = newfood();
            }
        }

        public food newfood() {
            Random rand = new Random();
            int x = rand.Next(0, 64);
            int y = rand.Next(0, 32);
            food f = new food(x,y);
            return f;
        }

        public void printframe(RGBLedMatrix matrix, RGBLedCanvas canvas) {
            canvas.Clear();
            //print food
            canvas.SetPixel(food.x, food.y, new Color(0, 255, 0));
            foreach (snakeSegment ss in Snake) {
                if (ss.head)
                {
                    canvas.SetPixel(ss.x, ss.y, new Color(255, 0, 0));
                }
                else
                {
                    canvas.SetPixel(ss.x, ss.y, new Color(255, 255, 255));
                }
            }
            if (!started) {
                //print press start on screen
                canvas.DrawText(font, 7, 15, new Color(255, 255, 255), "Press \n Start");
            }
            canvas = matrix.SwapOnVsync(canvas);
        }

        internal void update(Controller controller)
        {
            //Console.WriteLine("snake part count:"+ Snake.Count);
            //Console.WriteLine("snake controller start:" + controller.start);
            if (controller.up == 1)
            {
                Snake[0].direction = direction.up;
            }
            else if (controller.down == 1)
            {
                Snake[0].direction = direction.down;
            }
            else if (controller.left == 1)
            {
                Snake[0].direction = direction.left;
            }
            else if (controller.right == 1)
            {
                Snake[0].direction = direction.right;
            }
            else if (controller.start == 1) {
                started = true;
            }
            if (dead)
            {
                Snake.RemoveAt(Snake.Count - 1);
                if (Snake.Count == 0) {
                    startNewGame();
                }
            }
            else
            {
                //Console.WriteLine("move snake");
                movesnake();
                checkForFood();
            }
        }
    }
}
