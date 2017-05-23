using System;
using Invaders.Infrastructure;

namespace Invaders.Helpers
{
    public delegate void EnemyHitEventHandler(object sender, EnemyHitEventArgs e);

    public class EnemyHitEventArgs : EventArgs
    {
        private ICollidable m_CollidedEnemy;

        public ICollidable CollidedEnemy
        {
            get { return m_CollidedEnemy; }
            set { m_CollidedEnemy = value; }
        }

        public EnemyHitEventArgs(ICollidable i_CollidedEnemy)
        {
            m_CollidedEnemy = i_CollidedEnemy;
        }
    }
}
