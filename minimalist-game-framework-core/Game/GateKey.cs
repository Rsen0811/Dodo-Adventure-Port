using System;
using System.Collections.Generic;
using System.Text;

class GateKey : Item
{
    private Gate gate;
    private Texture spriteMap;
    private Vector2 pos;
    private Vector2 dir;

    public GateKey()
    {

    }

    public void move(Vector2 pos)
    {

    }
    public Vector2 getSize()
    {
        return Vector2.Zero;
    }
    public Gate getGate()
    {
        return gate;
    }
    public void Draw()
    {

    }
    public Rect CollisionZone()
    {
        return null;
    }
    public bool IsHeld()
    {
        return false;
    }
    public void Update(Rect Player)

    {

    }
    public bool Collides(Rect Rect)
    {
        return false;
    }
    public void Drop()
    {

    }
    public void Pickup()
    {

    }
}
