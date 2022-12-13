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
    Room currRoom;
    Random random = new Random();

    float reboundTimer = 0;
    float reboundSpeed;
    Vector2 reboundDir;
    float deathTimer = -1;
    float respawnTimer = 1;
    float deathHits = 0;

    bool gameOver = false;

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
        if (Engine.GetKeyDown(Key.X))
        {
            Drop();
        }
        else if (holding != null)
        {            
            holding.Update(Rect.GetSpriteBounds(pos, PLAYER_SIZE));
        }
    }
    public bool Move(Vector2 moveVector, Room currRoom = null)
    {
        bool absolute = (currRoom == null);
        if ((moveVector.Equals(Vector2.Zero) && reboundTimer <= 0) || !alive || !active)
        {
            if(respawnTimer < 1)
            {
                respawnTimer -= Engine.TimeDelta;
                if (respawnTimer <= 0)
                {
                    gameOver = true;
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
        if(reboundTimer > 0)
        {
            moveDir += reboundDir * (reboundSpeed * Engine.TimeDelta);
            reboundSpeed -= reboundSpeed / 15f;
            reboundTimer -= Engine.TimeDelta;
        }
        pos = currRoom.Move(pos, moveDir);

        return true;
    }
    public void Pickup(Item i)
    {
        if (holding != null)
        {
            this.Drop();
        }
        holding = i;
    }
    public void Drop()
    {

        if (holding != null)
        {
            holding.Drop();
            Room dropRoom = this.wrap();
            dropRoom.AddObject(holding);
            holding = null;
        }
    }
    public Room wrap()
    {
        Vector2 pos = new Vector2(holding.CollisionZone().X.min,holding.CollisionZone().Y.min);
        Vector2 tempRoom = currRoom.Position();
        
        if (pos.X > Game.Resolution.X)
        {
            holding.Move(new Vector2(Math.Abs(pos.X % Game.Resolution.X), Math.Abs(pos.Y % Game.Resolution.Y)));
            tempRoom.X++;
        }
        //doesnt work
        if (pos.X + holding.GetSize().X < 0)
        {
            holding.Move(new Vector2(Game.Resolution.X-Math.Abs(pos.X), Math.Abs(pos.Y % Game.Resolution.Y)));
            tempRoom.X--;
        }
        if (pos.Y > Game.Resolution.Y)
        {
            holding.Move(new Vector2(Math.Abs(pos.X % Game.Resolution.X), Math.Abs(pos.Y % Game.Resolution.Y)));
            tempRoom.Y++;
            
        }
        //doesnt work
        if (pos.Y + holding.GetSize().Y < 0)
        {
            holding.Move(new Vector2(Math.Abs(pos.X % Game.Resolution.X), Game.Resolution.Y-Math.Abs(pos.Y)));
            tempRoom.Y--;
        }
        
        return Game.getRoom(tempRoom);
    }

    public void DrawPlayer()
    {
        Engine.DrawTexture(player, pos, size: new Vector2(24, 24));
        if (holding != null)
        {
            holding.Draw();
        }
    }
    public void ChangeRoom(Room room)
    {
        currRoom = room;
    }

    public Item GetItem()
    {
        return holding;
    }

    public Vector2 Position()
    {
        return pos;
    }

    public void GetEaten(Vector2 deathPos)
    {
        Drop();
        if (deathTimer == 1.5f)
        {
            deathHits = random.Next(13, 25);
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
            Die(deathPos);
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
    public Vector2 Respawn()
    {
        pos = checkpointPos;
        alive = true;
        active = true;
        deathTimer = -1;
        respawnTimer = 1;
        deathHits = 0;
        return checkpointRoom;
    }

    public void Die(Vector2 deathPos)
    {
        alive = false;
        pos = deathPos;
        respawnTimer -= Engine.TimeDelta;
    }

    public bool IsAlive()
    {
        return alive;
    }

    public bool GameOver()
    {
        return gameOver;
    }

    public Vector2 Rebound(Vector2 dodoPos)
    {
        reboundDir = (new Vector2((pos.X + PLAYER_SIZE.X / 2) - (dodoPos.X + Dodo.GetSize().X / 2), 
            (pos.Y + PLAYER_SIZE.Y / 2) - (dodoPos.Y + Dodo.GetSize().Y / 2))).Normalized();
        reboundTimer = 0.7f;
        reboundSpeed = PLAYER_SPEED * 2f;
        return reboundDir;
    }

    public bool isActive()
    {
        return active;
    }
    public void setActive()
    {
        active = true;
    }

    public Rect getPlayerBounds()
    {
        return Rect.GetSpriteBounds(pos, PLAYER_SIZE);
    }
}


