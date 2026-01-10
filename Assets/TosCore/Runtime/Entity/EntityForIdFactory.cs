
using VContainer;

namespace TosCore.Entity
{
    public class EntityForIdFactory<T, TEntity> : IEntityForIdFactory<T, TEntity>
        where TEntity : IEntity
        where T : IEntity, new()
    {
        [Inject] private readonly IIdGenerator<TEntity> _idGenerator;

        public T Create()
        {
            var entity = new T();
            entity.AssignId(_idGenerator.NewId());

            return entity;
        }
    }
}