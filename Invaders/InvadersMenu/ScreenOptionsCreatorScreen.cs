using System;
using System.Collections.Generic;
using Infrastructure.Menu;
using Infrastructure.ObjectModel.Screens;
using Invaders.Infrastructure;
using Infrastructure.ServiceInterfaces;
using Invaders.Screens;

namespace Invaders.InvadersMenu
{
    public class ScreenOptionsCreatorScreen : MenuItemsCreator
    {
        public ScreenOptionsCreatorScreen(BaseGame i_Game, GameScreen i_GameScreen) : base(i_Game, i_GameScreen)
        {
        }

        public override List<MenuItem> MakeMenu()
        {
            IScreensManager m_ScreensManager = m_Game.Services.GetService(typeof(IScreensManager)) as IScreensManager;

            ToggleMenuItem<string, bool> mouseVisibilityToggleMenuItem = new ToggleMenuItem<string, bool>(m_Game, m_GameScreen, new List<string> { "Visible", "Invisible" }, new List<bool> { true, false }, "Mouse Visability: ");
            mouseVisibilityToggleMenuItem.CorrespondingToggledItem = m_GameSettingsManager.MouseVisibility;
            mouseVisibilityToggleMenuItem.Toggle += mouseVisibilityChanged;
            m_MenuItems.Add(mouseVisibilityToggleMenuItem);

            ToggleMenuItem<string, bool> allowResizingToggleMenuItem = new ToggleMenuItem<string, bool>(m_Game, m_GameScreen, new List<string> { "Off", "On" }, new List<bool> { false, true }, "Allow Window Resizing: ");
            allowResizingToggleMenuItem.CorrespondingToggledItem = m_GameSettingsManager.AllowWindowResize;
            allowResizingToggleMenuItem.Toggle += allowingWindowResizingChanged;
            m_MenuItems.Add(allowResizingToggleMenuItem);

            ToggleMenuItem<string, bool> fullScreenToggleMenuItem = new ToggleMenuItem<string, bool>(m_Game, m_GameScreen, new List<string> { "Off", "On" }, new List<bool> { false, true }, "Full Screen Mode: ");
            fullScreenToggleMenuItem.CorrespondingToggledItem = m_GameSettingsManager.FullScreen;
            fullScreenToggleMenuItem.Toggle += fullScreenToggled;
            m_MenuItems.Add(fullScreenToggleMenuItem);

            EnterMenuItem done = new EnterMenuItem(m_Game, "Done", m_GameScreen);
            done.EnterPressed += Done_EnterPressed;
            m_MenuItems.Add(done);
            return m_MenuItems;
        }

        private void Done_EnterPressed(object sender, EventArgs e)
        {
            m_ScreensManager.SetCurrentScreen(new MainMenuScreen(m_Game));
            m_ScreensManager.Remove(m_GameScreen);
        }

        private void fullScreenToggled(object sender, EventArgs e)
        {
            ToggleEventArgs<bool> scrrenToggledArgs = e as ToggleEventArgs<bool>;
            m_GameSettingsManager.FullScreen = scrrenToggledArgs.ItemValue;
        }

        private void allowingWindowResizingChanged(object sender, EventArgs e)
        {
            ToggleEventArgs<bool> windowResizingArgs = e as ToggleEventArgs<bool>;
            m_GameSettingsManager.AllowWindowResize = windowResizingArgs.ItemValue;
        }

        private void mouseVisibilityChanged(object sender, EventArgs e)
        {
            ToggleEventArgs<bool> mouseVisibleArgs = e as ToggleEventArgs<bool>;
            m_GameSettingsManager.MouseVisibility = mouseVisibleArgs.ItemValue;
        }
    }
}
