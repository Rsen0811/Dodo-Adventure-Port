using System;
using System.Collections.Generic;
using System.Text;

class GateKey : Item
{
    private String gateName;
    private Texture spriteMap;
    private bool held;
    private Rect collisionBox;
    private Vector2 pos;
    private Vector2 size;
    private Vector2 dir;
    private float r;

    public GateKey()
    {

    }
    public GateKey(String gateName,Vector2 pos)
    {
        this.gateName = gateName;
        spriteMap = Engine.LoadTexture("textures/keys/"+ gateName +".png");
        this.dir = new Vector2(1, 0);
        this.size = new Vector2(12, 24);
        this.collisionBox = new Rect(new Range(pos.X, pos.X + size.X), new Range(pos.Y, pos.Y + size.Y));
        this.held = false;
        this.pos = pos;
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
    public String getGate()
    {
        return gateName;
    }
    public void Draw()
    {
        if (!IsHeld())
        {
            Engine.DrawTexture(spriteMap, pos, size: size, rotation: 0);
            return;
        }
        //Engine.DrawRectEmpty(collisionBox.ToBounds(), Color.Red);
        if (dir.Y != 0)
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
    public void Update(Rect Player, Vector2 itemDirection)
    {
        Vector2 playerPos = new Vector2(Player.X.min, Player.Y.min);
        dir = itemDirection;
        r = 90.0f * dir.X + ((dir.Y == 1) ? 180.0f : 0);
        if (!held)
        {
            if (Engine.GetKeyDown(Key.Space))
            {
                Collides(Player);
            }
        }
        else
        {
            if (dir.Equals(new Vector2(0, -1)))
            {
                pos = new Vector2(playerPos.X + Game.PLAYER_SIZE.X / 2 - size.X / 2, playerPos.Y - size.Y);
                collisionBox = Rect.GetSpriteBounds(pos, size);
            }
            else if (dir.Equals(new Vector2(-1, 0)))
            {
                pos = new Vector2(playerPos.X - size.Y, playerPos.Y + Game.PLAYER_SIZE.Y / 2 - size.X / 2 + 4);
                collisionBox = Rect.GetSpriteBounds(pos, new Vector2(size.Y, size.X));
            }
            else if (dir.Equals(new Vector2(0, 1)))
            {
                pos = new Vector2(playerPos.X + Game.PLAYER_SIZE.X / 2 - size.X / 2, playerPos.Y + 24);
                collisionBox = Rect.GetSpriteBounds(pos, size);
            }
            else if (dir.Equals(new Vector2(1, 0)))
            {
                pos = new Vector2(playerPos.X + Game.PLAYER_SIZE.X, playerPos.Y + Game.PLAYER_SIZE.Y / 2 - size.X / 2 + 4);
                collisionBox = Rect.GetSpriteBounds(pos, new Vector2(size.Y, size.X));
            }
        }
    }
    public bool CheckGateIntersect(Gate gate)
    {
        if (held)
        {
            if (gate.getName().Equals(gateName) && Rect.CheckRectIntersect(gate, collisionBox))
            {
                gate.isOpen = true;
            }
        }
        return false;
    }
    public bool Collides(Rect player)
    {
        int radius = 100;
        Vector2 playerCenter = new Vector2((player.X.min+player.X.max)/2, (player.Y.min + player.Y.max) / 2);
        Vector2 keyCenter = pos;
        if (Math.Sqrt(Math.Pow((playerCenter.X - keyCenter.X), 2) + Math.Pow((playerCenter.Y - keyCenter.Y), 2)) <= radius)
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
    public Vector2 getDir()
    {
        return dir;
    }

    public Vector2 GetPos()
    {
        return pos;
    }
}
