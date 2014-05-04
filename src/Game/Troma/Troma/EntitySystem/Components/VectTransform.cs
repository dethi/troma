using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameEngine;

namespace Troma
{
    public class VectTransform : EntityComponent
    {
        public Vector3[] Position;
        public Vector3[] Rotation;
        public float[] Scale;

        public int Length { get; private set; }

        public Matrix GetWorld(int i)
        {
            return Matrix.CreateScale(Scale[i]) *
                Matrix.CreateFromYawPitchRoll(Rotation[i].Y, Rotation[i].X, Rotation[i].Z) *
                Matrix.CreateTranslation(Position[i]);
        }

        /// <summary>
        /// (X,Z) position
        /// </summary>
        public Vector2 GetPos2D(int i)
        {
            return new Vector2(Position[i].X, Position[i].Z);
        }

        public VectTransform(Entity aParent, Vector3[] pos)
            : base(aParent)
        {
            Vector3[] rot = new Vector3[pos.Length];
            float[] scale = new float[pos.Length];

            for (int i = 0; i < pos.Length; i++)
            {
                rot[i] = Vector3.Zero;
                scale[i] = 1;
            }

            Name = "VectTransform";
            SetValue(pos, rot, scale);
        }

        public VectTransform(Entity aParent, Vector3[] pos, Vector3[] rot, float[] scale)
            : base(aParent)
        {
            Name = "VectTransform";
            SetValue(pos, rot, scale);
        }

        private void SetValue(Vector3[] pos, Vector3[] rot, float[] scale)
        {
            if (pos.Length != rot.Length || rot.Length != scale.Length)
                throw new ArgumentException("Lenght vector's is not egal");

            Length = pos.Length;

            Position = pos;
            Rotation = rot;
            Scale = scale;
        }
    }
}
