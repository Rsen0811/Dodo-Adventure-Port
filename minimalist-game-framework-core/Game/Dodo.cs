using System;
using System.Collections.Generic;
using System.Text;

class Dodo
{
    private float walkSpeed;
    private float runSpeed;
    private float chargeSpeed;
    private float chaseDist;

    private float timer = 0;
    private float chargeTimer = 0;
    private bool charge = false;
    private int chargeChance = 3;
    private float damTimer = 0;
    private float eatTimer = 0;
    private Vector2 move;
    private int health = 2;
    private Texture dodoAlive;
    private Texture dodoDead;
    private Texture dodoDamaged;
    private bool mirror;
    private Vector2 dimentions = new Vector2(58, 82);

    private bool testEating = false;

    private Random random = new Random();

    private Vector2 dodoPos;
    public Dodo(Vector2 dodoPos)
    {
        this.dodoPos = dodoPos;
        walkSpeed = 1;
        runSpeed = 5;
        chaseDist = 400;
        move = new Vector2(random.Next(), random.Next());
        dodoAlive = Engine.LoadTexture("textures/dodoAlive.png");
        dodoDead = Engine.LoadTexture("textures/dodoDead.png");
        dodoDamaged = Engine.LoadTexture("textures/dodoDamaged.png");
    }
    public Dodo(float walkSpeed, float runSpeed, Vector2 dodoPos, float chaseDist, 
        Texture dodoAlive, Texture dodoDead, Texture dodoDamaged)
    {
        this.walkSpeed = walkSpeed;
        this.runSpeed = runSpeed;
        this.dodoPos = dodoPos;
        this.chaseDist = chaseDist;
        this.dodoAlive = dodoAlive;
        this.dodoDead = dodoDead;
        this.dodoDamaged = dodoDamaged;
        move = new Vector2(random.Next(), random.Next());
    }

    public void Update(Vector2 playerPos, float screenWidth)
    {
        playerPos += new Vector2(12, 12);
        float dist = (float)Math.Sqrt(Math.Pow(dodoPos.X + dimentions.X / 2 - playerPos.X, 2) +
            Math.Pow(dodoPos.Y + dimentions.Y / 4 - playerPos.Y, 2));
        if(chargeTimer > 2 && !charge)
        {
            if(random.Next(chargeChance) == 1) charge = true;
            chargeTimer = 0;
        }
        if (dist < 30 && !testEating) Eat(playerPos);
        if(health > 0)
        {
            if (damTimer > 0)
            {
                damTimer -= Engine.TimeDelta;
                displayDodoDamaged(mirror);
            }
            else if (eatTimer > 0)
            {
                eatTimer -= Engine.TimeDelta;
                displayDodoAlive(mirror);
            }
            else
            {
                if (dist > chaseDist)
                {
                    Idle();
                }
                else
                {
                    if (charge) charge = Charge(playerPos);
                    else
                    {
                        Run(playerPos);
                        chargeTimer++;
                    }
                }
                testEating = false;
            }
        }
        else displayDodoDead(mirror);
    }
    
    private void Idle()
    {
        if(timer >= 2)
        {
            timer = 0;
            move = new Vector2(random.Next() * 2 - 1, random.Next() * 2 - 1);
        }
        move = move.Normalized() * walkSpeed;
        dodoPos = Move(dodoPos, move);
        mirror = move.X < 0;
        displayDodoAlive(mirror);
        timer += Engine.TimeDelta;
    }

    private void Run(Vector2 playerPos)
    {
        Vector2 dir = new Vector2(playerPos.X + 12 - dodoPos.X - dimentions.X / 2, 
            playerPos.Y + 12 - dodoPos.Y - dimentions.Y / 4).Normalized();
        Vector2 move = dir * runSpeed;
        dodoPos = Move(dodoPos, move);
        mirror = move.X < 0;
        displayDodoAlive(mirror);
    }

    private bool Charge(Vector2 playerPos)
    {

    }
    
    public Vector2 Move(Vector2 start, Vector2 move)
    {
        Vector2 moveTo = start + move;
        if (moveTo.X >= 960 - 70)
        {
            moveTo = new Vector2(960 - 71, moveTo.Y);
        }
        else if (moveTo.X <= 15)
        {
            moveTo = new Vector2(16, moveTo.Y);
        }
        if (moveTo.Y >= 640 - 70)
        {
            moveTo = new Vector2(moveTo.X, 640 - 71);
        }
        else if(moveTo.Y <= 15)
        {
            moveTo = new Vector2(moveTo.X, 16);
        }
        return moveTo;
    }

    private void displayDodoAlive(bool mirror)
    {
        Engine.DrawTexture(dodoAlive, dodoPos, mirror: mirror ? TextureMirror.Horizontal : 
            TextureMirror.None);
    }

    private void displayDodoDead(bool mirror)
    {
        Engine.DrawTexture(dodoDead, dodoPos, mirror: mirror ? TextureMirror.Horizontal :
            TextureMirror.None);
    }
    private void displayDodoDamaged(bool mirror)
    {
        Engine.DrawTexture(dodoDamaged, dodoPos, mirror: mirror ? TextureMirror.Horizontal :
            TextureMirror.None);
    }

    public void Eat(Vector2 playerPos)
    {
        dodoPos =  playerPos + new Vector2(mirror ? 0 : -dimentions.X, -dimentions.Y / 4);
        eatTimer = 1.5f;
    }

    public void StopEat()
    {
        eatTimer = 0f;
    }

    public void Damage()
    {
        health--;
        damTimer = 1.5f;
        testEating = true;
    }
}
