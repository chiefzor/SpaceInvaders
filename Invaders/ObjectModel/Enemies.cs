using System;
using Invaders.Infrastructure;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel.Screens;

namespace Invaders.ObjectModel
{
    public class Enemies : GameComponent
    {
        private const int k_NumOfFrames = 2;
        private readonly Vector2 m_EnemyOne;
        private readonly Vector2 m_EnemyTwo;
        private readonly Vector2 m_EnemyThree;
        private int m_MatrixRowSize = 5;
        private int m_MatrixColSize = 9;      
        private BaseGame m_Game;
        private Enemy[,] m_EnemiesMatrix;
        private bool m_ReachedBottom;
        private float m_EnemyWidth;
        private float m_EnemyHeight;
        private float m_JumpTimeInterval;
        private float m_TimeToMoveEnemy;
        private float m_ShootTime;
        private Vector2 m_directionToMove;
        private Random m_EnemiesRandomShoot;

        public Enemies(GameScreen i_GameScreen, BaseGame i_Game) : base(i_Game)
        {
            m_EnemyOne = new Vector2(0, 0);
            m_EnemyTwo = new Vector2(0, 32);
            m_EnemyThree = new Vector2(0, 64);
            this.m_Game = i_Game;
            this.createEnemiesMatrix(i_GameScreen, i_Game);
            m_EnemiesRandomShoot = new Random();
        }

        private void createEnemiesMatrix(GameScreen i_GameScreen, BaseGame i_Game)
        {
            IPlayersManager playersManager = this.Game.Services.GetService(typeof(IPlayersManager)) as IPlayersManager;

            int relativeLevel = (playersManager.CurrentLevel - 1) % 6;

            m_MatrixColSize += relativeLevel;

            this.m_EnemiesMatrix = new Enemy[m_MatrixRowSize, m_MatrixColSize];

            for (int i = 0; i < m_MatrixRowSize; i++)
            {
                for (int j = 0; j < m_MatrixColSize; j++)
                {
                    if (i == 0)
                    {
                        this.m_EnemiesMatrix[i, j] = new Enemy(i_Game, Color.LightPink, i_GameScreen);
                    }
                    else if (i == 1 || i == 2)
                    {
                        this.m_EnemiesMatrix[i, j] = new Enemy(i_Game, Color.LightBlue, i_GameScreen);
                    }
                    else
                    {
                        this.m_EnemiesMatrix[i, j] = new Enemy(i_Game, Color.LightYellow, i_GameScreen);
                    }

                    this.m_EnemiesMatrix[i, j].HitFloor += this.enemiesHitBottom;
                    this.m_EnemiesMatrix[i, j].EnemyDied += this.enemyDied;
                }
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            this.m_JumpTimeInterval = 0.5f;
            this.m_directionToMove = new Vector2(1, 0);
            this.m_EnemyWidth = this.m_EnemiesMatrix[0, 0].Width;
            this.m_EnemyHeight = this.m_EnemiesMatrix[0, 0].Height;
            this.initEnemiesPositions();
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            this.randomShoot(i_GameTime);
            this.jump(i_GameTime);
        }

        private void initEnemiesPositions()
        {
            float rowPosition = 0, colPosition = 0;
            int enemyAnimationIndex;
            bool sameColorFlag;

            for (int i = 0; i < m_MatrixRowSize; i++)
            {
                sameColorFlag = false;
                enemyAnimationIndex = (i % k_NumOfFrames) * (int)m_EnemyWidth;
                colPosition = 0;

                if (i >= 1)
                {
                    if (this.m_EnemiesMatrix[i, 0].TintColor == this.m_EnemiesMatrix[i - 1, 0].TintColor)
                    {
                        sameColorFlag = true;
                    }
                }

                for (int j = 0; j < m_MatrixColSize; j++)
                {
                    if (i == 0)
                    {
                        rowPosition = this.m_EnemiesMatrix[i, j].Height * 3;
                    }

                    if (sameColorFlag)
                    {
                        this.m_EnemiesMatrix[i, j].UpdateSourceRectangle();
                    }

                    this.m_EnemiesMatrix[i, j].SetPosition(colPosition, rowPosition);

                    colPosition += (float)Math.Round(this.m_EnemiesMatrix[0, 0].Width + (0.6f * this.m_EnemiesMatrix[0, 0].Width));
                }

                rowPosition += (float)Math.Round(this.m_EnemiesMatrix[0, 0].Height + (0.6f * this.m_EnemiesMatrix[0, 0].Width));
            }
        }

        private bool checkIfWon()
        {
            foreach (Enemy enemy in this.m_EnemiesMatrix)
            {
                if (enemy.Visible)
                {
                    return false;
                }
            }

            return true;
        }

        private void randomShoot(GameTime i_GameTime)
        {
            int randomEnemiesRowIndex;
            int randomEnemiesColIndex;
            IPlayersManager playersManager = this.Game.Services.GetService(typeof(IPlayersManager)) as IPlayersManager;

            int relativeLevel = (playersManager.CurrentLevel - 1) % 6;

            do
            {
                randomEnemiesRowIndex = m_EnemiesRandomShoot.Next(0, this.m_EnemiesMatrix.GetLength(0));
                randomEnemiesColIndex = m_EnemiesRandomShoot.Next(0, this.m_EnemiesMatrix.GetLength(1));
            }
            while (!this.m_EnemiesMatrix[randomEnemiesRowIndex, randomEnemiesColIndex].Visible);
            this.m_ShootTime += (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            if (this.m_ShootTime > (float)m_EnemiesRandomShoot.Next(1, 5) - (relativeLevel * 0.17))
            {
                this.m_ShootTime = 0;
                this.m_EnemiesMatrix[randomEnemiesRowIndex, randomEnemiesColIndex].Shoot();
            }
        }

        private void jump(GameTime i_GameTime)
        {
            float jumpDistance = this.jumpDistance();
            this.m_TimeToMoveEnemy += (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            if (this.m_TimeToMoveEnemy >= this.m_JumpTimeInterval)
            {
                foreach (Enemy enemy in this.m_EnemiesMatrix)
                {
                    if (!this.m_ReachedBottom)
                    {
                        enemy.MoveEnemy(jumpDistance, this.m_directionToMove);
                        enemy.UpdateSourceRectangle();
                    }
                }

                if (this.m_directionToMove.Y == 1)
                {
                    this.m_JumpTimeInterval *= 0.94f;
                }

                if (jumpDistance != this.m_EnemyWidth / 2)
                {
                    if (this.m_directionToMove.X == 1)
                    {
                        this.m_directionToMove = new Vector2(-1, 1);
                    }
                    else
                    {
                        this.m_directionToMove = new Vector2(1, 1);
                    }
                }
                else
                {
                    this.m_directionToMove.Y = 0;
                }

                this.m_TimeToMoveEnemy -= this.m_JumpTimeInterval;
            }
        }

        private void changeTextureOfEnemiesToDraw()
        {
            foreach (Enemy enemy in m_EnemiesMatrix)
            {
                enemy.UpdateSourceRectangle();
            }
        }

        private float jumpDistance()
        {
            foreach (Enemy enemy in this.m_EnemiesMatrix)
            {
                if (this.m_directionToMove.X == 1)
                {
                    if ((float)this.Game.GraphicsDevice.Viewport.Width - (enemy.Position.X + enemy.Width + (this.m_EnemyWidth / 2)) < 0 && enemy.Visible)
                    {
                        return (float)this.Game.GraphicsDevice.Viewport.Width - (enemy.Position.X + enemy.Width);
                    }
                }
                else
                {
                    if (enemy.Position.X - (this.m_EnemyWidth / 2) < 0 && enemy.Visible)
                    {
                        return enemy.Position.X;
                    }
                }
            }

            return this.m_EnemyWidth / 2;
        }

        private void enemyDied(object sender, EventArgs e)
        {
            if (this.checkIfWon())
            {
                IPlayersManager playerManager = Game.Services.GetService(typeof(IPlayersManager)) as IPlayersManager;
                playerManager.LevelWon();
            }
            else
            {
                this.m_JumpTimeInterval *= 0.94f;
            }
        }

        private void enemiesHitBottom(object sender, EventArgs e)
        {
            this.m_ReachedBottom = true;
            IPlayersManager playerManager = Game.Services.GetService(typeof(IPlayersManager)) as IPlayersManager;
            playerManager.GameOver();
        }
    }
}