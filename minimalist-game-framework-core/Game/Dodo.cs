using System;
using System.Collections.Generic;
using System.Text;

class Dodo
{
    private float walkSpeed;
    private float runSpeed;
    private float chaseDist;

    private Player player;
    private float timer = 0;
    private float damTimer = 0;
    private Vector2 move;
    private int health = 2;
    private Texture dodoAlive;
    private Texture dodoDead;
    private Texture dodoDamaged;

    private Random random = new Random();

    private Vector2 dodoPos;
    public Dodo(Player player)
    {
        walkSpeed = 5;
        runSpeed = 10;
        dodoPos = new Vector2(50, 50);
        chaseDist = 100;
        this.player = player;
        move = new Vector2(random.Next(), random.Next());
        dodoAlive = Engine.LoadTexture("dodoAlive.png");
        dodoAlive = Engine.LoadTexture("dodoDead.png");
        dodoAlive = Engine.LoadTexture("dodoDamaged.png");
    }
    public Dodo(float walkSpeed, float runSpeed, Vector2 dodoPos, float chaseDist, Player player, 
        Texture dodoAlive, Texture dodoDead, Texture dodoDamaged)
    {
        this.walkSpeed = walkSpeed;
        this.runSpeed = runSpeed;
        this.dodoPos = dodoPos;
        this.chaseDist = chaseDist;
        this.player = player;
        this.dodoAlive = dodoAlive;
        this.dodoDead = dodoDead;
        this.dodoDamaged = dodoDamaged;
        move = new Vector2(random.Next(), random.Next());
    }

    public void Update(Vector2 playerPos, float screenWidth)
    {
        float dist = (float)Math.Sqrt(Math.Pow(dodoPos.X - playerPos.X, 2) +
            Math.Pow(dodoPos.Y - playerPos.Y, 2));
        if(health > 0)
        {
            if (damTimer > 0)
            {
                damTimer -= Engine.TimeDelta;
                displayDodoDamaged();
            }
            if (dist > chaseDist)
            {
                Idle();
                displayDodoAlive();
            }
            else
            {
                Run();
                displayDodoAlive();
            }
        }
        else
        {
            displayDodoDead();
        }
    }
    
    private void Idle()
    {
        if(timer >= 2)
        {
            timer = 0;
            move = new Vector2(random.Next(), random.Next());
        }
        move = move.Normalized() * walkSpeed;
        dodoPos = Move(dodoPos, move);
        timer += Engine.TimeDelta;
    }

    private void Run()
    {
        Vector2 move = player.getPos().Normalized() * runSpeed;
        dodoPos = Move(dodoPos, move);
    }
    
    public Vector2 Move(Vector2 start, Vector2 move)
    {
        Vector2 moveTo = start + move;
        if (moveTo.X >= 960 - 15 || moveTo.X <= 0 + 15)
        {
            moveTo = new Vector2(0, moveTo.Y);
        }
        if (moveTo.X >= 640 - 15 || moveTo.X <= 0 + 15)
        {
            moveTo = new Vector2(moveTo.X, 0);
        }
        return moveTo;
    }

    private void displayDodoAlive()
    {
        Engine.DrawTexture(dodoAlive, dodoPos);
    }

    private void displayDodoDead()
    {
        Engine.DrawTexture(dodoDead, dodoPos);
    }
    private void displayDodoDamaged()
    {
        Engine.DrawTexture(dodoDamaged, dodoPos);
    }

    public void damage()
    {
        health--;
        damTimer = 1.5f;
    }
}
