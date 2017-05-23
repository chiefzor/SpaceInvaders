using Infrastructure.ObjectModel.Screens;
using Invaders.Infrastructure;
using Microsoft.Xna.Framework;

namespace Invaders.ObjectModel
{
    public class Background : Sprite
    {
        private const string k_DefaultAssetName = @"Sprites\BG_Space01_1024x768";

        public Background(BaseGame i_Game, GameScreen i_GameScreen)
            : base(k_DefaultAssetName, i_Game, i_GameScreen)
        {
        }

        public Background(BaseGame i_Game, GameScreen i_GameScreen, int i_Opacity)
            : this(i_Game, i_GameScreen)
        {
            this.Opacity = i_Opacity;
        }

        public Background(BaseGame i_Game, GameScreen i_GameScreen, string i_AssetName) : base(i_AssetName, i_Game, i_GameScreen)
        {
        }

        protected override void InitBounds()
        {
            base.InitBounds();

            this.DrawOrder = int.MinValue;
        }

        public override void Draw(GameTime i_GameTime)
        {
            base.Draw(i_GameTime);
            m_SpriteBatch.Draw(this.Texture, this.GraphicsDevice.Viewport.Bounds, this.TintColor);
        }
    }
}
