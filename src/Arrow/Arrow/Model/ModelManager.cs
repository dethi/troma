using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Arrow
{
    public class ModelManager
    {
        public List<GameObject> Models { get; private set; }

        public ModelManager()
        {
            Models = new List<GameObject>();
        }

        public void Draw(GameTime gameTime)
        {
            foreach (GameObject item in Models)
            {
                item.Draw(gameTime);
            }
        }

        public void AddModel(GameObject item)
        {
            Models.Add(item);
        }

        public void RemoveModel(int index)
        {
            Models.RemoveAt(index);
        }
    }
}
