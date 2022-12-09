using System;
using System.Collections.Generic;
using System.Text;

interface Item
{
    void draw();

    Rect collisionZone();

    bool isHeld();

    void Update(Rect Player);

    bool collides(Rect bounds);

    void drop();

    void pickup();

}
