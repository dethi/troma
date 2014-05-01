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
            entity.AddComponent(new DrawModel3D(entity, effect));
            entity.AddComponent(new CollisionBox(entity));
            entity.AddComponent(new Target(entity));
        }
    }
}
