using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure.ObjectModel;

namespace Invaders.Infrastructure
{
    public class InputManager : GameService, IInputManager
    {
        private KeyboardState m_PrevKeyboardState;

        public KeyboardState PrevKeyboardState
        {
            get { return m_PrevKeyboardState; }
        }

        private KeyboardState m_KeyboardState;

        public KeyboardState KeyboardState
        {
            get { return m_KeyboardState; }
        }

        private MouseState m_PrevMouseState;

        public MouseState PrevMouseState
        {
            get { return m_PrevMouseState; }
        }

        private MouseState m_MouseState;

        public MouseState MouseState
        {
            get { return m_MouseState; }
        }

        private BaseGame m_Game;

        public InputManager(BaseGame i_Game)
            : base(i_Game, int.MinValue)
        {
            m_Game = i_Game;
        }

        public Rectangle BoundsMouse
        {
            get
            {
                return new Rectangle(new Point(MouseState.X, MouseState.Y), new Point(0, 0));
            }
        }

        public override void Initialize()
        {
            m_PrevKeyboardState = Keyboard.GetState();
            m_KeyboardState = m_PrevKeyboardState;

            m_PrevMouseState = Mouse.GetState();
            m_MouseState = m_PrevMouseState;
        }

        public bool MouseIntersectWithComponent(Component2D i_GameComponent)
        {
            float componentWidth = i_GameComponent.Width * m_Game.Window.ClientBounds.Size.X / (float)m_Game.PreviousGameWindowSize.X;
            float componentHeight = i_GameComponent.Height * m_Game.Window.ClientBounds.Size.Y / (float)m_Game.PreviousGameWindowSize.Y;
            Vector2 componentPosition = new Vector2(i_GameComponent.Position.X * m_Game.Window.ClientBounds.Size.X / (float)m_Game.PreviousGameWindowSize.X, i_GameComponent.Position.Y * m_Game.Window.ClientBounds.Size.Y / (float)m_Game.PreviousGameWindowSize.Y);
            Rectangle rectangleComponent = new Rectangle((int)componentPosition.X, (int)componentPosition.Y, (int)componentWidth, (int)componentHeight);

            return BoundsMouse.Intersects(rectangleComponent);
        }

        protected override void RegisterAsService()
        {
            Game.Services.AddService(typeof(IInputManager), this);
        }

        public override void Update(GameTime i_GameTime)
        {
            m_PrevKeyboardState = m_KeyboardState;
            m_KeyboardState = Keyboard.GetState();

            m_PrevMouseState = m_MouseState;
            m_MouseState = Mouse.GetState();
        }

        public bool KeyReleased(Keys i_Key)
        {
            return
                m_PrevKeyboardState.IsKeyDown(i_Key)
                &&
                m_KeyboardState.IsKeyUp(i_Key);
        }

        public bool KeyPressed(Keys i_Key)
        {
            return m_PrevKeyboardState.IsKeyUp(i_Key) && m_KeyboardState.IsKeyDown(i_Key);
        }

        public bool MouseMoved()
        {
            return m_PrevMouseState != m_MouseState;
        }

        public bool MouseLeftButtonPressed()
        {
            return m_MouseState.LeftButton == ButtonState.Pressed && m_PrevMouseState.LeftButton == ButtonState.Released;
        }

        public bool MouseRightButtonPressed()
        {
            return m_MouseState.RightButton == ButtonState.Pressed && m_PrevMouseState.RightButton == ButtonState.Released;
        }

        public bool MouseWheelScrolledUp()
        {
            return m_MouseState.ScrollWheelValue > m_PrevMouseState.ScrollWheelValue;
        }

        public bool MouseWheelScrolledDown()
        {
            return m_MouseState.ScrollWheelValue < m_PrevMouseState.ScrollWheelValue;
        }
    }
}
