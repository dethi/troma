using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine
{
    public interface IEntityComponent
    {
        /// <summary>
        /// The Component's name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// List of the required components.
        /// </summary>
        List<String> RequiredComponents { get; }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Called after initialize. Gather references to other components in the entity here.
        /// </summary>
        void Start();
    }
}
