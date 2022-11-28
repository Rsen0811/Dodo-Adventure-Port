using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

class Room
{
    Vector2 PLAYER_SIZE = new Vector2(24, 24);
    List<Bounds2> CollisionZones;
    Texture bg;
    String name;

    public Room(Vector2 pos) { 
        name = "" + pos.X + pos.Y;
        CollisionZones = readCollisionZones("rooms/" + name + "/" + name + "c.txt");
        bg = Engine.LoadTexture("rooms/" + name + "/" + name + "i.png");
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
        return new Bounds2(new Vector2(moveTo.X, moveTo.X + PLAYER_SIZE.X),
                                   new Vector2(moveTo.Y, moveTo.Y + PLAYER_SIZE.Y));
    }

    public void drawRoom()
    {
        Engine.DrawTexture(bg, new Vector2(0, 0));
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

    //read rectangle collisions from a text file
    public List<Bounds2> readCollisionZones(String file)
    {
        List<Bounds2> loader = new List<Bounds2>();

        using (StreamReader sr = File.OpenText("Assets/" + file))
        {
            string s;
            while ((s = sr.ReadLine()) != null)
            {
                String[] nums = s.Split(' ');

                loader.Add(new Bounds2(new Vector2(float.Parse(nums[0]) , float.Parse(nums[1])),
                                                new Vector2(float.Parse(nums[2]), float.Parse(nums[3]))));
            }
        }
        return loader;
    }
}

