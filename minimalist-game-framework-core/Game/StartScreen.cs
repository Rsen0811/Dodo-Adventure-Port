using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

class StartScreen
{
    static Texture startScreen = Engine.LoadTexture("startScreen/startScreen.png");
    static Font font = Engine.LoadFont("startScreen/font.ttf",18);
    static int difficulty = 0;
    static Rect topTriangle = new Rect(new Range(880, 905), new Range(45, 70));
    static Rect bottomTriangle = new Rect(new Range(880, 905), new Range(135, 160));
    static Rect trophies = new Rect(new Range(0.25f * Game.Resolution.X / 8 + 40, 300), new Range(Game.Resolution.Y / 8 + 5, 115));
    static Rect trophiesBack = new Rect(new Range(830, 960), new Range(30, 70));
    static Rect trophiesReset = new Rect(new Range(840, 940), new Range(588, 620));

    static bool shouldRun=true;
    static bool showTrophies = false;
    
    public static void Draw()
    {
        if (showTrophies)
        {
            Trophies.Draw();
        }
        else
        {
            Engine.DrawTexture(startScreen, Vector2.Zero, size: Game.Resolution);
            Engine.DrawString("Store", new Vector2(0.25f * Game.Resolution.X / 8 + 50, Game.Resolution.Y / 8 + 15), Color.White, font, TextAlignment.Center);
            Engine.DrawString("Trophies", new Vector2(1.5f * Game.Resolution.X / 8 + 50, Game.Resolution.Y / 8 + 15), Color.White, font, TextAlignment.Center);

            if (Math.Abs(difficulty) % 3 == 0)
            {
                Engine.DrawString("Easy", new Vector2(7 * Game.Resolution.X / 8 + 50, Game.Resolution.Y / 8 + 15), Color.White, font, TextAlignment.Center);
            }
            else if (Math.Abs(difficulty) % 3 == 1)
            {
                Engine.DrawString("Medium", new Vector2(7 * Game.Resolution.X / 8 + 50, Game.Resolution.Y / 8 + 15), Color.White, font, TextAlignment.Center);
            }
            else
            {
                Engine.DrawString("Hard", new Vector2(7 * Game.Resolution.X / 8 + 50, Game.Resolution.Y / 8 + 15), Color.White, font, TextAlignment.Center);
            }
        }
    }
    public static void Update()
    {
        if (Engine.GetMouseButtonDown(MouseButton.Left))
        {
            Rect mouseCursor = new Rect(new Range(Engine.MousePosition.X, Engine.MousePosition.X), 
                                        new Range(Engine.MousePosition.Y, Engine.MousePosition.Y));
            if (showTrophies)
            {
                if (Rect.CheckRectIntersect(mouseCursor, trophiesBack))
                {
                    showTrophies = false;
                }
                else if (Rect.CheckRectIntersect(mouseCursor, trophiesReset))
                {
                    Trophies.Reset();
                }
            }
            else
            {
                if (Rect.CheckRectIntersect(mouseCursor, topTriangle))
                {
                    difficulty++;
                }
                else if (Rect.CheckRectIntersect(mouseCursor, bottomTriangle))
                {
                    difficulty--;
                }
                else if (Rect.CheckRectIntersect(mouseCursor, trophies))
                {
                    showTrophies = true;
                    Trophies.Save();
                }
                else
                {
                    shouldRun = false;
                }
            }
            
        }
        if (difficulty >= 3)
        {
            difficulty = 0;
        }
    }
    public static int GetDifficulty()
    {
        return Math.Abs(difficulty)%3;
    }
    public static bool ShouldRun()
    {
        return shouldRun;
    }
    public static void reset()
    {
        startScreen = Engine.LoadTexture("startScreen/startScreen.png");
        font = Engine.LoadFont("startScreen/font.ttf", 18);
        difficulty = 0;
        topTriangle = new Rect(new Range(880, 905), new Range(45, 70));
        bottomTriangle = new Rect(new Range(880, 905), new Range(135, 160));
        shouldRun = true;
    }
}
