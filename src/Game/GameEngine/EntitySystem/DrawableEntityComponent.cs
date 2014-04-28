using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public class DrawableEntityComponent : EntityComponent, IEntityDrawable
    {
        #region Fields

        public event EventHandler<EventArgs> VisibleChanged;
        private bool _visible = true;

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
        /// <param name="aParent">The parent entity</param>
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
        /// Load the content for this entity.
        /// </summary>
        protected virtual void LoadContent() { }

        #endregion

        /// <summary>
        /// Draw the component.
        /// </summary>
        public virtual void Draw(GameTime gameTime, ICamera camera) { }

        /// <summary>
        /// Draw HUD
        /// </summary>
        public virtual void DrawHUD(GameTime gameTime) { }

        /// <summary>
        /// Called when the visible value is changed.
        /// </summary>
        protected virtual void OnVisibleChanged(object sender, EventArgs e) { }
    }
}
