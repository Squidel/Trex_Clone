using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trex_Clone.Entities
{
    public class EntityManager
    {
        private List<IGameEntity> _entitiesToAdd = new List<IGameEntity>();
        private List<IGameEntity> _entitiesToRemove = new List<IGameEntity>();
        private readonly List<IGameEntity> _entities = new List<IGameEntity>();

        public IEnumerable<IGameEntity> Entities { get {  return new ReadOnlyCollection<IGameEntity>(_entities); } }
        public void Update(GameTime gameTime)
        {
            foreach (var entity in _entities)
            {
                entity.Update(gameTime);
            }

            //this approach is to prevent manipulating the original list while iterating through the list
            foreach (var entity in _entitiesToAdd)
            {
                _entities.Add(entity);
            }
            foreach (var entity in _entitiesToRemove)
            {
                _entities.Remove(entity);
            }
            _entitiesToAdd.Clear();
            _entitiesToRemove.Clear();
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var orderdEntities = _entities.OrderBy(x => x.DrawOrder);
            foreach(var entity in orderdEntities)
            {
                entity.Draw(spriteBatch, gameTime);
            }
        }
        public bool AddEntity(IGameEntity entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Cannot add null as an entity to be added");
            }
            _entitiesToAdd.Add(entity);
            return true;
        }
        public bool RemoveEntity(IGameEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Cannot add null as an entity to be added");
            }
            _entitiesToRemove.Add(entity);
            return true;
        }
        public void Clear()
        {
            _entitiesToRemove.AddRange(_entities);
        }
    }
}
