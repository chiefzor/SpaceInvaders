using System;
using System.Collections.Generic;
using Infrastructure.ServiceInterfaces;
using Invaders.Infrastructure;
using Microsoft.Xna.Framework.Audio;

namespace Infrastructure.Managers
{
    public class SoundManager : GameService, ISoundManager
    {
        private float m_BackgroundMusicVolume;
        private float m_SoundsEffectsVolume;
        private bool m_ToggleSound;

        private List<SoundEffectInstance> m_SoundEffects;
        private SoundEffectInstance m_BackgroundMusic;

        public event EventHandler<EventArgs> ToggleSoundChanged;

        public float BackgroundMusicVolume
        {
            get
            {
                return m_ToggleSound ? m_BackgroundMusicVolume : 0;
            }

            set
            {
                if (m_BackgroundMusicVolume != value)
                {
                    m_BackgroundMusicVolume = value;

                    ChangeBackgroundMusicVolume();
                }
            }
        }

        public float SoundEffectsVolume
        {
            get
            {
                return m_ToggleSound ? m_SoundsEffectsVolume : 0;
            }

            set
            {
                if (m_SoundsEffectsVolume != value)
                {
                    m_SoundsEffectsVolume = value;

                    ChangeSoundEffectsVolume();
                }
            }
        }

        public bool ToggleSound
        {
            get
            {
                return m_ToggleSound;
            }

            set
            {
                m_ToggleSound = value;

                foreach (SoundEffectInstance soundInstance in m_SoundEffects)
                {
                    soundInstance.Volume = SoundEffectsVolume;
                }

                m_BackgroundMusic.Volume = BackgroundMusicVolume;

                if (ToggleSoundChanged != null)
                {
                    ToggleSoundChanged.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public SoundManager(BaseGame i_Game) : base(i_Game)
        {
            m_SoundEffects = new List<SoundEffectInstance>();
            m_BackgroundMusicVolume = 1;
            m_SoundsEffectsVolume = 1;
            m_ToggleSound = true;
        }

        protected override void RegisterAsService()
        {
            this.Game.Services.AddService(typeof(ISoundManager), this);
        }

        public void PlaySound(string i_AssetName, float i_Pitch = 0, float i_Pan = 0)
        {
            if (ToggleSound && SoundEffectsVolume > 0)
            {
                SoundEffect soundEffect = Game.Content.Load<SoundEffect>("Sounds/" + i_AssetName);
                soundEffect.Play(SoundEffectsVolume, i_Pitch, i_Pan);
            }
        }

        public SoundEffectInstance PlayContinuousSound(string i_AssetName, bool i_IsBackgroundMusic = false)
        {
            SoundEffectInstance soundEffectInstance = loadSoundEffect(i_AssetName).CreateInstance();
            soundEffectInstance.Volume = i_IsBackgroundMusic ? BackgroundMusicVolume : SoundEffectsVolume;
            soundEffectInstance.IsLooped = i_IsBackgroundMusic;
            soundEffectInstance.Play();

            if (i_IsBackgroundMusic)
            {
                m_BackgroundMusic = soundEffectInstance;
            }
            else
            {
                m_SoundEffects.Add(soundEffectInstance);
            }

            return soundEffectInstance;
        }

        private SoundEffect loadSoundEffect(string i_AssetName)
        {
            return Game.Content.Load<SoundEffect>("Sounds/" + i_AssetName);
        }

        protected virtual void ChangeBackgroundMusicVolume()
        {
            m_BackgroundMusic.Volume = BackgroundMusicVolume;
        }

        protected virtual void ChangeSoundEffectsVolume()
        {
            foreach (SoundEffectInstance soundEffects in m_SoundEffects)
            {
                soundEffects.Volume = SoundEffectsVolume;
            }
        }
    }
}
