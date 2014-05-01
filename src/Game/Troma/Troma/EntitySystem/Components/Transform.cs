using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameEngine;

namespace Troma
{
    public class Transform : EntityComponent
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public float Scale;

        public Matrix World
        {
            get
            {
                return Matrix.CreateScale(Scale) *
                    Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) *
                    Matrix.CreateTranslation(Position);
            }
        }

        /// <summary>
        /// (X,Z) position
        /// </summary>
        public Vector2 Pos2D
        {
            get { return new Vector2(Position.X, Position.Z); }
        }

        public Transform(Entity aParent)
            : this(aParent, Vector3.Zero, Vector3.Zero, 1)
        { }

        public Transform(Entity aParent, Vector3 pos)
            : this(aParent, pos, Vector3.Zero, 1)
        { }

        public Transform(Entity aParent, Vector3 pos, Vector3 rot)
            : this(aParent, pos, rot, 1)
        { }

        public Transform(Entity aParent, Vector3 pos, Vector3 rot, float scale)
            : base(aParent)
        {
            Name = "Transform";
            Position = pos;
            Rotation = rot;
            Scale = scale;
        }
    }
}
