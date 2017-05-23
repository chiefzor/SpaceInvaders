using Infrastructure.ObjectModel;
using Invaders.Infrastructure;
using Microsoft.Xna.Framework;

namespace Invaders.ObjectModel
{
    public class SpaceshipPlayer : Player
    {
        private const int k_NumOfSouls = 3;

        public Spaceship Spaceship
        {
            get { return PlayerObject as Spaceship; }
        }

        public SpaceshipPlayer(BaseGame i_Game, string i_AssetName, Spaceship i_Spaceship) : base(i_Game, i_AssetName, k_NumOfSouls, i_Spaceship)
        {
        }

        public void SetSoulsProperties(float i_YPosSouls)
        {
            this.ScaleSoul = new Vector2(0.5f, 0.5f);
            this.DirectionToPlaceEnemies = new Vector2(-1, 0);
            this.XSpacingBetweenSouls = -5;
            this.XPosSouls = m_Game.GraphicsDevice.Viewport.Width - 5;
            this.YPosSouls = i_YPosSouls;
            this.InitSouls();
            this.AddToPlayerManager();
        }

        public void AddToPlayerManager()
        {
            IPlayersManager livesManager = this.m_Game.Services.GetService(typeof(IPlayersManager)) as IPlayersManager;

            if (livesManager != null)
            {
                livesManager.AddObjectToMonitor(this as Player);
            }
        }
    }
}
