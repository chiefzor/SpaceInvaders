using System;
using Invaders.Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.ServiceInterfaces;

namespace Invaders.ObjectModel
{
    public class Barrier : Sprite, ICollidable2D
    {
        private const string k_AssetName = @"Sprites\Barrier_44x32";
        private const string k_SoundName = "BarrierHit";

        public Barrier(BaseGame i_Game, GameScreen i_BaseScreen) : base(k_AssetName, i_Game, i_BaseScreen)
        {
            this.Velocity = new Vector2(40, 0);
            m_UsePremultAlpha = false;
        }

        public override void Initialize()
        {
            base.Initialize();
            IPlayersManager playersManager = this.Game.Services.GetService(typeof(IPlayersManager)) as IPlayersManager;
            if ((playersManager.CurrentLevel - 1) % 6 == 0)
            {
                this.Velocity = new Vector2(0, 0);
            }
            else
            {
                this.Velocity = new Vector2(40 * (float)Math.Pow(1.06, (playersManager.CurrentLevel - 1) % 6), 0);
            }
        }

        protected override void InitBounds()
        {
            base.InitBounds();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is Bullet)
            {
                if (CheckPixelCollision(i_Collidable))
                {
                    bite(i_Collidable as Bullet);
                }
            }

            if (i_Collidable is Enemy)
            {
                erase(i_Collidable as Enemy);
            }
        }

        private void erase(Enemy i_Enemy)
        {
            Color[] rawDataBarrier = new Color[Texture.Width * Texture.Height];
            Texture.GetData<Color>(rawDataBarrier);

            if (i_Enemy != null && i_Enemy.Visible)
            {
                Color[] rawDataEnemy = new Color[i_Enemy.Texture.Width * i_Enemy.Texture.Height];
                i_Enemy.Texture.GetData<Color>(rawDataEnemy);
                int posXPositionCheckStart = Math.Max(this.Bounds.X, i_Enemy.Bounds.X);
                int posXPositionCheckEnd = Math.Min(this.Bounds.X + this.Bounds.Width, i_Enemy.Bounds.X + i_Enemy.Bounds.Width);
                int posYPositionCheckStart = Math.Max(this.Bounds.Y, i_Enemy.Bounds.Y);
                int posYPositionCheckEnd = Math.Min(this.Bounds.Y + this.Bounds.Height, i_Enemy.Bounds.Y + i_Enemy.Bounds.Height);

                for (int y = posYPositionCheckStart; y < posYPositionCheckEnd; y++)
                {
                    for (int x = posXPositionCheckStart; x < posXPositionCheckEnd; x++)
                    {
                        int indexPotentialCollisionPixelBarrier = (x - this.Bounds.X) + ((y - this.Bounds.Y) * this.Texture.Width);
                        Color PixelOfBarrier = rawDataBarrier[indexPotentialCollisionPixelBarrier];

                        if (PixelOfBarrier.A != 0)
                        {
                            rawDataBarrier[indexPotentialCollisionPixelBarrier].A = 0;
                        }
                    }
                }

                this.Texture = new Texture2D(this.Game.GraphicsDevice, this.Texture.Width, Texture.Height);
                this.Texture.SetData<Color>(rawDataBarrier);
            }
        }

        private void bite(Bullet i_Bullet)
        {
            Color[] rawDataBarrier = new Color[Texture.Width * Texture.Height];
            Texture.GetData<Color>(rawDataBarrier);

            Bullet bullet = i_Bullet;

            if (bullet != null && bullet.Visible)
            {
                bullet.Visible = false;
                int posXPositionCheckStart = Math.Max(this.Bounds.X, bullet.Bounds.X);
                int posXPositionCheckEnd = Math.Min(this.Bounds.X + this.Bounds.Width, bullet.Bounds.X + bullet.Bounds.Width);
                int posYPositionCheckStart = Math.Max(this.Bounds.Y, bullet.Bounds.Y);
                int posYPositionCheckEnd = Math.Min(this.Bounds.Y + this.Bounds.Height, bullet.Bounds.Y + bullet.Bounds.Height);

                if (bullet.Velocity.Y < 0)
                {
                    for (int i = posYPositionCheckStart + 1; i > posYPositionCheckStart - (int)(bullet.Height * 0.4f) && i >= this.Bounds.Y; i--)
                    {
                        for (int j = posXPositionCheckStart; j < posXPositionCheckEnd; j++)
                        {
                            int indexPotentialCollisionPixelBarrier = (j - this.Bounds.X) + ((i - this.Bounds.Y) * this.Texture.Width);
                            rawDataBarrier[indexPotentialCollisionPixelBarrier].A = 0;
                        }
                    }
                }
                else
                {
                    for (int i = posYPositionCheckStart; i < posYPositionCheckEnd + (int)(bullet.Height * 0.4f) && i < this.Bounds.Bottom; i++)
                    {
                        for (int j = posXPositionCheckStart; j < posXPositionCheckEnd; j++)
                        {
                            int indexPotentialCollisionPixelBarrier = (j - this.Bounds.X) + ((i - this.Bounds.Y) * this.Texture.Width);
                            rawDataBarrier[indexPotentialCollisionPixelBarrier].A = 0;
                        }
                    }
                }

                (Game.Services.GetService(typeof(ISoundManager)) as ISoundManager).PlaySound(k_SoundName);
                this.Texture = new Texture2D(this.Game.GraphicsDevice, this.Texture.Width, this.Texture.Height);
                this.Texture.SetData<Color>(rawDataBarrier);
            }
        }
    }
}
