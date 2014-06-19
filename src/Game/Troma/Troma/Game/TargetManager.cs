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

            foreach (Entity e in _masterList)
            {
                e.GetComponent<CollisionBox>().GenerateBoundingBox();
                CollisionManager.AddBox(e.GetComponent<CollisionBox>().BoxList);
            }
        }

        #endregion

        public static bool IsTargetAchieved(BoundingBox box)
        {
            TargetState result = Contains(box);

            if (result.State)
            {
                EntityManager.Remove(result.Entity);
                CollisionManager.Remove(result.Entity.GetComponent<CollisionBox>().BoxList);
                _masterList.Remove(result.Entity);
            }

            if (result.State)
                TimerManager.Add(45, PlayImpactSound);

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

        public static void PlayImpactSound(object o, EventArgs e)
        {
            SFXManager.Play("TargetImpact", 0.25f, 0);
        }
    }
}
