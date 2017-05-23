using System.Collections.Generic;
using Infrastructure.Menu;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.ServiceInterfaces;
using Invaders.Infrastructure;

namespace Invaders.InvadersMenu
{
    public abstract class MenuItemsCreator
    {
        protected List<MenuItem> m_MenuItems;

        protected BaseGame m_Game;

        protected GameScreen m_GameScreen;

        protected IScreensManager m_ScreensManager;

        protected IGameSettingsManager m_GameSettingsManager;

        public MenuItemsCreator(BaseGame i_Game, GameScreen i_GameScreen)
        {
            m_Game = i_Game;
            m_GameScreen = i_GameScreen;
            m_MenuItems = new List<MenuItem>();
            m_ScreensManager = m_Game.Services.GetService(typeof(IScreensManager)) as IScreensManager;
            m_GameSettingsManager = m_Game.Services.GetService(typeof(IGameSettingsManager)) as IGameSettingsManager;
        }

        public abstract List<MenuItem> MakeMenu();
    }
}
