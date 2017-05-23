using System;
using System.Collections.Generic;
using Infrastructure.Menu;
using Invaders.Infrastructure;
using Infrastructure.ObjectModel.Screens;
using Invaders.Screens;

namespace Invaders.InvadersMenu
{
    public class MainMenuCreator : MenuItemsCreator
    {
        public MainMenuCreator(BaseGame i_Game, GameScreen i_GameScreen) : base(i_Game, i_GameScreen)
        {
        }

        public override List<MenuItem> MakeMenu()
        {
            EnterMenuItem soundOptionsMenuItem = new EnterMenuItem(m_Game, "Sound Options", m_GameScreen);
            soundOptionsMenuItem.EnterPressed += SoundOptionsMenuItem_EnterPressed;
            m_MenuItems.Add(soundOptionsMenuItem);

            EnterMenuItem screenOptionsMenuItem = new EnterMenuItem(m_Game, "Screen Options", m_GameScreen);
            screenOptionsMenuItem.EnterPressed += ScreenOptionsMenuItem_EnterPressed;
            m_MenuItems.Add(screenOptionsMenuItem);

            ToggleMenuItem<string, int> playersNumMenuItem = new ToggleMenuItem<string, int>(m_Game, m_GameScreen, new List<string> { "One", "Two" }, new List<int> { 1, 2 }, "Players: ");
            playersNumMenuItem.CorrespondingToggledItem = m_GameSettingsManager.NumOfPlayers;
            playersNumMenuItem.Toggle += numberOfPlayersChanged;
            m_MenuItems.Add(playersNumMenuItem);

            EnterMenuItem play = new EnterMenuItem(m_Game, "Play", m_GameScreen);
            play.EnterPressed += Play_EnterPressed;
            m_MenuItems.Add(play);

            EnterMenuItem quit = new EnterMenuItem(m_Game, "Quit", m_GameScreen);
            quit.EnterPressed += Quit_EnterPressed;
            m_MenuItems.Add(quit);

            return m_MenuItems;
        }

        private void Quit_EnterPressed(object sender, EventArgs e)
        {
            m_Game.ExitGame();
        }

        private void Play_EnterPressed(object sender, EventArgs e)
        {
            m_ScreensManager.SetCurrentScreen(new LevelTransitionScreen(m_Game));
            m_ScreensManager.Remove(m_GameScreen);
        }

        private void ScreenOptionsMenuItem_EnterPressed(object sender, EventArgs e)
        {
            m_ScreensManager.SetCurrentScreen(new ScreenOptionsScreen(m_Game));
            m_ScreensManager.Remove(m_GameScreen);
        }

        private void SoundOptionsMenuItem_EnterPressed(object sender, EventArgs e)
        {
            m_ScreensManager.SetCurrentScreen(new SoundsOptionsScreen(m_Game));
            m_ScreensManager.Remove(m_GameScreen);
        }

        private void numberOfPlayersChanged(object i_Sender, EventArgs i_EventArgs)
        {
            ToggleEventArgs<int> toggleEventArgs = i_EventArgs as ToggleEventArgs<int>;
            m_GameSettingsManager.NumOfPlayers = toggleEventArgs.ItemValue;
        }
    }
}
