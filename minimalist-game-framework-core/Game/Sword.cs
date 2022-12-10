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
    private float r;
    public Sword(Vector2 pos, bool isHoly)
    {
        this.pos = pos;
        this.dir = new Vector2(1, 0);
        r = 90f;
        if (isHoly)
        {
            this.spriteMap = Engine.LoadTexture("textures/sword.png");
            this.size = new Vector2(18,36);
            this.damage = 2;
        }
        else {
            this.spriteMap = Engine.LoadTexture("textures/sword.png");
            this.size = new Vector2(12, 24);
            this.damage = 1;
        }
        this.isHoly = isHoly;
        this.held = false;
        this.collisionBox = new Rect(new Range(pos.X, pos.X + size.X),new Range(pos.Y, pos.Y + size.Y));
        
    }
    public void Drop()
    {
        held = false;
    }
    public void Move(Vector2 pos)
    {
        this.pos = pos;
        this.collisionBox = new Rect(new Range(pos.X, pos.X + size.X), new Range(pos.Y, pos.Y + size.Y));
    }
    public void Update(Rect Player)
    {
        Vector2 playerPos = new Vector2(Player.X.min, Player.Y.min);
        CollectInput(playerPos);
        if (!held)
        {
            if (Engine.GetKeyDown(Key.R))
            {
                Collides(Player);
            }
        }
        else
        {
            if ((dir-new Vector2(0, -1)).Equals(Vector2.Zero))
            {
                pos = new Vector2(playerPos.X + Game.PLAYER_SIZE.X/2 - size.X / 2, playerPos.Y - size.Y);
                collisionBox = Rect.GetSpriteBounds(pos, size);
            }
            else if ((dir - new Vector2(-1, 0)).Equals(Vector2.Zero))
            {
                pos = new Vector2(playerPos.X - size.Y, playerPos.Y + Game.PLAYER_SIZE.Y/2 - size.X / 2);
                collisionBox = Rect.GetSpriteBounds(pos, new Vector2(size.Y, size.X));
            }
            else if ((dir - new Vector2(0, 1)).Equals(Vector2.Zero))
            {
                pos = new Vector2(playerPos.X + Game.PLAYER_SIZE.X/2 - size.X / 2, playerPos.Y + 24);
                collisionBox = Rect.GetSpriteBounds(pos, size);
            }
            else if ((dir - new Vector2(1, 0)).Equals(Vector2.Zero))
            {
                pos = new Vector2(playerPos.X + Game.PLAYER_SIZE.X, playerPos.Y + Game.PLAYER_SIZE.Y/2 - size.X / 2);
                collisionBox = Rect.GetSpriteBounds(pos, new Vector2(size.Y, size.X));
            }
        }
    }
    public Vector2 GetSize()
    {
        if(dir.Equals(new Vector2(-1, 0))||dir.Equals(new Vector2(1, 0)))
        {
            return new Vector2(size.Y, size.X);
        }
        return size;
    }
    public void Draw()
    {
        if (!IsHeld())
        {
            Engine.DrawTexture(spriteMap, pos, size: size, rotation: 0);
            return;
        }
        Engine.DrawRectEmpty(collisionBox.ToBounds(), Color.Red);
        if(r == 0f || r == 180f)
        {
            Engine.DrawTexture(spriteMap, pos, size: size, rotation: r);
        } else
        {
            Engine.DrawTexture(spriteMap, new Vector2(pos.X + 6, pos.Y - size.Y / 4), size: size, rotation: r);
        }
    }

    public bool IsHeld()
    {
        return held;
    }

    public bool Collides(Rect player)
    {
        if(Rect.CheckRectIntersect(player, collisionBox))
        {
            held = true;
            return true;
        }
        return false ;
    }
    
    public Rect CollisionZone()
    {
        return collisionBox;
    }
    public void CollectInput(Vector2 playerPos)
    {
        if (Engine.GetKeyHeld(Key.W))
        {
            dir = new Vector2(0, -1);
            r = 0f;
        }
        else if (Engine.GetKeyHeld(Key.A))
        {
            dir = new Vector2(-1, 0);
            r = -90f;
        }
        else if (Engine.GetKeyHeld(Key.S))
        {
            dir = new Vector2(0, 1);
            r = 180f;
        }
        else if (Engine.GetKeyHeld(Key.D)) {
            dir = new Vector2(1, 0);
            r = 90f;
        }   
    }
    public void Pickup()
    {
        this.held = true;
    }

}
