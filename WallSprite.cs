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

        private Vector2 position = new Vector2(50, 200);

        private int xDirection;
        private int yDirection;

        private int speed = 50; //doubles as a range
        private int travelled;

        private BoundingRectangle bounds;

        /// <summary>
        /// 
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        public WallSprite(Vector2 position, int xDirection, int yDirection)
        {
            this.position = position;
            this.bounds = new BoundingRectangle(position - new Vector2(32,32), 32, 32);
            this.xDirection = xDirection;
            this.yDirection = yDirection;
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

            position += (float)gameTime.ElapsedGameTime.TotalSeconds * new Vector2(xDirection * speed, yDirection * speed);
            bounds.X = position.X;
            bounds.Y = position.Y;
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
