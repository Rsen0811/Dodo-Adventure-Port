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
        Bounds2 firebounds = new Bounds2(((int)animationFrame % 4) * size.X, 0, size.X, size.Y);
        Engine.DrawTexture(fire, position, source: firebounds, mirror: TextureMirror.None);
        animationFrame++;
    }
    public Rect GetBounds()
    {
        return Rect.GetSpriteBounds(position + size / 2, size);
    }
    public bool InBounds()
    {
        if (position.X >= Game.Resolution.X)
        {
            return false;
        }
        else if (position.X <= 0 - size.X)
        {
            return false;
        }
        if (position.Y >= Game.Resolution.Y)
        {
            return false;
        }
        else if (position.Y <= 0 - size.X)
        {
            return false;
        }
        return true;
    }
    public Vector2 Pos()
    {
        return position;
    }
    public static Vector2 Size()
    {
        return size;
    }
}

