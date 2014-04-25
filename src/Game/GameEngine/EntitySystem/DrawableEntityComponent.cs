using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public class DrawableEntityComponent : UpdateableEntityComponent, IEntityDrawable
    {
        #region Fields

        private bool _visible = true;

        public event EventHandler<EventArgs> VisibleChanged;

        public Vector3 DrawPosition = Vector3.Zero;

        /// <summary>
        /// Determines whether this component should be drawn or not.
        /// </summary>
        public bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                if (_visible != value)
                {
                    _visible = value;

                    if (VisibleChanged != null)
                        VisibleChanged(this, EventArgs.Empty);
                }
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor for the drawable component.
        /// </summary>
        /// <param name="aParent">the parent entity.  Chains to base constructor</param>
        public DrawableEntityComponent(Entity aParent)
            : base(aParent)
        {
            VisibleChanged += OnVisibleChanged;
        }

        /// <summary>
        /// Initialize values here, calls LoadContent at the end.
        /// </summary>
        public override void Initialize()
        {
            LoadContent();
        }

        /// <summary>
        /// Gather references to other components here, called after Initialize.
        /// </summary>
        public override void Start()
        {
        }

        /// <summary>
        /// Load the content for this entity, use the Services content manager.
        /// </summary>
        protected virtual void LoadContent() { }

        /// <summary>
        /// Unload this entity's content.
        /// </summary>
        public virtual void UnloadContent() { }

        #endregion

        #region Update and Draw

        public override void Update()
        {
        }

        /// <summary>
        /// Used to draw the component.
        /// </summary>
        /// <param name="spriteBatch">Spritebatch to be used in drawing (from EntityManager)</param>
        /// <param name="gameTime">drawing timing values</param>
        public virtual void Draw(SpriteBatch spriteBatch) { }

        public virtual void DrawHUD(SpriteBatch spriteBatch) { }

        #endregion

        #region  Event Handler methods

        /// <summary>
        /// Called when the visible value is changed.
        /// </summary>
        /// <param name="sender">the component in question.</param>
        /// <param name="e">Empty Event Args</param>
        protected virtual void OnVisibleChanged(object sender, EventArgs e) { }

        #endregion
    }
}
