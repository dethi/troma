using System;
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

        public void Draw(GameTime gameTime)
        {
            foreach (KeyValuePair<string, GameObject> item in Models)
            {
                item.Value.Draw(gameTime);
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
       
        public void MoveModel(Vector4 i_pos) // methode qui va modifier la position du ieme fbx du model manager
        {
            GameObject item = this.Models.ElementAt((int)i_pos.W).Value;
            item.position = Matrix.CreateTranslation(new Vector3(i_pos.X, game.map.GetHeight(i_pos.X, i_pos.Z), i_pos.Z));
            //Console.WriteLine(this.Models.ElementAt((int)i_pos.W).Key + " position : " + i_pos.X + " x ," + i_pos.Y + " y ," + i_pos.Z + " z ,");
        }
    }
}
