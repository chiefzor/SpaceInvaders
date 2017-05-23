using System;
using System.Collections.Generic;
using Infrastructure.Menu;
using Infrastructure.ObjectModel.Screens;
using Invaders.Infrastructure;
using Invaders.InvadersMenu;
using Invaders.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Invaders.Screens
{
    public class MainMenuScreen : GameScreen
    {     
        private Background m_Background;
        private Menu m_Menu;

        public MainMenuScreen(BaseGame i_Game)
            : base(i_Game)
        {
            m_Background = new Background(i_Game, this, 1);
            MainMenuCreator mainMenu = new MainMenuCreator(i_Game, this);

            List<MenuItem> m_MenuItems = mainMenu.MakeMenu();
            m_Menu = new Menu(i_Game, m_MenuItems);
            this.Add(m_Menu);
            this.ActivationLength = TimeSpan.FromSeconds(1);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.KeyPressed(Keys.Escape))
            {
                ExitScreen();
            }
        }
    }
}