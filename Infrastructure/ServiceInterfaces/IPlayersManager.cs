using System;
using System.Collections.Generic;
using Infrastructure.ObjectModel;

namespace Invaders.Infrastructure
{
    public interface IPlayersManager
    {
        event EventHandler GameOverEvent;

        event EventHandler LevelWonEvent;

        int CurrentLevel { get; set; }

        int MaxNumOfPlayers { get; set; }

        void Reset();

        void AddObjectToMonitor(Player i_LivableComponent);

        void CheckIfGameOver();

        Player GetCurrentPlayer(IPlayerObject i_PlayerObject);

        void GameOver();

        void LevelWon();

        List<Player> Players { get; }

        bool RestartGame { get; set; }
    }

    public interface IPlayerObject
    {
        Player Player { get; }

        string AssetName { get; set; }

        float Height { get; set; }
    }
}
