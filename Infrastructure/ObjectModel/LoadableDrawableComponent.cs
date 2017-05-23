using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Invaders.Infrastructure
{
    public abstract class LoadableDrawableComponent : DrawableGameComponent
    {
        public event EventHandler<EventArgs> Disposed;

        protected virtual void OnDisposed(object sender, EventArgs args)
        {
            if (Disposed != null)
            {
                Disposed.Invoke(sender, args);
            }
        }

        protected override void Dispose(bool i_disposing)
        {
            base.Dispose(i_disposing);
            OnDisposed(this, EventArgs.Empty);
        }

        public event EventHandler<EventArgs> PositionChanged;

        protected virtual void OnPositionChanged()
        {
            if (PositionChanged != null)
            {
                PositionChanged(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> SizeChanged;

        protected virtual void OnSizeChanged()
        {
            if (SizeChanged != null)
            {
                SizeChanged(this, EventArgs.Empty);
            }
        }

        protected string m_AssetName;

        protected BaseGame m_BaseGame;

        protected ContentManager ContentManager
        {
            get { return this.Game.Content; }
        }

        public string AssetName
        {
            get { return m_AssetName; }
            set { m_AssetName = value; }
        }

        public LoadableDrawableComponent(
            string i_AssetName, BaseGame i_Game, int i_UpdateOrder, int i_DrawOrder)
            : base(i_Game)
        {
            m_BaseGame = i_Game;
            this.AssetName = i_AssetName;
            this.UpdateOrder = i_UpdateOrder;
            this.DrawOrder = i_DrawOrder;
        }

        public LoadableDrawableComponent(
            string i_AssetName,
            BaseGame i_Game,
            int i_CallsOrder)
            : this(i_AssetName, i_Game, i_CallsOrder, i_CallsOrder)
        {
        }

        public LoadableDrawableComponent(BaseGame i_Game)
            : base(i_Game)
        {
            this.Game.Components.Add(this);
        }

        public override void Initialize()
        {
            base.Initialize();

            if (this is ICollidable)
            {
                ICollisionsManager collisionMgr =
                    this.Game.Services.GetService(typeof(ICollisionsManager))
                        as ICollisionsManager;

                if (collisionMgr != null)
                {
                    collisionMgr.AddObjectToMonitor(this as ICollidable);
                }
            }

            InitBounds();
        }

        protected abstract void InitBounds();
    }
}