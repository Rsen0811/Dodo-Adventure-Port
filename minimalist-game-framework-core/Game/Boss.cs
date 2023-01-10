using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

class Boss
{
    Texture bossAlive = Engine.LoadTexture("textures/dodoAlive");
    Texture bossDamaged = Engine.LoadTexture("textures/dodoDamaged");
    Texture bossDead = Engine.LoadTexture("textures/dodoDead");
    Vector2 pos;
    Vector2 size = new Vector2(200, 200);
    Player player;
    int maxHealth;
    int health;
    float speed;
    float idleTimer;
    Vector2 move;
    Random random = new Random();

    public Boss(Vector2 pos, int health = 10, float speed = 100)
    {
        this.pos = pos;
        this.health = health;
        maxHealth = health;
        this.speed = speed;
    }

    public void Update(Player player, float screenWidth)
    {
        this.player = player;
        if(health > 0 && player.IsAlive())
        {

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
        Engine.DrawTexture(bossAlive, pos, size: size);
    }

    public void Walk ()
    {
        if (health > 0)
        {
            if (idleTimer >= 3)
            {
                idleTimer = 0;
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
            idleTimer += Engine.TimeDelta;
        }
    }

    private void DisplayAlive()
    {
        Engine.DrawTexture(bossAlive, pos, size: size);
    }

    private void DisplayDead()
    {
        Engine.DrawTexture(bossDead, pos, size: size);
    }
    private void DisplayDamaged()
    {
        Engine.DrawTexture(bossDamaged, pos, size: size);
    }

    public bool Move(Vector2 moveTo)
    {
        if (moveTo.X >= 880)
        {
            return false;
        }
        else if (moveTo.X <= 25)
        {
            return false;
        }
        if (moveTo.Y >= 560)
        {
            return false;
        }
        else if (moveTo.Y <= 25)
        {
            return false;
        }
        return true;
    }
}
