using System;
using System.Collections.Generic;
using Infrastructure.ObjectModel.Screens;
using Invaders.Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Infrastructure.Menu
{
    public class ToggleMenuItem<T, K> : MenuItem
    {
        private List<T> m_ToggleItems;
        private List<K> m_CorrespondingMeaningToggleItems;

        private T m_ToggledItem;

        private int m_CurrentIdxItem = 0;

        public T ToggledItem
        {
            get { return m_ToggledItem; }
            set
            {
                m_ToggledItem = value;
                Text = m_MenuItemName + m_ToggledItem.ToString();
            }
        }

        private bool m_IsCircular = true;

        public bool IsCircular
        {
            get { return m_IsCircular; }
            set { m_IsCircular = value; }
        }

        private K m_CorrespondingToggledItem;

        public K CorrespondingToggledItem
        {
            get { return m_CorrespondingToggledItem; }
            set
            {
                m_CorrespondingToggledItem = value;
                m_CurrentIdxItem = m_CorrespondingMeaningToggleItems.IndexOf(m_CorrespondingToggledItem);
            }
        }

        public void UpdateBasedOnCorrespondingItem()
        {
            ToggledItem = m_ToggleItems[m_CurrentIdxItem];
        }

        public event EventHandler Toggle;

        public ToggleMenuItem(BaseGame i_Game, GameScreen i_GameScreen, List<T> i_ToggleItems, List<K> i_CorrespondingMeaningToggleItems, string i_MenuItemName) : base(i_Game, i_GameScreen, i_MenuItemName)
        {
            m_ToggleItems = i_ToggleItems;
            m_CorrespondingMeaningToggleItems = i_CorrespondingMeaningToggleItems;
        }

        public override void Initialize()
        {
            base.Initialize();
            if (m_ToggleItems.Count != 0)
            {
                ToggledItem = m_ToggleItems[m_CurrentIdxItem];
                CorrespondingToggledItem = m_CorrespondingMeaningToggleItems[m_CurrentIdxItem];
                Execute();
            }
        }

        public void Execute()
        {
            Text = m_MenuItemName + ToggledItem.ToString();
            if (Toggle != null)
            {
                Toggle.Invoke(this, new ToggleEventArgs<K>(CorrespondingToggledItem));
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (this.IsActive)
            {
                if (m_InputManager.KeyPressed(Keys.PageDown) || ((m_InputManager.MouseWheelScrolledDown() || m_InputManager.MouseRightButtonPressed()) && Bounds.Intersects(m_InputManager.BoundsMouse)))
                {
                    if (m_IsCircular)
                    {
                        m_CurrentIdxItem = m_ToggleItems.Count == m_CurrentIdxItem + 1 ? 0 : ++m_CurrentIdxItem;
                    }
                    else
                    {
                        m_CurrentIdxItem = m_ToggleItems.Count == m_CurrentIdxItem + 1 ? m_CurrentIdxItem : ++m_CurrentIdxItem;
                    }

                    ToggledItem = m_ToggleItems[m_CurrentIdxItem];
                    CorrespondingToggledItem = m_CorrespondingMeaningToggleItems[m_CurrentIdxItem];
                    Execute();
                }
                else if (m_InputManager.KeyPressed(Keys.PageUp) || (m_InputManager.MouseWheelScrolledUp() && Bounds.Intersects(m_InputManager.BoundsMouse)))
                {
                    if (m_IsCircular)
                    {
                        m_CurrentIdxItem = m_CurrentIdxItem - 1 == -1 ? m_ToggleItems.Count - 1 : --m_CurrentIdxItem;
                    }
                    else
                    {
                        m_CurrentIdxItem = m_CurrentIdxItem - 1 == -1 ? 0 : --m_CurrentIdxItem;
                    }

                    ToggledItem = m_ToggleItems[m_CurrentIdxItem];
                    CorrespondingToggledItem = m_CorrespondingMeaningToggleItems[m_CurrentIdxItem];
                    Execute();
                }
            }
        }
    }
}
