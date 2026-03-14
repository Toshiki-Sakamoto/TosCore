using System;
using System.Collections.Concurrent;

namespace TosCore.Entity
{
    /// <summary>
    /// エンティティリポジトリの共通インメモリ基底実装
    /// </summary>
    public abstract class EntityRepositoryBase<TEntity> : IEntityRepository<TEntity>
        where TEntity : IEntity
    {
        private readonly ConcurrentDictionary<IEntityId, TEntity> _entityMap = new();

        public bool TryGetValue(IEntityId id, out TEntity entity)
        {
            return _entityMap.TryGetValue(id, out entity);
        }

        public void Add(TEntity entity)
        {
            if (!_entityMap.TryAdd(entity.Id, entity))
            {
                throw new InvalidOperationException($"Entity already exists. id={entity.Id}");
            }
        }

        public void Remove(TEntity entity)
        {
            _entityMap.TryRemove(entity.Id, out _);
        }

        protected bool TryRemoveById(IEntityId id)
        {
            return _entityMap.TryRemove(id, out _);
        }
    }
}
