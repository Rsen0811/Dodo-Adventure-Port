﻿using System;
using System.Collections.Generic;
using System.Text;

public class GameOver
{
    Texture end = Engine.LoadTexture("textures/end2.png");
    Font font = Engine.LoadFont("startScreen/font.ttf", 18);
    bool isGameOver;

    public GameOver()
    {
        isGameOver = false;
        writeCoins();
        
    }

    public void Draw(int score)
    {
        
        Engine.DrawTexture(end, Vector2.Zero, size: Game.Resolution);
        Engine.DrawString("Dodos Defeated: " + score, new Vector2(480, 480), Color.Black, font);
    }

    private async void writeCoins()
    {
        await Coin.WriteCoins();
    }
    
}
