using System;
using System.Collections.Generic;
using System.Text;

interface Item
{

    void move(Vector2 pos);
    void draw();

    Vector2 getSize();
    
    Rect collisionZone();


    bool IsHeld();

    void Update(Rect Player);

    bool Collides(Rect bounds);

    void Drop();

    void Pickup();

}
