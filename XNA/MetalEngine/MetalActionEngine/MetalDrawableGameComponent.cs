using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace MetalActionEngine
{
    /// <summary>
    /// Pseudo Drawable Game Component that controls and abstracts a real DrawableGameComponent.
    /// </summary>
    public class MetalDrawableGameComponent : IGameComponent, IUpdateable, IDrawable, IDisposable
    {
        bool hasParent;


        protected Vector2 position;
        protected Vector2 size;


        #region Properties
        internal MetalDrawableGameComponent Parent { get; private set; }

        internal List<MetalDrawableGameComponent> Children { get; private set; }


        /// <summary>
        /// Instance of the Game (MetalGame) where the Game Component is contained.
        /// </summary>
        internal MetalGame Game { get; private set; }

        /// <summary>
        /// Shortcut to the SpriteBatch of the Game.
        /// </summary>
        protected SpriteBatch SpriteBatch
        {
            get { return Game.SpriteBatch; }
        }


        internal virtual float X
        {
            get { return position.X; }
            set { position.X = value; }
        }
        internal virtual float Y
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        public virtual float Width
        {
            get { return size.X; }
            set { size.X = value; }
        }
        public virtual float Height
        {
            get { return size.Y; }
            set { size.Y = value; }
        }
        #endregion Properties


        #region Interfaces properties implementation
        private bool _enabled = true;
        public bool Enabled
        {
            get { return _enabled; }
            internal set
            {
                if ( _enabled != value )
                {
                    _enabled = value;

                    if ( EnabledChanged != null )
                        EnabledChanged(this, null);
                }
            }
        }

        private bool _visible = true;
        public bool Visible
        {
            get { return _visible; }
            internal set
            {
                if ( _visible != value )
                {
                    _visible = value;

                    if ( VisibleChanged != null )
                        VisibleChanged(this, null);
                }
            }
        }

        private int _updateOrder = 0;
        public int UpdateOrder
        {
            get { return _updateOrder; }
            internal set
            {
                if ( _updateOrder != value )
                {
                    _updateOrder = value;

                    if ( UpdateOrderChanged != null )
                        UpdateOrderChanged(this, null);
                }
            }
        }

        private int _drawOrder = 0;
        public int DrawOrder
        {
            get { return _drawOrder; }
            internal set
            {
                if ( _drawOrder != value )
                {
                    _drawOrder = value;

                    if ( DrawOrderChanged != null )
                        DrawOrderChanged(this, null);
                }
            }
        }
        #endregion Interfaces properties implementation


        #region Interfaces event handlers implementations
        public event EventHandler<EventArgs> EnabledChanged;

        public event EventHandler<EventArgs> VisibleChanged;

        public event EventHandler<EventArgs> UpdateOrderChanged;

        public event EventHandler<EventArgs> DrawOrderChanged;
        #endregion Interfaces event handlers implementations


        #region Constructor
        public MetalDrawableGameComponent()
        {
            Children = new List<MetalDrawableGameComponent>();

            position = Vector2.Zero;
        }
        #endregion Constructor


        /// <summary>
        /// Sets a Game instance that this component is contained in.
        /// </summary>
        /// <param name="game">Instance of the game to add this component.</param>
        internal virtual void SetParent(MetalGame game)
        {
            this.Game = game;

            // Set the default width and height of this component, that is the size of the screen.
            if ( Width <= 0 ) Width = game.GraphicsDevice.Viewport.Width;
            if ( Height <= 0 ) Height = game.GraphicsDevice.Viewport.Height;

            // Add the XNA componet to the game.
            Game.Components.Add(this);
        }

        /// <summary>
        /// Sets the component that is the parent of this component.
        /// </summary>
        /// <param name="parent">Instance of the component to be the parent of this.</param>
        internal virtual void SetParent(MetalDrawableGameComponent parent)
        {
            // Sets the information that says that this component has a parent.
            hasParent = true;

            // Stores the instance of the parent in a propoerty.
            Parent = parent;

            // Set other default width and height of this component, that is same size of the parent.
            Width = parent.Width;
            Height = parent.Height;

            // Add this instance to the parent children list.
            parent.Children.Add(this);

            // Sets the game of this component, that is the same as the game of the parent.
            SetParent(parent.Game);
        }


        #region Centralization methods
        /// <summary>
        /// Positions the component in the horizontal center of the parent.
        /// </summary>
        internal void CenterHorizontally()
        {
            // If this component has a parent, center in the parent.
            if ( hasParent )
                X = Parent.X + (Parent.Width / 2) - (Width / 2);
            // If this component hasn't a parent, center in the screen (game).
            else
                X = (Game.GraphicsDevice.Viewport.Width / 2) - (Width / 2);
        }

        /// <summary>
        /// Positions the component in the vertical center of an area.
        /// </summary>
        /// <param name="top">The top point of the area.</param>
        /// <param name="bottom">The bottom point of the area.</param>
        internal void CenterVertically(float top, float bottom)
        {
            Y = ((top + bottom) / 2) - (Height / 2);
        }

        /// <summary>
        /// Positions the component in the vertical center of the parent.
        /// </summary>
        internal void CenterVertically()
        {
            // If this component has a parent, center in the parent.
            if ( hasParent )
                CenterVertically(Parent.position.Y, Parent.position.Y + Parent.Height);
            // If this component hasn't a parent, center in the screen (game).
            else
                CenterVertically(0, Game.GraphicsDevice.Viewport.Height);
        }
        #endregion Centralization methods


        #region Interface methods implementations
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public virtual void Update(GameTime gameTime) { }

        /// <summary>
        /// Allows the game component to draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public virtual void Draw(GameTime gameTime) { }

        /// <summary>
        /// Dispose this component instance.
        /// </summary>
        public void Dispose()
        {
            // Disposes all the child components of this instance too.
            for ( var i = Children.Count - 1; i >= 0; i-- )
            {
                Children[i].Dispose();
                Children[i] = null;
                Children.RemoveAt(i);
            }
        }
        #endregion Interface methods implementations
    }
}
