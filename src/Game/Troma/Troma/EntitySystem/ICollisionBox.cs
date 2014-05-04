using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Troma
{
    public interface ICollisionBox
    {
        List<BoundingBox> BoxList { get; }
    }
}