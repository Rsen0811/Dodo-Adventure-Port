using System;
using System.Collections.Generic;
using System.Text;

class Dodo
{
    private readonly float walkSpeed;
    private readonly float runSpeed;
    private readonly float chargeSpeed;
    private float stunSpeed;
    private float chaseDist;

    private float timer = 0;
    private float chargeTimer = 0;
    private bool charge = false;
    private readonly int chargeChance = 2;
    private Vector2 chargeDir;
    private float stunTimer = 0;
    private float damTimer = 0;
    private float eatTimer = 0;
    private Vector2 move;
    private int health = 2;
    private readonly Texture dodoAlive;
    private readonly Texture dodoDead;
    private readonly Texture dodoDamaged;
    private bool mirror;
    private Vector2 dimentions = new Vector2(58, 82);

    private Random random = new Random();

    private Vector2 dodoPos;
    public Dodo(Vector2 dodoPos)
    {
        this.dodoPos = dodoPos;
        walkSpeed = 1;
        runSpeed = 5;
        chargeSpeed = 10;
        chaseDist = 400;
        move = new Vector2(random.Next(), random.Next());
        dodoAlive = Engine.LoadTexture("textures/dodoAlive.png");
        dodoDead = Engine.LoadTexture("textures/dodoDead.png");
        dodoDamaged = Engine.LoadTexture("textures/dodoDamaged.png");
    }
    public Dodo(float walkSpeed, float runSpeed, float chargeSpeed, Vector2 dodoPos, float chaseDist, 
        Texture dodoAlive, Texture dodoDead, Texture dodoDamaged)
    {
        this.walkSpeed = walkSpeed;
        this.runSpeed = runSpeed;
        this.chargeSpeed = chargeSpeed;
        this.dodoPos = dodoPos;
        this.chaseDist = chaseDist;
        this.dodoAlive = dodoAlive;
        this.dodoDead = dodoDead;
        this.dodoDamaged = dodoDamaged;
        move = new Vector2(random.Next(), random.Next());
    }

    public void Update(Vector2 playerPos, float screenWidth)
    {
        if (health > 0)
        {
            playerPos += new Vector2(12, 12);
            float dist = (float)Math.Sqrt(Math.Pow(dodoPos.X + dimentions.X / 2 - playerPos.X, 2) +
                Math.Pow(dodoPos.Y + dimentions.Y / 4 - playerPos.Y, 2));
            if (dist < 30) Damage();
            if (chargeTimer > 1.4 && !charge)
            {
                if (random.Next(chargeChance) == 1)
                {
                    charge = true;
                }
                chargeTimer = 0;
            }

            if (stunTimer > 0)
            {
                if (stunTimer > 0.65 && Move(dodoPos + chargeDir * -1))
                {
                    dodoPos += chargeDir * stunSpeed;
                    stunSpeed -= stunSpeed / 10;
                }
                stunTimer -= Engine.TimeDelta;
            }
            else if (eatTimer > 0)
            {
                eatTimer -= Engine.TimeDelta;
            }
            else if (damTimer > 0)
            {
                damTimer -= Engine.TimeDelta;
            }
            else
            {
                if (charge)
                {
                    chaseDist = screenWidth;
                    charge = Charge(playerPos);
                }
                else if (dist > chaseDist)
                {
                    Idle();
                }

                else
                {
                    Run(playerPos);
                    chargeTimer += Engine.TimeDelta;
                }
            }
        }
        DrawDodo();
    }

    public void DrawDodo()
    {
        switch(health)
        { 
            case 2: 
                DisplayDodoAlive();
                break;
            case 1:
                DisplayDodoDamaged();
                break;
            case 0:
                DisplayDodoDead();
                break;
            default:
                DisplayDodoAlive();
                break;
        }
    }
    
    public void Idle()
    {
        if(health > 0)
        {
            if (timer >= 2)
            {
                timer = 0;
                move = new Vector2(random.Next() * 2 - 1, random.Next() * 2 - 1);
                move = move.Normalized() * walkSpeed;
            }
            if (!Move(dodoPos + move))
            {
                move = new Vector2(random.Next() * 2 - 1, random.Next() * 2 - 1);
                move = move.Normalized() * walkSpeed;
            }
            else
            {
                dodoPos += move;
            }
            mirror = move.X < 0;
            timer += Engine.TimeDelta;
        }
    }

    private void Run(Vector2 playerPos)
    {
        Vector2 dir = new Vector2(playerPos.X + 12 - dodoPos.X - dimentions.X / 2, 
            playerPos.Y + 12 - dodoPos.Y - dimentions.Y / 4).Normalized();
        move = dir * runSpeed;
        if (Move(dodoPos + move))
        {
            dodoPos += move;
        }
        mirror = move.X < 0;
    }

    private bool Charge(Vector2 playerPos)
    {
        if (chargeTimer > 1.4) return false;
        else if (chargeTimer >= 0.5)
        {
            Vector2 move = chargeDir * chargeSpeed;
            if (Move(dodoPos + move))
            {
                dodoPos += move;
            }
            else
            {
                stunTimer = 1;
                stunSpeed = chargeSpeed * -0.5f;
                return false;
            }
        }
        else if(chargeTimer >= 0.45) chargeDir = new Vector2(playerPos.X + 12 - dodoPos.X - 
            dimentions.X / 2, playerPos.Y + 12 - dodoPos.Y - dimentions.Y / 4).Normalized();
        chargeTimer += Engine.TimeDelta;
        return true;
    }
    
    public bool Move(Vector2 moveTo)
    {
        if (moveTo.X >= 890)
        {
            return false;
        }
        else if (moveTo.X <= 15)
        {
            return false;
        }
        if (moveTo.Y >= 570)
        {
            return false;
        }
        else if(moveTo.Y <= 15)
        {
            return false;
        }
        return true;
    }

    private void DisplayDodoAlive()
    {
        Engine.DrawTexture(dodoAlive, dodoPos, mirror: mirror ? TextureMirror.Horizontal : 
            TextureMirror.None);
    }

    private void DisplayDodoDead()
    {
        Engine.DrawTexture(dodoDead, dodoPos, mirror: mirror ? TextureMirror.Horizontal :
            TextureMirror.None);
    }
    private void DisplayDodoDamaged()
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
        if (damTimer <= 0)
        {
            health--;
            damTimer = 1.5f;
        }
    }

    public Rect getBounds()
    {
        Vector2 bodyTLC = new Vector2((mirror ? 10:2), 2);
        Vector2 size = new Vector2(46, 74);

        return Rect.getSpriteBounds(bodyTLC + dodoPos, size);
    }
}
