using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace GameEngine
{
    public static class FileManager
    {
        /// <summary>
        /// The Content Manager used to load all entity's content
        /// </summary>
        public static ContentManager Content { get; set; }

        /// <summary>
        /// Used to load assets
        /// </summary>
        public static T Load<T>(string assetName)
        {
            return Content.Load<T>(assetName);
        }
    }
}
