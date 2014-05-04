using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEngine;

namespace Troma
{
    public class Target : EntityComponent
    {
        public Target(Entity aParent)
            : base(aParent)
        {
            Name = "Target";
            _requiredComponents.Add("CollisionBox");
        }
    }
}
