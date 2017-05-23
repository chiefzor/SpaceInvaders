using System;
using System.Collections.Generic;
using Infrastructure.Menu;
using Infrastructure.ObjectModel.Screens;
using Invaders.Infrastructure;
using Infrastructure.ServiceInterfaces;
using Invaders.Screens;

namespace Invaders.InvadersMenu
{
    public class SoundsOptionsCreatorScreen : MenuItemsCreator
    {
        private ISoundManager m_SoundsManager;
        private ToggleMenuItem<string, bool> m_SoundToggleMenuItem;

        public SoundsOptionsCreatorScreen(BaseGame i_Game, GameScreen i_GameScreen) : base(i_Game, i_GameScreen)
        {
            m_SoundsManager = m_Game.Services.GetService(typeof(ISoundManager)) as ISoundManager;
            m_SoundToggleMenuItem = new ToggleMenuItem<string, bool>(m_Game, m_GameScreen, new List<string> { "Off", "On" }, new List<bool> { false, true }, "Toggle Sound: ");
        }

        public override List<MenuItem> MakeMenu()
        {
            IScreensManager m_ScreensManager = m_Game.Services.GetService(typeof(IScreensManager)) as IScreensManager;
            List<float> volumeList = new List<float> { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
            volumeList.Reverse();
            List<float> realVolumeList = new List<float> { 0, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f };
            realVolumeList.Reverse();
            ToggleMenuItem<float, float> changeBackgroundMusicVolumeMenuItem = new ToggleMenuItem<float, float>(m_Game, m_GameScreen, volumeList, realVolumeList, "Background Music Volume: ");
            ToggleMenuItem<float, float> changeSoundsEffectsMusicVolumeMenuItem = new ToggleMenuItem<float, float>(m_Game, m_GameScreen, volumeList, realVolumeList, "Sounds Effects Volume: ");
            if (!m_SoundsManager.ToggleSound)
            {
                m_SoundsManager.ToggleSound = !m_SoundsManager.ToggleSound;
                changeBackgroundMusicVolumeMenuItem.CorrespondingToggledItem = m_GameSettingsManager.BackgroundVolume;
                changeSoundsEffectsMusicVolumeMenuItem.CorrespondingToggledItem = m_GameSettingsManager.SoundEffectsVolume;
                m_SoundsManager.ToggleSound = !m_SoundsManager.ToggleSound;
            }
            else
            {
                changeBackgroundMusicVolumeMenuItem.CorrespondingToggledItem = m_GameSettingsManager.BackgroundVolume;
                changeSoundsEffectsMusicVolumeMenuItem.CorrespondingToggledItem = m_GameSettingsManager.SoundEffectsVolume;
            }

            changeBackgroundMusicVolumeMenuItem.Toggle += backgroundVolumeChanged;
            m_MenuItems.Add(changeBackgroundMusicVolumeMenuItem);

            changeSoundsEffectsMusicVolumeMenuItem.Toggle += soundsEffectsVolumeChanged;
            m_MenuItems.Add(changeSoundsEffectsMusicVolumeMenuItem);

            m_SoundToggleMenuItem.CorrespondingToggledItem = m_GameSettingsManager.MusicToggle;
            m_SoundToggleMenuItem.Toggle += soundToggleChanged;
            m_MenuItems.Add(m_SoundToggleMenuItem);

            EnterMenuItem done = new EnterMenuItem(m_Game, "Done", m_GameScreen);
            done.EnterPressed += Done_EnterPressed;
            m_MenuItems.Add(done);
            return m_MenuItems;
        }

        private void Done_EnterPressed(object sender, EventArgs e)
        {
            m_ScreensManager.Remove(m_GameScreen);
            m_ScreensManager.SetCurrentScreen(new MainMenuScreen(m_Game));
        }

        private void backgroundVolumeChanged(object sender, EventArgs e)
        {
            ToggleEventArgs<float> backgroundVolumeArgs = e as ToggleEventArgs<float>;
            m_GameSettingsManager.BackgroundVolume = backgroundVolumeArgs.ItemValue;
        }

        private void soundsEffectsVolumeChanged(object sender, EventArgs e)
        {
            ToggleEventArgs<float> soundsEffectsVolumeArgs = e as ToggleEventArgs<float>;
            m_GameSettingsManager.SoundEffectsVolume = soundsEffectsVolumeArgs.ItemValue;
        }

        private void soundToggleChanged(object sender, EventArgs e)
        {
            ToggleEventArgs<bool> soundsToggleArgs = e as ToggleEventArgs<bool>;
            m_GameSettingsManager.MusicToggle = soundsToggleArgs.ItemValue;
            m_SoundsManager.ToggleSoundChanged += M_SoundsManager_ToggleSoundChanged;
        }

        private void M_SoundsManager_ToggleSoundChanged(object sender, EventArgs e)
        {
            m_SoundToggleMenuItem.CorrespondingToggledItem = m_SoundsManager.ToggleSound;
            m_SoundToggleMenuItem.UpdateBasedOnCorrespondingItem();
        }
    }
}
