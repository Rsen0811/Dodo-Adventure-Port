using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

class Coin {

    private readonly int SIZE = 20;

    static StreamReader doc = File.OpenText("Assets/userData/coins.txt");
    static Texture coinTexture = Engine.LoadTexture("textures/coin.png");


    public bool collected;
    private Rect coinBounds;
    private int num;
    private Vector2 position;

    public Coin (Vector2 pos, int n)
    {
        position = pos;
        coinBounds = new Rect(new Range(pos.X, pos.X + SIZE), new Range(pos.Y, pos.Y + SIZE));
        num = n;
        collected = false;
    }

    public async void coinUpdate(Player p)
    {
        if(Rect.CheckRectIntersect(p.getPlayerBounds(), coinBounds))
        {
            
            String[] coins = doc.ReadToEnd().Split(",");
            coins[num] = "1";
            await WriteCoins(coins);

            collected = true;
        } 

    }

    private static async Task WriteCoins(String[] coins)
    {
        String coins2 = coins[0];
        for(int i = 1; i < coins.Length - 1; i++)
        {
            coins2 += "," + coins[i];
        }
        String[] coins3 = { coins2 };
        doc.Close();
        await File.WriteAllLinesAsync("Assets/userData/coins.txt", coins3);
    }

    public bool isCollected()
    {
        return collected;
    }

    public void Draw()
    {
        Engine.DrawTexture(coinTexture, position);
    }
}
