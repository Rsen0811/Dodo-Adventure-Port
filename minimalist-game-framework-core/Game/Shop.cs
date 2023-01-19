using System;
using System.Collections.Generic;
using System.Text;


class Shop
{
    static Texture[] skins= {Engine.LoadTexture("textures/Player/playerBlue.png"),
                             Engine.LoadTexture("textures/Player/playerRed.png"),
                             Engine.LoadTexture("textures/Player/playerViolet.png"),

                             Engine.LoadTexture("textures/Player/playerBlack.png"),
                             Engine.LoadTexture("textures/Player/playerGray.png"),
                             Engine.LoadTexture("textures/Player/playerCyan.png"),
                             Engine.LoadTexture("textures/Player/playerGreen.png"),
                             Engine.LoadTexture("textures/Player/playerPink.png"),
                             Engine.LoadTexture("textures/Player/playerYellow.png"),
                             Engine.LoadTexture("textures/Player/playerOrange.png")
        };
    static int currSkin = 0;
    static int currShowSkin = 0;
    static Texture shopBackground = Engine.LoadTexture("startScreen/shopScreen/skinRoomBackground.png");
    static Font font = Engine.LoadFont("startScreen/font.ttf", 16);
    static Texture lockTex =  Engine.LoadTexture("startScreen/shopScreen/lock.png");
    static Rect leftArrow = new Rect(new Range(5,55),new Range(265, 355));
    static Rect rightArrow = new Rect(new Range(900,950), new Range(260,350));
    static Rect selectButton = new Rect(new Range(390,550), new Range(515,600));
    public static void Update()
    {
        if (Engine.GetMouseButtonDown(MouseButton.Left))
        {
            Rect mouseCursor = new Rect(new Range(Engine.MousePosition.X, Engine.MousePosition.X),
                                        new Range(Engine.MousePosition.Y, Engine.MousePosition.Y));
            if (Rect.CheckRectIntersect(mouseCursor, leftArrow))
            {
                currShowSkin = (currShowSkin - 1) % (skins.Length);
                if (currShowSkin == -1)
                {
                    currShowSkin = skins.Length - 1;
                }
            }else if (Rect.CheckRectIntersect(mouseCursor,rightArrow))
            {
                currShowSkin = (currShowSkin + 1) % (skins.Length);
            }
            //if select
            else if (Coin.coins[currShowSkin].Equals("1") && currShowSkin != currSkin && Rect.CheckRectIntersect(mouseCursor, selectButton))
            {
                currSkin = currShowSkin;
            }
        }
    }
    public static void Draw()
    {
        Engine.DrawTexture(shopBackground, Vector2.Zero, size: Game.Resolution);
        Engine.DrawString("Back", new Vector2(840, 40), Color.White, font);

        int s = currShowSkin;
        Engine.DrawTexture(skins[currShowSkin], new Vector2(270, 100), size: new Vector2(400, 400), source: new Bounds2(0, 0, 48, 48));
        //not yet unlocked
        if (Coin.coins[currShowSkin].Equals("0"))
        {
            Engine.DrawTexture(lockTex, new Vector2(370, 300), size: new Vector2(200, 200));
            Engine.DrawString("Locked", new Vector2(420, 555), Color.LightGray, font);
        }
        else
        {
            if (currShowSkin == currSkin)
            {
                Engine.DrawString("Selected", new Vector2(410, 555), Color.LightGray, font);
            }
            else
            {
                Engine.DrawString("Select", new Vector2(420, 555), Color.LightGray, font);
            }
        }
        Engine.DrawString(""+currShowSkin+" "+currSkin, new Vector2(590, 555), Color.LightGray, font);

    }
    public static void ResetSkin()
    {
        currShowSkin = currSkin;
    }
    public static Texture GetPlayerTexture()
    {
        return skins[currSkin];
    }
}

