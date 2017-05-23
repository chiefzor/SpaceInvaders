using System.Collections.Generic;
using System.Linq;
using Invaders.Infrastructure;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel.Screens;
using Invaders.Screens;
using Infrastructure.Managers;

namespace Invaders.ObjectModel
{
    public class Barriers : GameComponent
    {
        private List<Barrier> m_Barriers;
        private int m_NumOfBarriers;

        private float xPositionBeforeMoving;

        private GameScreen m_GameScreen;

        public Barriers(GameScreen i_BaseScreen, BaseGame i_Game, int i_NumOfBarriers) : base(i_Game)
        {
            m_GameScreen = i_BaseScreen;
            m_Barriers = new List<Barrier>();
            m_NumOfBarriers = i_NumOfBarriers;
            createBarriers(i_BaseScreen, i_Game);
        }

        private void createBarriers(GameScreen i_BaseScreen, BaseGame i_Game)
        {
            for (int i = 0; i < m_NumOfBarriers; i++)
            {
                Barrier barrier = new Barrier(i_Game, i_BaseScreen);
                m_Barriers.Add(barrier);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            initBarriersPosition();
        }

        private void initBarriersPosition()
        {
            float positionBarriersX = (Game.GraphicsDevice.Viewport.Width / 2) - ((((m_NumOfBarriers * 2) - 1) * m_Barriers[0].Width) / 2);

            this.xPositionBeforeMoving = positionBarriersX;

            float positionBarriersY = 0;

            Spaceship someSpaceship =
                ((Game.Components.First(component => component is ScreensManager) as ScreensManager).First(screen => screen is PlayScreen) as PlayScreen).Component2Ds.First(sprite => sprite is Spaceship) as Spaceship;

            positionBarriersY = someSpaceship.Position.Y - (someSpaceship.Height * 2);

            foreach (IGameComponent gameComponent in m_GameScreen)
            {
                if (gameComponent is Spaceship)
                {
                    positionBarriersY = (gameComponent as Spaceship).Position.Y - ((gameComponent as Spaceship).Height * 2);
                    break;
                }
            }

            foreach (Barrier barrier in m_Barriers)
            {
                barrier.Position = new Vector2(positionBarriersX, positionBarriersY);
                positionBarriersX += barrier.Width * 2;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (m_Barriers[0].Position.X >= this.xPositionBeforeMoving + (m_Barriers[0].Width / 2) || m_Barriers[0].Position.X <= this.xPositionBeforeMoving - (m_Barriers[0].Width / 2))
            {
                foreach (Barrier barrier in m_Barriers)
                {
                    barrier.Velocity *= -1;
                }
            }
        }
    }
}
