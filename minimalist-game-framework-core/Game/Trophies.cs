using System;
using System.IO;
using System.Threading.Tasks;

class Trophies
{
    static Texture trophyEmpty = Engine.LoadTexture("startScreen/trophiesScreen/trophyEmpty.png");
    static Texture trophyFilled = Engine.LoadTexture("startScreen/trophiesScreen/trophyFilled.png");
    static readonly Font font = Engine.LoadFont("startScreen/font.ttf",18);
    static StreamReader doc = File.OpenText("Assets/userData/trophies.txt");
    static String[] trophies = doc.ReadToEnd().Split("-----");
    static String[] trophyVals = trophies[1].Split(",");
    static bool[] complete = new bool[trophyVals.Length];

    static int dodosKilled = int.Parse(trophyVals[0]);
    static bool beatEasy = bool.Parse(trophyVals[1]);
    static TimeSpan easyTime = TimeSpan.FromSeconds(float.Parse(trophyVals[2]));
    static bool beatMid = bool.Parse(trophyVals[3]);
    static TimeSpan midTime = TimeSpan.FromSeconds(float.Parse(trophyVals[4]));
    static bool beatHard = bool.Parse(trophyVals[5]);
    static TimeSpan hardTime = TimeSpan.FromSeconds(float.Parse(trophyVals[6]));
    public static void Draw()
    {
        Engine.DrawRectSolid(new Bounds2(Vector2.Zero, Game.Resolution), Color.Black);
        for(int i = 0; i < trophyVals.Length; i++)
        {
            // draws trophies, either filled or unfilled, based on complete[] array
            Engine.DrawTexture(complete[i] ? trophyFilled : trophyEmpty, new Vector2(48, 20 + 90 * i));
        }
        // trophy labels ---------------------------
        Engine.DrawString("Dodos Killed: " + dodosKilled + " / " + 15, new Vector2(140, 44), 
            Color.White, font);
        Engine.DrawString("Beat Easy", new Vector2(140, 134), Color.White, font);
        if(beatEasy)
        {
            Engine.DrawString("Easy Time: " + Math.Truncate(easyTime.TotalMinutes) + ":" + 
                easyTime.Seconds + " / " + "15:00" , new Vector2(140, 224), Color.White, font);
        }
        Engine.DrawString("Beat Medium", new Vector2(140, 314), Color.White, font);
        if (beatMid)
        {
            Engine.DrawString("Medium Time: " + Math.Truncate(midTime.TotalMinutes) + ":" + 
                midTime.Seconds + " / " + "15:00", new Vector2(140, 404), Color.White, font);
        }
        Engine.DrawString("Beat Hard", new Vector2(140, 494), Color.White, font);
        if (beatHard)
        {
            Engine.DrawString("Hard Time: " + Math.Truncate(hardTime.TotalMinutes) + ":" + 
                midTime.Seconds + " / " + "15:00", new Vector2(140, 584), Color.White, font);
        }

        // back and reset buttons
        Engine.DrawString("Back", new Vector2(840, 40), Color.White, font);
        Engine.DrawString("Reset", new Vector2(840, 588), Color.Red, font);
    }

    public static async void Save()
    {
        // converts bool, int, and time values into string for easy printing
        trophyVals.SetValue(dodosKilled.ToString(), 0);
        trophyVals.SetValue(beatEasy.ToString(), 1);
        trophyVals.SetValue(easyTime.TotalSeconds.ToString(), 2);
        trophyVals.SetValue(beatMid.ToString(), 3);
        trophyVals.SetValue(midTime.TotalSeconds.ToString(), 4);
        trophyVals.SetValue(beatHard.ToString(), 5);
        trophyVals.SetValue(hardTime.TotalSeconds.ToString(), 6);

        // updates whether trophy was gained
        complete[0] = int.Parse(trophyVals[0]) >= 15;
        complete[1] = bool.Parse(trophyVals[1]);
        complete[2] = float.Parse(trophyVals[2]) <= 900f;
        complete[3] = bool.Parse(trophyVals[3]);
        complete[4] = float.Parse(trophyVals[4]) <= 600f;
        complete[5] = bool.Parse(trophyVals[5]);
        complete[6] = float.Parse(trophyVals[6]) <= 480f;

        // formats trophies string to be printed in txt doc
        trophies.SetValue(trophyVals[0], 1);
        for(int i = 1; i < trophyVals.Length; i++)
        {
            trophies[1] += "," + trophyVals[i];
        }
        await WriteTrophies();
    }
    public static void Reset()
    {
        // sets trophy progress to base value
        dodosKilled = 0;
        beatEasy = false;
        easyTime = easyTime.Multiply(0).Add(TimeSpan.FromSeconds(5999));
        beatMid = false;
        midTime = midTime.Multiply(0).Add(TimeSpan.FromSeconds(5999));
        beatHard = false;
        hardTime = hardTime.Multiply(0).Add(TimeSpan.FromSeconds(5999));
        // resets coin collection as well
        Coin.Reset();
        Save();
    }
    public static void KillDodo(int dodos = 1)
    {
        dodosKilled += dodos;
    }

    public static void BeatEasy(float time)
    {
        beatEasy = true;
        if (time < easyTime.TotalSeconds) easyTime = easyTime.Multiply(0).Add(TimeSpan.FromSeconds(time));
    }

    public static void BeatMid(float time)
    {
        beatMid = true;
        if (time < midTime.TotalSeconds) midTime = midTime.Multiply(0).Add(TimeSpan.FromSeconds(time));
    }

    public static void BeatHard(float time)
    {
        beatHard = true;
        if (time < hardTime.TotalSeconds) hardTime = hardTime.Multiply(0).Add(TimeSpan.FromSeconds(time));
    }

    private static async Task WriteTrophies()
    {
        string[] lines =
        {
            trophies[0], "-----", trophies[1]
        };
        doc.Close();    //Closes StreamReader access to trophies file
        await File.WriteAllLinesAsync("Assets/userData/trophies.txt", lines);
    }
}
