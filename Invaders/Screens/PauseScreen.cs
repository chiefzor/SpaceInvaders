using System;
using Infrastructure.ObjectModel.Screens;
using Invaders.Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Invaders.Screens
{
    public class PauseScreen : GameScreen
    {
        private SpriteFont m_FontCalibri;
        private Vector2 m_MsgPosition = new Vector2(70, 300);

        public PauseScreen(BaseGame i_Game)
            : base(i_Game)
        {
            this.IsModal = true;
            this.IsOverlayed = true;
            this.UseGradientBackground = true;
            this.BlackTintAlpha = 0.60f;
            this.UseFadeTransition = true;

            this.ActivationLength = TimeSpan.FromSeconds(0.5f);
            this.DeactivationLength = TimeSpan.FromSeconds(0.5f);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            m_FontCalibri = ContentManager.Load<SpriteFont>(@"Fonts\24Calibri");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.KeyboardState.IsKeyDown(Keys.R))
            {
                this.ExitScreen();
            }

            m_MsgPosition.X = (float)Math.Pow(70, TransitionPosition);
        }

            public override void Draw(GameTime gameTime)
        {
            string text = @"
[ Pause ]

R - Resume Game";
            base.Draw(gameTime);

            SpriteBatch.Begin();
            SpriteBatch.DrawString(
                m_FontCalibri,
                text,
                m_MsgPosition,
                Color.White);

            SpriteBatch.End();
        }
    }
}
