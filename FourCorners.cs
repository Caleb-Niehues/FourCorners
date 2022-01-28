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

        private Texture2D atlas;

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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ball.LoadContent(Content);
            atlas = Content.Load<Texture2D>("64-64-sprite-pack");
            spriteFont = Content.Load<SpriteFont>("File");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            ball.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            ball.Draw(gameTime, spriteBatch);
            spriteBatch.Draw(atlas, new Vector2(350, 50), new Rectangle(0, 32, 32, 32), Color.White);
            spriteBatch.Draw(atlas, new Vector2(400, 200), new Rectangle(32, 32, 16, 16), Color.White);
            spriteBatch.Draw(atlas, new Vector2(350, 350), new Rectangle(48, 32, 16, 16), Color.White);
            spriteBatch.DrawString(spriteFont, "Press ESC to end game", new Vector2(2, 2), Color.Gold);
            spriteBatch.DrawString(spriteFont, "Left click to go fast and forward", new Vector2(2, 22), Color.Gold);
            spriteBatch.DrawString(spriteFont, "Right click to go slow and backward", new Vector2(2, 42), Color.Gold);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
