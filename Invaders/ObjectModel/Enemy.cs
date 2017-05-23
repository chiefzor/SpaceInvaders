using System;
using Invaders.Infrastructure;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.ServiceInterfaces;

namespace Invaders.ObjectModel
{
    public class Enemy : Sprite, ICollidable2D
    {
        private const int k_NumOfFrames = 2;
        private const int k_NumOfEnemyTypes = 3;
        private const string k_AssetName = @"Sprites\Enemies";
        private const string k_SoundGunName = "EnemyGunShot";
        private const string k_SoundKillName = "EnemyKill";
        private Bullet m_Bullet;
        private bool m_BulletHitEnemy;
        private bool m_CanFireBullet = true;
        private int m_CurrentFrameIdx = 0;

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

        public event EventHandler HitFloor;

        public event EventHandler EnemyDied;

        public Enemy(BaseGame i_Game, Color i_EnemyColor, GameScreen i_GameScreen)
            : base(k_AssetName, i_Game, i_GameScreen)
        {
            m_Bullet = new Bullet(m_BaseGame, i_GameScreen, eBulletOf.Enemy);
            this.TintColor = i_EnemyColor;
        }

        public override void Initialize()
        {
            base.Initialize();
            m_Bullet.Visible = false;
            this.RotationOrigin = new Vector2(this.Width / 2, this.Height / 2);
        }

        protected override void InitOrigins()
        {
            base.InitOrigins();
        }

        protected override void InitSourceRectangle()
        {
            base.InitSourceRectangle();
            m_WidthBeforeScale = m_WidthBeforeScale / k_NumOfFrames;
            m_HeightBeforeScale = m_HeightBeforeScale / k_NumOfEnemyTypes;
            this.setSourceRectangle();
        }

        public void UpdateSourceRectangle()
        {
            if (!BulletHitEnemy)
            {
                m_CurrentFrameIdx++;
                m_CurrentFrameIdx = m_CurrentFrameIdx % 2;
                this.setSourceRectangle();
            }
        }

        private void setSourceRectangle()
        {
            if (this.TintColor == Color.LightPink)
            {
                m_SourceRectangle = new Rectangle(m_CurrentFrameIdx * (int)Width, 0, (int)Width, (int)Height);
            }

            if (this.TintColor == Color.LightBlue)
            {
                m_SourceRectangle = new Rectangle(m_CurrentFrameIdx * (int)Width, (int)Height, (int)Width, (int)Height);
            }

            if (this.TintColor == Color.LightYellow)
            {
                m_SourceRectangle = new Rectangle(m_CurrentFrameIdx * (int)Width, (int)Height * 2, (int)Width, (int)Height);
            }
        }

        public void Shoot()
        {
            if (m_CanFireBullet)
            {
                m_CanFireBullet = false;
                m_Bullet.Visible = true;
                m_Bullet.BulletVanished += enemyBulletVanished;
                m_Bullet.TintColor = Color.Blue;
                m_Bullet.Position = new Vector2(
                    m_Position.X + (Width / 2),
                    m_Position.Y + Width + (m_Bullet.Height / 2));
                float bulletVelocity = 110;
                m_Bullet.Velocity = new Vector2(0, bulletVelocity);
                (Game.Services.GetService(typeof(ISoundManager)) as ISoundManager).PlaySound(k_SoundGunName);
            }
        }

        private void enemyBulletVanished(object sender, EventArgs e)
        {
            m_Bullet.Visible = false;
            m_CanFireBullet = true;
        }

        public void MoveEnemy(float i_JumpDistance, Vector2 i_DirectionToMove)
        {
            if (i_DirectionToMove.Y == 1)
            {
                this.Position += new Vector2(0, this.Height / 2);
                if (this.Position.Y + this.Height > this.Game.GraphicsDevice.Viewport.Bounds.Bottom && this.Visible)
                {
                    this.onHittingFloor();
                }
            }
            else
            {
                this.Position += new Vector2(i_DirectionToMove.X * i_JumpDistance, 0);
            }
        }

        private void onHittingFloor()
        {
            if (this.HitFloor != null)
            {
                this.HitFloor(this, EventArgs.Empty);
            }
        }

        public void onEnemyDeath()
        {
            if (this.EnemyDied != null)
            {
                this.EnemyDied(this, EventArgs.Empty);
            }
        }

        public void SetPosition(float colPosition, float rowPosition)
        {
            this.Position = new Vector2(colPosition, rowPosition);
        }

        private bool m_IsDead;

        public void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is Bullet)
            {
                Bullet bullet = i_Collidable as Bullet;
                if (bullet.BulletOf == eBulletOf.SpaceShip && !m_IsDead)
                {
                    m_IsDead = true;
                    die();
                }
            }
        }

        private void die()
        {
            (Game.Services.GetService(typeof(ISoundManager)) as ISoundManager).PlaySound(k_SoundKillName);
            this.TimeLeftForSpecialEffects = 1.8f;
            this.AngularVelocity = 7f;
            m_SizeToScale = 0.98f;
        }

        public override void afterEffects()
        {
            base.afterEffects();
            
            this.Visible = false;
            onEnemyDeath();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}