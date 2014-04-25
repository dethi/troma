using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine
{
    public class UpdateableEntityComponent : EntityComponent, IEntityUpdateable
    {
        #region Fields
        private bool _enabled = true;

        public event EventHandler<EventArgs> EnabledChanged;

        #endregion

        #region Public Properties and Accessors

        #region Enabled Getter and Setter
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

        ///// <summary>
        ///// Called at start, and whenever a component is added.
        ///// </summary>
        //public virtual void Initialize() { }

        ///// <summary>
        ///// Called after initialize.  Use to get references to other components in the parent entity.
        ///// </summary>
        //public virtual void Start() { }

        #endregion

        #region Update

        /// <summary>
        /// This component's update logic.
        /// </summary>
        public virtual void Update() { }

        #endregion

        #region Event Handler Methods

        /// <summary>
        /// Called when the enabled value is changed
        /// </summary>
        /// <param name="sender">the component in question</param>
        /// <param name="e">Empty Event Args</param>
        protected virtual void OnEnabledChanged(object sender, EventArgs e) { }

        #endregion
    }
}
