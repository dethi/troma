using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Troma
{
    public struct WeaponInfo
    {
        public int Munition;
        public int MunitionPerLoader;
        public int Loader;

        public bool Automatic;
        public uint ROF; // en ms

        public string Model;

        public Vector3 Position;
        public Vector3 Rotation;

        public Matrix MatrixPosition
        {
            get { return Matrix.CreateTranslation(Position); }
        }

        public Matrix MatrixRotation
        {
            get
            {
                return Matrix.CreateFromYawPitchRoll(
                    Rotation.Y,
                    Rotation.X,
                    Rotation.Z);
            }
        }

        public Vector3 PositionSight;
        public Vector3 RotationSight;

        public Matrix MatrixPositionSight
        {
            get { return Matrix.CreateTranslation(PositionSight); }
        }

        public Matrix MatrixRotationSight
        {
            get
            {
                return Matrix.CreateFromYawPitchRoll(
                    RotationSight.Y,
                    RotationSight.X,
                    RotationSight.Z);
            }
        }

        public string SFXEmpty;
        public string SFXReload;
        public string SFXShoot;
    }
}
