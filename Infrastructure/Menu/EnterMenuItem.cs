using System;
using Invaders.Infrastructure;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel.Screens;

namespace Infrastructure.Menu
{
    public class EnterMenuItem : MenuItem
    {
        public event EventHandler EnterPressed;

        public EnterMenuItem(BaseGame i_Game, string i_MenuItemName, GameScreen i_GameScreen) : base(i_Game, i_GameScreen, i_MenuItemName)
        {
        }

        public void Execute()
        {
            EnterPressed.Invoke(this, EventArgs.Empty);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
