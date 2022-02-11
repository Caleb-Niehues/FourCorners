using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FourCorners
{
    public class FourCorners : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;

        private BallSprite ball;
        private WallSprite[] walls;

        //private Texture2D atlas;

        public FourCorners()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            Window.Title = "Four Corners";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ball = new BallSprite();
            walls = new WallSprite[]
            {
                new WallSprite(new Vector2(250,200), 0, 1),
                new WallSprite(new Vector2(350,200), 0, -1),
                new WallSprite(new Vector2(450,200), 0, 1)
            };

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ball.LoadContent(Content);
            foreach (var wall in walls) wall.LoadContent(Content);
            spriteFont = Content.Load<SpriteFont>("File");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            ball.Update(gameTime);

            ball.Color = Color.White;
            foreach (var wall in walls)
            {
                if (wall.Bounds.CollidesWith(ball.Bounds))
                {
                    ball.Color = Color.Red;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            ball.Draw(gameTime, spriteBatch);
            foreach (var wall in walls)
            {
                wall.Draw(gameTime, spriteBatch);
            }
            if (ball.Bounds.Center != new Vector2(50, 200)) //really hacky check to see if title screen should be displayed, also helps check bounds/sprite alignment
            {
                spriteBatch.DrawString(spriteFont, "Press ESC to end game", new Vector2(2, 2), Color.Gold);
                spriteBatch.DrawString(spriteFont, "Left click to go fast and forward", new Vector2(2, 22), Color.Gold);
                spriteBatch.DrawString(spriteFont, "Right click to go slow and backward", new Vector2(2, 42), Color.Gold);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
