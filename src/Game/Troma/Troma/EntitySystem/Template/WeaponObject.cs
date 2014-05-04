using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public class WeaponObject
    {
        public static Entity BuildEntity(WeaponInfo weaponInfo, Effect effect)
        {
            Entity entity = new Entity(true);
            entity.AddComponent(new Transform(entity));
            entity.AddComponent(new Weapon(entity, weaponInfo));
            entity.AddComponent(new Model3D(entity, weaponInfo.Model));
            entity.AddComponent(new DrawWeapon(entity, effect.Clone()));

            return entity;
        }
    }
}
