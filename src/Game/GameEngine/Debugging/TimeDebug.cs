using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine
{
#if DEBUG
    public static class TimeDebug
    {
        public static string Debug(GameTime gameTime)
        {
            return String.Format("Time: {0:F3} sec.", gameTime.TotalGameTime.TotalSeconds);
        }
    }
#endif
}
