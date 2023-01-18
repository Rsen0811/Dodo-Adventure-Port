using System;
using System.Collections.Generic;
using System.Text;

public class GameOver
{
    Texture end = Engine.LoadTexture("textures/gameOver.png");
    Texture endCredits = Engine.LoadTexture("textures/endCredits.png");
    Font font = Engine.LoadFont("startScreen/font.ttf", 18);
    bool isGameOver;

    public GameOver()
    {
        isGameOver = false;
        writeCoins();
    }

    public void Draw(int score)
    {
        if (Player.VICTORY)
        {
            Engine.DrawTexture(endCredits, Vector2.Zero, size: Game.Resolution);
        }
        else
        {
            Engine.DrawTexture(end, Vector2.Zero, size: Game.Resolution);
            Engine.DrawString("click to continue", new Vector2(480, 480), Color.White, font,
                alignment: TextAlignment.Center);
        }
    }

    private async void writeCoins()
    {
        await Coin.WriteCoins();
    }
    
}
