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
        private bool previous_object_pressed;
        private bool next_object_pressed;

        public MapObjPos()
        {
            x_plus_pressed = false;
            x_moins_pressed = false;
            z_plus_pressed = false;
            z_moins_pressed = false;
            previous_object_pressed = false;
            next_object_pressed = false;
        }

        public Vector3 Change_x_z(Vector3 pos)
        {
            if (Keyboard.GetState().IsKeyDown(x_plus) || x_plus_pressed )
            {
                if (!x_plus_pressed)
                {
                    pos.X++; 
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
                    pos.X--;
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
                    pos.Z++;
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
                    pos.Z--;
                    z_moins_pressed = true;
                }
                else if (Keyboard.GetState().IsKeyUp(z_moins))
                {
                    z_moins_pressed = false;
                }
            }
            return pos;
        }

        public void Change_i(ref int ieme, int max)
        {
            if (Keyboard.GetState().IsKeyDown(previous_object) || previous_object_pressed)
            {
                if (!previous_object_pressed)
                {
                    if (ieme > 1)
                    {
                        ieme--;
                        previous_object_pressed = true;
                    }
                    else
                    {
                        ieme = max;
                    }
                }
                else if (Keyboard.GetState().IsKeyUp(previous_object))
                {
                    previous_object_pressed = false;
                }
            }
            else if (Keyboard.GetState().IsKeyDown(next_object) || next_object_pressed)
            {
                if (!next_object_pressed)
                {
                    if (ieme < max)
                    {
                        ieme++;
                        next_object_pressed = true;
                    }
                    else
                    {
                        ieme = 0;
                    }
                }
                else if (Keyboard.GetState().IsKeyUp(next_object))
                {
                    next_object_pressed = false;
                }
            }

        }
    }
}
