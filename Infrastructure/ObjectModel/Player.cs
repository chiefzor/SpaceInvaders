using System;
using System.Collections.Generic;
using Invaders.Infrastructure;
using Invaders.ObjectModel;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel
{
    public class Player
    {
        protected BaseGame m_Game;
        private Vector2 m_SoulScale = Vector2.Zero;
        private Sprite m_PlayerObject;
        private List<Soul> m_Souls;
        private Vector2 m_DirectionToPlaceEnemies;
        private string m_SoulAssetName;
        private int m_InitNumSouls;
        private float m_XPosSouls;
        private float m_YPosSouls;
        private float m_SoulHeight;
        private float m_SoulWidth;
        private float m_XSpacingBetweenSouls = 0;
        private float m_YSpacingBetweenSouls = 0;
        private ScoreText m_ScoreOfTheGame;

        public event EventHandler PlayerDestroyed;

        public Sprite PlayerObject
        {
            get { return m_PlayerObject; }

            set { m_PlayerObject = value; }
        }

        public Vector2 ScaleSoul
        {
            get { return m_SoulScale; }
            set
            {
                foreach (Soul soul in m_Souls)
                {
                    soul.Scales = value;
                }
            }
        }

        public float XPosSouls
        {
            get { return m_XPosSouls; }
            set { m_XPosSouls = value; }
        }

        public float YPosSouls
        {
            get { return m_YPosSouls; }
            set
            {
                m_YPosSouls = value;
            }
        }

        public ScoreText ScoreOfTheGame
        {
            get { return m_ScoreOfTheGame; }
            set { m_ScoreOfTheGame = value; }
        }

        public float SoulHeight
        {
            get
            {
                if (m_Souls.Count != 0)
                {
                    m_SoulHeight = m_Souls[0].Height;
                }

                return m_SoulHeight;
            }

            set { m_SoulHeight = value; }
        }

        public float SoulWidth
        {
            get
            {
                if (m_Souls.Count != 0)
                {
                    m_SoulWidth = m_Souls[0].Width;
                }

                return m_SoulWidth;
            }

            set
            {
                m_SoulWidth = value;
            }
        }

        public List<Soul> Souls
        {
            get { return m_Souls; }
            set { m_Souls = value; }
        }

        public float XSpacingBetweenSouls
        {
            get { return m_XSpacingBetweenSouls; }
            set { m_XSpacingBetweenSouls = value; }
        }

        public float YSpacingBetweenSouls
        {
            get { return m_YSpacingBetweenSouls; }
            set { m_YSpacingBetweenSouls = value; }
        }

        public Vector2 DirectionToPlaceEnemies
        {
            get { return m_DirectionToPlaceEnemies; }
            set { m_DirectionToPlaceEnemies = value; }
        }

        public Player(BaseGame i_Game, string i_SoulAssetName, int i_NumOfSouls, Sprite i_PlayerObject) 
        {
            m_Game = i_Game;
            m_PlayerObject = i_PlayerObject;
            m_SoulAssetName = i_SoulAssetName;
            m_InitNumSouls = i_NumOfSouls;
            Souls = new List<Soul>();

            for (int i = 0; i < m_InitNumSouls; i++)
            {
                Soul soul = new Soul(m_Game, i_PlayerObject.BaseScreen, m_SoulAssetName);
                Souls.Add(soul);
            }
        }

        public virtual void LoseASoul()
        {
            if (Souls.Count != 0)
            {
                Souls[Souls.Count - 1].Visible = false;
                Souls[Souls.Count - 1].Dispose();
                Souls.RemoveAt(Souls.Count - 1);
                if (Souls.Count == 0)
                {
                    onPlayerDestroyed();
                }
            }
        }

        private void onPlayerDestroyed()
        {
            if (PlayerDestroyed != null)
            {
                PlayerDestroyed.Invoke(this, EventArgs.Empty);
            }
        }

        public void InitSouls()
        {
            foreach (Soul soul in m_Souls)
            {
                m_XPosSouls += soul.Width * m_DirectionToPlaceEnemies.X;
                m_YPosSouls += soul.Height * m_DirectionToPlaceEnemies.Y;
                soul.Position = new Vector2(m_XPosSouls, m_YPosSouls);
                m_XPosSouls += XSpacingBetweenSouls;
                m_YPosSouls += YSpacingBetweenSouls;
            }
        }
    }
}
