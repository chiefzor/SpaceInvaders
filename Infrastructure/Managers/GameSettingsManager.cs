using Infrastructure.ServiceInterfaces;
using Invaders.Infrastructure;
using Microsoft.Xna.Framework;

namespace Infrastructure.Managers
{
    public class GameSettingsManager : GameService, IGameSettingsManager
    {
        private BaseGame m_Game;
        private ISoundManager m_SoundManager;
        private bool m_MouseVisibility = true;

        public bool MouseVisibility
        {
            get { return m_MouseVisibility; }
            set
            {
                m_MouseVisibility = value;
                m_Game.IsMouseVisible = value;
            }
        }

        private bool m_AllowResize;

        public bool AllowWindowResize
        {
            get { return m_AllowResize; }
            set
            {
                m_AllowResize = value;
                m_Game.Window.AllowUserResizing = value;
            }
        }

        private bool m_FullScreen;

        public bool FullScreen
        {
            get { return m_FullScreen; }
            set
            {
                m_FullScreen = value;
                if (value != m_Game.Graphics.IsFullScreen)
                {
                    m_Game.Graphics.IsFullScreen = value;
                    m_Game.Graphics.ApplyChanges();
                }
            }
        }

        private int m_NumOfPlayers = 2;

        public int NumOfPlayers
        {
            get { return m_NumOfPlayers; }
            set
            {
                m_NumOfPlayers = value;
                IPlayersManager playerManager = Game.Services.GetService(typeof(IPlayersManager)) as IPlayersManager;
                playerManager.MaxNumOfPlayers = value;
            }
        }

        public float BackgroundVolume
        {
            get
            {
                return m_SoundManager.BackgroundMusicVolume;
            }

            set
            {
                m_SoundManager.BackgroundMusicVolume = value;
            }
        }

        public float SoundEffectsVolume
        {
            get
            {
                return m_SoundManager.SoundEffectsVolume;
            }

            set
            {
                m_SoundManager.SoundEffectsVolume = value;
            }
        }

        public bool MusicToggle
        {
            get
            {
                return m_SoundManager.ToggleSound;
            }

            set
            {
                m_SoundManager.ToggleSound = value;
            }
        }

        public GameSettingsManager(BaseGame i_Game) : base(i_Game)
        {
            m_Game = i_Game;
            m_SoundManager = Game.Services.GetService(typeof(ISoundManager)) as ISoundManager;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void RegisterAsService()
        {
            this.Game.Services.AddService(typeof(IGameSettingsManager), this);
        }
    }
}
