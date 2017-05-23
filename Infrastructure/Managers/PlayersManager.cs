using System;
using System.Collections.Generic;
using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Infrastructure.ServiceInterfaces;

namespace Invaders.Infrastructure
{
    public class PlayersManager : GameService, IPlayersManager
    {
        private const string k_SoundLevelWinName = "LevelWin";

        private const string k_SoundGameOverName = "GameOver";

        private readonly List<Player> m_PlayersComponents = new List<Player>();

        private int m_CurrentLevel = 0;

        private int m_MaxNumOfPlayers = 2;

        private int m_NumOfRemainingPlayers = 0;

        private BaseGame m_BaseGame;

        private bool m_IsGameOver;

        public event EventHandler GameOverEvent;

        public event EventHandler LevelWonEvent;

        public List<Player> Players
        {
            get { return m_PlayersComponents; }
        }

        public int CurrentLevel
        {
            get
            {
                return m_CurrentLevel;
            }

            set
            {
                m_CurrentLevel = value;
            }
        }

        private bool m_RestartGame = false;

        public bool RestartGame
        {
            get { return m_RestartGame; }
            set
            {
                m_RestartGame = value;
                if (value)
                {
                    restartlevel();
                }
            }
        }

        private void restartlevel()
        {
            m_CurrentLevel = 0;
            Reset();
        }

        public int MaxNumOfPlayers
        {
            get
            {
                return m_MaxNumOfPlayers;
            }

            set
            {
                m_MaxNumOfPlayers = value;
            }
        }

        public PlayersManager(BaseGame i_Game)
            : base(i_Game, int.MinValue)
        {
            m_BaseGame = i_Game;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void RegisterAsService()
        {
            this.Game.Services.AddService(typeof(IPlayersManager), this);
        }

        public void AddObjectToMonitor(Player i_Player)
        {
            if (!this.m_PlayersComponents.Contains(i_Player))
            {
                m_PlayersComponents.Add(i_Player);
                i_Player.PlayerDestroyed += playerDestroyed;
                m_NumOfRemainingPlayers++;
            }
        }

        private void playerDestroyed(object sender, EventArgs e)
        {
            Player playerToRemove = sender as Player;
            m_NumOfRemainingPlayers--;
        }

        public void GameOver()
        {
            if (!m_IsGameOver)
            {
                string textToShow = "GameOver!\n";
                int maxScore = 0;
                Player bestPlayer = null;
                foreach (Player player in m_PlayersComponents)
                {
                    if (player.ScoreOfTheGame.PlayerScore > maxScore)
                    {
                        bestPlayer = player;
                        maxScore = player.ScoreOfTheGame.PlayerScore;
                    }

                    textToShow += player.ScoreOfTheGame.Text + "\n";
                }

                if (bestPlayer != null)
                {
                    textToShow += "\nThe winnder is: " + bestPlayer.ScoreOfTheGame.PlayerName + "\nWith " + bestPlayer.ScoreOfTheGame.PlayerScore + " points!";
                }

                (Game.Services.GetService(typeof(ISoundManager)) as ISoundManager).PlaySound(k_SoundGameOverName);
                GameOverEvent.Invoke(textToShow, EventArgs.Empty);
                m_IsGameOver = true;
            }
        }

        public void CheckIfGameOver()
        {
            if (m_NumOfRemainingPlayers == 0)
            {
                GameOver();
            }
        }

        public void LevelWon()
        {
            (Game.Services.GetService(typeof(ISoundManager)) as ISoundManager).PlaySound(k_SoundLevelWinName);
            LevelWonEvent.Invoke(this, EventArgs.Empty);
        }

        public Player GetCurrentPlayer(IPlayerObject i_PlayerObject)
        {
            Player player = m_PlayersComponents.Find(somePlayer => somePlayer.PlayerObject == i_PlayerObject);
            return player;
        }

        public void Reset()
        {
            m_PlayersComponents.Clear();
            m_NumOfRemainingPlayers = 0;
            m_IsGameOver = false;
        }
    }
}
