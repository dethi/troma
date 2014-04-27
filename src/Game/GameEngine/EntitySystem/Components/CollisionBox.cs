using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine
{
    public class CollisionBox : EntityComponent
    {
        public BoundingBox Box;

        public CollisionBox(Entity aParent)
            : base(aParent)
        {
            Name = "CollisionBox";
            _requiredComponents.Add("Model3D");
        }

        public override void Start()
        {
            base.Start();
            GenerateBoundingBox();
        }

        private void GenerateBoundingBox()
        {
            throw new NotImplementedException();
        }
    }
}
