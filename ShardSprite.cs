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
    public class ShardSprite
    {
        private Texture2D texture;

        private Vector2 position;
        private Rectangle source = new Rectangle(48, 32, 15, 15);

        private Vector2 direction;

        private int speed = 0;
        private bool active = true;

        /// <summary>
        /// 
        /// </summary>
        public bool Active => active;

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
        public ShardSprite(Vector2 position, Vector2 direction, int speed)
        {
            this.position = position;
            this.bounds = new BoundingRectangle(position - new Vector2(15, 15), 15, 15);
            this.direction = direction;
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
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void Explode()
        {
            active = false;
        }

        /// <summary>
        /// Draws the shard
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The SpriteBatch to draw with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(active) spriteBatch.Draw(texture, position, source, Color.White);
        }
    }
}
