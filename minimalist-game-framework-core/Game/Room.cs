using System;
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
    
    List<Item> items;
    List<Switch> switches= new List<Switch>();
    public List<Enemy> enemies; /// change back to private
    Boss boss;
    List<Glyph> glyphs;
    List<Coin> coins;
    private readonly int DIFFICULTY;

    public Room(Vector2 pos) {
        DIFFICULTY = StartScreen.GetDifficulty();

        String name = "" + pos.X + pos.Y;
        (CollisionZones, Gates) = ReadOnlyCollisions("rooms/" + name + "/" + name + "c.txt");
        bg = Engine.LoadTexture("rooms/" + name + "/" + name + "i.png");

        (items, enemies, glyphs, coins) = ReadObjects("rooms/" + name + "/" + name + "o.txt");

        this.pos = pos;
    }

    public Vector2 Position()
    {
        return pos;
    }

    public void AddEnemy(Enemy e)
    { 
        enemies.Add(e);
    }
    
    public void Update(Player p)
    {
        if (Engine.GetKeyDown(Key.Escape) && glyphs.Count != 0)
        {
            glyphs.RemoveAt(0);            
        }
        if (p.GetItem() != null && p.GetItem().GetType() == typeof(Sword))
        {
            swordSweep((Sword) p.GetItem());
        }
        foreach (Enemy d in enemies)
        {
            d.Update(p, 960);
        }

        List<Item> toRemove = new List<Item>();
        foreach (Item i in items)
        {
            i.Update(p.getPlayerBounds(), Vector2.Zero);
            if (i.IsHeld())
            {
                toRemove.Add(i);
            }
        }
        foreach(Item i in toRemove)
        {
            this.Pickup(p,i);
        }
        if (p.GetItem() != null && p.GetItem().GetType()==typeof(GateKey)){
            GateKey gateKey = (GateKey) p.GetItem();
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

        for(int i = 0; i < coins.Count; i++)
        {
            coins[i].coinUpdate(p);
            if (coins[i].isCollected())
            {
                coins.Remove(coins[i]);
            }
        }
    }

    public void Idle()
    {
        foreach (Enemy d in enemies)
        {
            d.Idle();
        }
    }

    public Vector2 Move(Vector2 start, Vector2 movement)
    {
        return Move(start, movement, PLAYER_SIZE);
    }
    public Vector2 Move(Vector2 start, Vector2 movement, Vector2 spriteSize)
    {
        // why do calcs if none needed
        if (movement.Equals(Vector2.Zero)) return start;

        Vector2 moveTo = start + movement;
        Rect playerBounds = Rect.GetSpriteBounds(moveTo, spriteSize);
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
                Rect spriteBoundsY = Rect.GetSpriteBounds(moveToY, spriteSize);
                Vector2 moveToX = start + new Vector2(movement.X, 0);
                Rect spriteBoundsX = Rect.GetSpriteBounds(moveToX, spriteSize);
                if (!Rect.CheckRectIntersect(collider, spriteBoundsY) && !Rect.CheckRectIntersect(collider, spriteBoundsX))
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
                    if (Rect.CheckRectIntersect(collider, spriteBoundsX))
                    {
                        //need to check if you are to the right or to the left
                        //if player to the right of the wall
                        if (collider.X.max<= Rect.GetSpriteBounds(start, spriteSize).X.min)
                        {
                            moveTo.X = collider.X.max;
                        }
                        //if player is to the left of the wall
                        else
                        {
                            moveTo.X = collider.X.min - spriteSize.X;
                        }
                    }
                    if (Rect.CheckRectIntersect(collider, spriteBoundsY))
                    {
                        //need to check if you are to the up or to the down
                        //if player is above the wall
                        if (collider.Y.min >= Rect.GetSpriteBounds(start, spriteSize).Y.max)
                        {
                            moveTo.Y = collider.Y.min - spriteSize.Y;
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
    public void toggleGate(String gateName)
    {
        foreach(Gate g in Gates)
        {
            if (g.getName().Equals(gateName))
            {
                g.Toggle();
            }
        }
    }
    public void DrawRoom()
    {
        Engine.DrawTexture(bg, new Vector2(0, 0));
        
        foreach (Item i in items)
        {
            i.Draw();
        }
        
        foreach(Gate g in Gates)
        {
            g.Draw();
        }

        foreach(Switch s in switches)
        {
            s.Draw();
         }

        if (glyphs.Count != 0)
        {
            glyphs[0].Draw();
        }

        foreach(Coin c in coins)
        {
            c.Draw();
        }

        foreach (Enemy d in enemies)
        {
            d.DrawDodo();
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
            if (filesplit.Length == 2)
            {
                return (ReadCollisionZones(filesplit[0].Trim()), ReadGates(filesplit[1].Trim()));
            }
            else
            {
                return (ReadCollisionZones(filesplit[0].Trim()), ReadGates(""));
            }
        }
    }
    public Gate GetGate(String gateName)
    {
        foreach(Gate g in Gates)
        {
            if (g.getName().Equals(gateName))
            {
                return g;
            }
        }
        return null;
    }
    public void addSwitch(List<String> pairs, Vector2 pos, String color) 
    {
        switches.Add(new Switch(pairs, new Rect(new Range(pos.X, pos.X + 32), new Range(pos.Y, pos.Y + 32)),color));
    }
    public Switch checkSwitchIntersect(Rect player)
    {
        foreach(Switch s in switches)
        {
            if (Rect.CheckRectIntersect(s, player)){
                return s;
            }
        }
        return null;
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
                String name = args[4];
                Rect rect = new Rect(new Range(float.Parse(args[0]), float.Parse(args[1])),
                                    new Range(float.Parse(args[2]), float.Parse(args[3])));
                Gate temp = new Gate(name, rect);
                if(args.Length > 5)
                {
                    temp.Toggle();
                }
                loader.Add(temp);
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

    public (List<Item>, List<Enemy>, List<Glyph>, List<Coin>) ReadObjects(String file)
    {
        using (StreamReader sr = File.OpenText("Assets/" + file))
        {
            string s = sr.ReadToEnd();
            String[] filesplit = s.Split("---");

            return (ReadItems(filesplit[0].Trim()), ReadDodos(filesplit[1].Trim()),
                ReadGlyphs(filesplit[2].Trim()), (filesplit.GetLength(0) == 4) ? ReadCoins(filesplit[3].Trim()) : new List<Coin>());
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

    public List<Enemy> ReadDodos(String dodos)
    {
        List<Enemy> loader = new List<Enemy>();

        byte[] byteArray = Encoding.ASCII.GetBytes(dodos);
        MemoryStream stream = new MemoryStream(byteArray);
        using (StreamReader sr = new StreamReader(stream))
        {
            String s;
            while ((s = sr.ReadLine()) != null)
            {
                String[] args = s.Trim().Split(' ');
                if (args.Length == 4 && int.Parse(args[3]) > DIFFICULTY) continue;
                if (args[0].Equals("D"))
                {
                    Vector2 pos = new Vector2(int.Parse(args[1]), int.Parse(args[2]));
                    switch (DIFFICULTY)
                    {
                        case 0:
                            loader.Add(Dodo.EasyDodo(pos));
                            break;
                        case 1:
                            loader.Add(Dodo.MidDodo(pos));
                            break;
                        case 2:
                            loader.Add(Dodo.HardDodo(pos));
                            break;
                    }
                }
                if (args[0].Equals("P"))
                {
                    Vector2 pos = new Vector2(int.Parse(args[1]), int.Parse(args[2]));
                    switch (DIFFICULTY)
                    {
                        case 0:
                            loader.Add(PDodo.EasyDodo(pos));
                            break;
                        case 1:
                            loader.Add(PDodo.MidDodo(pos));
                            break;
                        case 2:
                            loader.Add(PDodo.HardDodo(pos));
                            break;
                    }
                }
            }
        }
        return loader;
    }

    public List<Glyph> ReadGlyphs(String dodos)
    {
        List<Glyph> loader = new List<Glyph>();

        byte[] byteArray = Encoding.ASCII.GetBytes(dodos);
        MemoryStream stream = new MemoryStream(byteArray);
        using (StreamReader sr = new StreamReader(stream))
        {
            String s;
            while ((s = sr.ReadLine()) != null)
            {
                String[] args = s.Split(' ');
                if (args[0].Equals("T"))
                {
                    Vector2 pos = new Vector2(int.Parse(args[1]), int.Parse(args[2]));
                    loader.Add(new Glyph(pos, args[3]));
                }
            }
        }
        return loader;
    }
    public List<Coin> ReadCoins(String dodos)
    {
        List<Coin> loader = new List<Coin>();

        byte[] byteArray = Encoding.ASCII.GetBytes(dodos);
        MemoryStream stream = new MemoryStream(byteArray);
        using (StreamReader sr = new StreamReader(stream))
        {
            String s;
            while ((s = sr.ReadLine()) != null)
            {
                String[] args = s.Split(' ');
                if (args[0].Equals("C"))
                {
                    Vector2 pos = new Vector2(int.Parse(args[1]), int.Parse(args[2]));
                    loader.Add(new Coin(pos, int.Parse(args[3])));
                }
            }
        }
        return loader;
    }

    public void swordSweep(Sword s) { 
        foreach(Enemy enemy in enemies)
        {
            if (Rect.CheckRectIntersect(s.CollisionZone(), enemy.GetBounds()))
            {
                 enemy.Damage();
                
            }
        }
    }
    public bool allDead()
    {
        foreach(Enemy e in enemies)
        {
            if (e.IsAlive())
            {
                return false;
            }
        }
        return true;
    }
}

