using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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

            Model = "Weapon/M1",
            Position = new Vector3(-0.7f, -0.3f, 0),
            Rotation = new Vector3(0, 0, 0),
            PositionSight = new Vector3(0, 0, -1.3f),
            RotationSight = Vector3.Zero,

            SFXEmpty = "GarandM1_empty",
            SFXReload = "GarandM1_reload",
            SFXShoot = "GarandM1_shoot"
        };
    }
}
