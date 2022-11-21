using System;
using System.Collections.Generic;
using System.Text;
class Direction
{
    public static readonly int LEFT = 0;
    public static readonly int  RIGHT = 1;
    public static readonly int DOWN = 2;
    public static readonly int UP = 3;
    public static int reverse(int dir)
    {
        if (dir == LEFT)
        {
            return RIGHT;
        }
        if (dir == RIGHT)
        {
            return LEFT;
        }
        if (dir == UP)
        {
            return DOWN;
        }
        return UP;  
    }
}

