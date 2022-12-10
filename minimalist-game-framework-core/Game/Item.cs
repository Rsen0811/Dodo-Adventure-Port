using System;
using System.Collections.Generic;
using System.Text;

interface Item
{
    void move(Vector2 pos);
    void draw();

    Vector2 getSize();
    Rect collisionZone();

    bool isHeld();

    void Update(Rect Player);

    bool collides(Rect bounds);

    void drop();

    void pickup();

}
