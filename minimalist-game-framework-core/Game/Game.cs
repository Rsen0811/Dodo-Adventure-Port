using System;
using System.Collections.Generic;

class Game
{
    public static readonly string Title = "Minimalist Game Framework";
    public static readonly Vector2 Resolution = new Vector2(960, 640);
    Texture player = Engine.LoadTexture("textures/player.png");
    Vector2 tileSize = new Vector2(32, 32);
    Vector2 playerPos = new Vector2(250, 300);
    Vector2 currRoom = new Vector2(2, 4);
    public static readonly Vector2 PLAYER_SIZE = new Vector2(24, 24);
    Room currentRoom = new Room(new Vector2(2, 4));

    readonly int PLAYER_SPEED = 400;
    public Game()
    {
        
    }

    public void Update()
    {
        //three steps
        //1. collect input and predict the movement without colisions
        //2. check if the character intersects with a wall at those coordinates
        //3. change the movement appropriately
        
        // Input ---------------------------------------
        //collect input and predict movement
        Vector2 predictedMovement = Vector2.Zero;
        //collect unnormalized movement
        if (Engine.GetKeyHeld(Key.Up)) predictedMovement.Y -= 1;
        if (Engine.GetKeyHeld(Key.Down)) predictedMovement.Y += 1;
        if (Engine.GetKeyHeld(Key.Right)) predictedMovement.X += 1;
        if (Engine.GetKeyHeld(Key.Left)) predictedMovement.X -= 1;



        // Processing ----------------------------------

        //normalize the movement
        predictedMovement = predictedMovement.Normalized() * (PLAYER_SPEED * Engine.TimeDelta);
        //check if it intersects 
        playerPos=currentRoom.move(playerPos, predictedMovement);
        wrap();

        // Graphics ------------------------------------
        currentRoom.drawRoom();
        Engine.DrawTexture(player, playerPos, size: new Vector2(24, 24));
        
    }

    public void wrap()
    {
        if(playerPos.X > Resolution.X) swapRoom(0);
        if(playerPos.X + PLAYER_SIZE.X < 0) swapRoom(1);
        if (playerPos.Y > Resolution.Y) swapRoom(2);
        if (playerPos.Y + PLAYER_SIZE.Y < 0) swapRoom(3);
    }

    public void swapRoom(int i)
    {
        switch(i)
        {
            case 0:
                playerPos.X = 1 - PLAYER_SIZE.X;
                currRoom.X += 1;
                break;
            case 1:
                playerPos.X = Resolution.X - 1;
                currRoom.X -= 1;
                break;
            case 2:
                playerPos.Y = 1 - PLAYER_SIZE.Y;
                currRoom.Y += 1;
                break;
            case 3:
                playerPos.Y = Resolution.Y - 1;
                currRoom.Y -= 1;
                break;

        }

    }
}
