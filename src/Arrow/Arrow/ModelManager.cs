using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Arrow
{
    public class ModelManager
    {
        public List<FBX> Models { get; private set; }

        public ModelManager()
        {
            Models = new List<FBX>();
        }

        public void Update(GameTime gameTime)
        {
            foreach (FBX item in Models)
            {
                item.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (FBX item in Models)
            {
                item.Draw(gameTime);
            }
        }

        public void AddModel(FBX item)
        {
            Models.Add(item);
        }

        public void RemoveModel(int index)
        {
            Models.RemoveAt(index);
        }
    }
}
