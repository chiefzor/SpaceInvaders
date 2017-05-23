using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.ServiceInterfaces;
using Invaders.Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Infrastructure.Menu
{
    public class Menu : GameComponent
    {
        private const string k_SoundName = "MenuMove";

        public List<MenuItem> m_MenuItems;

        private IScreensManager m_ScreensManager;

        private ISoundManager m_SoundManager;

        private IInputManager m_InputManager;

        protected BaseGame m_Game;

        private MenuItem m_CurrentActiveItem;

        private int m_ItemNumber;

        public float Width
        {
            get
            {
                float width = m_MenuItems.Max(item => item.Width);
                return width;
            }
        }

        public float Height
        {
            get
            {
                float height = 0;
                if (m_MenuItems.Count != 0)
                {
                    height += (Spacing * (m_MenuItems.Count - 1)) + (m_MenuItems.Count * m_MenuItems[0].Height);
                }

                return height;
            }
        }

        public float Spacing
        {
            get
            {
                float spacing = 0;
                if (m_MenuItems.Count != 0)
                {
                    spacing = m_MenuItems[0].Height / 2;
                }

                return spacing;
            }
        }

        public MenuItem CurrentActiveItem
        {
            get
            {
                return m_CurrentActiveItem;
            }

            set
            {
                m_CurrentActiveItem = value;
            }
        }

        public Menu(BaseGame i_Game, List<MenuItem> i_MenuItems) : base(i_Game)
        {
            m_MenuItems = i_MenuItems;
            m_Game = i_Game;
            m_ItemNumber = 0;
        }

        public override void Initialize()
        {
            base.Initialize();
            m_InputManager = m_Game.Services.GetService(typeof(IInputManager)) as IInputManager;
            m_ScreensManager = m_Game.Services.GetService(typeof(IScreensManager)) as IScreensManager;
            m_SoundManager = Game.Services.GetService(typeof(ISoundManager)) as ISoundManager;
            initDefaultPosition();
        }

        private void initDefaultPosition()
        {
            CurrentActiveItem = null;
            if (m_MenuItems.Count != 0)
            {
                float startPosHeight = (m_Game.GraphicsDevice.Viewport.Height / 2) - (Height / 2);
                int i = 0;
                foreach (MenuItem item in m_MenuItems)
                {
                    item.Position = new Vector2((m_Game.GraphicsDevice.Viewport.Width / 2) - (item.Width / 2), startPosHeight);
                    startPosHeight += item.Height + Spacing;
                    if (item.Bounds.Intersects(m_InputManager.BoundsMouse))
                    {
                        CurrentActiveItem = item;
                        item.IsFocused = true;
                        m_ItemNumber = i;
                        item.IsActive = true;
                    }

                    i++;
                }
            }
        }

        private bool m_PreviousMouseIntersection = false;

        public override void Update(GameTime i_gameTime)
        {
            if (m_InputManager.KeyPressed(Keys.Down))
            {
                if (m_CurrentActiveItem == null)
                {
                    m_CurrentActiveItem = m_MenuItems[0];
                    m_ItemNumber = 0;
                }
                else
                {
                    m_MenuItems[m_ItemNumber].IsActive = false;
                    m_ItemNumber = m_MenuItems.Count == m_ItemNumber + 1 ? 0 : ++m_ItemNumber;
                    m_CurrentActiveItem = m_MenuItems[m_ItemNumber];
                }

                m_MenuItems[m_ItemNumber].IsActive = true;
                m_MenuItems[m_ItemNumber].IsFocused = true;
                m_SoundManager.PlaySound(k_SoundName);
            }
            else if (m_InputManager.KeyPressed(Keys.Up))
            {
                if (m_CurrentActiveItem == null)
                {
                    m_CurrentActiveItem = m_MenuItems[m_MenuItems.Count - 1];
                    m_ItemNumber = m_MenuItems.Count - 1;
                }
                else
                {
                    m_MenuItems[m_ItemNumber].IsActive = false;
                    m_ItemNumber = m_ItemNumber - 1 == -1 ? m_MenuItems.Count - 1 : --m_ItemNumber;
                    m_CurrentActiveItem = m_MenuItems[m_ItemNumber];
                }

                m_MenuItems[m_ItemNumber].IsActive = true;
                m_MenuItems[m_ItemNumber].IsFocused = true;
                m_SoundManager.PlaySound(k_SoundName);
            }
            else if (m_InputManager.KeyPressed(Keys.Enter) || (m_CurrentActiveItem != null && m_InputManager.MouseLeftButtonPressed() && m_InputManager.MouseIntersectWithComponent(m_CurrentActiveItem)))
            {
                if (m_CurrentActiveItem is EnterMenuItem)
                {
                    m_SoundManager.PlaySound(k_SoundName);
                    (m_CurrentActiveItem as EnterMenuItem).Execute();
                }
            }
            else if (m_InputManager.MouseMoved())
            {
                int i = 0;
                bool mouseIntersection = false;
                foreach (MenuItem item in m_MenuItems)
                {
                    if (m_InputManager.MouseIntersectWithComponent(item))
                    {
                        mouseIntersection = true;
                        if (!item.IsFocused && item != CurrentActiveItem)
                        {
                            item.IsFocused = true;

                            m_MenuItems[m_ItemNumber].IsActive = false;
                            m_ItemNumber = i;
                            item.IsActive = true;
                            m_CurrentActiveItem = item;
                            m_SoundManager.PlaySound(k_SoundName);
                            break;
                        }
                    }

                    i++;
                }

                if (mouseIntersection && !m_PreviousMouseIntersection)
                {
                    m_PreviousMouseIntersection = mouseIntersection;
                }

                if (!mouseIntersection && m_PreviousMouseIntersection)
                {
                    if (CurrentActiveItem != null)
                    {
                        CurrentActiveItem.IsActive = false;
                        CurrentActiveItem.IsFocused = false;
                        CurrentActiveItem = null;
                        m_PreviousMouseIntersection = false;
                        mouseIntersection = false;
                    }
                }
            }
        }
    }
}