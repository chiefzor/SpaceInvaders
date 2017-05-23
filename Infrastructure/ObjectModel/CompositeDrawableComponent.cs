using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Invaders.Infrastructure;

namespace Infrastructure.ObjectModel
{
    public abstract class CompositeDrawableComponent<ComponentType> :
        DrawableGameComponent, ICollection<ComponentType>
        where ComponentType : IGameComponent
    {
        // the entire collection, for general collection methods (count, foreach, etc.):
        private Collection<ComponentType> m_Components = new Collection<ComponentType>();

        // selective holders for specific operations each frame:
        private List<ComponentType> m_UninitializedComponents = new List<ComponentType>();
        protected List<IUpdateable> m_UpdateableComponents = new List<IUpdateable>();
        protected List<IDrawable> m_DrawableComponents = new List<IDrawable>();
        protected List<Component2D> m_Component2Ds = new List<Component2D>();

        public List<Component2D> Component2Ds
        {
            get { return m_Component2Ds; }
        }

        public event EventHandler<GameComponentEventArgs<ComponentType>> ComponentAdded;

        public event EventHandler<GameComponentEventArgs<ComponentType>> ComponentRemoved;

        protected virtual void OnComponentAdded(GameComponentEventArgs<ComponentType> e)
        {
            if (m_IsInitialized)
            {
                InitializeComponent(e.GameComponent);
            }
            else
            {
                m_UninitializedComponents.Add(e.GameComponent);
            }

            IUpdateable updatable = e.GameComponent as IUpdateable;
            if (updatable != null)
            {
                insertSorted(updatable);
                updatable.UpdateOrderChanged += new EventHandler<EventArgs>(childUpdateOrderChanged);
            }

            IDrawable drawable = e.GameComponent as IDrawable;
            if (drawable != null)
            {
                insertSorted(drawable);
                drawable.DrawOrderChanged += new EventHandler<EventArgs>(childDrawOrderChanged);
            }

            if (ComponentAdded != null)
            {
                ComponentAdded(this, e);
            }
        }

        protected virtual void OnComponentRemoved(GameComponentEventArgs<ComponentType> e)
        {
            if (!m_IsInitialized)
            {
                m_UninitializedComponents.Remove(e.GameComponent);
            }

            IUpdateable updatable = e.GameComponent as IUpdateable;
            if (updatable != null)
            {
                m_UpdateableComponents.Remove(updatable);
                updatable.UpdateOrderChanged -= childUpdateOrderChanged;
            }

            Sprite sprite = e.GameComponent as Sprite;
            if (sprite != null)
            {
                m_Component2Ds.Remove(sprite);
                sprite.DrawOrderChanged -= childDrawOrderChanged;
            }
            else
            {
                IDrawable drawable = e.GameComponent as IDrawable;
                if (drawable != null)
                {
                    m_DrawableComponents.Remove(drawable);
                    drawable.DrawOrderChanged -= childDrawOrderChanged;
                }
            }

            if (ComponentRemoved != null)
            {
                ComponentRemoved(this, e);
            }
        }

        private void childUpdateOrderChanged(object sender, EventArgs e)
        {
            IUpdateable updatable = sender as IUpdateable;
            m_UpdateableComponents.Remove(updatable);

            insertSorted(updatable);
        }

        private void childDrawOrderChanged(object sender, EventArgs e)
        {
            IDrawable drawable = sender as IDrawable;

            Sprite sprite = sender as Sprite;
            if (sprite != null)
            {
                m_Component2Ds.Remove(sprite);
            }
            else
            {
                m_DrawableComponents.Remove(drawable);
            }

            insertSorted(drawable);
        }

        public CompositeDrawableComponent(BaseGame i_Game)
            : base(i_Game)
        {
        }

        private void insertSorted(IUpdateable i_Updatable)
        {
            int idx = m_UpdateableComponents.BinarySearch(i_Updatable, UpdateableComparer.Default);
            if (idx < 0)
            {
                idx = ~idx;
            }

            m_UpdateableComponents.Insert(idx, i_Updatable);
        }

        private void insertSorted(IDrawable i_Drawable)
        {
            Component2D sprite = i_Drawable as Component2D;
            if (sprite != null)
            {
                int idx = m_Component2Ds.BinarySearch(sprite, DrawableComparer<Component2D>.Default);
                if (idx < 0)
                {
                    idx = ~idx;
                }

                m_Component2Ds.Insert(idx, sprite);
            }
            else
            {
                int idx = m_DrawableComponents.BinarySearch(i_Drawable, DrawableComparer<IDrawable>.Default);
                if (idx < 0)
                {
                    idx = ~idx;
                }

                m_DrawableComponents.Insert(idx, i_Drawable);
            }
        }

        private bool m_IsInitialized;

        public override void Initialize()
        {
            if (!m_IsInitialized)
            {
                while (m_UninitializedComponents.Count > 0)
                {
                    InitializeComponent(m_UninitializedComponents[0]);
                }

                base.Initialize();

                m_IsInitialized = true;
            }
        }

        private void InitializeComponent(ComponentType i_Component)
        {
            if (i_Component is Component2D)
            {
                (i_Component as Component2D).SpriteBatch = m_SpriteBatch;
            }

            i_Component.Initialize();
            m_UninitializedComponents.Remove(i_Component);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            m_SpriteBatch = new SpriteBatch(this.GraphicsDevice);

            foreach (Component2D sprite in m_Component2Ds)
            {
                sprite.SpriteBatch = m_SpriteBatch;
            }
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < m_UpdateableComponents.Count; i++)
            {
                IUpdateable updatable = m_UpdateableComponents[i];
                if (updatable.Enabled)
                {
                    updatable.Update(gameTime);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (IDrawable drawable in m_DrawableComponents)
            {
                if (drawable.Visible)
                {
                    drawable.Draw(gameTime);
                }
            }

            m_SpriteBatch.Begin(this.SpritesSortMode, this.BlendState, this.SamplerState, this.DepthStencilState, this.RasterizerState, this.Shader, this.TransformMatrix);

            foreach (Component2D sprite in m_Component2Ds)
            {
                if (sprite.Visible)
                {
                    sprite.Draw(gameTime);
                }
            }

            m_SpriteBatch.End();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                for (int i = 0; i < Count; i++)
                {
                    IDisposable disposable = m_Components[i] as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
            }

            base.Dispose(disposing);
        }

        public virtual void Add(ComponentType i_Component)
        {
            this.InsertItem(m_Components.Count, i_Component);
        }

        protected virtual void InsertItem(int i_Idx, ComponentType i_Component)
        {
            if (m_Components.IndexOf(i_Component) != -1)
            {
                throw new ArgumentException("Duplicate components are not allowed in the same GameComponentManager.");
            }

            if (i_Component != null)
            {
                m_Components.Insert(i_Idx, i_Component);

                OnComponentAdded(new GameComponentEventArgs<ComponentType>(i_Component));
            }
        }

        public void Clear()
        {
            for (int i = 0; i < Count; i++)
            {
                OnComponentRemoved(new GameComponentEventArgs<ComponentType>(m_Components[i]));
            }

            m_Components.Clear();
        }

        public bool Contains(ComponentType i_Component)
        {
            return m_Components.Contains(i_Component);
        }

        public void CopyTo(ComponentType[] io_ComponentsArray, int i_ArrayIndex)
        {
            m_Components.CopyTo(io_ComponentsArray, i_ArrayIndex);
        }

        public int Count
        {
            get { return m_Components.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public virtual bool Remove(ComponentType i_Component)
        {
            bool removed = m_Components.Remove(i_Component);

            if (i_Component != null && removed)
            {
                OnComponentRemoved(new GameComponentEventArgs<ComponentType>(i_Component));
            }

            return removed;
        }

        public IEnumerator<ComponentType> GetEnumerator()
        {
            return m_Components.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)m_Components).GetEnumerator();
        }

        protected SpriteBatch m_SpriteBatch;

        public SpriteBatch SpriteBatch
        {
            get { return m_SpriteBatch; }

            set { m_SpriteBatch = value; }
        }

        protected BlendState m_BlendState = BlendState.AlphaBlend;

        public BlendState BlendState
        {
            get { return m_BlendState; }

            set { m_BlendState = value; }
        }

        protected SpriteSortMode m_SpritesSortMode = SpriteSortMode.Deferred;

        public SpriteSortMode SpritesSortMode
        {
            get { return m_SpritesSortMode; }
            set { m_SpritesSortMode = value; }
        }

        protected SamplerState m_SamplerState = null;

        public SamplerState SamplerState
        {
            get { return m_SamplerState; }
            set { m_SamplerState = value; }
        }

        protected DepthStencilState m_DepthStencilState = null;

        public DepthStencilState DepthStencilState
        {
            get { return m_DepthStencilState; }
            set { m_DepthStencilState = value; }
        }

        protected RasterizerState m_RasterizerState = null;

        public RasterizerState RasterizerState
        {
            get { return m_RasterizerState; }
            set { m_RasterizerState = value; }
        }

        protected Effect m_Shader = null;

        public Effect Shader
        {
            get { return m_Shader; }
            set { m_Shader = value; }
        }

        protected Matrix m_TransformMatrix = Matrix.Identity;

        public Matrix TransformMatrix
        {
            get { return m_TransformMatrix; }
            set { m_TransformMatrix = value; }
        }

        protected Vector2 CenterOfViewPort
        {
            get
            {
                return new Vector2((float)Game.GraphicsDevice.Viewport.Width / 2, (float)Game.GraphicsDevice.Viewport.Height / 2);
            }
        }

        public ContentManager ContentManager
        {
            get { return this.Game.Content; }
        }

        public Collection<ComponentType> Components
        {
            get
            {
                return m_Components;
            }

            set
            {
                m_Components = value;
            }
        }
    }
}