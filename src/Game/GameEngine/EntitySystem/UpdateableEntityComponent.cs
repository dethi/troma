using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine
{
    public class UpdateableEntityComponent : EntityComponent, IEntityUpdateable
    {
        #region Fields

        private bool _enabled = true;
        public event EventHandler<EventArgs> EnabledChanged;

        /// <summary>
        /// Property for getting and setting whether this component
        /// should be updated.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;

                    if (EnabledChanged != null)
                        EnabledChanged(this, EventArgs.Empty);
                }
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor for Entity Components
        /// </summary>
        /// <param name="aParent">The entity this component is put in to.</param>
        public UpdateableEntityComponent(Entity aParent)
            : base(aParent)
        {
            EnabledChanged += OnEnabledChanged;
        }

        #endregion

        /// <summary>
        /// This component's update logic.
        /// </summary>
        public virtual void Update(GameTime gameTime) { }

        /// <summary>
        /// Called when the enabled value is changed
        /// </summary>
        protected virtual void OnEnabledChanged(object sender, EventArgs e) { }
    }
}
