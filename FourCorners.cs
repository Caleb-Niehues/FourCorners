using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FourCorners.Screens;
using FourCorners.StateManagement;

namespace FourCorners
{
    public class FourCorners : Game
    {
        private GraphicsDeviceManager _graphics;
        private readonly ScreenManager _screenManager;
        
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

            var screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);

            _screenManager = new ScreenManager(this);
            Components.Add(_screenManager);

            AddInitialScreens();
        }

        private void AddInitialScreens()
        {
            _screenManager.AddScreen(new BackgroundScreen(), null);
            _screenManager.AddScreen(new MainMenuScreen(), null);
        }

        protected override void Initialize()
        {
            base.Initialize();
            // TODO: Add your initialization logic here
            /*ball = new BallSprite();
            walls = new WallSprite[]
            {
                new WallSprite(new Vector2(250,200), 1, 1),
                new WallSprite(new Vector2(350,200), 0, -1),
                new WallSprite(new Vector2(450,200), 0, 1)
            };

            base.Initialize();*/
        }

        protected override void LoadContent() { }
        /*{
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ball.LoadContent(Content);
            foreach (var wall in walls) wall.LoadContent(Content);
            spriteFont = Content.Load<SpriteFont>("File");

        }*/

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            /*if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
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
                wall.Update(gameTime);
            }

            base.Update(gameTime);*/
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
            /*
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            foreach (var wall in walls)
            {
                wall.Draw(gameTime, spriteBatch);
            }
            ball.Draw(gameTime, spriteBatch);
            if (!ball.Moved)
            {
                spriteBatch.DrawString(spriteFont, "Press ESC to end game", new Vector2(2, 2), Color.Gold);
                spriteBatch.DrawString(spriteFont, "Left click to go fast and forward", new Vector2(2, 22), Color.Gold);
                spriteBatch.DrawString(spriteFont, "Right click to go slow and backward", new Vector2(2, 42), Color.Gold);
            }
            else
            {
                spriteBatch.DrawString(spriteFont, "Time Survived: N/A", new Vector2(2, 2), Color.Gold);
            }
            spriteBatch.End();

            base.Draw(gameTime);*/
        }
    }
}
