using System;
using System.Collections.Generic;
using System.Text;

class Player
{
    Vector2 PLAYER_SIZE = new Vector2(24, 24);
    Vector2 pos;
    bool active;
    bool alive;
    Item holding;
    Vector2 checkpointRoom;
    Vector2 checkpointPos;
    readonly int PLAYER_SPEED = 400;
    Texture player = Engine.LoadTexture("textures/player.png");
    
    Random random = new Random();

    float deathTimer = -1;
    float respawnTimer = 1;
    float deathHits = 0;

    public Player(Vector2 position, Vector2 room)
    {
        active = true;
        alive = true;
        pos = position;
        checkpointPos = position;
        checkpointRoom = room;
    }
    public void Update()
    {
        if (holding != null)
        {
            holding.Update(Rect.getSpriteBounds(pos, PLAYER_SIZE));
        }
    }
    public bool move(Vector2 moveVector, Room currRoom = null)
    {
        bool absolute = (currRoom == null);
        if (!alive || !active || moveVector.Equals(Vector2.Zero))
        {
            if(respawnTimer < 1)
            {
                respawnTimer -= Engine.TimeDelta;
                if (respawnTimer <= 0)
                {
                    respawn();
                }
            }
            return false;
        }
        if (deathTimer > -1) deathTimer -= Engine.TimeDelta;
        if (absolute)
        {
            pos = moveVector;
            return true;
        }
        Vector2 moveDir = moveVector.Normalized() * (PLAYER_SPEED * Engine.TimeDelta);
        pos = currRoom.move(pos, moveDir);

        return true;
    }
    public void pickup(Item i)
    {
        if (holding != null)
        {
            this.drop();
        }
        holding = i;
    }
    public void drop()
    {
        holding.drop();
        currRoom.addObject(holding);
    }

    public void drawPlayer()
    {
        Engine.DrawTexture(player, pos, size: new Vector2(24, 24));
        if (holding != null)
        {
            holding.draw();
        }
    }
    public void changeRoom(Room room)
    {
        currRoom = room;
    }

    public Vector2 position()
    {
        return pos;
    }

    public void getEaten(Vector2 deathPos)
    {
        if (deathTimer == 1.5f)
        {
            deathHits = random.Next(15, 20);
        }
        if (Engine.GetKeyDown(Key.Space))
        {
            deathHits -= 2;
        }
        if(Engine.GetKeyDown(Key.Up) || Engine.GetKeyDown(Key.Down) || Engine.GetKeyDown(Key.Right) || 
            Engine.GetKeyDown(Key.Left))
        {
            deathHits--;
        }
        if(deathTimer > 0)
        {
            if (deathHits > 0)
            {
                active = false;
                deathTimer -= Engine.TimeDelta;
            }
            else if (deathHits <= 0)
            {
                active = true;
            }
        }
        else if(deathHits > 0)
        {
            die(deathPos);
        }
        else
        {
            active = true;
        }
    }

    public void SetDeathTimer(float deathTimer)
    {
        this.deathTimer = deathTimer;
    }
    public Vector2 respawn()
    {
        pos = checkpointPos;
        alive = true;
        active = true;
        deathTimer = -1;
        respawnTimer = 1;
        deathHits = 0;
        return checkpointRoom;
    }

    public void die(Vector2 deathPos)
    {
        alive = false;
        pos = deathPos;
        respawnTimer -= Engine.TimeDelta;
    }

    public bool isAlive()
    {
        return alive;
    }
}


