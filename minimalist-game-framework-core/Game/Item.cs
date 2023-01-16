using System;
using System.Collections.Generic;
using System.Text;

interface Item
{

    void Move(Vector2 pos);
    void Draw();

    Vector2 GetSize();
    
    Rect CollisionZone();


    bool IsHeld();

    void Update(Rect Player, Vector2 pos);

    bool Collides(Rect bounds);

    void Drop();

    void Pickup();
}
