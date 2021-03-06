﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public struct SphereCollision
    {
        public bool IsCollide;
        public List<BoundingBox> CollisionWith;
    }

    public struct RayCollision
    {
        public bool IsCollide;
        public float Distance;
        public BoundingBox CollisionWith;
    }

    /// <summary>
    /// Manage all the BoundingBox in the game
    /// </summary>
    public class CollisionManager
    {
        #region Fields

        private static List<BoundingBox> _masterList = new List<BoundingBox>();
        private static List<BoundingBox> _currentList = new List<BoundingBox>();

#if DEBUG
        private static BasicEffect _effect;

        private static short[] bBoxIndices = 
        {
            0, 1, 1, 2, 2, 3, 3, 0,
            4, 5, 5, 6, 6, 7, 7, 4,
            0, 4, 1, 5, 2, 6, 3, 7
        };
#endif

        public static List<BoundingBox> MasterList
        {
            get { return _masterList; }
        }

        public static int Count
        {
            get { return _masterList.Count; }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the manager
        /// </summary>
        public static void Initialize()
        {
#if DEBUG
            _effect = new BasicEffect(GameServices.GraphicsDevice);
            _effect.World = Matrix.Identity;
            _effect.VertexColorEnabled = true;

            XConsole.AddDebug(Debug);
#endif
        }

        #endregion

        #region Update and Draw

        public static void Update(GameTime gameTime)
        {
            _currentList.Clear();
            _currentList.AddRange(_masterList);
        }

#if DEBUG
        /// <summary>
        /// Draws all BoundingBox in the manager
        /// </summary>
        public static void Draw(GameTime gameTime, ICamera camera)
        {
            if (DebugConfig.DisplayBox)
            {
                _effect.View = camera.View;
                _effect.Projection = camera.Projection;

                foreach (BoundingBox box in _currentList)
                {
                    Vector3[] corners = box.GetCorners();
                    VertexPositionColor[] primitiveList = new VertexPositionColor[corners.Length];

                    // Assign the 8 box vertices
                    for (int i = 0; i < corners.Length; i++)
                    {
                        primitiveList[i] = new VertexPositionColor(corners[i], Color.Red);
                    }

                    // Draw the box with a LineList
                    foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        GameServices.GraphicsDevice.DrawUserIndexedPrimitives(
                            PrimitiveType.LineList, primitiveList, 0, 8,
                            bBoxIndices, 0, 12);
                    }
                }
            }
        }
#endif

        public static string Debug(GameTime gameTime)
        {
            return ("N_Box: " + Count);
        }

        #endregion

        #region Collision Methods

        public static SphereCollision IsCollision(BoundingSphere sphere)
        {
            SphereCollision c = new SphereCollision { CollisionWith = new List<BoundingBox>() };

            foreach (BoundingBox box in _currentList)
            {
                if (box.Intersects(sphere))
                    c.CollisionWith.Add(box);
            }

            c.IsCollide = (c.CollisionWith.Count > 0);
            return c;
        }

        public static RayCollision IsCollision(Ray ray)
        {
            RayCollision c = new RayCollision();
            c.Distance = float.MaxValue;
            float? tmp;

            foreach (BoundingBox box in _currentList)
            {
                tmp = box.Intersects(ray);

                if (tmp.HasValue && tmp.Value < c.Distance)
                {
                    c.Distance = tmp.Value;
                    c.CollisionWith = box;
                }
            }

            c.IsCollide = (c.CollisionWith != null);
            return c;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a BoundingBox to the manager
        /// </summary>
        public static void AddBox(BoundingBox box)
        {
            _masterList.Add(box);
        }

        public static void AddBox(IEnumerable<BoundingBox> box)
        {
            _masterList.AddRange(box);
        }

        public static bool Contains(BoundingBox box)
        {
            return _masterList.Contains(box);
        }

        public static void Remove(BoundingBox box)
        {
            _masterList.Remove(box);
            _currentList.Remove(box);
        }

        public static void Remove(IEnumerable<BoundingBox> box)
        {
            foreach (BoundingBox item in box)
                Remove(item);
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
