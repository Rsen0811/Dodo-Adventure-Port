using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;

class Boss
{
    readonly Texture bossAlive = Engine.LoadTexture("textures/dodoAlive.png");
    readonly Texture bossDamaged = Engine.LoadTexture("textures/dodoDamaged.png");
    readonly Texture bossDead = Engine.LoadTexture("textures/dodoDead.png");
    Vector2 pos;
    static readonly Vector2 size = new Vector2(200, 200);
    readonly int maxHealth;
    readonly float speed;
    int health;
    BossRoom bossRoom;
    Player player;

    Vector2 move;
    Random random = new Random();
    bool mirror;

    int action;
    float actionTimer;
    float idleTimer;
    float damageTimer;

    readonly float chargeLength;
    Vector2 chargeDir;
    readonly float chargeSpeed; // based on speed

    double sprayAngle; // used only for FireSpray

    private float stunSpeed;
    private readonly float stunLength;

    private float projectileTimer;
    private readonly float projectileDelay;
    private readonly int projectileAmount;
    private readonly double projectileSpeed;

    List<Projectile> projectiles = new List<Projectile>();
    

    public Boss(Vector2 pos, BossRoom bossRoom, int health = 10, float speed = 15, float chargeLength = 6f, 
        float stunLength = 3f, float projectileDelay = 1f, int projectileAmount = 16, 
        double projectileSpeed = 100)
    {
        this.pos = pos;
        this.bossRoom = bossRoom;
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
        this.player = player;
        // actions:
        // -1: die
        // 0: stun (after charge)
        // 1: charge
        // 2: shoot projectiles in a circle around boss
        // 3: spray projectiles rapid fire in a circle around boss
        if (health > 0)
        {
            if (player.GetItem() != null && player.GetItem().GetType() == typeof(Sword))
            {
                // sword collision 
                SwordSweep((Sword)player.GetItem());
            }
            if (action != -1 && Rect.CheckRectIntersect(player.getPlayerBounds(), GetBounds()))
            {
                // if intercecting with player, set action to -1: die for 5 seconds
                action = -1;
                actionTimer = 5f;
            }
            if (actionTimer <= 0)
            {
                // when an action is complete, generate a random action to replace the previous
                action = random.Next(1, 4);
                switch (action)
                {
                    case 1:
                        // starts charge sequence
                        actionTimer = chargeLength;
                        break;
                    case 2:
                        // starts radial projectile sequence
                        actionTimer = 4f;
                        projectileTimer = 0;
                        break;
                    case 3:
                        // starts projectile spray sequence
                        actionTimer = 3f;
                        projectileTimer = 0;
                        sprayAngle = 0;
                        break;
                }
            }
            switch (action)
            {
                // runs action based on prescribed action state
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
        else
        {

        }
        if(damageTimer > 0)
        {
            damageTimer -= Engine.TimeDelta;
        }
        // projectiles --------------------------
        for(int i = projectiles.Count - 1; i >= 0; i--)
        {
            if (!projectiles[i].InBounds())
            {
                // deletes projectiles if off screen
                projectiles.RemoveAt(i);
            }
            else
            {
                // updates projectiles and checks for player intersect
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
            // draws projectiles
            projectiles[i].Draw();
        }
    }
    public bool IsAlive()
    {
        return health > 0;
    }
    public void Walk(Vector2 playerPos)
    {
        // see Dodo walk method
        if (health > 0)
        {
            if (idleTimer <= 0)
            {
                // generates random direction
                idleTimer = 3;
                move = new Vector2(random.Next() * 2 - 1, random.Next() * 2 - 1);
                move = move.Normalized() * speed;
            }
            if (!Move(pos + move))
            {
                // generates new random direction if previous is invalid
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
    private void Charge(Vector2 playerPos)
    {
        if (actionTimer <= chargeLength - stunLength / 2)
        {
            Vector2 move = chargeDir * chargeSpeed;
            if (Move(pos + move * Engine.TimeDelta)) pos += move * Engine.TimeDelta;
            else
            {
                // begin stun sequence if boss crashes into wall
                action = 0;
                actionTimer = stunLength;
                stunSpeed = chargeSpeed / 2.5f;
                for(int i = 0; i < random.Next(0, 3); i++)
                {
                    // adds 0-2 dodos on opposite side of screen after boss is stunned
                    switch(StartScreen.GetDifficulty())
                    {
                        case 0:
                            bossRoom.AddEnemy(new Dodo(new Vector2((pos.X < Game.Resolution.X / 2) ? 
                                Game.Resolution.X + 100 : -100, random.Next(15, (int)Game.Resolution.Y)), 
                                walkSpeed: 100, runSpeed: 100, chaseDist: 10000, maxHealth: 1, 
                                chargePauseLength: 0.9f, stunLength: 1.3f));
                            break;
                        case 1:
                            bossRoom.AddEnemy(new Dodo(new Vector2((pos.X < Game.Resolution.X / 2) ?
                                Game.Resolution.X + 100 : -100, random.Next(15, (int)Game.Resolution.Y)),
                                walkSpeed: 110, runSpeed: 140, chaseDist: 10000,
                                chargePauseLength: 0.8f, stunLength: 1.1f));
                            break;
                        case 2:
                            bossRoom.AddEnemy(new Dodo(new Vector2((pos.X < Game.Resolution.X / 2) ?
                                Game.Resolution.X + 100 : -100, random.Next(15, (int)Game.Resolution.Y)), 
                                chaseDist: 10000, runSpeed: 160));
                            break;
                    }
                }
            }
        }
        // pause for fractions of a second, then generate a target lock on player
        else chargeDir = new Vector2(playerPos.X + 12 - pos.X - size.X / 2 + random.Next(-50, 50),
            playerPos.Y + 12 - pos.Y - size.Y / 4 + random.Next(-50, 50)).Normalized();
    }

    private void Stun()
    {
        // simple decceleration
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
                // adds a circle of projectileAmount projectiles each heading in direction angle
                // to projectiles list. projectiles begin at beak
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
            // over a short time dependent on projectileAmount and projectileDelay, fire a
            // machine gun style spiral of projectiles heading radially outward in direction
            // sprayAngle (add to projectiles list). projectiles begin at beak
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
        // initiates eat sequence, teleporting player to boss beak, with no chance to escape
        Vector2 beakPos = new Vector2(pos.X + (mirror ? 15 : size.X - 15), pos.Y + size.Y / 4 - 10);
        Vector2 deathPos = new Vector2(pos.X + size.X / 4 + (mirror ? 5 : 0), pos.Y + size.Y / 2);
        player.GetBossEaten(beakPos, deathPos);
    }
    public void Damage()
    {
        // boss does not experience knockback, only player does
       if(damageTimer <= 0)
        {
            damageTimer = 0.5f;
            health--;
            player.BossRebound(pos);
            Game.PlaySwordHit();
        }
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
    public Rect GetDamageBounds()
    {
        Vector2 bodyTLC = new Vector2((mirror ? 10 : 2) * 3f, 2 * 2f);
        Vector2 size = new Vector2(46 * 3f, 74 * 2f);
        return Rect.GetSpriteBounds(bodyTLC + pos, size);
    }
    public void SwordSweep(Sword s)
    {
        if (Rect.CheckRectIntersect(s.CollisionZone(), GetBounds()))
        {
            Damage();
        }
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
