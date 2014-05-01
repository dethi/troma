using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameEngine;

namespace GameEngine
{
#if DEBUG
    public static class SysDebug
    {
        public static string Debug(GameTime gameTime)
        {
            return String.Format("FPS: {0:F1}\nMem: {1:F2} Mo",
                1000.0d / gameTime.ElapsedGameTime.TotalMilliseconds,
                GC.GetTotalMemory(false) / 1048576f);
        }
    }
#endif
}
