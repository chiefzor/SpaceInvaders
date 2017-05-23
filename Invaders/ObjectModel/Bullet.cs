using System;
using Invaders.Infrastructure;
using Microsoft.Xna.Framework;
using Invaders.Helpers;
using Infrastructure.ObjectModel.Screens;

namespace Invaders.ObjectModel
{
    public class Bullet : Sprite, ICollidable2D
    {
        private const string k_AssetName = @"Sprites\Bullet";
        private eBulletOf m_BulletOf;
        private Random m_BulletVanishRandom;

        public event EventHandler BulletVanished;

        public event EnemyHitEventHandler EnemyKilled;

        public eBulletOf BulletOf
        {
            get { return this.m_BulletOf; }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (((this.Position.Y < (float)GraphicsDevice.Viewport.Y) || (this.Position.Y > (float)GraphicsDevice.Viewport.Height)) && this.Visible)
            {
                onVanishedBullet();
            }
        }

        public Bullet(BaseGame i_Game, GameScreen i_GameScreen, eBulletOf i_BulletOf)
            : base(k_AssetName, i_Game, i_GameScreen)
        {
            this.m_BulletOf = i_BulletOf;
            m_BulletVanishRandom = new Random();
        }

        public void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is Bullet)
            {
                if (this.BulletOf == eBulletOf.SpaceShip)
                {
                    if ((i_Collidable as Bullet).BulletOf != eBulletOf.SpaceShip)
                    {
                        this.Visible = false;
                    }
                }
                else
                {
                    int removeEnemyBulletIfOne = m_BulletVanishRandom.Next(1, 3);
                    if (removeEnemyBulletIfOne == 1)
                    {
                        this.Visible = false;
                    }
                }
            }

            if ((i_Collidable is Enemy && this.m_BulletOf == eBulletOf.SpaceShip && !(i_Collidable as Enemy).BulletHitEnemy) || (i_Collidable is Spaceship && this.m_BulletOf == eBulletOf.Enemy) || (i_Collidable is MotherShip && !(i_Collidable as MotherShip).BulletHitEnemy))
            {
                if (i_Collidable is Enemy)
                {
                    (i_Collidable as Enemy).BulletHitEnemy = true;
                }
                else if (i_Collidable is MotherShip)
                {
                    (i_Collidable as MotherShip).BulletHitEnemy = true;
                }

                this.Visible = false;
                onEnemyKilled(i_Collidable);
            }
        }

        private void onEnemyKilled(ICollidable i_Collidable)
        {
            if (EnemyKilled != null)
            {
                EnemyHitEventArgs enemyHitEventArgs = new EnemyHitEventArgs(i_Collidable);
                EnemyKilled.Invoke(this, enemyHitEventArgs);
            }
        }

        private void onVanishedBullet()
        {
            if (BulletVanished != null)
            {
                this.Visible = false;
                BulletVanished(this, EventArgs.Empty);
            }
        }
    }
}