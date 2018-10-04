using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    class Player
    {
        public Sprite playerSprite = new Sprite();

        Game1 game = null;
        float runSpeed = 15000;

        Collision collision = new Collision();

        public Player()
        {

        }

        public void Load(ContentManager content, Game1 theGame)
        {
            playerSprite.Load(content, "hero", true);
            playerSprite.offset = new Vector2(24, 24);
            game = theGame; // We are now able to access the information stored in the 'Game1" class
            playerSprite.velocity = Vector2.Zero;
            playerSprite.position = new Vector2(64 * 2, 6400 - 64 - playerSprite.height * 2);
        }


        private void UpdateInput(float deltaTime)
        {
            Vector2 localAcceleration = new Vector2(0, 0);
            if (Keyboard.GetState().IsKeyDown(Keys.A)== true || Keyboard.GetState().IsKeyDown(Keys.Left)== true)
            {
                localAcceleration.X = -runSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D)== true || Keyboard.GetState().IsKeyDown(Keys.Right)== true)
            {
                localAcceleration.X = runSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W)== true || Keyboard.GetState().IsKeyDown(Keys.Up)== true)
            {
                localAcceleration.Y = -runSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S)== true || Keyboard.GetState().IsKeyDown(Keys.Down)== true)
            {
                localAcceleration.Y = runSpeed;
            }

            //foreach (Sprite tile in game.allCollisionTiles)
            //{
            //    if (collision.IsColliding(playerSprite, tile)== true)
            //    {
            //        int testVariable = 0;
            //    }
            //}

            playerSprite.velocity = localAcceleration * deltaTime;
            playerSprite.position += playerSprite.velocity * deltaTime;

            collision.game = game;
            playerSprite = collision.CollideWithPlatforms(playerSprite, deltaTime);

        }






        public void Update(float deltaTime)
        {
            UpdateInput(deltaTime);
            playerSprite.Update(deltaTime);
            playerSprite.UpdateHitBox();

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            playerSprite.Draw(spriteBatch);
        }



    }
}
