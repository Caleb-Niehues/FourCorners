using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FourCorners.Screens
{
    // The options screen is brought up over the top of the main menu
    // screen, and gives the user a chance to configure the game
    // in various hopefully useful ways.
    public class OptionsMenuScreen : MenuScreen
    {
        /// <summary>
        /// 
        /// </summary>
        public OptionsMenuScreen() : base("Options")
        {
            var back = new MenuEntry("Back");
            back.Selected += OnCancel;
            MenuEntries.Add(back);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GameTime gameTime)
        {
            
        }
    }
}
