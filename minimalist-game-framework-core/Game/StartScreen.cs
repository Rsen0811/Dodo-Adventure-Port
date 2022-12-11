using System;
using System.Collections.Generic;
using System.Text;

class StartScreen
{
    Texture startScreen = Engine.LoadTexture("startScreen/startScreen.png");
    Font font = Engine.LoadFont("startScreen/font.ttf",20);
    int difficulty = 0;
    Rect topTriangle = new Rect(new Range(880, 905), new Range(45, 70));
    Rect bottomTriangle = new Rect(new Range(880, 905), new Range(135, 160));
    public StartScreen()
    {

    }
    public void Draw()
    {
        Engine.DrawTexture(startScreen, Vector2.Zero, size: Game.Resolution);
        Engine.DrawRectEmpty(topTriangle.ToBounds(),Color.White);
        Engine.DrawString(""+difficulty%3, new Vector2(7 * Game.Resolution.X / 8+45, Game.Resolution.Y / 8+15),Color.White,font);
    }
    public void Update()
    {
        if (Engine.GetMouseButtonDown(MouseButton.Left))
        {
            Rect mouseCursor = new Rect(new Range(Engine.MousePosition.X, Engine.MousePosition.X+1), 
                                        new Range(Engine.MousePosition.Y, Engine.MousePosition.Y+1));
            if (Rect.CheckRectIntersect(mouseCursor, topTriangle))
            {
                difficulty++;
            }
            if (Rect.CheckRectIntersect(mouseCursor, bottomTriangle))
            {
                difficulty--;
            }

        }
    }
    public int GetDifficulty()
    {
        return difficulty%3;
    }
    
}
