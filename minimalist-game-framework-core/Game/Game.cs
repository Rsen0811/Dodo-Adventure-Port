using System;
using System.Collections.Generic;

class Game
{
    public static readonly string Title = "Minimalist Game Framework";
    public static readonly Vector2 Resolution = new Vector2(960, 640);
    Texture player = Engine.LoadTexture("textures/player.png");
    Vector2 tileSize = new Vector2(32, 32);
    Vector2 playerPos = new Vector2(416, 584);
    Room currentRoom = new Room(new Vector2(2, 4));

    readonly int PLAYER_SPEED = 150;
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

        // Graphics ------------------------------------
        currentRoom.drawRoom();
        Engine.DrawTexture(player, playerPos, size: new Vector2(24, 24));
        
    }
}
