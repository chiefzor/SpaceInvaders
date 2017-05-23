using Infrastructure.Managers;
using Invaders.Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure.ServiceInterfaces;

namespace Invaders.LocalManagers
{
    public class LocalSoundManager : SoundManager
    {
        private IInputManager m_InputManager;
        private IScreensManager m_IScreensManager;

        public LocalSoundManager(BaseGame i_Game) : base(i_Game)
        {
        }

        public override void Initialize()
        {
            m_InputManager = Game.Services.GetService(typeof(IInputManager)) as IInputManager;
            m_IScreensManager = Game.Services.GetService(typeof(IScreensManager)) as IScreensManager;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(m_InputManager.KeyPressed(Keys.M))
            {
                ToggleSound = !ToggleSound;
            }
        }
    }
}
