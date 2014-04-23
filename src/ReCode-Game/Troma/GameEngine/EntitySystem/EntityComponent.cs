using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine.EntitySystem
{
    /// <summary>
    /// Extend from this class when just needing a large container for values
    /// </summary>
    public class EntityComponent : IEntityComponent
    {
        private readonly Entity _entity;
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Entity Entity
        {
            get { return _entity; }
        }

        public EntityComponent(Entity aParent)
        {
            this._entity = aParent;
        }

        public virtual void Initialize() { }

        public virtual void Start() { }
    }
}
