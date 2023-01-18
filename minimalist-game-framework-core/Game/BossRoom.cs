using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

class BossRoom : Room
{
    private Boss boss;
    private String enterGate;
    private String exitGate;
    private bool bossIsDead = false;
    public BossRoom(Vector2 pos) : base(pos)
    {
        String name = "" + pos.X + pos.Y;
        ReadBoss("rooms/" + name + "/" + name + "b.txt");
    }
    public void Update(Player p)
    {
        if (base.GetGate(enterGate).isOpen == true && boss.isAlive()&&!Rect.CheckRectIntersect(p.getPlayerBounds(), base.GetGate(enterGate)))
        {
            base.toggleGate(enterGate);
        }
        base.Update(p);
        boss.Update(p);
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].Update(p, Game.Resolution.X);
        }
        if (!boss.isAlive() &&bossIsDead && base.allDead())
        {
            bossIsDead = !bossIsDead;
            base.toggleGate(enterGate);
            base.toggleGate(exitGate);
        }
    }
    public void DrawRoom()
    {
        base.DrawRoom();
        boss.Draw();
    }
    private void ReadBoss(String file)
    {
        using (StreamReader sr = File.OpenText("Assets/" + file))
        {

            String bossPos = sr.ReadLine();
            enterGate = sr.ReadLine().Trim();
            exitGate = sr.ReadLine().Trim();
            int health = 0;
            int projectileAmount = 0;
            switch(StartScreen.GetDifficulty())
            {
                case 0:
                    health = 10;
                    projectileAmount = 8;
                    break;
                case 1:
                    health = 20;
                    projectileAmount = 12;
                    break;
                case 2:
                    health = 30;
                    projectileAmount = 16;
                    break;
            }
            boss = new Boss(new Vector2(int.Parse(bossPos.Split()[0]), int.Parse(bossPos.Split()[1])), 
                this, health: health, projectileAmount: projectileAmount);
            //three lines
            //Line #1
            //boss pos
            //X Y
            //Line #2
            //gateName
            //Line #3 
            //gateName
            return;
        }
    }
}

