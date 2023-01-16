using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;

class Boss
{
    Texture bossAlive = Engine.LoadTexture("textures/dodoAlive.png");
    Texture bossDamaged = Engine.LoadTexture("textures/dodoDamaged.png");
    Texture bossDead = Engine.LoadTexture("textures/dodoDead.png");
    Vector2 pos;
    static readonly Vector2 size = new Vector2(200, 200);
    int maxHealth;
    int health;
    float speed;

    Vector2 move;
    Random random = new Random();
    bool mirror;

    int action;
    float actionTimer;
    float idleTimer;

    float chargeLength;
    Vector2 chargeDir;
    float chargeSpeed;

    double sprayAngle;

    private float stunSpeed;
    private float stunLength;

    private float projectileTimer;
    private float projectileDelay;
    private int projectileAmount;
    private double projectileSpeed;

    List<Projectile> projectiles = new List<Projectile>();

    public Boss(Vector2 pos, int health = 10, float speed = 25, float chargeLength = 6f, 
        float stunLength = 3f, float projectileDelay = 1f, int projectileAmount = 16, 
        double projectileSpeed = 100)
    {
        this.pos = pos;
        this.health = health;
        maxHealth = health;
        this.speed = speed;
        chargeSpeed = speed * 15;
        this.chargeLength = chargeLength;
        this.stunLength = stunLength;
        this.projectileDelay = projectileDelay;
        this.projectileAmount = projectileAmount;
        this.projectileSpeed = projectileSpeed;
    }

    public void Update(Player player)
    {
        if (health > 0)
        {
            if(action != -1 && Rect.CheckRectIntersect(player.getPlayerBounds(), GetBounds()))
            {
                action = -1;
                actionTimer = 5f;
            }
            if (actionTimer <= 0)
            {
                action = random.Next(1, 4);
                switch (action)
                {
                    case 1:
                        actionTimer = chargeLength;
                        break;
                    case 2:
                        actionTimer = 4f;
                        projectileTimer = 0;
                        break;
                    case 3:
                        actionTimer = 3f;
                        projectileTimer = 0;
                        sprayAngle = 0;
                        break;
                }
            }
            switch (action)
            {
                case -1:
                    Eat(player);
                    break;
                case 0:
                    Stun();
                    break;
                case 1:
                    Charge(player.Position());
                    break;
                case 2:
                    Walk(player.Position());
                    FireRadial();
                    break;
                case 3:
                    Walk(player.Position());
                    FireSpray();
                    break;
            }
            actionTimer -= Engine.TimeDelta;
        }
        for(int i = projectiles.Count - 1; i >= 0; i--)
        {
            if (!projectiles[i].InBounds())
            {
                projectiles.RemoveAt(i);
            }
            else
            {
                projectiles[i].Update();
                if(Rect.CheckRectIntersect(projectiles[i].GetBounds(), player.getPlayerBounds()))
                {
                    player.ProjectileRebound(projectiles[i].Pos());
                    projectiles.RemoveAt(i);
                }
            }
        }
    }
    public void Draw()
    {
        if (health >= maxHealth / 2)
        {
            DisplayAlive();
        }
        else if (health <= 0)
        {
            DisplayDead();
        }
        else
        {
            DisplayDamaged();
        }
        for (int i = 0; i < projectiles.Count; i++)
        {
            projectiles[i].Draw();
        }
    }

    public void Walk(Vector2 playerPos)
    {
        if (health > 0)
        {
            if (idleTimer <= 0)
            {
                idleTimer = 3;
                move = new Vector2(random.Next() * 2 - 1, random.Next() * 2 - 1);
                move = move.Normalized() * speed;
            }
            if (!Move(pos + move))
            {
                move = new Vector2(random.Next() * 2 - 1, random.Next() * 2 - 1);
                move = move.Normalized() * speed;
            }
            else
            {
                pos += move * Engine.TimeDelta;
            }
            mirror = playerPos.X < pos.X;
            idleTimer -= Engine.TimeDelta;
        }
    }
    private bool Charge(Vector2 playerPos)
    {
        if (actionTimer <= chargeLength - stunLength / 2)
        {
            Vector2 move = chargeDir * chargeSpeed;
            if (Move(pos + move * Engine.TimeDelta)) pos += move * Engine.TimeDelta;
            else
            {
                action = 0;
                actionTimer = stunLength;
                stunSpeed = chargeSpeed / 2.5f;
            }
        }
        else chargeDir = new Vector2(playerPos.X + 12 - pos.X - size.X / 2 + random.Next(-50, 50),
            playerPos.Y + 12 - pos.Y - size.Y / 4 + random.Next(-50, 50)).Normalized();
        return true;
    }

    private void Stun()
    {
        pos -= chargeDir * stunSpeed * Engine.TimeDelta;
        stunSpeed -= stunSpeed / 20;
    }

    private void FireRadial()
    {
        if(projectileTimer <= 0)
        {
            Vector2 beakPos = new Vector2(pos.X + (mirror ? 15 : size.X - 15), pos.Y + size.Y / 4 - 10);
            for(double angle = 0; angle <= 2*Math.PI; angle += 2*Math.PI / projectileAmount)
            {
                Vector2 dir = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                projectiles.Add(new Projectile(beakPos, dir, projectileSpeed));
            }
            projectileTimer = projectileDelay;
        }
        projectileTimer -= Engine.TimeDelta;
    }
    private void FireSpray()
    {
        if(projectileTimer <= 0)
        {
            Vector2 beakPos = new Vector2(pos.X + (mirror ? 15 : size.X - 15), pos.Y + size.Y / 4 - 10);
            Vector2 dir = new Vector2((float)Math.Cos(sprayAngle), (float)Math.Sin(sprayAngle));
            projectiles.Add(new Projectile(beakPos, dir, projectileSpeed));
            sprayAngle += 2 * Math.PI / (projectileAmount * 2);
            projectileTimer = projectileDelay / (projectileAmount * 2);
        }
        projectileTimer -= Engine.TimeDelta;
    }
    private void Eat(Player player)
    {
        Vector2 beakPos = new Vector2(pos.X + (mirror ? 15 : size.X - 15), pos.Y + size.Y / 4 - 10);
        Vector2 deathPos = new Vector2(pos.X + size.X / 4 + (mirror ? 5 : 0), pos.Y + size.Y / 2);
        player.GetBossEaten(beakPos, deathPos);
    }
    public void Damage(Player player)
    {
       health--;
       player.BossRebound(pos);
    }

    public static Vector2 Size()
    {
        return size;
    }
    public Rect GetBounds()
    {
        Vector2 bodyTLC = new Vector2((mirror ? 10 : 2) * 3.4f, 2 * 2.4f);
        Vector2 size = new Vector2(46 * 3.4f, 74 * 2.4f);
        return Rect.GetSpriteBounds(bodyTLC + pos, size);
    }

    private void DisplayAlive()
    {
        Engine.DrawTexture(bossAlive, pos, size: size, mirror: mirror ? TextureMirror.Horizontal :
            TextureMirror.None);
    }

    private void DisplayDead()
    {
        Engine.DrawTexture(bossDead, pos, size: size, mirror: mirror ? TextureMirror.Horizontal :
            TextureMirror.None);
    }
    private void DisplayDamaged()
    {
        Engine.DrawTexture(bossDamaged, pos, size: size, mirror: mirror ? TextureMirror.Horizontal :
            TextureMirror.None);
    }

    public bool Move(Vector2 moveTo)
    {
        if (moveTo.X >= Game.Resolution.X - size.X)
        {
            return false;
        }
        else if (moveTo.X <= 0)
        {
            return false;
        }
        if (moveTo.Y >= Game.Resolution.Y - size.Y)
        {
            return false;
        }
        else if (moveTo.Y <= 0)
        {
            return false;
        }
        return true;
    }
}
