using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.ObjectModel.Screens;

namespace Invaders.Infrastructure
{
    public class Sprite : Component2D
    {
        protected CompositeAnimator m_Animations;

        public CompositeAnimator Animations
        {
            get { return m_Animations; }
            set { m_Animations = value; }
        }

        protected bool m_IsActive = false;

        public bool IsActive
        {
            get { return m_IsActive; }
            set { m_IsActive = value; }
        }

        protected Texture2D m_Texture;

        public Texture2D Texture
        {
            get { return m_Texture; }
            set { m_Texture = value; }
        }

        protected bool m_UsePremultAlpha = true;

        public bool UsePremultAlpha
        {
            get { return m_UsePremultAlpha; }
            set { m_UsePremultAlpha = value; }
        }

        protected float m_TimeLeftForSpecialEffects = -1;

        public float TimeLeftForSpecialEffects
        {
            get { return m_TimeLeftForSpecialEffects; }
            set { m_TimeLeftForSpecialEffects = value; }
        }

        protected float m_BlinkRate = -1;

        public float BlinkRate
        {
            get { return m_BlinkRate; }
            set { m_BlinkRate = value; }
        }

        protected float m_SizeToScale = 1;

        public float SizeToScale
        {
            get { return m_SizeToScale; }
            set { m_SizeToScale = value; }
        }

        protected Rectangle m_SourceRectangle;

        public Rectangle SourceRectangle
        {
            get { return m_SourceRectangle; }
            set { m_SourceRectangle = value; }
        }

        public Vector2 TextureCenter
        {
            get
            {
                return new Vector2((float)(m_Texture.Width / 2), (float)(m_Texture.Height / 2));
            }
        }

        public Vector2 SourceRectangleCenter
        {
            get { return new Vector2((float)(m_SourceRectangle.Width / 2), (float)(m_SourceRectangle.Height / 2)); }
        }

        protected float m_OpacityFactor = 1;

        public float Opacity
        {
            get { return (float)m_TintColor.A / (float)byte.MaxValue; }
            set { m_TintColor.A = (byte)(value * (float)byte.MaxValue); }
        }

        protected SpriteEffects m_SpriteEffects = SpriteEffects.None;

        public SpriteEffects SpriteEffects
        {
            get { return m_SpriteEffects; }
            set { m_SpriteEffects = value; }
        }

        protected Vector2 m_Velocity = Vector2.Zero;

        /// <summary>
        /// Pixels per Second on 2 axis
        /// </summary>
        public Vector2 Velocity
        {
            get { return m_Velocity; }
            set { m_Velocity = value; }
        }

        private float m_AngularVelocity = 0;

        /// <summary>
        /// Radians per Second on X Axis
        /// </summary>
        public float AngularVelocity
        {
            get { return m_AngularVelocity; }
            set { m_AngularVelocity = value; }
        }

        public Sprite(string i_AssetName, BaseGame i_Game, GameScreen i_BaseScreen, int i_UpdateOrder, int i_DrawOrder)
            : base(i_AssetName, i_Game, i_BaseScreen, i_UpdateOrder, i_DrawOrder)
        {
        }

        public Sprite(string i_AssetName, BaseGame i_Game, GameScreen i_BaseScreen, int i_CallsOrder)
            : base(i_AssetName, i_Game, i_BaseScreen, i_CallsOrder)
        {
        }

        public Sprite(string i_AssetName, BaseGame i_Game, GameScreen i_BaseScreen)
            : base(i_AssetName, i_Game, i_BaseScreen, int.MaxValue)
        {
        }

        /// <summary>
        /// Default initialization of bounds
        /// </summary>
        /// <remarks>
        /// Derived classes are welcome to override this to implement their specific boudns initialization
        /// </remarks>
        protected override void InitBounds()
        {
            m_WidthBeforeScale = m_Texture.Width;
            m_HeightBeforeScale = m_Texture.Height;
            m_Position = Vector2.Zero;
            InitSourceRectangle();
            InitOrigins();
        }

        protected virtual void InitOrigins()
        {
            PositionOrigin = new Vector2(0, 0);
            m_OpacityFactor = 1;
            Opacity = 1;
            m_SizeToScale = 1;
            Scales = Vector2.One;
        }

        protected virtual void InitSourceRectangle()
        {
            m_SourceRectangle = new Rectangle(0, 0, (int)m_WidthBeforeScale, (int)m_HeightBeforeScale);
        }

        protected override void LoadContent()
        {
            m_Texture = Game.Content.Load<Texture2D>(m_AssetName);
            base.LoadContent();
        }

        public override void Initialize()
        {
            base.Initialize();
            m_Animations = new CompositeAnimator(this);
        }

        private float m_timeForCurrentVisibilityState = 0;

        public virtual void afterEffects()
        {
            TimeLeftForSpecialEffects = -1;
        }

        public override void Update(GameTime gameTime)
        {
            if (TimeLeftForSpecialEffects >= 0)
            {
                TimeLeftForSpecialEffects -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (m_TimeLeftForSpecialEffects <= 0)
                {
                    afterEffects();
                }

                if (m_BlinkRate != -1)
                {
                    m_timeForCurrentVisibilityState += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (Visible && m_timeForCurrentVisibilityState >= m_BlinkRate)
                    {
                        Visible = false;
                        m_timeForCurrentVisibilityState -= m_BlinkRate;
                    }

                    if (!Visible && m_timeForCurrentVisibilityState >= m_BlinkRate)
                    {
                        Visible = true;
                        m_timeForCurrentVisibilityState -= m_BlinkRate;
                    }
                }
            }

            this.Opacity *= m_OpacityFactor;
            this.Scales *= this.SizeToScale;
            this.Position += this.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.Rotation += this.AngularVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }

        /// <summary>
        /// Basic texture draw behavior, using a shared/own sprite batch
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (!m_UsePremultAlpha)
            {
                m_SpriteBatch.End();
                m_SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
            }
            else if (!m_UseSharedBatch)
            {
                m_SpriteBatch.Begin();
            }

            m_SpriteBatch.Draw(
                m_Texture, 
                this.PositionForDraw,
                this.SourceRectangle, 
                this.TintColor,
                this.Rotation,
                this.RotationOrigin,
                this.Scales,
                SpriteEffects.None,
                this.LayerDepth);

            if (!m_UsePremultAlpha || !m_UseSharedBatch)
            {
                m_SpriteBatch.End();
                m_SpriteBatch.Begin();
            }

            base.Draw(gameTime);
        }

        public virtual bool CheckCollision(ICollidable i_Source)
        {
            bool collided = false;
            ICollidable2D source = i_Source as ICollidable2D;
            if (source != null)
            {
                collided = source.Bounds.Intersects(this.Bounds);
            }

            return collided;
        }

        public virtual bool CheckPixelCollision(ICollidable i_Source)
        {
            Color[] rawDataThis = new Color[Texture.Width * Texture.Height];
            Texture.GetData<Color>(rawDataThis);
            ICollidable2D source = i_Source as ICollidable2D;

            if (source != null)
            {
                Color[] rawDataSource = new Color[source.Texture.Width * source.Texture.Height];
                source.Texture.GetData<Color>(rawDataSource);
                int posXPositionCheckStart = Math.Max(this.Bounds.X, source.Bounds.X);
                int posXPositionCheckEnd = Math.Min(this.Bounds.X + this.Bounds.Width, source.Bounds.X + source.Bounds.Width);
                int posYPositionCheckStart = Math.Max(this.Bounds.Y, source.Bounds.Y);
                int posYPositionCheckEnd = Math.Min(this.Bounds.Y + this.Bounds.Height, source.Bounds.Y + source.Bounds.Height);

                for (int y = posYPositionCheckStart; y < posYPositionCheckEnd; y++)
                {
                    for (int x = posXPositionCheckStart; x < posXPositionCheckEnd; x++)
                    {
                        int indexPotentialCollisionPixelThis = (x - this.Bounds.X) + ((y - this.Bounds.Y) * this.Texture.Width);
                        int indexPotentialCollisionPixelSource = (x - source.Bounds.X) + ((y - source.Bounds.Y) * source.Texture.Width);
                        Color PixelOfThis = rawDataThis[indexPotentialCollisionPixelThis];
                        Color PixelOfSource = rawDataSource[indexPotentialCollisionPixelSource];

                        if (PixelOfThis.A != 0 && PixelOfSource.A != 0)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}