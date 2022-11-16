using System;
using System.Collections.Generic;
using System.Text;

class Room
{
    Vector2 PLAYER_SIZE = new Vector2(24, 24);
    List<Bounds2> CollisionZones;
    
    public Room()
    {
        CollisionZones = new List<Bounds2>();
        CollisionZones.Add(new Bounds2(new Vector2(100, 200), new Vector2(150, 200)));
    }

    public void Update()
    {
    }

    public Vector2 move(Vector2 start, Vector2 move)
    {
        Vector2 moveTo = start + move;
        Bounds2 playerBounds = getPlayerBounds(moveTo);
        

        foreach (Bounds2 rect in CollisionZones)
        { // position is actual x range and size is actually y range
            if (checkIntervalIntersect(rect.Position, playerBounds.Position)
             && checkIntervalIntersect(rect.Size, playerBounds.Size))
            { // doesnt handle corners
                Vector2 moveToX = start + new Vector2(move.X, 0);
                Vector2 moveToY = start + new Vector2(0, move.Y);
                
                return (!checkIntervalIntersect(rect.Position, playerBounds.Position) ? moveToY : moveToX);
            }
            else return moveTo;
        }

        return moveTo;
    }

    private bool checkIntervalIntersect(Vector2 barrier, Vector2 player)
    {
        if (player.X < barrier.Y && player.X > barrier.X) return true;
        if (player.Y < barrier.Y && player.Y > barrier.X) return true;    
        if (barrier.X < player.Y && barrier.X > player.X) return true;
        return false;
    }
    
    private Bounds2 getPlayerBounds(Vector2 moveTo)
    {
         return new Bounds2(new Vector2(moveTo.X, moveTo.X + PLAYER_SIZE.X),
                                    new Vector2(moveTo.Y, moveTo.Y + PLAYER_SIZE.Y));
    }

    public void drawRoom()
    {
        Engine.DrawRectSolid(new Bounds2(new Vector2(100, 150), new Vector2(100, 50)), Color.White);
    }

    public void addObject()
    {

    }

    public void removeObject()
    {

    }

    public void pickup()
    {

    }
}

