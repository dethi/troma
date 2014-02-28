using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Arrow
{
    public partial class Player
    {
        const float HEIGHT = 7;
        const float CROUCH_HEIGHT = 4.8f;

        const float WALK_SPEED = 40f;
        const float COEF_RUN_SPEED = 1.7f;
        
        const Keys KB_UP = Keys.W;
        const Keys KB_BOTTOM = Keys.S;
        const Keys KB_LEFT = Keys.A;
        const Keys KB_RIGHT = Keys.D;
        
        /*
        // AZERTY KEYBOARD
        const Keys KB_UP = Keys.Z;
        const Keys KB_BOTTOM = Keys.S;
        const Keys KB_LEFT = Keys.Q;
        const Keys KB_RIGHT = Keys.D;
        */

        const Keys KB_CROUCH = Keys.LeftControl;
        const Keys KB_RUN = Keys.LeftShift;
        const Keys KB_JUMP = Keys.Space;
        const Keys KB_RELOAD = Keys.R;
    }
}
