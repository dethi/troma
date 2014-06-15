using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameEngine;

namespace Troma
{
    public static class Constants
    {
        public static WeaponInfo GarandM1 = new WeaponInfo()
        {
            MunitionPerLoader = 8,
            Loader = 10,

            Automatic = false,
            ROF = 500,

            Model = "M1",
            Position = Vector3.Zero,
            Rotation = Vector3.Zero,
            PositionSight = Vector3.Zero,
            RotationSight = Vector3.Zero,

            ChangeUp = new AnimInfo(0, 24),
            Shoot = new AnimInfo(24, 48),
            AimUp = new AnimInfo(48, 60),
            AimShoot = new AnimInfo(60, 84),
            AimDown = new AnimInfo(84, 96),
            Reload = new AnimInfo(96, 228),
            ChangeDown = new AnimInfo(228, 251),

            TimeToReload = 5,
            Weapon_nb_bone = 7,
            Arms_nb_bone = 45,


            SFXEmpty = "GarandM1_empty",
            SFXReload = "GarandM1_reload",
            SFXShoot = "GarandM1_shoot"
        };

        public static WeaponInfo ColtM1911 = new WeaponInfo()
        {
            MunitionPerLoader = 8,
            Loader = 10,

            Automatic = false,
            ROF = 500,

            Model = "M1911",
            Position = Vector3.Zero,
            Rotation = Vector3.Zero,
            PositionSight = Vector3.Zero,
            RotationSight = Vector3.Zero,

            ChangeUp = new AnimInfo(0, 24),
            Shoot = new AnimInfo(24, 48),
            AimUp = new AnimInfo(48, 60),
            AimShoot = new AnimInfo(60, 84),
            AimDown = new AnimInfo(84, 96),
            Reload = new AnimInfo(96, 135),
            ChangeDown = new AnimInfo(135, 158),

            TimeToReload = 2,
            Weapon_nb_bone = 7,
            Arms_nb_bone = 45,

            SFXEmpty = "GarandM1_empty",
            SFXReload = "GarandM1_reload",
            SFXShoot = "GarandM1_shoot"
        };
    }
}
