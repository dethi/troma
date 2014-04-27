using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    /// <summary>
    /// Manages all the entities in the game
    /// </summary>
    public static class EntityManager
    {
        #region Fields

        private static List<Entity> _masterList = new List<Entity>();
        private static List<Entity> _currentList = new List<Entity>();

        private static bool _isInitialized = false;

        public static List<Entity> MasterList
        {
            get { return _masterList; }
        }

        public static int Count
        {
            get { return _masterList.Count; }
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Initializes all entities in the manager
        /// </summary>
        public static void Initialize()
        {
            _currentList.Clear();
            _currentList.AddRange(_masterList);

            for (int i = 0; i < _currentList.Count; i++)
            {
                Entity current = _currentList[i] as Entity;

                current.Initialize();
            }

            _isInitialized = true;
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Updates All Entities in the manager
        /// </summary>
        public static void Update(GameTime gameTime)
        {
            _currentList.Clear();
            _currentList.AddRange(_masterList);

            for (int i = 0; i < _currentList.Count; i++)
            {
                _currentList[i].Update(gameTime);
            }
        }

        /// <summary>
        /// Draws all entities in the manager
        /// </summary>
        public static void Draw(GameTime gameTime, ICamera camera)
        {
            for (int i = 0; i < _currentList.Count; i++)
            {
                _currentList[i].Draw(gameTime, camera);
            }
        }

        public static void DrawHUD(GameTime gameTime)
        {
            for (int i = 0; i < _currentList.Count; i++)
            {
                _currentList[i].DrawHUD(gameTime);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a New Entity.
        /// </summary>
        public static Entity CreateNew()
        {
            return new Entity();
        }

        public static Entity CreateNew(params List<Entity>[] ListsToAddTo)
        {
            Entity tempEntity = new Entity();

            for (int i = 0; i < ListsToAddTo.Length; i++)
            {
                ListsToAddTo[i].Add(tempEntity);
            }

            return tempEntity;
        }

        /// <summary>
        /// Adds an entity to the manager
        /// </summary>
        public static void AddEntity(Entity aEntity)
        {
            //add entity to the master list
            if (!_masterList.Contains(aEntity))
                _masterList.Add(aEntity);

            if (_isInitialized)
                aEntity.Initialize();
        }

        public static bool Contains(Entity aEntity)
        {
            return _masterList.Contains(aEntity);
        }

        public static void Remove(Entity aEntity)
        {
            _masterList.Remove(aEntity);
            _currentList.Remove(aEntity);
        }

        /// <summary>
        /// Clear all the lists in the manager.
        /// </summary>
        public static void Clear()
        {
            _masterList.Clear();
            _currentList.Clear();
        }

        #endregion
    }
}
