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
        int row = 0;
        int col = 0;
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
    }
}
