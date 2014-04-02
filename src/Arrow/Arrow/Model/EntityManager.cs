using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arrow
{
    public class EntityManager
    {
        private Game game;
        private MapManager maps;

        public List<Entity> Entities { get; private set; }

        public EntityManager(Game game, MapManager maps)
        {
            this.game = game;
            this.maps = maps;
            Entities = new List<Entity>();
        }

        public void Draw()
        {
            foreach (Entity item in Entities)
                item.Draw();
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

        public void AddEntity(Model model, Vector2 pos)
        {
            AddEntity(new Entity(game, model, new Vector3(
                pos.X,
                maps.GetHeight(pos.X, pos.Y).Value,
                pos.Y)));
        }

        public void AddEntity(Model model, Vector3 pos)
        {
            AddEntity(new Entity(game, model, pos));
        }

        public void AddEntity(Entity item)
        {
            Entities.Add(item);
        }

        #endregion
    }
}
