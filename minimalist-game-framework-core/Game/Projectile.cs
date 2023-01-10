using System;
using System.Collections.Generic;
using System.Text;

class Projectile
{
    private static readonly double DEFAULT_SPEED = 100;
    private static readonly Texture fire = Engine.LoadTexture("textures/fireballClear.png");
    private static readonly Vector2 size = new Vector2(32, 32);

    private Vector2 dir;
    private double speed;
    private Vector2 position;
    private int animationFrame;

    public Projectile(Vector2 pos, Vector2 dir, double speed)
    {
        position = pos;
        this.dir = dir;
        this.speed = speed;
        animationFrame = 0;

    }
    public Projectile(Vector2 pos, Vector2 dir)
    {
        new Projectile(pos, dir, Projectile.DEFAULT_SPEED);
    }
    public void Update()
    {
        position += dir * (float)(Engine.TimeDelta * speed);
    }
    public void Draw() 
    {
        Bounds2 firebounds = new Bounds2(((int)animationFrame) * size.X, 0, size.X, size.Y);
        Engine.DrawTexture(fire, position, source: firebounds, mirror: TextureMirror.None);
        animationFrame++;
    }
}

