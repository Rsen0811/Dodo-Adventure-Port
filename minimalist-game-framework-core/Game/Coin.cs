using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

class Coin {

    private readonly int SIZE = 20;

    static StreamReader doc = File.OpenText("Assets/userData/coins.txt");
    static Texture coinTexture = Engine.LoadTexture("textures/coin.png");
    static String[] coins = doc.ReadToEnd().Trim().Split(",");


    public bool collected;
    private Rect coinBounds;
    private int num;
    private Vector2 position;
    

    public Coin (Vector2 pos, int n)
    {       
        position = pos;
        coinBounds = new Rect(new Range(pos.X, pos.X + SIZE), new Range(pos.Y, pos.Y + SIZE));
        num = n;
        collected = int.Parse(coins[num]) == 1;
    }

    public void coinUpdate(Player p)
    {

        if(Rect.CheckRectIntersect(p.getPlayerBounds(), coinBounds) && !collected)
        {
            coins[num] = "1";
            collected = true;
        } 

    }

    public static async Task WriteCoins()
    {

        String[] tempCoins = doc.ReadToEnd().Trim().Split(",");
        String coins2 = coins[0];
        for(int i = 1; i < coins.Length; i++)
        {
            coins2 += "," + coins[i];
        }
        
        doc.Close();
        await File.WriteAllTextAsync("Assets/userData/coins.txt", coins2);
        doc = File.OpenText("Assets/userData/coins.txt");
    }

    public bool isCollected()
    {
        return collected;
    }

    public void Draw()
    {
        Engine.DrawTexture(coinTexture, position);
    }

    public static async void Reset()
    {        
        for (int i = 0; i < coins.Length; i++)
        {
            coins[i] = "0";
            
        }
        await WriteCoins();
    }
}
