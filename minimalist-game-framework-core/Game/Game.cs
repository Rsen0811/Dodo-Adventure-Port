using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Principal;

class Game
{
    public static readonly string Title = "Adventure Atari #2";
    public static readonly Vector2 Resolution = new Vector2(400, 320);
    //background

    Texture _floor= Engine.LoadTexture("Floor.png");
    Texture _walls= Engine.LoadTexture("Walls-background.png");
    //player
    Texture _player=Engine.LoadTexture("Player.png");
    //path to map file
    string path = @"./Assets/Map1.txt";
    //array to store the map
    String[,] collisions = new String[4,5];
    //
    Vector2 tileSize = new Vector2(80, 80);
    Vector2 playerPos = new Vector2(100, 100);
    public Game()
    {
     using (StreamReader sr = File.OpenText(path))
        {
          string s;
          int col=0;
          int row= 0;
        while((s = sr.ReadLine()) != null)
         {
                String[] parts = s.Split(' ');
                foreach (String part in parts)
                {
                    collisions[row,col]= part;
                    Console.WriteLine(part);
                    col++;
                }
                row++;
                col = 0;
          }
        }
    }

    public void Update()
    {
        // three steps
        //1. collect input and predict the movement without colisions
        //2. check if the character intersects with a wall at those coordinates
        //3. change the movement appropriately
        //4. 
        //collect input and predict movement
        Vector2 predictedMovement = Vector2.Zero;
        if (Engine.MousePosition.X > playerPos.X)
        {
            predictedMovement.X++;
        }
        else
        {
            predictedMovement.X -= 1;
        }
        if (Engine.MousePosition.Y > playerPos.Y)
        {
            predictedMovement.Y++;
        }
        else
        {
            predictedMovement.Y -= 1;
        }
        //check if it intersects 
        Vector2 predictedPosition = predictedMovement + playerPos;
        //can simplify this to be faster
        int row = 0;
        int col = 0;
        Boolean collided = false;
        foreach (String tile in collisions)
        {
            if (!collided) { 
            String temp = collisions[row, col];
                if (temp == "1")
                {
                    //check if you intersect
                    //tile size is the width of the tiles 

                    Vector2 wallXRange = new Vector2(col * tileSize.X, col * tileSize.X + tileSize.X);
                    Vector2 wallYRange = new Vector2(row * tileSize.X, row * tileSize.X + tileSize.X);
                    Vector2 PlayerXRange = new Vector2(predictedPosition.X, predictedPosition.X + 40);
                    Vector2 PlayerYRange = new Vector2(predictedPosition.Y, predictedPosition.Y+40);

                    if (
                       checkIntervalIntersect(wallXRange.X, wallXRange.Y, PlayerXRange.X, PlayerXRange.Y) 
                       &&
                       checkIntervalIntersect(wallYRange.X, wallYRange.Y, PlayerYRange.X, PlayerYRange.Y))
                    {
                        predictedMovement = Vector2.Zero;
                    }
                }
            }
            col++;
            if (col % 5 == 0)
            {
                col = 0;
                row++;
            }
        }
        playerPos += predictedMovement;
        row = 0;
        col = 0;
        
        foreach (String tile in collisions)
        {
            String temp = collisions[row, col];
            if (temp == "1")
            {
                Engine.DrawTexture(_walls, new Vector2(col, row)*tileSize.X, size: tileSize);
            }
            else
            {
                Engine.DrawTexture(_floor, new Vector2(col, row)*tileSize.X, size: tileSize);
            }
            col++;
            if (col % 5 == 0)
            {
                col = 0;
                row++;
            }
        }
        Engine.DrawTexture(_player, playerPos, size: new Vector2(40, 40));
    }
    private bool checkIntervalIntersect(float x1,float x2, float y1, float y2)
    {
        if(y1<x2 && y1 > x1)
        {
            return true;
        }
        if (y2 < x2 && y2 > x1)
        {
            return true;
        }
        if (x1 < y2 && x1 > y1)
        {
            return true;
        }
        return false;
    }
}
