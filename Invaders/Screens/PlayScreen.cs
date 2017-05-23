using System;
using Infrastructure.ObjectModel.Screens;
using Invaders.Infrastructure;
using Invaders.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Invaders.Screens
{
    public class PlayScreen : GameScreen
    {
        private BaseGame m_Game;
        private SpriteFont m_FontCalibri;
        private IPlayersManager m_PlayersManager;

        public PlayScreen(BaseGame i_Game)
            : base(i_Game)
        {
            m_Game = i_Game;
            initLevelDetails();
        }

        private void initLevelDetails()
        {
            Background background = new Background(m_Game, this);
            SpaceshipPlayers players = new SpaceshipPlayers(m_Game, this);
            this.Add(players);
            MotherShip motherShip = new MotherShip(m_Game, this);
            Enemies enemies = new Enemies(this, m_Game);
            this.Add(enemies);
            int numOfBarriers = 4;
            Barriers barriers = new Barriers(this, m_Game, numOfBarriers);
            this.Add(barriers);
            m_PlayersManager = Game.Services.GetService(typeof(IPlayersManager)) as IPlayersManager;
            m_PlayersManager.GameOverEvent += onGameOver;
            m_PlayersManager.LevelWonEvent += onLevelWon;
        }

        private void onLevelWon(object sender, EventArgs e)
        {
            LevelTransitionScreen levelTransScreen = new LevelTransitionScreen(m_Game);
            ScreensManager.SetCurrentScreen(levelTransScreen);
            Dispose();
            ScreensManager.Remove(this);
        }

        private void onGameOver(object sender, EventArgs e)
        {
            string scoreText = sender as string;
            ScreensManager.SetCurrentScreen(new GameOverScreen(m_Game, scoreText));
            Dispose();
            ScreensManager.Remove(this);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            this.ActivationLength = TimeSpan.Zero;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_FontCalibri = ContentManager.Load<SpriteFont>(@"Fonts\24Calibri");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.KeyPressed(Keys.P))
            {
                ScreensManager.SetCurrentScreen(new PauseScreen(m_Game));
            }
        }
    }
}
