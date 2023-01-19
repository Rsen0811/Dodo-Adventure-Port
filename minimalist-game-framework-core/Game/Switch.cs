using System;
using System.Collections.Generic;
using System.Text;
class Switch : Rect
{
    private Texture leftPos=Engine.LoadTexture("switch.png");
    private Texture rightPos = Engine.LoadTexture("switch.png");
    private List<String> gates;
    private bool state = false;
    private Vector2 pos;
    public Switch(List<String> gates, Rect c, String color) : base(c.X, c.Y)
    {
        this.gates = gates;
        this.pos = new Vector2(c.X.min,c.Y.min);
        this.leftPos= Engine.LoadTexture("textures/switches/"+color+"Left.png");
        this.rightPos = Engine.LoadTexture("textures/switches/" + color + "Right.png");

    }
    public void Draw()
    {
        Texture map;
        Vector2 tempPos = pos;
        if (state == true)
        {
            map = leftPos;
            tempPos.X--;
        }
        else
        {
            map = rightPos;
        }
        int width = map.Width;
        Engine.DrawTexture(map, tempPos, size: this.ToBounds().Size, source: new Bounds2(0, 0, width, map.Height));
    }
    public void Toggle()
    {
        foreach(String g in gates)
        {
            Game.toggleGate(g);
        }
        state = !state;
    }
}
