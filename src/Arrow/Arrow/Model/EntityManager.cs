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
        private MapManager maps;

        public Dictionary<string, Entity> Entities { get; private set; }

        public EntityManager(Game game, MapManager maps)
        {
            this.game = game;
            this.maps = maps;
            Entities = new Dictionary<string, Entity>();
        }

        public void Draw()
        {
            foreach (KeyValuePair<string, Entity> item in Entities)
                item.Value.Draw();
        }

        #region AddEntity

        public void AddEntity(string entityName, Vector2 pos)
        {
            AddEntity(new Entity(game, entityName, new Vector3(
                pos.X, 
                maps.GetHeight(pos.X, pos.Y).Value, 
                pos.Y)));
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
    }
}
