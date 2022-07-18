using System;
using System.Collections.Generic;
using System.Text;

namespace CHIP
{
    enum direction { 
        up,
        down,
        left,
        right
    }
    class snakeSegment
    {
        public bool head = false;//tail-0 head-1
        snakeSegment following;
        public direction direction;
        public int x;
        public int y;
        public bool moving = false;
        public snakeSegment(int x, int y, bool head, direction direction, snakeSegment following)
        {
            this.x = x;
            this.y = y;
            this.head = head;
            this.direction = direction;
            this.following = following;
        }
        public snakeSegment(int x, int y, bool head, direction direction)
        {
            this.x = x;
            this.y = y;
            this.head = head;
            this.direction = direction;
        }
        internal void update()
        {
            if (moving)
            {
                switch (direction)
                {
                    case direction.up: y++; break;
                    case direction.down: y--; break;
                    case direction.left: x--; break;
                    case direction.right: x++; break;
                }
            }
            if (!head) {
                direction = following.direction;
                if ((x == following.x) && (y == following.y))
                {
                    moving = true;
                }
            }
        }

        




    }
}
