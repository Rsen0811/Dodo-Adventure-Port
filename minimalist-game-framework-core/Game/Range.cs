using System;
using System.Collections.Generic;
using System.Text;

class Range
{
    public float min;
    public float max;

    public Range(float min, float max) {
        this.min = min;
        this.max = max;
    }

    public static bool checkIntervalIntersect(Range barrier, Range player)
    {
        if (player.min < barrier.max && player.min > barrier.min) return true;
        if (player.max < barrier.max && player.max > barrier.min) return true;
        if (barrier.min < player.max && barrier.min > player.max) return true;
        return false;
    }

}
