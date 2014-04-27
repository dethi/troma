using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine
{
    /// <summary>
    /// Extend from this class when just needing a large container for values
    /// </summary>
    public class EntityComponent : IEntityComponent
    {
        private readonly Entity _entity;
        private string _name;
        protected List<String> _requiredComponents;

        public List<String> RequiredComponents
        {
            get { return _requiredComponents; }
        }

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
            _entity = aParent;
            _requiredComponents = new List<string>();
        }

        public virtual void Initialize() { }

        public virtual void Start()
        {
            foreach (string component in _requiredComponents)
            {
                if (!_entity.HasComponent(component))
                {
                    throw new KeyNotFoundException("This entity don't have the " +
                        component + " component.");
                }
            }
        }
    }
}
