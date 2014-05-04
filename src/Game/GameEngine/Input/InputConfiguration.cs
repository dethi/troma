using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace GameEngine
{
    public class InputConfiguration
    {
        #region Constants

        private static readonly Dictionary<KeyActions, Keys> QWERTY = new Dictionary<KeyActions, Keys>()
        {
            { KeyActions.Up,        Keys.W },
            { KeyActions.Bottom,    Keys.S },
            { KeyActions.Left,      Keys.A },
            { KeyActions.Right,     Keys.D },
            { KeyActions.Crouch,    Keys.C },
            { KeyActions.Run,       Keys.LeftShift },
            { KeyActions.Jump,      Keys.Space },
            { KeyActions.Reload,    Keys.R },
        };

        private static readonly Dictionary<KeyActions, Keys> AZERTY = new Dictionary<KeyActions, Keys>()
        {
            { KeyActions.Up,        Keys.Z },
            { KeyActions.Bottom,    Keys.S },
            { KeyActions.Left,      Keys.Q },
            { KeyActions.Right,     Keys.D },
            { KeyActions.Crouch,    Keys.C },
            { KeyActions.Run,       Keys.LeftShift },
            { KeyActions.Jump,      Keys.Space },
            { KeyActions.Reload,    Keys.R },
        };

        #endregion

        private static Dictionary<KeyActions, Keys> mapping;

        public InputConfiguration()
        {
            mapping = new Dictionary<KeyActions, Keys>()
            {
                { KeyActions.Up,        Keys.W },
                { KeyActions.Bottom,    Keys.S },
                { KeyActions.Left,      Keys.A },
                { KeyActions.Right,     Keys.D },
                { KeyActions.Crouch,    Keys.C },
                { KeyActions.Run,       Keys.LeftShift },
                { KeyActions.Jump,      Keys.Space },
                { KeyActions.Reload,    Keys.R },
            };
        }

        /// <summary>
        /// Return the key for this action
        /// </summary>
        public Keys Parse(KeyActions keyAction)
        {
            Keys key;

            if (mapping.TryGetValue(keyAction, out key))
                return key;
            else
                throw new KeyNotFoundException();
        }

        public static void ChangeKeyboard(string keyboard)
        {
            if (keyboard == "QWERTY")
                mapping = QWERTY;
            else
                mapping = AZERTY;
        }
    }
}
