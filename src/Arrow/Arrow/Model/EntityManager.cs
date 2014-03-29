using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Arrow
{
    public class EntityManager
    {
        private Game game;
        public Dictionary<string, Entity> Entities { get; private set; }

        public EntityManager(Game game)
        {
            this.game = game;
            Entities = new Dictionary<string, Entity>();
        }

        public void Draw()
        {
            foreach (KeyValuePair<string, Entity> item in Entities)
                item.Value.Draw();
        }

        #region AddEntity

        public void AddEntity(string entityName)
        {
            AddEntity(new Entity(game, entityName));
        }

        public void AddEntity(string entityName, Vector2 pos)
        {
            AddEntity(new Entity(game, entityName, pos));
        }

        public void AddEntity(string entityName, Vector3 pos)
        {
            AddEntity(new Entity(game, entityName, pos));
        }

        public void AddEntity(Entity item)
        {
            Entities.Add(item.entityName, item);
        }

        #endregion

        public void RemoveEntity(string entityName)
        {
            Entities.Remove(entityName);
        }
       
        public void MoveEntity(Vector4 i_pos)
        {
            Entity item = this.Entities.ElementAt((int)i_pos.W).Value;
            float? pos_Y = 0; //game.mapManager.GetHeight(i_pos.X, i_pos.Z);

            pos_Y = (pos_Y.HasValue) ? pos_Y.Value : 0;

            item.position = Matrix.CreateTranslation(
                new Vector3(i_pos.X, pos_Y.Value, i_pos.Z));
        }
    }
}
