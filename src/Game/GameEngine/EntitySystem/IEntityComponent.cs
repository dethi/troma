using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine
{
    public interface IEntityComponent
    {
        /// <summary>
        /// The Component's name.  Used when getting this component from another inside the parent entity.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Called after initialize.  Gather references to other components in the entity here.
        /// </summary>
        void Start();
    }
}
