﻿using System;
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
        if (boss != null)
        {
            if (base.GetGate(enterGate).isOpen == true && boss.IsAlive() && !Rect.CheckRectIntersect(p.getPlayerBounds(), base.GetGate(enterGate)))
            {
                base.toggleGate(enterGate);
            }
            base.Update(p);
            boss.Update(p);
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Update(p, Game.Resolution.X);
            }
            if (!boss.IsAlive() && !bossIsDead && base.allDead())
            {
                bossIsDead = !bossIsDead;
                base.toggleGate(enterGate);
                base.toggleGate(exitGate);
            }
        }
        else
        {
            if (base.GetGate(enterGate).isOpen == true && !base.allDead() && !Rect.CheckRectIntersect(p.getPlayerBounds(), base.GetGate(enterGate)))
            {
                base.toggleGate(enterGate);
            }
            base.Update(p);
            if (!bossIsDead && base.allDead())
            {
                bossIsDead = !bossIsDead;
                base.toggleGate(enterGate);
                base.toggleGate(exitGate);
            }
        }
    }
    public void DrawRoom()
    {
        base.DrawRoom();
        if (boss != null)
        {
            boss.Draw();
        }
    }
    private void ReadBoss(String file)
    {
        using (StreamReader sr = File.OpenText("Assets/" + file))
        {

            
            enterGate = sr.ReadLine().Trim();
            exitGate = sr.ReadLine().Trim();
            String bossPos = sr.ReadLine();
            int health = 0;
            int projectileAmount = 0;
            if (bossPos != null)
            {
                switch (StartScreen.GetDifficulty())
                {
                    case 0:
                        health = 10;
                        projectileAmount = 6;
                        break;
                    case 1:
                        health = 18;
                        projectileAmount = 8;
                        break;
                    case 2:
                        health = 25;
                        projectileAmount = 12;
                        break;
                }
                boss = new Boss(new Vector2(int.Parse(bossPos.Split()[0]), int.Parse(bossPos.Split()[1])),
                    this, health: health, projectileAmount: projectileAmount);
            }
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

