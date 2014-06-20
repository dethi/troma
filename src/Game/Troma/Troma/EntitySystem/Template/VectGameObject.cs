using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public class VectGameObject
    {
        public static void BuildEntity(Vector3[] pos, string model, Effect effect)
        {
            Entity entity = new Entity();
            entity.AddComponent(new Transform(entity, new Vector3(0, -50, 0))); // Little hack
            entity.AddComponent(new VectTransform(entity, pos));
            entity.AddComponent(new Model3D(entity, model));

            Effect _effect = effect.Clone();
            _effect.Name = effect.Name;

            entity.AddComponent(new VectDrawModel3D(entity, _effect));
            entity.AddComponent(new VectCollisionBox(entity));
        }
    }
}
