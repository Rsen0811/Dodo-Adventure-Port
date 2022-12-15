﻿using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

class Room
{
    readonly Vector2 PLAYER_SIZE = new Vector2(24, 24);
    List<Rect> CollisionZones;
    List<Gate> Gates;
    Texture bg;
    Vector2 pos;
    public List<Dodo> enemies; /// change back to private
    List<Item> items;

    public Room(Vector2 pos) {
        String name = "" + pos.X + pos.Y;
        
        (CollisionZones, Gates) = ReadOnlyCollisions("rooms/" + name + "/" + name + "c.txt");
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
        if (p.GetItem().GetType().Equals(new GateKey("", Vector2.Zero)));
        {
            GateKey gateKey =(GateKey) p.GetItem();
            foreach (Gate g in Gates)
            {
                //somehow check if p.holding is a gatekey and if it intersects with g
                if (gateKey.CheckGateIntersect(g))
                {
                    g.isOpen = true;
                    p.DeleteItem();
                }
            }
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
        List<Rect> temp =new List<Rect>();
        temp.AddRange(CollisionZones);
        foreach(Gate g in Gates){
            if (!g.isOpen)
            {
                temp.Add(g);
            }
        }

        foreach (Rect collider in temp)
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
        
        foreach(Gate g in Gates)
        {
            g.Draw();
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
    public (List<Rect>, List<Gate>) ReadOnlyCollisions(String file)
    {
        using (StreamReader sr = File.OpenText("Assets/" + file))
        {
            string s = sr.ReadToEnd();
            String[] filesplit = s.Split("---");

            return (ReadCollisionZones(filesplit[0].Trim()), ReadGates(filesplit[1].Trim()));
        }
    }
    
    public List<Gate> ReadGates(String gates)
    {
        List<Gate> loader = new List<Gate>();

        byte[] byteArray = Encoding.ASCII.GetBytes(gates);
        MemoryStream stream = new MemoryStream(byteArray);

        using (StreamReader sr = new StreamReader(stream))
        {
            String s;
            while ((s = sr.ReadLine()) != null)
            {
                String[] args = s.Split(' ');
                String name = "textures/gates/" + args[0];
                Rect rect = new Rect(new Range(float.Parse(args[1]), float.Parse(args[2])),
                                    new Range(float.Parse(args[3]), float.Parse(args[4]))); 
                loader.Add(new Gate(name,rect));
            }
        }
        return loader;
    }
    public List<Rect> ReadCollisionZones(String collisionZones)
    {
        List<Rect> loader = new List<Rect>();

        byte[] byteArray = Encoding.ASCII.GetBytes(collisionZones);
        MemoryStream stream = new MemoryStream(byteArray);

        using (StreamReader sr = new StreamReader(stream))
        {
            String s;
            while ((s = sr.ReadLine()) != null)
            {
                String[] args = s.Split(' ');
                loader.Add(new Rect(new Range(float.Parse(args[0]), float.Parse(args[1])),
                            new Range(float.Parse(args[2]), float.Parse(args[3]))));
            }
        }
        return loader;
    }

    public (List<Item>, List<Dodo>) ReadObjects(String file)
    {
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
                else if (args[0].Equals("K"))
                {
                    // in text file format as K X Y GateName
                    loader.Add(new GateKey(args[3], new Vector2(int.Parse(args[1]), int.Parse(args[2]))));
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
    public void readGates(String fileName)
    {

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

