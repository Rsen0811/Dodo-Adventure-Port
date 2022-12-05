using System;
using System.Collections.Generic;
using System.Text;

interface Item
{
    void draw();

    Rect collisionZone();

    bool isHeld();

    void Update();

    bool collides(Rect bounds);



}
