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
    //size when sword is pointing up/down
    private Vector2 size;
    private Rect collisionBox;
    private bool held;
    public Sword(String texturePath, Vector2 pos, bool isHoly, Vector2 size, int damage)
    {
        this.spriteMap = Engine.LoadTexture(texturePath);
        this.pos = pos;
        this.dir = new Vector2(1, 0);
        this.size = size;
        this.isHoly = isHoly;
        this.held = false;
        this.collisionBox = new Rect(new Range(pos.X, pos.X + size.X),new Range(pos.Y, pos.Y + size.Y));
        this.damage = damage;
    }
    public void drop()
    {
        held = false;
    }
    public void Update(Rect Player)
    {
        Vector2 playerPos = new Vector2(Player.X.min, Player.Y.min);
        collectInput(playerPos);
        if (!held)
        {
            collides(Player);
        }
        else
        {
            if ((dir-new Vector2(0, -1)).Equals(Vector2.Zero))
            {
                pos = new Vector2(playerPos.X + 12 - size.X / 2, playerPos.Y + size.Y);
                collisionBox = Rect.GetSpriteBounds(pos, size);

            }
            else if ((dir - new Vector2(-1, 0)).Equals(Vector2.Zero))
            {
                pos = new Vector2(playerPos.X - size.Y, playerPos.Y + 12 - size.X / 2);
                collisionBox = Rect.GetSpriteBounds(pos, new Vector2(size.Y, size.X));
            }
            else if ((dir - new Vector2(0, 1)).Equals(Vector2.Zero))
            {
                pos = new Vector2(playerPos.X + 12 - size.X / 2, playerPos.Y + 24);
                collisionBox = Rect.GetSpriteBounds(pos, size);
            }
            else if ((dir - new Vector2(1, 0)).Equals(Vector2.Zero))
            {
                pos = new Vector2(playerPos.X + 24, playerPos.Y + 12 - size.X / 2);
                collisionBox = Rect.GetSpriteBounds(pos, new Vector2(size.Y, size.X));
            }
        }
    }
    public void draw()
    {

    }

    public bool isHeld()
    {
        return held;
    }

    public bool collides(Rect player)
    {
        if(checkRectIntersect(player, collisionBox))
        {
            held = true;
            return true;
        }
        return false ;
    }
    private bool checkIntervalIntersect(Range barrier, Range player)
    {
        if (player.min < barrier.max && player.min > barrier.min) return true;
        if (player.max < barrier.max && player.max > barrier.min) return true;
        if (barrier.min < player.max && barrier.min > player.max) return true;
        return false;
    }

    private bool checkRectIntersect(Rect rect, Rect playerBounds)
    {
        return checkIntervalIntersect(rect.X, playerBounds.X)
             && checkIntervalIntersect(rect.Y, playerBounds.Y);
    }
    public Rect collisionZone()
    {
        return collisionBox;
    }
    public void collectInput(Vector2 playerPos)
    {
        if (Engine.GetKeyHeld(Key.W))
        {
            dir = new Vector2(0, -1);
        }
        else if (Engine.GetKeyHeld(Key.A))
        {
            dir = new Vector2(-1, 0);
        }
        else if (Engine.GetKeyHeld(Key.S))
        {
            dir = new Vector2(0, 1);
        }
        else if (Engine.GetKeyHeld(Key.D)) {
            dir = new Vector2(1, 0);
        }   
    }

}
