using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public class TargetObject
    {
        public static void BuildEntity(Vector3 pos, float rotY, Effect effect)
        {
            Entity entity = new Entity();

            entity.AddComponent(new Transform(entity, pos, 
                new Vector3(0, MathHelper.ToRadians(rotY), 0)));

            entity.AddComponent(new Model3D(entity, "cible"));
            entity.AddComponent(new DrawModel3D(entity, effect.Clone()));
            entity.AddComponent(new CollisionBox(entity));
            entity.AddComponent(new Target(entity));

            /*
            Entity entity = new Entity();

            entity.AddComponent(new Transform(entity, pos,
                new Vector3(1.57f, 3.14f + MathHelper.ToRadians(rotY), 0)));

            entity.AddComponent(new AnimatedModel3D(entity, "cible_animated"));
            entity.AddComponent(new DrawAnimatedModel3D(entity));
            entity.AddComponent(new UpdateAnimation(entity));

            entity.AddComponent(new Model3D(entity, "cible"));
            entity.AddComponent(new CollisionBox(entity));
            entity.AddComponent(new Target(entity));
             * */
        }
    }
}
