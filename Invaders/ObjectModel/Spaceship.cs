using System.Collections.Generic;
using System;
using Invaders.Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.ObjectModel;
using Invaders.Helpers;
using Infrastructure.ServiceInterfaces;
using Infrastructure.ObjectModel.Screens;

namespace Invaders.ObjectModel
{
    public class Spaceship : Sprite, ICollidable2D, IPlayerObject
    {
        private const float k_Velocity = 140f;
        private const int k_NumOfBullets = 2;
        private const string k_SoundGunShotName = "SSGunShot";
        private const string k_SoundLifeDieName = "LifeDie";
        private readonly List<Bullet> m_ShipBullets = new List<Bullet>();
        private IInputManager m_InputManager;
        private IPlayersManager m_PlayerManager;
        private Player m_Player = null;
        private bool m_IsDead;

        public List<Bullet> ShipBullets
        {
            get { return m_ShipBullets; }
        }

        public Player Player
        {
            get
            {
                if (m_Player == null)
                {
                    m_Player = m_PlayerManager.GetCurrentPlayer(this);
                }

                return m_Player;
            }
        }

        public Spaceship(BaseGame i_Game, string i_AssetName, GameScreen i_GameScreen)
            : base(i_AssetName, i_Game, i_GameScreen)
        {
        }

        private void initSpaceshipBullets()
        {
            for (int i = 0; i < k_NumOfBullets; i++)
            {
                Bullet bullet = new Bullet(m_BaseGame, m_BaseScreen, eBulletOf.SpaceShip);
                bullet.Visible = false;
                bullet.EnemyKilled += onBulletHitEnemy;
                bullet.BulletVanished += onBulletVanished;
                bullet.TintColor = Color.Red;

                m_ShipBullets.Add(bullet);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            initSpaceshipBullets();
            this.m_InputManager = Game.Services.GetService(typeof(IInputManager)) as IInputManager;
            m_PlayerManager = Game.Services.GetService(typeof(IPlayersManager)) as IPlayersManager;
            this.RotationOrigin = new Vector2(this.Width / 2, this.Height / 2);
            m_UsePremultAlpha = false;
        }

        public void Shoot()
        {
            if (Player.Souls.Count != 0)
            {
                foreach (Bullet bullet in m_ShipBullets)
                {
                    if (!bullet.Visible)
                    {
                        bullet.Position = new Vector2(
                            m_Position.X + (Width / 2) - (bullet.Width / 2),
                            m_Position.Y - (bullet.Height / 2));
                        float bulletVelocity = 110;
                        bullet.Velocity = new Vector2(0, -bulletVelocity);
                        bullet.Visible = true;
                        (Game.Services.GetService(typeof(ISoundManager)) as ISoundManager).PlaySound(k_SoundGunShotName);
                        break;
                    }
                }
            }
        }

        private void onBulletVanished(object sender, EventArgs e)
        {
            Bullet bullet = sender as Bullet;
        }

        private void onBulletHitEnemy(object sender, EnemyHitEventArgs e)
        {
            ICollidable someEnemy = e.CollidedEnemy;
            int pointsToAdd = 0;
            if (someEnemy is MotherShip)
            {
                pointsToAdd = 600;
            }
            else if (someEnemy is Enemy)
            {
                IPlayersManager playersManager = this.Game.Services.GetService(typeof(IPlayersManager)) as IPlayersManager;

                int relativeLevel = (playersManager.CurrentLevel - 1) % 6;

                Enemy enemy = someEnemy as Enemy;
                if (enemy.TintColor == Color.LightPink)
                {
                    pointsToAdd = 220 + (50 * relativeLevel);
                }
                else if (enemy.TintColor == Color.LightBlue)
                {
                    pointsToAdd = 160 + (50 * relativeLevel);
                }
                else
                {
                    pointsToAdd = 90 + (50 * relativeLevel);
                }
            }

            Player.ScoreOfTheGame.AddPoints(pointsToAdd);
        }

        private void die()
        {
            (Game.Services.GetService(typeof(ISoundManager)) as ISoundManager).PlaySound(k_SoundLifeDieName);
            this.m_BlinkRate = 1 / 9f;
            m_TimeLeftForSpecialEffects = 2.6f;
            onNumOfLivesChanged();
            if (Player.Souls.Count == 0)
            {
                m_OpacityFactor = 0.99f;
                AngularVelocity = 3f;
                m_IsDead = true;
            }

            float x = (float)GraphicsDevice.Viewport.X;
            float y = (float)GraphicsDevice.Viewport.Height;
            y -= this.Height / 2;
            y -= 30;
            this.Position = new Vector2(x, y);
        }

        private void onNumOfLivesChanged()
        {
            Player.LoseASoul();
            if (Player.ScoreOfTheGame.PlayerScore >= 1500)
            {
                Player.ScoreOfTheGame.RemovePoints(1500);
            }
            else
            {
                Player.ScoreOfTheGame.PlayerScore = 0;
            }
        }

        public override void afterEffects()
        {
            base.afterEffects();
            this.Visible = true;

            if (Player.Souls.Count == 0)
            {
                this.Visible = false;
                m_PlayerManager.CheckIfGameOver();
            }
        }

        public void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is Bullet && ((Bullet)i_Collidable).BulletOf == eBulletOf.Enemy)
            {
                if (!m_IsDead)
                {
                    this.die();
                }
            }
            else if (i_Collidable is Enemy)
            {
                m_PlayerManager.GameOver();
            }
        }
    }
}