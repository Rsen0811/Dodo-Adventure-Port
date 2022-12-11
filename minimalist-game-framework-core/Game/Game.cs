using System;
using System.Collections.Generic;

class Game
{
    public static readonly string Title = "Minimalist Game Framework";
    public static readonly Vector2 Resolution = new Vector2(960, 640);
    public static readonly Vector2 PLAYER_SIZE = new Vector2(24, 24);
    //readonly int PLAYER_SPEED = 400;

    Vector2 tileSize = new Vector2(32, 32);
    Vector2 startpos = new Vector2(250, 300);
    Vector2 currRoom = new Vector2(2, 4);
    Player player;
    static Room[,] rooms;
    StartScreen startScreen;
    public Game()
    {
        rooms = new Room[30, 20];
        rooms[(int)currRoom.X, (int)currRoom.Y] = new Room(currRoom);
        player = new Player(startpos, currRoom);
        startScreen = new StartScreen();
    }

    public void Update()
    {
        if (startScreen.ShouldRun())
        {
            startScreen.Update();
            startScreen.Draw();
            return;
        }
        if (player.GameOver()) GameOver();
        //three steps
        //1. collect input and predict the movement without colisions
        //2. check if the character intersects with a wall at those coordinates
        //3. change the movement appropriately
        
        // Input ---------------------------------------
        //collect input and predict movement
        Vector2 moveVector = Vector2.Zero;
        //collect unnormalized movement
        if (Engine.GetKeyHeld(Key.Up)) moveVector.Y -= 1;
        if (Engine.GetKeyHeld(Key.Down)) moveVector.Y += 1;
        if (Engine.GetKeyHeld(Key.Right)) moveVector.X += 1;
        if (Engine.GetKeyHeld(Key.Left)) moveVector.X -= 1;

        if (Engine.GetKeyHeld(Key.P)) rooms[(int)currRoom.X, (int)currRoom.Y].TestaddDodo();
        
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

    public void GameOver()
    {
        // add game over code here
        return;
    }
    public void Wrap()
    {
        Vector2 playerPos = player.position();

        if (playerPos.X > Resolution.X) SwapRoom(0);
        if (playerPos.X + PLAYER_SIZE.X < 0) SwapRoom(1);
        if (playerPos.Y > Resolution.Y) SwapRoom(2);
        if (playerPos.Y + PLAYER_SIZE.Y < 0) SwapRoom(3);
    }

    public void SwapRoom(int i)
    {
        Vector2 playerPos = player.position();
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
        if (rooms[(int)currRoom.X, (int)currRoom.Y] == null)
        {
            rooms[(int)currRoom.X, (int)currRoom.Y] = new Room(currRoom);
        }
        player.Move(playerPos);
    }
    public static Room getRoom(Vector2 address)
    {
        if(rooms[(int)address.X, (int)address.Y] == null)
        {
            rooms[(int)address.X, (int)address.Y] = new Room(address);
        }
        return rooms[(int)address.X, (int)address.Y];
    }

}
