using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameEngine;

namespace Troma
{
    public class OtherPlayerObject
    {
        public static Entity BuildAndReturnEntity(Vector3 pos, Vector3 rot, string model)
        {
            Entity entity = new Entity(true);
            entity.AddComponent(new Transform(entity, pos, rot));
            entity.AddComponent(new AnimatedModel3D(entity, model));

            entity.AddComponent(new DrawAnimatedModel3D(entity));
            entity.AddComponent(new UpdateAnimation(entity));

            return entity;
        }
    }
}
