﻿using System;
using System.Collections.Generic;
using System.Text;

class Dodo : Enemy
{
    private readonly float walkSpeed;
    private readonly float runSpeed; // seperate from walkSpeed
    private readonly float chargeSpeed; // determined based on runSpeed
    private float stunSpeed;
    private float chaseDist;
    private readonly int maxHealth;
    private int health;
    private readonly float chargeLength;
    private readonly float chargePauseLength;
    private readonly float stunLength;


    private float timer = 0; // used for changing directions while idling
    private float chargeTimer = 0;
    private bool charge = false;
    private readonly int chargeChance = 2;
    private Vector2 chargeDir;
    private Vector2 stunDir;
    private float stunTimer = 0;
    private float damTimer = 0;
    private float eatTimer = 0;
    private Vector2 move;
    
    private readonly Texture dodoAlive;
    private readonly Texture dodoDead;
    private readonly Texture dodoDamaged;
    private bool mirror;
    private static Vector2 dimentions = new Vector2(58, 82); // dimentions based on texture file

    private Random random = new Random();

    private Vector2 dodoPos;
    private Vector2 deathPos;
    private Player player;
    public Dodo(Vector2 dodoPos, float walkSpeed = 120, float runSpeed = 340, float chaseDist = 400, int maxHealth = 2, float chargeLength = 1.4f, 
        float chargePauseLength = 0.5f, float stunLength = 1f)
    {
        // private variables --------------------
        this.dodoPos = dodoPos;
        this.walkSpeed = walkSpeed;
        this.runSpeed = runSpeed;
        chargeSpeed = runSpeed * 2;
        this.chaseDist = chaseDist;
        this.maxHealth = maxHealth;
        health = maxHealth;
        this.chargeLength = chargeLength;
        this.chargePauseLength = chargePauseLength;
        this.stunLength = stunLength;
        this.chargePauseLength = chargePauseLength;
        // misc variables ----------------------
        move = new Vector2(random.Next(), random.Next());
        dodoAlive = Engine.LoadTexture("textures/dodoAlive.png");
        dodoDead = Engine.LoadTexture("textures/dodoDead.png");
        dodoDamaged = Engine.LoadTexture("textures/dodoDamaged.png");
        
    }

    public static Dodo HardDodo(Vector2 dodoPos)
    {
        return new Dodo(dodoPos);
    }

    public static Dodo MidDodo(Vector2 dodoPos)
    {
        return new Dodo(dodoPos, walkSpeed: 110, runSpeed: 280, chaseDist: 350, 
            chargePauseLength: 0.8f, stunLength: 1.1f);
    }
    public static Dodo EasyDodo(Vector2 dodoPos)
    {
        return new Dodo(dodoPos, walkSpeed: 100, runSpeed: 200, chaseDist: 300, maxHealth: 1, 
            chargePauseLength: 0.9f, stunLength: 1.3f);
    }

    public void Update(Player player, float screenWidth)
    {
        this.player = player;
        if (health > 0 && player.IsAlive())
        {
            Vector2 playerPos = player.Position() + new Vector2(12, 12);
            float dist = (float)Math.Sqrt(Math.Pow(dodoPos.X + dimentions.X / 2 - playerPos.X, 2) +
                Math.Pow(dodoPos.Y + dimentions.Y / 4 - playerPos.Y, 2));
            if (eatTimer > 0 || Rect.CheckRectIntersect(player.getPlayerBounds(), GetBounds()) 
                && stunTimer <= 0)
            {
                if (eatTimer <= 0)
                {
                    // begins eat sequence ------------------------
                    player.SetDeathTimer(1.5f);
                    eatTimer = 2f;
                    dodoPos = playerPos + new Vector2((mirror ? 5 : -dimentions.X - 5), -dimentions.Y / 4);
                    deathPos = new Vector2(dodoPos.X + dimentions.X / 4 + (mirror ? 5 : 0), dodoPos.Y + dimentions.Y / 2);
                    chargeTimer = 0;
                    stunTimer = 0;
                    damTimer = 0;
                }
                player.GetEaten(deathPos);
                eatTimer -= Engine.TimeDelta;
            }
            else
            {
                if (chargeTimer > chargeLength && !charge)
                {
                    // decides if dodo will charge every few seconds
                    if (random.Next(chargeChance) == 1)
                    {
                        charge = true;
                    }
                    chargeTimer = 0;
                }
                if (damTimer > 0)
                {
                    damTimer -= Engine.TimeDelta;
                }
                if (stunTimer > 0)
                {
                    // stun sequence -------------------------------
                    if (stunTimer > stunLength / 1.5 && Move(dodoPos + stunDir * stunSpeed / 10 * Engine.TimeDelta))
                    {
                        dodoPos += stunDir * stunSpeed * Engine.TimeDelta;
                        stunSpeed -= stunSpeed / 5;
                    }
                    stunTimer -= Engine.TimeDelta;
                }
                else
                {
                    // charge, idle, or run sequences -------------
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
        }
        else
        {
            if (!player.isActive())
            {
                player.setActive();
            }
        }
        DrawDodo();
    }

    public void DrawDodo()
    {
        if(health == maxHealth)
        {
            DisplayDodoAlive();
        }
        else if (health <= 0)
        {
            DisplayDodoDead();
        }
        else
        {
            DisplayDodoDamaged();
        }
    }
    
    public void Idle()
    {
        if(health > 0)
        {
            if (timer >= 2)
            {
                // generates new move direction
                timer = 0;
                move = new Vector2(random.Next() * 2 - 1, random.Next() * 2 - 1);
                move = move.Normalized() * walkSpeed;
            }
            if (!Move(dodoPos + move))
            {
                // generates new move direction if previous move dir is invalid
                move = new Vector2(random.Next() * 2 - 1, random.Next() * 2 - 1);
                move = move.Normalized() * walkSpeed;
            }
            else
            {
                dodoPos += move * Engine.TimeDelta;
            }
            mirror = move.X < 0;
            timer += Engine.TimeDelta;
        }
    }

    private void Run(Vector2 playerPos)
    {
        // chases player
        Vector2 dir = new Vector2(playerPos.X + 12 - dodoPos.X - dimentions.X / 2, 
            playerPos.Y + 12 - dodoPos.Y - dimentions.Y / 4).Normalized();
        move = dir * runSpeed;
        dodoPos += move * Engine.TimeDelta;
        mirror = move.X < 0;
    }

    private bool Charge(Vector2 playerPos)
    {
        if (chargeTimer > chargeLength) return false;
        else if (chargeTimer >= chargePauseLength)
        {
            Vector2 move = chargeDir * chargeSpeed;
            if (Move(dodoPos + move * Engine.TimeDelta))
            {
                dodoPos += move * Engine.TimeDelta;
            }
            else
            {
                stunTimer = stunLength;
                stunSpeed = chargeSpeed * 0.5f;
                stunDir = -chargeDir;
                return false;
            }
        }
        // waits for fractions of a second, then generates locked direction for charge
        // towards player with a random element of inacuraccy in player lock
        else if(chargeTimer >= chargePauseLength - 0.05) chargeDir = 
                new Vector2(playerPos.X + 12 - dodoPos.X - dimentions.X / 2 + 
                random.Next(-50, 50), playerPos.Y + 12 - dodoPos.Y - dimentions.Y / 4 + 
                random.Next(-50, 50)).Normalized();
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
        // moves dodo on to player
        dodoPos =  playerPos + new Vector2(mirror ? 0 : -dimentions.X, -dimentions.Y / 4);
        eatTimer = 1.5f;
    }

    public void Damage()
    {
        if (damTimer <= 0)
        {
            // damage and stun sequence
            health--;
            stunDir = player.Rebound(dodoPos);
            stunSpeed = chargeSpeed * -0.3f;
            stunTimer = stunLength;
            damTimer = stunLength;
            Game.PlaySwordHit();
        }
    }

    public static Vector2 GetSize()
    {
        return dimentions;
    }

    public bool IsAlive()
    {
        if (health > 0) return true;
        return false;
    }

    public Rect GetBounds()
    {
        Vector2 bodyTLC = new Vector2((mirror ? 10:2), 2);
        Vector2 size = new Vector2(46, 74);

        return Rect.GetSpriteBounds(bodyTLC + dodoPos, size);
    }
}
