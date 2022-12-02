using System;
using System.Collections.Generic;
using System.Text;

class Player
{
    Vector2 pos;
    bool active;
    Item holding;
    Vector2 currRoom;
    readonly int SPEED = 100;
    Texture player = Engine.LoadTexture("textures/player.png");

    public Player(Vector2 position, Vector2 spawn)
    {
        pos = position;
        currRoom = spawn;
    }
    public Vector2 move(Vector2 newPos)
    {
        if (active)
        {
            pos = newPos;
        }
        return newPos;
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

    public Vector2 Pos()
    {
        return pos;
    }

    public void getEaten()
    {
        active = false;
    }
}

class Item {

}
