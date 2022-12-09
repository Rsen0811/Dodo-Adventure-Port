using System;
using System.Collections.Generic;
using System.Text;

class Sword : Item
{
    Vector2 PLAYERSIZE =new Vector2(24, 24);
    public bool isHoly;
    public int damage;
    private Texture spriteMap;
    private Vector2 pos;
    private Vector2 dir;
    //size when sword is pointing up/down
    private Vector2 size;
    private Rect collisionBox;
    private bool held;
    public Sword(Vector2 pos, bool isHoly)
    {
        this.pos = pos;
        this.dir = new Vector2(1, 0);
        if (isHoly)
        {
            this.spriteMap = Engine.LoadTexture("textures/player.png");
            this.size = new Vector2(18,36);
            this.damage = 2;
        }
        else {
            this.spriteMap = Engine.LoadTexture("textures/player.png");
            this.size = new Vector2(12, 24);
            this.damage = 1;
        }
        this.isHoly = isHoly;
        this.held = false;
        this.collisionBox = new Rect(new Range(pos.X, pos.X + size.X),new Range(pos.Y, pos.Y + size.Y));
        
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
                pos = new Vector2(playerPos.X + PLAYERSIZE.X/2 - size.X / 2, playerPos.Y - size.Y);
                collisionBox = Rect.getSpriteBounds(pos, size);

            }
            else if ((dir - new Vector2(-1, 0)).Equals(Vector2.Zero))
            {
                pos = new Vector2(playerPos.X - size.Y, playerPos.Y + PLAYERSIZE.Y/2 - size.X / 2);
                collisionBox = Rect.getSpriteBounds(pos, new Vector2(size.Y, size.X));
            }
            else if ((dir - new Vector2(0, 1)).Equals(Vector2.Zero))
            {
                pos = new Vector2(playerPos.X + PLAYERSIZE.X/2 - size.X / 2, playerPos.Y + 24);
                collisionBox = Rect.getSpriteBounds(pos, size);
            }
            else if ((dir - new Vector2(1, 0)).Equals(Vector2.Zero))
            {
                pos = new Vector2(playerPos.X + PLAYERSIZE.X, playerPos.Y + PLAYERSIZE.Y/2 - size.X / 2);
                collisionBox = Rect.getSpriteBounds(pos, new Vector2(size.Y, size.X));
            }
        }
    }
    public void draw()
    {
        Engine.DrawRectEmpty(collisionBox.toBounds(), Color.Red);
    }

    public bool isHeld()
    {
        return held;
    }

    public bool collides(Rect player)
    {
        if(Rect.checkRectIntersect(player, collisionBox))
        {
            held = true;
            return true;
        }
        return false ;
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
    public void pickup()
    {
        this.held = true;
    }

}
