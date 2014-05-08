using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public class GameObject
    {
        public static void BuildEntity(Vector3 pos, string model, Effect effect)
        {
            Entity entity = new Entity();
            entity.AddComponent(new Transform(entity, pos));
            entity.AddComponent(new Model3D(entity, model));

            Effect _effect = effect.Clone();
            _effect.Name = effect.Name;

            entity.AddComponent(new DrawModel3D(entity, _effect));
            entity.AddComponent(new CollisionBox(entity));
        }
    }
}
