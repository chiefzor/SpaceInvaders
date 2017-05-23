using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel
{
    public class GameComponentEventArgs<ComponentType> : EventArgs
        where ComponentType : IGameComponent
    {
        private ComponentType m_Component;

        public GameComponentEventArgs(ComponentType gameComponent)
        {
            m_Component = gameComponent;
        }

        public ComponentType GameComponent
        {
            get { return m_Component; }
        }
    }
}
