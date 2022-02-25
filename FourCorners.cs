using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FourCorners.Screens;
using FourCorners.StateManagement;
using Microsoft.Xna.Framework.Media;


namespace FourCorners
{
    public class FourCorners : Game
    {
        private GraphicsDeviceManager _graphics;
        private readonly ScreenManager _screenManager;

        private Song backgroundMusic;

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
            backgroundMusic = Content.Load<Song>("DeeYan-Key-TheGame");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);
        }

        protected override void LoadContent() { }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}
