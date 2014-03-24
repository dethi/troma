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
        public Dictionary<string, Entity> Models { get; private set; }

        public ModelManager(Game game)
        {
            this.game = game;
            Models = new Dictionary<string, Entity>();
        }

        public void Draw()
        {
            foreach (KeyValuePair<string, Entity> item in Models)
            {
                item.Value.Draw();
            }
        }

        #region AddModel

        public void AddModel(string modelName)
        {
            AddModel(new Entity(game, modelName));
        }

        public void AddModel(string modelName, Vector2 pos)
        {
            AddModel(new Entity(game, modelName, pos));
        }

        public void AddModel(string modelName, Vector3 pos)
        {
            AddModel(new Entity(game, modelName, pos));
        }

        public void AddModel(Entity item)
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
            Entity item = this.Models.ElementAt((int)i_pos.W).Value;
            item.position = Matrix.CreateTranslation(new Vector3(i_pos.X, game.map.GetHeight(i_pos.X, i_pos.Z), i_pos.Z));
            //Console.WriteLine(this.Models.ElementAt((int)i_pos.W).Key + " position : " + i_pos.X + " x ," + i_pos.Y + " y ," + i_pos.Z + " z ,");
        }
    }
}
