using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public class Entity
    {
        #region Fields

        /// <summary>
        /// Listing of all components in the entity, used for lookups.
        /// </summary>
        public Dictionary<string, IEntityComponent> Components = new Dictionary<string, IEntityComponent>();

        //lists used for storing components, updateable componenets, and drawable components
        private List<IEntityComponent> _components = new List<IEntityComponent>();
        private List<IEntityUpdateable> _updateableComponents = new List<IEntityUpdateable>();
        private List<IEntityDrawable> _drawableComponents = new List<IEntityDrawable>();

        //lists the updating and drawing are performed on
        private List<IEntityComponent> _tempComponents = new List<IEntityComponent>();
        private List<IEntityUpdateable> _tempUpdateableComponents = new List<IEntityUpdateable>();
        private List<IEntityDrawable> _tempDrawableComponents = new List<IEntityDrawable>();

        //whether or not the game has been initialized
        private bool _isInitialized = false;

        /// <summary>
        /// The amount of components this entity contains
        /// </summary>
        public int ComponentCount
        {
            get { return _components.Count; }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor for entity
        /// </summary>
        public Entity()
        {
            _components.Clear();
            _updateableComponents.Clear();
            _drawableComponents.Clear();

            EntityManager.AddEntity(this);
        }

        public Entity(bool dontAddToEntityManager)
        {
            _components.Clear();
            _updateableComponents.Clear();
            _drawableComponents.Clear();

            if (!dontAddToEntityManager)
                EntityManager.AddEntity(this);
        }

        /// <summary>
        /// Initialize method for this entity
        /// </summary>
        public void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }

            _tempComponents.Clear();
            _tempComponents.AddRange(_components);

            for (int i = 0; i < _tempComponents.Count; i++)
            {
                _tempComponents[i].Initialize();
            }

            for (int i = 0; i < _tempComponents.Count; i++)
            {
                _tempComponents[i].Start();
            }

            _isInitialized = true;
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Update all updateable components in the entity
        /// </summary>
        public void Update(GameTime gameTime)
        {
            _tempUpdateableComponents.Clear();
            _tempUpdateableComponents.AddRange(_updateableComponents);

            for (int i = 0; i < _tempUpdateableComponents.Count; i++)
            {
                if (_tempUpdateableComponents[i].Enabled)
                {
                    _tempUpdateableComponents[i].Update(gameTime);
                }
            }
        }

        /// <summary>
        /// Draws all drawable components in the entity
        /// </summary>
        /// <param name="spriteBatch">the spritebatch used for drawing</param>
        public void Draw(GameTime gameTime, ICamera camera)
        {
            _tempDrawableComponents.Clear();
            _tempDrawableComponents.AddRange(_drawableComponents);

            for (int i = 0; i < _tempDrawableComponents.Count; i++)
            {
                if (_tempDrawableComponents[i].Visible)
                {
                    _tempDrawableComponents[i].Draw(gameTime, camera);
                }
            }
        }

        public void DrawHUD(GameTime gameTime)
        {
            for (int i = 0; i < _tempDrawableComponents.Count; i++)
            {
                if (_tempDrawableComponents[i].Visible)
                {
                    _tempDrawableComponents[i].DrawHUD(gameTime);
                }
            }
        }

        #endregion

        #region Adding and Removing Components

        /// <summary>
        /// Adds a component to this entity.  Only allows one component of each type
        /// to be in a single entity.
        /// </summary>
        /// <param name="aComponent">The component to add</param>
        public void AddComponent(EntityComponent aComponent)
        {
            if (aComponent == null)
                throw new ArgumentNullException("component was null");

            if (_components.Contains(aComponent))
                return;

            //add to master and lookup list
            _components.Add(aComponent);
            Components.Add(aComponent.Name, aComponent);

            IEntityUpdateable updateable = aComponent as IEntityUpdateable;
            IEntityDrawable drawable = aComponent as IEntityDrawable;

            //if the component can be updated, add it to that list
            if (updateable != null)
                _updateableComponents.Add(updateable);

            //if the component can be draw, add it to that list
            if (drawable != null)
                _drawableComponents.Add(drawable);

            //if the entity has already initialized, call this item's initialize and start methods
            if (_isInitialized)
            {
                aComponent.Initialize();
                aComponent.Start();
            }
        }

        /// <summary>
        /// Removes a component from the entity
        /// </summary>
        /// <param name="aComponent">The component to remove</param>
        /// <returns>true if a component was removed, false otherwise</returns>
        public bool RemoveComponent(IEntityComponent aComponent)
        {
            if (aComponent == null)
                throw new ArgumentNullException("component was null");

            if (_components.Remove(aComponent))
            {
                IEntityUpdateable updateable = aComponent as IEntityUpdateable;
                IEntityDrawable drawable = aComponent as IEntityDrawable;

                //if the component was updateable, remove it from that list
                if (updateable != null)
                    _updateableComponents.Remove(updateable);

                //if the component was drawable, remove it from that list
                if (drawable != null)
                    _drawableComponents.Remove(drawable);

                return true;
            }

            return false;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks to see if this entity has the component in question
        /// </summary>
        /// <param name="aComponent">The component to check for</param>
        /// <returns>true if the entity has the component, false otherwise</returns>
        public bool HasComponent(IEntityComponent aComponent)
        {
            return this.Components.ContainsKey(aComponent.Name);
        }

        public bool HasComponent(string aComponent)
        {
            return this.Components.ContainsKey(aComponent);
        }

        public bool HasComponent<T>() where T : EntityComponent
        {
            return this.Components.ContainsKey(typeof(T).Name);
        }

        /// <summary>
        /// Gets a component inside the entity based on that component's name property.
        /// </summary>
        /// <param name="aComponentName">The name of the desired component</param>
        /// <returns>the component as an entity component.</returns>
        public IEntityComponent GetComponent(string aComponentName)
        {
            if (this.Components.ContainsKey(aComponentName))
                return Components[aComponentName] as IEntityComponent;
            else
                throw new ArgumentOutOfRangeException(aComponentName);
        }

        public T GetComponent<T>() where T : EntityComponent
        {
            for (int i = 0; i < _components.Count; i++)
            {
                if (_components[i].GetType() == typeof(T))
                    return _components[i] as T;
            }

            return null;
        }

        public void Destroy()
        {
            //remove all the components from this entity
            RemoveAllComponents();
        }

        public void ReplaceComponent(EntityComponent aComponentToAdd, 
            EntityComponent aComponentToRemove)
        {
            RemoveComponent(aComponentToRemove);
            AddComponent(aComponentToAdd);
        }

        public void RemoveFromList(List<Entity> aListToRemoveFrom)
        {
            aListToRemoveFrom.Remove(this);
        }

        #endregion

        #region Helper Methods

        private void RemoveAllComponents()
        {
            for (int i = _components.Count - 1; i >= 0; i--)
                RemoveComponent(_components[i]);

            _components.Clear();
        }

        #endregion
    }
}
