using System;
using Infrastructure.ObjectModel.Screens;
using Invaders.Infrastructure;
using Invaders.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Invaders.Screens
{
    public class GameOverScreen : GameScreen
    {
        private string m_ScoreText;
        private SpriteFont m_FontCalibri;
        private Vector2 m_MsgPosition = new Vector2(20, 300);
        private IPlayersManager m_PlayerManager;
        private BaseGame m_Game;

        public GameOverScreen(BaseGame i_Game, string i_ScoreText)
            : base(i_Game)
        {
            m_Game = i_Game;
            Background background = new Background(i_Game, this, @"Sprites\GameOver");
            background.TintColor = Color.Red;
            m_ScoreText = i_ScoreText + "\n\r\n\rPress Esc to Quit\n\rR to Restart\n\rH for Main Menu";
            this.ActivationLength = TimeSpan.FromSeconds(1);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_FontCalibri = ContentManager.Load<SpriteFont>(@"Fonts\Calibri");
        }

        public override void Initialize()
        {
            base.Initialize();

            m_PlayerManager = m_Game.Services.GetService(typeof(IPlayersManager)) as IPlayersManager;
            m_PlayerManager.RestartGame = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.KeyPressed(Keys.Escape))
            {
                m_Game.ExitGame();
            }

            if(InputManager.KeyPressed(Keys.H))
            {
                ScreensManager.SetCurrentScreen(new MainMenuScreen(m_Game));
                Dispose();
                ExitScreen();
            }

            if (InputManager.KeyPressed(Keys.R))
            {
                ScreensManager.SetCurrentScreen(new LevelTransitionScreen(m_Game));
                Dispose();
                ExitScreen();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SpriteBatch.Begin();
            SpriteBatch.DrawString(m_FontCalibri, m_ScoreText, m_MsgPosition, Color.White);
            SpriteBatch.End();
        }
    }
}