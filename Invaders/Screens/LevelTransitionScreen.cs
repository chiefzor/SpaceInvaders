using System;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Invaders.Infrastructure;
using Invaders.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Invaders.Screens
{
    public class LevelTransitionScreen : GameScreen
    {
        private string m_LevelCounterText;
        private GameText2D m_LevelGameText;
        private int m_Level;
        private string m_LevelText;
        private SpriteFont m_FontCalibri;
        private Vector2 m_LevelCounterTextPosition;
        private IPlayersManager m_PlayersManager;

        private BaseGame m_Game;

        public LevelTransitionScreen(BaseGame i_Game)
            : base(i_Game)
        {
            m_LevelCounterText = "3";
            m_Game = i_Game;
            Background background = new Background(i_Game, this, 1);
            m_LevelGameText = new GameText2D(i_Game, this, @"Fonts\40BoldSizeCalibri");

            this.ActivationLength = TimeSpan.FromSeconds(1);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_FontCalibri = ContentManager.Load<SpriteFont>(@"Fonts\24Calibri");
        }

        public override void Initialize()
        {
            base.Initialize();
            m_LevelCounterTextPosition = new Vector2(m_Game.GraphicsDevice.Viewport.Width / 2, m_Game.GraphicsDevice.Viewport.Height / 2);
            m_PlayersManager = this.Game.Services.GetService(typeof(IPlayersManager)) as IPlayersManager;
            m_LevelGameText.Text = "Level " + (m_PlayersManager.CurrentLevel + 1).ToString();
            m_LevelGameText.Position = new Vector2(m_LevelCounterTextPosition.X - (m_LevelGameText.Width / 2), m_LevelGameText.Height);
            m_Level = m_PlayersManager.CurrentLevel + 1;
        }

        private float m_TimeLeft = 3;

        private float m_PartialTime;

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            float totalSec = (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            m_PartialTime += totalSec;

            if (m_PartialTime >= 1)
            {
                m_TimeLeft--;
                m_PartialTime = 0;
            }

            m_LevelCounterText = m_TimeLeft.ToString();

            if (m_TimeLeft < 1)
            {
                m_TimeLeft = 3;
                m_PlayersManager.CurrentLevel++;
                ScreensManager.SetCurrentScreen(new PlayScreen(m_Game));
                Dispose();
                ScreensManager.Remove(this);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            m_LevelText = string.Format(m_LevelCounterText);

            base.Draw(gameTime);
            SpriteBatch.Begin();
            SpriteBatch.DrawString(m_FontCalibri, m_LevelText, m_LevelCounterTextPosition, Color.White);
            SpriteBatch.End();
        }
    }
}
