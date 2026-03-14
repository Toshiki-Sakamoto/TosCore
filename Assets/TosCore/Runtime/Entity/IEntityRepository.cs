namespace TosCore.Entity
{
    /// <summary>
    /// エンティティ参照・登録の共通リポジトリ契約
    /// </summary>
    public interface IEntityRepository<TEntity>
        where TEntity : IEntity
    {
        bool TryGetValue(IEntityId id, out TEntity entity);

        void Add(TEntity entity);
        
        void Remove(TEntity entity);
    }
}
