using System;
using Invaders.Infrastructure;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.ServiceInterfaces;

namespace Invaders.ObjectModel
{
    public class MotherShip : Sprite, ICollidable2D
    {
        private const string k_AssetName = @"Sprites\MotherShip_32x120";
        private const string k_SoundName = "MotherShipKill";
        private const float k_DefaultVelocity = 95f;
        private float m_AppearTime;
        private bool m_BulletHitEnemy;
        private bool m_IsOnScreen;
        private bool m_IsDead;
        private Random m_RandomMotherShipAppearance;

        public bool BulletHitEnemy
        {
            get
            {
                return m_BulletHitEnemy;
            }

            set
            {
                m_BulletHitEnemy = value;
            }
        }

        public MotherShip(BaseGame i_Game, GameScreen i_GameScreen) : base(k_AssetName, i_Game, i_GameScreen)
        {
            this.TintColor = Color.Red;
            m_RandomMotherShipAppearance = new Random();
        }

        public override void Initialize()
        {
            m_UsePremultAlpha = false;

            base.Initialize();
        }

        protected override void InitBounds()
        {
            base.InitBounds();
            this.Position = new Vector2(this.Game.GraphicsDevice.Viewport.Bounds.Left - Width, Height);
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            if (!this.m_IsOnScreen)
            {
                this.randomAppearance(i_GameTime);
            }

            if ((this.Position.X > this.Game.GraphicsDevice.Viewport.Bounds.Right) && this.TimeLeftForSpecialEffects == -1)
            {
                this.Velocity = new Vector2(0);
                this.m_IsOnScreen = false;
                InitBounds();
            }
        }

        private void randomAppearance(GameTime i_GameTime)
        {
            this.m_AppearTime += (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            if (this.m_AppearTime > m_RandomMotherShipAppearance.Next(7, 15))
            {
                this.Visible = true;
                this.m_AppearTime = 0;
                m_IsOnScreen = true;
                m_BulletHitEnemy = false;
                m_IsDead = false;
                this.Velocity = new Vector2(k_DefaultVelocity, 0);
            }
        }

        private void die()
        {
            this.TimeLeftForSpecialEffects = 2.6f;
            m_OpacityFactor = 0.99f;
            m_SizeToScale = 0.99f;
            m_BlinkRate = 0.1f;
            (Game.Services.GetService(typeof(ISoundManager)) as ISoundManager).PlaySound(k_SoundName);
        }

        public override void afterEffects()
        {
            this.Visible = false;
            base.afterEffects();
        }

        public void Collided(ICollidable i_Collidable)
        {
            if (!m_IsDead)
            {
                die();
                m_IsDead = true;
            }
        }
    }
}