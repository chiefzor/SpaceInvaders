using System.Collections.Generic;
using Invaders.Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure.ObjectModel.Screens;

namespace Invaders.ObjectModel
{
    public class SpaceshipPlayers : GameComponent
    {
        private const string k_AssetNameShip1 = @"Sprites\Ship01_32x32";
        private const string k_AssetNameShip2 = @"Sprites\Ship02_32x32";
        private const float k_Velocity = 140f;
        private readonly List<SpaceshipPlayer> m_SpaceshipPlayers;
        private BaseGame m_Game;
        private int m_NumOfPlayers;
        private bool m_Spaceship1StartedMoving = false;
        private IInputManager m_InputManager;
        private IPlayersManager m_PlayerManager;
        private GameScreen m_GameScreen;

        public SpaceshipPlayers(BaseGame i_Game, GameScreen i_BaseScreen) : base(i_Game)
        {
            m_SpaceshipPlayers = new List<SpaceshipPlayer>();
            m_GameScreen = i_BaseScreen;
            m_Game = i_Game;
            this.m_PlayerManager = Game.Services.GetService(typeof(IPlayersManager)) as IPlayersManager;
            m_NumOfPlayers = m_PlayerManager.MaxNumOfPlayers;
            this.createSpaceshipPlayers(i_BaseScreen, i_Game);
        }

        private void createSpaceshipPlayers(GameScreen i_BaseScreen, BaseGame i_Game)
        {
            for (int i = 0; i < m_NumOfPlayers; i++)
            {
                SpaceshipPlayer spaceshipPlayer = null;

                if (i == 0)
                {
                    Spaceship spaceship = new Spaceship(m_Game, k_AssetNameShip1, i_BaseScreen);
                    spaceshipPlayer = new SpaceshipPlayer(m_Game, k_AssetNameShip1, spaceship);
                }

                if (i == 1)
                {
                    Spaceship spaceship = new Spaceship(m_Game, k_AssetNameShip2, i_BaseScreen);
                    spaceshipPlayer = new SpaceshipPlayer(m_Game, k_AssetNameShip2, spaceship);
                }

                if (spaceshipPlayer != null)
                {
                    m_SpaceshipPlayers.Add(spaceshipPlayer);
                }
            }
        }

        public override void Initialize()
        {
            this.m_InputManager = Game.Services.GetService(typeof(IInputManager)) as IInputManager;

            initSpaceShipPositions();
            initPlayersProperties();
            base.Initialize();
        }

        private void initPlayersProperties()
        {
            float yPosSouls = 5;
            int playerNum = 1;

            List<int> playersScores = null;
            int i = 0;
            if (!m_PlayerManager.RestartGame && m_PlayerManager.Players.Count != 0)
            {
                playersScores = new List<int>();
                for (; i < m_PlayerManager.Players.Count; i++)
                {
                    playersScores.Add(m_PlayerManager.Players[i].ScoreOfTheGame.PlayerScore);
                }

                i = 0;
            }

            m_PlayerManager.RestartGame = false;
            m_PlayerManager.Reset();

            foreach (SpaceshipPlayer spaceshipPlayer in m_SpaceshipPlayers)
            {
                spaceshipPlayer.SetSoulsProperties(yPosSouls);
                spaceshipPlayer.ScoreOfTheGame = new ScoreText(m_Game, m_GameScreen, @"Fonts\CalibriScore", playerNum);
                if (playersScores != null)
                {
                    spaceshipPlayer.ScoreOfTheGame.PlayerScore = playersScores[i++];
                }

                spaceshipPlayer.ScoreOfTheGame.Position = new Vector2(5, yPosSouls);
                if (playerNum == 1)
                {
                    spaceshipPlayer.ScoreOfTheGame.TintColor = Color.Blue;
                }

                if (playerNum == 2)
                {
                    spaceshipPlayer.ScoreOfTheGame.TintColor = Color.Green;
                }

                yPosSouls += spaceshipPlayer.SoulHeight + 5;
                playerNum++;
            }
        }

        private void initSpaceShipPositions()
        {
            float initXPos = this.Game.GraphicsDevice.Viewport.X;
            float initYPos = 0;
            foreach (SpaceshipPlayer spaceshipPlayer in m_SpaceshipPlayers)
            {
                initYPos = this.Game.GraphicsDevice.Viewport.Height - (spaceshipPlayer.Spaceship.Height / 2) - 30;
                spaceshipPlayer.Spaceship.Position = new Vector2(initXPos, initYPos);
                spaceshipPlayer.Spaceship.Position = new Vector2(initXPos, initYPos);
                initXPos += spaceshipPlayer.Spaceship.Width;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            moveSpaceship1();
            if (m_NumOfPlayers > 1)
            {
                moveSpaceship2();
            }

            this.restrictBounds();
        }

        private void moveSpaceship1()
        {
            if (this.m_InputManager.MouseMoved() && this.m_Spaceship1StartedMoving)
            {
                m_SpaceshipPlayers[0].Spaceship.Position = new Vector2(this.m_InputManager.MouseState.X, m_SpaceshipPlayers[0].Spaceship.Position.Y);
            }

            m_Spaceship1StartedMoving = true;

            if (this.m_InputManager.KeyboardState.IsKeyDown(Keys.Left))
            {
                m_SpaceshipPlayers[0].Spaceship.Velocity = new Vector2(k_Velocity * -1, m_SpaceshipPlayers[0].Spaceship.Velocity.Y);
            }
            else if (this.m_InputManager.KeyboardState.IsKeyDown(Keys.Right))
            {
                m_SpaceshipPlayers[0].Spaceship.Velocity = new Vector2(k_Velocity, m_SpaceshipPlayers[0].Spaceship.Velocity.Y);
            }
            else
            {
                m_SpaceshipPlayers[0].Spaceship.Velocity = Vector2.Zero;
            }

            if (this.m_InputManager.KeyPressed(Keys.Space) || this.m_InputManager.MouseLeftButtonPressed())
            {
                m_SpaceshipPlayers[0].Spaceship.Shoot();
            }
        }

        private void moveSpaceship2()
        {
            if (this.m_InputManager.KeyboardState.IsKeyDown(Keys.S))
            {
                m_SpaceshipPlayers[1].Spaceship.Velocity = new Vector2(k_Velocity * -1, m_SpaceshipPlayers[1].Spaceship.Velocity.Y);
            }
            else if (this.m_InputManager.KeyboardState.IsKeyDown(Keys.F))
            {
                m_SpaceshipPlayers[1].Spaceship.Velocity = new Vector2(k_Velocity, m_SpaceshipPlayers[1].Spaceship.Velocity.Y);
            }
            else
            {
                m_SpaceshipPlayers[1].Spaceship.Velocity = Vector2.Zero;
            }

            if (this.m_InputManager.KeyPressed(Keys.E))
            {
                m_SpaceshipPlayers[1].Spaceship.Shoot();
            }
        }

        private void restrictBounds()
        {
            foreach (SpaceshipPlayer spaceshipPlayer in m_SpaceshipPlayers)
            {
                float xBoundsShip = MathHelper.Clamp(spaceshipPlayer.Spaceship.Position.X, this.m_Game.GraphicsDevice.Viewport.Bounds.Left, this.Game.GraphicsDevice.Viewport.Bounds.Right - spaceshipPlayer.Spaceship.Width);
                spaceshipPlayer.Spaceship.Position = new Vector2(xBoundsShip, spaceshipPlayer.Spaceship.Position.Y);
            }
        }
    }
}
