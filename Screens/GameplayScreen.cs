using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FourCorners.StateManagement;
using Microsoft.Xna.Framework.Audio;

namespace FourCorners.Screens
{
    public enum GameState
    {
        InProgress,
        Won,
        Lost
    }

    // This screen implements the actual game logic. It is just a
    // placeholder to get the idea across: you'll probably want to
    // put some more interesting gameplay in here!
    public class GameplayScreen : GameScreen
    {
        private ContentManager _content;
        private SpriteFont _gameFont;

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;
        private FireworkParticleSystem _fireworks;

        private BallSprite ball;
        private ShardSprite shard;
        private WallSprite[] walls;

        //private SoundEffect drain;
        private double scoreBucket;
        private int score;
        private GameState gs = GameState.InProgress;

        private bool currentContact; //used to reset ball.Contact on iterative check
        private bool oldContact; //used to detect frame of contact start/stop - true means contact occurred in last frame

        private float _shakeTime;
        private Matrix shakeTransform = Matrix.Identity;

        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);
        }

        // Load graphics content for the game
        public override void Activate()
        {
            if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");
            _fireworks = new FireworkParticleSystem(this.ScreenManager.Game, 20);
            this.ScreenManager.Game.Components.Add(_fireworks);

            //drain = _content.Load<SoundEffect>("Drain");
            _gameFont = _content.Load<SpriteFont>("File");

            ball = new BallSprite();
            shard = new ShardSprite(new Vector2(200, 240), new Vector2(0,0), 0);

            int wallCount = 15;
            int wallSpeed = 25;
            walls = new WallSprite[2 * wallCount];
            for (int i = 0; i < wallCount; i++)
            {
                walls[i] = new WallSprite(new Vector2(760, i * 32), -(i + 1), 0, wallSpeed - i);
                walls[i+wallCount] = new WallSprite(new Vector2(760, 448 - (i * 32)), -(i + 1), 0, wallSpeed - i);
            }

            ball.LoadContent(_content);
            shard.LoadContent(_content);
            foreach (var wall in walls) wall.LoadContent(_content);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            _content.Unload();
        }

        // This method checks the GameScreen.IsActive property, so the game will
        // stop updating when the pause menu is active, or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);
        }

        // Unlike the Update method, this will only be called when the gameplay screen is active.
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];
            var gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                if (ball.Position.X < 0) gs = GameState.Lost;
                //score = -1;
                if (760 < ball.Position.X + 32) gs = GameState.Won;
                //score = 61;
                if (ball.Position.Y < 0 || 480 < ball.Position.Y + 32)
                {
                    ball.Bounce();
                    _shakeTime = 1500;
                }

                if (gs != GameState.InProgress)
                {
                    _shakeTime = 0;
                    return;
                }
                // Otherwise move the player position.
                ball.Update(gameTime);
                oldContact = currentContact;
                currentContact = false;

                if (shard.Active && shard.Bounds.CollidesWith(ball.Bounds))
                {
                    score += 25;
                    _fireworks.PlaceFirework(shard.Explode());
                    _shakeTime = 1500;
                }

                foreach (var wall in walls)
                {
                    if (wall.Bounds.CollidesWith(ball.Bounds))
                    {
                        currentContact = true;
                        _shakeTime += 10;
                    }
                    wall.Update(gameTime);
                }

                if (currentContact)
                {
                    scoreBucket -= ball.Distance;
                    ball.Color = Color.Red;
                    //if (!oldContact) drain.Play();
                }
                else
                {
                    scoreBucket += ball.Distance / 10;
                    ball.Color = Color.White;
                }

                if (scoreBucket > 250)
                {
                    score++;
                    scoreBucket = 0;
                }
                else if (scoreBucket < -250)
                {
                    score--;
                    scoreBucket = 0;
                }

                if (score < 0 || score > 50) gs = GameState.Lost;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            // Our player and enemy are both actually just text strings.
            var spriteBatch = ScreenManager.SpriteBatch;

            shakeTransform = Matrix.Identity;
            if (_shakeTime > 0)
            {
                shakeTransform = Matrix.CreateTranslation(MathF.Sin(_shakeTime), MathF.Cos(_shakeTime), 0);
                _shakeTime -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            spriteBatch.Begin(transformMatrix: shakeTransform);
            foreach (var wall in walls)
                wall.Draw(gameTime, spriteBatch);
            shard.Draw(gameTime, spriteBatch);
            ball.Draw(gameTime, spriteBatch);
            
            if(gs == GameState.Lost) spriteBatch.DrawString(_gameFont, "Game Over, restart program.", new Vector2(2, 2), Color.Gold);
            else if(gs == GameState.Won) spriteBatch.DrawString(_gameFont, "You won! Restart.", new Vector2(2, 2), Color.Gold);
            else if(score > 25) spriteBatch.DrawString(_gameFont, "Score: " + score + ", you will lose if you exceed 50 (shards give 25).", new Vector2(2, 2), Color.Gold);
            else spriteBatch.DrawString(_gameFont, "Score: " + score, new Vector2(2, 2), Color.Gold);

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}
