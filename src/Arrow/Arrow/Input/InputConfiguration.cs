using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Arrow
{
    public class InputConfiguration
    {
        #if BUILD

        // AZERTY KEYBOARD
        public readonly Keys KB_UP = Keys.Z;
        public readonly Keys KB_BOTTOM = Keys.S;
        public readonly Keys KB_LEFT = Keys.Q;
        public readonly Keys KB_RIGHT = Keys.D;
        
        #else

        // QWERTY KEYBOARD
        public readonly Keys KB_UP = Keys.W;
        public readonly Keys KB_BOTTOM = Keys.S;
        public readonly Keys KB_LEFT = Keys.A;
        public readonly Keys KB_RIGHT = Keys.D;

        #endif

        public readonly Keys KB_CROUCH = Keys.LeftControl;
        public readonly Keys KB_RUN = Keys.LeftShift;
        public readonly Keys KB_JUMP = Keys.Space;
        public readonly Keys KB_RELOAD = Keys.R;
    }
}
