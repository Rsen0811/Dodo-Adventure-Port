using System;
using System.Collections.Generic;
using System.Text;
class Switch : Rect
{
    private Texture map=Engine.LoadTexture("switch.png");
    private List<String> gates;
    private bool state = false;
    private Vector2 pos;
    public Switch(List<String> gates, Rect c) : base(c.X, c.Y)
    {
        this.gates = gates;
        this.pos = new Vector2(c.X.min,c.Y.min);
    }
    public void Draw()
    {
        int width = map.Width / 2;
        Engine.DrawTexture(map, pos, size: this.ToBounds().Size, source: new Bounds2(state ? width : 0, 0, width, map.Height));
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
