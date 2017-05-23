using Invaders.Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.ServiceInterfaces;

namespace Infrastructure.ObjectModel
{
    public abstract class Component2D : LoadableDrawableComponent
    {
        protected bool m_UseSharedBatch = false;
        protected SpriteBatch m_SpriteBatch;

        public SpriteBatch SpriteBatch
        {
            set
            {
                m_SpriteBatch = value;
                m_UseSharedBatch = true;
            }
        }

        protected float m_WidthBeforeScale;

        public float WidthBeforeScale
        {
            get { return m_WidthBeforeScale; }
            set { m_WidthBeforeScale = value; }
        }

        protected float m_HeightBeforeScale;

        public float HeightBeforeScale
        {
            get { return m_HeightBeforeScale; }
            set { m_HeightBeforeScale = value; }
        }

        public float Width
        {
            get { return m_WidthBeforeScale * m_Scales.X; }
            set { m_WidthBeforeScale = value / m_Scales.X; }
        }

        public float Height
        {
            get { return m_HeightBeforeScale * m_Scales.Y; }
            set { m_HeightBeforeScale = value / m_Scales.Y; }
        }

        protected Vector2 m_Position = Vector2.Zero;

        public Vector2 Position
        {
            get { return m_Position; }
            set
            {
                if (m_Position != value)
                {
                    m_Position = value;
                    OnPositionChanged();
                }
            }
        }

        protected Vector2 m_Scales = Vector2.One;

        public Vector2 Scales
        {
            get { return m_Scales; }
            set
            {
                if (m_Scales != value)
                {
                    m_Scales = value;
                    OnPositionChanged();
                }
            }
        }

        protected Vector2 m_PositionOrigin;

        public Vector2 PositionOrigin
        {
            get { return m_PositionOrigin; }
            set { m_PositionOrigin = value; }
        }

        public Vector2 m_RotationOrigin = Vector2.Zero;

        public Vector2 RotationOrigin
        {
            get { return m_RotationOrigin; }
            set { m_RotationOrigin = value; }
        }

        protected Vector2 PositionForDraw
        {
            get
            {
                return this.Position - this.PositionOrigin + this.RotationOrigin;
            }
        }

        protected float m_Rotation = 0;

        public float Rotation
        {
            get { return m_Rotation; }
            set { m_Rotation = value; }
        }

        protected Color m_TintColor = Color.White;

        public Color TintColor
        {
            get { return m_TintColor; }
            set { m_TintColor = value; }
        }

        protected float m_LayerDepth;

        public float LayerDepth
        {
            get { return m_LayerDepth; }
            set { m_LayerDepth = value; }
        }

        public Component2D ShallowClone()
        {
            return this.MemberwiseClone() as Component2D;
        }

        protected GameScreen m_BaseScreen;

        public GameScreen BaseScreen
        {
            get { return m_BaseScreen; }
        }

        public Vector2 TopLeftPosition
        {
            get { return this.Position - this.PositionOrigin; }
            set { this.Position = value + this.PositionOrigin; }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)TopLeftPosition.X,
                    (int)TopLeftPosition.Y,
                    (int)this.Width,
                    (int)this.Height);
            }
        }

        public Rectangle BoundsBeforeScale
        {
            get
            {
                return new Rectangle(
                    (int)TopLeftPosition.X,
                    (int)TopLeftPosition.Y,
                    (int)this.WidthBeforeScale,
                    (int)this.HeightBeforeScale);
            }
        }

        public Component2D(string i_AssetName, BaseGame i_Game, GameScreen i_BaseScreen)
            : base(i_AssetName, i_Game, int.MaxValue)
        {
            addThisInScreen(i_BaseScreen);
        }

        public Component2D(string i_AssetName, BaseGame i_Game, GameScreen i_BaseScreen, int i_UpdateOrder, int i_DrawOrder)
            : base(i_AssetName, i_Game, i_UpdateOrder, i_DrawOrder)
        {
            addThisInScreen(i_BaseScreen);
        }

        public Component2D(string i_AssetName, BaseGame i_Game,  GameScreen i_BaseScreen, int i_CallsOrder)
            : base(i_AssetName, i_Game, i_CallsOrder)
        {
            addThisInScreen(i_BaseScreen);
        }

        private IGameSettingsManager m_GameSettingsManager;

        public override void Initialize()
        {
            base.Initialize();
            m_GameSettingsManager = m_BaseGame.Services.GetService(typeof(IGameSettingsManager)) as IGameSettingsManager;
        }

        private void addThisInScreen(GameScreen i_BaseScreen)
        {
            m_BaseScreen = i_BaseScreen;
            i_BaseScreen.Add(this);
        }

        protected override abstract void InitBounds();
    }
}