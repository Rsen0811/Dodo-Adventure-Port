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
        CollisionZones.Add(new Bounds2(new Vector2(200, 250), new Vector2(100, 150)));
        CollisionZones.Add(new Bounds2(new Vector2(100, 200), new Vector2(150, 200)));
        CollisionZones.Add(new Bounds2(new Vector2(200, 250), new Vector2(200, 300)));
        CollisionZones.Add(new Bounds2(new Vector2(250, 350), new Vector2(200, 250)));
    }

    public void Update()
    {
    }

    public Vector2 move(Vector2 start, Vector2 move)
    {
        // why do calcs if none needed
        if (move.Equals(Vector2.Zero)) return start;

        Vector2 moveTo = start + move;
        Bounds2 playerBounds = getPlayerBounds(moveTo);


        foreach (Bounds2 collider in CollisionZones)
        { // position is actual x range and size is actually y range
            if (checkRectIntersect(collider, playerBounds))
            { // doesnt handle corners
                Vector2 moveToX = start + new Vector2(move.X, 0);
                Vector2 moveToY = start + new Vector2(0, move.Y);
                if (checkRectIntersect(collider, getPlayerBounds(moveToX)))
                {
                    moveTo.X = start.X;
                }
                if (checkRectIntersect(collider, getPlayerBounds(moveToY)))
                {
                    moveTo.Y = start.Y;
                }
            }
        }
        if (moveTo.Equals(start) && move.X == -move.Y) // bug only appears in y = -x motion
        {
            //if on corner, deflects of from it depending on which side the collsion is on            
            moveTo.Y += (move.X < 0) ? 1 : -1;
            
            // yes, its n^2, but only for one edgecase, for one frame
            // makes sure deflection doesnt noclip though other block
            foreach (Bounds2 otherBounds in CollisionZones)
            {
                if (checkRectIntersect(otherBounds, getPlayerBounds(moveTo))) return start;
            }
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

    private bool checkRectIntersect(Bounds2 rect, Bounds2 playerBounds)
    {
        return checkIntervalIntersect(rect.Position, playerBounds.Position)
             && checkIntervalIntersect(rect.Size, playerBounds.Size);
    }

    private Bounds2 getPlayerBounds(Vector2 moveTo)
    { // a -1 makes the boundaries even
         return new Bounds2(new Vector2(moveTo.X-1, moveTo.X + PLAYER_SIZE.X),
                                    new Vector2(moveTo.Y-1, moveTo.Y + PLAYER_SIZE.Y));
    }

    public void drawRoom()
    {
        Engine.DrawRectSolid(new Bounds2(new Vector2(200, 100), new Vector2(50, 50)), Color.White);
        Engine.DrawRectSolid(new Bounds2(new Vector2(100, 150), new Vector2(100, 50)), Color.White);
        Engine.DrawRectSolid(new Bounds2(new Vector2(200, 200), new Vector2(50, 100)), Color.White);
        Engine.DrawRectSolid(new Bounds2(new Vector2(250, 200), new Vector2(100, 50)), Color.White);
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

