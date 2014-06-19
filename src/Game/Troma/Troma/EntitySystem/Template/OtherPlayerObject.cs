using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public class OtherPlayerObject
    {
        public static Entity BuildAnimatedEntity(Vector3 pos, Vector3 rot, string model)
        {
            Entity entity = new Entity(true);
            entity.AddComponent(new Transform(entity, pos, rot));
            entity.AddComponent(new AnimatedModel3D(entity, model));

            entity.AddComponent(new DrawAnimatedModel3D(entity));
            entity.AddComponent(new UpdateAnimation(entity));

            return entity;
        }

        public static Entity BuildEntity(Vector3 pos, Vector3 rot, string model)
        {
            Entity entity = new Entity(true);
            entity.AddComponent(new Transform(entity, pos, rot));
            entity.AddComponent(new Model3D(entity, model));

            entity.AddComponent(new DrawModel3D(entity, 
                FileManager.Load<Effect>("Effects/GameObject")));

            return entity;
        }
    }
}
