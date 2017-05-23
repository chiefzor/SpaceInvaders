using System;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.ServiceInterfaces;
using Invaders.Infrastructure;
using Microsoft.Xna.Framework;

namespace Infrastructure.Menu
{
    public class MenuItem : GameText2D
    {
        private CompositeAnimator m_Animators;
        private bool m_IsActive;
        protected IScreensManager m_ScreensManager;
        protected IInputManager m_InputManager;
        protected BaseGame m_Game;

        protected string m_MenuItemName;

        public bool IsFocused { get; set; }

        public bool IsActive
        {
            get { return m_IsActive; }
            set
            {
                m_IsActive = value;
                TintColor = value == true ? Color.Red : Color.White;
                if (value)
                {
                    m_Animators.Resume();
                }
                else
                {
                    IsFocused = false;
                    m_Animators.Reset();
                    m_Animators.Pause();
                }
            }
        }

        public MenuItem(BaseGame i_Game, GameScreen i_GameScreen, string i_MenuItemName) : base(i_Game, i_GameScreen, @"Fonts\40BoldSizeCalibri")
        {
            m_Game = i_Game;
            m_MenuItemName = i_MenuItemName;
        }

        public override void Initialize()
        {
            base.Initialize();

            m_ScreensManager = Game.Services.GetService(typeof(IScreensManager)) as IScreensManager;
            PulseAnimator menuItemPulseAnimator = new PulseAnimator("Pulse", TimeSpan.Zero, 1.1f, 1f);
            m_InputManager = m_Game.Services.GetService(typeof(IInputManager)) as IInputManager;
            m_Animators = new CompositeAnimator(this);
            m_Animators.Add(menuItemPulseAnimator);
            Text = m_MenuItemName;
            RotationOrigin = new Vector2(Position.X + (Width / 2), Position.Y + (Height / 2));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (m_Animators != null)
            {
                m_Animators.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}