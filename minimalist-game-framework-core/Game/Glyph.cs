using System;
using System.Collections.Generic;
using System.Text;

class Glyph
{
    readonly Texture img;
    Vector2 position;
    public Glyph(Vector2 pos, String texture) {
        position = pos;
        img = Engine.LoadTexture("textures/glyphs/" + texture + ".png");
    }
    public void Update() {
        return; 
    }
    public void Draw() {
        Engine.DrawTexture(img, position);
    }
    
}

