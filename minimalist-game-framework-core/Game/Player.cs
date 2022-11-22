using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Game.Game
{
    internal class Player
    {
        Vector2 pos;
        bool active;
        Item holding;
        Vector2 currRoom;

        public Vector2 move()
        {
            return Vector2.Zero;
        }
        public Item pickup()
        {
            return Item;
        }
        public Item drop()
        {
            return Item;
        }

        public Vector2 drawPlayer()
        {
            return Vector2.Zero;
        }
    }
}
