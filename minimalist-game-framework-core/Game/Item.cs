using System;
using System.Collections.Generic;
using System.Text;

interface Item
{
    void Draw();

    Rect CollisionZone();

    bool IsHeld();

    void Update(Rect Player);

    bool Collides(Rect bounds);

    void Drop();

    void Pickup();

}
