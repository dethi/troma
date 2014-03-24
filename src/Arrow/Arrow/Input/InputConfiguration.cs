﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Arrow
{
    public class InputConfiguration
    {
        private Dictionary<KeyActions, Keys> mapping;

        public InputConfiguration()
        {
            mapping = new Dictionary<KeyActions, Keys>()
            {
                { KeyActions.Up,        Keys.W },
                { KeyActions.Bottom,    Keys.S },
                { KeyActions.Left,      Keys.A },
                { KeyActions.Right,     Keys.D },
                { KeyActions.Crouch,    Keys.LeftControl },
                { KeyActions.Run,       Keys.LeftShift },
                { KeyActions.Jump,      Keys.Space },
                { KeyActions.Reload,    Keys.R },
            };
        }

        public Keys Parse(KeyActions keyAction)
        {
            Keys key;

            if (mapping.TryGetValue(keyAction, out key))
                return key;
            else
                throw new KeyNotFoundException();
        }
    }
}
