using Infrastructure.ObjectModel;
using Invaders.Infrastructure;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel.Screens;

namespace Invaders.ObjectModel
{
    public class ScoreText : GameText2D
    {
        private string m_PlayerNameText;
        private int m_PlayerScore;

        private string playerName;

        public int PlayerScore
        {
            get { return m_PlayerScore; }
            set { m_PlayerScore = value; }
        }

        public string PlayerName
        {
            get { return playerName; }
            set { playerName = value; }
        }

        public ScoreText(BaseGame i_Game, GameScreen i_GameScreen, string i_AssetName, int i_PlayerNum) : base(i_Game, i_GameScreen, i_AssetName)
        {
            playerName = "P" + i_PlayerNum;
            m_PlayerNameText = playerName + " Score: ";
            m_PlayerScore = 0;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Text = m_PlayerNameText + m_PlayerScore;
        }

        public void AddPoints(int i_Points)
        {
            m_PlayerScore += i_Points;
        }

        public void RemovePoints(int i_Points)
        {
            AddPoints(-i_Points);
        }
    }
}
