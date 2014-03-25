using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Arrow
{
    public partial class EntityPos
    {
        private InputState input;

        public EntityPos(InputState input)
        {
            this.input = input;
        }

        public Vector3 Change_x_z(Vector3 pos)
        {
            if (input.IsPressed(KB_X_MORE) ||
                (input.IsDown(KB_X_MORE) && input.IsDown(KB_SPEED)))
                pos.X++;
            else if (input.IsPressed(KB_X_LESS) ||
                (input.IsDown(KB_X_LESS) && input.IsDown(KB_SPEED)))
                pos.X--;

            if (input.IsPressed(KB_Z_MORE) ||
                (input.IsDown(KB_Z_MORE) && input.IsDown(KB_SPEED)))
                pos.Z++;
            else if (input.IsPressed(KB_Z_LESS) ||
                (input.IsDown(KB_Z_LESS) && input.IsDown(KB_SPEED)))
                pos.Z--;

            return pos;
        }

        public void Change_i(ref int i, int max)
        {
            if (input.IsPressed(KB_PREVIOUS_OBJ))
            {
                if (i > 1)
                    i--;
                else
                    i = max;
            }
            else if (input.IsPressed(KB_NEXT_OBJ))
            {
                if (i < max)
                    i++;
                else
                    i = 0;
            }
        }
    }
}
