using Infrastructure.ObjectModel.Screens;

namespace Invaders.Infrastructure
{
    public class Soul : Sprite
    {
        public Soul(BaseGame i_Game, GameScreen i_GameScreen, string i_AssetName) : base(i_AssetName, i_Game, i_GameScreen)
        {
            m_UsePremultAlpha = false;
        }

        public override void Initialize()
        {
            base.Initialize();
            this.Opacity = 0.5f;
        }
    }
}
