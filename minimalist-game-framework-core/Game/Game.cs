using System;
using System.Collections.Generic;
using System.IO;

class Game
{
    public static readonly string Title = "Dodventure";
    public static readonly Vector2 Resolution = new Vector2(960, 640);
    public static readonly Vector2 PLAYER_SIZE = new Vector2(24, 24);
    public static readonly Vector2 SPAWN = new Vector2(2, 4);
    public static readonly Vector2 SPAWNPOS = new Vector2(250, 300);
    //readonly int PLAYER_SPEED = 400;


    static readonly Vector2[] existingRooms = {new Vector2(2, 4), new Vector2(2, 5),
        new Vector2(0,2), new Vector2(0, 0), new Vector2(0, 1), new Vector2(1,1),
        new Vector2(1, 2), new Vector2(1, 0), new Vector2(2, 2) ,
        new Vector2(0, 3), new Vector2(1, 3), new Vector2(0,4), new Vector2(1,4)};
    Vector2 tileSize = new Vector2(32, 32);
    Vector2 startpos = SPAWNPOS;
    Vector2 currRoom = SPAWN;
    Player player=null;
    static Room[,] rooms;
    GameOver endScreen;
    readonly Music music = Engine.LoadMusic("sounds/adventureSoundtrack.mp3");
    readonly static Sound swordHit = Engine.LoadSound("sounds/swordHit.mp3");
    float gameTime = 0;
    public Game()
    {
        Engine.PlayMusic(music, looping: true);
        rooms = new Room[30, 20];
        rooms[(int)currRoom.X, (int)currRoom.Y] = new Room(currRoom);
        
        endScreen = new GameOver();
    }

    public void Update()
    {
        if (StartScreen.ShouldRun())
        {
            if (Engine.GetKeyDown(Key.I))
            {
                Trophies.KillDodo();
                Trophies.Save();
            }
            StartScreen.Update();
            StartScreen.Draw();
            return;
        }
        if (player == null)
        {
            loadAllRooms();
            player = new Player(startpos, currRoom,
            maxDeathHits: StartScreen.GetDifficulty() == 3 ? 15 : 12);
        }
        if (player.GameOver())
        {
            // temp score code
            int deadDodoCount = 0;
            foreach(Room r in rooms)
            {
                if (r == null) continue;
                foreach(Enemy d in r.enemies)
                {
                    if (!d.IsAlive()) 
                    {
                        deadDodoCount++;
                    }
                }
            }
            endScreen.Draw(deadDodoCount);
            if (Engine.GetMouseButtonUp(MouseButton.Left))
            {
                Trophies.KillDodo(dodos: deadDodoCount);
                switch (StartScreen.GetDifficulty())
                {
                    case 0:
                        Trophies.BeatEasy(gameTime);
                        break;
                    case 1:
                        Trophies.BeatMid(gameTime);
                        break;
                    case 2:
                        Trophies.BeatHard(gameTime);
                        break;
                }
                Trophies.Save();
                gameTime = 0;
                reset();
            }
            return;
        }
        
        //three steps
        //1. collect input and predict the movement without colisions
        //2. check if the character intersects with a wall at those coordinates
        //3. change the movement appropriately

        // Input ---------------------------------------
        //collect input and predict movement
        Vector2 moveVector = Vector2.Zero;
        //collect unnormalized movement
        if (Engine.GetKeyHeld(Key.W)) moveVector.Y -= 1;
        if (Engine.GetKeyHeld(Key.S)) moveVector.Y += 1;
        if (Engine.GetKeyHeld(Key.D)) moveVector.X += 1;
        if (Engine.GetKeyHeld(Key.A)) moveVector.X -= 1;
        
        //normalize the movement
        moveVector = moveVector.Normalized();


        // Processing ----------------------------------

        //update player
        bool successfulMove = player.Move(moveVector, rooms[(int)currRoom.X, (int)currRoom.Y]);
        if (successfulMove) Wrap();
        player.ChangeRoom(rooms[(int)currRoom.X, (int)currRoom.Y]);
        rooms[(int)currRoom.X, (int)currRoom.Y].Update(player);
        Idle();
        player.Update();
        // Game Timer ----------------------------------
        gameTime += Engine.TimeDelta;
        // Graphics ------------------------------------
        rooms[(int)currRoom.X, (int)currRoom.Y].DrawRoom();
        player.DrawPlayer();
        // Dodo ----------------------------------------
    }

    public void Idle() 
    { 
        foreach(Room r in rooms)
        {
            // if room is neighboring
            if (r != null && Math.Abs((r.Position() - currRoom).Length() - 1) <= 0)
            {
                r.Idle();
            }
        }
    }

    public void loadAllRooms()
    {
        foreach(Vector2 pos in existingRooms)
        {
            rooms[(int)pos.X, (int)pos.Y] = new Room(pos);
        }
        
        //read switches
        using (StreamReader sr = File.OpenText("Assets/" + "switches.txt"))
        {

            //ROOM# xmin ymin GateName1 GateName2 GateName3
            while (sr.Peek() != -1)
            {
                string[] s = sr.ReadLine().Split();
                Vector2 roomPos = new Vector2(int.Parse(s[0].Substring(0,1)), int.Parse(s[0].Substring(1, 1)));
                Vector2 pos = new Vector2(int.Parse(s[1]), int.Parse(s[2]));
                List<String> gates = new List<String>();
                for(int i= 3;i< s.Length; i++)
                {
                    gates.Add(s[i]);
                }
                rooms[(int)roomPos.X, (int)roomPos.Y].addSwitch(gates, pos);
            } 
        }
    }
    public void Wrap()
    {
        Vector2 playerPos = player.Position();

        if (playerPos.X > Resolution.X) SwapRoom(0);
        if (playerPos.X + PLAYER_SIZE.X < 0) SwapRoom(1);
        if (playerPos.Y > Resolution.Y) SwapRoom(2);
        if (playerPos.Y + PLAYER_SIZE.Y < 0) SwapRoom(3);

        if (rooms[(int)currRoom.X, (int)currRoom.Y] == null)
        {
            rooms[(int)currRoom.X, (int)currRoom.Y] = new Room(currRoom);
        }
    }

    public void SwapRoom(int i)
    {
        Vector2 playerPos = player.Position();
        switch(i)
        {
            case 0:
                playerPos.X = 1;
                currRoom.X += 1;
                break;
            case 1:
                playerPos.X = Resolution.X - PLAYER_SIZE.X;
                currRoom.X -= 1;
                break;
            case 2:
                playerPos.Y = 1;
                currRoom.Y += 1;
                break;
            case 3:
                playerPos.Y = Resolution.Y - PLAYER_SIZE.Y;
                currRoom.Y -= 1;
                break;
        }
        
        player.Move(playerPos);
    }
    public static void toggleGate(string gateName)
    {
        for (int i = 0; i < existingRooms.Length; i++)
        {
            Vector2 r = existingRooms[i];
            rooms[(int)r.X, (int)r.Y].toggleGate(gateName);
        }
        
    }
    public static Room getRoom(Vector2 address)
    {
        if (rooms[(int)address.X, (int)address.Y] == null)
        {
            rooms[(int)address.X, (int)address.Y] = new Room(address);
        }
        return rooms[(int)address.X, (int)address.Y];
    }

    public void reset()
    {
        Engine.PlayMusic(music, looping: true);
        currRoom = SPAWN;
        startpos = SPAWNPOS;
        rooms = new Room[30, 20];
        rooms[(int)currRoom.X, (int)currRoom.Y] = new Room(currRoom);
        player = new Player(startpos, currRoom, 
            maxDeathHits: StartScreen.GetDifficulty() == 3 ? 15 : 11);
        StartScreen.reset();
        endScreen = new GameOver();

    }

    public static void PlaySwordHit()
    {
        Engine.PlaySound(swordHit);
    }

}
