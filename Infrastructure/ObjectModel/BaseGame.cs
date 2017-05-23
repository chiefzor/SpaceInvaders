using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Invaders.Infrastructure
{
    public abstract class BaseGame : Game
    {
        private SpriteBatch m_SpriteBatch;

        protected GraphicsDeviceManager m_Graphics;

        public GraphicsDeviceManager Graphics
        {
            get { return m_Graphics; }
        }

        private Point m_PreviousWindowSize;

        public Point PreviousGameWindowSize
        {
            get
            {
                return m_PreviousWindowSize;
            }

            set
            {
                m_PreviousWindowSize = value;
            }
        }

        protected SpriteBatch SpriteBatch
        {
            get
            {
                return m_SpriteBatch;
            }

            set
            {
                m_SpriteBatch = value;
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
            m_PreviousWindowSize = Window.ClientBounds.Size;
        }

        public abstract void ExitGame();
    }
}