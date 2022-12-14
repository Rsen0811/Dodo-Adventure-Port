using System;
using System.Collections.Generic;
using System.Text;

class Gate : Rect
 {
    private Vector2 room;
    private Texture map;
    public bool isOpen;
    private Vector2 pos;

    public Gate(Vector2 r,Vector2 pos, String texturePath, Rect c) : base(c.X,c.Y)
    {
        isOpen = false;
        room = r;
        map = Engine.LoadTexture(texturePath);
        this.pos = pos;
        
    }
    public void Draw()
    {
        int width=map.Width / 2;
        Engine.DrawTexture(map,pos,size: this.ToBounds().Size, source: new Bounds2(isOpen ? width:0, 0,width,map.Height));
    }
    public Vector2 getRoom()
    {
        return room;
    }

 }
