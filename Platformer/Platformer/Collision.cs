using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class Collision
    {
        public Game1 game;

        public bool IsColliding(Sprite hero, Sprite otherSprite)
        {
            // Compares the position of each rectangles edges aginst the other
            // it compares the opposite edges of each rectangle, ie, the left edge of one with the right of the other
            if (hero.rightEdge < otherSprite.leftEdge || hero.leftEdge > otherSprite.rightEdge || hero.bottomEdge < otherSprite.topEdge || hero.topEdge > otherSprite.bottomEdge)
            {
                // these two rectangles are not colliding
                return false;
            }
            // Otherwise, the two AABB rectangles overlap, therefore there is a collision
            return true;
        }
        public Sprite CollideWithPlatforms
    }
}
