using System;
using System.Collections.Generic;
using System.Text;

class Sword : Item
{
    private readonly int PICKUP = 40;
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


    private Vector2 swordCenterPos;
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
    public void Update(Rect Player, Vector2 itemDirection)
    {
        swordCenterPos = new Vector2(pos.X + size.X / 2, pos.Y + size.Y / 2); // debug
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
        // debug
        Engine.DrawRectEmpty(new Bounds2(swordCenterPos.X - PICKUP, swordCenterPos.Y - PICKUP, PICKUP * 2, PICKUP * 2), Color.Green); // debug
        if (!IsHeld())
        {
            Engine.DrawTexture(spriteMap, pos, size: size, rotation: 0);
            
            return;
        }
        //Engine.DrawRectEmpty(collisionBox.ToBounds(), Color.Red);
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
        Vector2 playerCenterPos = new Vector2(player.X.min + Game.PLAYER_SIZE.X / 2, player.Y.min + Game.PLAYER_SIZE.Y / 2);
        Vector2 swordCenterPos = new Vector2(pos.X + this.GetSize().X, pos.Y + this.GetSize().Y);
        Bounds2 swordCollider = new Bounds2((playerCenterPos - new Vector2(PICKUP, PICKUP) / 2),
                                new Vector2(PICKUP, PICKUP));
        if (Math.Abs((playerCenterPos - swordCenterPos).Length()) < PICKUP)
        {
            held = true;
            //Engine.DrawRectEmpty(swordCollider, Color.Red);
            return true;
        }

        return false;
    }
    
    public Rect CollisionZone()
    {
        return collisionBox;
    }
    public void Pickup()
    {
        this.held = true;
    }
}
