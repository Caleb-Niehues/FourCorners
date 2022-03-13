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
    /// <summary>
    /// A class representing a ball
    /// </summary>
    public class BallSprite
    {
        private MouseState mouseState;
        private MouseState previousMouseState;

        private Vector2 position = new Vector2(50, 224);

        /// <summary>
        /// 
        /// </summary>
        public Vector2 Position => position;

        private Vector2 direction = new Vector2(1,-1);

        /// <summary>
        /// 
        /// </summary>
        public Vector2 Direction
        {
            get => direction;
            set => direction = value;
        }

        private int speed = 0;

        /// <summary>
        /// 
        /// </summary>
        public int Speed
        {
            get => speed;
            set => speed = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public double Distance => Math.Sqrt(Math.Pow(speed * direction.X, 2) + Math.Pow(speed * direction.Y, 2));

        private BoundingCircle bounds = new BoundingCircle(new Vector2(50 - 16, 200 - 16), 16);

        /// <summary>
        /// 
        /// </summary>
        public BoundingCircle Bounds => bounds;

        private short animationFrame;

        private Texture2D texture;

        /// <summary>
        /// 
        /// </summary>
        public Color Color { get; set; } = Color.White;

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
            previousMouseState = mouseState;
            mouseState = Mouse.GetState();

            //maybe flip "fast" direction to push you towards the dead zone?
            if (previousMouseState != mouseState && mouseState.LeftButton == ButtonState.Pressed)
            {
                direction.X = 1;
                direction.Y *= -1;
                speed = 50;
            }
            else if (previousMouseState != mouseState && mouseState.RightButton == ButtonState.Pressed)
            {
                direction.X = -1;
                direction.Y *= -1;
                speed = 150;
            }

            position += (float)gameTime.ElapsedGameTime.TotalSeconds * new Vector2(Direction.X * speed, Direction.Y * speed);
            bounds.Center.X = position.X - 16;
            bounds.Center.Y = position.Y - 16;
        }

        public void Bounce()
        {
            position.Y = Math.Clamp(position.Y, 0, 448);
            direction.Y *= -1;
        }

        /// <summary>
        /// Draws the animated ball
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The SpriteBatch to draw with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Update animation frame
            if (Direction.Y < 0) animationFrame = 0;
            else animationFrame = 1;

            //Draw the sprite
            var source = new Rectangle(animationFrame * 32, 0, 32, 32);
            spriteBatch.Draw(texture, position, source, Color);
        }
    }
}
