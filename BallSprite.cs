using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace FourCorners
{
    /// <summary>
    /// A class representing a ball
    /// </summary>
    public class BallSprite
    {
        private MouseState mouseState;
        private MouseState previousMouseState;

        private Texture2D texture;

        private Vector2 position = new Vector2(50, 200);

        private int xDirection;
        private int yDirection;
        
        private int speed = 25;

        private bool up = true;

        private short animationFrame;

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

            if (previousMouseState != mouseState && mouseState.LeftButton == ButtonState.Pressed)
            {
                if (up)
                {
                    xDirection = 4;
                    yDirection = -4;
                }
                else
                {
                    xDirection = 4;
                    yDirection = 4;
                }
                up = !up;
            }
            else if (previousMouseState != mouseState && mouseState.RightButton == ButtonState.Pressed)
            {
                if (!up)
                {
                    xDirection = -1;
                    yDirection = -1;
                }
                else
                {
                    xDirection = -1;
                    yDirection = 1;
                }
                up = !up;
            }

            position += (float)gameTime.ElapsedGameTime.TotalSeconds * new Vector2(xDirection * speed, yDirection * speed);
        }

        /// <summary>
        /// Draws the animated ball
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The SpriteBatch to draw with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Update animation timer
            //animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //Update animation frame
            if (up) animationFrame = 0;
            else animationFrame = 1;

            //Draw the sprite
            var source = new Rectangle(animationFrame * 32, 0, 32, 32);
            spriteBatch.Draw(texture, position, source, Color.White);
        }
    }
}
