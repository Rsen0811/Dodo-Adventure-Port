﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

class Dodo
{
    private float walkSpeed;
    private float runSpeed;
    private float chaseDist;

    private float timer = 0;
    private float damTimer = 0;
    private Vector2 move;
    private int health = 2;
    private Texture dodoAlive;
    private Texture dodoDead;
    private Texture dodoDamaged;
    private bool mirror;

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
        float dist = (float)Math.Sqrt(Math.Pow(dodoPos.X - playerPos.X, 2) +
            Math.Pow(dodoPos.Y - playerPos.Y, 2));
        if (dist < 30 && damTimer <= 0) Damage();
        if(health > 0)
        {
            if (damTimer > 0)
            {
                damTimer -= Engine.TimeDelta;
                displayDodoDamaged(mirror);
            }
            else
            {
                if (dist > chaseDist)
                {
                    Idle();
                }
                else
                {
                    Run(playerPos);
                }
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
        Vector2 dir = new Vector2(playerPos.X - dodoPos.X , playerPos.Y - dodoPos.Y).Normalized();
        Vector2 move = dir * runSpeed;
        dodoPos = Move(dodoPos, move);
        mirror = move.X < 0;
        displayDodoAlive(mirror);
    }
    
    public Vector2 Move(Vector2 start, Vector2 move)
    {
        Vector2 moveTo = start + move;
        if (moveTo.X >= 960 - 70 || moveTo.X <= 15)
        {
            moveTo = new Vector2(start.X, moveTo.Y);
        }
        if (moveTo.Y >= 640 - 90 || moveTo.Y <= 15)
        {
            moveTo = new Vector2(moveTo.X, start.Y);
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

    public void Damage()
    {
        health--;
        damTimer = 1.5f;
    }
}
