using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public enum Weapons
    {
        M1,
        M1911
    }

    public struct WeaponInfo
    {
        public string Name;
        public Weapons Type;

        public int Munition;
        public int MunitionPerLoader;
        public int Loader;

        public bool Automatic;
        public uint ROF; // en ms

        public string Model;

        public Vector3 Position;
        public Vector3 Rotation;

        public AnimInfo ChangeUp;
        public AnimInfo ChangeDown;
        public AnimInfo Shoot;
        public AnimInfo AimShoot;
        public AnimInfo AimUp;
        public AnimInfo AimDown;
        public AnimInfo Reload;

        public uint TimeToReload;
        public uint StartReloadSFX;

        public int Weapon_nb_bone;
        public int Arms_nb_bone;

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

        public string SFXEmpty;
        public string SFXReload;
        public string SFXShoot;

        public Vector2 MuzzleOffset;
    }
}
