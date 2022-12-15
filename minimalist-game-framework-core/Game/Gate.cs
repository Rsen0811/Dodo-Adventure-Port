using System;
using System.Collections.Generic;
using System.Text;

class Gate : Rect
 {
    private Texture map;
    public bool isOpen;
    private Vector2 pos;
    private String name;

    public Gate(String gateName, Rect c) : base(c.X,c.Y)
    {
        name = gateName;
        isOpen = false;
        map = Engine.LoadTexture("textures/gates/"+gateName+".png");
        this.pos = new Vector2(c.X.min,c.Y.min);
        
    }
    public void Draw()
    {
        int width=map.Width / 2;
        Engine.DrawTexture(map,pos,size: this.ToBounds().Size, source: new Bounds2(isOpen ? width:0, 0,width,map.Height));
    }
    public String getName()
    {
        return name;
    }

 }
