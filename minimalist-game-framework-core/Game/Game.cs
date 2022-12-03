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
    Room[,] rooms;
    
    public Game()
    {        
        rooms = new Room[30, 20];
        rooms[(int)currRoom.X, (int)currRoom.Y] = new Room(currRoom);
        player = new Player(startpos, currRoom);
    }

    public void Update()
    {
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

        //normalize the movement
        moveVector = moveVector.Normalized();


        // Processing ----------------------------------
                     
        //update player
        bool successfulMove = player.move(moveVector, rooms[(int)currRoom.X, (int)currRoom.Y]);
        if (successfulMove) wrap();

        // Graphics ------------------------------------
        rooms[(int)currRoom.X, (int)currRoom.Y].drawRoom();
        player.drawPlayer();

        // Dodo ----------------------------------------
    }

    public void wrap()
    {
        Vector2 playerPos = player.position();

        if (playerPos.X > Resolution.X) swapRoom(0);
        if (playerPos.X + PLAYER_SIZE.X < 0) swapRoom(1);
        if (playerPos.Y > Resolution.Y) swapRoom(2);
        if (playerPos.Y + PLAYER_SIZE.Y < 0) swapRoom(3);
    }

    public void swapRoom(int i)
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
        player.move(playerPos);
    }
}
