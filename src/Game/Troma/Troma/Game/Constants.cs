using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameEngine;
using ClientServerExtension;

namespace Troma
{
    public static class Constants
    {
        public static WeaponInfo GarandM1 = new WeaponInfo()
        {
            Name = "M1 Garand",
            Type = Weapons.M1,

            MunitionPerLoader = 8,
            Loader = 10,

            Automatic = false,
            ROF = 500,

            Model = "M1",
            Position = Vector3.Zero,
            Rotation = Vector3.Zero,

            ChangeUp = new AnimInfo(0, 24),
            Shoot = new AnimInfo(24, 48),
            AimUp = new AnimInfo(48, 60),
            AimShoot = new AnimInfo(60, 84),
            AimDown = new AnimInfo(84, 96),
            Reload = new AnimInfo(96, 228),
            ChangeDown = new AnimInfo(228, 251),

            TimeToReload = 1700,
            StartReloadSFX = 1200,

            Weapon_nb_bone = 7,
            Arms_nb_bone = 45,

            SFXEmpty = "GarandM1_empty",
            SFXReload = "GarandM1_reload",
            SFXShoot = "GarandM1_shoot",

            MuzzleOffset = new Vector2(75, 50)
        };

        public static WeaponInfo ColtM1911 = new WeaponInfo()
        {
            Name = "Colt M1911",
            Type = Weapons.M1911,

            MunitionPerLoader = 9,
            Loader = 10,

            Automatic = false,
            ROF = 300,

            Model = "M1911",
            Position = Vector3.Zero,
            Rotation = Vector3.Zero,

            ChangeUp = new AnimInfo(0, 24),
            Shoot = new AnimInfo(24, 48),
            AimUp = new AnimInfo(48, 60),
            AimShoot = new AnimInfo(60, 84),
            AimDown = new AnimInfo(84, 96),
            Reload = new AnimInfo(96, 135),
            ChangeDown = new AnimInfo(135, 158),

            TimeToReload = 800,
            StartReloadSFX = 300,

            Weapon_nb_bone = 7,
            Arms_nb_bone = 45,

            SFXEmpty = "M1911_empty",
            SFXReload = "M1911_reload",
            SFXShoot = "M1911_shoot",

            MuzzleOffset = new Vector2(275, 100)
        };

        public static OtherPlayerAnim M1 = new OtherPlayerAnim()
        {
            Bone = 51,

            Debout_Arret_Vise_Haut = new AnimInfo(0, 12),
            Debout_Arret_Vise_Bas = new AnimInfo(12, 24),
            Debout_Arret_Rechargement = new AnimInfo(24, 72),
            Debout_Marche = new AnimInfo(72, 96),
            Debout_Marche_Vise_Haut = new AnimInfo(132, 156),
            Debout_Marche_vise = new AnimInfo(0, 0),
            Debout_Marche_Vise_Bas = new AnimInfo(0, 0),
            Debout_Marche_Rechargement = new AnimInfo(96, 132),
            Mise_Accroupi = new AnimInfo(192, 216),
            Accroupi_Arret_Vise_Haut = new AnimInfo(216, 228),
            Accroupi_Arret_Vise_Bas = new AnimInfo(228, 240),
            Accroupi_Arret_Rechargement = new AnimInfo(240, 288),
            Accroupi_Marche = new AnimInfo(288, 312),
            Accroupi_Marche_Vise_Haut = new AnimInfo(312, 336),
            Accroupi_Marche_vise = new AnimInfo(0, 0),
            Accroupi_Marche_Vise_Bas = new AnimInfo(336, 360),
            Accroupi_Marche_Rechargement = new AnimInfo(360, 396),
            Mise_debout = new AnimInfo(408, 432),
            Course = new AnimInfo(432, 444)
        };


        public static OtherPlayerAnim M1911 = new OtherPlayerAnim()
        {
            Bone = 51,

            Debout_Arret_Vise_Haut = new AnimInfo(0, 12),
            Debout_Arret_Vise_Bas = new AnimInfo(12, 24),
            Debout_Arret_Rechargement = new AnimInfo(24, 48),
            Debout_Marche = new AnimInfo(312, 336),
            Debout_Marche_Vise_Haut = new AnimInfo(48, 60),
            Debout_Marche_vise = new AnimInfo(60, 84),
            Debout_Marche_Vise_Bas = new AnimInfo(84, 96),
            Debout_Marche_Rechargement = new AnimInfo(96, 120),
            Mise_Accroupi = new AnimInfo(120, 144),
            Accroupi_Arret_Vise_Haut = new AnimInfo(144, 156),
            Accroupi_Arret_Vise_Bas = new AnimInfo(156, 168),
            Accroupi_Arret_Rechargement = new AnimInfo(168, 192),
            Accroupi_Marche = new AnimInfo(192, 216),
            Accroupi_Marche_Vise_Haut = new AnimInfo(216, 228),
            Accroupi_Marche_vise = new AnimInfo(228, 252),
            Accroupi_Marche_Vise_Bas = new AnimInfo(252, 264),
            Accroupi_Marche_Rechargement = new AnimInfo(264, 288),
            Mise_debout = new AnimInfo(288, 312),
            Course = new AnimInfo(336, 348)
        };
    }
}
