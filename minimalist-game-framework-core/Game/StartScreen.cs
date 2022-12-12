using System;
using System.Collections.Generic;
using System.Text;

class StartScreen
{
    Texture startScreen = Engine.LoadTexture("startScreen/startScreen.png");
    Font font = Engine.LoadFont("startScreen/font.ttf",18);
    int difficulty = 0;
    Rect topTriangle = new Rect(new Range(880, 905), new Range(45, 70));
    Rect bottomTriangle = new Rect(new Range(880, 905), new Range(135, 160));
    bool shouldRun=true;
    public StartScreen()
    {

    }
    public void Draw()
    {
        Engine.DrawTexture(startScreen, Vector2.Zero, size: Game.Resolution);
        Engine.DrawString("Store", new Vector2(0.25f * Game.Resolution.X / 8 + 50, Game.Resolution.Y / 8 + 15), Color.White, font, TextAlignment.Center);
        Engine.DrawString("Trophies", new Vector2(1.5f * Game.Resolution.X / 8 + 50, Game.Resolution.Y / 8 + 15), Color.White, font, TextAlignment.Center);

        if (Math.Abs(difficulty) % 3 == 0)
        {
            Engine.DrawString("Easy" , new Vector2(7 * Game.Resolution.X / 8 + 50, Game.Resolution.Y / 8 + 15), Color.White, font, TextAlignment.Center);
        }
        else if(Math.Abs(difficulty) % 3==1)
        {
            Engine.DrawString("Medium", new Vector2(7 * Game.Resolution.X / 8 + 50, Game.Resolution.Y / 8 + 15), Color.White, font, TextAlignment.Center);
        }
        else
        {
            Engine.DrawString("Hard", new Vector2(7 * Game.Resolution.X / 8 + 50, Game.Resolution.Y / 8 + 15), Color.White, font, TextAlignment.Center);
        }
    }
    public void Update()
    {
        if (Engine.GetMouseButtonDown(MouseButton.Left))
        {
            Rect mouseCursor = new Rect(new Range(Engine.MousePosition.X, Engine.MousePosition.X), 
                                        new Range(Engine.MousePosition.Y, Engine.MousePosition.Y));
            if (Rect.CheckRectIntersect(mouseCursor, topTriangle))
            {
                difficulty++;
            }
            else if (Rect.CheckRectIntersect(mouseCursor, bottomTriangle))
            {
                difficulty--;
            }
            else
            {
                shouldRun = false;
            }
            
        }
    }
    public int GetDifficulty()
    {
        return Math.Abs(difficulty)%3;
    }
    public bool ShouldRun()
    {
        return shouldRun;
    }
}
