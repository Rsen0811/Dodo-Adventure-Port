using System;
using System.Collections.Generic;
using System.Text;

class Player
{
    Vector2 PLAYER_SIZE = new Vector2(24, 24);
    Vector2 pos;
    bool active;
    Item holding;
    Vector2 checkpointRoom;
    Vector2 checkpointPos;
    readonly int PLAYER_SPEED = 400;
    Texture player = Engine.LoadTexture("textures/player.png");
    Room currRoom;
    public Player(Vector2 position, Vector2 room)
    {
        active = true;
        pos = position;
        checkpointPos = position;
        checkpointRoom = room;
    }
    public void Update()
    {
        if (Engine.GetKeyDown(Key.X))
        {
            drop();
        }
        else if (holding != null)
        {
            
            holding.Update(Rect.getSpriteBounds(pos, PLAYER_SIZE));
        }
    }
    public bool move(Vector2 moveVector, Room currRoom = null)
    {
        bool absolute = (currRoom == null);

        if (!active || moveVector.Equals(Vector2.Zero)) return false;
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
        if (holding != null)
        {
            holding.drop();
            currRoom.addObject(holding);
        }
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

    public void getEaten()
    {
        active = false;
    }
    public Vector2 respawn()
    {
        pos = checkpointPos;
        return checkpointRoom;

    }
}


