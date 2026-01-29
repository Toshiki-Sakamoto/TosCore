
using VContainer;

namespace TosCore.Entity
{
    public class EntityForIdFactory<TEntity, TEntityAbstract> : IEntityForIdFactory<TEntity>
        where TEntity : TEntityAbstract, new()
        where TEntityAbstract : IEntity
    {
        [Inject] private readonly IIdGenerator<TEntityAbstract> _idGenerator;

        public TEntity Create()
        {
            var entity = new TEntity();
            entity.AssignId(_idGenerator.NewId());

            return entity;
        }
    }
    
    public class EntityForIdFactory<TEntity> : EntityForIdFactory<TEntity, TEntity>
        where TEntity : IEntity, new()
    {
    }
}