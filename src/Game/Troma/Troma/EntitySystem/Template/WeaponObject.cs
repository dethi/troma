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
        public static Entity BuildEntity(WeaponInfo weaponInfo)
        {
            Entity entity = new Entity(true);
            Transform transform = new Transform(entity);

            Entity arms = new Entity(true);
            arms.AddComponent(transform);
            arms.AddComponent(new AnimatedModel3D(arms, "arms_" + weaponInfo.Model));
            arms.AddComponent(new DrawAnimatedModel3D(arms));
            arms.AddComponent(new UpdateAnimation(arms));


            entity.AddComponent(transform);
            entity.AddComponent(new Weapon(entity, arms, weaponInfo));
            entity.AddComponent(new AnimatedModel3D(entity, weaponInfo.Model));

            entity.AddComponent(new DrawAnimatedModel3D(entity));
            entity.AddComponent(new UpdateAnimation(entity));

            entity.AddComponent(new DrawWeapon(entity));

            return entity;
        }
    }
}
