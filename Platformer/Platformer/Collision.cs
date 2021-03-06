﻿using Microsoft.Xna.Framework;
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

        bool CheckForTile(Vector2 coordinates) // Checks if there is a tile at the specified coordinates
        {
            int column = (int)coordinates.X;
            int row = (int)coordinates.Y;

            if (column < 0 || column > game.levelTileWidth - 1)
            {
                return false;
            }
            if (row < 0 || row > game.levelTileHeight - 1)
            {
                return true;
            }

            Sprite tileFound = game.levelGrid[column, row];

            if (tileFound != null)
            {
                return true;
            }

            return false;
        }

        Sprite CollideLeft(Sprite hero, Vector2 tileIndex, Sprite playerPrediction)
        {
            Sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];
            if (IsColliding(playerPrediction, tile) == true && hero.velocity.X < 0)
            {
                hero.position.X = tile.rightEdge + hero.offset.X;
                hero.velocity.X = 0;
            }

            return hero;
        }

        Sprite CollideRight(Sprite hero, Vector2 tileIndex, Sprite playerPrediction)
        {
            Sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];

            if (IsColliding(playerPrediction, tile) == true && hero.velocity.X > 0)
            {
                hero.position.X = tile.leftEdge - hero.width + hero.offset.X;
                hero.velocity.X = 0;
            }

            return hero;
        }

        Sprite CollideAbove(Sprite hero, Vector2 tileIndex, Sprite playerPrediction)
        {
            Sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];

            if (IsColliding(playerPrediction, tile) == true && hero.velocity.Y < 0)
            {
                hero.position.Y = tile.bottomEdge + hero.offset.Y;
                hero.velocity.Y = 0;
            }

            return hero;
        }

        Sprite CollideBelow(Sprite hero, Vector2 tileIndex, Sprite playerPrediction)
        {
            Sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];
            if (IsColliding(playerPrediction, tile) == true && hero.velocity.Y > 0)
            {
                hero.position.Y = tile.topEdge - hero.height + hero.offset.Y;
                hero.velocity.Y = 0;
                hero.canJump = true;
            }

            return hero;
        }

        Sprite CollideBottomDiagonals(Sprite hero, Vector2 tileIndex, Sprite playerPrediction)
        {
            Sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];
            int leftEdgeDistance = Math.Abs(tile.leftEdge - playerPrediction.rightEdge);
            int rightEdgeDistance = Math.Abs(tile.rightEdge - playerPrediction.leftEdge);
            int topEdgeDistance = Math.Abs(tile.topEdge - playerPrediction.bottomEdge);

            if (IsColliding(playerPrediction, tile) == true)
            {
                if (topEdgeDistance < rightEdgeDistance && topEdgeDistance < leftEdgeDistance)
                {
                    // if the top edge is closest, collision is happening to the right of the platform
                    hero.position.Y = tile.topEdge - hero.height + hero.offset.Y;
                    hero.canJump = true;
                    hero.velocity.Y = 0;
                }
                else if (rightEdgeDistance < leftEdgeDistance)
                {
                    // if the right edge is closest, the collision is happening to the right of the platform
                    hero.position.X = tile.rightEdge + hero.offset.X;
                    //hero.velocity.X = 0;
                }
                else
                {
                    // else if the left edge is cloest, the collision is happening to the left of the platform
                    hero.position.X = tile.leftEdge - hero.width + hero.offset.X;
                    //hero.velocity.X = 0;
                }
            }
            return hero;
        }

        Sprite CollideAboveDiagonals(Sprite hero, Vector2 tileIndex, Sprite playerPrediction)
        {
            Sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];
            int leftEdgeDistance = Math.Abs(tile.rightEdge - playerPrediction.leftEdge);
            int rightEdgeDistance = Math.Abs(tile.leftEdge - playerPrediction.rightEdge);
            int bottomEdgeDistance = Math.Abs(tile.bottomEdge - playerPrediction.topEdge);
            if (IsColliding(playerPrediction, tile) == true)
            {
                if (bottomEdgeDistance < leftEdgeDistance && bottomEdgeDistance < rightEdgeDistance)
                {
                    hero.position.Y = tile.bottomEdge + hero.offset.Y;
                    //hero.velocity.Y = 0;
                }
                else if (leftEdgeDistance < rightEdgeDistance)
                {
                    hero.position.X = tile.rightEdge + hero.offset.X;
                    //hero.velocity.X = 0;
                }
                else
                {
                    hero.position.X = tile.leftEdge - hero.width + hero.offset.X;
                    //hero.velocity.X = 0;
                }
            }
            return hero;
        }







        public Sprite CollideWithPlatforms(Sprite hero, float deltaTime)
        {
            //Create a copy of the hero that will move to where the hero will be in the naxt frame
            Sprite playerPrediction = new Sprite();
            playerPrediction.position = hero.position;
            playerPrediction.width = hero.width;
            playerPrediction.height = hero.height;
            playerPrediction.offset = hero.offset;
            playerPrediction.UpdateHitBox();

            playerPrediction.position += hero.velocity * deltaTime;

            int playerColumn = (int)Math.Round(playerPrediction.position.X / game.tileHeight);
            int playerRow = (int)Math.Round(playerPrediction.position.Y / game.tileHeight);

            Vector2 playerTile = new Vector2(playerColumn, playerRow);

            Vector2 leftTile = new Vector2(playerTile.X - 1, playerTile.Y);
            Vector2 rightTile = new Vector2(playerTile.X + 1, playerTile.Y);
            Vector2 topTile = new Vector2(playerTile.X, playerTile.Y - 1);
            Vector2 bottomTile = new Vector2(playerTile.X, playerTile.Y + 1);

            Vector2 bottomLeftTile = new Vector2(playerTile.X - 1, playerTile.Y + 1);
            Vector2 bottomRightTile = new Vector2(playerTile.X + 1, playerTile.Y + 1);
            Vector2 topLeftTile = new Vector2(playerTile.X - 1, playerTile.Y - 1);
            Vector2 topRightTile = new Vector2(playerTile.X + 1, playerTile.Y - 1);
            // ....This allows us to predict if the hero will be overlapping an obstacle

            bool leftCheck = CheckForTile(leftTile);
            bool rightCheck = CheckForTile(rightTile);
            bool topCheck = CheckForTile(topTile);
            bool bottomCheck = CheckForTile(bottomTile);

            bool bottomLeftCheck = CheckForTile(bottomLeftTile);
            bool bottomRightCheck = CheckForTile(bottomRightTile);
            bool topLeftCheck = CheckForTile(topLeftTile);
            bool topRightCheck = CheckForTile(topRightTile);

            if(leftCheck == true) // check for collisions with the tiles left of the player
            {
                hero = CollideLeft(hero, leftTile, playerPrediction);
            }
            
            if (rightCheck == true) // check for collisions with the tiles right of the player
            {
                hero = CollideRight(hero, rightTile, playerPrediction);
            }
            if (bottomCheck == true) // check for collisions with the tiles below the player
            {
                hero = CollideBelow(hero, bottomTile, playerPrediction);
            }
            if (topCheck == true) // check for collisions with the tiles above the player
            {
                hero = CollideAbove(hero, topTile, playerPrediction);
            }

            //check for collisions with the tiles below and to the left of the player
            if (leftCheck == false && bottomCheck == false && bottomLeftCheck == true)
            {
                //... then properly check for the diagonals.
                hero = CollideBottomDiagonals(hero, bottomLeftTile, playerPrediction);
            }

            // Check for collisions with the tiles below and to the right of the player
            if (rightCheck == false && bottomCheck == false & bottomRightCheck == true)
            {
                //... then properly fheck for diagonals
                hero = CollideBottomDiagonals(hero, bottomRightTile, playerPrediction);
            }

            //Check for collisions with the tiles above and to the left of the player
            if (leftCheck == false && topCheck == false && topLeftCheck == true)
            {
                hero = CollideAboveDiagonals(hero, topLeftTile, playerPrediction);
            }

            if (rightCheck == false && topCheck == false && topRightCheck == true)
            {
                hero = CollideAboveDiagonals(hero, topRightTile, playerPrediction);
            }

            return hero;
        }

        public Sprite CollideWithMonsta(Player hero, Enemy monsta, float deltaTime, Game1 theGame)
        {
            Sprite playerPrediction = new Sprite();
            playerPrediction.position = hero.playerSprite.position;
            playerPrediction.width = hero.playerSprite.width;
            playerPrediction.height = hero.playerSprite.height;
            playerPrediction.offset = hero.playerSprite.offset;
            playerPrediction.UpdateHitBox();

            playerPrediction.position += hero.playerSprite.velocity * deltaTime;

            //if dere is a cowwision
            if (IsColliding(hero.playerSprite, monsta.enemySprite))
            {
                int leftEdgeDistance = Math.Abs(monsta.enemySprite.leftEdge - playerPrediction.rightEdge);
                int rightEdgeDistance = Math.Abs(monsta.enemySprite.rightEdge - playerPrediction.leftEdge);
                int topEdgeDistance = Math.Abs(monsta.enemySprite.topEdge - playerPrediction.bottomEdge);
                int bottomEdgeDistance = Math.Abs(monsta.enemySprite.bottomEdge - playerPrediction.topEdge);

                // ...Check wich edge of da monsta is closest.............
                if (topEdgeDistance < leftEdgeDistance && topEdgeDistance < rightEdgeDistance && topEdgeDistance < bottomEdgeDistance)
                {
                    theGame.enemies.Remove(monsta);
                    hero.playerSprite.velocity.Y -= hero.jumpStrength * deltaTime;
                    hero.playerSprite.canJump = false;
                }
                else
                {
                    //player ded
                    theGame.Exit();
                }
            }
            return hero.playerSprite;
        }

    }
}
