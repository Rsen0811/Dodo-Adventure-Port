using System;
using System.Collections.Generic;
using System.Text;

class Player
{
    Vector2 pos;
    bool active;
    Item holding;
    Vector2 checkpointRoom;
    Vector2 checkpointPos;
    readonly int PLAYER_SPEED = 400;
    Texture player = Engine.LoadTexture("textures/player.png");

    public Player(Vector2 position, Vector2 room)
    {
        active = true;
        pos = position;
        checkpointPos = position;
        checkpointRoom = room;
    }
    public bool move(Vector2 moveVector, Room currRoom = null)
    {
        bool absolute = (currRoom == null);

        if (!active) return false;
        if (absolute)
        {
            pos = moveVector;
            return true;
        }
        Vector2 moveDir = moveVector.Normalized() * (PLAYER_SPEED * Engine.TimeDelta);
        pos = currRoom.move(pos, moveDir);

        return true;
    }
    public Item pickup()
    {
        return null;
    }
    public Item drop()
    {
        return null;
    }

    public void drawPlayer()
    {
        Engine.DrawTexture(player, pos, size: new Vector2(24, 24));
    }

    public Vector2 position()
    {
        return pos;
    }

    public Vector2 respawn()
    {
        pos = checkpointPos;
        return checkpointRoom;
    }
}

class Item {

}
