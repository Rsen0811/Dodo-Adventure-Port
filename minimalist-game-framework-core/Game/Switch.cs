using System;
using System.Collections.Generic;
using System.Text;
class Switch : Rect
{
    private Texture map;
    private List<Gate> gates;
    public Switch(List<Gate> gates, Rect c) : base(c.X, c.Y)
    {
        this.gates = gates;
    }
    public void Toggle()
    {
        foreach(Gate g in gates)
        {
            g.Toggle();
        }
    }
}
