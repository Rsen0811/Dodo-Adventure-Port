using System;
using System.Collections.Generic;
using System.Text;

class StartScreen
{
    Texture startScreen = Engine.LoadTexture("startScreen/startScreen.png");
    Font font = Engine.LoadFont("font.ttf",25);
    int difficulty = 0;
    public StartScreen()
    {

    }
    public void draw()
    {
        Engine.DrawTexture(startScreen, Vector2.Zero, size: Game.Resolution) ;

    }
    public void Update()
    {

    }
    public int GetDifficulty()
    {
        return difficulty;
    }
}
