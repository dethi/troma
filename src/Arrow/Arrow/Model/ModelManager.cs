﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Arrow
{
    public class ModelManager
    {
        private Game game;
        public Dictionary<string, GameObject> Models { get; private set; }

        public ModelManager(Game game)
        {
            this.game = game;
            Models = new Dictionary<string, GameObject>();
        }

        public void Draw()
        {
            foreach (KeyValuePair<string, GameObject> item in Models)
            {
                item.Value.Draw();
            }
        }

        #region AddModel

        public void AddModel(string modelName)
        {
            AddModel(new GameObject(game, modelName));
        }

        public void AddModel(string modelName, Vector2 pos)
        {
            AddModel(new GameObject(game, modelName, pos));
        }

        public void AddModel(string modelName, Vector3 pos)
        {
            AddModel(new GameObject(game, modelName, pos));
        }

        public void AddModel(GameObject item)
        {
            Models.Add(item.modelName, item);
        }

        #endregion

        public void RemoveModel(string modelName)
        {
            Models.Remove(modelName);
        }
    }
}
