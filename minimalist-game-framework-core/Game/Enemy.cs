using System;
using System.Collections.Generic;
using System.Text;


interface Enemy
{
    void Update(Player player, float screenWidth);
    void DrawDodo();
    Rect GetBounds();
    void Damage();
    bool IsAlive();
    void Idle();
}

