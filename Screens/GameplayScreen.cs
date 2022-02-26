﻿using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FourCorners.StateManagement;
using Microsoft.Xna.Framework.Audio;

namespace FourCorners.Screens
{
    // This screen implements the actual game logic. It is just a
    // placeholder to get the idea across: you'll probably want to
    // put some more interesting gameplay in here!
    public class GameplayScreen : GameScreen
    {
        private ContentManager _content;
        private SpriteFont _gameFont;

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        private BallSprite ball;
        private WallSprite[] walls;

        private SoundEffect drain;
        private double scoreBucket;
        private int score;

        private bool currentContact; //used to reset ball.Contact on iterative check
        private bool oldContact; //used to detect frame of contact start/stop - true means contact occurred in last frame

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

            drain = _content.Load<SoundEffect>("Drain");
            _gameFont = _content.Load<SpriteFont>("File");

            ball = new BallSprite();
            int wallCount = 15;
            int wallSpeed = 25;
            walls = new WallSprite[2 * wallCount];
            for (int i = 0; i < wallCount; i++)
            {
                walls[i] = new WallSprite(new Vector2(760, i * 32), -(i + 1), 0, wallSpeed - i);
                walls[i+wallCount] = new WallSprite(new Vector2(760, 448 - (i * 32)), -(i + 1), 0, wallSpeed - i);
            }

            ball.LoadContent(_content);
            foreach (var wall in walls) wall.LoadContent(_content);

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            //Thread.Sleep(1000);

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
                if (ball.Position.X < 0)
                    score = -1;
                if (760 < ball.Position.X + 32)
                    score = 61;
                if (ball.Position.Y < 0 || 480 < ball.Position.Y + 32)
                    ball.Direction = new Vector2(ball.Direction.X, ball.Direction.Y * -1);

                if (score < 0 || score > 50) return;
                // Otherwise move the player position.
                ball.Update(gameTime);
                oldContact = currentContact;
                currentContact = false;

                foreach (var wall in walls)
                {
                    if (wall.Bounds.CollidesWith(ball.Bounds)) currentContact = true;
                    wall.Update(gameTime);
                }

                if (currentContact)
                {
                    scoreBucket -= ball.Distance;
                    ball.Color = Color.Red;
                    if (!oldContact) drain.Play();
                }
                else
                {
                    scoreBucket += ball.Distance/10;
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
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            // Our player and enemy are both actually just text strings.
            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            foreach (var wall in walls)
                wall.Draw(gameTime, spriteBatch);
            ball.Draw(gameTime, spriteBatch);
            
            if(score < 0 || score == 50) spriteBatch.DrawString(_gameFont, "Game Over, restart program.", new Vector2(2, 2), Color.Gold);
            else if(score > 60) spriteBatch.DrawString(_gameFont, "You won! Restart.", new Vector2(2, 2), Color.Gold);
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
