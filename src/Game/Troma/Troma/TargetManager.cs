using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameEngine;

namespace Troma
{
    public struct TargetState
    {
        public bool State;
        public Entity Entity;
    }

    public class TargetManager
    {
        #region Fields

        private static List<Entity> _masterList = new List<Entity>();

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
        /// Initializes the manager
        /// </summary>
        public static void Initialize()
        {
            _masterList.AddRange(EntityManager.EntitiesWith<Target>());
        }

        #endregion

        public static bool IsTargetAchieved(BoundingBox box)
        {
            TargetState result = Contains(box);

            if (result.State)
            {
                EntityManager.Remove(result.Entity);
                _masterList.Remove(result.Entity);
            }

            return result.State;
        }

        private static TargetState Contains(BoundingBox box)
        {
            foreach (Entity entity in _masterList)
            {
                if (entity.GetComponent<CollisionBox>().BoxList.Contains(box))
                    return new TargetState { State = true, Entity = entity };
            }

            return new TargetState { State = false };
        }

        /// <summary>
        /// Clear all the lists in the manager.
        /// </summary>
        public static void Clear()
        {
            _masterList.Clear();
        }
    }
}
