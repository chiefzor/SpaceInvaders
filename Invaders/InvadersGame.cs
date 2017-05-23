using Invaders.Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.Managers;
using Invaders.Screens;
using Infrastructure.ServiceInterfaces;
using Invaders.LocalManagers;

namespace Invaders
{
    public class InvadersGame : BaseGame
    {
        private const string k_SoundBackgroundName = "BGMusic";
        private ISoundManager m_SoundManager;

        public InvadersGame()
        {
            this.m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IInputManager inputManager = new InputManager(this);
            IPlayersManager livesManager = new PlayersManager(this);
            ICollisionsManager collisionsManager = new CollisionsManager(this);
            IScreensManager screensManager = new ScreensManager(this);
            m_SoundManager = new LocalSoundManager(this);
            IGameSettingsManager IGameSettingsManager = new GameSettingsManager(this);
            screensManager.SetCurrentScreen(new WelcomeScreen(this));           
        }

        protected override void Initialize()
        {
            this.m_Graphics.PreferredBackBufferHeight = 600;
            this.m_Graphics.PreferredBackBufferWidth = 800;
            this.m_Graphics.ApplyChanges();
            this.SpriteBatch = new SpriteBatch(GraphicsDevice);
            this.Services.AddService(typeof(SpriteBatch), this.SpriteBatch);
            this.IsMouseVisible = true;
            m_SoundManager.PlayContinuousSound(k_SoundBackgroundName, true);
            base.Initialize();
        }
      
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
             
        protected override void Draw(GameTime gameTime)
        {
            this.m_Graphics.GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
        
        public override void ExitGame()
        {
            this.Exit();
        }
    }
}