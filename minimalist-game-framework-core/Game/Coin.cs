using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

class Coin {

    private readonly int SIZE = 20;

    static StreamReader doc = File.OpenText("Assets/userData/coins.txt");

    public bool collected;
    private Rect bounds;
    private int num;


    public Coin (Vector2 pos, int n)
    {
        bounds = new Rect(new Range(pos.X, pos.X + SIZE), new Range(pos.Y, pos.Y + SIZE));
        num = n;
        collected = false;
    }

    public async void coinUpdate(Player p)
    {
        if(Rect.CheckRectIntersect(p.getPlayerBounds(), bounds))
        {
            collected = true;

            String[] coins = doc.ReadToEnd().Split("");
            coins[num] = "1";
            await WriteCoins(coins);
        }

    }

    private static async Task WriteCoins(string[] coins)
    {
        
        doc.Close();
        await File.WriteAllLinesAsync("Assets/userData/trophies.txt", coins);
    }

    public bool isCollected()
    {
        return collected;
    }

}
