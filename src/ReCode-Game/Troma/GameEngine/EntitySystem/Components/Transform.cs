using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine.EntitySystem
{
    public class Transform : EntityComponent
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;

        public Matrix TransformMatrix
        {
            get
            {
                return Matrix.CreateScale(Scale) *
                    Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) *
                    Matrix.CreateTranslation(Position);
            }
        }

        public Transform(Entity aParent, Vector3 pos, Vector3 rot, Vector3 scale)
            : base(aParent)
        {
            Name = "Transform";
            Position = pos;
            Rotation = rot;
            Scale = scale;
        }
    }
}
