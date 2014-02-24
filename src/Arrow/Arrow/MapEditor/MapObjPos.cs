using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Arrow
{
    public partial class MapObjPos
    {
        public Vector3 Change_x_z(Vector3 pos)
        {
            if (Keyboard.GetState().IsKeyDown(x_plus))
            {
                pos.X = (pos.X + 1);    
            }
            else if (Keyboard.GetState().IsKeyDown(x_moins))
            {
                pos.X = (pos.X - 1);
            }
            else if (Keyboard.GetState().IsKeyDown(z_plus))
            {
                pos.Z = (pos.Z + 1);
            }
            else if (Keyboard.GetState().IsKeyDown(z_moins))
            {
                pos.Z = (pos.Z - 1);
            }

            return pos;
        }
    }
}
