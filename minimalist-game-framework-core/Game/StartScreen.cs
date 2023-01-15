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
    static Rect trophies = new Rect(new Range(1.5f * Game.Resolution.X / 8 +15 , 335), new Range(Game.Resolution.Y / 8 + 10, 115));
    static Rect skins = new Rect(new Range(0.6f * Game.Resolution.X / 8 + 15, 1.5f * Game.Resolution.X / 8 + 20), new Range(Game.Resolution.Y / 8 + 15, 115));
    static Rect back = new Rect(new Range(830, 960), new Range(30, 70));
    
    static Rect trophiesReset = new Rect(new Range(840, 940), new Range(588, 620));

    static bool shouldRun=true;
    static bool showTrophies = false;
    static bool showSkins = false;

    public static void Draw()
    {
        if (showSkins)
        {
            Shop.Draw();
        }
        else if (showTrophies)
        {
            Trophies.Draw();
        }
        else
        {
            Engine.DrawTexture(startScreen, Vector2.Zero, size: Game.Resolution);
            Engine.DrawString("Skins", new Vector2(0.6f * Game.Resolution.X / 8+20 , Game.Resolution.Y / 8 + 15), Color.White, font);
            Engine.DrawString("Trophies", new Vector2(1.5f * Game.Resolution.X / 8+20 , Game.Resolution.Y / 8 + 15), Color.White, font);

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
        Engine.DrawString(Engine.MousePosition.X + " " + Engine.MousePosition.Y, new Vector2(350, 350), Color.White, font);
    }
    public static void Update()
    {
        if (Engine.GetMouseButtonDown(MouseButton.Left))
        {
            Rect mouseCursor = new Rect(new Range(Engine.MousePosition.X, Engine.MousePosition.X), 
                                        new Range(Engine.MousePosition.Y, Engine.MousePosition.Y));
            if (showSkins)
            {
                if (Rect.CheckRectIntersect(mouseCursor, back))
                {
                    showSkins = false;
                    Shop.ResetSkin();
                }
                Shop.Update();
            }
            else if (showTrophies)
            {
                if (Rect.CheckRectIntersect(mouseCursor, back))
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
                else if(Rect.CheckRectIntersect(mouseCursor, skins))
                {
                    showSkins = true;
                    Shop.ResetSkin();
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
        topTriangle = new Rect(new Range(880, 905), new Range(45, 70));
        bottomTriangle = new Rect(new Range(880, 905), new Range(135, 160));
        shouldRun = true;
    }
}
