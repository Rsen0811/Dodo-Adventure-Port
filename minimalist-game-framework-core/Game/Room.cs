using System;
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
        CollisionZones = ReadCollisionZones("rooms/" + name + "/" + name + "c.txt");
        bg = Engine.LoadTexture("rooms/" + name + "/" + name + "i.png");

        (items, enemies) = ReadObjects("rooms/" + name + "/" + name + "o.txt");

        this.pos = pos;
    }

    public Vector2 Position()
    {
        return pos;
    }

    public void TestaddDodo()
    { 
        enemies.Add(new Dodo(new Vector2(200, 200)));
    }
    public void Update(Player p)
    {
        if (p.GetItem() != null && p.GetItem().GetType() == typeof(Sword))
        {
            swordSweep((Sword) p.GetItem());
        }
        foreach (Dodo d in enemies)
        {
            d.Update(p, 960);
        }
        List<Item> toRemove = new List<Item>();
        foreach (Item i in items)
        {
            i.Update(Rect.GetSpriteBounds(p.Position(), PLAYER_SIZE));
            if (i.IsHeld())
            {
                toRemove.Add(i);
            }
        }
        foreach(Item i in toRemove)
        {
            this.Pickup(p,i);
        }
    }

    public void Idle()
    {
        foreach (Dodo d in enemies)
        {
            d.Idle();
        }
    }

    public Vector2 Move(Vector2 start, Vector2 movement)
    {
        // why do calcs if none needed
        if (movement.Equals(Vector2.Zero)) return start;

        Vector2 moveTo = start + movement;
        Rect playerBounds = Rect.GetSpriteBounds(moveTo, PLAYER_SIZE);


        foreach (Rect collider in CollisionZones)
        { // position is actual x range and size is actually y range
            if (Rect.CheckRectIntersect(collider, playerBounds))
            { // does handle corners
                Vector2 moveToY = start + new Vector2(0, movement.Y);
                Rect playerBoundsY = Rect.GetSpriteBounds(moveToY, PLAYER_SIZE);
                Vector2 moveToX = start + new Vector2(movement.X, 0);
                Rect playerBoundsX = Rect.GetSpriteBounds(moveToX, PLAYER_SIZE);
                if (!Rect.CheckRectIntersect(collider, playerBoundsY) && !Rect.CheckRectIntersect(collider, playerBoundsX))
                {
                    //check just x and just y and which ever moves farther is the one we use
                    Vector2 Xmove= Move(start, new Vector2(movement.X, 0));
                    float XmoveLength = (Xmove - start).Length();
                    Vector2 Ymove = Move(start, new Vector2(0, movement.Y));
                    float YmoveLength = (Ymove - start).Length();
                    return XmoveLength > YmoveLength ? Xmove : Ymove;
                }
                else
                {
                    if (Rect.CheckRectIntersect(collider, playerBoundsX))
                    {
                        //need to check if you are to the right or to the left
                        //if player to the right of the wall
                        if (collider.X.max<= Rect.GetSpriteBounds(start, PLAYER_SIZE).X.min)
                        {
                            moveTo.X = collider.X.max;
                        }
                        //if player is to the left of the wall
                        else
                        {
                            moveTo.X = collider.X.min - PLAYER_SIZE.X;
                        }
                    }
                    if (Rect.CheckRectIntersect(collider, playerBoundsY))
                    {
                        //need to check if you are to the up or to the down
                        //if player is above the wall
                        if (collider.Y.min >= Rect.GetSpriteBounds(start, PLAYER_SIZE).Y.max)
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

    public void DrawRoom()
    {
        Engine.DrawTexture(bg, new Vector2(0, 0));
        foreach (Dodo d in enemies)
        {
            d.DrawDodo();
        }

        foreach (Item i in items)
        {
            i.Draw();
        }
    }

    public void AddObject(Item i)
    {
        items.Add(i);
    }

    public void RemoveObject(Item i)
    {
        items.Remove(i);
    }

    public void Pickup(Player p, Item i)
    {
        RemoveObject(i);
        i.Pickup();
        p.Pickup(i);
    }

    //read rectangle collisions from a text file
    public List<Rect> ReadCollisionZones(String file)
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

    public (List<Item>, List<Dodo>) ReadObjects(String file)
    {
        List<Rect> loader = new List<Rect>();

        using (StreamReader sr = File.OpenText("Assets/" + file))
        {
            string s = sr.ReadToEnd();
            String[] filesplit = s.Split("---");

            return (ReadItems(filesplit[0].Trim()), ReadDodos(filesplit[1].Trim()));
        }
    }

    public List<Item> ReadItems(String items)
    {
        List<Item> loader = new List<Item>();

        byte[] byteArray = Encoding.ASCII.GetBytes(items);
        MemoryStream stream = new MemoryStream(byteArray);

        using (StreamReader sr = new StreamReader(stream))
        {
            String s;
            while ((s = sr.ReadLine()) != null)
            {
                String[] args = s.Split(' ');
                if (args[0].Equals("S")) {
                    Vector2 pos = new Vector2(int.Parse(args[1]), int.Parse(args[2]));
                    bool isHoly = (args[3].Equals(true));
                    loader.Add(new Sword(pos, isHoly));
                }                
            }
        }
        return loader;
    }

    public List<Dodo> ReadDodos(String dodos)
    {
        List<Dodo> loader = new List<Dodo>();

        byte[] byteArray = Encoding.ASCII.GetBytes(dodos);
        MemoryStream stream = new MemoryStream(byteArray);
        using (StreamReader sr = new StreamReader(stream))
        {
            String s;
            while ((s = sr.ReadLine()) != null)
            {
                String[] args = s.Split(' ');
                if (args[0].Equals("D"))
                {
                    Vector2 pos = new Vector2(int.Parse(args[1]), int.Parse(args[2]));
                    loader.Add(new Dodo(pos));
                }
            }
        }
        return loader;
    }

    public void swordSweep(Sword s) { 
        foreach(Dodo enemy in enemies)
        {
            if (Rect.CheckRectIntersect(s.CollisionZone(), enemy.GetBounds()))
            {
                 enemy.Damage();
            }
        }
    }
}

