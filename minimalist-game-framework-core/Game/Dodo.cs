using System;
using System.Collections.Generic;
using System.Text;

class Dodo
{
    private float walkSpeed;
    private float runSpeed;
    private float chaseDist;

    private Room room;
    private float timer = 0;

    private Random random = new Random();

    private Vector2 dodoPos;
    public Dodo()
    {
        walkSpeed = 5;
        runSpeed = 10;
        dodoPos = new Vector2(50, 50);
        chaseDist = 100;
        room = new Room();
    }
    public Dodo(float walkSpeed, float runSpeed, Vector2 dodoPos, float chaseDist, Room room)
    {
        this.walkSpeed = walkSpeed;
        this.runSpeed = runSpeed;
        this.dodoPos = dodoPos;
        this.chaseDist = chaseDist;
        this.room = room;
    }

    public void Update(Vector2 playerPos, float screenWidth)
    {
        float dist = (float)Math.Sqrt(Math.Pow(dodoPos.X - playerPos.X, 2) +
            Math.Pow(dodoPos.Y - playerPos.Y, 2));
        if(dist > chaseDist)
        {
            Idle();
        }
        else
        {
            Run();
        }
    }
    
    private void Idle()
    {
        Vector2 move = new Vector2();
        if(timer >= 2)
        {
            timer = 0;
        }
        if (timer == 0)
        {
            move = new Vector2(random.Next(), random.Next());
        }
        move = move.Normalized() * walkSpeed;
        dodoPos = Move(dodoPos, move);
    }

    private void Run()
    {
        Vector2 move = Player.;
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
}
