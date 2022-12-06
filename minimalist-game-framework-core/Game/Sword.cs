using System;
using System.Collections.Generic;
using System.Text;

class Sword : Item
{
    public bool isHoly;
    public int damage;
    private Texture spriteMap;
    private Vector2 pos;
    private Vector2 dir;


    public Sword()
    {

    }

    public void Update()
    {

    }
    public void draw()
    {

    }

    public bool isHeld()
    {
        return true;
    }

    public bool collides(Rect player)
    {
        return true;
    }

    public bool dodoCollides()
    {
        return true;
    }

    public Rect collisionZone()
    {
        return null;
    }
}
