using System;
using System.Collections.Generic;
using System.Text;

class StartScreen
{
    Texture startScreen = Engine.LoadTexture("startScreen/startScreen.png");
    Font font = Engine.LoadFont("startScreen/font.ttf",20);
    int difficulty = 0;
    public StartScreen()
    {

    }
    public void draw()
    {
        Engine.DrawTexture(startScreen, Vector2.Zero, size: Game.Resolution) ;
        Engine.DrawString(""+difficulty%3, new Vector2(7 * Game.Resolution.X / 8+45, Game.Resolution.Y / 8+15),Color.White,font);
    }
    public void Update()
    {
        
    }
    public int GetDifficulty()
    {
        return difficulty%3;
    }
}
