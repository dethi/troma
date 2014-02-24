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
        private bool x_plus_pressed;
        private bool x_moins_pressed;
        private bool z_plus_pressed;
        private bool z_moins_pressed;

        public MapObjPos()
        {
            x_plus_pressed = false;
            x_moins_pressed = false;
            z_plus_pressed = false;
            z_moins_pressed= false;
        }

        public Vector3 Change_x_z(Vector3 pos)
        {
            if (Keyboard.GetState().IsKeyDown(x_plus) || x_plus_pressed )
            {
                if (!x_plus_pressed)
                {
                    pos.X = (pos.X + 1); 
                    x_plus_pressed = true;
                }
                else if (Keyboard.GetState().IsKeyUp(x_plus))
                {
                    x_plus_pressed = false;
                }
            }
            else if (Keyboard.GetState().IsKeyDown(x_moins) || x_moins_pressed)
            {
                if (!x_moins_pressed)
                {
                    pos.X = (pos.X - 1);
                    x_moins_pressed = true;
                }
                else if (Keyboard.GetState().IsKeyUp(x_moins))
                {
                    x_moins_pressed = false;
                }
            }
            else if (Keyboard.GetState().IsKeyDown(z_plus) || z_plus_pressed)
            {
                if (!z_plus_pressed)
                {
                    pos.Z = (pos.Z + 1);
                    z_plus_pressed = true;
                }
                else if (Keyboard.GetState().IsKeyUp(z_plus))
                {
                    z_plus_pressed = false;
                }
            }
            else if (Keyboard.GetState().IsKeyDown(z_moins) || z_moins_pressed)
            {
                if (!z_moins_pressed)
                {
                    pos.Z = (pos.Z - 1);
                    z_moins_pressed = true;
                }
                else if (Keyboard.GetState().IsKeyUp(z_moins))
                {
                    z_moins_pressed = false;
                }
            }

            return pos;
        }
    }
}
