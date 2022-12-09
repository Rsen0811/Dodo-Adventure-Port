﻿using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

class Room
{
    readonly Vector2 PLAYER_SIZE = new Vector2(24, 24);
    List<Rect> CollisionZones;
    Texture bg;
    Vector2 pos;
    List<Dodo> enemies;
    List<Item> items;

    public Room(Vector2 pos) {
        String name = "" + pos.X + pos.Y;
        CollisionZones = readCollisionZones("rooms/" + name + "/" + name + "c.txt");
        bg = Engine.LoadTexture("rooms/" + name + "/" + name + "i.png");
        
        enemies = new List<Dodo>();
        enemies.Add(new Dodo(new Vector2(200, 200)));

        items = new List<Item>();
        items.Add(new Sword(new Vector2(100,100),false));

        this.pos = pos;
    }

    public Vector2 position()
    {
        return pos;
    }

    public void testaddDodo()
    {
        enemies.Add(new Dodo(new Vector2(200, 200)));
    }
    public void Update(Player p)
    {
        foreach (Dodo d in enemies)
        {
            d.Update(p.position(), 960);
        }
        List<Item> toRemove = new List<Item>();
        foreach (Item i in items)
        {
            i.Update(Rect.getSpriteBounds(p.position(), PLAYER_SIZE));
            if (i.isHeld())
            {
                toRemove.Add(i);
            }
        }
        foreach(Item i in toRemove)
        {
            this.pickup(p,i);
        }
    }

    public void idle()
    {
        foreach (Dodo d in enemies)
        {
            d.Idle();
        }
    }

    public Vector2 move(Vector2 start, Vector2 movement)
    {
        // why do calcs if none needed
        if (movement.Equals(Vector2.Zero)) return start;

        Vector2 moveTo = start + movement;
        Rect playerBounds = Rect.getSpriteBounds(moveTo, PLAYER_SIZE);


        foreach (Rect collider in CollisionZones)
        { // position is actual x range and size is actually y range
            if (checkRectIntersect(collider, playerBounds))
            { // does handle corners
                Vector2 moveToY = start + new Vector2(0, movement.Y);
                Rect playerBoundsY = Rect.getSpriteBounds(moveToY, PLAYER_SIZE);
                Vector2 moveToX = start + new Vector2(movement.X, 0);
                Rect playerBoundsX = Rect.getSpriteBounds(moveToX, PLAYER_SIZE);
                if (!checkRectIntersect(collider, playerBoundsY) && !checkRectIntersect(collider, playerBoundsX))
                {
                    //check just x and just y and which ever moves farther is the one we use
                    Vector2 Xmove= move(start, new Vector2(movement.X, 0));
                    float XmoveLength = (Xmove - start).Length();
                    Vector2 Ymove = move(start, new Vector2(0, movement.Y));
                    float YmoveLength = (Ymove - start).Length();
                    return XmoveLength > YmoveLength ? Xmove : Ymove;
                }
                else
                {
                    if (checkRectIntersect(collider, playerBoundsX))
                    {
                        //need to check if you are to the right or to the left
                        //if player to the right of the wall
                        if (collider.X.max<= Rect.getSpriteBounds(start, PLAYER_SIZE).X.min)
                        {
                            moveTo.X = collider.X.max;
                        }
                        //if player is to the left of the wall
                        else
                        {
                            moveTo.X = collider.X.min - PLAYER_SIZE.X;
                        }
                    }
                    if (checkRectIntersect(collider, playerBoundsY))
                    {
                        //need to check if you are to the up or to the down
                        //if player is above the wall
                        if (collider.Y.min >= Rect.getSpriteBounds(start, PLAYER_SIZE).Y.max)
                        {
                            moveTo.Y = collider.Y.min - PLAYER_SIZE.Y;
                        }
                        //if player is below the wall
                        else
                        {
                            moveTo.Y = collider.Y.max;
                        }
                    }
                }
               
            }
        }
        return moveTo;
    }
    private bool checkIntervalIntersect(Range barrier, Range player)
    {
        if (player.min < barrier.max && player.min > barrier.min) return true;
        if (player.max < barrier.max && player.max > barrier.min) return true;
        if (barrier.min < player.max && barrier.min > player.max) return true;
        return false;
    }

    private bool checkRectIntersect(Rect rect, Rect playerBounds)
    {
        return checkIntervalIntersect(rect.X, playerBounds.X)
             && checkIntervalIntersect(rect.Y, playerBounds.Y);
    }

    public void drawRoom()
    {
        Engine.DrawTexture(bg, new Vector2(0, 0));
        foreach (Dodo d in enemies)
        {
            d.DrawDodo();
        }

        foreach (Item i in items)
        {
            i.draw();
        }
    }

    public void addObject(Item i)
    {
        items.Add(i);
    }

    public void removeObject(Item i)
    {
        items.Remove(i);
    }

    public void pickup(Player p, Item i)
    {
        removeObject(i);
        i.pickup();
        p.pickup(i);
    }

    //read rectangle collisions from a text file
    public List<Rect> readCollisionZones(String file)
    {
        List<Rect> loader = new List<Rect>();

        using (StreamReader sr = File.OpenText("Assets/" + file))
        {
            string s;
            while ((s = sr.ReadLine()) != null)
            {
                String[] nums = s.Split(' ');

                loader.Add(new Rect(new Range(float.Parse(nums[0]) , float.Parse(nums[1])),
                                                new Range(float.Parse(nums[2]), float.Parse(nums[3]))));
            }
        }
        return loader;
    }
}

