using System;
using System.Collections.Generic;
using System.Text;

class GateKey : Item
{
    private Gate gate;
    private Texture spriteMap;
    private bool held;
    private Rect collisionBox;
    private Vector2 pos;
    private Vector2 size;
    private Vector2 dir;
    private float r;

    public GateKey(String texturePath)
    {
        gate = null;
        spriteMap = Engine.LoadTexture(texturePath);
        this.dir = new Vector2(1, 0);
        this.size = new Vector2(12, 24);
        this.collisionBox = new Rect(new Range(pos.X, pos.X + size.X), new Range(pos.Y, pos.Y + size.Y));
        this.held = false;
        r = 90f;
    }
    public GateKey(Gate g , String texturePath)
    {
        gate = g;
        spriteMap = Engine.LoadTexture(texturePath);
        this.dir = new Vector2(1, 0);
        this.size = new Vector2(12, 24);
        this.collisionBox = new Rect(new Range(pos.X, pos.X + size.X), new Range(pos.Y, pos.Y + size.Y));
        this.held = false;
        r = 90f;
    }

    public void Move(Vector2 pos)
    {
        this.pos = pos;
        this.collisionBox = new Rect(new Range(pos.X, pos.X + size.X), new Range(pos.Y, pos.Y + size.Y));
    }
    public Vector2 GetSize()
    {
        if (dir.Equals(new Vector2(-1, 0)) || dir.Equals(new Vector2(1, 0)))
        {
            return new Vector2(size.Y, size.X);
        }
        return size;
    }
    public Gate getGate()
    {
        return gate;
    }
    public void Draw()
    {
        if (!IsHeld())
        {
            Engine.DrawTexture(spriteMap, pos, size: size, rotation: 0);
            return;
        }
        //Engine.DrawRectEmpty(collisionBox.ToBounds(), Color.Red);
        if (r == 0f || r == 180f)
        {
            Engine.DrawTexture(spriteMap, pos, size: size, rotation: r);
        }
        else
        {
            Engine.DrawTexture(spriteMap, new Vector2(pos.X + 6, pos.Y - size.Y / 4), size: size, rotation: r);
        }
    }
    public Rect CollisionZone()
    {
        return collisionBox;
    }
    public bool IsHeld()
    {
        return held;
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
            if ((dir - new Vector2(0, -1)).Equals(Vector2.Zero))
            {
                pos = new Vector2(playerPos.X + Game.PLAYER_SIZE.X / 2 - size.X / 2, playerPos.Y - size.Y);
                collisionBox = Rect.GetSpriteBounds(pos, size);
            }
            else if ((dir - new Vector2(-1, 0)).Equals(Vector2.Zero))
            {
                pos = new Vector2(playerPos.X - size.Y, playerPos.Y + Game.PLAYER_SIZE.Y / 2 - size.X / 2);
                collisionBox = Rect.GetSpriteBounds(pos, new Vector2(size.Y, size.X));
            }
            else if ((dir - new Vector2(0, 1)).Equals(Vector2.Zero))
            {
                pos = new Vector2(playerPos.X + Game.PLAYER_SIZE.X / 2 - size.X / 2, playerPos.Y + 24);
                collisionBox = Rect.GetSpriteBounds(pos, size);
            }
            else if ((dir - new Vector2(1, 0)).Equals(Vector2.Zero))
            {
                pos = new Vector2(playerPos.X + Game.PLAYER_SIZE.X, playerPos.Y + Game.PLAYER_SIZE.Y / 2 - size.X / 2);
                collisionBox = Rect.GetSpriteBounds(pos, new Vector2(size.Y, size.X));
            }
        }
    }
    public bool CheckGateIntersect(Room r)
    {
        if (held&& gate!=null)
        {
            if (gate.getRoom().Equals(r) && Rect.CheckRectIntersect(gate, collisionBox))
            {
                gate.isOpen = true;
            }
        }
        return false;
    }
    public bool Collides(Rect player)
    {
        if (Rect.CheckRectIntersect(player, collisionBox))
        {
            held = true;
            return true;
        }
        return false;
    }
    public void Drop()
    {
        this.held = false;
    }
    public void Pickup()
    {
        this.held = true;
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
        else if (Engine.GetKeyHeld(Key.D))
        {
            dir = new Vector2(1, 0);
            r = 90f;
        }
    }
}
