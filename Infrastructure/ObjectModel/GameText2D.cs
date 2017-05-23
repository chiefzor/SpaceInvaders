using Invaders.Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.ObjectModel.Screens;

namespace Infrastructure.ObjectModel
{
    public class GameText2D : Component2D
    {
        private SpriteFont m_Font;
        private string m_OutputText = string.Empty;

        public string Text
        {
            get { return m_OutputText; }
            set
            {
                m_OutputText = value;
                InitBounds();
            }
        }

        public GameText2D(BaseGame m_Game, GameScreen i_GameScreen, string i_AssetName) : base(i_AssetName, m_Game, i_GameScreen)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            this.m_SpriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
        }

        protected override void InitBounds()
        {
            Vector2 size = m_Font.MeasureString(m_OutputText);
            WidthBeforeScale = size.X;
            HeightBeforeScale = size.Y;
        }

        protected override void LoadContent()
        {
            m_Font = Game.Content.Load<SpriteFont>(AssetName);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            m_SpriteBatch.DrawString(m_Font, m_OutputText, PositionForDraw, TintColor, Rotation, RotationOrigin, Scales, SpriteEffects.None, LayerDepth);
            base.Draw(gameTime);
        }
    }
}