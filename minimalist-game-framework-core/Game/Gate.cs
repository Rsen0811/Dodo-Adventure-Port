using System;
using System.Collections.Generic;
using System.Text;

class Gate : Rect
 {
    public Vector2 room;
    private Texture map;
    public bool isOpen;
    

    public Gate(Vector2 r, Texture m, Rect c) : base(c.X,c.Y)
    {
        isOpen = false;
        room = r;
        map = m;
        
    }

    private bool Collides()
    {
        return false;
    }

    private void Draw()
    {

    }

 }
