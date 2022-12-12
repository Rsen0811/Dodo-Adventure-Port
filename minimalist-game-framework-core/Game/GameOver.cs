using System;
using System.Collections.Generic;
using System.Text;

public class GameOver
{
    Texture end = Engine.LoadTexture("textures/end.png");
    bool isGameOver;

    public GameOver()
    {
        isGameOver = false;
        
    }

    public void Draw()
    {
        Engine.DrawTexture(end, Vector2.Zero, size: Game.Resolution);
    }

    
}
