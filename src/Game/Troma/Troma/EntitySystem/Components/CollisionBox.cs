using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;
using ClientServerExtension;

namespace Troma
{
    public class CollisionBox : EntityComponent, ICollisionBox
    {
        public List<BoundingBox> BoxList { get; private set; }

        public CollisionBox(Entity aParent)
            : base(aParent)
        {
            Name = "CollisionBox";
            _requiredComponents.Add("Model3D");

            BoxList = new List<BoundingBox>();
        }

        public void GenerateBoundingBox()
        {
            Box box = new Box();
            box.Generate(Entity.GetComponent<Model3D>().Model,
                Entity.GetComponent<Transform>().World);
            BoxList.AddRange(box.BoudingBox);
        }
    }
}
