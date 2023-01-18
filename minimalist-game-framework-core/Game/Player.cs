using System;
using System.Collections.Generic;
using System.Text;

class Player
{
    Vector2 PLAYER_SIZE = new Vector2(24, 24);
    Vector2 TEXTURE_OFFSET = new Vector2(-12, -16);
    Vector2 pos;
    bool active;
    bool alive;
    public static bool VICTORY;
    Item holding;
    private Vector2 facing;
    Vector2 checkpointRoom;
    Vector2 checkpointPos;
    readonly int PLAYER_SPEED = 400;
    Texture player;
    Room currRoom;
    Random random = new Random();

    private readonly int maxDeathHits; // must be greater than 8

    float reboundTimer = 0;
    float reboundSpeed;
    Vector2 reboundDir;
    float deathTimer = -1;
    float bossDeathTimer = 2;
    float respawnTimer = 1;
    float deathHits = 0;

    bool spaceDown = false;
    bool gameOver = false;

    int frameCounter;
    bool walking;

    public Player(Vector2 position, Vector2 room, int maxDeathHits = 15)
    {
        active = true;
        alive = true;
        pos = position;
        checkpointPos = position;
        checkpointRoom = room;
        this.maxDeathHits = maxDeathHits;
        facing = new Vector2(0, 1);
        player = Shop.GetPlayerTexture();
        frameCounter = 0;
        walking = false;
    }
    public void Update()
    {
        Vector2 input = getFacingDirection();
        facing = (input.Equals(Vector2.Zero)) ? facing : input;

        if (Engine.GetKeyDown(Key.Space))
        {
            if (!spaceDown)
            {
                Switch s = currRoom.checkSwitchIntersect(Rect.GetSpriteBounds(pos, PLAYER_SIZE));
                if (s != null)
                {
                    s.Toggle();
                }
                else
                {
                    Drop();
                }
                spaceDown = true;
            }
        }
        else if (holding != null)
        {
            holding.Update(Rect.GetSpriteBounds(pos, PLAYER_SIZE), facing);
        }
        if (Engine.GetKeyUp(Key.Space))
        {
            spaceDown = false;
        }
    }
    public bool Move(Vector2 moveVector, Room currRoom = null)
    {
        walking = !moveVector.Equals(Vector2.Zero);

        bool absolute = (currRoom == null);
        if ((moveVector.Equals(Vector2.Zero) && reboundTimer <= 0) || !alive || !active)
        {
            if (respawnTimer < 1)
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
        if (reboundTimer > 0)
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
        spaceDown = true;
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
        Vector2 pos = new Vector2(holding.CollisionZone().X.min, holding.CollisionZone().Y.min);
        Vector2 tempRoom = currRoom.Position();

        if (pos.X > Game.Resolution.X)
        {
            holding.Move(new Vector2(Math.Abs(pos.X % Game.Resolution.X), Math.Abs(pos.Y % Game.Resolution.Y)));
            tempRoom.X++;
        }
        //doesnt work
        if (pos.X + holding.GetSize().X < 0)
        {
            holding.Move(new Vector2(Game.Resolution.X - Math.Abs(pos.X), Math.Abs(pos.Y % Game.Resolution.Y)));
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
            holding.Move(new Vector2(Math.Abs(pos.X % Game.Resolution.X), Game.Resolution.Y - Math.Abs(pos.Y)));
            tempRoom.Y--;
        }
        return Game.getRoom(tempRoom);
    }

    public void DrawPlayer()
    {
        frameCounter++;

        Engine.DrawRectEmpty(getPlayerBounds().ToBounds(), Color.Orange);
        // framecounter/8 gives 4fps
        Bounds2 playerBounds = new Bounds2((frameCounter/8)%4 * 48,  // animation frame
                                            ((facing.Y == 0) ? 0 : (72 + facing.Y * 24)) // facing
                                            + ((walking) ? 144 : 0), // walking
                                            48, 48); // size
        TextureMirror playerMirror = (facing.X > 0) ? TextureMirror.Horizontal : TextureMirror.None;


        Engine.DrawTexture(player, pos + TEXTURE_OFFSET, source: playerBounds, mirror: playerMirror);

        if (holding != null && facing.Y != -1)
        {
            Engine.DrawRectEmpty(holding.CollisionZone().ToBounds(), Color.Red);
            holding.Draw();
        }
        Engine.DrawRectEmpty(getPlayerBounds().ToBounds(), Color.Orange);
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
            deathHits = random.Next(maxDeathHits - 8, maxDeathHits);
        }
        if (Engine.GetKeyDown(Key.W) || Engine.GetKeyDown(Key.S) || Engine.GetKeyDown(Key.D) ||
            Engine.GetKeyDown(Key.A))
        {
            deathHits--;
            pos = currRoom.Move(pos, new Vector2(random.Next(-2, 3), random.Next(-2, 3)));
        }
        if (deathTimer > 0)
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
        else if (deathHits > 0)
        {
            Die(deathPos);
        }
        else
        {
            active = true;
        }
    }
    public void GetBossEaten(Vector2 beakPos, Vector2 deathPos)
    {
        if (bossDeathTimer > 0) pos = beakPos;
        else Die(deathPos);
        bossDeathTimer -= Engine.TimeDelta;
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

    public void Win()
    {
        alive = false;
        respawnTimer -= Engine.TimeDelta;
        victory = true;
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

    public void BossRebound(Vector2 bossPos)
    {
        reboundDir = (new Vector2((pos.X + PLAYER_SIZE.X / 2) - (bossPos.X + Boss.Size().X / 2),
            (pos.Y + PLAYER_SIZE.Y / 2) - (bossPos.Y + Boss.Size().Y / 2))).Normalized();
        reboundTimer = 0.7f;
        reboundSpeed = PLAYER_SPEED * 4f;
    }
    public void ProjectileRebound(Vector2 projectilePos)
    {
        reboundDir = (new Vector2((pos.X + PLAYER_SIZE.X / 2) - (projectilePos.X + Projectile.Size().X / 2),
            (pos.Y + PLAYER_SIZE.Y / 2) - (projectilePos.Y + Projectile.Size().Y / 2))).Normalized();
        reboundTimer = 0.7f;
        reboundSpeed = PLAYER_SPEED * 1.5f;
    }

    public bool isActive()
    {
        return active;
    }

    public bool IsAlive()
    {
        return alive;
    }
    public void setActive()
    {
        active = true;
    }
    public void DeleteItem()
    {
        holding = null;
    }
    public Rect getPlayerBounds()
    {
        return Rect.GetSpriteBounds(pos, PLAYER_SIZE);
    }

    public Vector2 getFacingDirection()
    {
        if (Engine.GetKeyHeld(Key.Up))
        {
            return new Vector2(0, -1);
        }
        if (Engine.GetKeyHeld(Key.Left))
        {
            return new Vector2(-1, 0);
        }
        if (Engine.GetKeyHeld(Key.Down))
        {
            return new Vector2(0, 1);
        }
        if (Engine.GetKeyHeld(Key.Right))
        {
            return new Vector2(1, 0);
        }
        return Vector2.Zero;
    }
}


