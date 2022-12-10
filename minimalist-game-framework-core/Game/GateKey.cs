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
    public void draw()
    {

    }
    public Rect collisionZone()
    {
        return null;
    }
    public bool isHeld()
    {
        return false;
    }
    public void Update(Rect Player)

    {

    }
    public bool collides(Rect Rect)
    {
        return false;
    }
    public void drop()
    {

    }
    public void pickup()
    {

    }
}
