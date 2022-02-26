using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using FourCorners.Collisions;

namespace FourCorners
{
    public class WallSprite
    {
        private Texture2D texture;

        private Vector2 position;

        private int xDirection;
        private int yDirection;

        private int speed = 0; //doubles as a range
        private double travelled = 0;

        private BoundingRectangle bounds;

        /// <summary>
        /// 
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="xDirection"></param>
        /// <param name="yDirection"></param>
        public WallSprite(Vector2 position, int xDirection, int yDirection, int speed)
        {
            this.position = position;
            this.bounds = new BoundingRectangle(position - new Vector2(32,32), 32, 32);
            this.xDirection = xDirection;
            this.yDirection = yDirection;
            this.speed = speed;
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("64-64-sprite-pack");
        }

        /// <summary>
        /// Updates the sprite's position based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public void Update(GameTime gameTime)
        {
            travelled += gameTime.ElapsedGameTime.TotalSeconds * Math.Sqrt(Math.Pow(xDirection * speed, 2) + Math.Pow(yDirection * speed, 2));
            if (Math.Abs(travelled) > speed * 30)
            {
                xDirection *= -1;
                yDirection *= -1;
                travelled = -speed;
            }
            position += (float)gameTime.ElapsedGameTime.TotalSeconds * new Vector2(xDirection * speed, yDirection * speed);
            bounds.X = position.X - 32;
            bounds.Y = position.Y - 32;
        }

        /// <summary>
        /// Draws the animated ball
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The SpriteBatch to draw with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Draw the sprite
            var source = new Rectangle(0, 32, 32, 32);
            spriteBatch.Draw(texture, position, source, Color.White);
        }
    }
}
